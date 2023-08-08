using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

using System.IO;
using TMPro;

public class Button_Option : MonoBehaviour
{
    public static Button_Option instance;
     
    // 옵션값
    public bool setVolume_Background = true;
    public bool setVolume_Effect = true;

    public GameObject Cam;
    public GameObject UICam;
    public GameObject AfterUI_Upside;
    public GameObject AfterUI_Downside;
    public GameObject G_Enemy;
    private Animator CamAni;
    private Animator AfterUI_UpsideAni;
    private Animator AfterUI_DownsideAni;
    public bool b_WindowOn = false;
    public bool b_Golobby = false;
    public bool b_isSlowTime = false;

    public GameObject G_Before;
    public GameObject Before_Option;
    public GameObject Before_RadioButton_BGM_ON;
    public GameObject Before_RadioButton_BGM_OFF;
    public GameObject Before_RadioButton_Effect_ON;
    public GameObject Before_RadioButton_Effect_OFF;

    public GameObject G_After;
    public GameObject After_Pause;
    public GameObject After_RadioButton_BGM_ON;
    public GameObject After_RadioButton_BGM_OFF;
    public GameObject After_RadioButton_Effect_ON;
    public GameObject After_RadioButton_Effect_OFF;

    public GameObject G_Home;
    public GameObject go_TextGold_Dia_Exp;

    public GameObject G_Skill;
    public GameObject g_BeforeSkill;
    public GameObject g_SkillSpeech;
    public bool b_isSkillActive;

    public GameObject G_Timer;
    public GameObject g_StageBG;
    public GameObject g_RankButton;
    public GameObject g_RankPoint;

    public GameObject G_CoinShop;
    public GameObject G_GemShop;

    public GameObject G_ManPower;

    public GameObject G_VendingMachine;
    public GameObject g_MainCinemaCam;

    public GameObject G_Shop;

    public GameObject G_CharacterShop;

    public GameObject G_LevelUp;

    public GameObject G_Mission;

    public GameObject t_Warning;

    public GameObject g_FadeInOut;

    public GameObject g_FirstBG;

    public GameObject g_DownSideGroup;

    public GameObject g_SkillPopUpList;

    public GameObject go_Notapplicable;

    public GameObject go_PlayerHitScore;

    public GameObject g_StageButton;
    
    public GameObject G_Stage;

    public GameObject G_Attendance;

    public GameObject G_Review;

    public GameObject G_Inventory;
    public GameObject G_Synthesis;
    public GameObject G_SkipButton;
    public GameObject g_boxLight;
    public ChangeLobbyBG LobbyBG;
    public TextMeshProUGUI t_MissionTime;

    public int i_PopUpSkillNum;
    float tmpTime;

    public bool b_Attence = false;
    public bool b_InGame = false;
    public bool b_isStart;
    public bool b_isBoss;
    public bool b_isLvUp;
    public bool b_isPopUpOn;
    public bool b_isEndIntro;
    public bool b_GoAttendanceActive;
    #region 스크롤뷰 버튼 민감도 관련
    private const float inchToCm = 2.54f;
    [SerializeField] private EventSystem eventSystem = null;
    [SerializeField] private float dragThresholdCM = 0.5f; //For drag Threshold
    #endregion

    Coroutine c_MissionTime;

    private void Awake()
    {

        if (instance == null)
            instance = this;
        else
            return;
    }
    private void SetDragThreshold()
    {
        if (eventSystem != null)
        {
            eventSystem.pixelDragThreshold = (int)(dragThresholdCM * Screen.dpi / inchToCm);
        }
    }
    void Start()
    {
        b_Golobby = true;
        SetDragThreshold();
        CamAni = Cam.transform.GetComponent<Animator>();
        AfterUI_UpsideAni = AfterUI_Upside.transform.GetComponent<Animator>();
        AfterUI_DownsideAni = AfterUI_Downside.transform.GetComponent<Animator>();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Invoke("setPos_Canvas", 0.1f);
        Invoke("GoogleDriveSave", 3.0f);
        c_MissionTime = StartCoroutine(MissionTimeChecker());
    }

    public void GoogleDriveSave()
    {
#if UNITY_ANDROID
        if (GoogleManager_Scr.instance.b_OffLoad)
            GoogleManager_Scr.instance.SaveCloud();

#elif UNITY_IOS
        if (AppleManager_Scr.instance.b_OffLoad)
            AppleManager_Scr.instance.SaveCloud();

#endif
    }

    IEnumerator MissionTimeChecker()
    {
        while (true)
        {
            DateTime Target = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            Target = Target.AddDays(1);
            TimeSpan LeftTime = Target - DateTime.Now;
            t_MissionTime.text = string.Concat(LeftTime.Hours, ":");
            if (LeftTime.Minutes >= 10)
                t_MissionTime.text = string.Concat(t_MissionTime.text, LeftTime.Minutes + 1);
            else
            {
                t_MissionTime.text = string.Concat(t_MissionTime.text, "0");
                t_MissionTime.text = string.Concat(t_MissionTime.text, LeftTime.Minutes + 1);
            }
            yield return null;
        }
    }

    void setPos_Canvas()
    {
        for (int i = 5; i < G_After.transform.childCount; i++)
        {
            G_After.transform.GetChild(i).gameObject.SetActive(false);
        }
        G_After.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        G_Timer.SetActive(false); // 타이머 비활성화
        g_BeforeSkill.SetActive(false); // 스킬버튼 비활성화

        ManPowerManager_Scr.instance.Coroutine_Sort();

        Invoke("InitBG", 0.1f);
    }

