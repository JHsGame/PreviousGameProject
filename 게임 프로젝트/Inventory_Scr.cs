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

    private List<ItemSlot_Scr> slots = new List<ItemSlot_Scr>();   // ������ ���� ����Ʈ
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
    private bool b_isFull;              // ���� �κ��丮�� �������� �� ���־� �� �̻� �������� ȹ���� �� ���� üũ���ִ� ����.

    // �굵 Item�� ���� ���� �߰� ������ �Ȱ��� �����ͼ� ��������.
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

    // �߰��Ǵ� ���� ������ ���̳� ĳ���� �� �ο�������.
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
    

    // �κ��丮�� �ִ� �����ۿ� ���� �߰� �ɷ�ġ ��� �Լ�.
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

    // ���� ũ�⿡ ���� ������ Ȱ��ȭ��Ű��.
    IEnumerator InventoryReSize()
    {
        // ���� ���� �� ������ ���� ���� ���� ��������.
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
                AddItemAbility(item.sort, slot.itemValue);   // ���� �߰��Ǵ� �ɷ�ġ�� 2���� �̻��̶�� ������ �ʿ䰡 ����.
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
        // ���⿡ ���� ���� �޾ƿ���
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

    // �߰� �ɷ�ġ ���.
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
        // �Ƹ� �������� ������ ������ �����ָ� ���� ������? - �ǵ���� ���� ���Ͻ� ������ ���� ����
        GoldManager_Scr.instance.i_Coin += i_SoldGold;
        i_SoldGold = 0;

        ClearList();

        Save_Load.instance.Save();
    }


    // ���ڱ� �õ��� ���� ��ĭ�� ������ �ִ��� üũ �� ���� ������ -> ���� ī��Ʈ �ִ��� ������ üũ || ������ ���ٸ� ���â ���� (������ �����ϰų� ���׷��̵� ���ּ���)
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

            if (isfull) // ��ĭ�� �����̴� ���â ����
            {
                InventoryCaching_Scr.instance.warninginventory.SetActive(true);
            }
            else // ��ĭ ���������� ���� ī��Ʈ �����ϴ��� üũ�ϱ� 
            {
                // ���� ī��Ʈ���� Ȯ��
                if (DeliveryBoxManager_Scr.instance.GetBoxCount > 0)
                {
                    // ���� ����ϰڳĴ� â ���� 
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
                // ���� ī��Ʈ��� ������ 10 �ִ��� Ȯ���ϱ� 
                else
                {
                    if (GoldManager_Scr.instance.i_Gem >= 10)
                    {

                        // ���� ����ϰڳĴ� â ���� 
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
                    // 10 ���� ������ ������ ���� ���� ���â ����
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

        if (isfull) // ��ĭ�� �����̴� ���â ����
        {
            InventoryCaching_Scr.instance.warninginventory.SetActive(true);
        }
        else // ��ĭ ���������� ���� ī��Ʈ �����ϴ��� üũ�ϱ� 
        {
            // ���� ī��Ʈ���� Ȯ��
            if (DeliveryBoxManager_Scr.instance.GetBoxCount > 0)
            {
                // ���� ����ϰڳĴ� â ���� 
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
            // ���� ī��Ʈ��� ������ 10 �ִ��� Ȯ���ϱ� 
            else
            {
                if (GoldManager_Scr.instance.i_Gem >= 10)
                {
                    // ���� ����ϰڳĴ� â ���� 
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
                // 10 ���� ������ ������ ���� ���� ���â ����
                else
                {
                    InventoryCaching_Scr.instance.warninginventoryGem.SetActive(true);
                }
            }
        }
    }
}