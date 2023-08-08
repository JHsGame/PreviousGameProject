using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TranslateManager_Scr : MonoBehaviour
{ 
    public enum UIFont { Maple, NanumBarunGothic }
    public static TranslateManager_Scr instance;
    public SynthesisEffectChasing synthesisEffect;
    RegionInfo curCountry;  // 국가 정보

    public TextAsset myTxt;
    public TextAsset t_SurnameCSV;
    public List<Dictionary<string, object>> TranslateData;
    public List<Dictionary<string, object>> SurnameData;

    public Button_Option g_Lobby;
    public GameObject newVersionAvailable; // 버전확인 UI
    public Transform t_BeforeTranslateParent;
    public Transform t_AfterTranslateParent;
    public TextMeshProUGUI t_ADPremium;
    public TextMeshProUGUI OpenBoxOk;
    public TextMeshProUGUI OpenGoldOK;
    public TextMeshProUGUI OpenItemOK;
    public TextMeshProUGUI OpenHeroOK;
    public TextMeshProUGUI HeroInvenOK;

    public Image i_Title;
    public Sprite[] s_Title;

    // 0-한국, 1-영어, 2-일본, 3-중국
    public GameObject[] g_BeforeButtons;
    public GameObject[] g_AfterButtons;
    public GameObject g_BeforeDropDown;
    public GameObject g_AfterDropDown;
    public TextMeshProUGUI[] g_Review;

    public bool b_isKorean = true;
    public bool b_isEnglish = false;
    public bool b_isJapanese = false;
    public bool b_isChinese = false;
    public bool b_isIndia = false;
    public bool b_isVietnam = false;
    public bool b_isFrance = false;
    public bool b_isGermany = false;
    public bool b_isItalia = false;


    public int i_LanguageValue;
    public int i_LanguageIdx;
    public int i_TMPNum = 0;
    public string s_Language = "";

    public Font[] f_Font = new Font[2];
    public TMP_FontAsset[] f_TMPFont = new TMP_FontAsset[6];
    public Material[] f_TMPMaterial = new Material[9];

    int Idx = 0;
    string str;
    string[] split;

    SystemLanguage language;
    public UIFont myFont;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            TranslateData = CSVReader.Read(myTxt);
            SurnameData = CSVReader.Read(t_SurnameCSV);


            //f_Font[1] = Resources.GetBuiltinResource<Font>("Arial.ttf");

        }
        else
            return;

    }

    private void OnEnable()
    {
        // 휴대폰 기기가 사용하는 언어 정보를 가져와서 해당 언어로 번역해서 시작하도록 설정
        language = Application.systemLanguage;
        switch (language)
        {
            case SystemLanguage.Afrikaans:
                s_Language = "English";
                break;
            case SystemLanguage.Arabic:
                s_Language = "English";
                break;
            case SystemLanguage.Basque:
                s_Language = "English";
                break;
            case SystemLanguage.Belarusian:
                s_Language = "English";
                break;
            case SystemLanguage.Bulgarian:
                s_Language = "English";
                break;
            case SystemLanguage.Catalan:
                s_Language = "English";
                break;
            case SystemLanguage.Chinese:
                s_Language = "Chinese";
                break;
            case SystemLanguage.Czech:
                s_Language = "English";
                break;
            case SystemLanguage.Danish:
                s_Language = "English";
                break;
            case SystemLanguage.Dutch:
                s_Language = "English";
                break;
            case SystemLanguage.English:
                s_Language = "English";
                break;
            case SystemLanguage.Estonian:
                s_Language = "English";
                break;
            case SystemLanguage.Faroese:
                s_Language = "English";
                break;
            case SystemLanguage.Finnish:
                s_Language = "English";
                break;
            case SystemLanguage.French:
                s_Language = "France";
                break;
            case SystemLanguage.German:
                s_Language = "Germany";
                break;
            case SystemLanguage.Greek:
                s_Language = "English";
                break;
            case SystemLanguage.Hebrew:
                s_Language = "English";
                break;
            case SystemLanguage.Hungarian:
                s_Language = "English";
                break;
            case SystemLanguage.Icelandic:
                s_Language = "English";
                break;
            case SystemLanguage.Indonesian:
                s_Language = "English";
                break;
            case SystemLanguage.Italian:
                s_Language = "Italia";
                break;
            case SystemLanguage.Japanese:
                s_Language = "Japanese";
                break;
            case SystemLanguage.Korean:
                s_Language = "Korean";
                break;
            case SystemLanguage.Latvian:
                s_Language = "English";
                break;
            case SystemLanguage.Lithuanian:
                s_Language = "English";
                break;
            case SystemLanguage.Norwegian:
                s_Language = "English";
                break;
            case SystemLanguage.Polish:
                s_Language = "English";
                break;
            case SystemLanguage.Portuguese:
                s_Language = "English";
                break;
            case SystemLanguage.Romanian:
                s_Language = "English";
                break;
            case SystemLanguage.Russian:
                s_Language = "English";
                break;
            case SystemLanguage.SerboCroatian:
                s_Language = "English";
                break;
            case SystemLanguage.Slovak:
                s_Language = "English";
                break;
            case SystemLanguage.Slovenian:
                s_Language = "English";
                break;
            case SystemLanguage.Spanish:
                s_Language = "English";
                break;
            case SystemLanguage.Swedish:
                s_Language = "English";
                break;
            case SystemLanguage.Thai:
                s_Language = "English";
                break;
            case SystemLanguage.Turkish:
                s_Language = "English";
                break;
            case SystemLanguage.Ukrainian:
                s_Language = "English";
                break;
            case SystemLanguage.Vietnamese:
                s_Language = "Vietnam";
                break;
            case SystemLanguage.ChineseSimplified:
                s_Language = "Chinese";
                break;
            case SystemLanguage.ChineseTraditional:
                s_Language = "Chinese";
                break;
            case SystemLanguage.Unknown:
                s_Language = "English";
                break;
            default:
                break;
        }

        TitleImage(s_Language);

        if (s_Language == "Korean")
            i_LanguageValue = 0;
        else if (curCountry.Name != "Indian" && s_Language == "English")
            i_LanguageValue = 1;
        else if (s_Language == "Japanese")
            i_LanguageValue = 2;
        else if (s_Language == "Chinese")
            i_LanguageValue = 3;
        else if (curCountry.Name == "Indian")
            i_LanguageValue = 4;
        else if (curCountry.Name == "Vietnam")
            i_LanguageValue = 5;
        else if (curCountry.Name == "France")
            i_LanguageValue = 6;
        else if (curCountry.Name == "Germany")
            i_LanguageValue = 7;
        else if (curCountry.Name == "Italia")
            i_LanguageValue = 8;

        OnCheckImage(i_LanguageValue);

        Invoke("TranslateCoroutine", 0.25f);

        if (StageManager_Scr.instance != null)
        {
            StageManager_Scr.instance.StageNameChecker();
        }

        Invoke("unactive", 0.1f);
    }

    public void DetectCountry()
    {
        curCountry = RegionInfo.CurrentRegion;
        if (curCountry.Name == "IN")
            b_isIndia = true;
    }

    public void TitleImage(string str)
    {
        if (str == "Korean")
        {
            i_Title.sprite = s_Title[0];
        }
        else
            i_Title.sprite = s_Title[1];
    }

    void unactive()
    {
        for (int i = 0; i < 4; ++i)
        {
            g_BeforeButtons[i].transform.GetChild(1).gameObject.SetActive(false);
            g_AfterButtons[i].transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        AttendanceManager_Scr.instance.g_DontSeeObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        switch (s_Language)
        {
            case "Korean":
                g_BeforeButtons[0].transform.GetChild(1).gameObject.SetActive(true);
                g_AfterButtons[0].transform.GetChild(1).gameObject.SetActive(true);
                break;

            case "English":

                AttendanceManager_Scr.instance.g_DontSeeObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(100, 0, 0);
                g_BeforeButtons[1].transform.GetChild(1).gameObject.SetActive(true);
                g_AfterButtons[1].transform.GetChild(1).gameObject.SetActive(true);
                break;

            case "Japanese":

                g_BeforeButtons[2].transform.GetChild(1).gameObject.SetActive(true);
                g_AfterButtons[2].transform.GetChild(1).gameObject.SetActive(true);
                break;

            case "Chinese":

                g_BeforeButtons[3].transform.GetChild(1).gameObject.SetActive(true);
                g_AfterButtons[3].transform.GetChild(1).gameObject.SetActive(true);
                break;
        }
    }

    public void OnCheckImage(int _value)
    {
        if (Save_Load.instance.b_isLoad)
            SoundManager_sfx.instance.PlaySE("UI_Click1", false);

        i_LanguageValue = _value;
        g_BeforeDropDown.GetComponent<TMP_Dropdown>().value = i_LanguageValue;
        g_AfterDropDown.GetComponent<TMP_Dropdown>().value = i_LanguageValue;
    }

    public void OnClickDropDownMenu()
    {
        Invoke("DropDownMenuActive", 2f);
    }

    public void OnCheckImage(GameObject obj)
    {
        if(Save_Load.instance.b_isLoad)
            SoundManager_sfx.instance.PlaySE("UI_Click1", false);
        
        for (int i = 0; i < obj.transform.parent.childCount; ++i)
        {
            obj.transform.parent.GetChild(i).GetChild(1).gameObject.SetActive(false);
        }
        i_LanguageIdx = obj.transform.GetSiblingIndex();
    }

    public void OnTranslateLanguage(string _language)
    {
        s_Language = _language;
        TranslateCoroutine();
        TitleImage(s_Language);
        g_Lobby.TranslateUI();
    }

    public void ChangeLanguage(TMP_Dropdown box)
    {
        b_isIndia = false;
        OnCheckImage(box.value);
        switch (i_LanguageValue)
        {
            case 0:
                s_Language = "Korean";
                break;
            case 1:
                s_Language = "English";
                break;
            case 2:
                s_Language = "Japanese";
                break;
            case 3:
                s_Language = "Chinese";
                break;
            case 4:
                s_Language = "English";
                b_isIndia = true;
                break;
            case 5:
                s_Language = "Vietnam";
                break;
            case 6:
                s_Language = "France";
                break;
            case 7:
                s_Language = "Germany";
                break;
            case 8:
                s_Language = "Italia";
                break;
            default:
                break;
        }
        TranslateCoroutine();
        TitleImage(s_Language);
    }

    public void TranslateCoroutine()
    {
        StartCoroutine(TranslateLanguage());
    }

    // 모든 확인 버튼 6번으로
    IEnumerator TranslateLanguage()
    {
        #region UI 일시적으로 켜기

        if(ManPowerManager_Scr.instance != null)
        {

            RectTransform Pos = g_Lobby.G_ManPower.GetComponent<RectTransform>();
            Pos.anchoredPosition = new Vector2(3000, 0);
            g_Lobby.G_ManPower.SetActive(true);
            ManPowerManager_Scr.instance.Coroutine_Sort();

            Pos = g_Lobby.G_VendingMachine.GetComponent<RectTransform>();
            Pos.anchoredPosition = new Vector2(3000, 0);
            g_Lobby.G_VendingMachine.SetActive(true);

            Pos = g_Lobby.G_CoinShop.GetComponent<RectTransform>();
            Pos.anchoredPosition = new Vector2(3000, 0);
            g_Lobby.G_CoinShop.SetActive(true);

            Pos = g_Lobby.G_GemShop.GetComponent<RectTransform>();
            Pos.anchoredPosition = new Vector2(3000, 0);
            g_Lobby.G_GemShop.SetActive(true);

            Pos = g_Lobby.G_CharacterShop.GetComponent<RectTransform>();
            Pos.anchoredPosition = new Vector2(3000, 0);
            g_Lobby.G_CharacterShop.SetActive(true);

            Pos = g_Lobby.G_Shop.GetComponent<RectTransform>();
            Pos.anchoredPosition = new Vector2(3000, 0);
            g_Lobby.G_Shop.SetActive(true);

            Pos = g_Lobby.G_Skill.GetComponent<RectTransform>();
            Pos.anchoredPosition = new Vector2(3000, 0);
            g_Lobby.G_Skill.SetActive(true);

            Pos = g_Lobby.G_Mission.GetComponent<RectTransform>();
            Pos.anchoredPosition = new Vector2(3000, 0);
            g_Lobby.G_Mission.SetActive(true);

            #endregion

            myFont = UIFont.NanumBarunGothic;
            switch (s_Language)
            {
                case "Korean":
                    i_TMPNum = 0;
                    b_isKorean = true;
                    b_isEnglish = false;
                    b_isJapanese = false;
                    b_isChinese = false;
                    b_isIndia = false;
                    b_isVietnam = false;
                    b_isFrance = false;
                    b_isGermany = false;
                    b_isItalia = false;
                    break;

                case "English":
                    i_TMPNum = 0;
                    b_isKorean = false;
                    b_isEnglish = true;
                    b_isJapanese = false;
                    b_isChinese = false;
                    b_isIndia = false;
                    b_isVietnam = false;
                    b_isFrance = false;
                    b_isGermany = false;
                    b_isItalia = false;
                    break;

                case "Japanese":
                    i_TMPNum = 1;
                    b_isKorean = false;
                    b_isEnglish = false;
                    b_isJapanese = true;
                    b_isChinese = false;
                    b_isIndia = false;
                    b_isVietnam = false;
                    b_isFrance = false;
                    b_isGermany = false;
                    b_isItalia = false;
                    break;

                case "Chinese":
                    i_TMPNum = 2;
                    b_isKorean = false;
                    b_isEnglish = false;
                    b_isJapanese = false;
                    b_isChinese = true;
                    b_isIndia = false;
                    b_isVietnam = false;
                    b_isFrance = false;
                    b_isGermany = false;
                    b_isItalia = false;
                    break;

                case "India":
                    i_TMPNum = 0;
                    b_isKorean = false;
                    b_isEnglish = false;
                    b_isJapanese = false;
                    b_isChinese = false;
                    b_isIndia = true;
                    b_isVietnam = false;
                    b_isFrance = false;
                    b_isGermany = false;
                    b_isItalia = false;
                    break;

                case "Vietnam":
                    i_TMPNum = 0;
                    b_isKorean = false;
                    b_isEnglish = false;
                    b_isJapanese = false;
                    b_isChinese = false;
                    b_isIndia = false;
                    b_isVietnam = true;
                    b_isFrance = false;
                    b_isGermany = false;
                    b_isItalia = false;
                    break;

                case "France":
                    i_TMPNum = 0;
                    b_isKorean = false;
                    b_isEnglish = false;
                    b_isJapanese = false;
                    b_isChinese = false;
                    b_isIndia = false;
                    b_isVietnam = false;
                    b_isFrance = true;
                    b_isGermany = false;
                    b_isItalia = false;
                    break;

                case "Germany":
                    i_TMPNum = 0;
                    b_isKorean = false;
                    b_isEnglish = false;
                    b_isJapanese = false;
                    b_isChinese = false;
                    b_isIndia = false;
                    b_isVietnam = false;
                    b_isFrance = false;
                    b_isGermany = true;
                    b_isItalia = false;
                    break;

                case "Italia":
                    i_TMPNum = 0;
                    b_isKorean = false;
                    b_isEnglish = false;
                    b_isJapanese = false;
                    b_isChinese = false;
                    b_isIndia = false;
                    b_isVietnam = false;
                    b_isFrance = false;
                    b_isGermany = false;
                    b_isItalia = true;
                    break;
            }

            #region 폰트 변경
            ChangeFont(g_Lobby.g_RankButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>(), i_TMPNum);
            ChangeFont(ShopManager_Scr.instance.g_ShopUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>(), i_TMPNum);
            ChangeFont(g_Lobby.g_DownSideGroup.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>(), i_TMPNum);
            ChangeFont(g_Lobby.g_DownSideGroup.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>(), i_TMPNum);
            ChangeFont(g_Lobby.g_DownSideGroup.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>(), i_TMPNum);
            ChangeFont(g_Lobby.g_DownSideGroup.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>(), i_TMPNum);

            ChangeFont(MissionManager_Scr.instance.G_MissionList[3].GetComponent<MissionSlot_Scr>().t_Context, i_TMPNum);
            #endregion

            for (int i = 0; i < RespawnManager.instance.ActiveBlock.Count; i++)
            {
                if (b_isIndia)
                    RespawnManager.instance.ActiveBlock[i].transform.GetComponent<BlockControl>().setName(s_Language, "Indian");
                else
                    RespawnManager.instance.ActiveBlock[i].transform.GetComponent<BlockControl>().setName(s_Language, s_Language);
            }

            #region 확인버튼

            if (b_isEnglish || b_isIndia || b_isItalia || b_isGermany || b_isFrance || b_isVietnam)
            {
                TutorialManager.instance.go_TutorialRankGroup.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
                g_Lobby.G_CoinShop.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
                g_Lobby.G_GemShop.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
                g_Lobby.G_Skill.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
                g_Lobby.G_ManPower.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
                g_Lobby.G_VendingMachine.transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
                g_Lobby.G_Shop.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
                g_Lobby.G_CharacterShop.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
                g_Lobby.Before_Option.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
                g_Lobby.After_Pause.transform.GetChild(1).GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
                g_Lobby.G_Attendance.transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
                g_Lobby.G_Mission.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
                g_Lobby.G_Stage.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
                HeroInvenOK.text = TranslateData[6]["English"].ToString();
                OpenBoxOk.text = TranslateData[6]["English"].ToString();
                OpenItemOK.text = TranslateData[6]["English"].ToString();
                OpenGoldOK.text = TranslateData[6]["English"].ToString();
                OpenHeroOK.text = TranslateData[6]["English"].ToString();
                ManPowerManager_Scr.instance.t_sellCoin.transform.parent.GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
                SynthesisCaching_Scr.instance.Hint.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
                InventoryCaching_Scr.instance.OKButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
                InventoryCaching_Scr.instance.infoUI.transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
                synthesisEffect.g_itemInfo.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
                synthesisEffect.g_heroInfo.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
                InventoryCaching_Scr.instance.warninginventory.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
                InventoryCaching_Scr.instance.warninginventoryGem.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
                RewardManager_Scr.instance.G_Lose.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
                if (TutorialManager.instance.go_TutorialGroup != null)
                {
                    TutorialManager.instance.go_TutorialGroup.transform.GetChild(9).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
                }
            }
            else
            {
                TutorialManager.instance.go_TutorialRankGroup.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
                g_Lobby.G_CoinShop.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
                g_Lobby.G_GemShop.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
                g_Lobby.G_Skill.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
                g_Lobby.G_ManPower.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
                g_Lobby.G_VendingMachine.transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
                g_Lobby.G_Shop.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
                g_Lobby.G_CharacterShop.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
                g_Lobby.Before_Option.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
                g_Lobby.After_Pause.transform.GetChild(1).GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
                g_Lobby.G_Attendance.transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
                g_Lobby.G_Mission.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
                g_Lobby.G_Stage.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
                HeroInvenOK.text = TranslateData[6][s_Language].ToString();
                OpenBoxOk.text = TranslateData[6][s_Language].ToString();
                OpenItemOK.text = TranslateData[6][s_Language].ToString();
                OpenGoldOK.text = TranslateData[6][s_Language].ToString();
                OpenHeroOK.text = TranslateData[6][s_Language].ToString();
                ManPowerManager_Scr.instance.t_sellCoin.transform.parent.GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
                SynthesisCaching_Scr.instance.Hint.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
                InventoryCaching_Scr.instance.OKButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
                InventoryCaching_Scr.instance.infoUI.transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
                synthesisEffect.g_itemInfo.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
                synthesisEffect.g_heroInfo.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
                InventoryCaching_Scr.instance.warninginventory.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
                InventoryCaching_Scr.instance.warninginventoryGem.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
                RewardManager_Scr.instance.G_Lose.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
                if (TutorialManager.instance.go_TutorialGroup != null)
                {
                    TutorialManager.instance.go_TutorialGroup.transform.GetChild(9).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
                }

            }
            #endregion

            #region 튜토리얼 - 랭킹전
            if (b_isEnglish || b_isIndia || b_isItalia || b_isGermany || b_isFrance || b_isVietnam)
            {
                TutorialManager.instance.go_TutorialRankGroup.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = TranslateData[5]["English"].ToString();
            }
            else
            {
                TutorialManager.instance.go_TutorialRankGroup.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = TranslateData[5][s_Language].ToString();
            }
            TutorialManager.instance.go_TutorialRankGroup.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = TranslateData[144][s_Language] + "\n" + TranslateData[145][s_Language].ToString();
            TutorialManager.instance.go_TutorialRankGroup.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = TranslateData[146][s_Language] + "\n" + TranslateData[147][s_Language].ToString();
            #endregion

            #region 로비 버튼들
            if (b_isEnglish || b_isIndia || b_isItalia || b_isGermany || b_isFrance || b_isVietnam)
            {
                g_Lobby.G_Before.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = TranslateData[0]["English"].ToString();
                g_Lobby.G_Before.transform.GetChild(6).transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[1]["English"].ToString(); // 로비 위쪽 레이아웃 그룹의 랭킹
                g_Lobby.G_Before.transform.GetChild(6).transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[2]["English"].ToString();
                g_Lobby.G_Before.transform.GetChild(6).transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[3]["English"].ToString();
                g_Lobby.G_Before.transform.GetChild(6).transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[4]["English"].ToString();
                ChangeFont(g_Lobby.G_Before.transform.GetChild(6).transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>(), i_TMPNum);
                ChangeFont(g_Lobby.G_Before.transform.GetChild(6).transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>(), i_TMPNum);
                ChangeFont(g_Lobby.G_Before.transform.GetChild(6).transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>(), i_TMPNum);
                ChangeFont(g_Lobby.G_Before.transform.GetChild(6).transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>(), i_TMPNum);

                str = TranslateData[5]["English"].ToString();
                split = str.Split(' ');
                str = null;
                for (int i = 0; i < split.Length; ++i)
                {
                    str += split[i];
                    if (i != split.Length - 1)
                    {
                        str += "\n";
                    }
                }
                g_Lobby.g_RankButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = split[0] + "\n" + split[1];   // 로비의 랭킹전 버튼

                g_Lobby.g_DownSideGroup.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = TranslateData[8]["English"].ToString();    // 로비 아래쪽 레이아웃 그룹의 스킬
                g_Lobby.g_DownSideGroup.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[9]["English"].ToString();
                g_Lobby.g_DownSideGroup.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[10]["English"].ToString();
                ShopManager_Scr.instance.g_ShopUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[11]["English"].ToString();
            }
            else
            {
                g_Lobby.G_Before.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = TranslateData[0][s_Language].ToString();
                g_Lobby.G_Before.transform.GetChild(6).transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[1][s_Language].ToString(); // 로비 위쪽 레이아웃 그룹의 랭킹
                g_Lobby.G_Before.transform.GetChild(6).transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[2][s_Language].ToString();
                g_Lobby.G_Before.transform.GetChild(6).transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[3][s_Language].ToString();
                g_Lobby.G_Before.transform.GetChild(6).transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[4][s_Language].ToString();
                ChangeFont(g_Lobby.G_Before.transform.GetChild(6).transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>(), i_TMPNum);
                ChangeFont(g_Lobby.G_Before.transform.GetChild(6).transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>(), i_TMPNum);
                ChangeFont(g_Lobby.G_Before.transform.GetChild(6).transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>(), i_TMPNum);
                ChangeFont(g_Lobby.G_Before.transform.GetChild(6).transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>(), i_TMPNum);

                g_Lobby.g_RankButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[5][s_Language].ToString();   // 로비의 랭킹전 버튼

                g_Lobby.g_DownSideGroup.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = TranslateData[8][s_Language].ToString();    // 로비 아래쪽 레이아웃 그룹의 스킬
                g_Lobby.g_DownSideGroup.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[9][s_Language].ToString();
                g_Lobby.g_DownSideGroup.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[10][s_Language].ToString();
                ShopManager_Scr.instance.g_ShopUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[11][s_Language].ToString();
            }
            #endregion

            #region 코인, 보석 상점
            if (b_isKorean || b_isChinese || b_isJapanese)
            {
                g_Lobby.G_CoinShop.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[12][s_Language].ToString();    // 코인 충전 상점 제목
                g_Lobby.G_GemShop.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[13][s_Language].ToString();    // 젬 충전 상점 제목
            }
            else
            {
                g_Lobby.G_CoinShop.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[12]["English"].ToString();    // 코인 충전 상점 제목
                g_Lobby.G_GemShop.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[13]["English"].ToString();    // 젬 충전 상점 제목
            }
            #endregion

            #region 스킬창
            if (b_isKorean || b_isChinese || b_isJapanese)
                g_Lobby.G_Skill.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[8][s_Language].ToString();
            else
                g_Lobby.G_Skill.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[8]["English"].ToString();
            g_Lobby.G_Skill.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = TranslateData[175][s_Language].ToString();
            g_Lobby.G_Skill.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = TranslateData[162][s_Language].ToString();

            str = TranslateData[14][s_Language].ToString();
            split = str.Split('_');
            str = null;
            for (int i = 0; i < split.Length; ++i)
            {
                str += split[i];
                if (i != split.Length - 1)
                {
                    str += "\n";
                }
            }
            g_Lobby.G_Skill.transform.GetChild(2).GetChild(0).GetComponent<SkillStat_Scr>().s_SkillName = str;   // 스킬들 이름
            str = TranslateData[15][s_Language].ToString();
            split = str.Split('_');
            str = null;
            for (int i = 0; i < split.Length; ++i)
            {
                str += split[i];
                if (i != split.Length - 1)
                {
                    str += "\n";
                }
            }
            g_Lobby.G_Skill.transform.GetChild(2).GetChild(1).GetComponent<SkillStat_Scr>().s_SkillName = str;
            str = TranslateData[16][s_Language].ToString();
            split = str.Split('_');
            str = null;
            for (int i = 0; i < split.Length; ++i)
            {
                str += split[i];
                if (i != split.Length - 1)
                {
                    str += "\n";
                }
            }
            g_Lobby.G_Skill.transform.GetChild(2).GetChild(2).GetComponent<SkillStat_Scr>().s_SkillName = str;
            str = TranslateData[17][s_Language].ToString();
            split = str.Split('_');
            str = null;
            for (int i = 0; i < split.Length; ++i)
            {
                str += split[i];
                if (i != split.Length - 1)
                {
                    str += "\n";
                }
            }
            g_Lobby.G_Skill.transform.GetChild(2).GetChild(3).GetComponent<SkillStat_Scr>().s_SkillName = str;

            for (int i = 0; i < g_Lobby.g_SkillPopUpList.transform.childCount; ++i)
            {
                TextMeshProUGUI t_Context = g_Lobby.g_SkillPopUpList.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
                t_Context.text = TranslateContext(170 + i);
                if (t_Context.text.Length > 22)
                {
                    g_Lobby.g_SkillPopUpList.transform.GetChild(i).GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(500f, 150f);
                    g_Lobby.g_SkillPopUpList.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(480f, 100f);
                }
                else
                {
                    g_Lobby.g_SkillPopUpList.transform.GetChild(i).GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(500f, 100f);
                    g_Lobby.g_SkillPopUpList.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(480f, 50f);
                }
            }
            #endregion

            #region 동료창
            if (b_isKorean || b_isChinese || b_isJapanese)
                g_Lobby.G_ManPower.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[9][s_Language].ToString();     // 동료 총 공격력, 제목 버튼
            else
                g_Lobby.G_ManPower.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[9]["English"].ToString();
            g_Lobby.G_ManPower.transform.GetChild(6).GetComponent<AllMansAtk_Scr>().s_Atk = TranslateData[18][s_Language].ToString();

            ManPowerManager_Scr.instance.active_sellbutton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[228][s_Language].ToString();
            #endregion

            #region 자판기창
            if (b_isKorean || b_isChinese || b_isJapanese)
            {
                g_Lobby.G_VendingMachine.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[10][s_Language].ToString();  // 자판기 총 공격력, 제목 버튼
                g_Lobby.G_VendingMachine.transform.GetChild(8).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[139][s_Language].ToString();   // 자판기 구매 타이틀
                g_Lobby.G_VendingMachine.transform.GetChild(8).GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[23][s_Language].ToString();
                g_Lobby.G_VendingMachine.transform.GetChild(8).GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[35][s_Language].ToString();
            }
            else
            {
                g_Lobby.G_VendingMachine.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[10]["English"].ToString();
                g_Lobby.G_VendingMachine.transform.GetChild(8).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[139]["English"].ToString();
                g_Lobby.G_VendingMachine.transform.GetChild(8).GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[23]["English"].ToString();
                g_Lobby.G_VendingMachine.transform.GetChild(8).GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[35]["English"].ToString();
            }
            g_Lobby.G_VendingMachine.transform.GetChild(7).GetComponent<AllMachinesAtk_Scr>().s_Atk = TranslateData[18][s_Language].ToString();
            g_Lobby.G_VendingMachine.transform.GetChild(8).GetChild(4).GetComponent<TextMeshProUGUI>().text = TranslateData[138][s_Language].ToString();
            #endregion

            #region 캐릭터 구매 상점
            if (b_isKorean || b_isChinese || b_isJapanese)
            {
                g_Lobby.G_Shop.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[11][s_Language].ToString();    // 상점 제목 및 확인버튼
                g_Lobby.G_CharacterShop.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[37][s_Language].ToString();   // 캐릭터 상점 제목 및 확인버튼
            }
            else
            {
                g_Lobby.G_Shop.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[11]["English"].ToString();    // 상점 제목 및 확인버튼
                g_Lobby.G_CharacterShop.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[37]["English"].ToString();   // 캐릭터 상점 제목 및 확인버튼
            }

            // 이름 추가 리스트들의 각 직급(대리같은 직급)
            for (int i = 0; i < 4; ++i)
            {
                ShopManager_Scr.instance.s_MiddleNameList[i] = TranslateData[24 + i][s_Language].ToString();
                ShopManager_Scr.instance.s_TopNameList[i] = TranslateData[28 + i][s_Language].ToString();
            }

            Idx = 0;
            for (int i = 0; i < ChangeManager_Scr.instance.t_CharacterList.childCount * 2; i += 2)
            {
                CharChangerStat_Scr obj = ChangeManager_Scr.instance.t_CharacterList.GetChild(Idx++).GetComponent<CharChangerStat_Scr>();

                if (b_isEnglish || b_isIndia || b_isItalia || b_isGermany || b_isFrance || b_isVietnam)
                {
                    obj.g_Button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[23]["English"].ToString();
                    obj.g_BoughtButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[137]["English"].ToString();
                }
                else
                {
                    obj.g_Button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[23][s_Language].ToString();
                    obj.g_BoughtButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[137][s_Language].ToString();
                }

                if (b_isKorean || b_isJapanese || b_isChinese || b_isVietnam)
                    obj.s_Name = TranslateData[38 + i][s_Language].ToString();
                else
                    obj.s_Name = TranslateData[38 + i]["English"].ToString();
                obj.s_Ability = TranslateData[38 + i + 1][s_Language].ToString();
            }
            #endregion

            #region 옵션창
            if (b_isEnglish || b_isIndia || b_isItalia || b_isGermany || b_isFrance || b_isVietnam)
            {
                g_Lobby.Before_Option.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[50]["English"].ToString();
                g_Lobby.Before_Option.transform.GetChild(1).GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>().text = TranslateData[51]["English"].ToString();
                g_Lobby.Before_Option.transform.GetChild(1).GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = TranslateData[52]["English"].ToString();
                g_Lobby.Before_Option.transform.GetChild(1).GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[53]["English"].ToString();

                g_Lobby.After_Pause.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[50]["English"].ToString();
                g_Lobby.After_Pause.transform.GetChild(1).GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().text = TranslateData[51]["English"].ToString();
                g_Lobby.After_Pause.transform.GetChild(1).GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>().text = TranslateData[52]["English"].ToString();
                g_Lobby.After_Pause.transform.GetChild(1).GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[53]["English"].ToString();

                g_Lobby.G_Home.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[67]["English"].ToString();
                g_Lobby.G_Home.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[68]["English"].ToString();
            }
            else
            {
                g_Lobby.Before_Option.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[50][s_Language].ToString();
                g_Lobby.Before_Option.transform.GetChild(1).GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>().text = TranslateData[51][s_Language].ToString();
                g_Lobby.Before_Option.transform.GetChild(1).GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = TranslateData[52][s_Language].ToString();
                g_Lobby.Before_Option.transform.GetChild(1).GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[53][s_Language].ToString();

                g_Lobby.After_Pause.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[50][s_Language].ToString();
                g_Lobby.After_Pause.transform.GetChild(1).GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().text = TranslateData[51][s_Language].ToString();
                g_Lobby.After_Pause.transform.GetChild(1).GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>().text = TranslateData[52][s_Language].ToString();
                g_Lobby.After_Pause.transform.GetChild(1).GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[53][s_Language].ToString();

                g_Lobby.G_Home.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[67][s_Language].ToString();
                g_Lobby.G_Home.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[68][s_Language].ToString();
            }
            g_Lobby.G_Home.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[57][s_Language].ToString();
            #endregion

            #region 결과창
            if (b_isKorean || b_isChinese || b_isJapanese)
                str = TranslateData[62][s_Language].ToString();
            else
                str = TranslateData[62]["English"].ToString();
            split = str.Split(' ');
            str = null;
            for (int i = 0; i < split.Length; ++i)
            {
                str += split[i];
                if (i != split.Length - 1)
                {
                    str += "\n";
                }
            }
            RewardManager_Scr.instance.g_CurLevelClear.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = str;
            #endregion

            #region 레벨업창
            if (b_isKorean || b_isChinese || b_isJapanese)
            {
                g_Lobby.G_LevelUp.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[63][s_Language].ToString();
                g_Lobby.G_LevelUp.transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
            }
            else
            {
                g_Lobby.G_LevelUp.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[63]["English"].ToString();
                g_Lobby.G_LevelUp.transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[6]["English"].ToString();
            }
            #endregion

            #region 게임종료창
            if (b_isKorean || b_isChinese || b_isJapanese)
            {
                QuitManager_Scr.instance.g_QuitPopUpMenu.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[65][s_Language].ToString();
                QuitManager_Scr.instance.g_QuitPopUpMenu.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[67][s_Language].ToString();
                QuitManager_Scr.instance.g_QuitPopUpMenu.transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[68][s_Language].ToString();
            }
            else
            {
                QuitManager_Scr.instance.g_QuitPopUpMenu.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[65]["English"].ToString();
                QuitManager_Scr.instance.g_QuitPopUpMenu.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[67]["English"].ToString();
                QuitManager_Scr.instance.g_QuitPopUpMenu.transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[68]["English"].ToString();
            }
            QuitManager_Scr.instance.g_QuitPopUpMenu.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[66][s_Language].ToString();
            #endregion

            #region 출석창
            if (b_isKorean || b_isChinese || b_isJapanese)
                g_Lobby.G_Attendance.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[100][s_Language].ToString();
            else
                g_Lobby.G_Attendance.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[100]["English"].ToString();
            g_Lobby.G_Attendance.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[103][s_Language].ToString();

            for (int i = 0; i < AttendanceManager_Scr.instance.t_SlotParent.childCount; ++i)
            {
                GameObject obj = AttendanceManager_Scr.instance.t_SlotParent.GetChild(i).gameObject;
                if (b_isKorean || b_isChinese || b_isJapanese)
                    obj.GetComponent<AttendanceSlot_Scr>().g_GetButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateContext(106);
                else
                    obj.GetComponent<AttendanceSlot_Scr>().g_GetButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateContext(106, "English");
            }
            #endregion

            #region 미션창
            if (b_isKorean || b_isChinese || b_isJapanese)
                g_Lobby.G_Mission.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[104][s_Language].ToString();
            else
                g_Lobby.G_Mission.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[104]["English"].ToString();

            for (int i = 0; i < MissionManager_Scr.instance.G_MissionList.Length - 1; ++i)
            {
                MissionSlot_Scr slot = MissionManager_Scr.instance.G_MissionList[i].GetComponent<MissionSlot_Scr>();
                if (b_isKorean || b_isChinese || b_isJapanese)
                    slot.g_Clear.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateContext(174);
                else
                    slot.g_Clear.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateContext(174, "English");
                switch (MissionManager_Scr.instance.i_MissionSlot[i])
                {
                    case 0:
                        slot.t_Context.text = TranslateContext(109 + (4 * i));
                        break;
                    case 1:
                        slot.t_Context.text = TranslateContext(110 + (4 * i));
                        break;
                    case 2:
                        slot.t_Context.text = TranslateContext(111 + (4 * i));
                        break;
                    case 3:
                        slot.t_Context.text = TranslateContext(112 + (4 * i));
                        break;
                    default:
                        break;
                }

                ChangeFont(slot.t_Context, i_TMPNum);
            }
            MissionManager_Scr.instance.G_MissionList[3].GetComponent<MissionSlot_Scr>().t_Context.text = TranslateContext(174);
            if (b_isKorean || b_isChinese || b_isJapanese)
                MissionManager_Scr.instance.G_MissionList[3].GetComponent<MissionSlot_Scr>().g_Clear.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateContext(174);
            else
                MissionManager_Scr.instance.G_MissionList[3].GetComponent<MissionSlot_Scr>().g_Clear.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateContext(174, "English");


            #endregion

            #region 튜토리얼 아이콘
            if (TutorialManager.instance.i_TutorialCount < 8)
            {
                g_Lobby.G_After.transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[8][s_Language].ToString();
                g_Lobby.G_After.transform.GetChild(10).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[9][s_Language].ToString();
                g_Lobby.G_After.transform.GetChild(10).GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[10][s_Language].ToString();
                g_Lobby.G_After.transform.GetChild(10).GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[11][s_Language].ToString();
            }
            #endregion

            #region 말풍선

            RespawnManager.instance.ShuffleScriptList();
            RespawnManager.instance.i_SpeechCount = 0;

            for (int i = 0; i < RespawnManager.instance.go_GroupSpeechBubble.transform.childCount; i++)
            {
                if (i != 4)
                {
                    SpeechBubble script = RespawnManager.instance.go_GroupSpeechBubble.transform.GetChild(i).transform.GetComponent<SpeechBubble>();
                    if (!Button_Option.instance.b_WindowOn)
                    {
                        script.Delay();
                        script.f_Changetime = 0.2f;
                        script.b_ChangedSize = false;
                    }
                    else
                    {
                        script.DelayStart();
                        script.f_Changetime = 0.2f;
                        script.b_ChangedSize = false;
                    }
                }
                else
                {
                    for (int j = 0; j < RespawnManager.instance.go_GroupSpeechBubble.transform.GetChild(j).childCount; j++)
                    {
                        SpeechBubble script = RespawnManager.instance.go_GroupSpeechBubble.transform.GetChild(i).GetChild(j).GetComponent<SpeechBubble>();
                        if (!Button_Option.instance.b_WindowOn)
                        {
                            script.Delay();
                            script.f_Changetime = 0.2f;
                            script.b_ChangedSize = false;
                        }
                        else
                        {
                            script.b_isSpeech = false;
                            script.DelayStart();
                            script.f_Changetime = 0.2f;
                            script.b_ChangedSize = false;
                        }
                    }
                }
            }

            for (int i = 0; i < RespawnManager.instance.go_BulldogSpeechBubble.transform.childCount; i++)
            {
                SpeechBubble script = RespawnManager.instance.go_BulldogSpeechBubble.transform.GetChild(i).GetComponent<SpeechBubble>();
                if (!Button_Option.instance.b_WindowOn)
                {
                    script.Delay();
                    script.f_Changetime = 0.2f;
                    script.b_ChangedSize = false;
                }
                else
                {
                    script.b_isSpeech = false;
                    script.DelayStart();
                    script.f_Changetime = 0.2f;
                    script.b_ChangedSize = false;
                }
            }
            #endregion

            // 가방, 상자 업그레이드 csv 추가
            #region 이름 변경 상점

            ShopManager_Scr.instance.t_Shop.GetChild(0).transform.GetComponent<ProductStat_Scr>().t_Context.text = TranslateData[158][s_Language].ToString();

            ShopManager_Scr.instance.t_Shop.GetChild(1).transform.GetComponent<ProductStat_Scr>().t_Name.text = "가방 업그레이드2";
            ShopManager_Scr.instance.t_Shop.GetChild(1).transform.GetComponent<ProductStat_Scr>().t_Context.text = "가방10칸+";

            ShopManager_Scr.instance.t_Shop.GetChild(2).transform.GetComponent<ProductStat_Scr>().t_Name.text = "가방 업그레이드3";
            ShopManager_Scr.instance.t_Shop.GetChild(2).transform.GetComponent<ProductStat_Scr>().t_Context.text = "가방10칸+";

            ShopManager_Scr.instance.t_Shop.GetChild(3).transform.GetComponent<ProductStat_Scr>().t_Name.text = "가방 업그레이드4";
            ShopManager_Scr.instance.t_Shop.GetChild(3).transform.GetComponent<ProductStat_Scr>().t_Context.text = "가방10칸+";

            ShopManager_Scr.instance.t_Shop.GetChild(4).transform.GetComponent<ProductStat_Scr>().t_Name.text = "가방 업그레이드5";
            ShopManager_Scr.instance.t_Shop.GetChild(4).transform.GetComponent<ProductStat_Scr>().t_Context.text = "가방10칸+";

            ShopManager_Scr.instance.t_Shop.GetChild(5).transform.GetComponent<ProductStat_Scr>().t_Name.text = "가방 업그레이드6";
            ShopManager_Scr.instance.t_Shop.GetChild(5).transform.GetComponent<ProductStat_Scr>().t_Context.text = "가방10칸+";

            ShopManager_Scr.instance.t_Shop.GetChild(6).transform.GetComponent<ProductStat_Scr>().t_Name.text = "가방 업그레이드7";
            ShopManager_Scr.instance.t_Shop.GetChild(6).transform.GetComponent<ProductStat_Scr>().t_Context.text = "가방10칸+";

            ShopManager_Scr.instance.t_Shop.GetChild(7).transform.GetComponent<ProductStat_Scr>().t_Name.text = "상자 업그레이드2";
            ShopManager_Scr.instance.t_Shop.GetChild(7).transform.GetComponent<ProductStat_Scr>().t_Context.text = "티어획득확률+";

            ShopManager_Scr.instance.t_Shop.GetChild(8).transform.GetComponent<ProductStat_Scr>().t_Name.text = "상자 업그레이드3";
            ShopManager_Scr.instance.t_Shop.GetChild(8).transform.GetComponent<ProductStat_Scr>().t_Context.text = "티어획득확률+";

            ShopManager_Scr.instance.t_Shop.GetChild(9).transform.GetComponent<ProductStat_Scr>().t_Name.text = "무기 업그레이드";
            ShopManager_Scr.instance.t_Shop.GetChild(9).transform.GetComponent<ProductStat_Scr>().t_Context.text = "다음 무기로 업그레이드";

            ShopManager_Scr.instance.t_Shop.GetChild(10).transform.GetComponent<ProductStat_Scr>().t_Name.text = "히어로 보유량 업그레이드";
            ShopManager_Scr.instance.t_Shop.GetChild(10).transform.GetComponent<ProductStat_Scr>().t_Context.text = "히어로 보유 가능수 +10";

            ShopManager_Scr.instance.t_Shop.GetChild(11).transform.GetComponent<ProductStat_Scr>().t_Name.text = "가늠자 업그레이드";
            ShopManager_Scr.instance.t_Shop.GetChild(11).transform.GetComponent<ProductStat_Scr>().t_Context.text = "2단 가늠자로 교체";

            ShopManager_Scr.instance.t_Shop.GetChild(12).transform.GetComponent<ProductStat_Scr>().t_Name.text = "초급 패키지";
            ShopManager_Scr.instance.t_Shop.GetChild(12).transform.GetComponent<ProductStat_Scr>().t_Context.text = "1회 구매가능";

            ShopManager_Scr.instance.t_Shop.GetChild(13).transform.GetComponent<ProductStat_Scr>().t_Name.text = "중급 패키지";
            ShopManager_Scr.instance.t_Shop.GetChild(13).transform.GetComponent<ProductStat_Scr>().t_Context.text = "1회 구매가능";

            ShopManager_Scr.instance.t_Shop.GetChild(14).transform.GetComponent<ProductStat_Scr>().t_Name.text = "고급 패키지";
            ShopManager_Scr.instance.t_Shop.GetChild(14).transform.GetComponent<ProductStat_Scr>().t_Context.text = "1회 구매가능";

            ShopManager_Scr.instance.t_Shop.GetChild(15).transform.GetComponent<ProductStat_Scr>().t_Name.text = "랭킹전 입장권";
            ShopManager_Scr.instance.t_Shop.GetChild(15).transform.GetComponent<ProductStat_Scr>().t_Context.text = "입장권10장추가";

            ShopManager_Scr.instance.t_Shop.GetChild(16).transform.GetComponent<ProductStat_Scr>().t_Name.text = "보스전 입장권";
            ShopManager_Scr.instance.t_Shop.GetChild(16).transform.GetComponent<ProductStat_Scr>().t_Context.text = "입장권10장추가";

            ShopManager_Scr.instance.t_Shop.GetChild(17).transform.GetComponent<ProductStat_Scr>().t_Name.text = "스파이 입장권";
            ShopManager_Scr.instance.t_Shop.GetChild(17).transform.GetComponent<ProductStat_Scr>().t_Context.text = "입장권10장추가";

            ShopManager_Scr.instance.t_Shop.GetChild(18).transform.GetComponent<ProductStat_Scr>().t_Name.text = "카오스 입장권";
            ShopManager_Scr.instance.t_Shop.GetChild(18).transform.GetComponent<ProductStat_Scr>().t_Context.text = "입장권10장추가";

            ShopManager_Scr.instance.t_Shop.GetChild(19).transform.GetComponent<ProductStat_Scr>().t_Name.text = "동료 영입권";
            ShopManager_Scr.instance.t_Shop.GetChild(19).transform.GetComponent<ProductStat_Scr>().t_Context.text = "영입권3장추가";

            ShopManager_Scr.instance.t_Shop.GetChild(20).transform.GetComponent<ProductStat_Scr>().t_Name.text = "택배상자 이용권";
            ShopManager_Scr.instance.t_Shop.GetChild(20).transform.GetComponent<ProductStat_Scr>().t_Context.text = "이용권10장추가";

            ShopManager_Scr.instance.t_Shop.GetChild(21).transform.GetComponent<ProductStat_Scr>().t_Name.text = "재료 꾸러미";
            ShopManager_Scr.instance.t_Shop.GetChild(21).transform.GetComponent<ProductStat_Scr>().t_Context.text = "랜덤재료10개";

            // 이름 바꾸는 상점
            if (b_isKorean || b_isChinese || b_isJapanese)
            {
                ShopManager_Scr.instance.g_ChangeWindow.transform.GetChild(5).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateContext(34);
                ShopManager_Scr.instance.g_ChangeWindow.transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateContext(35);
                ShopManager_Scr.instance.g_ChangeWindow.transform.GetChild(4).GetComponent<TMP_InputField>().placeholder.GetComponent<TextMeshProUGUI>().text = TranslateContext(36);
            }
            else
            {
                ShopManager_Scr.instance.g_ChangeWindow.transform.GetChild(5).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateContext(34, "English");
                ShopManager_Scr.instance.g_ChangeWindow.transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateContext(35, "English");
                ShopManager_Scr.instance.g_ChangeWindow.transform.GetChild(4).GetComponent<TMP_InputField>().placeholder.GetComponent<TextMeshProUGUI>().text = TranslateContext(35, "English");
            }


            for (int i = 0; i < ShopManager_Scr.instance.g_MiddleNames.Length; ++i)
            {
                ShopManager_Scr.instance.g_MiddleNames[i].GetComponent<ProductStat_Scr>().i_JobNum = i;
                ShopManager_Scr.instance.g_MiddleNames[i].GetComponent<ProductStat_Scr>().b_isMiddle = true;
                ShopManager_Scr.instance.g_MiddleNames[i].GetComponent<ProductStat_Scr>().b_isTop = false;

                for (int j = 0; j < 4; ++j)
                {
                    ShopManager_Scr.instance.s_MiddleNameList[j] = TranslateContext(24 + j);
                    ShopManager_Scr.instance.g_MiddleNames[i].GetComponent<ProductStat_Scr>().t_Name.text = ShopManager_Scr.instance.s_MiddleNameList[i];
                    ShopManager_Scr.instance.g_MiddleNames[i].GetComponent<ProductStat_Scr>().t_Context.text = TranslateContext(32);

                    if (b_isKorean || b_isEnglish
                        || b_isIndia || b_isVietnam
                        || b_isFrance || b_isGermany
                        || b_isItalia)
                    {
                        ChangeFont(ShopManager_Scr.instance.g_MiddleNames[i].GetComponent<ProductStat_Scr>().t_Context, 0);
                    }
                    else if (b_isJapanese)
                    {
                        ChangeFont(ShopManager_Scr.instance.g_MiddleNames[i].GetComponent<ProductStat_Scr>().t_Context, 1);
                    }
                    else if (b_isChinese)
                    {
                        ChangeFont(ShopManager_Scr.instance.g_MiddleNames[i].GetComponent<ProductStat_Scr>().t_Context, 2);
                    }
                }
                ShopManager_Scr.instance.g_MiddleNames[i].GetComponent<ProductStat_Scr>().i_Cash = (int)ShopManager_Scr.instance.ShopData[14 + i]["Buy"];
            }
            int k = 0;
            for (int i = 0; i < ShopManager_Scr.instance.g_TopNames.Length; ++i)
            {
                ShopManager_Scr.instance.g_TopNames[i].GetComponent<ProductStat_Scr>().i_JobNum = k++;
                ShopManager_Scr.instance.g_TopNames[i].GetComponent<ProductStat_Scr>().b_isMiddle = false;
                ShopManager_Scr.instance.g_TopNames[i].GetComponent<ProductStat_Scr>().b_isTop = true;

                for (int j = 0; j < 4; ++j)
                {
                    ShopManager_Scr.instance.s_TopNameList[j] = TranslateContext(28 + j);
                    ShopManager_Scr.instance.g_TopNames[i].GetComponent<ProductStat_Scr>().t_Name.text = ShopManager_Scr.instance.s_TopNameList[i];
                    ShopManager_Scr.instance.g_TopNames[i].GetComponent<ProductStat_Scr>().t_Context.text = TranslateContext(32);
                    if (b_isKorean || b_isEnglish
                        || b_isIndia || b_isVietnam
                        || b_isFrance || b_isGermany
                        || b_isItalia)
                    {
                        ChangeFont(ShopManager_Scr.instance.g_TopNames[i].GetComponent<ProductStat_Scr>().t_Name, 0);
                    }
                    else if (b_isJapanese)
                    {
                        ChangeFont(ShopManager_Scr.instance.g_TopNames[i].GetComponent<ProductStat_Scr>().t_Name, 1);
                    }
                    else if (b_isChinese)
                    {
                        ChangeFont(ShopManager_Scr.instance.g_TopNames[i].GetComponent<ProductStat_Scr>().t_Name, 2);
                    }
                }

                ShopManager_Scr.instance.g_TopNames[i].GetComponent<ProductStat_Scr>().i_Cash = (int)ShopManager_Scr.instance.ShopData[18 + i]["Buy"];
            }

            #endregion

            #region 스테이지 선택

            g_Lobby.G_Stage.transform.GetChild(5).GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[160][s_Language].ToString();

            if (b_isKorean || b_isChinese || b_isJapanese)
            {
                g_Lobby.G_Stage.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[159][s_Language].ToString();
                g_Lobby.G_Stage.transform.GetChild(5).GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[67][s_Language].ToString();
                g_Lobby.G_Stage.transform.GetChild(5).GetChild(0).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[68][s_Language].ToString();
            }
            else
            {
                g_Lobby.G_Stage.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[159]["English"].ToString();
                g_Lobby.G_Stage.transform.GetChild(5).GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[67]["English"].ToString();
                g_Lobby.G_Stage.transform.GetChild(5).GetChild(0).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[68]["English"].ToString();
            }
            #endregion

            #region 스테이지 결과창
            // 현 스테이지 클리어
            if (b_isKorean || b_isChinese || b_isJapanese)
                str = TranslateContext(62);
            else
                str = TranslateContext(62, "English");
            string[] strs = str.Split(' ');
            if (strs.Length > 1)
            {
                str = strs[0] + "\n" + strs[1];
            }
            else
                str = strs[0];
            RewardManager_Scr.instance.G_Win.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = str;
            if (b_isKorean || b_isChinese || b_isJapanese)
            {
                RewardManager_Scr.instance.G_Win.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[161][s_Language].ToString();
                RewardManager_Scr.instance.G_Win.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[2][s_Language].ToString();

                // 로비
                RewardManager_Scr.instance.G_Win.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[161][s_Language].ToString();
                // 공유
                RewardManager_Scr.instance.G_Win.transform.GetChild(0).GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[2][s_Language].ToString();

                // 랭킹
                RewardManager_Scr.instance.G_Lose.transform.GetChild(1).GetChild(0).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[1][s_Language].ToString();
                // 공유
                RewardManager_Scr.instance.G_Lose.transform.GetChild(1).GetChild(0).GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[2][s_Language].ToString();
            }
            else
            {
                RewardManager_Scr.instance.G_Win.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[161]["English"].ToString();
                RewardManager_Scr.instance.G_Win.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[2]["English"].ToString();

                // 로비
                RewardManager_Scr.instance.G_Win.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[161]["English"].ToString();
                // 공유
                RewardManager_Scr.instance.G_Win.transform.GetChild(0).GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[2]["English"].ToString();

                // 랭킹
                RewardManager_Scr.instance.G_Lose.transform.GetChild(1).GetChild(0).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[1]["English"].ToString();
                // 공유
                RewardManager_Scr.instance.G_Lose.transform.GetChild(1).GetChild(0).GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = TranslateData[2]["English"].ToString();
            }
            #endregion

            #region 버전체크 창(업데이트)
            if (newVersionAvailable != null)
            {
                if (b_isKorean || b_isChinese || b_isJapanese)
                {
                    newVersionAvailable.transform.GetChild(2).GetChild(0).GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = TranslateData[168][s_Language].ToString() + "\n" + TranslateData[169][s_Language].ToString();
                    newVersionAvailable.transform.GetChild(2).GetChild(1).GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = TranslateData[167][s_Language].ToString();
                    newVersionAvailable.transform.GetChild(2).GetChild(2).GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = TranslateData[166][s_Language].ToString();
                    newVersionAvailable.transform.GetChild(2).GetChild(3).GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = TranslateData[53][s_Language].ToString();
                }
                else
                {
                    newVersionAvailable.transform.GetChild(2).GetChild(0).GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = TranslateData[168]["English"].ToString() + "\n" + TranslateData[169]["English"].ToString();
                    newVersionAvailable.transform.GetChild(2).GetChild(1).GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = TranslateData[167]["English"].ToString();
                    newVersionAvailable.transform.GetChild(2).GetChild(2).GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = TranslateData[166]["English"].ToString();
                    newVersionAvailable.transform.GetChild(2).GetChild(3).GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = TranslateData[53]["English"].ToString();
                }
            }
            #endregion

            #region 리뷰권유창
            if (b_isKorean || b_isChinese || b_isJapanese)
            {
                g_Review[1].text = TranslateData[176][s_Language].ToString();
                g_Review[2].text = TranslateData[67][s_Language].ToString();
                g_Review[3].text = TranslateData[68][s_Language].ToString();
            }
            else
            {
                g_Review[1].text = TranslateData[176]["English"].ToString();
                g_Review[2].text = TranslateData[67]["English"].ToString();
                g_Review[3].text = TranslateData[68]["English"].ToString();
            }

            if (b_isJapanese)
            {
                string _str = TranslateData[177][s_Language].ToString();
                string[] _strs = _str.Split('ら');
                if (_strs.Length > 1)
                {
                    _str = _strs[0] + "ら" + "\n" + _strs[1];
                }
                else
                    _str = _strs[0];

                g_Review[0].text = _str;
            }
            else
            {
                /*
                string _str = TranslateData[177][s_Language].ToString();
                string[] _strs = _str.Split(',');
                if (_strs.Length > 1)
                {
                    _str = _strs[0] + "," + "\n" + _strs[1];
                }
                else
                    _str = _strs[0];
                g_Review[0].text = _str;
                */


                string str1 = TranslateData[177][s_Language].ToString();
                string str4 = str1.Replace("_", "\n");


                g_Review[0].text = str4;
            }


            #endregion

            #region 프리미어모드 보유 체크
            if (b_isEnglish || b_isIndia || b_isItalia || b_isGermany || b_isFrance || b_isVietnam)
            {
                t_ADPremium.text = TranslateData[137]["English"].ToString();
            }
            else
                t_ADPremium.text = TranslateData[137][s_Language].ToString();

            ShopManager_Scr.instance.ChangeInputField();
            #endregion

            #region 택배깡

            InventoryCaching_Scr.instance.warninginventory.transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>().text = TranslateData[187][s_Language].ToString();
            InventoryCaching_Scr.instance.warninginventory.transform.GetChild(2).transform.GetComponent<TextMeshProUGUI>().text = TranslateData[188][s_Language].ToString();
            DeliveryBoxManager_Scr.instance.openningBox.transform.GetChild(2).GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = TranslateData[181][s_Language].ToString();
            InventoryCaching_Scr.instance.warninginventoryGem.transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>().text = TranslateData[61][s_Language].ToString();

            #endregion

            #region 합성창
            SynthesisCaching_Scr.instance.Okbutton.transform.transform.GetComponent<TextMeshProUGUI>().text = TranslateData[6][s_Language].ToString();
            SynthesisCaching_Scr.instance.Title.transform.GetComponent<TextMeshProUGUI>().text = TranslateData[192][s_Language].ToString();
            SynthesisCaching_Scr.instance.Explanation.transform.GetComponent<TextMeshProUGUI>().text = TranslateData[193][s_Language].ToString();
            SynthesisCaching_Scr.instance.synthesis_t.transform.GetComponent<TextMeshProUGUI>().text = TranslateData[192][s_Language].ToString();
            SynthesisCaching_Scr.instance.heroTab_t.transform.GetComponent<TextMeshProUGUI>().text = TranslateData[194][s_Language].ToString();
            SynthesisCaching_Scr.instance.itemTab_t.transform.GetComponent<TextMeshProUGUI>().text = TranslateData[195][s_Language].ToString();
            SynthesisCaching_Scr.instance.synthesisButton_t.transform.GetComponent<TextMeshProUGUI>().text = TranslateData[196][s_Language].ToString();


            // 번역 추가
            SynthesisCaching_Scr.instance.Hint.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "합성";
            ChangeFont(SynthesisCaching_Scr.instance.Hint.transform.GetChild(1).GetComponent<TextMeshProUGUI>(), i_TMPNum);
            SynthesisCaching_Scr.instance.Hint.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "1티어 6개 = 2티어\n2티어 7개 = 3티어\n3티어 8개 = 4티어\n4티어 9개 = 5티어\n5티어 10개 = 6티어";
            ChangeFont(SynthesisCaching_Scr.instance.Hint.transform.GetChild(2).GetComponent<TextMeshProUGUI>(), i_TMPNum);

            #endregion

            #region 상자깡, 합성창, 가방창 튜토리얼 문구
            TutorialManager.instance.tmp_BoxTutorial_Title.text = TranslateData[223][s_Language].ToString();
            TutorialManager.instance.tmp_BoxTutorial_Contents.text = TranslateData[224][s_Language].ToString()
                                                                    + "\n" + TranslateData[225][s_Language].ToString()
            + "\n" + TranslateData[226][s_Language].ToString()
            + "\n" + TranslateData[227][s_Language].ToString()
            + "\n" + TranslateData[229][s_Language].ToString()
            + "\n" + TranslateData[230][s_Language].ToString()
            + "\n" + TranslateData[236][s_Language].ToString()
            + "\n" + TranslateData[237][s_Language].ToString()
            + "\n" + TranslateData[238][s_Language].ToString();

            TutorialManager.instance.tmp_BoxTutorial_DontseeText.text = TranslateData[210][s_Language].ToString();
            TutorialManager.instance.tmp_BoxTutorial_Confirm.text = TranslateData[6][s_Language].ToString();

            TutorialManager.instance.tmp_BoxTutorial_HeroName.text = Bag_Hero_ItmeCSV.instance.HeroData[1][s_Language+"_Name"].ToString();



            TutorialManager.instance.tmp_BagTutorial_Title.text = TranslateData[217][s_Language].ToString();
            TutorialManager.instance.tmp_BagTutorial_Contents.text = TranslateData[218][s_Language].ToString()
                                                                    + "\n" + TranslateData[219][s_Language].ToString()
            + "\n" + TranslateData[220][s_Language].ToString()
            + "\n" + TranslateData[221][s_Language].ToString()
            + "\n" + TranslateData[222][s_Language].ToString()
            + "\n" + TranslateData[240][s_Language].ToString()
            + "\n" + TranslateData[241][s_Language].ToString();

            TutorialManager.instance.tmp_BagTutorial_DontseeText.text = TranslateData[210][s_Language].ToString();
            TutorialManager.instance.tmp_BagTutorial_Confirm.text = TranslateData[6][s_Language].ToString(); 
            TutorialManager.instance.tmp_BoxTutorial_FakeTitle.text = Bag_Hero_ItmeCSV.instance.BagData[0][s_Language].ToString(); 



            TutorialManager.instance.tmp_SynthesisTutorial_Title.text = TranslateData[211][s_Language].ToString();
            TutorialManager.instance.tmp_SynthesisTutorial_Contents.text = TranslateData[212][s_Language].ToString()
                                                                    + "\n" + TranslateData[213][s_Language].ToString()
            + "\n" + TranslateData[214][s_Language].ToString()
            + "\n" + TranslateData[215][s_Language].ToString()
            + "\n" + TranslateData[216][s_Language].ToString()
            + "\n" + TranslateData[231][s_Language].ToString()
            + "\n" + TranslateData[239][s_Language].ToString();

            TutorialManager.instance.tmp_SynthesisTutorial_DontseeText.text = TranslateData[210][s_Language].ToString();
            TutorialManager.instance.tmp_SynthesisTutorial_Confirm.text = TranslateData[6][s_Language].ToString();


            #endregion



            InventoryTranslate();

            TutorialManager.instance.InitTutorial();
            ShopManager_Scr.instance.InitShop();

            Button_Option.instance.TranslateUI();

        }
        yield return null;
    }

    public void InventoryTranslate()
    {
        g_Lobby.g_DownSideGroup.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "item";
        InventoryCaching_Scr.instance.Title.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Bag_Hero_ItmeCSV.instance.BagData[Inventory_Scr.instance.InventoryLevel - 1][s_Language].ToString();
    }

    public string TranslateContext(int _Num)
    {
        // 74부터 + 2
        return TranslateData[_Num][s_Language].ToString();
    }
    public string TranslateContext(int _Num, string str)
    {
        // 74부터 + 2
        return TranslateData[_Num][str].ToString();
    }

    public void ChangeFont(Text text)
    {
        text.font = f_Font[(int)myFont];
    }

    public void ChangeFont(TextMeshProUGUI text, int num)
    {
        text.font = f_TMPFont[num];
    }

    public void ChangeFontMaterial(TextMeshProUGUI text, int num)
    {
        text.fontMaterial = f_TMPMaterial[num];
    }

    public void ChangeFont(Text text, int num)
    {
        text.font = f_Font[num];
    }

    public class Country
    {
        public string businessName;
        public string businessWebsite;
        public string city;
        public string continent;
        public string country;
        public string countryCode;
        public string ipName;
        public string ipType;
        public string isp;
        public string lat;
        public string lon;
        public string org;
        public string query;
        public string region;
        public string status;
    }
}