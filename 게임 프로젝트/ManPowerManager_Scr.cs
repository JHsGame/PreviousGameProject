using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManPowerManager_Scr : MonoBehaviour
{
    public static ManPowerManager_Scr instance;

    #region 레벨, 인력수
    public int i_Level;             // 현 레벨
    public int i_LevelManCount;
    //public int i_ManCount;          // 현재 고용 숫자
    public int i_MaxLevel = 1000;
    public const int i_MaxMan = 1000; // 최대 고용 숫자
    public int i_PossibleMan;       // 고용 가능한 숫자
    #endregion

    #region 텍스트 표기
    public TextMeshProUGUI t_LobbyLevel;
    public TextMeshProUGUI t_InGameLevel;
    #endregion

    public int i_preLevelCount;  // 최대 인력수를 확인하기 위한 변수
    public int i_BuyCoin = 10;
    public int i_AllAtk = 0; // 현재 공의 총 공격력

    public bool b_IsLoaded;
    public bool b_isSort;
    public bool b_isSorted; 
    public bool b_isUp; // 레벨업 후 최대 인력 수가 증가하였는지 체크하는 변수
    public bool b_isPointerEnter;
    public bool[] b_isGetAchievement = new bool[6];

    public GameObject g_CheckImage;
    public GameObject g_NewManCnt;
    public GameObject g_TutorialShield;
    //public GameObject g_BuyManPanel;
    public Transform t_ParentMans;      // 각 인력들의 위치를 잡기 위한 부모 위치
    public List<GameObject> manList = new List<GameObject>();
    public List<GameObject> BoughtINmanList = new List<GameObject>();
    public List<GameObject> heroList = new List<GameObject>();
    public List<GameObject> sellList = new List<GameObject>();
    public Transform[] BulletGroup;
    public GameObject[] g_Bullet;
    public GameObject g_HeroSlot;


    public GameObject ready_sellbutton;
    public GameObject active_sellbutton;
    public GameObject Cancel_sellbutton;

    public Sprite[] s_Icon;

    public TextMeshProUGUI t_NewManCount;
    public TextMeshProUGUI t_NowManCount;
    public TextMeshProUGUI t_MaxManCount;
    public TextMeshProUGUI t_MaxHp;
    public TextMeshProUGUI t_UIManCount;
    public TextMeshProUGUI t_LevelUpStat;
    public TextMeshProUGUI t_sellCoin;
    public int i_sellCoin;

    // csv에서 데이터를 뽑고 데이터 내용을 주기 위한 변수들
    string s_ManStat;
    string[] s_ManSplit;

    public TextAsset myTxt;
    public List<Dictionary<string, object>> ManData;

    private Coroutine c_UpdateCoroutine;


    // GetComponent 미리 캐싱
    //private ManStat_Scr Button_Option_RankPoint_Text;


    int i_boughtCount = 0;
    int i_Available = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            ManData = CSVReader.Read(myTxt);
        }
        else
            return;
    } 

    void Start()
    {
        b_isSort = false;
        b_isSorted = true;


        if (!b_IsLoaded)
        {
            for (int i = 0; i < t_ParentMans.childCount; ++i)
                t_ParentMans.GetChild(i).gameObject.GetComponent<ManStat_Scr>().i_ListNum = i + 1;

        }
        i_preLevelCount = 1;

      //  StartCoroutine(UpdateCoroutine());
    }


    public void initBall()
    {
        t_ParentMans.GetChild(0).gameObject.GetComponent<ManStat_Scr>().b_Bought = false;
        t_ParentMans.GetChild(0).gameObject.GetComponent<ManStat_Scr>().i_UpgradeNum = 1;

        // 이름
        s_ManStat = ManData[1]["Name_Korean"].ToString();
        s_ManSplit = s_ManStat.Split('_');
        s_ManStat = s_ManSplit[0];
        t_ParentMans.GetChild(0).gameObject.GetComponent<ManStat_Scr>().s_Name = s_ManStat;

        // 공격력
        t_ParentMans.GetChild(0).gameObject.GetComponent<ManStat_Scr>().f_Atk = (int)ManData[1]["Attack"];

        s_ManStat = ManData[1]["Name_Korean"].ToString();
        s_ManSplit = s_ManStat.Split('_');
        t_ParentMans.GetChild(0).gameObject.GetComponent<ManStat_Scr>().i_Level = int.Parse(s_ManSplit[1].Substring(2));
        t_ParentMans.GetChild(0).gameObject.GetComponent<ManStat_Scr>().g_Upgrade.SetActive(true);
    }

    // 튜토리얼 수행 후 동료 획득
    public void TutorialReward()
    {
        manList.Clear();
        for (int i = 0; i < 5; i++)
        {
            t_ParentMans.GetChild(i).gameObject.GetComponent<ManStat_Scr>().b_Bought = true;
            t_ParentMans.GetChild(i).gameObject.GetComponent<ManStat_Scr>().i_UpgradeNum = 1;

            // 이름
            s_ManStat = ManData[1]["Name_Korean"].ToString();
            s_ManSplit = s_ManStat.Split('_');
            s_ManStat = s_ManSplit[0];
            t_ParentMans.GetChild(i).gameObject.GetComponent<ManStat_Scr>().s_Name = s_ManStat;

            // 공격력
            t_ParentMans.GetChild(i).gameObject.GetComponent<ManStat_Scr>().f_Atk = (int)ManData[1]["Attack"];

            s_ManStat = ManData[1]["Name_Korean"].ToString();
            s_ManSplit = s_ManStat.Split('_');
            t_ParentMans.GetChild(i).gameObject.GetComponent<ManStat_Scr>().i_Level = int.Parse(s_ManSplit[1].Substring(2));
            t_ParentMans.GetChild(i).gameObject.GetComponent<ManStat_Scr>().g_Upgrade.SetActive(true);


            GameObject obj = t_ParentMans.GetChild(i).gameObject;
            obj.SetActive(true);
            manList.Add(obj);
        }


        i_LevelManCount = 5;
        GoldManager_Scr.instance.i_ManPowerList = i_LevelManCount;

        for (int i = 0; i < manList.Count; ++i)
        {
            // 공격력이 낮은 순서대로 볼카운트 넣어주기
            // UI에서 보여줄 인력수 카운트
            if (manList[i].GetComponent<ManStat_Scr>().b_Bought)
            {
                if (Player_Input.instance.i_BallCount[manList[i].GetComponent<ManStat_Scr>().i_TypeNum] < 6)
                {
                    Player_Input.instance.i_BallCount[manList[i].GetComponent<ManStat_Scr>().i_TypeNum]++;
                }
            }
            manList[i].GetComponent<ManStat_Scr>().GetTutorial();
        }
    }
    
    void Update()
    {
        // 플레이어 레벨에 따른 동료 최대 획득 수치
        int manCnt = 0;
        if (i_LevelManCount < i_MaxMan)
        {
            if (i_Level <= 2)
            {
                if (TutorialManager.instance.b_isManPowerTutorial)
                    i_LevelManCount = 5;
                else
                    i_LevelManCount = 1;
            }
            else
            {
                if (TutorialManager.instance.b_isManPowerTutorial)
                    i_LevelManCount = (i_Level + 1) / 2 + 4;
                else
                    i_LevelManCount = (i_Level + 1) / 2;

            }
            if (GoldManager_Scr.instance != null)
                GoldManager_Scr.instance.i_ManPowerList = i_LevelManCount;
        }
        else
        {
            i_LevelManCount = i_MaxMan;
            GoldManager_Scr.instance.i_ManPowerList = i_LevelManCount;
        }

        i_boughtCount = 0;
        for (int i = 0; i < t_ParentMans.childCount; i++)
        {
            if (!t_ParentMans.GetChild(i).gameObject.GetComponent<ManStat_Scr>().b_isHero && t_ParentMans.GetChild(i).gameObject.GetComponent<ManStat_Scr>().b_Bought)
            {
                i_boughtCount++;
            }
        }

        // 영입권 애들 숫자
        i_Available = i_LevelManCount - i_boughtCount;

        if (t_ParentMans.childCount >= i_LevelManCount)
        {
            for (int i = 0; i < t_ParentMans.childCount; ++i)
            {
                GameObject obj = t_ParentMans.GetChild(i).gameObject;

                if(i_Available > 0 && !obj.GetComponent<ManStat_Scr>().b_Bought)
                {
                    obj.SetActive(true);
                    i_Available--;
                }
                /*
                if (obj.GetComponent<ManStat_Scr>().i_ListNum <= i_LevelManCount)
                    obj.SetActive(true);
                    */

                else
                {
                    if (obj.GetComponent<ManStat_Scr>().b_Bought)
                    {
                        obj.SetActive(true);
                    }
                    else
                    {
                        obj.SetActive(false);
                    }
                }

                if (obj.GetComponent<ManStat_Scr>().b_Bought)
                    manCnt++;
            }
        }
        else
        {
            GameObject obj = Instantiate(Resources.Load("Slot/Man"), t_ParentMans.position, Quaternion.identity) as GameObject;
            obj.transform.SetParent(t_ParentMans);
            obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            obj.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);
        }

        if (i_LevelManCount != i_preLevelCount)
            b_isUp = true;

        i_preLevelCount = i_LevelManCount;

        t_NowManCount.text = string.Concat((manCnt - heroList.Count).ToString(), " /");
        t_MaxManCount.text = i_LevelManCount.ToString();
        t_NewManCount.text = (i_LevelManCount - (manCnt - heroList.Count)).ToString();

        if (TutorialManager.instance.b_isManPowerTutorial && i_LevelManCount - (manCnt - heroList.Count) > 0)
        {
            g_NewManCnt.SetActive(true);
        }
        else if (!TutorialManager.instance.b_isManPowerTutorial || i_LevelManCount - (manCnt - heroList.Count) <= 0)
        {
            g_NewManCnt.SetActive(false);
        }
        if (HPManager_Scr.instance != null)
        {
            t_MaxHp.text = ((int)HPManager_Scr.instance.f_FullHp).ToString();
        }

        t_LobbyLevel.text = string.Concat("Lv.", i_Level);
        t_InGameLevel.text = string.Concat("Lv.", i_Level);

        if (!TutorialManager.instance.b_FirstPlaying)
        {
            t_UIManCount.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 106f);
            t_UIManCount.text = manCnt.ToString();
        }
        else
            t_UIManCount.text = "";

        if(!Button_Option.instance.G_ManPower.activeSelf)
            SortBall();

    }

    private IEnumerator UpdateCoroutine()
    {
        WaitForEndOfFrame delay = new WaitForEndOfFrame();
        while (true)
        {
            int manCnt = 0;
            if (i_LevelManCount < i_MaxMan)
            {
                if (i_Level <= 2)
                {
                    if (TutorialManager.instance.b_isManPowerTutorial)
                        i_LevelManCount = 5;
                    else
                        i_LevelManCount = 1;
                }
                else
                {
                    if (TutorialManager.instance.b_isManPowerTutorial)
                        i_LevelManCount = (i_Level + 1) / 2 + 4;
                    else
                        i_LevelManCount = (i_Level + 1) / 2;

                }
                if (GoldManager_Scr.instance != null)
                    GoldManager_Scr.instance.i_ManPowerList = i_LevelManCount;
            }
            else
            {
                i_LevelManCount = i_MaxMan;
                GoldManager_Scr.instance.i_ManPowerList = i_LevelManCount;
            }


            i_boughtCount = 0;
            for (int i = 0; i < t_ParentMans.childCount; i++)
            {
                if (!t_ParentMans.GetChild(i).gameObject.GetComponent<ManStat_Scr>().b_isHero && t_ParentMans.GetChild(i).gameObject.GetComponent<ManStat_Scr>().b_Bought)
                {
                    i_boughtCount++;
                }
            }

            // 영입권 애들 숫자
            i_Available = i_LevelManCount - i_boughtCount;

            if (t_ParentMans.childCount >= i_LevelManCount)
            {
                for (int i = 0; i < t_ParentMans.childCount; ++i)
                {
                    GameObject obj = t_ParentMans.GetChild(i).gameObject;


                    if (i_Available > 0 && !obj.GetComponent<ManStat_Scr>().b_Bought)
                    {
                        obj.SetActive(true);
                        i_Available--;
                    }
                    /*
                    if (obj.GetComponent<ManStat_Scr>().i_ListNum <= i_LevelManCount)
                        obj.SetActive(true);

                    */
                    else
                    {

                        if (obj.GetComponent<ManStat_Scr>().b_Bought)
                        {
                            obj.SetActive(true);
                        }
                        else
                        {
                            obj.SetActive(false);
                        }
                    }

                    if (obj.GetComponent<ManStat_Scr>().b_Bought)
                        manCnt++;
                }
            }
            else
            {
                GameObject obj = Instantiate(Resources.Load("Slot/Man"), t_ParentMans.position, Quaternion.identity) as GameObject;
                obj.transform.SetParent(t_ParentMans);
                obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                obj.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);
            }

            if (i_LevelManCount != i_preLevelCount)
                b_isUp = true;

            i_preLevelCount = i_LevelManCount;

            t_NowManCount.text = string.Concat(manCnt.ToString(), " /");
            t_MaxManCount.text = i_LevelManCount.ToString();
            t_NewManCount.text = (i_LevelManCount - (manCnt - heroList.Count)).ToString();

            if (TutorialManager.instance.b_isManPowerTutorial && i_LevelManCount - (manCnt - heroList.Count) > 0)
            {
                g_NewManCnt.SetActive(true);
            }
            else if (!TutorialManager.instance.b_isManPowerTutorial || i_LevelManCount - (manCnt - heroList.Count) <= 0)
            {
                g_NewManCnt.SetActive(false);
            }
            if(HPManager_Scr.instance != null)
            {
                t_MaxHp.text = ((int)HPManager_Scr.instance.f_FullHp).ToString();
            }

            t_LobbyLevel.text = string.Concat("Lv.", i_Level);
            t_InGameLevel.text = string.Concat("Lv.", i_Level);

            if (!TutorialManager.instance.b_FirstPlaying)
            {
                t_UIManCount.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 106f);
                t_UIManCount.text = manCnt.ToString();
            }
            else
                t_UIManCount.text = "";

            if (!Button_Option.instance.G_LevelUp.activeSelf)
                SortBall();

            yield return delay;
        }
    }

    public void Coroutine_Sort()
    {
        if (!TutorialManager.instance.b_FirstPlaying)
        {
            StopCoroutine(SortBallCorutine());
            StartCoroutine(SortBallCorutine());
        }
    }

    // 볼 정렬
    public void SortBall()
    {
        manList.Clear();
        BoughtINmanList.Clear();
        int heroNum = 0;
        int buyMan = 0;
        int dontBuy = 0;

        for (int i = 0; i < Player_Input.instance.i_BallCount.Length; ++i)
        {
            Player_Input.instance.i_BallCount[i] = 0;
        }

        for (int i = 0; i < t_ParentMans.childCount; ++i)
        {
            GameObject obj = t_ParentMans.GetChild(i).gameObject;

            if (obj.transform.GetComponent<ManStat_Scr>().b_Bought)
            {
                BoughtINmanList.Add(obj);
                obj.transform.gameObject.SetActive(true);
            }

            if (obj.activeSelf)
            {
                manList.Add(obj);


                if (!obj.transform.GetComponent<ManStat_Scr>().b_Bought)
                {
                    dontBuy++;
                }
                else
                {
                    if (obj.transform.GetComponent<ManStat_Scr>().b_isHero)
                    {
                        heroNum++;
                    }
                    else
                    {
                        buyMan++;
                    }
                }
            }
        }


        int k = 0;
        for (int i = 0; i < t_ParentMans.childCount; ++i)
        {
            GameObject obj = t_ParentMans.GetChild(i).gameObject;

            if (obj.transform.GetComponent<ManStat_Scr>().b_isHero)
            {
                obj.transform.SetSiblingIndex(dontBuy + k);

                if(k < heroNum)
                {
                    k++;
                }
            }
        }

        // 공격력이 낮은 순서대로 우선 정렬
        manList.Sort(delegate (GameObject A, GameObject B)
        {
            if (A.GetComponent<ManStat_Scr>().f_Atk > B.GetComponent<ManStat_Scr>().f_Atk) return 1;
            else if (A.GetComponent<ManStat_Scr>().f_Atk < B.GetComponent<ManStat_Scr>().f_Atk) return -1;
            else return 0;
        });

        if (!b_isPointerEnter && b_isSort && !b_isSorted)
        {
            int j = 0;
            // 정렬버튼 클릭 시 낮은 순서대로 위치 잡아주기
            for (int i = 0; i < manList.Count; ++i)
            {
                if (!manList[i].transform.GetComponent<ManStat_Scr>().b_Bought)
                {
                    manList[i].transform.SetSiblingIndex(i);
                }
                else
                {
                    if (!manList[i].transform.GetComponent<ManStat_Scr>().b_isHero)
                    {
                        print(dontBuy + heroNum + i);
                        manList[i].transform.SetSiblingIndex(dontBuy + heroNum + j);

                        if(j < buyMan)
                        {
                            j++;
                        }
                    }
                }
            }
            b_isSorted = true;
        }
        /*
        for (int i = 0; i < manList.Count; ++i)
        {
            // 공격력이 낮은 순서대로 볼카운트 넣어주기
            // UI에서 보여줄 인력수 카운트
            if (manList[i].GetComponent<ManStat_Scr>().b_Bought)
            {
                Player_Input.instance.i_BallCount[manList[i].GetComponent<ManStat_Scr>().i_TypeNum]++;
            }
        }*/

        for (int i = 0; i < t_ParentMans.transform.childCount; ++i)
        {
            if (t_ParentMans.transform.GetChild(i).GetComponent<ManStat_Scr>().b_Bought)
            {
                Player_Input.instance.i_BallCount[t_ParentMans.transform.GetChild(i).GetComponent<ManStat_Scr>().i_TypeNum]++;
            }
        }
    }

    IEnumerator SortBallCorutine()
    {
        manList.Clear();
        BoughtINmanList.Clear();
        int heroNum = 0;
        int buyMan = 0;
        int dontBuy = 0;

        for (int i = 0; i < Player_Input.instance.i_BallCount.Length; ++i)
        {
            Player_Input.instance.i_BallCount[i] = 0;
        }

        for (int i = 0; i < t_ParentMans.childCount; ++i)
        {
            GameObject obj = t_ParentMans.GetChild(i).gameObject;

            if (obj.transform.GetComponent<ManStat_Scr>().b_Bought)
            {
                BoughtINmanList.Add(obj);
                obj.transform.gameObject.SetActive(true);
            }

            if (obj.activeSelf)
            {
                manList.Add(obj);


                if (!obj.transform.GetComponent<ManStat_Scr>().b_Bought)
                {
                    dontBuy++;
                }
                else
                {
                    if (obj.transform.GetComponent<ManStat_Scr>().b_isHero)
                    {
                        heroNum++;
                    }
                    else
                    {
                        buyMan++;
                    }
                }
            }
        }

        int k = 0;
        for (int i = 0; i < t_ParentMans.childCount; ++i)
        {
            GameObject obj = t_ParentMans.GetChild(i).gameObject;

            if (obj.transform.GetComponent<ManStat_Scr>().b_isHero)
            {
                obj.transform.SetSiblingIndex(dontBuy + k);

                if (k < heroNum)
                {
                    k++;
                }
            }
        }

        // 공격력이 낮은 순서대로 우선 정렬
        manList.Sort(delegate (GameObject A, GameObject B)
        {
            if (A.GetComponent<ManStat_Scr>().f_Atk > B.GetComponent<ManStat_Scr>().f_Atk) return 1;
            else if (A.GetComponent<ManStat_Scr>().f_Atk < B.GetComponent<ManStat_Scr>().f_Atk) return -1;
            else return 0;
        });

        if (!b_isPointerEnter && b_isSort && !b_isSorted)
        {
            int j = 0;
            // 정렬버튼 클릭 시 낮은 순서대로 위치 잡아주기
            for (int i = 0; i < manList.Count; ++i)
            {
                if (!manList[i].transform.GetComponent<ManStat_Scr>().b_Bought)
                {
                    manList[i].transform.SetSiblingIndex(i);
                }
                else
                {
                    if (!manList[i].transform.GetComponent<ManStat_Scr>().b_isHero)
                    {
                        print(dontBuy + heroNum + i);
                        manList[i].transform.SetSiblingIndex(dontBuy + heroNum + j);

                        if (j < buyMan)
                        {
                            j++;
                        }
                    }
                }
            }
            b_isSorted = true;
        }
        /*
        for (int i = 0; i < manList.Count; ++i)
        {
            // 공격력이 낮은 순서대로 볼카운트 넣어주기
            // UI에서 보여줄 인력수 카운트
            if (manList[i].GetComponent<ManStat_Scr>().b_Bought)
            {
                Player_Input.instance.i_BallCount[manList[i].GetComponent<ManStat_Scr>().i_TypeNum]++;
            }
        }*/

        for (int i = 0; i < t_ParentMans.transform.childCount; ++i)
        {
            if (t_ParentMans.transform.GetChild(i).GetComponent<ManStat_Scr>().b_Bought)
            {
                Player_Input.instance.i_BallCount[t_ParentMans.transform.GetChild(i).GetComponent<ManStat_Scr>().i_TypeNum]++;
            }
        }
        StopCoroutine(SortBallCorutine());
        yield return null;
    }

    public void WeakSort()
    {
        b_isSort = !b_isSort;
        g_CheckImage.SetActive(b_isSort);

        if (b_isSort)
            b_isSorted = false;
    }

    public void OnManPowerWindowOn()
    {
        b_isSort = true;
        g_CheckImage.SetActive(true);
        b_isSorted = false;
    }

    public void LevelUp()
    {
        i_Level++;

#if UNITY_ANDROID
        if (GoogleManager_Scr.instance != null && GoogleManager_Scr.instance.b_isLogin)
        {
            switch (i_Level)
            {
                case 10:
                    GoogleManager_Scr.instance.GetAchievementLV10();
                    break;
                case 50:
                    GoogleManager_Scr.instance.GetAchievementLV50();
                    break;
                case 100:
                    GoogleManager_Scr.instance.GetAchievementLV100();
                    break;
                case 200:
                    GoogleManager_Scr.instance.GetAchievementLV200();
                    break;
                case 300:
                    GoogleManager_Scr.instance.GetAchievementLV300();
                    break;
                case 400:
                    GoogleManager_Scr.instance.GetAchievementLV400();
                    break;
                case 500:
                    GoogleManager_Scr.instance.GetAchievementLV500();
                    break;
                case 600:
                    GoogleManager_Scr.instance.GetAchievementLV600();
                    break;
                case 700:
                    GoogleManager_Scr.instance.GetAchievementLV700();
                    break;
                case 800:
                    GoogleManager_Scr.instance.GetAchievementLV800();
                    break;
                case 900:
                    GoogleManager_Scr.instance.GetAchievementLV900();
                    break;
            }

            if (i_Level == i_MaxLevel)
            {
                GoogleManager_Scr.instance.GetAchievementLVMAX();
                ExpManager_Scr.instance.f_nowExp = ExpManager_Scr.instance.f_fullExp;
            }
        }

#elif UNITY_IOS
        if (AppleManager_Scr.instance != null && AppleManager_Scr.instance.b_isLogin)
        {
            switch (i_Level)
            {
                case 10:
                    AppleManager_Scr.instance.GetAchievementLV10();
                    break;
                case 50:
                    AppleManager_Scr.instance.GetAchievementLV50();
                    break;
                case 100:
                    AppleManager_Scr.instance.GetAchievementLV100();
                    break;
                case 200:
                    AppleManager_Scr.instance.GetAchievementLV200();
                    break;
                case 300:
                    AppleManager_Scr.instance.GetAchievementLV300();
                    break;
                case 400:
                    AppleManager_Scr.instance.GetAchievementLV400();
                    break;
                case 500:
                    AppleManager_Scr.instance.GetAchievementLV500();
                    break;
                case 600:
                    AppleManager_Scr.instance.GetAchievementLV600();
                    break;
                case 700:
                    AppleManager_Scr.instance.GetAchievementLV700();
                    break;
                case 800:
                    AppleManager_Scr.instance.GetAchievementLV800();
                    break;
                case 900:
                    AppleManager_Scr.instance.GetAchievementLV900();
                    break;
            }

            if (i_Level == i_MaxLevel)
            {
                AppleManager_Scr.instance.GetAchievementLVMAX();
                ExpManager_Scr.instance.f_nowExp = ExpManager_Scr.instance.f_fullExp;
            }
        }

#endif
        Button_Option.instance.LevelUpEft();
    }

    public void DeleteHero(GameObject slot) // 히어로 조합, 판매에 쓰일 히어로 삭제  // 동료창의 히어로 슬롯 오브젝트를 slot변수에 넣으면 됨
    {
        if (slot.transform.GetComponent<ManStat_Scr>().b_isHero)
        {
            heroList.Remove(slot);
            manList.Remove(slot);
            BoughtINmanList.Remove(slot);

            slot.SetActive(false);
            //SortBall();

            Destroy(slot);
        }
        else
        {
            heroList.Remove(slot);
            manList.Remove(slot);
            BoughtINmanList.Remove(slot);

            slot.SetActive(false); 
            SortBall();
            slot.transform.GetComponent<ManStat_Scr>().b_Bought = false;
            slot.transform.GetComponent<ManStat_Scr>().g_LockPurchase.SetActive(true);
            

            slot.transform.GetComponent<ManStat_Scr>().i_Level = 0;
            slot.transform.GetComponent<ManStat_Scr>().f_Atk = -999;
            slot.transform.GetComponent<ManStat_Scr>().i_Money = 10;
            slot.transform.GetComponent<ManStat_Scr>().i_UpgradeNum = 0;
            slot.transform.GetComponent<ManStat_Scr>().i_UpgradeMoney = 10;
            slot.transform.GetComponent<ManStat_Scr>().i_TypeNum = 0;
            slot.transform.GetComponent<ManStat_Scr>().i_HeroNum = 0;
            slot.transform.GetComponent<ManStat_Scr>().f_TextAtk = 0;
            slot.transform.GetComponent<ManStat_Scr>().b_selectSell = false;



            slot.transform.GetComponent<ManStat_Scr>().StopSellMode();
        }

       // Coroutine_Sort();
       // SortBall();
       // Coroutine_Sort();
       // OnManPowerWindowOn();
    }

    // 아래 두 함수가 동료 히어로 팔기
    public void OnClickSell()
    {
        //   이상태에서 판매하면 히어로 리스트 for문 돌려서 해당 bool이 켜져 있는 놈들 삭제하며 코인 획득


        for (int i = 0; i < sellList.Count; i++)
        {
            if (sellList[i].transform.GetComponent<ManStat_Scr>().b_selectSell)
            {
                DeleteHero(sellList[i]);
                sellList[i].transform.SetSiblingIndex(0);
            }
        }

        GoldManager_Scr.instance.PlusGold(i_sellCoin);
        sellList.Clear();
        SoundManager_sfx.instance.PlaySE("Take2", false);

        i_sellCoin = 0;
        OnClickSellModeOFF();
        ready_sellbutton.SetActive(true);
        active_sellbutton.SetActive(false);

    }

    public void OnClickSellModeOn()
    {
        // 쓰레기통 혹은 판매모드 클릭시 켜지는 함수 
        // 쓰레기통 버튼을 누르면 활성화된 동료슬롯에 전부 체크칸이 켜진다. 체크칸을 클릭하면 해당 슬롯 스크립트의 bool이 on되고 체크가 켜진다.
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);

        ready_sellbutton.SetActive(false);
        active_sellbutton.SetActive(true);
        Cancel_sellbutton.SetActive(true);

        for (int i = 0; i < manList.Count; i++)
        {
            if (manList[i].transform.GetComponent<ManStat_Scr>().b_Bought)
            {
                manList[i].transform.GetComponent<ManStat_Scr>().readytoSelect();
            }
        }

        i_sellCoin = 0;
        t_sellCoin.text = i_sellCoin.ToString() + "G";
    }

    public void OnClickSellModeOFF()
    {
        // 쓰레기통 혹은 판매모드 클릭시 켜지는 함수 
        // 쓰레기통 버튼을 누르면 활성화된 동료슬롯에 전부 체크칸이 켜진다. 체크칸을 클릭하면 해당 슬롯 스크립트의 bool이 on되고 체크가 켜진다.
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);



        i_sellCoin = 0;
        sellList.Clear();


        ready_sellbutton.SetActive(true);
        active_sellbutton.SetActive(false);
        Cancel_sellbutton.SetActive(false);

        for (int i = 0; i < manList.Count; i++)
        {
            if (manList[i].transform.GetComponent<ManStat_Scr>().b_Bought)
            {
                manList[i].transform.GetComponent<ManStat_Scr>().StopSellMode();
            }
        }
    }
}
