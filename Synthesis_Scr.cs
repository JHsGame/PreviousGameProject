using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;



public class Synthesis_Scr : MonoBehaviour
{
    private static Synthesis_Scr Instance;
    private SynthesisButton_Scr buttonScr;
    private SynthesisCaching_Scr cachingScr;

    private List<ItemSlot_Scr> itemSlots = new List<ItemSlot_Scr>();   // 아이템 슬롯 리스트
    private List<HeroSlot_Scr> heroSlots = new List<HeroSlot_Scr>();   // 히어로 슬롯 리스트 -> 만들어줘야함.
    private List<SynthesisSlot_Scr> synthesisSlots = new List<SynthesisSlot_Scr>();

    //[SerializeField]
    //OpenBox openBox_Scr;
    ItemSlot_Scr tmpItem;

    private bool b_isNewHero = false;
    [SerializeField]
    private bool b_isFull = false;

    [SerializeField]
    private int i_synthesisLevel;
    private const int i_synthesisAmount = 10;
    private int i_randSynthesisNum;
    [SerializeField]
    private int maxPoint = 0;

   //public OpenBox OpenBoxScr { get => openBox_Scr; }
    public static Synthesis_Scr instance { get => Instance; }
    public int SynthesisLevel { get => i_synthesisLevel; }
    public bool isFull { get => b_isFull; }

    public SynthesisEffectChasing g_SynthesisEffectChasing;
    public ParticleSystem par_Particle;
    public ParticleSystem[] par_JackpotParticle;
    public SynthesisSlot_Scr g_SynthesisSlot_Scr;
    public GameObject g_SynthesisSlot_parent;
    public bool itemListTapOpen;


    private void Awake()
    {
        if(Instance != null)
        {
            return;
        }
        else
        {
            Instance = this;
        }
    }

    public void readytoDelay()
    {
        Invoke("StartingCoroutine", 1f);
    }

    public void StartingCoroutine()
    {
        buttonScr = SynthesisButton_Scr.instance;
        cachingScr = SynthesisCaching_Scr.instance;

        buttonScr.SetScript(this, cachingScr);
        cachingScr.SetScript(this, buttonScr);
        StartCoroutine(ListChecker());


        Transform parent = InventoryCaching_Scr.instance.heroSlotParent;

        for (int i = 0; i < parent.childCount; ++i)
        {
            heroSlots.Add(parent.GetChild(i).GetComponent<HeroSlot_Scr>());
            heroSlots[i].isEmpty = true;
        }

        parent = SynthesisCaching_Scr.instance.synthesisParent;

        for (int i = 0; i < parent.childCount; ++i)
        {
            synthesisSlots.Add(parent.GetChild(i).GetComponent<SynthesisSlot_Scr>());
            LoadHeroSlot(Inventory_Scr.instance.loadHeroList, Inventory_Scr.instance.heroListObjs);
        }
    }

    public void readytoSynthesisEffect()
    {

        for (int i = 0; i < synthesisSlots.Count; ++i)
        {
            synthesisSlots[i].gameObject.SetActive(true);
        }
    }

    public void activeFusionEffect()
    {
        StartCoroutine(FusionEffect());
    }

    IEnumerator FusionEffect()
    {
        WaitForSeconds delay = new WaitForSeconds(0.35f);

        int dummy = 0;
        for (int i = 0; i < synthesisSlots.Count; ++i)
        {
            if (synthesisSlots[i].isEmpty)
            {
                synthesisSlots[i].delayeffect();
            }
            else
            {
                synthesisSlots[i].invokeEffect();
                SoundManager_sfx.instance.PlaySE("synthesisEffect1", false);

                dummy = i + 1;
                if(dummy < synthesisSlots.Count)
                {
                    if (synthesisSlots[i + 1].isEmpty)
                    {
                        for (int j = 0; j < synthesisSlots.Count; j++)
                        {
                            synthesisSlots[j].delayeffect();
                        }
                    }
                }
                yield return delay;
            }
        }

        SynthesisCaching_Scr.instance.SynthesisEffect.SetActive(true);
        
        ItemSynthesis();
    }

