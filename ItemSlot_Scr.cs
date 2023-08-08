using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot_Scr : MonoBehaviour
{
    [SerializeField]
    ItemSlotButton_Scr buttonScr;
    ItemSlotCaching_Scr cachingScr;

    private Item myItem;

    [SerializeField]
    private bool b_isSelected;
    private int i_slotNum;
    private int i_csvNum;
    private int i_soldGold = 0;
    private List<float> f_Value = new List<float>();

    public Item item { get => myItem; }
    public bool isSelected { get => b_isSelected; set => b_isSelected = value; }
    public int slotNum { get => i_slotNum; }
    public int itemLevel { get => myItem.Level; }
    public int csvNum { get => i_csvNum; }
    public int soldGold { get => i_soldGold; }
    public List<float> itemValue { get => f_Value; }


    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        cachingScr = this.GetComponent<ItemSlotCaching_Scr>();

        buttonScr.SetScript(this, cachingScr);
        cachingScr.SetScript(this, buttonScr);
        i_slotNum = transform.GetSiblingIndex() + 1;    // �� ���� ��ȣ�� 1������

        StartCoroutine(UpdateCoroutine());
    }

    IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            if (InventoryCaching_Scr.instance != null)
            {
                if (InventoryCaching_Scr.instance.SellMode && myItem != null)
                {
                    cachingScr.CheckBox.SetActive(true);
                }
                else
                {
                    cachingScr.CheckBox.SetActive(false);
                }
            }
            yield return CoroutineManager_Scr.WaitForEndOfFrame;
        }
    }

    // ���� ��忡�� ���� ������ ���Ե��� Ȱ��ȭ �����ִ� �۾�
    // �Ǹ� ��忡���� ��� ���Ե��� ���� ����.
    // ���õǾ��ִ� ���Ե��� üũ�̹����� Ȱ��ȭ �����ݽô�.

    public void CheckImageOn()
    {
        cachingScr.CheckImage.gameObject.SetActive(true);
    }

    public void CheckImageOff()
    {
        cachingScr.CheckImage.gameObject.SetActive(false);
    }

    public void impossibleSelect()
    {
        b_isSelected = false;
        CheckImageOff();
        cachingScr.itemImage.GetComponent<Button>().enabled = false;
    }

    public void SelectedOff()
    {
        b_isSelected = false;
        CheckImageOff();
        cachingScr.itemImage.GetComponent<Button>().enabled = true;
    }

    public void PopUpInfo()
    {
        // 3D�� ������� ���� �� �ش� ������ ���ô�.
        //InventoryCaching_Scr.instance.infoImage.sprite = cashingScr.itemImage.sprite;
        // ������ �־��ݽô�.
        //InventoryCaching_Scr.instance.infoText.text = 

        string str = Bag_Hero_ItmeCSV.instance.ItemData[csvNum]["Effect"].ToString();
        string[] split = str.Split('_');

        for (int i = 0; i < 6; ++i)
        {
            InventoryCaching_Scr.instance.InfoParent.GetChild(i).GetComponent<Image>().sprite = InventoryCaching_Scr.instance.statIcons[i];
            InventoryCaching_Scr.instance.InfoParent.GetChild(i).gameObject.SetActive(false);
        }

        int infoNum = 0;

        for (int i = 0; i < split.Length; ++i)
        {
            string tmp = "";
            if (split[i] == "ü��")
            {
                if (TranslateManager_Scr.instance.s_Language != "Korean")
                {
                    tmp += "HP";
                }
                else
                {
                    tmp += "ü��";
                }

                InventoryCaching_Scr.instance.InfoParent.GetChild(0).gameObject.SetActive(true);
                InventoryCaching_Scr.instance.InfoParent.GetChild(0).GetChild(0).gameObject.SetActive(true);
                InventoryCaching_Scr.instance.InfoParent.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = tmp;
                InventoryCaching_Scr.instance.InfoParent.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = Convert.ToSingle(Bag_Hero_ItmeCSV.instance.ItemData[csvNum]["HP"]).ToString();
                infoNum++;
            }
            if (split[i] == "ȸ��")
            {
                if (TranslateManager_Scr.instance.s_Language != "Korean")
                {
                    tmp += "Heal";
                }
                else
                {
                    tmp += "ȸ��";
                }

                InventoryCaching_Scr.instance.InfoParent.GetChild(1).gameObject.SetActive(true);
                InventoryCaching_Scr.instance.InfoParent.GetChild(1).GetChild(0).gameObject.SetActive(true);
                InventoryCaching_Scr.instance.InfoParent.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = tmp;
                InventoryCaching_Scr.instance.InfoParent.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = Convert.ToSingle(Bag_Hero_ItmeCSV.instance.ItemData[csvNum]["Heal"]).ToString();
                infoNum++;
            }
            if (split[i] == "���ǵ�")
            {
                if (TranslateManager_Scr.instance.s_Language != "Korean")
                {
                    tmp += "Speed";
                }
                else
                {
                    tmp += "���ǵ�";
                }

                InventoryCaching_Scr.instance.InfoParent.GetChild(4).gameObject.SetActive(true);
                InventoryCaching_Scr.instance.InfoParent.GetChild(4).GetChild(0).gameObject.SetActive(true);
                InventoryCaching_Scr.instance.InfoParent.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = tmp;
                InventoryCaching_Scr.instance.InfoParent.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().text = Convert.ToSingle(Bag_Hero_ItmeCSV.instance.ItemData[csvNum]["Speed"]).ToString();
                infoNum++;
            }
            if (split[i] == "�����Ƽ")
            {
                if (TranslateManager_Scr.instance.s_Language != "Korean")
                {
                    tmp += "Ability";
                }
                else
                {
                    tmp += "�����Ƽ";
                }

                InventoryCaching_Scr.instance.InfoParent.GetChild(2).gameObject.SetActive(true);
                InventoryCaching_Scr.instance.InfoParent.GetChild(2).GetChild(0).gameObject.SetActive(true);
                InventoryCaching_Scr.instance.InfoParent.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = tmp;
                InventoryCaching_Scr.instance.InfoParent.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = Convert.ToSingle(Bag_Hero_ItmeCSV.instance.ItemData[csvNum]["Ability"]).ToString();
                infoNum++;
            }
            if (split[i] == "ġ��")
            {
                if (TranslateManager_Scr.instance.s_Language != "Korean")
                {
                    tmp += "Critical";
                }
                else
                {
                    tmp += "ġ��";
                }

                InventoryCaching_Scr.instance.InfoParent.GetChild(5).gameObject.SetActive(true);
                InventoryCaching_Scr.instance.InfoParent.GetChild(5).GetChild(0).gameObject.SetActive(true);
                InventoryCaching_Scr.instance.InfoParent.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = tmp;
                InventoryCaching_Scr.instance.InfoParent.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = Convert.ToSingle(Bag_Hero_ItmeCSV.instance.ItemData[csvNum]["Critical"]).ToString() + "%";
                infoNum++;
            }
            if (split[i] == "����")
            {
                if (TranslateManager_Scr.instance.s_Language != "Korean")
                {
                    tmp += "Attack";
                }
                else
                {
                    tmp += "����";
                }

                InventoryCaching_Scr.instance.InfoParent.GetChild(3).gameObject.SetActive(true);
                InventoryCaching_Scr.instance.InfoParent.GetChild(3).GetChild(0).gameObject.SetActive(true);
                InventoryCaching_Scr.instance.InfoParent.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = tmp;
                InventoryCaching_Scr.instance.InfoParent.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = Convert.ToSingle(Bag_Hero_ItmeCSV.instance.ItemData[csvNum]["Attack"]).ToString();
                infoNum++;
            }
        }

        if(infoNum <= 3)
        {
            InventoryCaching_Scr.instance.InfoParent.GetComponent<GridLayoutGroup>().padding.top = 0;
            if (infoNum == 1)
            {
                InventoryCaching_Scr.instance.InfoParent.GetComponent<GridLayoutGroup>().padding.top = 80;
            }
            else  if (infoNum == 2)
            {
                InventoryCaching_Scr.instance.InfoParent.GetComponent<GridLayoutGroup>().padding.top = 40;
            }
            InventoryCaching_Scr.instance.InfoParent.GetComponent<GridLayoutGroup>().constraintCount = 1;
        }
        else
        {
            if(infoNum == 4)
            {
                InventoryCaching_Scr.instance.InfoParent.GetComponent<GridLayoutGroup>().padding.top = 40;
            }
            else if(infoNum >= 5)
            {
                InventoryCaching_Scr.instance.InfoParent.GetComponent<GridLayoutGroup>().padding.top = 0;
            }
            InventoryCaching_Scr.instance.InfoParent.GetComponent<GridLayoutGroup>().constraintCount = 2;
        }

        InventoryCaching_Scr.instance.infoTierImage.sprite = InventoryCaching_Scr.instance.TierIcons[itemLevel - 1];
        InventoryCaching_Scr.instance.infoTitle.text = Bag_Hero_ItmeCSV.instance.ItemData[csvNum][TranslateManager_Scr.instance.s_Language].ToString();

        for(int i = 0; i < InventoryCaching_Scr.instance.itemIcons.Length; ++i)
        {
            if (i_csvNum == i)
            {
                InventoryCaching_Scr.instance.infoImage.sprite = InventoryCaching_Scr.instance.itemIcons[i];
            }
        }

        InventoryCaching_Scr.instance.infoUI.SetActive(true);
    }

    public void LoadInfo(int num)
    {
        i_csvNum = num;
        myItem = ItemList_Scr.instance.ItemLists[num];
        // �ش� ������ ��ȣ�� �̹����� ���������� ����
        cachingScr.itemImage.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
        cachingScr.itemImage.sprite = InventoryCaching_Scr.instance.itemIcons[num];

        if (myItem.HP > 0)
        {
            f_Value.Add(myItem.HP);
        }

        if (myItem.Heal > 0)
        {
            f_Value.Add(myItem.Heal);
        }

        if (myItem.Attack > 0)
        {
            f_Value.Add(myItem.Attack);
        }

        if (myItem.Speed > 0)
        {
            f_Value.Add(myItem.Speed);
        }

        if (myItem.Ability > 0)
        {
            f_Value.Add(myItem.Ability);
        }

        if (myItem.Critical > 0)
        {
            f_Value.Add(myItem.Critical);
        }

        switch (myItem.Level)
        {
            case 1:
                i_soldGold = 100;
                break;
            case 2:
                i_soldGold = 220;
                break;
            case 3:
                i_soldGold = 480;
                break;
            case 4:
                i_soldGold = 1040;
                break;
            case 5:
                i_soldGold = 2200;
                break;
            case 6:
                i_soldGold = 4640;
                break;
            default:
                break;
        }
    }

    public void ResetSlot()
    {
        myItem = null;
        b_isSelected = false;
        CheckImageOff();
        f_Value.Clear();
        cachingScr.itemImage.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0f / 255f);
        cachingScr.itemImage.sprite = null;
    }
}