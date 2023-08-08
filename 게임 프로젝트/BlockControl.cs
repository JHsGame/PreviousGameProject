using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlockControl : MonoBehaviour
{
    [Header("블럭 이름")]
    public bool b_CoinBlock;
    public bool b_DiaBlock;
    public bool b_CabinetBlock;
    public bool b_ComputerBlock;
    public bool b_DocumentboxBlock;

    public bool b_JuimBlock;
    public bool b_DealiBlock;
    public bool b_GwajangBlock;
    public bool b_BujangBlock;
    public bool b_YongyeogBlock;

    public bool b_IsaBlock; 
    public bool b_SangmuBlock;
    public bool b_jeonmuBlock;
    public bool b_SajangBlock;

    [Header("기본 속성")]
    public int i_Blocknum = 0;
    public bool b_CustomName = false;
    public int i_CustomNameNum = 0;
    public int i_Kinds = 0; // 개, 벌레, 바이러스, 좀비, 사물

    public bool b_BossBlock = false;
    public bool b_onBossBlock = false;
    public bool b_SpeechBlock = false;
    public bool b_isSpeech = false;
    public bool b_isRespawnParticleOn = false;
    public bool b_isEventBlock = false;
    public bool[] b_Line = new bool[12];

    public Vector3 v_DieDir;
    public Vector3 v_before_movePos;
    public Vector3 v_after_movePos;
    private Vector3 originPos;

    public GameObject go_HitBall;
    public GameObject g_DieCube;
    public GameObject go_ClearText; // 보스나 마지막 블럭 죽을 시 nice, good출력
    public GameObject go_StreakImage; // 3, 6, 9, 12개 블럭을 한 번의 공격으로 파괴시 출력
    public GameObject go_SpeechPos;
    public GameObject go_HpBar;
    public GameObject go_CinemachineCam;
    public GameObject go_Desk_mesh;
    public GameObject go_Char_mesh;
    public GameObject go_EventMark;
    public GameObject go_Mark;
    public Text t_hitScore;
    // public GameObject[] go_HitParticle = new GameObject[5];
    public ParticleSystem[] par_HitParticle = new ParticleSystem[0];
    public ParticleSystem[] par_BigHitParticle = new ParticleSystem[0];
    public ParticleSystem[] par_ExplosionHitParticle = new ParticleSystem[0];
    public ParticleSystem[] par_AbilityHitParticle = new ParticleSystem[0];
    public Animator anim_myani;
    private Animator anim_hittextani;
    private Image i_HP;
    private Text t_HP;
    public TextMeshProUGUI t_Name;
    private Coroutine ShakeCoroutine;   
    public ParticleSystem[] par_RespawnBulldog = new ParticleSystem[2];

    public TextAsset t_TranslateCSV;
    public TextAsset t_SurnameCSV;
   // public List<Dictionary<string, object>> TranslateData;
   // public List<Dictionary<string, object>> SurnameData;


    [Header("수치 속성")]
    public float f_FullPoint;
    public float f_HitPoint;
    public float f_GateDistance;
    public int i_Exp;
    public int i_Gold;
    public int i_Cash;
    public float f_Atk;
    public float f_ScreamColtime = 0.1f;
    public Color c_HpColor;

    public static bool b_SkillLevelUp = false;

    private void Awake()
    {
       // TranslateData = CSVReader.Read(t_TranslateCSV);
     //   SurnameData = CSVReader.Read(t_SurnameCSV);
        g_DieCube = GameObject.Find("G.Debris");
    }

    private void OnDisable()
    {
        if(go_EventMark != null)
        {
            go_EventMark.SetActive(false);
            go_Mark.transform.SetParent(go_EventMark.transform);
            go_Mark.transform.SetAsFirstSibling();
        }

        t_hitScore.color = new Color32(0, 0, 0, 0);

        if (b_BossBlock)
        {
            Button_Option.instance.UICam.transform.GetComponent<UICamFOV>().enabled = false;
            transform.GetChild(2).GetComponent<RespawnBulldog>().i_TouchLineNum = 2;
            go_CinemachineCam.SetActive(false);
        }
        if (b_CustomName)
        {
            b_CustomName = false;
            if (b_JuimBlock)
            {
                RespawnManager.instance.s_CheckJuimName[i_CustomNameNum] = false;
            }
            else if (b_DealiBlock)
            {
                RespawnManager.instance.s_CheckDealiName[i_CustomNameNum] = false;
            }
            else if (b_GwajangBlock)
            {
                RespawnManager.instance.s_CheckGwajangName[i_CustomNameNum] = false;
            }
            else if (b_BujangBlock)
            {
                RespawnManager.instance.s_CheckBujangName[i_CustomNameNum] = false;
            }
        }
        b_isEventBlock = false;
        if(go_Mark != null)
        {
            go_Mark.transform.SetParent(go_EventMark.transform);
        }
    }

    public void OnEnable()
    {
        if (b_YongyeogBlock)
        {
            GameObject bulldogspeech = RespawnManager.instance.go_BulldogSpeechBubble;
            for (int i = 0; i < bulldogspeech.transform.childCount; i++)
            {
                if (!bulldogspeech.transform.GetChild(i).GetComponent<SpeechBubble>().b_isSpeech)
                {
                    bulldogspeech.transform.GetChild(i).gameObject.SetActive(true);
                    SpeechBubble SpeechBubbleScript = bulldogspeech.transform.GetChild(i).GetComponent<SpeechBubble>();
                    SpeechBubbleScript.go_TartgetBlock = gameObject;
                    SpeechBubbleScript.go_TartgetBlockPos = go_SpeechPos;
                    SpeechBubbleScript.StartSpeech(2, i_Kinds);
                    transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetBool("isSpeaking", true);
                    break;  
                }
            }            
        }

        if(TranslateManager_Scr.instance.b_isIndia)
        {
            setName(TranslateManager_Scr.instance.s_Language, "Indian");

        }
        else
        {
            setName(TranslateManager_Scr.instance.s_Language, TranslateManager_Scr.instance.s_Language);

        }
        if (ScenarioEventManager_Scr.instance.i_EventNum == 5)
        {
            if (b_isEventBlock)
            {
                f_Atk =0;
            }
        }

        NameColor();
        //TranslateManager_Scr.instance.ChangeFont(t_hitScore);
    }

    public void setDefult()
    {
        if (b_YongyeogBlock)
        {
            GameObject bulldogspeech = RespawnManager.instance.go_BulldogSpeechBubble;
            for (int i = 0; i < bulldogspeech.transform.childCount; i++)
            {
                if (!bulldogspeech.transform.GetChild(i).GetComponent<SpeechBubble>().b_isSpeech)
                {
                    bulldogspeech.transform.GetChild(i).gameObject.SetActive(true);
                    SpeechBubble SpeechBubbleScript = bulldogspeech.transform.GetChild(i).GetComponent<SpeechBubble>();
                    SpeechBubbleScript.go_TartgetBlock = gameObject;
                    SpeechBubbleScript.go_TartgetBlockPos = go_SpeechPos;
                   // SpeechBubbleScript.StartSpeech(2, i_Kinds);
                    transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetBool("isSpeaking", true);
                    break;  
                }
            }            
        }

        if(TranslateManager_Scr.instance.b_isIndia)
        {
            setName(TranslateManager_Scr.instance.s_Language, "Indian");

        }
        else
        {
            setName(TranslateManager_Scr.instance.s_Language, TranslateManager_Scr.instance.s_Language);

        }
        if (ScenarioEventManager_Scr.instance.i_EventNum == 5)
        {
            if (b_isEventBlock)
            {
                f_Atk =0;
            }
        }

        NameColor();
    }

    void NameColor()
    {
        if (b_JuimBlock || b_DealiBlock || b_GwajangBlock || b_BujangBlock || b_YongyeogBlock)
        {
            t_Name.color = new Color(255f / 255f, 121f / 255f, 0f / 255f, 255f / 255f);
        }
        else if (b_BossBlock)
        {
            t_Name.color = new Color(214f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
        }
    }


    public void setName(string Language, string SurNameLanguage)
    {
        if (transform.GetChild(0).GetChild(2).GetChild(1).gameObject != null)
        {
            t_Name = transform.GetChild(0).GetChild(2).GetChild(1).GetChild(0).transform.GetComponent<TextMeshProUGUI>();

            if (TranslateManager_Scr.instance.b_isKorean || TranslateManager_Scr.instance.b_isEnglish
                || TranslateManager_Scr.instance.b_isIndia || TranslateManager_Scr.instance.b_isVietnam
                || TranslateManager_Scr.instance.b_isFrance || TranslateManager_Scr.instance.b_isGermany
                || TranslateManager_Scr.instance.b_isItalia)
            {
                TranslateManager_Scr.instance.ChangeFont(t_Name, 3);
            }
            else if (TranslateManager_Scr.instance.b_isJapanese)
            {
                TranslateManager_Scr.instance.ChangeFont(t_Name, 4);
            }
            else if (TranslateManager_Scr.instance.b_isChinese)
            {
                TranslateManager_Scr.instance.ChangeFont(t_Name, 5);
            }



            TranslateManager_Scr.instance.ChangeFont(t_hitScore, 0);
            if (b_CoinBlock)
                t_Name.text = TranslateManager_Scr.instance.SurnameData[20][SurNameLanguage].ToString();
            else if (b_DiaBlock)
                t_Name.text = TranslateManager_Scr.instance.SurnameData[21][SurNameLanguage].ToString();
            else if (b_CabinetBlock)
                t_Name.text = TranslateManager_Scr.instance.SurnameData[22][SurNameLanguage].ToString();
            else if (b_ComputerBlock)
                t_Name.text = TranslateManager_Scr.instance.SurnameData[23][SurNameLanguage].ToString();
            else if (b_DocumentboxBlock)
                t_Name.text = TranslateManager_Scr.instance.SurnameData[24][SurNameLanguage].ToString();
            else if (b_JuimBlock || b_DealiBlock || b_GwajangBlock || b_BujangBlock)
            {
                if (b_JuimBlock && ShopManager_Scr.instance.s_JuimName.Count > 0)
                {
                    int ableUseCount = 0;
                    for (int i = 0; i < RespawnManager.instance.s_CheckJuimName.Count; i++)
                    {
                        if (!RespawnManager.instance.s_CheckJuimName[i])
                        {
                            ableUseCount++;
                            break;
                        }
                    }
                    if (ableUseCount >= 1)
                    {
                        while (true)
                        {
                            int randomSelect = Random.Range(0, RespawnManager.instance.s_CheckJuimName.Count);
                            if (!RespawnManager.instance.s_CheckJuimName[randomSelect])
                            {
                                RespawnManager.instance.s_CheckJuimName[randomSelect] = true;
                                t_Name.text = ShopManager_Scr.instance.s_JuimName[randomSelect];
                                i_CustomNameNum = randomSelect;
                                b_CustomName = true;
                                break;
                            }
                        }
                    }
                    else if (ableUseCount <= 0) 
                    {
                        string R_FirstName = TranslateManager_Scr.instance.SurnameData[Random.Range(0, 18)][SurNameLanguage].ToString();

                        string Name = TranslateManager_Scr.instance.TranslateData[24][Language].ToString();
                     //   t_Name.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.3f, 2);
                        if (TranslateManager_Scr.instance.b_isKorean)
                        {
                            t_Name.text = R_FirstName + Name;
                        }
                        else if (TranslateManager_Scr.instance.b_isEnglish)
                        {
                            t_Name.transform.parent.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                            t_Name.text = Name + "\n" + R_FirstName;
                        }
                        else if (TranslateManager_Scr.instance.b_isJapanese)
                        {
                            t_Name.text = R_FirstName + Name;
                        }
                        else if (TranslateManager_Scr.instance.b_isVietnam || TranslateManager_Scr.instance.b_isGermany || TranslateManager_Scr.instance.b_isFrance || TranslateManager_Scr.instance.b_isItalia)
                        {
                            t_Name.transform.parent.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                            t_Name.text = R_FirstName + "\n" + Name;
                        }
                        else
                        {
                            t_Name.transform.parent.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                            t_Name.text = R_FirstName + Name;
                        }
                    }

                    //   int SelectNum = Random.Range(0, ShopManager_Scr.instance.s_JuimName.Count);
                    //   t_Name.text = ShopManager_Scr.instance.s_JuimName[SelectNum];
                }
                else if (b_DealiBlock && ShopManager_Scr.instance.s_DealiName.Count > 0)
                {
                    int ableUseCount = 0;
                    for (int i = 0; i < RespawnManager.instance.s_CheckDealiName.Count; i++)
                    {
                        if (!RespawnManager.instance.s_CheckDealiName[i])
                        {
                            ableUseCount++;
                            break;
                        }
                    }
                    if (ableUseCount >= 1)
                    {
                        while (true)
                        {
                            int randomSelect = Random.Range(0, RespawnManager.instance.s_CheckDealiName.Count);
                            if (!RespawnManager.instance.s_CheckDealiName[randomSelect])
                            {
                                RespawnManager.instance.s_CheckDealiName[randomSelect] = true;
                                t_Name.text = ShopManager_Scr.instance.s_DealiName[randomSelect];
                                i_CustomNameNum = randomSelect;
                                b_CustomName = true;
                                break;
                            }
                        }
                    }
                    else if (ableUseCount <= 0)
                    {
                        string R_FirstName = TranslateManager_Scr.instance.SurnameData[Random.Range(0, 18)][SurNameLanguage].ToString();

                        string Name = TranslateManager_Scr.instance.TranslateData[25][Language].ToString();
                      //  t_Name.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.3f, 2);
                        if (TranslateManager_Scr.instance.b_isKorean)
                        {
                            t_Name.text = R_FirstName + Name;
                        }
                        else if (TranslateManager_Scr.instance.b_isEnglish)
                        {
                            t_Name.transform.parent.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                            t_Name.text = Name + "\n" + R_FirstName;
                        }
                        else if (TranslateManager_Scr.instance.b_isJapanese)
                        {
                            t_Name.text = R_FirstName + Name;
                        }
                        else if (TranslateManager_Scr.instance.b_isVietnam || TranslateManager_Scr.instance.b_isGermany || TranslateManager_Scr.instance.b_isFrance || TranslateManager_Scr.instance.b_isItalia)
                        {
                            t_Name.transform.parent.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                            t_Name.text = R_FirstName + "\n" + Name;
                        }
                        else
                        {
                            t_Name.transform.parent.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                            t_Name.text = R_FirstName + Name;
                        }
                    }
                    //  int SelectNum = Random.Range(0, ShopManager_Scr.instance.s_DealiName.Count);
                    // t_Name.text = ShopManager_Scr.instance.s_DealiName[SelectNum];
                }
                else if (b_GwajangBlock && ShopManager_Scr.instance.s_GwajangName.Count > 0)
                {
                    int ableUseCount = 0;
                    for (int i = 0; i < RespawnManager.instance.s_CheckGwajangName.Count; i++)
                    {
                        if (!RespawnManager.instance.s_CheckGwajangName[i])
                        {
                            ableUseCount++;
                            break;
                        }
                    }
                    if (ableUseCount >= 1)
                    {
                        while (true)
                        {
                            int randomSelect = Random.Range(0, RespawnManager.instance.s_CheckGwajangName.Count);
                            if (!RespawnManager.instance.s_CheckGwajangName[randomSelect])
                            {
                                RespawnManager.instance.s_CheckGwajangName[randomSelect] = true;
                                t_Name.text = ShopManager_Scr.instance.s_GwajangName[randomSelect];
                                i_CustomNameNum = randomSelect;
                                b_CustomName = true;
                                break;
                            }
                        }
                    }
                    else if (ableUseCount <= 0)
                    {
                        string R_FirstName = TranslateManager_Scr.instance.SurnameData[Random.Range(0, 18)][SurNameLanguage].ToString();
                        string Name = TranslateManager_Scr.instance.TranslateData[26][Language].ToString();
                      //  t_Name.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.3f, 2);
                        if (TranslateManager_Scr.instance.b_isKorean)
                        {
                            t_Name.text = R_FirstName + Name;
                        }
                        else if (TranslateManager_Scr.instance.b_isEnglish)
                        {
                            t_Name.transform.parent.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                            t_Name.text = Name + "\n" + R_FirstName;
                        }
                        else if (TranslateManager_Scr.instance.b_isJapanese)
                        {
                            t_Name.text = R_FirstName + Name;
                        }
                        else if (TranslateManager_Scr.instance.b_isVietnam || TranslateManager_Scr.instance.b_isGermany || TranslateManager_Scr.instance.b_isFrance || TranslateManager_Scr.instance.b_isItalia)
                        {
                            t_Name.transform.parent.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                            t_Name.text = R_FirstName + "\n" + Name;
                        }
                        else
                        {
                            t_Name.transform.parent.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                            t_Name.text = R_FirstName + Name;
                        }
                    }
                    //  int SelectNum = Random.Range(0, ShopManager_Scr.instance.s_GwajangName.Count);
                    //   t_Name.text = ShopManager_Scr.instance.s_GwajangName[SelectNum];
                }
                else if (b_BujangBlock && ShopManager_Scr.instance.s_BujangName.Count > 0)
                {
                    int ableUseCount = 0;
                    for (int i = 0; i < RespawnManager.instance.s_CheckBujangName.Count; i++)
                    {
                        if (!RespawnManager.instance.s_CheckBujangName[i])
                        {
                            ableUseCount++;
                            break;
                        }
                    }
                    if (ableUseCount >= 1)
                    {
                        while (true)
                        {
                            int randomSelect = Random.Range(0, RespawnManager.instance.s_CheckBujangName.Count);
                            if (!RespawnManager.instance.s_CheckBujangName[randomSelect])
                            {
                                RespawnManager.instance.s_CheckBujangName[randomSelect] = true;
                                t_Name.text = ShopManager_Scr.instance.s_BujangName[randomSelect];
                                i_CustomNameNum = randomSelect;
                                b_CustomName = true;
                                break;
                            }
                        }
                    }
                    else if (ableUseCount <= 0)
                    {
                        string R_FirstName = TranslateManager_Scr.instance.SurnameData[Random.Range(0, 18)][SurNameLanguage].ToString();
                        string Name = TranslateManager_Scr.instance.TranslateData[27][Language].ToString();
                      //  t_Name.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.3f, 2);
                        if (TranslateManager_Scr.instance.b_isKorean)
                        {
                            t_Name.text = R_FirstName + Name;
                        }
                        else if (TranslateManager_Scr.instance.b_isEnglish)
                        {
                            t_Name.transform.parent.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                            t_Name.text = Name + "\n" + R_FirstName;
                        }
                        else if (TranslateManager_Scr.instance.b_isJapanese)
                        {
                            t_Name.text = R_FirstName + Name;
                        }
                        else if (TranslateManager_Scr.instance.b_isVietnam || TranslateManager_Scr.instance.b_isGermany || TranslateManager_Scr.instance.b_isFrance || TranslateManager_Scr.instance.b_isItalia)
                        {
                            t_Name.transform.parent.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                            t_Name.text = R_FirstName + "\n" + Name;
                        }
                        else
                        {
                            t_Name.transform.parent.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                            t_Name.text = R_FirstName + Name;
                        }
                    }
                    //  int SelectNum = Random.Range(0, ShopManager_Scr.instance.s_BujangName.Count);
                    //  t_Name.text = ShopManager_Scr.instance.s_BujangName[SelectNum];
                }
                else
                {
                    string R_FirstName = TranslateManager_Scr.instance.SurnameData[Random.Range(0, 18)][SurNameLanguage].ToString();
                    string Name = "";
                    if (b_JuimBlock)
                        Name = TranslateManager_Scr.instance.TranslateData[24][Language].ToString();
                    else if (b_DealiBlock)
                        Name = TranslateManager_Scr.instance.TranslateData[25][Language].ToString();
                    else if (b_GwajangBlock)
                        Name = TranslateManager_Scr.instance.TranslateData[26][Language].ToString();
                    else if (b_BujangBlock)
                        Name = TranslateManager_Scr.instance.TranslateData[27][Language].ToString();

                  //  t_Name.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.3f, 2);
                    if (TranslateManager_Scr.instance.b_isKorean)
                    {
                        t_Name.text = R_FirstName + Name;
                    }
                    else if (TranslateManager_Scr.instance.b_isEnglish)
                    {
                        t_Name.transform.parent.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                        t_Name.text = Name + "\n" + R_FirstName;
                    }
                    else if (TranslateManager_Scr.instance.b_isJapanese)
                    {
                        t_Name.text = R_FirstName + Name;
                    }
                    else if (TranslateManager_Scr.instance.b_isVietnam || TranslateManager_Scr.instance.b_isGermany || TranslateManager_Scr.instance.b_isFrance || TranslateManager_Scr.instance.b_isItalia)
                    {
                        t_Name.transform.parent.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                        t_Name.text = R_FirstName + "\n" + Name;
                    }
                    else
                    {
                        t_Name.transform.parent.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                        t_Name.text = R_FirstName + Name;
                    }
                }
            }
            else if (b_YongyeogBlock)
                t_Name.text = TranslateManager_Scr.instance.SurnameData[25][SurNameLanguage].ToString();
            else if (b_IsaBlock || b_SangmuBlock || b_jeonmuBlock || b_SajangBlock)
            {
                if (b_JuimBlock && ShopManager_Scr.instance.s_IsaName.Count > 0)
                {
                    int SelectNum = Random.Range(0, ShopManager_Scr.instance.s_IsaName.Count);
                    t_Name.transform.parent.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                    t_Name.text = ShopManager_Scr.instance.s_IsaName[SelectNum];
                    b_CustomName = true;
                }
                else if (b_SangmuBlock && ShopManager_Scr.instance.s_SangmuName.Count > 0)
                {
                    int SelectNum = Random.Range(0, ShopManager_Scr.instance.s_SangmuName.Count);
                    t_Name.transform.parent.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                    t_Name.text = ShopManager_Scr.instance.s_SangmuName[SelectNum];
                    b_CustomName = true;
                }
                else if (b_jeonmuBlock && ShopManager_Scr.instance.s_JeonmuName.Count > 0)
                {
                    int SelectNum = Random.Range(0, ShopManager_Scr.instance.s_JeonmuName.Count);
                    t_Name.transform.parent.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                    t_Name.text = ShopManager_Scr.instance.s_JeonmuName[SelectNum];
                    b_CustomName = true;
                }
                else if (b_SajangBlock && ShopManager_Scr.instance.s_SajangName.Count > 0)
                {
                    int SelectNum = Random.Range(0, ShopManager_Scr.instance.s_SajangName.Count);
                    t_Name.transform.parent.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                    t_Name.text = ShopManager_Scr.instance.s_SajangName[SelectNum];
                    b_CustomName = true;
                }
                else
                {
                    string R_FirstName = TranslateManager_Scr.instance.SurnameData[Random.Range(0, 18)][SurNameLanguage].ToString();
                    string Name = "";
                    if (b_IsaBlock)
                        Name = TranslateManager_Scr.instance.TranslateData[28][Language].ToString();
                    else if (b_SangmuBlock)
                        Name = TranslateManager_Scr.instance.TranslateData[29][Language].ToString();
                    else if (b_jeonmuBlock)
                        Name = TranslateManager_Scr.instance.TranslateData[30][Language].ToString();
                    else if (b_SajangBlock)
                        Name = TranslateManager_Scr.instance.TranslateData[31][Language].ToString();

                    //t_Name.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                    t_Name.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 110);
                    if (TranslateManager_Scr.instance.b_isKorean)
                    {
                        t_Name.text = R_FirstName + Name;
                    }
                    else if (TranslateManager_Scr.instance.b_isEnglish)
                    {
                        t_Name.transform.parent.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                        t_Name.text = Name + "\n" + R_FirstName;
                    }
                    else if (TranslateManager_Scr.instance.b_isJapanese)
                    {
                        t_Name.transform.parent.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                        t_Name.text = R_FirstName + Name;
                    }
                    else if (TranslateManager_Scr.instance.b_isChinese)
                    {
                        t_Name.transform.parent.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                        t_Name.text = R_FirstName + Name;
                    }
                    else
                    {
                        t_Name.transform.parent.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0.5f, 2);
                        t_Name.text = R_FirstName + "\n" + Name;
                    }
                }
            }

            t_Name.transform.parent.GetComponent<NameCoillder>().reset_value();
        }
    }


    void Start()
    {
        originPos = transform.GetChild(0).localPosition;
        /*
        if (!b_BossBlock)
        {
            for (int i = 0; i < transform.GetChild(0).GetChild(3).childCount; i++)
            {
                go_HitParticle[i] = transform.GetChild(0).GetChild(3).GetChild(i).gameObject;
                par_HitParticle[i] = go_HitParticle[i].transform.GetComponent<ParticleSystem>();
            }
        }
        else
        {
            for (int i = 0; i < transform.GetChild(0).GetChild(4).childCount; i++)
            {
                go_HitParticle[i] = transform.GetChild(0).GetChild(4).GetChild(i).gameObject;
                par_HitParticle[i] = go_HitParticle[i].transform.GetComponent<ParticleSystem>();
            }
        }*/
        anim_myani = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        anim_hittextani = transform.GetChild(1).GetComponent<Animator>();
        i_HP = transform.GetChild(0).GetChild(1).GetChild(1).transform.GetComponent<Image>();
        t_HP = transform.GetChild(0).GetChild(1).GetChild(2).transform.GetComponent<Text>();
        t_HP.text = f_HitPoint.ToString();
        //TranslateManager_Scr.instance.ChangeFont(t_HP);

        v_before_movePos = transform.position;
        /*
        if (!b_YongyeogBlock)
        {
            v_after_movePos = (transform.position + new Vector3(0, 0, -1f));
        }
        else
        {
            v_after_movePos = transform.position;
        }*/
    }
    private void Update()
    {
        if (b_SpeechBlock && f_ScreamColtime >= 0)
        {
            f_ScreamColtime -= Time.deltaTime;
        }

        if (go_HpBar.activeSelf)
            go_HpBar.transform.GetChild(1).GetComponent<Image>().color = c_HpColor;

        if (!Player_Input.instance.b_isAttacking){

            transform.position = Vector3.Lerp(transform.position, v_after_movePos, (5f * Time.deltaTime));
        }

        if (b_isRespawnParticleOn && (!par_RespawnBulldog[0].isPlaying && !par_RespawnBulldog[1].isPlaying)){
            b_isRespawnParticleOn = false;
            par_RespawnBulldog[0].gameObject.SetActive(false);
        }

        if (b_isEventBlock)
        {
            if (ScenarioEventManager_Scr.instance.i_EventNum == 0)
            {
                go_EventMark.SetActive(false);
                b_SpeechBlock = true;
                b_isEventBlock = false;

                go_Mark.transform.SetParent(go_EventMark.transform);
                go_Mark.transform.SetAsFirstSibling();
            }
            else 
            {
                if (Button_Option.instance.b_WindowOn)
                {
                    go_Mark.SetActive(false);
                }
                else
                {
                    go_Mark.SetActive(true);
                }
            }
        }
    }

    public void PlaySwpanParticle()
    {
        for (int i = 0; i < 2; i++)
        {
            par_RespawnBulldog[0].gameObject.SetActive(true);
            par_RespawnBulldog[i].Play();
            b_isRespawnParticleOn = true;
        }
    }

    public void setinfo() // 스테이지에 따른 이 블럭의 능력치 재설정
    {
        if (b_CoinBlock)
        {
            i_Gold = (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Coin_Gain"];
            i_Exp = 0;
            f_FullPoint = Random.Range((int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Coin_Min_HP"], (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Coin_Max_HP"] + 1);
            i_Cash = 0;
            f_Atk = 0;
        }
        else if (b_DiaBlock)
        {
            i_Gold = 0;
            i_Exp = 0;
            f_FullPoint = Random.Range((int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Gem_Min_HP"], (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Gem_Max_HP"] + 1);
            i_Cash = (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Gem_Gain"];
            f_Atk = 0;
        }
        else if (b_ComputerBlock || b_CabinetBlock || b_DocumentboxBlock)
        {
            if (RespawnManager.instance.b_NomalGame)
            {
                i_Gold = 0;
                i_Exp = (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Obj_EXP"];
                f_FullPoint = Random.Range((int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Obj_Min_HP"], (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Obj_Max_HP"] + 1);
                i_Cash = 0;
                f_Atk = Random.Range((int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Obj_Min_Attack"], (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Obj_Max_Attack"] + 1);
            }
            else
            {
                if(StageManager_Scr.instance.i_StageRound + StageManager_Scr.instance.i_StageClearLevel > 1000)
                {
                    i_Gold = 0;
                    i_Exp = (int)RespawnManager.instance.RespawnData[1000]["Obj_EXP"] + ((StageManager_Scr.instance.i_StageRound - 1000) * 3);
                    f_FullPoint = Random.Range((int)RespawnManager.instance.RespawnData[1000]["Obj_Min_HP"], (int)RespawnManager.instance.RespawnData[1000]["Obj_Max_HP"] + 1) + ((StageManager_Scr.instance.i_StageRound - 1000) * 6);
                    i_Cash = 0;
                    f_Atk = Random.Range((int)RespawnManager.instance.RespawnData[1000]["Obj_Min_Attack"], (int)RespawnManager.instance.RespawnData[1000]["Obj_Max_Attack"] + 1) + ((StageManager_Scr.instance.i_StageRound - 1000) * 1);

                }
                else
                {
                    i_Gold = 0;
                    i_Exp = (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageRound - 1 + StageManager_Scr.instance.i_StageClearLevel-1]["Obj_EXP"];
                    f_FullPoint = Random.Range((int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageRound - 1 + StageManager_Scr.instance.i_StageClearLevel-1]["Obj_Min_HP"], (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageRound - 1 + StageManager_Scr.instance.i_StageClearLevel-1]["Obj_Max_HP"] + 1);
                    i_Cash = 0;
                    f_Atk = Random.Range((int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageRound - 1 + StageManager_Scr.instance.i_StageClearLevel-1]["Obj_Min_Attack"], (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageRound - 1 + StageManager_Scr.instance.i_StageClearLevel-1]["Obj_Max_Attack"] + 1);

                }
            }
        }
        else if (b_JuimBlock || b_DealiBlock || b_GwajangBlock || b_BujangBlock || b_YongyeogBlock)
        {
            if (RespawnManager.instance.b_NomalGame)
            {
                i_Gold = 0;
                i_Exp = (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Mob_EXP"];
                f_FullPoint = Random.Range((int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Mob_Min_HP"], (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Mob_Max_HP"] + 1);
                i_Cash = 0;
                f_Atk = Random.Range((int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Mob_Min_Attack"], (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Mob_Max_Attack"] + 1);

            }
            else
            {
                if (StageManager_Scr.instance.i_StageRound + StageManager_Scr.instance.i_StageClearLevel > 1000)
                {
                    i_Gold = 0;
                    i_Exp = (int)RespawnManager.instance.RespawnData[1000]["Mob_EXP"] + ((StageManager_Scr.instance.i_StageRound - 1000) * 6);
                    f_FullPoint = Random.Range((int)RespawnManager.instance.RespawnData[1000]["Mob_Min_HP"], (int)RespawnManager.instance.RespawnData[1000]["Mob_Max_HP"] + 1) + ((StageManager_Scr.instance.i_StageRound - 1000) * 20);
                    i_Cash = 0;
                    f_Atk = Random.Range((int)RespawnManager.instance.RespawnData[1000]["Mob_Min_Attack"], (int)RespawnManager.instance.RespawnData[1000]["Mob_Max_Attack"] + 1) + ((StageManager_Scr.instance.i_StageRound - 1000) *1);

                }
                else
                {
                    i_Gold = 0;
                    i_Exp = (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageRound - 1 + StageManager_Scr.instance.i_StageClearLevel-1]["Mob_EXP"];
                    f_FullPoint = Random.Range((int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageRound - 1 + StageManager_Scr.instance.i_StageClearLevel-1]["Mob_Min_HP"], (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageRound - 1 + StageManager_Scr.instance.i_StageClearLevel-1]["Mob_Max_HP"] + 1);
                    i_Cash = 0;
                    f_Atk = Random.Range((int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageRound - 1 + StageManager_Scr.instance.i_StageClearLevel-1]["Mob_Min_Attack"], (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageRound - 1 + StageManager_Scr.instance.i_StageClearLevel-1]["Mob_Max_Attack"] + 1);

                }
            }
        }
        else if (b_BossBlock)
        {
            if (RespawnManager.instance.b_NomalGame)
            {
                i_Gold = 0;
                i_Exp = (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Boss_EXP"];
                f_FullPoint = Random.Range((int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Boss_Min_HP"], (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Boss_Max_HP"] + 1);
                i_Cash = 0;
                f_Atk = 9999;
            }
            else
            {
                if (StageManager_Scr.instance.i_StageRound + StageManager_Scr.instance.i_StageClearLevel > 1000)
                {
                    i_Gold = 0;
                    i_Exp = (int)RespawnManager.instance.RespawnData[1000]["Boss_EXP"] + ((StageManager_Scr.instance.i_StageRound - 1000) * 7);
                    f_FullPoint = Random.Range((int)RespawnManager.instance.RespawnData[1000]["Boss_Min_HP"], (int)RespawnManager.instance.RespawnData[1000]["Boss_Max_HP"] + 1) + ((StageManager_Scr.instance.i_StageRound - 1000) * 60);
                    i_Cash = 0;
                    f_Atk = 9999;
                }
                else
                {
                    i_Gold = 0;
                    i_Exp = (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageRound - 1 + StageManager_Scr.instance.i_StageClearLevel]["Boss_EXP"];
                    f_FullPoint = Random.Range((int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageRound - 1 + StageManager_Scr.instance.i_StageClearLevel]["Boss_Min_HP"], (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageRound - 1 + StageManager_Scr.instance.i_StageClearLevel]["Boss_Max_HP"] + 1);
                    i_Cash = 0;
                    f_Atk = 9999;
                }
            }
        }
        f_HitPoint = f_FullPoint;
    }

    public void setPos_Forward() // 앞으로 움직일 위치 설정
    {
        v_before_movePos = transform.position;
        v_after_movePos = (transform.position + new Vector3(0, 0, -1f));
    }

    public void ResetVar(bool EventSpawn) // 위치, 정보 초기화
    {
        v_before_movePos = transform.position;
        if (!EventSpawn)
        {
            if (!b_YongyeogBlock)
            {
                v_after_movePos = (transform.position + new Vector3(0, 0, -1f));
            }
            else
            {
                v_after_movePos = transform.position;
            }
        }
        else
        {
            v_after_movePos = transform.position;
        }


        f_HitPoint = f_FullPoint;
        i_HP = transform.GetChild(0).GetChild(1).GetChild(1).transform.GetComponent<Image>();
        i_HP.fillAmount = (f_HitPoint / f_FullPoint);
        t_HP = transform.GetChild(0).GetChild(1).GetChild(2).transform.GetComponent<Text>();
        t_HP.text = f_HitPoint.ToString();
        anim_myani = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
    }
        

    public void Explosion_Cube() // 파편 활성화
    {
        if (b_CoinBlock || b_DiaBlock || b_CabinetBlock || b_DocumentboxBlock)
        {
            for (int i = 0; i < g_DieCube.transform.GetChild(0).childCount; i++)
            {
                GameObject Debris_clone = g_DieCube.transform.GetChild(0).GetChild(i).gameObject;
                if (!Debris_clone.activeSelf)
                {
                    Debris_clone.SetActive(true);
                    Debris_clone.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                    for (int j = 0; j < Debris_clone.transform.childCount; j++)
                    {
                        Rigidbody Cube_rigids = Debris_clone.transform.GetChild(j).GetComponent<Rigidbody>();
                        Cube_rigids.AddForce((Debris_clone.transform.GetChild(j).position - v_DieDir).normalized * 0.7f, ForceMode.Impulse);
                    }
                    break;
                }
            }
        }
        else if (b_ComputerBlock || b_JuimBlock || b_DealiBlock || b_GwajangBlock || b_BujangBlock)
        {
            for (int i = 0; i < g_DieCube.transform.GetChild(1).childCount-5; i++)
            {
                GameObject Debris_clone = g_DieCube.transform.GetChild(1).GetChild(i).gameObject;
                if (!Debris_clone.activeSelf)
                {
                    Debris_clone.SetActive(true);
                    Debris_clone.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                    for (int j = 0; j < Debris_clone.transform.childCount; j++)
                    {
                        Rigidbody Cube_rigids = Debris_clone.transform.GetChild(j).GetComponent<Rigidbody>();
                        Cube_rigids.AddForce((Debris_clone.transform.GetChild(j).position - v_DieDir).normalized * 0.7f, ForceMode.Impulse);
                    }
                    break;
                }
            }
        }
        else if (b_YongyeogBlock)
        {
            for (int i = 0; i < g_DieCube.transform.GetChild(2).childCount-3; i++)
            {
                GameObject Debris_clone = g_DieCube.transform.GetChild(2).GetChild(i).gameObject;
                if (!Debris_clone.activeSelf)
                {
                    Debris_clone.SetActive(true);
                    Debris_clone.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                    for (int j = 0; j < Debris_clone.transform.childCount; j++)
                    {
                        Rigidbody Cube_rigids = Debris_clone.transform.GetChild(j).GetComponent<Rigidbody>();
                        Cube_rigids.AddForce((Debris_clone.transform.GetChild(j).position - v_DieDir).normalized * 0.7f, ForceMode.Impulse);
                    }
                    break;
                }
            }
        }
        else if (b_BossBlock)
        {
            for (int i = 0; i < g_DieCube.transform.GetChild(3).childCount; i++)
            {
                GameObject Debris_clone = g_DieCube.transform.GetChild(3).GetChild(i).gameObject;
                if (!Debris_clone.activeSelf)
                {
                    Debris_clone.SetActive(true);
                    Debris_clone.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                    for (int j = 0; j < Debris_clone.transform.childCount; j++)
                    {
                        Rigidbody Cube_rigids = Debris_clone.transform.GetChild(j).GetComponent<Rigidbody>();
                        Cube_rigids.AddForce((Debris_clone.transform.GetChild(j).position - v_DieDir).normalized * 0.7f, ForceMode.Impulse);
                    }
                    break;
                }
            }
        }
    }

    public void ApplySkill(int SkillNum)
    {
        MissionChecker();
        if ((SkillNum == 3 && b_BossBlock) || (SkillNum == 3 && b_onBossBlock))
        {
            transform.GetChild(0).GetComponent<ApplyBossSkill>().setPos_Back();
        }
        else
        {
            t_hitScore.color = new Color(0, 0, 0, 0);

            Vector3 screenPos = Button_Option.instance.Cam.GetComponent<Camera>().WorldToScreenPoint(transform.position);
            GameObject go_Text_Exp = Button_Option.instance.go_TextGold_Dia_Exp.transform.GetChild(2).gameObject;

            // 경험치 획득 애니메이션
            if (!TutorialManager.instance.b_FirstPlaying)
            {
                for (int i = 0; i < go_Text_Exp.transform.childCount; i++)
                {
                    if (go_Text_Exp.transform.GetChild(i).gameObject.activeSelf == false)
                    {
                        GameObject T_Exp = go_Text_Exp.transform.GetChild(i).gameObject;
                        Vector2 ExpPos;
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(T_Exp.transform.GetComponent<RectTransform>(), screenPos, Camera.main, out ExpPos);
                        T_Exp.SetActive(true);
                        T_Exp.transform.localPosition = ExpPos;
                        T_Exp.transform.GetChild(0).GetComponent<Animator>().Play("GetExp");
                        T_Exp.transform.GetChild(0).GetComponent<Text>().text = "+ " + i_Exp.ToString();
                        break;
                    }
                    else if (i >= go_Text_Exp.transform.childCount - 1)
                    {
                        // 추가 생성하는 곳 
                        print("경험치 문구 더 생성해놓자");
                    }
                }
            }

            // 스킬로 인한 파편 생성 및 날라가는 방향 설정
            if (b_CabinetBlock || b_DocumentboxBlock)
            {
                for (int i = 0; i < g_DieCube.transform.GetChild(0).childCount; i++)
                {
                    GameObject Debris_clone = g_DieCube.transform.GetChild(0).GetChild(i).gameObject;
                    if (!Debris_clone.activeSelf)
                    {
                        Debris_clone.SetActive(true);
                        Debris_clone.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                        for (int j = 0; j < Debris_clone.transform.childCount; j++)
                        {
                            Rigidbody Cube_rigids = Debris_clone.transform.GetChild(j).GetComponent<Rigidbody>();
                            Cube_rigids.AddForce((Debris_clone.transform.GetChild(j).position - (transform.position + new Vector3(0, 0, -1f))).normalized * 0.6f, ForceMode.Impulse);
                        }
                        break;
                    }
                }
            }
            else if (b_ComputerBlock || b_JuimBlock || b_DealiBlock || b_GwajangBlock || b_BujangBlock)
            {
                for (int i = 0; i < g_DieCube.transform.GetChild(1).childCount; i++)
                {
                    GameObject Debris_clone = g_DieCube.transform.GetChild(1).GetChild(i).gameObject;
                    if (!Debris_clone.activeSelf)
                    {
                        Debris_clone.SetActive(true);
                        Debris_clone.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                        for (int j = 0; j < Debris_clone.transform.childCount; j++)
                        {
                            Rigidbody Cube_rigids = Debris_clone.transform.GetChild(j).GetComponent<Rigidbody>();
                            Cube_rigids.AddForce((Debris_clone.transform.GetChild(j).position - (transform.position + new Vector3(0, 0, -1f))).normalized * 0.6f, ForceMode.Impulse);
                        }
                        break;
                    }
                }
            }
            else if (b_YongyeogBlock)
            {
                for (int i = 0; i < g_DieCube.transform.GetChild(2).childCount; i++)
                {
                    GameObject Debris_clone = g_DieCube.transform.GetChild(2).GetChild(i).gameObject;
                    if (!Debris_clone.activeSelf)
                    {
                        Debris_clone.SetActive(true);
                        Debris_clone.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                        for (int j = 0; j < Debris_clone.transform.childCount; j++)
                        {
                            Rigidbody Cube_rigids = Debris_clone.transform.GetChild(j).GetComponent<Rigidbody>(); 
                            Cube_rigids.AddForce((Debris_clone.transform.GetChild(j).position - (transform.position + new Vector3(0, 0, -1f))).normalized * 0.6f, ForceMode.Impulse);
                        }
                        break;
                    }
                }
            }

            SoundManager_sfx.instance.PlaySE("Brokenblock", false);

            for (int i = 0; i < b_Line.Length; i++)
            {
                if (b_Line[i])
                {
                    switch (i)
                    {
                        case 0:
                            RespawnManager.instance.Line0.Remove(gameObject);
                            break;

                        case 1:
                            RespawnManager.instance.Line1.Remove(gameObject);
                            break;

                        case 2:
                            RespawnManager.instance.Line2.Remove(gameObject);
                            break;

                        case 3:
                            RespawnManager.instance.Line3.Remove(gameObject);
                            break;

                        case 4:
                            RespawnManager.instance.Line4.Remove(gameObject);
                            break;

                        case 5:
                            RespawnManager.instance.Line5.Remove(gameObject);
                            break;

                        case 6:
                            RespawnManager.instance.Line6.Remove(gameObject);
                            break;

                        case 7:
                            RespawnManager.instance.Line7.Remove(gameObject);
                            break;

                        case 8:
                            RespawnManager.instance.Line8.Remove(gameObject);
                            break;

                        case 9:
                            RespawnManager.instance.Line9.Remove(gameObject);
                            break;

                        case 10:
                            RespawnManager.instance.Line10.Remove(gameObject);
                            break;

                        case 11:
                            RespawnManager.instance.Line11.Remove(gameObject);
                            break;

                        default:
                            break;
                    }
                    b_Line[i] = false;
                }
            } // 죽을 때 자신이 속한 라인리스트에서 자신 제거

            if (!TutorialManager.instance.b_FirstPlaying)
            {
                if (RespawnManager.instance.b_NomalGame && RespawnManager.instance.AllBlock.Count == 1) // 레벨 초기화
                {
                    RewardManager_Scr.instance.b_isEnd = true;
                    StageManager_Scr.instance.G_TimerBar.SetActive(false);
                    Player_Input.instance.resetRound();
                    //Player_Input.instance.PlayerState = Player_Input.State.End_Round;

                    Player_Input.instance.g_ButtonShield.SetActive(true);
                    if (!Button_Option.instance.b_WindowOn)
                    {
                        Time.timeScale = 0.3f;
                    }
                    RespawnManager.instance.timer_Lock = true;
                    go_ClearText = transform.parent.parent.parent.GetChild(6).gameObject;
                    go_ClearText.SetActive(true);
                    go_ClearText.transform.GetChild(0).GetComponent<Animator>().Play("Text_GoodPingpong");
                    go_ClearText.transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
                    Invoke("ani_ClearText", 1.5f);
                    Invoke("Win_RwardwindowOn", 1.5f);
                }
                else if (RespawnManager.instance.ActiveBlock.Count == 1) // 라운드 초기화
                {
                    Player_Input.instance.resetRound();
                    Invoke("DamagetoPlayer_true", 0.5f);
                }


                if (RespawnManager.instance.b_NomalGame)
                {
                    ExpManager_Scr.instance.IncreaseExp(i_Exp);
                }
                else
                {
                    ExpManager_Scr.instance.IncreaseRankPoint(i_Exp);
                }
            }
            else if (RespawnManager.instance.b_NomalGame && RespawnManager.instance.AllBlock.Count == 1 && TutorialManager.instance.i_TutorialCount > 4)
            {
                RewardManager_Scr.instance.b_isEnd = true;
                StageManager_Scr.instance.G_TimerBar.SetActive(false);
                Player_Input.instance.g_ButtonShield.SetActive(true);

                if (!Button_Option.instance.b_WindowOn)
                    Time.timeScale = 0.3f;
                RespawnManager.instance.timer_Lock = true;
                go_ClearText = transform.parent.parent.parent.GetChild(6).gameObject;
                go_ClearText.SetActive(true);
                go_ClearText.transform.GetChild(0).GetComponent<Animator>().Play("Text_GoodPingpong");
                go_ClearText.transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
                Invoke("ani_ClearText", 1.5f);
                Invoke("Win_RwardwindowOn", 1.5f);
                ExpManager_Scr.instance.IncreaseExp(i_Exp);
            }

            if (ExpManager_Scr.instance.f_nowExp >= ExpManager_Scr.instance.f_fullExp)
            {
                b_SkillLevelUp = true;
            }
            else
            {
                Time.timeScale = 1;
            }

            RespawnManager.instance.ActiveBlock.Remove(gameObject);
            RespawnManager.instance.AllBlock.Remove(gameObject);
            if (b_SpeechBlock)
            {
                RespawnManager.instance.SpeechBlock.Remove(gameObject);
            }
            gameObject.SetActive(false);
        }
    }

    public void ApplyDamage(float damage, bool isCri, int bounceCount, bool CanAttack, int PlusAbility, int HeroType)
    {
        if (StageManager_Scr.instance.myAbility < StageManager_Scr.instance.needAbility)
        {
            StageManager_Scr.instance.myAbility += 1 + PlusAbility + Inventory_Scr.instance.AddAbility;
        }
        Player_Input.instance.f_BackTime = 3.0f;
        Player_Input.instance.b_BallBack = true;
        f_HitPoint -= damage;
        i_HP.fillAmount = (f_HitPoint / f_FullPoint);
        t_HP.text = f_HitPoint.ToString();
        if (f_HitPoint <= 0)
        {
            if (HeroType == 21 || HeroType == 22)
            {
                for (int i = 0; i < RespawnManager.instance.selfdestructParticle.Count; i++)
                {
                    if (!RespawnManager.instance.selfdestructParticle[i].isPlaying)
                    {
                        RespawnManager.instance.selfdestructParticle[i].transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                        RespawnManager.instance.selfdestructParticle[i].Play();
                        break;
                    }
                   
                    else
                    {
                        if( i >= RespawnManager.instance.selfdestructParticle.Count - 1)
                        {
                            GameObject particle_obj = Instantiate(RespawnManager.instance.selfdestructParticle_prefabs, RespawnManager.instance.selfdestructParticle[0].transform.parent);
                            ParticleSystem par = particle_obj.transform.GetComponent<ParticleSystem>();
                            RespawnManager.instance.selfdestructParticle.Add(par);
                            par.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                            par.Play();
                        }
                    }
                }                
            }

            if (CanAttack && go_HitBall != null)
            {
                Player_Input.instance.OpenNewhead();
                int num = go_HitBall.transform.GetComponent<Ball>().i_ListNum + 1;
                if (num < Player_Input.instance.ShotBall.Count)
                {
                    Player_Input.instance.ShotBall[num].transform.GetComponent<Ball>().b_Head = true;
                    Player_Input.instance.ShotBall[num].transform.GetComponent<Ball>().deletList();                    
                }
            }

            if (ScenarioEventManager_Scr.instance.i_EventNum != 0)
            {
                if (b_isEventBlock && ScenarioEventManager_Scr.instance.i_EventNum != 5)
                {
                    go_StreakImage = transform.parent.parent.parent.GetChild(5).gameObject;
                    go_StreakImage.SetActive(true);
                    go_StreakImage.transform.GetChild(0).GetComponent<Animator>().Play("Text_GoodPingpong");
                    go_StreakImage.transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
                }
                if (ScenarioEventManager_Scr.instance.i_EventNum == 5)
                {
                    if (b_isEventBlock)
                    {
                        if (CanAttack)
                        {
                            // 미션성공
                            ScenarioEventManager_Scr.instance.setReward();
                            Time.timeScale = 0;
                        }
                        else
                        {
                            // 미션 실패
                            ScenarioEventManager_Scr.instance.MissionFail();
                        }
                    }
                }
                else if (ScenarioEventManager_Scr.instance.i_EventNum != 3)
                {
                    if (b_isEventBlock)
                    {
                        ScenarioEventManager_Scr.instance.i_NowQuantity++;
                        if (ScenarioEventManager_Scr.instance.i_NowQuantity == ScenarioEventManager_Scr.instance.i_GoalQuantity)
                        {
                            // 미션성공
                            ScenarioEventManager_Scr.instance.setReward();
                            Time.timeScale = 0;
                        }
                    }
                    else if (!b_isEventBlock && ScenarioEventManager_Scr.instance.i_EventNum == 1 && !CanAttack)
                    {
                        // 미션 실패
                        ScenarioEventManager_Scr.instance.MissionFail();
                    }
                }
                else
                {
                    if (b_isEventBlock)
                    {
                        if (bounceCount == ScenarioEventManager_Scr.instance.i_EventBounce || CanAttack)
                        {
                            ScenarioEventManager_Scr.instance.i_NowQuantity++;
                            if (ScenarioEventManager_Scr.instance.i_NowQuantity == ScenarioEventManager_Scr.instance.i_GoalQuantity)
                            {
                                // 미션성공
                                ScenarioEventManager_Scr.instance.setReward();
                                Time.timeScale = 0;
                            }
                        }
                        else
                        {
                            // 미션 실패
                            ScenarioEventManager_Scr.instance.MissionFail();
                        }
                    }
                }
            }

            MissionChecker();

            Player_Input.instance.i_BlockStreak++;
            if (TutorialManager.instance.i_TutorialCount > 5)
            {
                switch (Player_Input.instance.i_BlockStreak)
                {
                    case 5:
                        // 포인트 획득 애니메이션
                        Vector3 screenPos_Streak0 = Button_Option.instance.Cam.GetComponent<Camera>().WorldToScreenPoint(transform.position);
                        GameObject go_Text_Point0 = Button_Option.instance.go_TextGold_Dia_Exp.transform.GetChild(0).gameObject;
                        for (int i = 0; i < go_Text_Point0.transform.childCount; i++)
                        {
                            if (go_Text_Point0.transform.GetChild(i).gameObject.activeSelf == false)
                            {
                                if (!b_CoinBlock)
                                {
                                    GameObject T_Point = go_Text_Point0.transform.GetChild(i).gameObject;
                                    Vector2 PointPos = Vector2.zero;
                                    RectTransformUtility.ScreenPointToLocalPointInRectangle(T_Point.transform.GetComponent<RectTransform>(), screenPos_Streak0, Camera.main, out PointPos);
                                    T_Point.SetActive(true);
                                    T_Point.transform.localPosition = new Vector3(PointPos.x, PointPos.y, +100);
                                    T_Point.transform.GetChild(0).GetComponent<Animator>().Play("GetCoin");
                                    int coin = (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Coin_Reward"];
                                    T_Point.transform.GetChild(0).GetComponent<Icon_GetCoin>().i_getGold = (int)(coin * 0.05f);
                                    break;
                                }
                                else if (b_CoinBlock)
                                {
                                    int coin = (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Coin_Reward"];
                                    i_Gold += (int)(coin * 0.05f);
                                }
                            }
                        }

                        // 연속처치 글자 이미지 출력
                        if (RespawnManager.instance.AllBlock.Count > 1 && !b_BossBlock)
                        {
                            go_StreakImage = transform.parent.parent.parent.GetChild(7).gameObject;
                            go_StreakImage.SetActive(true);
                            go_StreakImage.transform.GetChild(0).GetComponent<Animator>().Play("Text_GoodPingpong");
                            go_StreakImage.transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
                            break;
                        }
                        break;

                    case 10:
                        // 포인트 획득 애니메이션
                        Vector3 screenPos_Streak1 = Button_Option.instance.Cam.GetComponent<Camera>().WorldToScreenPoint(transform.position);
                        GameObject go_Text_Point1 = Button_Option.instance.go_TextGold_Dia_Exp.transform.GetChild(0).gameObject;
                        for (int i = 0; i < go_Text_Point1.transform.childCount; i++)
                        {
                            if (go_Text_Point1.transform.GetChild(i).gameObject.activeSelf == false)
                            {
                                if (!b_CoinBlock)
                                {
                                    GameObject T_Point = go_Text_Point1.transform.GetChild(i).gameObject;
                                    Vector2 PointPos = Vector2.zero;
                                    RectTransformUtility.ScreenPointToLocalPointInRectangle(T_Point.transform.GetComponent<RectTransform>(), screenPos_Streak1, Camera.main, out PointPos);
                                    T_Point.SetActive(true);
                                    T_Point.transform.localPosition = new Vector3(PointPos.x, PointPos.y, +100);
                                    T_Point.transform.GetChild(0).GetComponent<Animator>().Play("GetCoin");
                                    int coin = (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Coin_Reward"];
                                    T_Point.transform.GetChild(0).GetComponent<Icon_GetCoin>().i_getGold = (int)(coin * 0.1f);
                                    break;
                                }
                                else if (b_CoinBlock)
                                {
                                    int coin = (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Coin_Reward"];
                                    i_Gold += (int)(coin * 0.1f);
                                }
                            }
                        }

                        // 연속처치 글자 이미지 출력
                        if (RespawnManager.instance.AllBlock.Count > 1 && !b_BossBlock)
                        {
                            go_StreakImage = transform.parent.parent.parent.GetChild(8).gameObject;
                            go_StreakImage.SetActive(true);
                            go_StreakImage.transform.GetChild(0).GetComponent<Animator>().Play("Text_GoodPingpong");
                            go_StreakImage.transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
                            break;
                        }
                        break;

                    case 15:
                        // 포인트 획득 애니메이션
                        Vector3 screenPos_Streak2 = Button_Option.instance.Cam.GetComponent<Camera>().WorldToScreenPoint(transform.position);
                        GameObject go_Text_Point2 = Button_Option.instance.go_TextGold_Dia_Exp.transform.GetChild(0).gameObject;
                        for (int i = 0; i < go_Text_Point2.transform.childCount; i++)
                        {
                            if (go_Text_Point2.transform.GetChild(i).gameObject.activeSelf == false)
                            {
                                if (!b_CoinBlock)
                                {
                                    GameObject T_Point = go_Text_Point2.transform.GetChild(i).gameObject;
                                    Vector2 PointPos = Vector2.zero;
                                    RectTransformUtility.ScreenPointToLocalPointInRectangle(T_Point.transform.GetComponent<RectTransform>(), screenPos_Streak2, Camera.main, out PointPos);
                                    T_Point.SetActive(true);
                                    T_Point.transform.localPosition = new Vector3(PointPos.x, PointPos.y, +100);
                                    T_Point.transform.GetChild(0).GetComponent<Animator>().Play("GetCoin");
                                    int coin = (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Coin_Reward"];
                                    T_Point.transform.GetChild(0).GetComponent<Icon_GetCoin>().i_getGold = (int)(coin * 0.2f);
                                    break;
                                }
                                else if (b_CoinBlock)
                                {
                                    int coin = (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Coin_Reward"];
                                    i_Gold += (int)(coin * 0.2f);
                                }
                            }
                        }

                        // 연속처치 글자 이미지 출력
                        if (RespawnManager.instance.AllBlock.Count > 1 && !b_BossBlock)
                        {
                            go_StreakImage = transform.parent.parent.parent.GetChild(9).gameObject;
                            go_StreakImage.SetActive(true);
                            go_StreakImage.transform.GetChild(0).GetComponent<Animator>().Play("Text_GoodPingpong");
                            go_StreakImage.transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
                            break;
                        }
                        break;

                    case 20:
                        // 포인트 획득 애니메이션
                        Vector3 screenPos_Streak3 = Button_Option.instance.Cam.GetComponent<Camera>().WorldToScreenPoint(transform.position);
                        GameObject go_Text_Point3 = Button_Option.instance.go_TextGold_Dia_Exp.transform.GetChild(0).gameObject;
                        for (int i = 0; i < go_Text_Point3.transform.childCount; i++)
                        {
                            if (go_Text_Point3.transform.GetChild(i).gameObject.activeSelf == false)
                            {
                                if (!b_CoinBlock)
                                {
                                    GameObject T_Point = go_Text_Point3.transform.GetChild(i).gameObject;
                                    Vector2 PointPos = Vector2.zero;
                                    RectTransformUtility.ScreenPointToLocalPointInRectangle(T_Point.transform.GetComponent<RectTransform>(), screenPos_Streak3, Camera.main, out PointPos);
                                    T_Point.SetActive(true);
                                    T_Point.transform.localPosition = new Vector3(PointPos.x, PointPos.y, +100);
                                    T_Point.transform.GetChild(0).GetComponent<Animator>().Play("GetCoin");
                                    int coin = (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Coin_Reward"];
                                    T_Point.transform.GetChild(0).GetComponent<Icon_GetCoin>().i_getGold = (int)(coin * 0.3f);
                                    break;
                                }
                                else if (b_CoinBlock)
                                {
                                    int coin = (int)RespawnManager.instance.RespawnData[StageManager_Scr.instance.i_StageLevel - 1]["Coin_Reward"];
                                    i_Gold += (int)(coin * 0.3f);
                                }
                            }
                        }

                        // 연속처치 글자 이미지 출력
                        if (RespawnManager.instance.AllBlock.Count > 1 && !b_BossBlock)
                        {
                            go_StreakImage = transform.parent.parent.parent.GetChild(10).gameObject;
                            go_StreakImage.SetActive(true);
                            go_StreakImage.transform.GetChild(0).GetComponent<Animator>().Play("Text_GoodPingpong");
                            go_StreakImage.transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
                            break;
                        }
                        break;

                    default:
                        break;
                }
            }

            transform.GetChild(0).localPosition = originPos;
            t_hitScore.color = new Color(0, 0, 0, 0);
            b_onBossBlock = false;
            transform.GetChild(1).GetComponent<Text>().color = new Color(1, 1, 1, 0);
            TranslateManager_Scr.instance.ChangeFont(transform.GetChild(1).GetComponent<Text>());
            if (b_BossBlock)
            {
                // 보스의 용역소환 스킬 미발동하고 죽을 시 용역 블럭들을 종류별 리스트, 올블럭 리스트에서 삭제 시킨다.
                if (!RespawnManager.instance.b_RespawnBulldog)
                {
                    for (int i = 0; i < RespawnManager.instance.G_enemy.transform.GetChild(9).childCount; i++)
                    {
                        RespawnManager.instance.AllBlock.Remove(RespawnManager.instance.G_enemy.transform.GetChild(9).GetChild(i).gameObject);
                        RespawnManager.instance.BulldogBlock.Remove(RespawnManager.instance.G_enemy.transform.GetChild(9).GetChild(i).gameObject);
                    }
                }
                for (int i = 0; i < RespawnManager.instance.ActiveBlock.Count; i++)
                {
                    RespawnManager.instance.ActiveBlock[i].transform.GetComponent<BlockControl>().b_onBossBlock = false;
                }
                Player_Input.instance.g_ButtonShield.SetActive(true);
                if (!Button_Option.instance.b_WindowOn)
                    Time.timeScale = 0.3f;
                go_ClearText = transform.parent.parent.GetChild(5).gameObject;
                go_ClearText.SetActive(true);
                go_ClearText.transform.GetChild(0).GetComponent<Animator>().Play("Text_GoodPingpong");
                go_ClearText.transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
                Invoke("ani_ClearText", 1.5f);
                if (RespawnManager.instance.b_NomalGame && RespawnManager.instance.AllBlock.Count == 1)
                {
                    RewardManager_Scr.instance.b_isEnd = true;
                    StageManager_Scr.instance.G_TimerBar.SetActive(false);
                    Invoke("Win_RwardwindowOn", 1.5f);
                }
                Player_Input.instance.f_BackTime = 3.0f;
                Player_Input.instance.b_BallBack = false;
                Player_Input.instance.b_IsBack = true;
            }
            else
            {
                if (RespawnManager.instance.b_NomalGame && RespawnManager.instance.AllBlock.Count == 1 && TutorialManager.instance.i_TutorialCount > 4)
                {
                    RewardManager_Scr.instance.b_isEnd = true;
                    StageManager_Scr.instance.G_TimerBar.SetActive(false);
                    if (TutorialManager.instance.i_TutorialCount >= 8)
                        Player_Input.instance.g_ButtonShield.SetActive(true);

                    if (!Button_Option.instance.b_WindowOn)
                        Time.timeScale = 0.3f;
                    RespawnManager.instance.timer_Lock = true;
                    go_ClearText = transform.parent.parent.parent.GetChild(6).gameObject;
                    go_ClearText.SetActive(true);
                    go_ClearText.transform.GetChild(0).GetComponent<Animator>().Play("Text_GoodPingpong");
                    go_ClearText.transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
                    Invoke("ani_ClearText", 1.5f);
                    Invoke("Win_RwardwindowOn", 1.5f);
                }
            }
            Vector3 screenPos = Button_Option.instance.Cam.GetComponent<Camera>().WorldToScreenPoint(transform.position);
            GameObject go_TextGold_Dia_Exp = Button_Option.instance.go_TextGold_Dia_Exp;
            GameObject go_Text_Point = go_TextGold_Dia_Exp.transform.GetChild(0).gameObject;
            GameObject go_Text_Dia = go_TextGold_Dia_Exp.transform.GetChild(1).gameObject;
            GameObject go_Text_Exp = go_TextGold_Dia_Exp.transform.GetChild(2).gameObject;

            if (b_CoinBlock)
            {
                // 포인트 획득 애니메이션
                for (int i = 0; i < go_Text_Point.transform.childCount; i++)
                {
                    if (go_Text_Point.transform.GetChild(i).gameObject.activeSelf == false)
                    {
                        GameObject T_Point = go_Text_Point.transform.GetChild(i).gameObject;
                        Vector2 PointPos = Vector2.zero;
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(T_Point.transform.GetComponent<RectTransform>(), screenPos, Camera.main, out PointPos);
                        T_Point.SetActive(true);
                        T_Point.transform.localPosition = new Vector3(PointPos.x, PointPos.y, +100);
                        T_Point.transform.GetChild(0).GetComponent<Animator>().Play("GetCoin");
                        T_Point.transform.GetChild(0).GetComponent<Icon_GetCoin>().i_getGold = i_Gold;
                        
                        break;
                    }
                    else if (i >= go_Text_Point.transform.childCount - 1)
                    {
                        // 추가 생성하는 곳 
                    }
                }
            }
            else if (b_DiaBlock)
            {
                // 다이아 획득 애니메이션
                for (int i = 0; i < go_Text_Dia.transform.childCount; i++)
                {
                    if (go_Text_Dia.transform.GetChild(i).gameObject.activeSelf == false)
                    {
                        GameObject T_Dia = go_Text_Dia.transform.GetChild(i).gameObject;
                        Vector2 PointPos = Vector2.zero;
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(T_Dia.transform.GetComponent<RectTransform>(), screenPos, Camera.main, out PointPos);
                        T_Dia.SetActive(true);
                        T_Dia.transform.localPosition = new Vector3(PointPos.x, PointPos.y, +100);
                        T_Dia.transform.GetChild(0).GetComponent<Animator>().Play("GetCoin");
                        T_Dia.transform.GetChild(0).GetComponent<Icon_GetCoin>().i_getCash = i_Cash;
                        break;
                    }
                    else if (i >= go_Text_Dia.transform.childCount - 1)
                    {
                        // 추가 생성하는 곳 
                    }
                }
            }
            else
            {
                if (TutorialManager.instance.i_TutorialCount > 5)
                {
                    // 경험치 획득 애니메이션
                    for (int i = 0; i < go_Text_Exp.transform.childCount; i++)
                    {
                        if (go_Text_Exp.transform.GetChild(i).gameObject.activeSelf == false)
                        {
                            GameObject T_Exp = go_Text_Exp.transform.GetChild(i).gameObject;
                            Vector2 ExpPos = Vector2.zero;
                            RectTransformUtility.ScreenPointToLocalPointInRectangle(T_Exp.transform.GetComponent<RectTransform>(), screenPos, Camera.main, out ExpPos);
                            T_Exp.SetActive(true);
                            T_Exp.transform.localPosition = ExpPos;
                            T_Exp.transform.GetChild(0).GetComponent<Animator>().Play("GetExp");
                            T_Exp.transform.GetChild(0).GetComponent<Text>().text = "+ " + i_Exp.ToString();
                            break;
                        }
                        else if (i >= go_Text_Exp.transform.childCount - 1)
                        {
                            // 추가 생성하는 곳 
                        }
                    }
                    if (RespawnManager.instance.b_NomalGame)
                    {
                        ExpManager_Scr.instance.IncreaseExp(i_Exp);
                    }
                    else
                    {
                        ExpManager_Scr.instance.IncreaseRankPoint(i_Exp);
                    }
                }
            }

            RespawnManager.instance.AllBlock.Remove(gameObject);
            RespawnManager.instance.ActiveBlock.Remove(gameObject);
            if (b_SpeechBlock)
            {
                RespawnManager.instance.SpeechBlock.Remove(gameObject);
            }
            Explosion_Cube();
            SoundManager_sfx.instance.PlaySE("Brokenblock", false);

            for (int i = 0; i < b_Line.Length; i++)
            {
                if (b_Line[i])
                {
                    switch (i)
                    {
                        case 0:
                            RespawnManager.instance.Line0.Remove(gameObject);
                            break;

                        case 1:
                            RespawnManager.instance.Line1.Remove(gameObject);
                            break;

                        case 2:
                            RespawnManager.instance.Line2.Remove(gameObject);
                            break;

                        case 3:
                            RespawnManager.instance.Line3.Remove(gameObject);
                            break;

                        case 4:
                            RespawnManager.instance.Line4.Remove(gameObject);
                            break;

                        case 5:
                            RespawnManager.instance.Line5.Remove(gameObject);
                            break;

                        case 6:
                            RespawnManager.instance.Line6.Remove(gameObject);
                            break;

                        case 7:
                            RespawnManager.instance.Line7.Remove(gameObject);
                            break;

                        case 8:
                            RespawnManager.instance.Line8.Remove(gameObject);
                            break;

                        case 9:
                            RespawnManager.instance.Line9.Remove(gameObject);
                            break;

                        case 10:
                            RespawnManager.instance.Line10.Remove(gameObject);
                            break;

                        case 11:
                            RespawnManager.instance.Line11.Remove(gameObject);
                            break;

                    }
                    b_Line[i] = false;
                }
            }
            gameObject.SetActive(false);

        }
        else // 공격 받고 체력 남아있을때
        {

            if (b_SpeechBlock && !b_isSpeech)
            {
                if (!RespawnManager.instance.go_HitSpeechBubble.transform.GetComponent<SpeechBubble>().b_isSpeech)
                {
                    b_isSpeech = true;
                    transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetBool("isSpeaking", true);
                    GameObject SpeechBubble = RespawnManager.instance.go_HitSpeechBubble.transform.gameObject;
                    SpeechBubble SpeechBubbleScript = SpeechBubble.GetComponent<SpeechBubble>();
                    SpeechBubbleScript.go_TartgetBlockPos = go_SpeechPos;
                    SpeechBubbleScript.go_TartgetBlock = transform.gameObject;
                    SpeechBubbleScript.StartSpeech(3, i_Kinds);
                }
            }

            if (f_ScreamColtime <= 0)
            {
                SoundManager_Voice.instance.PlaySE_block(0, i_Kinds);
                f_ScreamColtime = 0.1f;
            }

            go_Desk_mesh.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 1, 1));
            go_Char_mesh.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 1, 1));

            go_Desk_mesh.GetComponent<Renderer>().material.SetColor("_HColor", new Color(1, 1, 1));
            go_Char_mesh.GetComponent<Renderer>().material.SetColor("_HColor", new Color(1, 1, 1));

            go_Desk_mesh.GetComponent<Renderer>().material.SetColor("_SColor", new Color(1, 1, 1));
            go_Char_mesh.GetComponent<Renderer>().material.SetColor("_SColor", new Color(1, 1, 1));
            Invoke("resetMaterial", 0.1f);
            ShakeCoroutine = StartCoroutine(Shake(0.1f, 0.5f)); // 흔들림효과(강도, 지속시간)

            if (!isCri)
            {
                switch (HeroType)
                {
                    case 0: // 일반뎀 캔, 동료
                        anim_hittextani.Play("TextPingPong_Normal");
                        break;
                    case 1: // 히어로 깡 뎀지 주는 놈 1  3
                    case 3:
                        anim_hittextani.Play("TextPingPong_hero");
                        break;
                    case 2: // 어빌리티 2 4 6 8 10 12 14 16 18 20 
                    case 4:
                    case 6:
                    case 8:
                    case 10:
                    case 12:
                    case 14:
                    case 16:
                    case 18:
                    case 20:
                        anim_hittextani.Play("TextPingPong_ability");
                        break;
                    case 5: // 원거리 5 11 17
                    case 11:
                    case 17:
                        anim_hittextani.Play("TextPingPong_range");
                        break;
                    case 15: // 관통 15 19
                    case 19:
                        anim_hittextani.Play("TextPingPong_pass");
                        break;
                    case 7: // 점진적 7 9 13 
                    case 9:
                    case 13:
                        anim_hittextani.Play("TextPingPong_gradual");
                        break;
                }
            }
            else
            {
                // 크리뎀 히어로 동료 구분없이 1순위 
                anim_hittextani.Play("TextPingPong_Cri");
            }
            if (!b_CoinBlock && !b_CabinetBlock && !b_ComputerBlock && !b_DiaBlock && !b_DocumentboxBlock)
            {
                anim_myani.Play("TakeDamage");
            }
            if(damage > 0)
            {
                t_hitScore.text = damage.ToString();
            }
            else
            {
                t_hitScore.text = ((int)(1 + PlusAbility + Inventory_Scr.instance.AddAbility)).ToString();
            }
        }
    }

    public void MissionChecker()
    {
        for (int i = 0; i < 2; ++i)
        {
            MissionSlot_Scr slot = MissionManager_Scr.instance.G_MissionList[i].GetComponent<MissionSlot_Scr>();
            if (i == 0 && slot.b_GetMission && !MissionManager_Scr.instance.b_MissionSuccess[0])
            {
                if (b_JuimBlock && MissionManager_Scr.instance.i_MissionSlot[0] == 0)
                    MissionManager_Scr.instance.i_ClearMissionNum[0]++;
                else if (b_DealiBlock && MissionManager_Scr.instance.i_MissionSlot[0] == 1)
                    MissionManager_Scr.instance.i_ClearMissionNum[0]++;
                else if (b_GwajangBlock && MissionManager_Scr.instance.i_MissionSlot[0] == 2)
                    MissionManager_Scr.instance.i_ClearMissionNum[0]++;
                else if (b_BujangBlock && MissionManager_Scr.instance.i_MissionSlot[0] == 3)
                    MissionManager_Scr.instance.i_ClearMissionNum[0]++;
            }
            else if (i == 1 && slot.b_GetMission && !MissionManager_Scr.instance.b_MissionSuccess[1])
            {
                if (b_IsaBlock && MissionManager_Scr.instance.i_MissionSlot[1] == 0)
                    MissionManager_Scr.instance.i_ClearMissionNum[1]++;
                else if (b_SangmuBlock && MissionManager_Scr.instance.i_MissionSlot[1] == 1)
                    MissionManager_Scr.instance.i_ClearMissionNum[1]++;
                else if (b_jeonmuBlock && MissionManager_Scr.instance.i_MissionSlot[1] == 2)
                    MissionManager_Scr.instance.i_ClearMissionNum[1]++;
                else if (b_SajangBlock && MissionManager_Scr.instance.i_MissionSlot[1] == 3)
                    MissionManager_Scr.instance.i_ClearMissionNum[1]++;
            }
        }
    }
    public void ani_ClearText()
    {
        go_ClearText.SetActive(false);
    }
    public void Win_RwardwindowOn()
    {
        if (HPManager_Scr.instance.f_Hp > 0)
        {
            if (!RewardManager_Scr.instance.b_isEnd && !Button_Option.instance.b_Golobby)
                RewardManager_Scr.instance.b_isEnd = true;
            RewardManager_Scr.instance.RewardWindowOn();
        }
    }

    public void resetMaterial()
    {
        go_Desk_mesh.GetComponent<Renderer>().material.SetColor("_Color", new Color(191 / 255f, 204 / 255f, 255 / 255f));
        go_Char_mesh.GetComponent<Renderer>().material.SetColor("_Color", new Color(191 / 255f, 204 / 255f, 255 / 255f));

        go_Desk_mesh.GetComponent<Renderer>().material.SetColor("_HColor", new Color(255 / 255f, 208 / 255f, 208 / 255f));
        go_Char_mesh.GetComponent<Renderer>().material.SetColor("_HColor", new Color(255 / 255f, 208 / 255f, 208 / 255f));

        go_Desk_mesh.GetComponent<Renderer>().material.SetColor("_SColor", new Color(50 / 255f, 50 / 255f, 50 / 255f));
        go_Char_mesh.GetComponent<Renderer>().material.SetColor("_SColor", new Color(50 / 255f, 50 / 255f, 50 / 255f));
    }

    public void setDir(Vector3 Dir) // 파편에 힘가하는 방향 설정
    {
        v_DieDir = Dir;
    }

    public IEnumerator Shake(float _amount, float _duration) // 블럭이 피격시 몸체 흔들어주기
    {
        float timer = 0;
        while (timer <= _duration)
        {
            if (Time.timeScale != 0)
            {
                transform.GetChild(0).localPosition = (Vector3)Random.insideUnitCircle * _amount + originPos;
                timer += Time.deltaTime;
            }
            yield return null;
        }
        transform.GetChild(0).localPosition = originPos;
        StopCoroutine(ShakeCoroutine);
    }

    public void DamagetoPlayer_true()
    {
        DamagetoPlayer(true);
    }
    public void DamagetoPlayer_false()
    {
        DamagetoPlayer(false);  
    }
    public void DamagetoPlayer(bool nextRound)
    {
        if (nextRound)
        {
            RespawnManager.instance.ReadytoRoundStart();
        }
        else if (!nextRound)
        {
            BlockControl Block_script = RespawnManager.instance.Line0[0].transform.GetComponent<BlockControl>();
            Block_script.setPos_Forward();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Ball"))
        {
            var collisionPoint = other.ClosestPoint(transform.position);
            if (other.gameObject.transform.GetComponent<Ball>())
            {

                switch (other.gameObject.transform.GetComponent<Ball>().i_HeroType)
                {
                    case 0:
                        for (int i = 0; i < par_HitParticle.Length; i++)
                        {
                            if (!par_HitParticle[i].isPlaying)
                            {
                                par_HitParticle[i].transform.position = new Vector3(collisionPoint.x, collisionPoint.y + 0.4f, collisionPoint.z);
                                par_HitParticle[i].Play();
                                break;
                            }
                        }
                        break;

                    case 1:
                    case 7:
                    case 13:
                    case 15:
                    case 19:
                        for (int i = 0; i < par_BigHitParticle.Length; i++)
                        {
                            if (!par_BigHitParticle[i].isPlaying)
                            {
                                par_BigHitParticle[i].transform.position = new Vector3(collisionPoint.x, collisionPoint.y + 0.4f, collisionPoint.z);
                                par_BigHitParticle[i].Play();
                                break;
                            }
                        }
                        break;

                    case 3:
                    case 9:
                        for (int i = 0; i < par_ExplosionHitParticle.Length; i++)
                        {
                            if (!par_ExplosionHitParticle[i].isPlaying)
                            {
                                par_ExplosionHitParticle[i].transform.position = new Vector3(collisionPoint.x, collisionPoint.y + 0.4f, collisionPoint.z);
                                par_ExplosionHitParticle[i].Play();
                                break;
                            }
                        }
                        break;

                    case 2:
                    case 4:
                    case 6:
                    case 8:
                    case 10:
                    case 12:
                    case 14:
                    case 16:
                    case 18:
                    case 20:
                        for (int i = 0; i < par_AbilityHitParticle.Length; i++)
                        {
                            if (!par_AbilityHitParticle[i].isPlaying)
                            {
                                par_AbilityHitParticle[i].transform.position = new Vector3(collisionPoint.x, collisionPoint.y + 0.4f, collisionPoint.z);
                                par_AbilityHitParticle[i].Play();
                                break;
                            }
                        }
                        break;
                }
            }
            else
            {
                for (int i = 0; i < par_HitParticle.Length; i++)
                {
                    if (!par_HitParticle[i].isPlaying)
                    {
                        par_HitParticle[i].transform.position = new Vector3(collisionPoint.x, collisionPoint.y + 0.4f, collisionPoint.z);
                        par_HitParticle[i].Play();
                        break;
                    }
                }
            }
        }

        if (other.transform.CompareTag("Gate"))
        {
            SoundManager_sfx.instance.PlaySE("WallHit", false);

            if (b_isEventBlock && RespawnManager.instance.AllBlock.Count != 1)
            {
                if (HPManager_Scr.instance.f_Hp > f_Atk)
                {
                    if (ScenarioEventManager_Scr.instance.i_EventNum == 5)
                    {
                        // 미션성공
                        RespawnManager.instance.b_MissionSucc = true;
                        // ScenarioEventManager_Scr.instance.setReward();
                    }
                    else
                    {
                        // 미션실패
                        RespawnManager.instance.b_MissionFail = true;

                        //RespawnManager.instance.b_FailEvent = true;
                        // ScenarioEventManager_Scr.instance.MissionFail();
                    }
                }
            }
            for (int i = 0; i < other.transform.childCount; i++)
            {
                ParticleSystem hitParticle = other.transform.GetChild(0).GetChild(i).transform.GetComponent<ParticleSystem>();
                if (!hitParticle.isPlaying)
                {
                    other.transform.GetChild(0).GetChild(i).transform.position = (transform.position + new Vector3(0, 1f, 0));
                    hitParticle.Play();
                    break;
                }
            }
            b_Line[0] = false;
            if (RespawnManager.instance.Line0.Count <= 0 && (HPManager_Scr.instance.f_Hp > f_Atk))
            {
                Player_Input.instance.g_ButtonShield.SetActive(false);
                Invoke("DamagetoPlayer_true", 0.5f);
            }
            else if (RespawnManager.instance.Line0.Count >= 1 && (HPManager_Scr.instance.f_Hp > f_Atk))
            {
                Invoke("DamagetoPlayer_false", 0.5f);
            }
            v_DieDir = (transform.position + new Vector3(0, 0, -1f));
            Explosion_Cube();
            RespawnManager.instance.Line0.Remove(gameObject);
            RespawnManager.instance.ActiveBlock.Remove(gameObject);
            RespawnManager.instance.AllBlock.Remove(gameObject);
            if (b_SpeechBlock)
            {
                RespawnManager.instance.SpeechBlock.Remove(gameObject);
            }
            if (!b_CoinBlock && !b_DiaBlock)
            {
                HPManager_Scr.instance.TakeDamage(f_Atk);
            }

            if (RespawnManager.instance.AllBlock.Count == 0)
            {
                Invoke("Win_RwardwindowOn", 1.0f);
            }
            
            gameObject.SetActive(false);
        }
        else if (other.transform.CompareTag("Block_Top"))
        {
            b_onBossBlock = true;
        }
        else if (other.transform.CompareTag("WarningLine") && RespawnManager.instance.b_isRoundStart)
        {
            Button_Option.instance.Warning();
        }
        else if(other.transform.CompareTag("Wall_HitScore"))
        {
            if (!b_CoinBlock && !b_DiaBlock)
            {
                int num = other.transform.GetComponent<GateHitPos>().i_hitPosNum;
                Button_Option.instance.go_PlayerHitScore.transform.GetChild(num).GetComponent<Text>().text = f_Atk.ToString();
                Button_Option.instance.go_PlayerHitScore.transform.GetChild(num).GetComponent<Animator>().Play("Text_PlayerHitScore");
                //TranslateManager_Scr.instance.ChangeFont(Button_Option.instance.go_PlayerHitScore.transform.GetChild(num).GetComponent<Text>());
            }
        }

        if (other.transform.CompareTag("Line (0)"))
        {
            b_Line[0] = true;
            if (!RespawnManager.instance.Line0.Contains(gameObject))
            {
                RespawnManager.instance.Line0.Add(gameObject);
            }
        }
        if (other.transform.CompareTag("Line (1)"))
        {
            b_Line[1] = true;
            if (!RespawnManager.instance.Line1.Contains(gameObject))
            {
                RespawnManager.instance.Line1.Add(gameObject);
            }
        }
        if (other.transform.CompareTag("Line (2)"))
        {
            b_Line[2] = true;
            if (b_BossBlock)
                transform.GetChild(2).GetComponent<RespawnBulldog>().i_TouchLineNum = 0;
            if (!RespawnManager.instance.Line2.Contains(gameObject))
            {
                RespawnManager.instance.Line2.Add(gameObject);
            }
        }
        if (other.transform.CompareTag("Line (3)"))
        {
            b_Line[3] = true;
            if (b_BossBlock)
                transform.GetChild(2).GetComponent<RespawnBulldog>().i_TouchLineNum = 1;
            if (!RespawnManager.instance.Line3.Contains(gameObject))
            {
                RespawnManager.instance.Line3.Add(gameObject);
            }
        }
        if (other.transform.CompareTag("Line (4)"))
        {
            b_Line[4] = true;
            if (b_BossBlock)
                transform.GetChild(2).GetComponent<RespawnBulldog>().i_TouchLineNum = 2;
            if (!RespawnManager.instance.Line4.Contains(gameObject))
            {
                RespawnManager.instance.Line4.Add(gameObject);
            }
        }
        if (other.transform.CompareTag("Line (5)"))
        {
            b_Line[5] = true;
            if (!RespawnManager.instance.Line5.Contains(gameObject))
            {
                RespawnManager.instance.Line5.Add(gameObject);
            }
        }
        if (other.transform.CompareTag("Line (6)"))
        {
            b_Line[6] = true;
            if (!RespawnManager.instance.Line6.Contains(gameObject))
            {
                RespawnManager.instance.Line6.Add(gameObject);
            }
        }
        if (other.transform.CompareTag("Line (7)"))
        {
            b_Line[7] = true;
            if (!RespawnManager.instance.Line7.Contains(gameObject))
            {
                RespawnManager.instance.Line7.Add(gameObject);
            }
        }
        if (other.transform.CompareTag("Line (8)"))
        {
            b_Line[8] = true;
            if (!RespawnManager.instance.Line8.Contains(gameObject))
            {
                RespawnManager.instance.Line8.Add(gameObject);
            }
        }
        if (other.transform.CompareTag("Line (9)"))
        {
            b_Line[9] = true;
            if (!RespawnManager.instance.Line9.Contains(gameObject))
            {
                RespawnManager.instance.Line9.Add(gameObject);
            }
        }
        if (other.transform.CompareTag("Line (10)"))
        {
            b_Line[10] = true;
            if (!RespawnManager.instance.Line10.Contains(gameObject))
            {
                RespawnManager.instance.Line10.Add(gameObject);
                if (b_BossBlock)
                {
                    transform.GetChild(0).GetComponent<ApplyBossSkill>().b_isBack = false;
                }
            }
        }
        if (other.transform.CompareTag("Line (11)"))
        {
            b_Line[11] = true;
            if (!RespawnManager.instance.Line11.Contains(gameObject))
            {
                RespawnManager.instance.Line11.Add(gameObject);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Block_Top"))
        {
            b_onBossBlock = false;
        }

        if (other.transform.CompareTag("Line (0)"))
        {
            b_Line[0] = false;
            RespawnManager.instance.Line0.Remove(gameObject);
        }
        if (other.transform.CompareTag("Line (1)"))
        {
            b_Line[1] = false;
            RespawnManager.instance.Line1.Remove(gameObject);
        }
        if (other.transform.CompareTag("Line (2)"))
        {
            b_Line[2] = false;
            RespawnManager.instance.Line2.Remove(gameObject);
        }
        if (other.transform.CompareTag("Line (3)"))
        {
            b_Line[3] = false;
            RespawnManager.instance.Line3.Remove(gameObject);
        }
        if (other.transform.CompareTag("Line (4)"))
        {
            b_Line[4] = false;
            RespawnManager.instance.Line4.Remove(gameObject);
        }
        if (other.transform.CompareTag("Line (5)"))
        {
            b_Line[5] = false;
            RespawnManager.instance.Line5.Remove(gameObject);
        }
        if (other.transform.CompareTag("Line (6)"))
        {
            b_Line[6] = false;
            RespawnManager.instance.Line6.Remove(gameObject);
        }
        if (other.transform.CompareTag("Line (7)"))
        {
            b_Line[7] = false;
            RespawnManager.instance.Line7.Remove(gameObject);
        }
        if (other.transform.CompareTag("Line (8)"))
        {
            b_Line[8] = false;
            RespawnManager.instance.Line8.Remove(gameObject);
        }
        if (other.transform.CompareTag("Line (9)"))
        {
            b_Line[9] = false;
            RespawnManager.instance.Line9.Remove(gameObject);
        }
        if (other.transform.CompareTag("Line (10)"))
        {
            b_Line[10] = false;
            RespawnManager.instance.Line10.Remove(gameObject);
            if (b_BossBlock)
            {
                transform.GetChild(0).GetComponent<ApplyBossSkill>().b_isBack = true;
            }
        }
        if (other.transform.CompareTag("Line (11)"))
        {
            b_Line[11] = false;
            RespawnManager.instance.Line11.Remove(gameObject);
        }
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Ball"))
        {
            if (col.gameObject.transform.GetComponent<Ball>())
            {
                switch (col.gameObject.transform.GetComponent<Ball>().i_HeroType)
                {
                    case 0:
                        for (int i = 0; i < par_HitParticle.Length; i++)
                        {
                            if (!par_HitParticle[i].isPlaying)
                            {
                                par_HitParticle[i].transform.position = new Vector3(col.contacts[0].point.x, col.contacts[0].point.y + 0.5f, col.contacts[0].point.z);
                                par_HitParticle[i].Play();
                                break;
                            }
                        }
                        break;

                    case 1:
                    case 7:
                    case 13:
                    case 15:
                    case 19:
                        for (int i = 0; i < par_BigHitParticle.Length; i++)
                        {
                            if (!par_BigHitParticle[i].isPlaying)
                            {
                                par_BigHitParticle[i].transform.position = new Vector3(col.contacts[0].point.x, col.contacts[0].point.y + 0.5f, col.contacts[0].point.z);
                                par_BigHitParticle[i].Play();
                                break;
                            }
                        }
                        break;

                    case 3:
                    case 9:
                        for (int i = 0; i < par_ExplosionHitParticle.Length; i++)
                        {
                            if (!par_ExplosionHitParticle[i].isPlaying)
                            {
                                par_ExplosionHitParticle[i].transform.position = new Vector3(col.contacts[0].point.x, col.contacts[0].point.y + 0.5f, col.contacts[0].point.z);
                                par_ExplosionHitParticle[i].Play();
                                break;
                            }
                        }
                        break;

                    case 2:
                    case 4:
                    case 6:
                    case 8:
                    case 10:
                    case 12:
                    case 14:
                    case 16:
                    case 18:
                    case 20:
                        for (int i = 0; i < par_AbilityHitParticle.Length; i++)
                        {
                            if (!par_AbilityHitParticle[i].isPlaying)
                            {
                                par_AbilityHitParticle[i].transform.position = new Vector3(col.contacts[0].point.x, col.contacts[0].point.y + 0.5f, col.contacts[0].point.z);
                                par_AbilityHitParticle[i].Play();
                                break;
                            }
                        }
                        break;
                }
            }
            else
            {
                for (int i = 0; i < par_HitParticle.Length; i++)
                {
                    if (!par_HitParticle[i].isPlaying)
                    {
                        par_HitParticle[i].transform.position = new Vector3(col.contacts[0].point.x, col.contacts[0].point.y + 0.5f, col.contacts[0].point.z);
                        par_HitParticle[i].Play();
                        break;
                    }
                }
            }
        }
    }
}
