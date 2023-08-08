using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager_Scr : MonoBehaviour
{
    public static StageManager_Scr instance;

    public GameObject Q_Stage; 
    public GameObject G_TimerBar;

    public Image i_AbilityBar;
    public TextMeshProUGUI t_LobbyStageLevel;
    public TextMeshProUGUI t_InGameStageLevel;
    public TextMeshProUGUI t_InGameStageName;

    [SerializeField]
    private float f_Ability;
    [SerializeField]
    private float f_needAbility = 100f;
    [SerializeField]
    private float f_PlusAbility = 0f;

    public int i_StageLevel;
    public int i_StageClearLevel = 1;
    public int i_GoLevel;

    public int i_StageRound;
    public int i_StageReset;
    public int i_QuarterLevel;
    public int i_BGIdx = 2;
    public int i_TileIdx = 0;

    public bool b_isLoaded = true;
    public bool b_isRanking = false;
    public bool[] b_StageChange = new bool[10];

    public GameObject[] obj;
    public GameObject g_BG;
    public Material[] tiles;
    
    public float myAbility { get => f_Ability; set => f_Ability = value; }
    public float needAbility { get => f_needAbility; }
    public float plusAbility { get => f_PlusAbility; set => f_PlusAbility = value; }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            return;


    }

    public void StageNameChecker()
    {
        StartCoroutine(ChangeStageName());
    }

    public void readytoDelay()
    {
        Invoke("StartingCoroutine", 1f);
    }

    void StartingCoroutine()
    {
        StartCoroutine(UpdateCoroutine());
    }

    IEnumerator ChangeStageName()
    {
        WaitForEndOfFrame delay = new WaitForEndOfFrame();
        while (true)
        {
            if (i_StageLevel % 200 >= 1 && i_StageLevel % 200 < 21)
            {
                if (TranslateManager_Scr.instance.b_isKorean || TranslateManager_Scr.instance.b_isChinese || TranslateManager_Scr.instance.b_isJapanese)
                    t_InGameStageName.text = TranslateManager_Scr.instance.TranslateContext(121);
                else
                    t_InGameStageName.text = TranslateManager_Scr.instance.TranslateContext(121, "English");
            }

            else if (i_StageLevel % 200 >= 21 && i_StageLevel % 200 < 41)
            {
                if (TranslateManager_Scr.instance.b_isKorean || TranslateManager_Scr.instance.b_isChinese || TranslateManager_Scr.instance.b_isJapanese)
                    t_InGameStageName.text = TranslateManager_Scr.instance.TranslateContext(122);
                else
                    t_InGameStageName.text = TranslateManager_Scr.instance.TranslateContext(122, "English");
            }

            else if (i_StageLevel % 200 >= 41 && i_StageLevel % 200 < 61)
            {
                if (TranslateManager_Scr.instance.b_isKorean || TranslateManager_Scr.instance.b_isChinese || TranslateManager_Scr.instance.b_isJapanese)
                    t_InGameStageName.text = TranslateManager_Scr.instance.TranslateContext(123);
                else
                    t_InGameStageName.text = TranslateManager_Scr.instance.TranslateContext(123, "English");
            }

            else if (i_StageLevel % 200 >= 61 && i_StageLevel % 200 < 81)
            {
                if (TranslateManager_Scr.instance.b_isKorean || TranslateManager_Scr.instance.b_isChinese || TranslateManager_Scr.instance.b_isJapanese)
                    t_InGameStageName.text = TranslateManager_Scr.instance.TranslateContext(124);
                else
                    t_InGameStageName.text = TranslateManager_Scr.instance.TranslateContext(124, "English");
            }

            else if (i_StageLevel % 200 >= 81 && i_StageLevel % 200 < 101)
            {
                if (TranslateManager_Scr.instance.b_isKorean || TranslateManager_Scr.instance.b_isChinese || TranslateManager_Scr.instance.b_isJapanese)
                    t_InGameStageName.text = TranslateManager_Scr.instance.TranslateContext(125);
                else
                    t_InGameStageName.text = TranslateManager_Scr.instance.TranslateContext(125, "English");
            }

            else if (i_StageLevel % 200 >= 101 && i_StageLevel % 200 < 121)
            {
                if (TranslateManager_Scr.instance.b_isKorean || TranslateManager_Scr.instance.b_isChinese || TranslateManager_Scr.instance.b_isJapanese)
                    t_InGameStageName.text = TranslateManager_Scr.instance.TranslateContext(126);
                else
                    t_InGameStageName.text = TranslateManager_Scr.instance.TranslateContext(126, "English");
            }

            else if (i_StageLevel % 200 >= 121 && i_StageLevel % 200 < 141)
            {
                if (TranslateManager_Scr.instance.b_isKorean || TranslateManager_Scr.instance.b_isChinese || TranslateManager_Scr.instance.b_isJapanese)
                    t_InGameStageName.text = TranslateManager_Scr.instance.TranslateContext(127);
                else
                    t_InGameStageName.text = TranslateManager_Scr.instance.TranslateContext(127, "English");
            }

            else if (i_StageLevel % 200 >= 141 && i_StageLevel % 200 < 161)
            {
                if (TranslateManager_Scr.instance.b_isKorean || TranslateManager_Scr.instance.b_isChinese || TranslateManager_Scr.instance.b_isJapanese)
                    t_InGameStageName.text = TranslateManager_Scr.instance.TranslateContext(128);
                else
                    t_InGameStageName.text = TranslateManager_Scr.instance.TranslateContext(128, "English");
            }

            else if (i_StageLevel % 200 >= 161 && i_StageLevel % 200 < 181)
            {
                if (TranslateManager_Scr.instance.b_isKorean || TranslateManager_Scr.instance.b_isChinese || TranslateManager_Scr.instance.b_isJapanese)
                    t_InGameStageName.text = TranslateManager_Scr.instance.TranslateContext(129);
                else
                    t_InGameStageName.text = TranslateManager_Scr.instance.TranslateContext(129, "English");
            }

            else if (i_StageLevel % 200 >= 181 && i_StageLevel % 200 == 0)
            {
                if (TranslateManager_Scr.instance.b_isKorean || TranslateManager_Scr.instance.b_isChinese || TranslateManager_Scr.instance.b_isJapanese)
                    t_InGameStageName.text = TranslateManager_Scr.instance.TranslateContext(130);
                else
                    t_InGameStageName.text = TranslateManager_Scr.instance.TranslateContext(130, "English");
            }


            yield return delay;
        }
    }

    IEnumerator UpdateCoroutine()
    {
        WaitForEndOfFrame delay = new WaitForEndOfFrame();
        while (true)
        {
            if (RespawnManager.instance.b_NomalGame)
            {
                f_needAbility = Convert.ToSingle(RespawnManager.instance.RespawnData[i_StageLevel - 1]["Ability"]);
            }
            else
            {
                f_needAbility = 0;
                f_needAbility = f_PlusAbility;
            }

            GameObject Icon_Skill = Button_Option.instance.g_DownSideGroup.transform.GetChild(0).GetChild(0).gameObject;
            //var c = Color.HSVToRGB(0, 0, 0.5f + Mathf.PingPong(Time.time * 0.5f, 1));
            //Icon_Skill.GetComponent<Image>().color = c;
            Icon_Skill.GetComponent<Image>().fillAmount = f_Ability / f_needAbility;

            i_AbilityBar.fillAmount = f_Ability / f_needAbility;

            if (Icon_Skill.GetComponent<Image>().fillAmount == 1)
            {
                Icon_Skill.transform.parent.GetComponent<Button>().enabled = true;
            }
            else
            {
                Icon_Skill.transform.parent.GetComponent<Button>().enabled = false;
            }

            if (!Button_Option.instance.G_Stage.activeSelf)
            {
                float value = 1 - i_StageClearLevel / 1000f + 0.005f;
                Button_Option.instance.G_Stage.transform.GetChild(2).GetComponent<ScrollRect>().verticalNormalizedPosition = value;
                Button_Option.instance.G_Stage.transform.GetChild(2).GetChild(1).GetComponent<Scrollbar>().value = value;
            }

            if (RespawnManager.instance != null)
            {
                if (Button_Option.instance.b_Golobby || RespawnManager.instance.b_NomalGame)
                {
                    if (Button_Option.instance.G_Timer.activeSelf == false)
                    {
                        Button_Option.instance.G_Timer.SetActive(true);
                        Button_Option.instance.g_StageBG.SetActive(true);
                        Button_Option.instance.g_RankPoint.SetActive(false);
                        if (i_StageLevel != 1000)
                        {
                            t_LobbyStageLevel.text = t_InGameStageLevel.text = i_StageLevel.ToString();
                        }
                        else
                        {
                            t_LobbyStageLevel.text = t_InGameStageLevel.text = "MAX";
                        }
                    }
                }
                else
                {
                    if (Button_Option.instance.G_Timer.activeSelf == true)
                    {
                        Button_Option.instance.G_Timer.SetActive(false);
                        Button_Option.instance.g_StageBG.SetActive(false);
                        Button_Option.instance.g_RankPoint.SetActive(true);
                    }
                }
            }
            yield return delay;
        }
    }

    // 저장되어있는 정보에 따른 타일 및 씬 변경 -> x
    public void LoadBG()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        b_isLoaded = false;
        Invoke("BGChange", 0.3f);
    }

    public void RankingModeBG()
    {
        b_isRanking = true;
        for (int i = 0; i < b_StageChange.Length; ++i)
            b_StageChange[i] = false;

        if(SceneManager.GetSceneByBuildIndex(i_BGIdx).isLoaded)
            SceneManager.UnloadSceneAsync(i_BGIdx);

        if (!SceneManager.GetSceneByBuildIndex(12).isLoaded)
            SceneManager.LoadScene(12, LoadSceneMode.Additive);
        
        System.GC.Collect();    // 씬 로딩이 끝난고 난 후 가비지 컬렉터를 수행하도록 설정
        Invoke("RankingModeTile", 0.1f);
    }

    public void RankingModeTile()
    {
        obj = SceneManager.GetSceneByBuildIndex(12).GetRootGameObjects();
        obj[0].transform.GetChild(7).GetComponent<MeshRenderer>().material = tiles[4];
    }

    // 스테이지 진행에 따른 타일 및 씬 변경
    public void BGChange()
    {
        b_isLoaded = false;
        if (i_StageLevel % 200 >= 1 && i_StageLevel % 200 < 21 && !b_StageChange[0])
        {
            for (int i = 0; i < b_StageChange.Length; ++i)
                b_StageChange[i] = false;
            i_BGIdx = 2;
            b_StageChange[0] = true;
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
        }

        else if (i_StageLevel % 200 >= 21 && i_StageLevel % 200 < 41 && !b_StageChange[1])
        {
            for (int i = 0; i < b_StageChange.Length; ++i)
                b_StageChange[i] = false;
            i_BGIdx = 3;
            b_StageChange[1] = true;
            SceneManager.LoadScene(3, LoadSceneMode.Additive);
        }

        else if (i_StageLevel % 200 >= 41 && i_StageLevel % 200 < 61 && !b_StageChange[2])
        {
            for (int i = 0; i < b_StageChange.Length; ++i)
                b_StageChange[i] = false;
            i_BGIdx = 4;
            b_StageChange[2] = true;
            SceneManager.LoadScene(4, LoadSceneMode.Additive);
        }

        else if (i_StageLevel % 200 >= 61 && i_StageLevel % 200 < 81 && !b_StageChange[3])
        {
            for (int i = 0; i < b_StageChange.Length; ++i)
                b_StageChange[i] = false;
            i_BGIdx = 5;
            b_StageChange[3] = true;
            SceneManager.LoadScene(5, LoadSceneMode.Additive);
        }

        else if (i_StageLevel % 200 >= 81 && i_StageLevel % 200 < 101 && !b_StageChange[4])
        {
            for (int i = 0; i < b_StageChange.Length; ++i)
                b_StageChange[i] = false;
            i_BGIdx = 6;
            b_StageChange[4] = true;
            SceneManager.LoadScene(6, LoadSceneMode.Additive);
        }

        else if (i_StageLevel % 200 >= 101 && i_StageLevel % 200 < 121 && !b_StageChange[5])
        {
            for (int i = 0; i < b_StageChange.Length; ++i)
                b_StageChange[i] = false;
            i_BGIdx = 7;
            b_StageChange[5] = true;
            SceneManager.LoadScene(7, LoadSceneMode.Additive);
        }

        else if (i_StageLevel % 200 >= 121 && i_StageLevel % 200 < 141 && !b_StageChange[6])
        {
            for (int i = 0; i < b_StageChange.Length; ++i)
                b_StageChange[i] = false;
            i_BGIdx = 8;
            b_StageChange[6] = true;
            SceneManager.LoadScene(8, LoadSceneMode.Additive);
        }

        else if (i_StageLevel % 200 >= 141 && i_StageLevel % 200 < 161 && !b_StageChange[7])
        {
            for (int i = 0; i < b_StageChange.Length; ++i)
                b_StageChange[i] = false;
            i_BGIdx = 9;
            b_StageChange[7] = true;
            SceneManager.LoadScene(9, LoadSceneMode.Additive);
        }

        else if (i_StageLevel % 200 >= 161 && i_StageLevel % 200 < 181 && !b_StageChange[8])
        {
            for (int i = 0; i < b_StageChange.Length; ++i)
                b_StageChange[i] = false;
            i_BGIdx = 10;
            b_StageChange[8] = true;
            SceneManager.LoadScene(10, LoadSceneMode.Additive);
        }

        else if (i_StageLevel % 200 >= 181 && i_StageLevel % 200 == 0 && !b_StageChange[9])
        {
            for (int i = 0; i < b_StageChange.Length; ++i)
                b_StageChange[i] = false;
            i_BGIdx = 11;
            b_StageChange[9] = true;
            SceneManager.LoadScene(11, LoadSceneMode.Additive);
        }

        // 열린 씬이 있으면 없애줌
        for (int i = 2; i < 12; ++i)
        {
            if (i != i_BGIdx && SceneManager.GetSceneByBuildIndex(i).isLoaded)
                SceneManager.UnloadSceneAsync(i);
        }

        if (b_isRanking)
        {
            if (SceneManager.GetSceneByBuildIndex(12).isLoaded)
                SceneManager.UnloadSceneAsync(12);
            b_isRanking = false;
        }

        System.GC.Collect();    // 씬 로딩이 끝난고 난 후 가비지 컬렉터를 수행하도록 설정
        Invoke("TileChange", 0.1f);
    }

    public void TileChange()
    {
        obj = SceneManager.GetSceneByBuildIndex(i_BGIdx).GetRootGameObjects();
        for (int i = 0; i < obj.Length; ++i)
        {
            if (obj[i].name == "G.LevelArt")
            {
                for (int j = 0; j < obj[i].transform.childCount; ++j)
                {
                    if (obj[i].transform.GetChild(j).name == "BGbase")
                    {
                        g_BG = obj[i].transform.GetChild(j).gameObject;
                    }
                }
            }
        }
        if (i_StageLevel <= 200 && g_BG.GetComponent<MeshRenderer>().material != tiles[0])
        {
            i_TileIdx = 0;
        }
        else if (i_StageLevel > 200 && i_StageLevel <= 400 && g_BG.GetComponent<MeshRenderer>().material != tiles[1])
        {
            i_TileIdx = 1;
        }
        else if (i_StageLevel > 400 && i_StageLevel <= 600 && g_BG.GetComponent<MeshRenderer>().material != tiles[2])
        {
            i_TileIdx = 2;
        }
        else if (i_StageLevel > 600 && i_StageLevel <= 800 && g_BG.GetComponent<MeshRenderer>().material != tiles[3])
        {
            i_TileIdx = 3;
        }
        else if (i_StageLevel > 800 && i_StageLevel <= 1000 && g_BG.GetComponent<MeshRenderer>().material != tiles[4])
        {
            i_TileIdx = 4;
        }

        if (i_TileIdx == 0 && g_BG != tiles[0])
            g_BG.GetComponent<MeshRenderer>().material = tiles[0];
        else if (i_TileIdx == 1 && g_BG != tiles[1])
            g_BG.GetComponent<MeshRenderer>().material = tiles[1];
        else if (i_TileIdx == 2 && g_BG != tiles[2])
            g_BG.GetComponent<MeshRenderer>().material = tiles[2];
        else if (i_TileIdx == 3 && g_BG != tiles[3])
            g_BG.GetComponent<MeshRenderer>().material = tiles[3];
        else if (i_TileIdx == 4 && g_BG != tiles[4])
            g_BG.GetComponent<MeshRenderer>().material = tiles[4];
    }

    public void OnClickGoLevelYes()
    {
        if(ScenarioEventManager_Scr.instance.i_EventNum != 0)
        {
            ScenarioEventManager_Scr.instance.DisableEvent();
        }
        i_StageLevel = i_GoLevel;
        RespawnManager.instance.ChangeStageRound(i_StageLevel);
        t_LobbyStageLevel.text = t_InGameStageLevel.text = i_StageLevel.ToString();
        if (!Button_Option.instance.b_InGame)
            Button_Option.instance.OnClickStart(true);

        if (Button_Option.instance.b_InGame)
        {
            SoundManager_sfx.instance.PlaySE("UI_Click1", false);
            for (int i = 0; i < RespawnManager.instance.AllBlock.Count; i++)
            {
                RespawnManager.instance.AllBlock[i].SetActive(false);
            }
            for (int k = 0; k < Player_Input.instance.AllBall.Count; k++)
            {
                Player_Input.instance.AllBall[k].transform.GetComponent<Ball>().b_NaviOn = false;
            }
            RespawnManager.instance.timer_Lock = true;
            Player_Input.instance.b_isAttacking = false;
            Player_Input.instance.b_resetPos = true;
            Player_Input.instance.resetRound();
            Player_Input.instance.b_OnceReturn = false;
            Player_Input.instance.b_Aiming = false;
            //Player_Input.instance.t_Downside.GetComponent<Animator>().SetBool("isShoot", false);
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
            Time.timeScale = 1;
            Button_Option.instance.g_FadeInOut.SetActive(true);

            Button_Option.instance.g_FadeInOut.GetComponent<Animator>().SetTrigger("FadeOut");

            if (Button_Option.instance.G_Stage.activeSelf)
                Button_Option.instance.G_Stage.SetActive(false);
            BGChange();
        }

        Q_Stage.SetActive(false);
        Button_Option.instance.G_Stage.SetActive(false);
    }
    public void OnClickGoLevelNo()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);
        Q_Stage.SetActive(false);
    }

    int test;
    public void OnClickTest(int _num)
    {
        test = _num + 2;
        for (int i = 0; i < b_StageChange.Length; ++i)
            b_StageChange[i] = false;

        for (int i = 2; i < 12; ++i)
        {
            if (SceneManager.GetSceneByBuildIndex(i).isLoaded)
                SceneManager.UnloadSceneAsync(i);
        }

        b_StageChange[_num] = true;
        SceneManager.LoadScene(test, LoadSceneMode.Additive);

        Invoke("TestTile", 0.1f);
    }

    public void TestTile()
    {
        obj = SceneManager.GetSceneByBuildIndex(test).GetRootGameObjects();
        for (int i = 0; i < obj.Length; ++i)
        {
            if (obj[i].name == "G.LevelArt")
            {
                for (int j = 0; j < obj[i].transform.childCount; ++j)
                {
                    if (obj[i].transform.GetChild(j).name == "BGbase")
                    {
                        g_BG = obj[i].transform.GetChild(j).gameObject;
                    }
                }
            }
        }
        if (i_StageLevel <= 200 && g_BG.GetComponent<MeshRenderer>().material != tiles[0])
        {
            i_TileIdx = 0;
        }
        else if (i_StageLevel > 200 && i_StageLevel <= 400 && g_BG.GetComponent<MeshRenderer>().material != tiles[1])
        {
            i_TileIdx = 1;
        }
        else if (i_StageLevel > 400 && i_StageLevel <= 600 && g_BG.GetComponent<MeshRenderer>().material != tiles[2])
        {
            i_TileIdx = 2;
        }
        else if (i_StageLevel > 600 && i_StageLevel <= 800 && g_BG.GetComponent<MeshRenderer>().material != tiles[3])
        {
            i_TileIdx = 3;
        }
        else if (i_StageLevel > 800 && i_StageLevel <= 1000 && g_BG.GetComponent<MeshRenderer>().material != tiles[4])
        {
            i_TileIdx = 4;
        }

        if (i_TileIdx == 0 && g_BG != tiles[0])
            g_BG.GetComponent<MeshRenderer>().material = tiles[0];
        else if (i_TileIdx == 1 && g_BG != tiles[1])
            g_BG.GetComponent<MeshRenderer>().material = tiles[1];
        else if (i_TileIdx == 2 && g_BG != tiles[2])
            g_BG.GetComponent<MeshRenderer>().material = tiles[2];
        else if (i_TileIdx == 3 && g_BG != tiles[3])
            g_BG.GetComponent<MeshRenderer>().material = tiles[3];
        else if (i_TileIdx == 4 && g_BG != tiles[4])
            g_BG.GetComponent<MeshRenderer>().material = tiles[4];
    }
}
