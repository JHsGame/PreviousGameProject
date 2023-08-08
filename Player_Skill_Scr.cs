using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_Skill_Scr : MonoBehaviour
{
    public static Player_Skill_Scr instance;
    public GameObject go_PlayerBase;
    public GameObject go_Enemy;
    public GameObject go_ICon;
    public GameObject go_SkillExplanation;
    //public GameObject go_Skillcam;
    public string[] s_SkillsfxName = new string[4];
    public Animator myAni;
    public int useSkillNum;
    public TextAsset t_AmbassadorCSV;
    public TextAsset t_Script_SkillCSV;
    public TextAsset t_TranslateCSV;
    public List<Dictionary<string, object>> AmbassadorData;
    public List<Dictionary<string, object>> Script_SkillData;
    public List<Dictionary<string, object>> TranslateData;
    public string str;
    string[] split;

    private void Awake()
    {
        //AmbassadorData = CSVReader.Read(t_AmbassadorCSV);
        Script_SkillData = CSVReader.Read(t_Script_SkillCSV);
        TranslateData = CSVReader.Read(t_TranslateCSV);
    }

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            return;

        myAni = transform.GetComponent<Animator>();
    }

    public void FirstSpeech()
    {
        switch (useSkillNum)
        {
            case 0:
                SoundManager_sfx.instance.PlaySE("Skill_Software_1", false);

                Button_Option.instance.g_SkillSpeech.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text
                    = Script_SkillData[useSkillNum][TranslateManager_Scr.instance.s_Language].ToString();
                /*
                if (TranslateManager_Scr.instance.b_isKorean)
                {
                    Button_Option.instance.g_SkillSpeech.transform.GetChild(0).GetChild(0).GetComponent<Text>().text
                        = Script_SkillData[useSkillNum]["Korean"].ToString();
                }
                else if (TranslateManager_Scr.instance.b_isEnglish)
                {
                    Button_Option.instance.g_SkillSpeech.transform.GetChild(0).GetChild(0).GetComponent<Text>().text
                        = Script_SkillData[useSkillNum]["English"].ToString();
                }
                else if (TranslateManager_Scr.instance.b_isJapanese)
                {
                    Button_Option.instance.g_SkillSpeech.transform.GetChild(0).GetChild(0).GetComponent<Text>().text
                        = Script_SkillData[useSkillNum]["Japanese"].ToString();
                }
                else if (TranslateManager_Scr.instance.b_isChinese)
                {
                    Button_Option.instance.g_SkillSpeech.transform.GetChild(0).GetChild(0).GetComponent<Text>().text
                        = Script_SkillData[useSkillNum]["Chinese"].ToString();
                }*/

                break;

            case 1:
                SoundManager_sfx.instance.PlaySE("Skill_Tax_1", false);
                if (TranslateManager_Scr.instance.b_isEnglish)
                {
                    Button_Option.instance.g_SkillSpeech.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text
                        = Script_SkillData[2]["English"].ToString().Substring(0, 26) + "\n" + Script_SkillData[2]["English"].ToString().Substring(27, 19);
                }
                else
                {
                    Button_Option.instance.g_SkillSpeech.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text
                        = Script_SkillData[2][TranslateManager_Scr.instance.s_Language].ToString();
                }
                /*
                if (TranslateManager_Scr.instance.b_isKorean)
                {
                    Button_Option.instance.g_SkillSpeech.transform.GetChild(0).GetChild(0).GetComponent<Text>().text
                        = Script_SkillData[2]["Korean"].ToString();
                }
                else if (TranslateManager_Scr.instance.b_isEnglish)
                {
                    Button_Option.instance.g_SkillSpeech.transform.GetChild(0).GetChild(0).GetComponent<Text>().text
                        = Script_SkillData[2]["English"].ToString().Substring(0, 26) + "\n" + Script_SkillData[2]["English"].ToString().Substring(27, 19);
                }
                else if (TranslateManager_Scr.instance.b_isJapanese)
                {
                    Button_Option.instance.g_SkillSpeech.transform.GetChild(0).GetChild(0).GetComponent<Text>().text
                        = Script_SkillData[2]["Japanese"].ToString();
                }
                else if (TranslateManager_Scr.instance.b_isChinese)
                {
                    Button_Option.instance.g_SkillSpeech.transform.GetChild(0).GetChild(0).GetComponent<Text>().text
                        = Script_SkillData[2]["Chinese"].ToString();
                }*/

                break;

            case 2:
                SoundManager_sfx.instance.PlaySE("Skill_Police_1", false);
                if (TranslateManager_Scr.instance.b_isEnglish)
                {
                    Button_Option.instance.g_SkillSpeech.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text
                        = Script_SkillData[4]["English"].ToString().Substring(0, 32) + "\n" + Script_SkillData[4]["English"].ToString().Substring(33, 19);
                }
                else
                {
                    Button_Option.instance.g_SkillSpeech.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text
                        = Script_SkillData[4][TranslateManager_Scr.instance.s_Language].ToString();
                }

                break;

            case 3:
                SoundManager_sfx.instance.PlaySE("Skill_SCV_1", false);
                if (TranslateManager_Scr.instance.b_isEnglish)
                {
                    Button_Option.instance.g_SkillSpeech.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text
                        = Script_SkillData[6]["English"].ToString().Substring(0, 26) + "\n" + Script_SkillData[6]["English"].ToString().Substring(27, 19);
                }
                else
                {
                    Button_Option.instance.g_SkillSpeech.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text
                        = Script_SkillData[6][TranslateManager_Scr.instance.s_Language].ToString();
                }

                break;
        }
    }

    public void SecondSpeech()
    {
        switch (useSkillNum)
        {
            case 0:
                SoundManager_sfx.instance.PlaySE("Skill_Software_2", false);
                Button_Option.instance.g_SkillSpeech.transform.GetChild(1).gameObject.SetActive(true);

                str = Script_SkillData[1][TranslateManager_Scr.instance.s_Language].ToString();
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
                Button_Option.instance.g_SkillSpeech.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = str;

              /*  if (TranslateManager_Scr.instance.b_isJapanese)
                {
                    
                        = Script_SkillData[1]["Japanese"].ToString().Substring(0, 6) + "\n" + Script_SkillData[1]["Japanese"].ToString().Substring(7, 21);
                }
                else 
                {
                    Button_Option.instance.g_SkillSpeech.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>().text
                        = Script_SkillData[1][TranslateManager_Scr.instance.s_Language].ToString();
                }*/
                break;

            case 1:
                SoundManager_sfx.instance.PlaySE("Skill_Tax_2", false);
                Button_Option.instance.g_SkillSpeech.transform.GetChild(1).gameObject.SetActive(true);

                str = Script_SkillData[3][TranslateManager_Scr.instance.s_Language].ToString();
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
                Button_Option.instance.g_SkillSpeech.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = str;

                /*
                if (TranslateManager_Scr.instance.b_isEnglish)
                {
                    Button_Option.instance.g_SkillSpeech.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>().text
                        = Script_SkillData[3]["English"].ToString().Substring(0, 26) + "\n" + Script_SkillData[3]["English"].ToString().Substring(27, 29);
                }
                else if (TranslateManager_Scr.instance.b_isJapanese)
                {
                    str = Script_SkillData[3]["Japanese"].ToString();
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
                    Button_Option.instance.g_SkillSpeech.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>().text
                        = str;
                }
                else
                {
                    Button_Option.instance.g_SkillSpeech.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>().text
                        = Script_SkillData[3][TranslateManager_Scr.instance.s_Language].ToString();
                }*/
                break;

            case 2:
                SoundManager_sfx.instance.PlaySE("Skill_Police_2", false);
                Button_Option.instance.g_SkillSpeech.transform.GetChild(1).gameObject.SetActive(true);

                str = Script_SkillData[5][TranslateManager_Scr.instance.s_Language].ToString();
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
                Button_Option.instance.g_SkillSpeech.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = str;


                /*
                if (TranslateManager_Scr.instance.b_isEnglish)
                {
                    Button_Option.instance.g_SkillSpeech.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>().text
                        = Script_SkillData[5]["English"].ToString().Substring(0, 28) + "\n" + Script_SkillData[5]["English"].ToString().Substring(29, 31);
                }
                else
                {
                    Button_Option.instance.g_SkillSpeech.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>().text
                        = Script_SkillData[5][TranslateManager_Scr.instance.s_Language].ToString();
                }*/
                break;

            case 3:
                SoundManager_sfx.instance.PlaySE("Skill_SCV_2", false);
                Button_Option.instance.g_SkillSpeech.transform.GetChild(1).gameObject.SetActive(true);

                str = Script_SkillData[7][TranslateManager_Scr.instance.s_Language].ToString();
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
                Button_Option.instance.g_SkillSpeech.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = str;

                /*
                if (TranslateManager_Scr.instance.b_isEnglish)
                {
                    str = Script_SkillData[7]["English"].ToString();
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
                    Button_Option.instance.g_SkillSpeech.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>().text
                        = str;
                }
                else if (TranslateManager_Scr.instance.b_isJapanese)
                {
                    str = Script_SkillData[7]["Japanese"].ToString();
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
                    Button_Option.instance.g_SkillSpeech.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>().text
                        = str;
                }
                else
                {
                    Button_Option.instance.g_SkillSpeech.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>().text
                        = Script_SkillData[7][TranslateManager_Scr.instance.s_Language].ToString();
                }*/
                break;
        }
    }

    public void endSkill()
    {
        go_SkillExplanation.transform.GetComponent<TextMeshProUGUI>().text = " ";
        go_SkillExplanation.SetActive(false);
        Button_Option.instance.g_SkillSpeech.transform.GetChild(1).gameObject.SetActive(false);
        Button_Option.instance.g_SkillSpeech.SetActive(false);
        go_ICon.SetActive(false);
        //go_Skillcam.SetActive(false);
        myAni.SetBool("isAttack2", false);

        if (useSkillNum == 0) // 컴퓨터
        {
            for (int i = 0; i < go_Enemy.transform.GetChild(0).childCount; i++)
            {
                if (go_Enemy.transform.GetChild(0).GetChild(i).gameObject.activeSelf == true)
                {
                    go_Enemy.transform.GetChild(0).GetChild(i).transform.GetComponent<BlockControl>().ApplySkill(useSkillNum);
                }
            }
        }
        else if (useSkillNum == 1) // 캐비닛 서류상자
        {
            for (int i = 0; i < go_Enemy.transform.GetChild(2).childCount; i++) // 캐비닛
            {
                if (go_Enemy.transform.GetChild(2).GetChild(i).gameObject.activeSelf == true)
                {
                    go_Enemy.transform.GetChild(2).GetChild(i).transform.GetComponent<BlockControl>().ApplySkill(useSkillNum);
                }
            }
            for (int i = 0; i < go_Enemy.transform.GetChild(1).childCount; i++) // 서류상자
            {
                if (go_Enemy.transform.GetChild(1).GetChild(i).gameObject.activeSelf == true)
                {
                    go_Enemy.transform.GetChild(1).GetChild(i).transform.GetComponent<BlockControl>().ApplySkill(useSkillNum);
                }
            }
        }
        else if (useSkillNum == 2) // 간부
        {
            if(ScenarioEventManager_Scr.instance.i_EventNum == 1 || ScenarioEventManager_Scr.instance.i_EventNum == 2 || ScenarioEventManager_Scr.instance.i_EventNum == 3 || ScenarioEventManager_Scr.instance.i_EventNum == 4 || ScenarioEventManager_Scr.instance.i_EventNum == 5)
            {
                // 미션 실패
                ScenarioEventManager_Scr.instance.MissionFail();
            }
            for (int i = 0; i < go_Enemy.transform.GetChild(5).childCount; i++) // 주임
            {
                if (go_Enemy.transform.GetChild(5).GetChild(i).gameObject.activeSelf == true)
                {
                    go_Enemy.transform.GetChild(5).GetChild(i).transform.GetComponent<BlockControl>().ApplySkill(useSkillNum);
                }
            }
            for (int i = 0; i < go_Enemy.transform.GetChild(6).childCount; i++) // 대리
            {
                if (go_Enemy.transform.GetChild(6).GetChild(i).gameObject.activeSelf == true)
                {
                    go_Enemy.transform.GetChild(6).GetChild(i).transform.GetComponent<BlockControl>().ApplySkill(useSkillNum);
                }
            }
            for (int i = 0; i < go_Enemy.transform.GetChild(7).childCount; i++) // 과장
            {
                if (go_Enemy.transform.GetChild(7).GetChild(i).gameObject.activeSelf == true)
                {
                    go_Enemy.transform.GetChild(7).GetChild(i).transform.GetComponent<BlockControl>().ApplySkill(useSkillNum);
                }
            }
            for (int i = 0; i < go_Enemy.transform.GetChild(8).childCount; i++) // 부장
            {
                if (go_Enemy.transform.GetChild(8).GetChild(i).gameObject.activeSelf == true)
                {
                    go_Enemy.transform.GetChild(8).GetChild(i).transform.GetComponent<BlockControl>().ApplySkill(useSkillNum);
                }
            }
            /*
            for (int i = 0; i < go_Enemy.transform.GetChild(9).childCount; i++) // 용역
            {
                if (go_Enemy.transform.GetChild(9).GetChild(i).gameObject.activeSelf == true)
                {
                    go_Enemy.transform.GetChild(9).GetChild(i).transform.GetComponent<BlockControl>().ApplySkill(useSkillNum);
                }
            }*/
        }
        else if (useSkillNum == 3) // 보스
        {
            
            for (int i = 0; i < RespawnManager.instance.AllBlock.Count; i++)
            {
                if (RespawnManager.instance.AllBlock[i].layer == LayerMask.NameToLayer("Skill_SCV") || RespawnManager.instance.AllBlock[i].transform.GetComponent<BlockControl>().b_onBossBlock)
                {
                    RespawnManager.instance.AllBlock[i].transform.GetComponent<BlockControl>().ApplySkill(useSkillNum);
                }
            }
            RespawnManager.instance.b_ApplySkill_Boss = true;
            Time.timeScale = 1;
        }
        if (BlockControl.b_SkillLevelUp)
        {
            Time.timeScale = 0;
        }


        go_PlayerBase.transform.GetComponent<Player_Input>().b_isSkill = false;

        Player_Input.instance.g_ButtonShield.SetActive(false);
        if (RespawnManager.instance.ActiveBlock.Count > 0 && !TutorialManager.instance.b_FirstPlaying)
        {
            RespawnManager.instance.timer_Lock = false;

            RespawnManager.instance.EndRound();
        }
    }

    public void TextSkillFunction(bool isTutorial)
    {
        if (!isTutorial)
        {
            go_SkillExplanation.SetActive(true);
            go_SkillExplanation.transform.GetComponent<TextMeshProUGUI>().text = TranslateData[useSkillNum + 127]["Korean"].ToString();
        }
    }
}