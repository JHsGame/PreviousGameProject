using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SynthesisEffectChasing : MonoBehaviour
{

    public ParticleSystem par_Particle;
    public GameObject g_itemInfo;
    public GameObject g_heroInfo;
       
    // 히어로 인포
    public GameObject heroInfo;
    public Text t_name;
    public Image i_Hero;
    public Image i_Tier;
    public Text t_Content;
    public GameObject shield;
    public GameObject okbutton;
    public Animator heroFadein;

    // 아이템 인포
    public GameObject itemInfo;
    public Text t_itemname;
    public Image i_item;
    public Image i_itemTier;
    public GameObject g_statlistParent;
    public GameObject t_itemContent;
    public GameObject itemshield;
    public GameObject itemokbutton;
    public Animator itemFadein;

    public void setInfo_Hero(int heronum, int tiernum, bool noParticle)
    {
        print(tiernum);
        if (!noParticle)
        {
            par_Particle.Play();
        }
        heroInfo.SetActive(true);
        shield.SetActive(true);
        i_Hero.gameObject.SetActive(true);
        t_name.gameObject.SetActive(true);
        i_Tier.gameObject.SetActive(true);
        t_Content.gameObject.SetActive(true);
        okbutton.SetActive(true);

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

    void delaySetinfo_Hero()
    {
        i_Hero.gameObject.SetActive(true);
        t_name.gameObject.SetActive(true);
        i_Tier.gameObject.SetActive(true);
        t_Content.gameObject.SetActive(true);
        okbutton.SetActive(true);
        heroFadein.SetBool("item", false);
        heroFadein.SetBool("hero", false);
    }
    public void setInfo_item(int itemnum, int tiernum, bool noParticle)
    {
        print(tiernum);
        if (!noParticle)
        {
            par_Particle.Play();
        }
        itemInfo.SetActive(true);
        itemshield.SetActive(true);
        i_item.gameObject.SetActive(true);
        t_itemname.gameObject.SetActive(true);
        i_itemTier.gameObject.SetActive(true);
        t_itemContent.SetActive(true);
        itemokbutton.SetActive(true);

        //  FadeIn(true);

        t_itemname.text = Bag_Hero_ItmeCSV.instance.ItemData[itemnum][TranslateManager_Scr.instance.s_Language].ToString();
        i_item.sprite = InventoryCaching_Scr.instance.itemIcons[itemnum];
        i_itemTier.sprite = InventoryCaching_Scr.instance.TierIcons[tiernum];

        PopUpInfo(itemnum, tiernum);
        Invoke("delaySetinfo_item", 2.0f);
    }

    void delaySetinfo_item()
    {
        i_item.gameObject.SetActive(true);
        t_itemname.gameObject.SetActive(true);
        i_itemTier.gameObject.SetActive(true);
        t_itemContent.SetActive(true);
        itemokbutton.SetActive(true);
        heroFadein.SetBool("item", false);
        heroFadein.SetBool("hero", false);
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
            else if(infoNum >= 5)
            {
                g_statlistParent.transform.GetComponent<GridLayoutGroup>().padding.top = 0;
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