    void InitBG()
    {
        g_FirstBG.SetActive(false);
        G_Stage.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 40, -9.3f);
        G_Stage.SetActive(false);
    }

    private void Update()
    {
        if (!b_Golobby)
        {
            G_Attendance.SetActive(false);
        }

        if (G_Attendance.activeSelf || b_isStart)
        {
            cancelinvoke_attendance();
        }

        if (setVolume_Background)
        {
            Before_RadioButton_BGM_ON.SetActive(true);
            After_RadioButton_BGM_ON.SetActive(true);
            Before_RadioButton_BGM_OFF.SetActive(false);
            After_RadioButton_BGM_OFF.SetActive(false);
        }
        else
        {
            Before_RadioButton_BGM_OFF.SetActive(true);
            After_RadioButton_BGM_OFF.SetActive(true);
            Before_RadioButton_BGM_ON.SetActive(false);
            After_RadioButton_BGM_ON.SetActive(false);
        }

        if (setVolume_Effect)
        {
            Before_RadioButton_Effect_ON.SetActive(true);
            After_RadioButton_Effect_ON.SetActive(true);
            Before_RadioButton_Effect_OFF.SetActive(false);
            After_RadioButton_Effect_OFF.SetActive(false);
        }
        else
        {
            Before_RadioButton_Effect_OFF.SetActive(true);
            After_RadioButton_Effect_OFF.SetActive(true);
            Before_RadioButton_Effect_ON.SetActive(false);
            After_RadioButton_Effect_ON.SetActive(false);
        }
        if (!b_InGame)
        {
            g_StageButton.GetComponent<Button>().enabled = true;
            RespawnManager.instance.AllBlock.Clear();
            RespawnManager.instance.ActiveBlock.Clear();
            RespawnManager.instance.ObjBlock.Clear();
            RespawnManager.instance.MobBlock.Clear();
            RespawnManager.instance.CoinBlock.Clear();
            RespawnManager.instance.GemBlock.Clear();
            RespawnManager.instance.BulldogBlock.Clear();
            RespawnManager.instance.BossBlock.Clear();
            RespawnManager.instance.SpeechBlock.Clear();
            RespawnManager.instance.ResetLine();
        }

        if (b_InGame && Player_Input.instance.b_isAttacking)
        {
            g_StageButton.GetComponent<Button>().enabled = false;
        }
        else if(b_InGame && !Player_Input.instance.b_isAttacking)
        {
            g_StageButton.GetComponent<Button>().enabled = true;
        }

        if (G_Stage.activeSelf && RewardManager_Scr.instance.G_ButtonShield.activeSelf)
            RewardManager_Scr.instance.G_ButtonShield.SetActive(false);
    }


    public void cancelinvoke_attendance()
    {
        CancelInvoke("OnClickAttendanceOn");
    }

    public void OnClickBGM()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);
        if (setVolume_Background)
        {
            setVolume_Background = false;
            Before_RadioButton_BGM_OFF.SetActive(true);
            After_RadioButton_BGM_OFF.SetActive(true);
            Before_RadioButton_BGM_ON.SetActive(false);
            After_RadioButton_BGM_ON.SetActive(false);
        }
        else
        {
            setVolume_Background = true;
            Before_RadioButton_BGM_ON.SetActive(true);
            After_RadioButton_BGM_ON.SetActive(true);
            Before_RadioButton_BGM_OFF.SetActive(false);
            After_RadioButton_BGM_OFF.SetActive(false);
        }
    }

    public void Fullinventory()
    {
        InventoryCaching_Scr.instance.warninginventory.SetActive(false);
        InventoryCaching_Scr.instance.warninginventoryGem.SetActive(false);
        DeliveryBoxManager_Scr.instance.openningBox.SetActive(false);
        DeliveryBoxManager_Scr.instance.g_PartipleCanvas.SetActive(false);
        OpenBox.instance.inven_heroInfo.SetActive(false);
        OpenBox.instance.heroInfo.SetActive(false);
        OpenBox.instance.itemInfo.SetActive(false);
        OpenBox.instance.GoldInfo.SetActive(false);
        OpenBox.instance.g_Box.SetActive(true);
        OpenBox.instance.g_Box_etc.SetActive(true);
        b_WindowOn = true;
        
        if (!b_Attence)
        {
            DelayAttendanceON();
        }
    }

    public void OpenBox_()
    {
        OpenBox.instance.b_isOpen = true;
        SoundManager_sfx.instance.PlaySE("boxEffect", false);
        DeliveryBoxManager_Scr.instance.par_OpenParticle.Play();
        OpenBox.instance.g_Box_etc.SetActive(false);

        if (DeliveryBoxManager_Scr.instance.GetBoxCount <= 0)
        {
            if (GoldManager_Scr.instance.i_Gem >= 10)
            {
                GoldManager_Scr.instance.i_Gem -= 10;
            }
        }
        Invoke("getBoxitem", 2.0f);
    }
    public void getBoxitem()
    {
        DeliveryBoxManager_Scr.instance.GetDeliveryBox();

    }

    public void RoundSkip()
    {
        //t_Upside.GetComponent<Animator>().SetBool("isShoot", false);
        Player_Input.instance.t_Downside.GetComponent<Animator>().SetBool("isShoot", false);
        Player_Input.instance.b_OnceReturn = false;
        Player_Input.instance.resetRound();
        Player_Input.instance.i_SumlaunchBallCount = 0;

        RespawnManager.instance.timer_Lock = false;


        RespawnManager.instance.EndRound();
    }

    public void OnClickEft()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);
        if (setVolume_Effect)
        {
            setVolume_Effect = false;
            Before_RadioButton_Effect_OFF.SetActive(true);
            After_RadioButton_Effect_OFF.SetActive(true);
            Before_RadioButton_Effect_ON.SetActive(false);
            After_RadioButton_Effect_ON.SetActive(false);
        }
        else
        {
            setVolume_Effect = true;
            Before_RadioButton_Effect_ON.SetActive(true);
            After_RadioButton_Effect_ON.SetActive(true);
            Before_RadioButton_Effect_OFF.SetActive(false);
            After_RadioButton_Effect_OFF.SetActive(false);
        }
    }

    public void OnClickBeforeOptionON()
    {
        b_WindowOn = true;
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);
        Before_Option.SetActive(true);
    }

    public void OnClickBeforeOptionOFF()
    {
        b_WindowOn = false;
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);
        Before_Option.SetActive(false);
    }

    public void OnClickStart(bool NomalGame)
    {

        g_boxLight.SetActive(false);
        RewardManager_Scr.instance.ChangeWinLoseBG(true, 0);
        //G_After.transform.GetChild(G_After.transform.childCount - 1).gameObject.SetActive(true);
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);
        Time.timeScale = 1;
        UICam.transform.GetComponent<Cam_Size>().enabled = false;
        //Cam.transform.GetComponent<Cam_Size>().enabled = false;
        ShopManager_Scr.instance.g_ShopUI.SetActive(false);
        SynthesisCaching_Scr.instance.synthesisUI.SetActive(false);

        StageManager_Scr.instance.myAbility = 0;
        DeliveryBoxManager_Scr.instance.BoxUI.SetActive(false);
        if (NomalGame)
        {
            RespawnManager.instance.b_NomalGame = true;

            StageManager_Scr.instance.BGChange();
        }
        else
        {
            // 구글 로그인이 되어있고, 인터넷이 연결되어있는 경우만 가능(데이터를 사용하든, 와이파이를 사용하든)
            /*if (GoogleManager_Scr.instance.b_isLogin && Application.internetReachability != NetworkReachability.NotReachable)
            {
                RespawnManager.instance.b_NomalGame = false;
                StageManager_Scr.instance.RankingModeBG();
            }*/
            StageManager_Scr.instance.plusAbility = Convert.ToSingle(RespawnManager.instance.RespawnData[0]["Ability"]);
            RespawnManager.instance.b_NomalGame = false;
            StageManager_Scr.instance.RankingModeBG();
        }
        StageManager_Scr.instance.G_TimerBar.SetActive(true);
        Cam.transform.GetChild(0).GetComponent<FirstUILoading>().enabled = false;
        b_isStart = true;
        CamAni.SetBool("isEnd", false);
        CamAni.SetBool("isShoot", false);
        Player_Input.instance.b_resetPos = false;
        Player_Input.instance.b_Aiming = false;
        Cam.transform.GetComponent<Animator>().enabled = true;
        CamAni.SetBool("isStart", true);
        G_After.SetActive(true);
        ManPowerManager_Scr.instance.Coroutine_Sort();
        G_ManPower.SetActive(false);
        G_VendingMachine.SetActive(false);
        MachineSelectManager_Scr.instance.OffMachine();
        G_Shop.SetActive(false);
        G_CharacterShop.SetActive(false);
        G_CoinShop.SetActive(false);
        G_GemShop.SetActive(false);
        g_DownSideGroup.GetComponent<RectTransform>().anchoredPosition = new Vector3(-160, -670f, 0);
        G_LevelUp.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 100, 0);
        G_LevelUp.SetActive(false);
        AfterUI_UpsideAni.SetBool("isStart", true);
        AfterUI_UpsideAni.SetBool("isEnd", false);
        AfterUI_DownsideAni.SetBool("isStart", true);
        AfterUI_DownsideAni.SetBool("isEnd", false);
        G_Before.SetActive(false);
        G_Timer.SetActive(true); // 타이머 활성화
        g_BeforeSkill.SetActive(true); // 스킬버튼 활성화
        b_InGame = true;
        RewardManager_Scr.instance.OnClickClosePanel();

        if (ADMobManager_Scr.instance != null && ADMobManager_Scr.instance.b_isPremiumMode)
            ADMobManager_Scr.instance.HideNativeAD();
        g_FadeInOut.SetActive(true);
        g_FadeInOut.GetComponent<Animator>().SetTrigger("FadeOut");
        Player_Input.instance.t_Downside.GetComponent<Animator>().SetBool("isShoot", false);

        /*
        if ((int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Start_Spawn"] > 0)
        {
        }*/


        RespawnManager.instance.RespawnStart();
        Invoke("changeLobbyImage", 0.1f);
    }

    void changeLobbyImage()
    {
        LobbyBG.ChageImage();
    }

    public void CamAniStart_end()
    {
        b_Golobby = false;

        Cam.transform.GetComponent<Animator>().enabled = false;
    }
    public void CamAniEnd_end()
    {
        if (!b_isStart)
        {
            Cam.transform.GetComponent<Animator>().enabled = false;
        }
    }

    public void CamAniBoss_end()
    {
        Cam.transform.GetComponent<Animator>().enabled = false;
        Cam.transform.GetComponent<Animator>().SetBool("isShoot", false);
        Player_Input.instance.f_Delayatk = 0.0f;
        RespawnManager.instance.timer_Lock = false;
        RespawnManager.instance.b_BossSkillTime = false;
        Player_Input.instance.b_Aiming = true;
        Player_Input.instance.g_ButtonShield.SetActive(false);
        b_isBoss = false;
    }

    public void CamAniBoss_Dududungjang()
    {
        Cam.transform.GetComponent<Animator>().enabled = true;
        CamAni.SetTrigger("Boss");
        b_isBoss = true;
    }

    public void OnClickAfterPauseON()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);

        if (b_isStart)
        {
            Time.timeScale = 0;
            After_Pause.SetActive(true);
            b_WindowOn = true;
            RewardManager_Scr.instance.G_ButtonShield.SetActive(false);
        }
        else
        {
            b_WindowOn = true;
            Before_Option.SetActive(true);
        }
    }
    public void OnClickAfterPauseOFF()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);

        if (b_isStart)
        {
            Time.timeScale = 1;
            After_Pause.SetActive(false);
            G_Home.SetActive(false);
            b_WindowOn = false;
        }
        else
        {
            b_WindowOn = false;
            Before_Option.SetActive(false);
        }
    }
    public void OnClickAfterPauseHOME()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);

        G_Home.SetActive(true);
        Time.timeScale = 0;
        b_WindowOn = true;
    }

    void delayActiveBox()
    {
        DeliveryBoxManager_Scr.instance.BoxUI.SetActive(true);
    }

    public void CheckBox_BoxTutorial()
    {
        if (TutorialManager.instance.b_BoxTutorial)
        {
            TutorialManager.instance.b_BoxTutorial = false;
            TutorialManager.instance.G_Box_Tutorial_CheckImage.SetActive(false);
        }
        else
        {
            TutorialManager.instance.b_BoxTutorial = true;
            TutorialManager.instance.G_Box_Tutorial_CheckImage.SetActive(true);
        }
    }
    public void CheckBox_BagTutorial()
    {
        if (TutorialManager.instance.b_BagTutorial)
        {
            TutorialManager.instance.b_BagTutorial = false;
            TutorialManager.instance.G_Bag_Tutorial_CheckImage.SetActive(false);
        }
        else
        {
            TutorialManager.instance.b_BagTutorial = true;
            TutorialManager.instance.G_Bag_Tutorial_CheckImage.SetActive(true);
        }
    }
    public void CheckBox_SynthesisTutorial()
    {
        if (TutorialManager.instance.b_SynthesisTutorial)
        {
            TutorialManager.instance.b_SynthesisTutorial = false;
            TutorialManager.instance.G_Synthesis_Tutorial_CheckImage.SetActive(false);
        }
        else
        {
            TutorialManager.instance.b_SynthesisTutorial = true;
            TutorialManager.instance.G_Synthesis_Tutorial_CheckImage.SetActive(true);
        }
    }

    public void Confirm_BoxTutorial()
    {
        TutorialManager.instance.G_BoxTutorial.SetActive(false);
        Inventory_Scr.instance.InventoryCheck_ConfirmTutorial();
    }
    public void Confirm_BagTutorial()
    {
        TutorialManager.instance.G_Bag_Tutorial.SetActive(false);

        SoundManager_sfx.instance.PlaySE("UI_Click1", false);
        cancelinvoke_attendance();


        Time.timeScale = 0;
        Player_Input.instance.b_Aiming = false;
        Inventory_Scr.instance.SoldGold = 0;
        InventoryCaching_Scr.instance.itemList.GetComponent<RectTransform>().anchoredPosition = new Vector2(60f, -10f);
        InventoryCaching_Scr.instance.itemList.GetComponent<RectTransform>().sizeDelta = new Vector2(700f, 700f);
        InventoryCaching_Scr.instance.itemList.gameObject.SetActive(true);
        G_Inventory.SetActive(true);
        b_WindowOn = true;
    }
    public void Confirm_SynthesisTutorial()
    {
        TutorialManager.instance.G_Synthesis_Tutorial.SetActive(false);
    }



    public void OnClickAfterPauseHomeOn()
    {

        StageManager_Scr.instance.myAbility = 0;
        g_boxLight.SetActive(true);
        //G_After.transform.GetChild(G_After.transform.childCount - 1).gameObject.SetActive(false);
        Time.timeScale = 1;
        if(!RewardManager_Scr.instance.b_isEnd)
            SoundManager_sfx.instance.PlaySE("UI_Click1", false);

        print("체크1");
        Invoke("delayActiveBox", 0.9f);
        g_DownSideGroup.GetComponent<RectTransform>().anchoredPosition = new Vector3(5, -670f, 0);
        AfterUI_DownsideAni.SetTrigger("GoLobby");
        Player_Input.instance.go_BackCursor.SetActive(false);

        StageManager_Scr.instance.G_TimerBar.SetActive(false);

        // 라운드 시작 시 아래쪽 사이드바 투명하지 않게 만들도록 설정
        ShopManager_Scr.instance.g_ShopUI.SetActive(true);
        SynthesisCaching_Scr.instance.synthesisUI.SetActive(true);

        for (int i = 1; i < g_DownSideGroup.transform.childCount; ++i)
        {
            GameObject obj = g_DownSideGroup.transform.GetChild(i).gameObject;
            obj.GetComponent<Image>().color = new Color(255 / 255, 255 / 255, 255 / 255, 255 / 255);
            for (int j = 0; j < obj.transform.childCount; ++j)
            {
                GameObject obj2 = obj.transform.GetChild(j).gameObject;
                if (obj2.GetComponent<TextMeshProUGUI>())
                {
                    obj2.GetComponent<TextMeshProUGUI>().color = new Color(255 / 255, 255 / 255, 255 / 255, 255 / 255);
                }

                if (obj2.gameObject.name == "NewManCount")
                {
                    obj2.transform.GetChild(0).GetComponent<Image>().color = new Color(255 / 255, 255 / 255, 255 / 255, 255 / 255);
                    obj2.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(255 / 255, 255 / 255, 255 / 255, 255 / 255);
                }
            }
        }
        AfterUI_Downside.transform.GetChild(1).GetComponent<Image>().color = new Color(255 / 255, 255 / 255, 255 / 255, 255 / 255);
        AfterUI_Downside.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(255 / 255, 255 / 255, 255 / 255, 255 / 255);

        if (!RewardManager_Scr.instance.b_isEnd)
        {
            g_FadeInOut.SetActive(true);
            g_FadeInOut.GetComponent<Animator>().SetTrigger("FadeIn");
        }
        RespawnManager.instance.timer_nextwave = 10f;
        for (int i = 0; i < RespawnManager.instance.AllBlock.Count; i++)
        {
            RespawnManager.instance.AllBlock[i].SetActive(false);
        }
        for (int k = 0; k < Player_Input.instance.AllBall.Count; k++)
        {
            Player_Input.instance.AllBall[k].transform.GetComponent<Ball>().b_NaviOn = false;
        }
        if (!RewardManager_Scr.instance.b_isEnd)
            HPManager_Scr.instance.f_Hp = HPManager_Scr.instance.f_FullHp;
        Player_Input.instance.b_isAttacking = false;
        RespawnManager.instance.timer_Lock = true;
        b_WindowOn = false;
        b_Golobby = true;
        Player_Input.instance.b_resetPos = true;
        Player_Input.instance.resetRound();
        Player_Input.instance.b_OnceReturn = false;
        Cam.transform.GetComponent<Animator>().enabled = true;
        if (CamAni.GetBool("isShoot"))
        {
            CamAni.SetBool("isShoot",false);
        }
        CamAni.SetBool("isEnd", true);
        CamAni.SetBool("isStart", false);
        AfterUI_UpsideAni.SetBool("isEnd", true);
        AfterUI_UpsideAni.SetBool("isStart", false);
        AfterUI_DownsideAni.SetBool("isEnd", true);
        AfterUI_DownsideAni.SetBool("isStart", false);
        Player_Input.instance.t_Downside.GetComponent<Animator>().SetBool("isShoot", false);
        Player_Input.instance.b_Aiming = false;
        G_Enemy.SetActive(false);
        After_Pause.SetActive(false);
        b_InGame = false;
        b_isStart = false;
        G_Home.SetActive(false);
        G_VendingMachine.SetActive(false);
        ManPowerManager_Scr.instance.Coroutine_Sort();
        G_ManPower.SetActive(false);
        G_Timer.SetActive(false); // 타이머 비활성화
        g_BeforeSkill.SetActive(false); // 스킬버튼 비활성화

        StageManager_Scr.instance.i_StageLevel = StageManager_Scr.instance.i_StageClearLevel;
        Player_Input.instance.resetRound();
       // Save_Load.instance.Save();
    }

    public void OnClickAfterPauseHomeOff()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);


        G_Home.SetActive(false);
        Time.timeScale = 1;
        b_WindowOn = false;
    }

    public void OnClickSkill()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);


        go_Notapplicable.SetActive(false);
           b_isSkillActive = !b_isSkillActive;
        if (b_isSkillActive)
        {
            Time.timeScale = 0;
            G_Skill.SetActive(true);
            Player_Input.instance.b_Aiming = false;
            b_WindowOn = true;
        }
        else
        {
            GameObject obj = G_Skill.transform.GetChild(2).gameObject;
            for (int i = 0; i < obj.transform.childCount; ++i)
            {
                SkillPopUp obj1 = obj.transform.GetChild(i).GetChild(0).GetComponent<SkillPopUp>();
                obj1.b_isClick = false;
                SkillPopUp obj2 = obj.transform.GetChild(i).GetChild(4).GetComponent<SkillPopUp>();
                obj2.b_isClick = false;
            }
            if (g_SkillPopUpList.transform.GetChild(i_PopUpSkillNum).gameObject.activeSelf)
                g_SkillPopUpList.transform.GetChild(i_PopUpSkillNum).gameObject.SetActive(false);
            Player_Input.instance.b_Aiming = true;
            Time.timeScale = 1;
            G_Skill.SetActive(false);
            RespawnManager.instance.timer_Lock = false;

            b_WindowOn = false;
        }
    }

    public void OnClickManPowerON()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);

        if (!b_Attence)
        {
            cancelinvoke_attendance();
        }


        Time.timeScale = 0;
        Player_Input.instance.b_Aiming = false;
        G_ManPower.SetActive(true);
        G_ManPower.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
        ManPowerManager_Scr.instance.Coroutine_Sort();
        ManPowerManager_Scr.instance.OnManPowerWindowOn();

        b_WindowOn = true;
    }

    public void OnClickManPowerOFF()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);
        ManPowerManager_Scr.instance.Coroutine_Sort();
        G_ManPower.SetActive(false);

        ManPowerManager_Scr.instance.OnClickSellModeOFF();
        if (!b_Attence)
        {
            DelayAttendanceON();
        }
        Time.timeScale = 1;
        if (b_isStart)
        {
            Player_Input.instance.b_Aiming = true;
            RespawnManager.instance.timer_Lock = false;
        }

        b_WindowOn = false;
    }

    public void OnClickVendingMachineOn()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);


        if (!b_Attence)
        {
            cancelinvoke_attendance();
        }



        Time.timeScale = 0;
        MachineSelectManager_Scr.instance.OffMachine();

        G_VendingMachine.SetActive(true);
        Player_Input.instance.b_Aiming = false;
        b_WindowOn = true;
    }

    public void OnClickVendingMachineOff()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);


        if (!b_Attence)
        {
            DelayAttendanceON();
        }

        if (!MachineSelectManager_Scr.instance.g_SelectBuyButton.activeSelf)
        {
            G_VendingMachine.SetActive(false);
            Time.timeScale = 1;

            if (b_isStart)
            {
                Player_Input.instance.b_Aiming = true;
                RespawnManager.instance.timer_Lock = false;
            }
            MachineSelectManager_Scr.instance.OffMachine();
            b_WindowOn = false;
        }
    }
    public void OnClickShopOn()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);
        if (!b_Attence)
        {
            cancelinvoke_attendance();
        }
        Time.timeScale = 0;
        Player_Input.instance.b_Aiming = false;
        G_Shop.SetActive(true);
        G_Shop.transform.GetChild(5).gameObject.SetActive(true);
        G_Shop.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(0, 0);

        b_WindowOn = true;
    }
    public void OnClickShopOFF()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);
        if (!b_Attence)
        {
            DelayAttendanceON();
        }

        ShopManager_Scr.instance.g_ChangeWindow.SetActive(false);
        G_Shop.SetActive(false);
        Time.timeScale = 1;
        if (b_isStart)
        {
            Player_Input.instance.b_Aiming = true;
            RespawnManager.instance.timer_Lock = false;
        }
        AttendanceManager_Scr.instance.b_AutoPause = true;
        b_WindowOn = false;
    }

    public void OnClickCharacterOn()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);

        if (!b_Attence)
        {
            cancelinvoke_attendance();
        }
        if (!Player_Input.instance.b_isAttacking)
        {
            Time.timeScale = 0;
            Player_Input.instance.b_Aiming = false;
            G_CharacterShop.SetActive(true);
            G_CharacterShop.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(0, 0);

            b_WindowOn = true;
        }
    }
    public void OnClickCharacterOFF()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);

        if (!b_Attence)
        {
            DelayAttendanceON();
        }


        G_CharacterShop.SetActive(false);
        Time.timeScale = 1;
        if (b_isStart)
        {
            Player_Input.instance.b_Aiming = true;
            RespawnManager.instance.timer_Lock = false;
        }

        b_WindowOn = false;
    }

    public void LevelUpEft()
    {
        Player_Input.instance.g_ButtonShield.SetActive(false);

        Time.timeScale = 0;
        b_isLvUp = true;
        G_LevelUp.SetActive(true);
        G_LevelUp.transform.GetChild(2).transform.GetComponent<Animator>().Play("LevelUp");

        G_ManPower.GetComponent<RectTransform>().anchoredPosition = new Vector2(3000f, 0f);
        G_ManPower.SetActive(true);

        ManPowerManager_Scr.instance.Coroutine_Sort();

        G_ManPower.SetActive(false);
        G_ManPower.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 40f);


        b_WindowOn = true;
    }

    public void OnClickLevelUpBtn()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);

        if (BlockControl.b_SkillLevelUp)
            BlockControl.b_SkillLevelUp = false;
        G_LevelUp.SetActive(false);

        ManPowerManager_Scr.instance.t_MaxManCount.text = ManPowerManager_Scr.instance.i_LevelManCount.ToString();
        ManPowerManager_Scr.instance.b_isUp = false;
        if (!b_isSlowTime)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0.3f;
        }
        b_WindowOn = false;
    }

    public void OnClickMissionOn()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);

        if (!b_Attence)
        {
            cancelinvoke_attendance();
        }

        G_Mission.SetActive(true);
        Time.timeScale = 1;
        b_WindowOn = true;
    }

    public void OnClickMissionOff()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);


        if (!b_Attence)
        {
            DelayAttendanceON();
        }

        G_Mission.SetActive(false);
        Time.timeScale = 1;
        b_WindowOn = false;
    }


    public void OnClickCoinShopOn()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);


        Time.timeScale = 0;
        Player_Input.instance.b_Aiming = false;
        G_CoinShop.SetActive(true);

        b_WindowOn = true;
    }

    public void OnClickCoinShopOff()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);


        G_CoinShop.SetActive(false);
        Time.timeScale = 1;

        if (b_isStart)
        {
            Player_Input.instance.b_Aiming = true;
            //RespawnManager.instance.timer_Lock = false;
        }

        b_WindowOn = false;
    }

    public void OnClickCashShopOn()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);


        Time.timeScale = 0;
        Player_Input.instance.b_Aiming = false;
        G_GemShop.SetActive(true);

        b_WindowOn = true;

    }

    public void OnClickCashShopOff()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);


        G_GemShop.SetActive(false);
        Time.timeScale = 1;
        if (b_isStart)
        {
            Player_Input.instance.b_Aiming = true;
            //RespawnManager.instance.timer_Lock = false;
        }

        AttendanceManager_Scr.instance.b_AutoPause = true;
        b_WindowOn = false;
    }

    public void OnClickStageOn()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);


        Time.timeScale = 0;
        Player_Input.instance.b_Aiming = false;
        G_Stage.SetActive(true);

        b_WindowOn = true;
    }

    public void OnClickStageOff()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);


        G_Stage.SetActive(false);
        Time.timeScale = 1;
        if (b_isStart)
        {
            Player_Input.instance.b_Aiming = true;
            //RespawnManager.instance.timer_Lock = false;
        }
        b_WindowOn = false;
    }

    public void DelayAttendanceON()
    {
        if (AttendanceManager_Scr.instance != null && !AttendanceManager_Scr.instance.b_DontSee)
        {
            RewardManager_Scr.instance.G_ButtonShield.SetActive(false);
            Invoke("OnClickAttendanceOn", 2.0f);
            b_GoAttendanceActive = true;
        }
    }

    public void OnClickAttendanceOn()
    {
        AttendanceManager_Scr.instance.b_AutoPause = true;
        SoundManager_sfx.instance.PlaySE("Pop-Up", false);
        Time.timeScale = 0;
        Player_Input.instance.b_Aiming = false;
        G_Attendance.SetActive(true);

        b_WindowOn = true;
    }

    public void OnClickAttendanceOff()
    {
        b_Attence = true;
        b_GoAttendanceActive = false;
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);


        G_Attendance.SetActive(false);
        Time.timeScale = 1;
        if (b_isStart)
        {
            Player_Input.instance.b_Aiming = true;
            //RespawnManager.instance.timer_Lock = false;
        }
        b_WindowOn = false;

    }

    public void ReviewOn()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);


        Time.timeScale = 0;
        Player_Input.instance.b_Aiming = false;
        G_Review.SetActive(true);

        b_WindowOn = true;
    }

    public void OnClickReviewOff()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);


        G_Review.SetActive(false);
        Time.timeScale = 1;
        if (b_isStart)
        {
            Player_Input.instance.b_Aiming = true;
            //RespawnManager.instance.timer_Lock = false;
        }
        b_WindowOn = false;
    }
    public void OnClickInventoryOn()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);
        if (!b_Attence)
        {
            cancelinvoke_attendance();
        }

        if (!TutorialManager.instance.b_BagTutorial)
        {
            TutorialManager.instance.G_Bag_Tutorial.SetActive(true);
        }
        else
        {


            Time.timeScale = 0;
            Player_Input.instance.b_Aiming = false;
            Inventory_Scr.instance.SoldGold = 0;
            InventoryCaching_Scr.instance.itemList.GetComponent<RectTransform>().anchoredPosition = new Vector2(60f, -10f);
            InventoryCaching_Scr.instance.itemList.GetComponent<RectTransform>().sizeDelta = new Vector2(700f, 700f);
            InventoryCaching_Scr.instance.itemList.gameObject.SetActive(true);
            G_Inventory.SetActive(true);
            b_WindowOn = true;
        }
    }

    public void OnClickInventoryOff()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);

        if (!b_Attence)
        {
            DelayAttendanceON();
        }
        G_Inventory.SetActive(false);
        InventoryCaching_Scr.instance.itemList.gameObject.SetActive(false);
        Inventory_Scr.instance.ClearList();
        Time.timeScale = 1;
        if (b_isStart)
        {
            Player_Input.instance.b_Aiming = true;
            //RespawnManager.instance.timer_Lock = false;
        }
        b_WindowOn = false;
    }

    public void OnClickSynthesisOn()
    {
        if (!TutorialManager.instance.b_SynthesisTutorial)
        {
            TutorialManager.instance.G_Synthesis_Tutorial.SetActive(true);
        }


        if (!b_Attence)
        {
            cancelinvoke_attendance();
        }


        SoundManager_sfx.instance.PlaySE("UI_Click1", false);

        //Time.timeScale = 0;
        Player_Input.instance.b_Aiming = false;
        // 히어로탭을 먼저 열어줍시다.

        SynthesisCaching_Scr.instance.heroTab.GetComponent<Image>().color = new Color(155f / 255f, 155f / 255f, 155f / 255f, 255f / 255f);
        SynthesisCaching_Scr.instance.heroTab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
        SynthesisCaching_Scr.instance.itemTab.GetComponent<Image>().color = new Color(65f / 255f, 65f / 255f, 65f / 255f, 255f / 255f);
        SynthesisCaching_Scr.instance.itemTab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(155f / 255f, 155f / 255f, 155f / 255f, 255f / 255f);
        SynthesisCaching_Scr.instance.SynthesisMode = true;
        Synthesis_Scr.instance.readytoSynthesisEffect();
        InventoryCaching_Scr.instance.heroList.GetComponent<RectTransform>().anchoredPosition = new Vector2(60f, -190f);
        InventoryCaching_Scr.instance.heroList.GetComponent<RectTransform>().sizeDelta = new Vector2(700f, 490f);
        if (Synthesis_Scr.instance.itemListTapOpen)
        {
            SynthesisCaching_Scr.instance.itemTab.GetComponent<Image>().color = new Color(155f / 255f, 155f / 255f, 155f / 255f, 255f / 255f);
            SynthesisCaching_Scr.instance.itemTab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            SynthesisCaching_Scr.instance.heroTab.GetComponent<Image>().color = new Color(65f / 255f, 65f / 255f, 65f / 255f, 255f / 255f);
            SynthesisCaching_Scr.instance.heroTab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(155f / 255f, 155f / 255f, 155f / 255f, 255f / 255f);
            InventoryCaching_Scr.instance.itemList.GetComponent<RectTransform>().anchoredPosition = new Vector2(60f, -190f);
            InventoryCaching_Scr.instance.itemList.GetComponent<RectTransform>().sizeDelta = new Vector2(700f, 490f);
            InventoryCaching_Scr.instance.heroList.gameObject.SetActive(false);
            InventoryCaching_Scr.instance.itemList.gameObject.SetActive(true);
        }
        else if(!Synthesis_Scr.instance.itemListTapOpen)
        {
            SynthesisCaching_Scr.instance.heroTab.GetComponent<Image>().color = new Color(155f / 255f, 155f / 255f, 155f / 255f, 255f / 255f);
            SynthesisCaching_Scr.instance.heroTab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            SynthesisCaching_Scr.instance.itemTab.GetComponent<Image>().color = new Color(65f / 255f, 65f / 255f, 65f / 255f, 255f / 255f);
            SynthesisCaching_Scr.instance.itemTab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(155f / 255f, 155f / 255f, 155f / 255f, 255f / 255f);

            InventoryCaching_Scr.instance.heroList.GetComponent<RectTransform>().anchoredPosition = new Vector2(60f, -190f);
            InventoryCaching_Scr.instance.heroList.GetComponent<RectTransform>().sizeDelta = new Vector2(700f, 490f);
            InventoryCaching_Scr.instance.heroList.gameObject.SetActive(true);
            InventoryCaching_Scr.instance.itemList.gameObject.SetActive(false);
        }

        G_Synthesis.SetActive(true);
        b_WindowOn = true;
    }

    public void OnClickSynthesisOff()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);


        if (!b_Attence)
        {
            DelayAttendanceON();
        }
        Synthesis_Scr.instance.ClearList(false);
        SynthesisCaching_Scr.instance.SynthesisMode = false;
        InventoryCaching_Scr.instance.itemList.gameObject.SetActive(false);
        InventoryCaching_Scr.instance.heroList.gameObject.SetActive(false);
        G_Synthesis.SetActive(false);
        Time.timeScale = 1;
        if (b_isStart)
        {
            Player_Input.instance.b_Aiming = true;
            //RespawnManager.instance.timer_Lock = false;
        }
        b_WindowOn = false;
    }

    public void OnClickSynthesisEffectOff()
    {
        SynthesisCaching_Scr.instance.SynthesisEffect.SetActive(false);
        DeliveryBoxManager_Scr.instance.g_PartipleCanvas.SetActive(false);
        SynthesisCaching_Scr.instance.SynthesisEffect.transform.GetComponent<SynthesisEffectChasing>().g_heroInfo.SetActive(false);
        SynthesisCaching_Scr.instance.SynthesisEffect.transform.GetComponent<SynthesisEffectChasing>().g_itemInfo.SetActive(false);
        OnClickSynthesisOn();
    }

    public void Warning()
    {
        if(!QuitManager_Scr.instance.b_BossScene)
            t_Warning.SetActive(true);
    }

    public void SkillPopUpOn(int _num)
    {
        b_isPopUpOn = true;
        i_PopUpSkillNum = _num;
        g_SkillPopUpList.transform.GetChild(_num).gameObject.SetActive(true);
    }

    public void OnSkillPopUpOff(int _num)
    {
        g_SkillPopUpList.transform.GetChild(_num).gameObject.SetActive(false);
    }

    public void TranslateUI()
    {
        tmpTime = Time.unscaledTime;
        StartCoroutine(TranslateUIOff());
    }

    IEnumerator TranslateUIOff()
    {
        while (true)
        {
            float tmp = Time.unscaledTime;
            if(tmp - tmpTime > 0.1f)
            {
                StopCoroutine(TranslateUIOff());
                TranslateUIs();
                break;
            }
            yield return null;
        }
    }

    public void TranslateUIs()
    {
        RectTransform Pos = G_ManPower.GetComponent<RectTransform>();
        G_ManPower.SetActive(false);
        Pos.anchoredPosition = new Vector2(0, 40);
        G_ManPower.GetComponent<RectTransform>().anchoredPosition = Pos.anchoredPosition;

        Pos = G_VendingMachine.GetComponent<RectTransform>();
        G_VendingMachine.SetActive(false);
        Pos.anchoredPosition = new Vector2(0, 40);
        G_VendingMachine.GetComponent<RectTransform>().anchoredPosition = Pos.anchoredPosition;

        Pos = G_CoinShop.GetComponent<RectTransform>();
        G_CoinShop.SetActive(false);
        Pos.anchoredPosition = new Vector2(0, 80);
        G_CoinShop.GetComponent<RectTransform>().anchoredPosition = Pos.anchoredPosition;

        Pos = G_GemShop.GetComponent<RectTransform>();
        G_GemShop.SetActive(false);
        Pos.anchoredPosition = new Vector2(0, 80);
        G_GemShop.GetComponent<RectTransform>().anchoredPosition = Pos.anchoredPosition;

        Pos = G_CharacterShop.GetComponent<RectTransform>();
        G_CharacterShop.SetActive(false);
        Pos.anchoredPosition = new Vector2(0, 40);
        G_CharacterShop.GetComponent<RectTransform>().anchoredPosition = Pos.anchoredPosition;

        Pos = G_Shop.GetComponent<RectTransform>();
        G_Shop.SetActive(false);
        Pos.anchoredPosition = new Vector2(0, 26);
        G_Shop.GetComponent<RectTransform>().anchoredPosition = Pos.anchoredPosition;

        Pos = G_Skill.GetComponent<RectTransform>();
        G_Skill.SetActive(false);
        Pos.anchoredPosition = new Vector2(0, 80);
        G_Skill.GetComponent<RectTransform>().anchoredPosition = Pos.anchoredPosition;

        Pos = G_Mission.GetComponent<RectTransform>();
        G_Mission.SetActive(false);
        Pos.anchoredPosition = new Vector2(0, 40);
        G_Mission.GetComponent<RectTransform>().anchoredPosition = Pos.anchoredPosition;
    }
    
    /*
    public void TestHide()
    {
        this.gameObject.SetActive(false);
        Invoke("TestShow", 5f);
    }

    public void TestShow()
    {
        this.gameObject.SetActive(true);
    }

    public void testdwon()
    {
        Save_Load.instance.Datadownload();
    }
    */
}