    IEnumerator ListChecker()
    {
        while (true)
        {
            int nowInputSlot = 0;

            itemSlots = Inventory_Scr.instance.myItems;

            for (int i = 0; i < synthesisSlots.Count; ++i)
            {
                if(!synthesisSlots[i].isEmpty)
                {
                    nowInputSlot++;
                }
            }

            for(int i = 0; i < heroSlots.Count; ++i)
            {
                if (heroSlots[i].isEmpty)
                {
                    heroSlots[i].transform.SetSiblingIndex(heroSlots.Count - 1);
                    heroSlots[i].transform.gameObject.SetActive(false);
                }
                else
                {
                    heroSlots[i].transform.gameObject.SetActive(true);
                }
            }
            
            for(int i = 0; i < synthesisSlots.Count; ++i)
            {
                if (synthesisSlots[i].isEmpty)
                {
                    synthesisSlots[i].transform.SetSiblingIndex(synthesisSlots.Count - 1);
                }
            }

            if(nowInputSlot >= 2)
            {
                SynthesisCaching_Scr.instance.synthesisButton.enabled = true;
            }
            else
            {
                SynthesisCaching_Scr.instance.synthesisButton.enabled = false;
            }

            if(nowInputSlot >= i_synthesisAmount)
            {
                b_isFull = true;
            }
            else
            {
                b_isFull = false;
            }

            yield return CoroutineManager_Scr.WaitForEndOfFrame;
        }
    }

    public void LoadHeroSlot(List<int> lists, List<GameObject> objs)
    {
        for (int i = 0; i < lists.Count; ++i)
        {
            if (heroSlots[i].isEmpty)
            {
                heroSlots[i].LoadInfo(lists[i], objs[i]);
            }
        }
    }

    public void SetHeroSlot(int num, GameObject obj)
    {
        for (int i = 0; i < heroSlots.Count; ++i)
        {
            if(heroSlots[i].isEmpty)
            {
                heroSlots[i].LoadInfo(num, obj);
                break;
            }
        }
    }

    public void ClearList(bool _isSynthesis)
    {
        for (int i = 0; i < itemSlots.Count; ++i)
        {
            itemSlots[i].SelectedOff();
        }

        for (int i = 0; i < synthesisSlots.Count; ++i)
        {
            if (!synthesisSlots[i].isEmpty)
            {
                RejectCombination(synthesisSlots[i], _isSynthesis);
            }
            // 여기서 위로 올렸던 합성 목록들을 다시 돌려주자.
        }
    }

    public void SetCombinationSlot(int _level, ItemSlot_Scr _slot)
    {
        for (int i = 0; i < synthesisSlots.Count; ++i)
        {
            if (synthesisSlots[i].isEmpty)
            {
                synthesisSlots[i].SetInfo(false, _slot, null);

                int i_synthesisPoint = 0;

                if (_level == 6)
                {
                    string tmp = Bag_Hero_ItmeCSV.instance.SynthesisData[_level - 1]["tier_range"].ToString();
                    i_synthesisPoint += int.Parse(tmp);
                    synthesisSlots[i].SynthesisPoint = i_synthesisPoint;
                }
                else
                {
                    string tmp = Bag_Hero_ItmeCSV.instance.SynthesisData[_level - 1]["tier_range"].ToString();
                    string[] split = tmp.Split('~');
                    int a = System.Convert.ToInt32(split[0]);
                    int b = System.Convert.ToInt32(split[1]);
                    int randTmp = Random.Range(a, b + 1);
                    i_synthesisPoint += randTmp;
                    synthesisSlots[i].SynthesisPoint = i_synthesisPoint;
                }

                break;
            }
        }

        _slot.ResetSlot();
    }

    public void SetCombinationSlot(int _level, HeroSlot_Scr _slot)
    {
        for (int i = 0; i < synthesisSlots.Count; ++i)
        {
            if (synthesisSlots[i].isEmpty)
            {
                synthesisSlots[i].SetInfo(true, null, _slot);

                int i_synthesisPoint = 0;

                if (_level == 6)
                {
                    string tmp = Bag_Hero_ItmeCSV.instance.SynthesisData[_level - 1]["tier_range"].ToString();
                    i_synthesisPoint += int.Parse(tmp);
                    synthesisSlots[i].SynthesisPoint = i_synthesisPoint;
                }
                else
                {
                    string tmp = Bag_Hero_ItmeCSV.instance.SynthesisData[_level - 1]["tier_range"].ToString();
                    string[] split = tmp.Split('~');
                    int a = System.Convert.ToInt32(split[0]);
                    int b = System.Convert.ToInt32(split[1]);
                    int randTmp = Random.Range(a, b + 1);
                    i_synthesisPoint += randTmp;
                    synthesisSlots[i].SynthesisPoint = i_synthesisPoint;
                }

                break;
            }
        }

        _slot.ResetSlot();
    }

