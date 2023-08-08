using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpenBox : MonoBehaviour
{
    public static OpenBox instance;

    public bool b_isOpen = false;

    // 박스 인포
    public GameObject g_Box;
    public GameObject g_Box_etc;
    public GameObject g_ShowNeedGem;

    // 골드 인포
    public GameObject GoldInfo;
    public TextMeshProUGUI t_Goldname;
    public TextMeshProUGUI t_GoldContent;
    public GameObject Goldshield;
    public GameObject Goldokbutton;

    // 히어로 인포
    public GameObject heroInfo;
    public TextMeshProUGUI t_name;
    public Image i_Hero;
    public Image i_Tier;
    public TextMeshProUGUI t_Content;
    public GameObject shield;
    public GameObject okbutton;
    public Animator heroFadein;

    // 인벤토리 히어로 인포
    public GameObject inven_heroInfo;
    public TextMeshProUGUI inven_t_name;
    public Image inven_i_Hero;
    public Image inven_i_Tier;
    public TextMeshProUGUI inven_t_Content;
    public GameObject inven_shield;
    public GameObject inven_okbutton;
    public Animator inven_heroFadein;


    // 아이템 인포
    public GameObject itemInfo;
    public TextMeshProUGUI t_itemname;
    public Image i_item;
    public Image i_itemTier;
    public GameObject g_statlistParent;
    public GameObject t_itemContent;
    public GameObject itemshield;
    public GameObject itemokbutton;
    public Animator itemFadein;

    public void readytoDelay()
    {
        if(instance != null)
        {
            return;
        }
        else
        {
            instance = this;
        }
    }


    public void setInfo_Gold(int amount)
    {
        GoldInfo.SetActive(true);
        g_Box.SetActive(false);
        t_Goldname.gameObject.SetActive(false);
        t_GoldContent.gameObject.SetActive(false);
        Goldshield.SetActive(false);
        Goldokbutton.SetActive(false);

        //  FadeIn(false);
        t_Goldname.text = TranslateManager_Scr.instance.TranslateData[189][TranslateManager_Scr.instance.s_Language].ToString();

        t_GoldContent.text = amount.ToString();

        Invoke("delaySetinfo_Gold", 2.0f);
    }

    void delaySetinfo_Gold()
    {
        t_Goldname.gameObject.SetActive(true);
        t_GoldContent.gameObject.SetActive(true);
        Goldshield.SetActive(true);
        Goldokbutton.SetActive(true);
        b_isOpen = false;
    }


    public void setInfo_Hero(int heronum, int tiernum, bool useinventory)
    {
        if (useinventory)
        {
            inven_heroInfo.SetActive(true);
            inven_t_name.gameObject.SetActive(true);
            inven_i_Tier.gameObject.SetActive(true);
            inven_t_Content.gameObject.SetActive(true);
            inven_shield.SetActive(true);
            inven_okbutton.SetActive(true);


            inven_t_name.text = Bag_Hero_ItmeCSV.instance.HeroData[heronum][TranslateManager_Scr.instance.s_Language + "_Name"].ToString();
            inven_i_Hero.sprite = InventoryCaching_Scr.instance.BoximageHeroicons[heronum];
            inven_i_Tier.sprite = InventoryCaching_Scr.instance.TierIcons[tiernum];

            string str1 = Bag_Hero_ItmeCSV.instance.HerointroductionData[heronum][TranslateManager_Scr.instance.s_Language].ToString();
            string str2 = str1.Replace("{0}", Bag_Hero_ItmeCSV.instance.HeroData[heronum]["{0}"].ToString());
            string str4 = str2.Replace("_", "\n");


            inven_t_Content.text = str4;
        }
        else
        {
            heroInfo.SetActive(true);
            g_Box.SetActive(false);
            t_name.gameObject.SetActive(false);
            i_Tier.gameObject.SetActive(false);
            t_Content.gameObject.SetActive(false);
            shield.SetActive(false);
            okbutton.SetActive(false);

            //  FadeIn(false);

            t_name.text = Bag_Hero_ItmeCSV.instance.HeroData[heronum][TranslateManager_Scr.instance.s_Language + "_Name"].ToString();
            i_Hero.sprite = InventoryCaching_Scr.instance.BoximageHeroicons[heronum];
            i_Tier.sprite = InventoryCaching_Scr.instance.TierIcons[tiernum];

            string str1 = Bag_Hero_ItmeCSV.instance.HerointroductionData[heronum][TranslateManager_Scr.instance.s_Language].ToString();
            string str2 = str1.Replace("{0}", Bag_Hero_ItmeCSV.instance.HeroData[heronum]["{0}"].ToString());


            string str4 = str2.Replace("_", "\n");


            t_Content.text = str4;

            Invoke("delaySetinfo_Hero", 2.0f);
        }
    }

    void delaySetinfo_Hero()
    {
        t_name.gameObject.SetActive(true);
        i_Tier.gameObject.SetActive(true);
        t_Content.gameObject.SetActive(true);
        shield.SetActive(true);
        okbutton.SetActive(true);
        heroFadein.SetBool("item", false);
        heroFadein.SetBool("hero", false);
        b_isOpen = false;
    }

    void FadeIn(bool isItem)
    {
        if (isItem)
        {
            heroFadein.SetBool("item", true);
            heroFadein.Play("FadeIN_Item");
        }
        else
        {
            heroFadein.SetBool("hero", true);
           itemFadein.Play("FadeIN_Hero");
        }
    }

    public void setInfo_item(int itemnum, int tiernum)
    {
        itemInfo.SetActive(true);
        g_Box.SetActive(false);
        t_itemname.gameObject.SetActive(false);
        i_itemTier.gameObject.SetActive(false);
        t_itemContent.SetActive(false);
        itemshield.SetActive(false);
        itemokbutton.SetActive(false);

        //  FadeIn(true);

        t_itemname.text = Bag_Hero_ItmeCSV.instance.ItemData[itemnum][TranslateManager_Scr.instance.s_Language].ToString();
        i_item.sprite = InventoryCaching_Scr.instance.itemIcons[itemnum];
        i_itemTier.sprite = InventoryCaching_Scr.instance.TierIcons[tiernum];

        PopUpInfo(itemnum, tiernum);
        Invoke("delaySetinfo_item", 2.0f);
    }

    void delaySetinfo_item()
    {
        t_itemname.gameObject.SetActive(true);
        i_itemTier.gameObject.SetActive(true);
        t_itemContent.SetActive(true);
        itemshield.SetActive(true);
        itemokbutton.SetActive(true);
        heroFadein.SetBool("item", false);
        heroFadein.SetBool("hero", false);
        b_isOpen = false;
    }

    public void PopUpInfo(int csvNum, int tiernum)
    {
        // 3D를 사용하지 않을 시 해당 내용을 씁시다.
        //InventoryCaching_Scr.instance.infoImage.sprite = cashingScr.itemImage.sprite;
        // 내용을 넣어줍시다.
        //InventoryCaching_Scr.instance.infoText.text = 


        string str = Bag_Hero_ItmeCSV.instance.ItemData[csvNum]["Effect"].ToString();
        string[] split = str.Split('_');
        
        for (int i = 0; i < 6; ++i)
        {
            g_statlistParent.transform.GetChild(i).gameObject.SetActive(false);
        }
        
        int infoNum = 0; // 스탯 갯수 

        for (int i = 0; i < split.Length; ++i)
        {
            string tmp = "";
            if (split[i] == "체력")
            {
                if (TranslateManager_Scr.instance.s_Language != "Korean")
                {
                    tmp += "HP";
                }
                else
                {
                    tmp += "체력";
                }

                g_statlistParent.transform.GetChild(0).gameObject.SetActive(true);
                g_statlistParent.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = tmp;
                g_statlistParent.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = Convert.ToSingle(Bag_Hero_ItmeCSV.instance.ItemData[csvNum]["HP"]).ToString();
                infoNum++;
            }
            if (split[i] == "회복")
            {
                if (TranslateManager_Scr.instance.s_Language != "Korean")
                {
                    tmp += "Heal";
                }
                else
                {
                    tmp += "회복";
                }

                g_statlistParent.transform.GetChild(1).gameObject.SetActive(true);
                g_statlistParent.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = tmp;
                g_statlistParent.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = Convert.ToSingle(Bag_Hero_ItmeCSV.instance.ItemData[csvNum]["Heal"]).ToString();
                infoNum++;
            }
            if (split[i] == "스피드")
            {
                if (TranslateManager_Scr.instance.s_Language != "Korean")
                {
                    tmp += "Speed";
                }
                else
                {
                    tmp += "스피드";
                }

                g_statlistParent.transform.GetChild(4).gameObject.SetActive(true);
                g_statlistParent.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = tmp;
                g_statlistParent.transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().text = Convert.ToSingle(Bag_Hero_ItmeCSV.instance.ItemData[csvNum]["Speed"]).ToString();
                infoNum++;
            }
            if (split[i] == "어빌리티")
            {
                if (TranslateManager_Scr.instance.s_Language != "Korean")
                {
                    tmp += "Ability";
                }
                else
                {
                    tmp += "어빌리티";
                }

                g_statlistParent.transform.GetChild(2).gameObject.SetActive(true);
                g_statlistParent.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = tmp;
                g_statlistParent.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = Convert.ToSingle(Bag_Hero_ItmeCSV.instance.ItemData[csvNum]["Ability"]).ToString();
                infoNum++;
            }
            if (split[i] == "치명")
            {
                if (TranslateManager_Scr.instance.s_Language != "Korean")
                {
                    tmp += "Critical";
                }
                else
                {
                    tmp += "치명";
                }

                g_statlistParent.transform.GetChild(5).gameObject.SetActive(true);
                g_statlistParent.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = tmp;
                g_statlistParent.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = Convert.ToSingle(Bag_Hero_ItmeCSV.instance.ItemData[csvNum]["Critical"]).ToString() + "%";
                infoNum++;
            }
            if (split[i] == "공격")
            {
                if (TranslateManager_Scr.instance.s_Language != "Korean")
                {
                    tmp += "Attack";
                }
                else
                {
                    tmp += "공격";
                }

                g_statlistParent.transform.GetChild(3).gameObject.SetActive(true);
                g_statlistParent.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = tmp;
                g_statlistParent.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = Convert.ToSingle(Bag_Hero_ItmeCSV.instance.ItemData[csvNum]["Attack"]).ToString();
                infoNum++;
            }
        }

        if (infoNum <= 3)
        {
            g_statlistParent.transform.GetComponent<GridLayoutGroup>().padding.top = 0;
            if (infoNum == 1)
            {
                g_statlistParent.transform.GetComponent<GridLayoutGroup>().padding.top = 80;
            }
            else if (infoNum == 2)
            {
                g_statlistParent.transform.GetComponent<GridLayoutGroup>().padding.top = 40;
            }
            g_statlistParent.transform.GetComponent<GridLayoutGroup>().constraintCount = 1;
        }
        else
        {
            if (infoNum == 4)
            {
                g_statlistParent.transform.GetComponent<GridLayoutGroup>().padding.top = 40;
            }
            g_statlistParent.transform.GetComponent<GridLayoutGroup>().constraintCount = 2;
        }

        /*
        InventoryCaching_Scr.instance.infoTierImage.sprite = InventoryCaching_Scr.instance.TierIcons[tiernum];
        InventoryCaching_Scr.instance.infoTitle.text = Bag_Hero_ItmeCSV.instance.ItemData[csvNum][TranslateManager_Scr.instance.s_Language].ToString();

        for (int i = 0; i < InventoryCaching_Scr.instance.itemIcons.Length; ++i)
        {
            if (csvNum == i)
            {
                InventoryCaching_Scr.instance.infoImage.sprite = InventoryCaching_Scr.instance.itemIcons[i];
            }
        }*/

       // InventoryCaching_Scr.instance.infoUI.SetActive(true);
    }

}