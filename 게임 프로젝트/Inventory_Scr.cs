using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_Scr : MonoBehaviour
{
    private static Inventory_Scr Instance;

    // csv
    public List<Dictionary<string, object>> bagData;

    private List<ItemSlot_Scr> slots = new List<ItemSlot_Scr>();   // 아이템 슬롯 리스트
    private List<ItemSlot_Scr> sellSlots = new List<ItemSlot_Scr>();
    private List<int> heroListNum = new List<int>();
    private List<GameObject> heroList = new List<GameObject>();
    private int i_SoldGold = 0;
    private int i_itemAmount = 0;
    [SerializeField]
    private int i_inventoryLevel = 1;
    private int i_InventorySize = 20;
    private int i_inventoryPrice = 0;
    [SerializeField]
    private bool b_isFull;              // 현재 인벤토리에 아이템이 꽉 차있어 더 이상 아이템을 획득할 수 없게 체크해주는 변수.

    // 얘도 Item에 들어가는 스텟 추가 정보를 똑같이 가져와서 적어주자.
    [SerializeField]
    private float f_HP;
    [SerializeField]
    private float f_Heal;
    [SerializeField]
    private float f_Attack;
    [SerializeField]
    private float f_Speed;
    [SerializeField]
    private float f_Ability;
    [SerializeField]
    private float f_Critical;


    List<float> Item_Stats = new List<float>();

    // 추가되는 스텟 정보를 공이나 캐릭터 등등에 부여해주자.
    public static Inventory_Scr instance { get => Instance; }
    public bool InventoryFull { get => b_isFull; }
    public int InventoryLevel { get => i_inventoryLevel; set => i_inventoryLevel = value; }
    public int InventoryPrice { get => i_inventoryPrice; }
    public int SoldGold { get => i_SoldGold; set => i_SoldGold = value; }
    public float AddHP { get => f_HP; }
    public float AddHeal { get => f_Heal; }
    public float AddAttack { get => f_Attack; }
    public float AddBallSpeed { get => f_Speed; }
    public float AddAbility { get => f_Ability; }
    public float AddCritical { get => f_Critical; }
    public List<ItemSlot_Scr> myItems { get => slots; }
    public List<int> loadHeroList { get => heroListNum; }
    public List<GameObject> heroListObjs { get => heroList; }
    void Start()
    {
        if (Instance != null)
        {
            return;
        }
        else
        {
            Instance = this;
        }
    }

    public void ClearList()
    {
        for (int i = 0; i < slots.Count; ++i)
        {
            slots[i].SelectedOff();
        }

        sellSlots.Clear();
        InventoryCaching_Scr.instance.SellMode = false;
        InventoryCaching_Scr.instance.SellOrBackImage.sprite = InventoryCaching_Scr.instance.SellOrBackIcons[0];

        InventoryCaching_Scr.instance.OKButton.SetActive(true);
        InventoryCaching_Scr.instance.SellButton.SetActive(false);

        i_SoldGold = 0;
    }

    public void readytoDelay()
    {
        Invoke("StartingCoroutine", 1f);

        Transform parent = InventoryCaching_Scr.instance.itemSlotParent;

        for (int i = 0; i < parent.childCount; ++i)
        {
            slots.Add(parent.GetChild(i).GetComponent<ItemSlot_Scr>());
        }
    }

    public void StartingCoroutine()
    {
        for (int i = 0; i < 6; ++i)
        {
            InventoryCaching_Scr.instance.statParent.GetChild(i).GetComponent<Image>().sprite = InventoryCaching_Scr.instance.statIcons[i];
        }

        StartCoroutine(InventoryChecker());
        StartCoroutine(InventoryReSize());
    }
    

    // 인벤토리에 있는 아이템에 따른 추가 능력치 계산 함수.
    IEnumerator InventoryChecker()
    {
        while (true)
        {
            InventoryCaching_Scr.instance.tierImage.sprite = InventoryCaching_Scr.instance.TierIcons[i_inventoryLevel - 1];

            if (!Button_Option.instance.G_Inventory.activeSelf)
            {
                CalculateStats();
            }

            if (InventoryCaching_Scr.instance.SoldUI.activeSelf)
            {
                InventoryCaching_Scr.instance.SoldUI.transform.GetChild(0).GetComponent<Text>().text = i_SoldGold.ToString();
            }


            int count = 0;
            b_isFull = false;
            for (int i = 0; i < i_InventorySize; ++i)
            {

                if (slots[i].item == null)
                {
                    b_isFull = false;
                    break;
                }
                else
                {
                    count++;
                    if (count >= i_InventorySize)
                    {

                        b_isFull = true;
                    }
                }
            }

            for (int i = 0; i < i_InventorySize; ++i)
            {
                if (slots[i].item != null)
                {
                    slots[i].GetComponent<ItemSlotCaching_Scr>().itemImage.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
                }
                else
                {
                    slots[i].GetComponent<ItemSlotCaching_Scr>().itemImage.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0f / 255f);
                    slots[i].transform.SetSiblingIndex(i_InventorySize - 1);
                }
            }

            string tmp = i_itemAmount + "/" + i_InventorySize + "\n";
            if (InventoryCaching_Scr.instance.invenUI.activeSelf)
            {
                InventoryCaching_Scr.instance.invenUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tmp + TranslateManager_Scr.instance.TranslateContext(197);

                TranslateManager_Scr.instance.ChangeFont(InventoryCaching_Scr.instance.invenUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>(), TranslateManager_Scr.instance.i_TMPNum);
            }

            yield return CoroutineManager_Scr.WaitForEndOfFrame;
        }
    }

    // 가방 크기에 따라 슬롯을 활성화시키기.
    IEnumerator InventoryReSize()
    {
        // 최초 실행 시 레벨에 따른 설정 값을 가져오기.
        InitInventory();

        while (true)
        {
            if (slots.Count > 0)
            {
                foreach (ItemSlot_Scr slot in slots)
                {
                    if (slot.slotNum <= i_InventorySize)
                    {
                        slot.gameObject.SetActive(true);
                    }
                    else
                    {
                        slot.gameObject.SetActive(false);
                    }
                }
            }
            yield return CoroutineManager_Scr.WaitForEndOfFrame;
        }
    }

    public void InitInventory()
    {
        if (bagData != null)
        {
            InventoryCaching_Scr.instance.invenUI.GetComponent<Image>().sprite = InventoryCaching_Scr.instance.bagIcons[i_inventoryLevel - 1];
            i_InventorySize = (int)(Bag_Hero_ItmeCSV.instance.BagData[i_inventoryLevel - 1]["capacity"]);
            i_inventoryPrice = (int)(Bag_Hero_ItmeCSV.instance.BagData[i_inventoryLevel]["Price"]);
            TranslateManager_Scr.instance.InventoryTranslate();
        }
    }

    public void CalculateStats()
    {
        Item_Stats.Clear();
        i_itemAmount = 0;
        f_HP = f_Heal = f_Attack = f_Speed = f_Ability = f_Critical = 0f;

        foreach (ItemSlot_Scr slot in slots)
        {
            if (slot.gameObject.activeSelf && slot.item != null)
            {
                i_itemAmount++;
                Item item = slot.item;
                AddItemAbility(item.sort, slot.itemValue);   // 만일 추가되는 능력치가 2가지 이상이라면 수정할 필요가 있음.
            }
        }
        Item_Stats.Add(f_HP);
        Item_Stats.Add(f_Heal);
        Item_Stats.Add(f_Ability);
        Item_Stats.Add(f_Attack);
        Item_Stats.Add(f_Speed);
        Item_Stats.Add(f_Critical);

        for (int i = 0; i < 6; ++i)
        {
            InventoryCaching_Scr.instance.statParent.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = (System.Math.Truncate(Item_Stats[i] * 100) / 100).ToString();
        }
    }

    public void LoadItem(int invenNum, int itemNum)
    {
        slots[invenNum].LoadInfo(itemNum);
    }

    public void LoadHero(int HeroNum)
    {
        // 여기에 영웅 정보 받아오기
        GetHeroSlot(HeroNum, true);
        ManPowerManager_Scr.instance.Coroutine_Sort();
    }


    public void GetItem(int start, int end, int tier)
    {
        for (int i = 0; i < slots.Count; ++i)
        {
            if (slots[i].item == null)
            {
                print(start + ", " + end + ", " + "tier : " + tier + 1);
                int tmp = Random.Range(start, end);
                OpenBox.instance.setInfo_item(tmp, tier);
                slots[i].LoadInfo(tmp);
                break;
            }
        }
    }

    public void GetHero(float Tier)
    {
        int start = 0;
        int end = 0;

        switch (Tier)
        {
            case 0:
                start = 0;
                end = 2;
                break;
            case 1:
                start = 2;
                end = 6;
                break;
            case 2:
                start = 6;
                end = 12;
                break;
            case 3:
                start = 12;
                end = 18;
                break;
            case 4:
                start = 18;
                end = 21;
                break;
            case 5:
                start = 21;
                end = 22;
                break;
        }
        
        int tmp = Random.Range(start, end);

        OpenBox.instance.setInfo_Hero(tmp, (int)Tier, false);
        GetHeroSlot(tmp, false);

        ManPowerManager_Scr.instance.Coroutine_Sort();
    }

    public void GetHeroSlot(int num, bool isLoad)
    {
        GameObject obj = Instantiate(ManPowerManager_Scr.instance.g_HeroSlot, ManPowerManager_Scr.instance.t_ParentMans);
        obj.transform.GetComponent<ManStat_Scr>().i_HeroNum = num;
        obj.transform.GetComponent<ManStat_Scr>().setHero();

        GetHeroInfo(obj, obj.transform.GetComponent<ManStat_Scr>().i_HeroNum, isLoad);
    }

    public void GetHeroInfo(GameObject obj, int heroNum, bool isLoad)
    {
        ManPowerManager_Scr.instance.heroList.Add(obj);
        if (isLoad)
        {
            heroListNum.Add(heroNum);
            heroListObjs.Add(obj);
        }
        else
        {
            Synthesis_Scr.instance.SetHeroSlot(heroNum, obj);
        }
    }

    public void GetGold()
    {
        float getGold = Random.Range(30, 301);

        getGold = getGold * 0.1f;
        int result = Mathf.CeilToInt(getGold) * 10;

        GoldManager_Scr.instance.PlusGold(result);
        OpenBox.instance.setInfo_Gold(result);
    }

    // 추가 능력치 계산.
    public void AddItemAbility(List<string> str, List<float> values)
    {
        for (int i = 0; i < str.Count; ++i)
        {
            switch (str[i])
            {
                case "HP":
                    f_HP += values[i];
                    break;
                case "Heal":
                    f_Heal += values[i];
                    break;
                case "Attack":
                    f_Attack += values[i];
                    break;
                case "Speed":
                    f_Speed += values[i];
                    break;
                case "Ability":
                    f_Ability += values[i];
                    break;
                case "Critical":
                    f_Critical += values[i];
                    break;
                default:
                    break;
            }
        }
    }
    
    public void SetSellSlot(ItemSlot_Scr _slot)
    {
        sellSlots.Add(_slot);
    }

    public void RejectSell(ItemSlot_Scr _slot)
    {
        _slot.CheckImageOff();
        sellSlots.Remove(_slot);
    }

    public void SellSlots()
    {
        for (int i = 0; i < sellSlots.Count; ++i)
        {
            sellSlots[i].ResetSlot();
        }
        // 아마 보석으로 샀으니 보석에 더해주면 되지 않을까? - 피디님이 골드로 정하심 가격을 추후 결정
        GoldManager_Scr.instance.i_Coin += i_SoldGold;
        i_SoldGold = 0;

        ClearList();

        Save_Load.instance.Save();
    }


    // 상자깡 시도시 현재 템칸이 여유가 있는지 체크 후 여유 있으면 -> 무료 카운트 있는지 없는지 체크 || 여유가 없다면 경고창 띄우기 (가방을 정리하거나 업그레이드 해주세요)
    public void InventoryCheck()
    {

        SoundManager_sfx.instance.PlaySE("UI_Click1", false);
        if (!Button_Option.instance.b_Attence)
        {
            Button_Option.instance.cancelinvoke_attendance();
        }

        if (!TutorialManager.instance.b_BoxTutorial)
        {
            TutorialManager.instance.G_BoxTutorial.SetActive(true);
        }
        else
        {
            int count = 0;
            bool isfull = false;
            for (int i = 0; i < i_InventorySize; ++i)
            {

                if (slots[i].item == null)
                {
                    isfull = false;
                    break;
                }
                else
                {
                    count++;
                    if (count >= i_InventorySize)
                    {

                        isfull = true;
                    }
                }
            }

            if (isfull) // 템칸이 가득이니 경고창 띄우기
            {
                InventoryCaching_Scr.instance.warninginventory.SetActive(true);
            }
            else // 템칸 여유있으니 무료 카운트 존재하는지 체크하기 
            {
                // 무료 카운트인지 확인
                if (DeliveryBoxManager_Scr.instance.GetBoxCount > 0)
                {
                    // 상자 사용하겠냐는 창 띄우기 
                    DeliveryBoxManager_Scr.instance.openningBox.SetActive(true);

                    for (int i = 0; i < 2; i++)
                    {
                        DeliveryBoxManager_Scr.instance.openningBox.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        if (DeliveryBoxManager_Scr.instance.BoxUI.transform.GetChild(i).gameObject.activeSelf)
                        {
                            DeliveryBoxManager_Scr.instance.openningBox.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
                            break;
                        }
                    }


                }
                // 유료 카운트라면 보석이 10 있는지 확인하기 
                else
                {
                    if (GoldManager_Scr.instance.i_Gem >= 10)
                    {

                        // 상자 사용하겠냐는 창 띄우기 
                        DeliveryBoxManager_Scr.instance.openningBox.SetActive(true);

                        for (int i = 0; i < 2; i++)
                        {
                            DeliveryBoxManager_Scr.instance.openningBox.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
                        }
                        for (int i = 0; i < 2; i++)
                        {
                            if (DeliveryBoxManager_Scr.instance.BoxUI.transform.GetChild(i).gameObject.activeSelf)
                            {
                                DeliveryBoxManager_Scr.instance.openningBox.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
                                break;
                            }
                        }
                    }
                    // 10 보석 없으면 소지된 보석 부족 경고창 띄우기
                    else
                    {
                        InventoryCaching_Scr.instance.warninginventoryGem.SetActive(true);
                    }
                }
            }
        }


    }


    public void InventoryCheck_ConfirmTutorial()
    {
        int count = 0;
        bool isfull = false;
        for (int i = 0; i < i_InventorySize; ++i)
        {

            if (slots[i].item == null)
            {
                isfull = false;
                break;
            }
            else
            {
                count++;
                if (count >= i_InventorySize)
                {

                    isfull = true;
                }
            }
        }

        if (isfull) // 템칸이 가득이니 경고창 띄우기
        {
            InventoryCaching_Scr.instance.warninginventory.SetActive(true);
        }
        else // 템칸 여유있으니 무료 카운트 존재하는지 체크하기 
        {
            // 무료 카운트인지 확인
            if (DeliveryBoxManager_Scr.instance.GetBoxCount > 0)
            {
                // 상자 사용하겠냐는 창 띄우기 
                DeliveryBoxManager_Scr.instance.openningBox.SetActive(true);

                for (int i = 0; i < 2; i++)
                {
                    DeliveryBoxManager_Scr.instance.openningBox.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
                }
                for (int i = 0; i < 2; i++)
                {
                    if (DeliveryBoxManager_Scr.instance.BoxUI.transform.GetChild(i).gameObject.activeSelf)
                    {
                        DeliveryBoxManager_Scr.instance.openningBox.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
                        break;
                    }
                }


            }
            // 유료 카운트라면 보석이 10 있는지 확인하기 
            else
            {
                if (GoldManager_Scr.instance.i_Gem >= 10)
                {
                    // 상자 사용하겠냐는 창 띄우기 
                    DeliveryBoxManager_Scr.instance.openningBox.SetActive(true);

                    for (int i = 0; i < 2; i++)
                    {
                        DeliveryBoxManager_Scr.instance.openningBox.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        if (DeliveryBoxManager_Scr.instance.BoxUI.transform.GetChild(i).gameObject.activeSelf)
                        {
                            DeliveryBoxManager_Scr.instance.openningBox.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
                            break;
                        }
                    }
                }
                // 10 보석 없으면 소지된 보석 부족 경고창 띄우기
                else
                {
                    InventoryCaching_Scr.instance.warninginventoryGem.SetActive(true);
                }
            }
        }
    }
}