    // 조합모드에서 아이템을 선택 취소하는 함수.
    public void RejectCombination(SynthesisSlot_Scr _slot, bool _isSynthesis)
    {
        if (!_isSynthesis)
        {
            if (_slot.isHero)
            {
                for (int i = 0; i < heroSlots.Count; ++i)
                {
                    if (heroSlots[i].isEmpty)
                    {
                        heroSlots[i].LoadInfo(_slot.CsvNum, _slot.hero.heroSlot);
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < itemSlots.Count; ++i)
                {
                    if (itemSlots[i].item == null)
                    {
                        itemSlots[i].LoadInfo(_slot.CsvNum);
                        break;
                    }
                }
            }
        }

        _slot.ClearSlot();
    }

    // 아이템 조합
    public void ItemSynthesis()
    {
        bool b_isHero = false;
        if (InventoryCaching_Scr.instance.heroList.gameObject.activeSelf)
        {
            b_isHero = true;
        }

        par_Particle.gameObject.SetActive(true);
        DeliveryBoxManager_Scr.instance.g_PartipleCanvas.SetActive(false);
        par_Particle.Play();
        SoundManager_sfx.instance.PlaySE("synthesisEffect2", false);
        maxPoint = 0;

        for (int i = 0; i < synthesisSlots.Count; ++i)
        {
            if (!synthesisSlots[i].isEmpty)
            {
                if(synthesisSlots[i].hero != null)
                {
                    ManPowerManager_Scr.instance.DeleteHero(synthesisSlots[i].hero.heroSlot);
                }
                maxPoint += synthesisSlots[i].SynthesisPoint;
                print(maxPoint);
            }
        }


        ClearList(true);

        for (int i = 5; i >= 0; --i)
        {
            int Point = System.Convert.ToInt32(Bag_Hero_ItmeCSV.instance.SynthesisData[i]["synthesis_point"]);

            if (Point <= maxPoint)
            {
                i_synthesisLevel = i + 1;
                break;
            }
        }

        print(maxPoint + ", " + i_synthesisLevel);

        b_isNewHero = false;

        if (b_isHero)
        {
            print("여기1");
            b_isNewHero = true;
            switch (i_synthesisLevel)
            {
                case 1:
                    i_randSynthesisNum = Random.Range(0, 2);
                    break;
                case 2:
                    i_randSynthesisNum = Random.Range(2, 6);
                    break;
                case 3:
                    i_randSynthesisNum = Random.Range(6, 12);
                    break;
                case 4:
                    i_randSynthesisNum = Random.Range(12, 18);
                    break;
                case 5:
                    i_randSynthesisNum = Random.Range(18, 21);
                    break;
                case 6:
                    i_randSynthesisNum = 21;
                    break;
                default:
                    break;
            }
        }
        else
        {
            b_isNewHero = false;
            switch (i_synthesisLevel)
            {
                case 1:
                    i_randSynthesisNum = Random.Range(0, 6);
                    break;
                case 2:
                    i_randSynthesisNum = Random.Range(6, 21);
                    break;
                case 3:
                    i_randSynthesisNum = Random.Range(21, 40);
                    break;
                case 4:
                    i_randSynthesisNum = Random.Range(40, 46);
                    break;
                case 5:
                    i_randSynthesisNum = Random.Range(46, 50);
                    break;
                case 6:
                    i_randSynthesisNum = 50;
                    break;
                default:
                    break;
            }
        }

        tmpItem = null;

        for (int i = 0; i < itemSlots.Count; ++i)
        {
            if (itemSlots[i].item == null)
            {
                tmpItem = itemSlots[i];
                break;
            }
        }

        i_randSynthesisNum = 0;

        if (b_isNewHero)
        {
            print("여기2");
            switch (i_synthesisLevel)
            {
                case 1:
                    i_randSynthesisNum = Random.Range(0, 2);
                    break;
                case 2:
                    i_randSynthesisNum = Random.Range(2, 6);
                    break;
                case 3:
                    i_randSynthesisNum = Random.Range(6, 12);
                    break;
                case 4:
                    i_randSynthesisNum = Random.Range(12, 18);
                    break;
                case 5:
                    i_randSynthesisNum = Random.Range(18, 21);
                    break;
                case 6:
                    i_randSynthesisNum = 21;
                    break;
                default:
                    break;
            }

            g_SynthesisSlot_Scr.resultSynthesis(true, i_randSynthesisNum);
            Invoke("miniiconPopup", 1.2f);
            Invoke("popUPinvoke_hero", 4.0f);
        }
        else
        {
            switch (i_synthesisLevel)
            {
                case 1:
                    i_randSynthesisNum = Random.Range(0, 6);
                    break;
                case 2:
                    i_randSynthesisNum = Random.Range(6, 21);
                    break;
                case 3:
                    i_randSynthesisNum = Random.Range(21, 40);
                    break;
                case 4:
                    i_randSynthesisNum = Random.Range(40, 45);
                    break;
                case 5:
                    i_randSynthesisNum = Random.Range(45, 49);
                    break;
                case 6:
                    i_randSynthesisNum = 50;
                    break;
                default:
                    break;
            }

            g_SynthesisSlot_Scr.resultSynthesis(false, i_randSynthesisNum);
            Invoke("miniiconPopup", 1.2f);
            Invoke("popUPinvoke_item", 4.0f);
        }

        print(maxPoint + ", " + b_isNewHero + ", " + i_randSynthesisNum);

       // Save_Load.instance.Save();
    }

    void miniiconPopup_item()
    {

    }
    void miniiconPopup()
    {
        g_SynthesisSlot_parent.SetActive(true);
        SoundManager_sfx.instance.PlaySE("Gain", false);
        g_SynthesisSlot_Scr.gameObject.SetActive(true);

        DeliveryBoxManager_Scr.instance.g_PartipleCanvas.SetActive(true);
        Invoke("HighTierEffect", 0.1f);
    }

    void HighTierEffect()
    {


        switch (i_synthesisLevel)
        {
            case 4:
                par_JackpotParticle[0].Play();
                break;
            case 5:
                par_JackpotParticle[1].Play();
                break;
            case 6:
                par_JackpotParticle[2].Play();
                break;
        }

        /*
        if (i_synthesisLevel >= 4)
        {
            switch (i_synthesisLevel)
            {
                case 4:
                    par_JackpotParticle[0].Play();
                    SoundManager_sfx.instance.PlaySE("Jackpot", false);
                    break;
                case 5:
                    par_JackpotParticle[1].Play();
                    SoundManager_sfx.instance.PlaySE("Jackpot", false);
                    break;
                case 6:
                    par_JackpotParticle[2].Play();
                    SoundManager_sfx.instance.PlaySE("Jackpot", false);
                    break;
            }
        }*/
        Invoke("EffectSound", 0.8f);
    }

    void EffectSound()
    {
        switch (i_synthesisLevel)
        {
            case 4:
                SoundManager_sfx.instance.PlaySE("Jackpot", false);
                break;
            case 5:
                SoundManager_sfx.instance.PlaySE("Jackpot", false);
                break;
            case 6:
                SoundManager_sfx.instance.PlaySE("Jackpot", false);
                break;
        }
    }


    void popUPinvoke_item()
    {
        itemListTapOpen = true;
        tmpItem.LoadInfo(i_randSynthesisNum);

        Player_Input.instance.g_ButtonShield.SetActive(false);
        g_SynthesisSlot_parent.SetActive(false);
        Button_Option.instance.OnClickSynthesisOff();
        g_SynthesisEffectChasing.g_itemInfo.SetActive(true);
        g_SynthesisEffectChasing.setInfo_item(i_randSynthesisNum, i_synthesisLevel - 1, true);
    }
    void popUPinvoke_hero()
    {
        itemListTapOpen = false;
        Player_Input.instance.g_ButtonShield.SetActive(false);
        Inventory_Scr.instance.GetHeroSlot(i_randSynthesisNum, false);
        g_SynthesisSlot_parent.SetActive(false);
        Button_Option.instance.OnClickSynthesisOff();
        g_SynthesisEffectChasing.g_heroInfo.SetActive(true);
        g_SynthesisEffectChasing.setInfo_Hero(i_randSynthesisNum, i_synthesisLevel - 1, true);
    }
}