using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class ShopManager_Scr : MonoBehaviour
{ 
    // 세이브 해야할 것들 -> 구매 여부, 변경한 이름들
    public static ShopManager_Scr instance;

    public GameObject g_ShopUI;
    public GameObject g_ChangeWindow;
    public GameObject g_InputField;
    public GameObject g_TutorialShield;
    public GameObject[] g_Inventory;
    public GameObject[] g_DeliveryBox;
    public GameObject[] g_MiddleNames;
    public GameObject[] g_TopNames;
    public GameObject[] g_BuyButton;

    public Sprite[] KR_Layer;
    public Sprite[] Other_Layer;

    public int i_ListNum;
    public int i_JobNum;
    public int i_Gem;

    public bool b_isMiddle;
    public bool b_isTop;

    public string[] s_MiddleNameList = { "주임", "대리", "과장", "부장" };
    public string[] s_TopNameList = { "이사", "상무", "전무", "사장" };

    public List<string> s_JuimName;
    public List<string> s_DealiName;
    public List<string> s_GwajangName;
    public List<string> s_BujangName;

    public List<string> s_IsaName;
    public List<string> s_SangmuName;
    public List<string> s_JeonmuName;
    public List<string> s_SajangName;

    public TextMeshProUGUI t_ShopJobName;
    public TMP_InputField t_ChangeName;

    public Transform t_GemShop;             // 젬 상점 위치
    public Transform t_CoinShop;            // 코인 상점 위치
    public Transform t_ParentCharacter;     // 캐릭터 상점 위치
    public Transform t_Shop;                // 이름 변경 상점 위치
    public Transform t_EnemyParent;         // 각 몬스터들의 위치를 잡기위한 부모 위치
    public Transform t_SkillParent;         // 스킬 상점 위치

    public Sprite[] s_MoneyIcon;

    public TextAsset myTxt;
    public List<Dictionary<string, object>> ShopData;

    public TextAsset t_TranslateCSV;
    public List<Dictionary<string, object>> TranslateData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
            return;
        ShopData = CSVReader.Read(myTxt);
    }
    public void readytoDelay()
    {
        Invoke("StartingCoroutine", 1f);
    }

    void StartingCoroutine()
    {
        StartCoroutine(UpdateCoroutine());
    }

    IEnumerator UpdateCoroutine()
    {
        while (true)
        { 
            if (Button_Option.instance.G_Shop.activeSelf)
            {
                for (int i = 0; i < g_Inventory.Length; ++i)
                {
                    if(i == (Inventory_Scr.instance.InventoryLevel - 1))
                    {
                        g_Inventory[i].SetActive(true);
                    }
                    else
                    {
                        g_Inventory[i].SetActive(false);
                    }
                }

                for (int i = 0; i < g_DeliveryBox.Length; ++i)
                {
                    if (i == (DeliveryBoxManager_Scr.instance.DeliveryLevel- 1))
                    {
                        g_DeliveryBox[i].SetActive(true);
                    }
                    else
                    {
                        g_DeliveryBox[i].SetActive(false);
                    }
                }
            }

            yield return CoroutineManager_Scr.WaitForEndOfFrame;
        }
    }

    public void InitShop()
    {
        // 젬 상점
        for (int i = 0; i < t_GemShop.childCount; ++i)
        {
            t_GemShop.GetChild(i).gameObject.GetComponent<ChargeCash_Scr>().i_PlusGem = int.Parse(ShopData[i]["Name"].ToString().Substring(2));
            t_GemShop.GetChild(i).gameObject.GetComponent<ChargeCash_Scr>().i_CashPrice = (int)ShopData[i]["Buy"];
        }

        // 코인 상점
        for (int i = 0; i < t_CoinShop.childCount; ++i)
        {
            t_CoinShop.GetChild(i).gameObject.GetComponent<ChargeCoin_Scr>().i_PlusCoin = int.Parse(ShopData[4 + i]["Name"].ToString().Substring(2));
            t_CoinShop.GetChild(i).gameObject.GetComponent<ChargeCoin_Scr>().i_GemPrice = (int)ShopData[4 + i]["Buy"];
        }

        // 캐릭터 바꾸는 상점
        for (int i = 0; i < t_ParentCharacter.childCount; ++i)
        {
            t_ParentCharacter.GetChild(i).gameObject.GetComponent<CharChangerStat_Scr>().i_Money = (int)ShopData[8 + i]["Buy"];
        }

        // 상점 프리미엄 아이템
        t_Shop.GetChild(0).GetComponent<ProductStat_Scr>().t_Name.text = TranslateManager_Scr.instance.TranslateContext(146);
        t_Shop.GetChild(0).GetComponent<ProductStat_Scr>().t_Context.text = TranslateManager_Scr.instance.TranslateContext(158);

        TranslateManager_Scr.instance.ChangeFont(t_Shop.GetChild(0).GetComponent<ProductStat_Scr>().t_Name, TranslateManager_Scr.instance.i_TMPNum);
        t_Shop.GetChild(0).GetComponent<ProductStat_Scr>().i_Cash = (int)ShopData[26]["Buy"];
        // 이름 바꾸는 상점
        for (int i = 0; i < g_MiddleNames.Length; ++i)
        {
            g_MiddleNames[i].GetComponent<ProductStat_Scr>().i_JobNum = i;
            g_MiddleNames[i].GetComponent<ProductStat_Scr>().b_isMiddle = true;
            g_MiddleNames[i].GetComponent<ProductStat_Scr>().b_isTop = false;

            for (int j = 0; j < 4; ++j)
            {
                if(TranslateManager_Scr.instance.b_isKorean || TranslateManager_Scr.instance.b_isJapanese || TranslateManager_Scr.instance.b_isChinese)
                    s_MiddleNameList[j] = TranslateManager_Scr.instance.TranslateData[24 + j][TranslateManager_Scr.instance.s_Language].ToString();
                else
                    s_MiddleNameList[j] = TranslateManager_Scr.instance.TranslateData[24 + j]["English"].ToString();
                g_MiddleNames[i].GetComponent<ProductStat_Scr>().t_Name.text = s_MiddleNameList[i];
                g_MiddleNames[i].GetComponent<ProductStat_Scr>().t_Context.text = TranslateManager_Scr.instance.TranslateData[32][TranslateManager_Scr.instance.s_Language].ToString();


                TranslateManager_Scr.instance.ChangeFont(g_MiddleNames[i].GetComponent<ProductStat_Scr>().t_Name, TranslateManager_Scr.instance.i_TMPNum);
            }
            g_MiddleNames[i].GetComponent<ProductStat_Scr>().i_Cash = (int)ShopData[14 + i]["Buy"];
        }
        int k = 0;
        for (int i = 0; i < g_TopNames.Length; ++i)
        {
            g_TopNames[i].GetComponent<ProductStat_Scr>().i_JobNum = k++;
            g_TopNames[i].GetComponent<ProductStat_Scr>().b_isMiddle = false;
            g_TopNames[i].GetComponent<ProductStat_Scr>().b_isTop = true;

            for (int j = 0; j < 4; ++j)
            {
                if (TranslateManager_Scr.instance.b_isKorean || TranslateManager_Scr.instance.b_isJapanese || TranslateManager_Scr.instance.b_isChinese)
                    s_TopNameList[j] = TranslateManager_Scr.instance.TranslateData[28 + j][TranslateManager_Scr.instance.s_Language].ToString();
                else
                    s_TopNameList[j] = TranslateManager_Scr.instance.TranslateData[28 + j]["English"].ToString();
                g_TopNames[i].GetComponent<ProductStat_Scr>().t_Name.text = s_TopNameList[i];
                g_TopNames[i].GetComponent<ProductStat_Scr>().t_Context.text = TranslateManager_Scr.instance.TranslateData[32][TranslateManager_Scr.instance.s_Language].ToString();



                TranslateManager_Scr.instance.ChangeFont(g_TopNames[i].GetComponent<ProductStat_Scr>().t_Name, TranslateManager_Scr.instance.i_TMPNum);
            }

            g_TopNames[i].GetComponent<ProductStat_Scr>().i_Cash = (int)ShopData[18 + i]["Buy"];
        }

        for (int i = 0; i < t_SkillParent.childCount; ++i)
        {
            if (ShopData[22 + t_SkillParent.GetChild(i).GetSiblingIndex()]["Type"].ToString() == "Coin")
            {
                t_SkillParent.GetChild(i).gameObject.GetComponent<SkillStat_Scr>().s_Icon = s_MoneyIcon[0];
                //t_SkillParent.GetChild(i).gameObject.GetComponent<SkillStat_Scr>().b_isGold = true;
                //t_SkillParent.GetChild(i).gameObject.GetComponent<SkillStat_Scr>().i_Coin = (int)ShopData[22 + t_SkillParent.GetChild(i).GetSiblingIndex()]["Buy"];
            }
            else if (ShopData[22 + t_SkillParent.GetChild(i).GetSiblingIndex()]["Type"].ToString() == "Gem")
            {
                t_SkillParent.GetChild(i).gameObject.GetComponent<SkillStat_Scr>().s_Icon = s_MoneyIcon[1];
                //t_SkillParent.GetChild(i).gameObject.GetComponent<SkillStat_Scr>().b_isGold = false;
                //t_SkillParent.GetChild(i).gameObject.GetComponent<SkillStat_Scr>().i_Gem = (int)ShopData[22 + t_SkillParent.GetChild(i).GetSiblingIndex()]["Buy"];
            }
        }
    }

    public void ChangeInputField()
    {
        //t_ChangeName.placeholder.GetComponent<Text>().text = TranslateManager_Scr.instance.TranslateContext(157);
    }

    public void OnClickChangeName(bool top, int num)
    {
        if (!string.IsNullOrWhiteSpace(t_ChangeName.text))
        {
            if (!top)
            {
                switch (num)
                {
                    case 0:
                        s_JuimName.Add(t_ChangeName.text);
                        RespawnManager.instance.s_CheckJuimName.Add(false);
                        break;
                    case 1:
                        s_DealiName.Add(t_ChangeName.text);
                        RespawnManager.instance.s_CheckDealiName.Add(false);
                        break;
                    case 2:
                        s_GwajangName.Add(t_ChangeName.text);
                        RespawnManager.instance.s_CheckGwajangName.Add(false);
                        break;
                    case 3:
                        s_BujangName.Add(t_ChangeName.text);
                        RespawnManager.instance.s_CheckBujangName.Add(false);
                        break;
                    default:
                        break;
                }
            }
            else if (top)
            {
                switch (num)
                {
                    case 0:
                        s_IsaName.Add(t_ChangeName.text);
                        break;
                    case 1:
                        s_SangmuName.Add(t_ChangeName.text);
                        break;
                    case 2:
                        s_JeonmuName.Add(t_ChangeName.text);
                        break;
                    case 3:
                        s_SajangName.Add(t_ChangeName.text);
                        break;
                    default:
                        break;
                }
            }

            t_ChangeName.text = null;
            Save_Load.instance.Save();
            Close();
        }
    }

    public void OnClickTutorialAddName()
    {
        if (!string.IsNullOrWhiteSpace(t_ChangeName.text))
        {
            s_JuimName.Add(t_ChangeName.text);
            RespawnManager.instance.s_CheckJuimName.Add(false);
        }

        if (!Button_Option.instance.b_Golobby && RespawnManager.instance.b_NomalGame && TutorialManager.instance.b_FirstPlaying && !TutorialManager.instance.b_LastTutorial)
        {
            for (int i = 0; i < 5; i++)
            {
                Button_Option.instance.G_Shop.transform.GetChild(i).gameObject.SetActive(true);
            }

            TutorialManager.instance.go_TutorialGroup.gameObject.SetActive(true);
            TutorialManager.instance.go_BG.transform.GetComponent<Animator>().Play("Tutorial_FadeOut_BG");
            TutorialManager.instance.go_TutorialTitle.transform.GetComponent<Animator>().Play("Tutorial_FadeOut_Text");
            TutorialManager.instance.go_TutorialContent.transform.GetComponent<Animator>().Play("Tutorial_FadeOut_Text");
            TutorialManager.instance.go_TutorialMissionGroup.SetActive(true);
            Button_Option.instance.G_Shop.SetActive(false);
            Time.timeScale = 0;
        }

        g_ChangeWindow.transform.GetChild(1).gameObject.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(750f, 700f);
        g_ChangeWindow.transform.GetChild(2).gameObject.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -70f);
        g_ChangeWindow.transform.GetChild(3).gameObject.SetActive(true);
        g_ChangeWindow.transform.GetChild(4).gameObject.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -132f);
        g_ChangeWindow.transform.GetChild(6).gameObject.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(150f, -250f);

        Save_Load.instance.Save();
        Close();
    }

    public void Close()
    {
        t_ChangeName.text = null;
        if (TutorialManager.instance.b_FirstPlaying)
        {
            if (!Button_Option.instance.b_Golobby && RespawnManager.instance.b_NomalGame && TutorialManager.instance.b_FirstPlaying && !TutorialManager.instance.b_LastTutorial)
            {
                for (int i = 0; i < 5; i++)
                {
                    Button_Option.instance.G_Shop.transform.GetChild(i).gameObject.SetActive(true);
                }
                TutorialManager.instance.go_TutorialGroup.gameObject.SetActive(true);
                TutorialManager.instance.go_BG.transform.GetComponent<Animator>().Play("Tutorial_FadeOut_BG");
                TutorialManager.instance.go_TutorialTitle.transform.GetComponent<Animator>().Play("Tutorial_FadeOut_Text");
                TutorialManager.instance.go_TutorialContent.transform.GetComponent<Animator>().Play("Tutorial_FadeOut_Text");
                TutorialManager.instance.go_TutorialMissionGroup.SetActive(true);
                Button_Option.instance.G_Shop.SetActive(false);
                Time.timeScale = 0;
            }
        }
        g_ChangeWindow.SetActive(false);
    }

    public void InventoryLevelUp()
    {
        Inventory_Scr.instance.InventoryLevel++;
        Save_Load.instance.Save();
    }

    public void DeliveryBoxLevelUp()
    {
        DeliveryBoxManager_Scr.instance.DeliveryLevel++;
        Save_Load.instance.Save();
    }

    public void OnClickCanclePruchase()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);
        AttendanceManager_Scr.instance.b_AutoPause = true;
    }
}