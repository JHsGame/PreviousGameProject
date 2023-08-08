using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    // 일반전 튜토
    public GameObject go_TutorialGroup;
    public TextMeshProUGUI go_TutorialTitle;
    public TextMeshProUGUI go_TutorialContent;
    public TextMeshProUGUI t_ManCount;
    public GameObject go_TutorialReward;
    public GameObject go_NameProduct;
    public GameObject go_Button;
    public GameObject go_BG;
    public GameObject go_Mission;
    public GameObject[] go_Icon = new GameObject[4];
    public GameObject go_TutorialMissionGroup;
    public GameObject go_TutorialSpwan;
    public Image i_TutorialCharimage;
    public bool b_LastTutorial = false;
    public Animator ani_Atfet;
    public Sprite s_TutoShopImage;

    public bool b_FirstPlaying;
    public bool b_isManPowerTutorial = false;
    public bool b_StopinputESC = false;
    public int i_TutorialCount = 0;
    public int i_CharaterCount = 0;

    // 랭크전 튜토
    //public bool b_FirstRankGame; // 거짓-설명창 켜야함, 참-튜토 경험완료
    public bool b_RankMode = false;
    public GameObject go_TutorialRankGroup;
    //public Text text_TopText;
    public TextMeshProUGUI text_RankTitle;
    public TextMeshProUGUI text_RankContent;
    public TextMeshProUGUI text_RankContent2;

    private Coroutine c_UpdateCoroutine;


    // 박스 및 가방 및 합성 튜토리얼
    public GameObject G_BoxTutorial;
    public GameObject G_Box_Tutorial_CheckImage;
    public bool b_BoxTutorial;
    public TextMeshProUGUI tmp_BoxTutorial_Title;
    public TextMeshProUGUI tmp_BoxTutorial_Contents;
    public TextMeshProUGUI tmp_BoxTutorial_DontseeText;
    public TextMeshProUGUI tmp_BoxTutorial_Confirm;
    public TextMeshProUGUI tmp_BoxTutorial_HeroName;

    public GameObject G_Bag_Tutorial;
    public GameObject G_Bag_Tutorial_CheckImage;
    public bool b_BagTutorial;
    public TextMeshProUGUI tmp_BagTutorial_Title;
    public TextMeshProUGUI tmp_BagTutorial_Contents;
    public TextMeshProUGUI tmp_BagTutorial_DontseeText;
    public TextMeshProUGUI tmp_BagTutorial_Confirm;
    public TextMeshProUGUI tmp_BoxTutorial_FakeTitle;

    public GameObject G_Synthesis_Tutorial;
    public GameObject G_Synthesis_Tutorial_CheckImage;
    public bool b_SynthesisTutorial;
    public TextMeshProUGUI tmp_SynthesisTutorial_Title;
    public TextMeshProUGUI tmp_SynthesisTutorial_Contents;
    public TextMeshProUGUI tmp_SynthesisTutorial_DontseeText;
    public TextMeshProUGUI tmp_SynthesisTutorial_Confirm;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            return;
    }
    public void InitTutorial()
    {
        text_RankTitle.text = TranslateManager_Scr.instance.TranslateData[5][TranslateManager_Scr.instance.s_Language].ToString();
        text_RankContent.text = TranslateManager_Scr.instance.TranslateData[144][TranslateManager_Scr.instance.s_Language].ToString() + "\n" +
                                  TranslateManager_Scr.instance.TranslateData[145][TranslateManager_Scr.instance.s_Language].ToString();
        text_RankContent2.text = TranslateManager_Scr.instance.TranslateData[146][TranslateManager_Scr.instance.s_Language].ToString() + "\n" +
                                  TranslateManager_Scr.instance.TranslateData[147][TranslateManager_Scr.instance.s_Language].ToString();
        /*
        if (TranslateManager_Scr.instance.b_isKorean)
        {
            text_RankTitle.text = TranslateManager_Scr.instance.TranslateData[5]["Korean"].ToString();
            text_RankContent.text = TranslateManager_Scr.instance.TranslateData[144]["Korean"].ToString() + "\n" +
                                      TranslateManager_Scr.instance.TranslateData[145]["Korean"].ToString();
            text_RankContent2.text = TranslateManager_Scr.instance.TranslateData[146]["Korean"].ToString() + "\n" +
                                      TranslateManager_Scr.instance.TranslateData[147]["Korean"].ToString();
        }
        else if (TranslateManager_Scr.instance.b_isEnglish)
        {
            text_RankTitle.text = TranslateManager_Scr.instance.TranslateData[5]["English"].ToString();
            text_RankContent.text = TranslateManager_Scr.instance.TranslateData[144]["English"].ToString() + "\n" +
                                      TranslateManager_Scr.instance.TranslateData[145]["English"].ToString();
            text_RankContent2.text = TranslateManager_Scr.instance.TranslateData[146]["English"].ToString() + "\n" +
                                      TranslateManager_Scr.instance.TranslateData[147]["English"].ToString();
        }
        else if (TranslateManager_Scr.instance.b_isJapanese)
        {
            text_RankTitle.text = TranslateManager_Scr.instance.TranslateData[5]["Japanese"].ToString();
            text_RankContent.text = TranslateManager_Scr.instance.TranslateData[144]["Japanese"].ToString() + "\n" +
                                      TranslateManager_Scr.instance.TranslateData[145]["Japanese"].ToString();
            text_RankContent2.text = TranslateManager_Scr.instance.TranslateData[146]["Japanese"].ToString() + "\n" +
                                      TranslateManager_Scr.instance.TranslateData[147]["Japanese"].ToString();
        }
        else if (TranslateManager_Scr.instance.b_isChinese)
        {
            text_RankTitle.text = TranslateManager_Scr.instance.TranslateData[5]["Chinese"].ToString();
            text_RankContent.text = TranslateManager_Scr.instance.TranslateData[144]["Chinese"].ToString() + "\n" +
                                      TranslateManager_Scr.instance.TranslateData[145]["Chinese"].ToString();
            text_RankContent2.text = TranslateManager_Scr.instance.TranslateData[146]["Chinese"].ToString() + "\n" +
                                      TranslateManager_Scr.instance.TranslateData[147]["Chinese"].ToString();
        }*/


        c_UpdateCoroutine = StartCoroutine(Cou_Update());
    }

    IEnumerator Cou_Update()
    {
        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            if (i_TutorialCount == 7)
            {
                Time.timeScale = 0;
            }
            if (!b_FirstPlaying)
            {
                Destroy(go_TutorialGroup);
            }
            else if (go_TutorialGroup != null && go_TutorialGroup.activeSelf)
            {
                Player_Input.instance.reset_PlayerPos();
                Player_Input.instance.g_ButtonShield.SetActive(false);
            }
            if (go_TutorialMissionGroup != null && Button_Option.instance.b_Golobby)
            {
                go_TutorialMissionGroup.SetActive(false);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Skip_EventIntro()
    {
        // 애니메이션 작동시키기 
        go_TutorialTitle.transform.GetComponent<Animator>().Play("EventFadeOut_Text");
        go_TutorialContent.transform.GetComponent<Animator>().Play("EventFadeOut_Text");
        go_BG.transform.GetComponent<Animator>().Play("EventFadeOut_BG");
    }


    public void Tutorial_RankButton()
    {
        RespawnManager.instance.timer_nextwave = 10;
        Player_Input.instance.f_Delayatk = 1;
        go_TutorialRankGroup.SetActive(false);
        //RespawnManager.instance.ReadytoLevelStart();
        //RespawnManager.instance.ReadytoRoundStart();

        //b_FirstRankGame = true;
        Time.timeScale = 1;
    }

    public void TutorialCountPlus(bool Continue_tutorial)
    {
        i_TutorialCount++;

        SoundManager_sfx.instance.PlaySE("UI_Click1", false);

        CancelInvoke();
        RespawnManager.instance.timer_nextwave = 999f;

        for (int i = 0; i < go_Icon.Length; i++)
        {
            go_Icon[i].transform.GetComponent<Animator>().enabled = false;
            go_Icon[i].transform.GetChild(0).GetComponent<Animator>().enabled = false;

            go_Icon[i].transform.GetComponent<Button>().enabled = false;
            go_Icon[i].transform.GetChild(0).GetComponent<Button>().enabled = false;

            go_Icon[i].transform.GetComponent<Image>().color = new Color(100f / 255f, 100f / 255f, 100f / 255f, 1);
            go_Icon[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(100f / 255f, 100f / 255f, 100f / 255f, 1);
            if (i == 2)
            {
                go_Icon[i].transform.GetChild(1).GetComponent<Animator>().enabled = false;
                go_Icon[i].transform.GetChild(1).GetComponent<Button>().enabled = false;

                go_Icon[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(100f / 255f, 100f / 255f, 100f / 255f, 1);
            }
        }

        for (int i = 0; i < go_TutorialReward.transform.childCount; i++)
        {
            go_TutorialReward.transform.GetChild(i).gameObject.SetActive(false);
        }
        if (i_TutorialCount < 8)
        {
            go_TutorialReward.transform.GetChild(i_TutorialCount).gameObject.SetActive(true);
        }


        Vector3 screenPos = Button_Option.instance.Cam.GetComponent<Camera>().WorldToScreenPoint(transform.position);
        GameObject go_TextGold_Dia_Exp = Button_Option.instance.go_TextGold_Dia_Exp;
        GameObject go_Text_Point = go_TextGold_Dia_Exp.transform.GetChild(0).gameObject;
        GameObject go_Text_Dia = go_TextGold_Dia_Exp.transform.GetChild(1).gameObject;

        switch (i_TutorialCount)
        {
            case 0:
                go_TutorialTitle.text = TranslateManager_Scr.instance.TranslateContext(140);
                go_TutorialContent.text = TranslateManager_Scr.instance.TranslateContext(141) + "\n" +
                                          TranslateManager_Scr.instance.TranslateContext(142) + "\n" +
                                          TranslateManager_Scr.instance.TranslateContext(143);
                break;

            case 1:
                Video_Charater();

                go_Icon[i_TutorialCount - 1].transform.GetComponent<Animator>().enabled = true;
                go_Icon[i_TutorialCount - 1].transform.GetChild(0).GetComponent<Animator>().enabled = true;
                go_Icon[i_TutorialCount - 1].transform.GetComponent<Animator>().Play("Tutorial_Emphasize");
                go_Icon[i_TutorialCount - 1].transform.GetChild(0).GetComponent<Animator>().Play("Tutorial_Emphasize_Text");

                go_Icon[i_TutorialCount - 1].transform.GetComponent<Button>().enabled = true;
                go_Icon[i_TutorialCount - 1].transform.GetChild(0).GetComponent<Button>().enabled = true;
                ChangeManager_Scr.instance.g_TutorialShield.SetActive(true);

                if (!Continue_tutorial)
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
                            T_Point.transform.localPosition = new Vector3(PointPos.x, PointPos.y - 300, -200);
                            T_Point.transform.GetChild(0).GetComponent<Animator>().Play("GetCoin");
                            T_Point.transform.GetChild(0).GetComponent<Icon_GetCoin>().i_getGold = 20;
                            break;
                        }
                        else if (i >= go_Text_Point.transform.childCount - 1)
                        {
                            // 추가 생성하는 곳 
                        }
                    }
                }

                go_TutorialTitle.text = TranslateManager_Scr.instance.TranslateContext(76);
                go_TutorialContent.text = TranslateManager_Scr.instance.TranslateContext(77) + "\n" +
                                          TranslateManager_Scr.instance.TranslateContext(78);
                break;

            case 2:
                RespawnManager.instance.DestroyAllblock();

                Player_Input.instance.ActiveCharater(0);
                Video_Skill();

                go_Icon[i_TutorialCount - 1].transform.GetComponent<Animator>().enabled = true;
                go_Icon[i_TutorialCount - 1].transform.GetChild(0).GetComponent<Animator>().enabled = true;
                go_Icon[i_TutorialCount - 1].transform.GetComponent<Animator>().Play("Tutorial_Emphasize");
                go_Icon[i_TutorialCount - 1].transform.GetChild(0).GetComponent<Animator>().Play("Tutorial_Emphasize_Text");

                go_Icon[i_TutorialCount - 1].transform.GetComponent<Button>().enabled = true;
                go_Icon[i_TutorialCount - 1].transform.GetChild(0).GetComponent<Button>().enabled = true;
                ChangeManager_Scr.instance.g_TutorialShield.SetActive(false);
                if (!Continue_tutorial)
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
                            T_Point.transform.localPosition = new Vector3(PointPos.x, PointPos.y - 300, -200);
                            T_Point.transform.GetChild(0).GetComponent<Animator>().Play("GetCoin");
                            T_Point.transform.GetChild(0).GetComponent<Icon_GetCoin>().i_getGold = 30;
                            break;
                        }
                        else if (i >= go_Text_Point.transform.childCount - 1)
                        {
                            // 추가 생성하는 곳 
                        }
                    }
                }


                go_TutorialTitle.text = TranslateManager_Scr.instance.TranslateContext(8);
                go_TutorialContent.text = TranslateManager_Scr.instance.TranslateContext(190) + "\n" +
                                          TranslateManager_Scr.instance.TranslateContext(191) + "\n" +
                                          TranslateManager_Scr.instance.TranslateContext(79) + "\n" +
                                          TranslateManager_Scr.instance.TranslateContext(80) + "\n" +
                                          TranslateManager_Scr.instance.TranslateContext(81);
                break;

            case 3:
                RespawnManager.instance.DestroyAllblock();
                Time.timeScale = 1;
                Invoke("Invoke_Cteatebloack", 2f);
                Invoke("Invoke_ManPower", 4f);

                go_Icon[i_TutorialCount - 1].transform.GetComponent<Animator>().enabled = true;
                go_Icon[i_TutorialCount - 1].transform.GetChild(0).GetComponent<Animator>().enabled = true;
                go_Icon[i_TutorialCount - 1].transform.GetChild(1).GetComponent<Animator>().enabled = true;
                go_Icon[i_TutorialCount - 1].transform.GetComponent<Animator>().Play("Tutorial_Emphasize");
                go_Icon[i_TutorialCount - 1].transform.GetChild(0).GetComponent<Animator>().Play("Tutorial_Emphasize_Text");
                go_Icon[i_TutorialCount - 1].transform.GetChild(1).GetComponent<Animator>().Play("Tutorial_Emphasize_Text");

                go_Icon[i_TutorialCount - 1].transform.GetComponent<Button>().enabled = true;
                go_Icon[i_TutorialCount - 1].transform.GetChild(0).GetComponent<Button>().enabled = true;
                go_Icon[i_TutorialCount - 1].transform.GetChild(1).GetComponent<Button>().enabled = true;

                ManPowerManager_Scr.instance.g_TutorialShield.SetActive(true);

                if (!Continue_tutorial)
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
                            T_Point.transform.localPosition = new Vector3(PointPos.x, PointPos.y - 300, -200);
                            T_Point.transform.GetChild(0).GetComponent<Animator>().Play("GetCoin");
                            T_Point.transform.GetChild(0).GetComponent<Icon_GetCoin>().i_getGold = 50;
                            break;
                        }
                        else if (i >= go_Text_Point.transform.childCount - 1)
                        {
                            // 추가 생성하는 곳 
                        }
                    }
                }


                go_TutorialTitle.text = TranslateManager_Scr.instance.TranslateContext(9);
                go_TutorialContent.text = TranslateManager_Scr.instance.TranslateContext(82) + "\n" +
                                          TranslateManager_Scr.instance.TranslateContext(83) + "\n" +
                                          TranslateManager_Scr.instance.TranslateContext(84) + "\n" +
                                          TranslateManager_Scr.instance.TranslateContext(202);
                break;

            case 4:// 인력 보상 공1짜리 5개 선물
                RespawnManager.instance.DestroyAllblock();
                t_ManCount.text = "5";
                ManPowerManager_Scr.instance.t_UIManCount.text = "5";


                if (Player_Input.instance.b_isAttacking)
                {
                    Player_Input.instance.TutorialReturnBall();
                    Player_Input.instance.b_isAttacking = false;
                }
                b_StopinputESC = true;
                go_Icon[i_TutorialCount - 2].transform.GetComponent<Animator>().enabled = true;
                go_Icon[i_TutorialCount - 2].transform.GetChild(0).GetComponent<Animator>().enabled = true;
                go_Icon[i_TutorialCount - 2].transform.GetChild(1).GetComponent<Animator>().enabled = true;
                go_Icon[i_TutorialCount - 2].transform.GetComponent<Animator>().Play("Tutorial_Emphasize");
                go_Icon[i_TutorialCount - 2].transform.GetChild(0).GetComponent<Animator>().Play("Tutorial_Emphasize_Text");
                go_Icon[i_TutorialCount - 2].transform.GetChild(1).GetComponent<Animator>().Play("Tutorial_Emphasize_Text");

                go_Icon[i_TutorialCount - 2].transform.GetComponent<Button>().enabled = true;
                go_Icon[i_TutorialCount - 2].transform.GetChild(0).GetComponent<Button>().enabled = true;
                go_Icon[i_TutorialCount - 2].transform.GetChild(1).GetComponent<Button>().enabled = true;
                ManPowerManager_Scr.instance.g_TutorialShield.SetActive(false);
                MachineSelectManager_Scr.instance.g_TutorialShield.SetActive(true);


                // Video_VendingMachine();
                if (!Continue_tutorial)
                {
                    go_Button.SetActive(false);
                    go_TutorialReward.transform.GetChild(i_TutorialCount).gameObject.SetActive(false);
                    go_TutorialReward.transform.GetChild(3).gameObject.SetActive(true);
                    go_TutorialReward.transform.GetChild(3).GetComponent<Animator>().Play("Tutorial_Reward_Man");
                }
                else
                {
                    go_TutorialReward.transform.GetChild(3).GetComponent<TutorialFadeOut>().AniEnd(0);
                }
                break;

            case 5: // 자판기 보상 : 자판기 1개, 캔 1개
                RespawnManager.instance.DestroyAllblock();
                Player_Input.instance.TutorialReturnBall();

                b_StopinputESC = true;
                go_Icon[i_TutorialCount - 2].transform.GetComponent<Animator>().enabled = true;
                go_Icon[i_TutorialCount - 2].transform.GetChild(0).GetComponent<Animator>().enabled = true;
                go_Icon[i_TutorialCount - 2].transform.GetComponent<Animator>().Play("Tutorial_Emphasize");
                go_Icon[i_TutorialCount - 2].transform.GetChild(0).GetComponent<Animator>().Play("Tutorial_Emphasize_Text");

                go_Icon[i_TutorialCount - 2].transform.GetComponent<Button>().enabled = true;
                go_Icon[i_TutorialCount - 2].transform.GetChild(0).GetComponent<Button>().enabled = true;
                MachineSelectManager_Scr.instance.g_TutorialShield.SetActive(false);
                ShopManager_Scr.instance.g_TutorialShield.SetActive(true);
                if (!Continue_tutorial)
                {
                    go_Button.SetActive(false);
                    go_TutorialReward.transform.GetChild(i_TutorialCount).gameObject.SetActive(false);
                    go_TutorialReward.transform.GetChild(4).gameObject.SetActive(true);
                    go_TutorialReward.transform.GetChild(4).GetComponent<Animator>().Play("Tutorial_Reward_Can");
                }
                else
                {
                    go_TutorialReward.transform.GetChild(4).GetComponent<TutorialFadeOut>().AniEnd(1);
                }
                MachineSelectManager_Scr.instance.i_LastSelected = 0;
                break;

            case 6:// 주임 이름 추가하기 선물
                RespawnManager.instance.DestroyAllblock();
                Player_Input.instance.b_isAttacking = false;
                b_StopinputESC = true;

                if (!Continue_tutorial)
                {
                    Button_Option.instance.G_Shop.SetActive(true);
                    for (int i = 0; i < 6; i++)
                    {
                        Button_Option.instance.G_Shop.transform.GetChild(i).gameObject.SetActive(false);
                    }
                    //go_NameProduct.transform.GetComponent<ProductStat_Scr>().OnBuyNameChange();
                    ShopManager_Scr.instance.t_ShopJobName.text = string.Concat(ShopManager_Scr.instance.s_MiddleNameList[0], " ");
                    ShopManager_Scr.instance.t_ShopJobName.text = string.Concat(ShopManager_Scr.instance.t_ShopJobName.text + TranslateManager_Scr.instance.TranslateContext(32));
                  // ShopManager_Scr.instance.g_ChangeWindow.transform.GetChild(3).GetChild(0).GetComponent<Image>().sprite = s_TutoShopImage;
                    ShopManager_Scr.instance.ChangeInputField();
                    ShopManager_Scr.instance.g_ChangeWindow.transform.GetChild(1).gameObject.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(750f, 450f);
                    ShopManager_Scr.instance.g_ChangeWindow.transform.GetChild(2).gameObject.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -210f);
                    ShopManager_Scr.instance.g_ChangeWindow.transform.GetChild(3).gameObject.SetActive(false);
                    ShopManager_Scr.instance.g_ChangeWindow.transform.GetChild(4).gameObject.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
                    ShopManager_Scr.instance.g_ChangeWindow.transform.GetChild(6).gameObject.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(150f, -130f);

                    ShopManager_Scr.instance.g_ChangeWindow.SetActive(true);
                    for(int i=0; i < ShopManager_Scr.instance.g_BuyButton.Length; ++i)
                    {
                        if (ShopManager_Scr.instance.g_BuyButton[i].activeSelf)
                            ShopManager_Scr.instance.g_BuyButton[i].SetActive(false);
                    }
                    ShopManager_Scr.instance.g_BuyButton[0].SetActive(true);
                }

                ShopManager_Scr.instance.g_TutorialShield.SetActive(false);


                go_TutorialTitle.text = TranslateManager_Scr.instance.TranslateContext(91);
                go_TutorialContent.text = TranslateManager_Scr.instance.TranslateContext(92) + "\n" +
                                          TranslateManager_Scr.instance.TranslateContext(93);
                go_TutorialMissionGroup.transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>().text = TranslateManager_Scr.instance.TranslateContext(94);


                go_TutorialGroup.gameObject.SetActive(false);
                go_Button.SetActive(false);

                if (Continue_tutorial)
                {
                    go_TutorialGroup.gameObject.SetActive(true);
                    go_BG.transform.GetComponent<Animator>().Play("Tutorial_FadeOut_BG");
                    go_TutorialTitle.transform.GetComponent<Animator>().Play("Tutorial_FadeOut_Text");
                    go_TutorialContent.transform.GetComponent<Animator>().Play("Tutorial_FadeOut_Text");
                    go_TutorialMissionGroup.SetActive(true);
                    Time.timeScale = 0;
                }

                break;

            case 7:

                go_TutorialTitle.text = TranslateManager_Scr.instance.TranslateContext(96);
                go_TutorialContent.text = TranslateManager_Scr.instance.TranslateContext(97) + "\n" +
                                          TranslateManager_Scr.instance.TranslateContext(98) + "\n" +
                                          TranslateManager_Scr.instance.TranslateContext(99);
                break;

            case 8:
                // 다이아 획득 애니메이션
                b_FirstPlaying = false;
                Button_Option.instance.g_RankButton.SetActive(true);
                MissionManager_Scr.instance.ResetMission();
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
                        T_Dia.transform.GetChild(0).GetComponent<Icon_GetCoin>().i_getCash = 10;
                        break;
                    }
                    else if (i >= go_Text_Dia.transform.childCount - 1)
                    {
                        // 추가 생성하는 곳 
                    }
                }
                Destroy(go_TutorialMissionGroup);
                Destroy(go_TutorialGroup);
                Time.timeScale = 1;
                break;
        }
    }

    void CreateBlock_Tutorial()
    {
        int AllRespawnCount = go_TutorialSpwan.transform.childCount-1;

        for (int i = 0; i < AllRespawnCount; i++)
        {
            int Whichone = Random.Range(0, 2);
            if(Whichone == 0)
            {
                bool newBlock = false;
                int count = 0;

                // Obj 블럭 중 하나 선택
                int BlockNum = Random.Range(0, 3);

                // 선별된 블럭이 생성된 것이 있다면 그걸 선택하고 여유분이 없으면 새로 생성
                switch (BlockNum)
                {
                    case 0: // 컴퓨터
                        for (int j = 0; j < RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).childCount; j++)
                        {
                            if (RespawnManager.instance.AllBlock.Contains(RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).GetChild(j).gameObject)) // 이미 목록에 들어가 있다.
                            {
                                count++;
                                if (count >= RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).childCount) // 여유분이 없으므로 새로 생성한다.
                                {
                                    newBlock = true;
                                }
                            }
                            else // 목록에 없는 새로운 녀석이다.
                            {
                                RespawnManager.instance.AllBlock.Add(RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).GetChild(j).gameObject);
                                break;
                            }
                        }
                        // newBlock이 true면 추가생성
                        if (newBlock)
                        {
                            GameObject go_newBlock0 = Instantiate(RespawnManager.instance.p_Computer, go_TutorialSpwan.transform.GetChild(i).position, Quaternion.Euler(0, 0, 0));
                            go_newBlock0.SetActive(false);
                            go_newBlock0.transform.SetParent(RespawnManager.instance.G_enemy.transform.GetChild(BlockNum));
                            RespawnManager.instance.AllBlock.Add(go_newBlock0);
                        }
                        break;

                    case 1: // 금고
                        for (int j = 0; j < RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).childCount; j++)
                        {
                            if (RespawnManager.instance.AllBlock.Contains(RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).GetChild(j).gameObject)) // 이미 목록에 들어가 있다.
                            {
                                count++;
                                if (count >= RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).childCount) // 여유분이 없으므로 새로 생성한다.
                                {
                                    newBlock = true;
                                }
                            }
                            else // 목록에 없는 새로운 녀석이다.
                            {
                                RespawnManager.instance.AllBlock.Add(RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).GetChild(j).gameObject);
                                break;
                            }
                        }
                        // newBlock이 true면 추가생성
                        if (newBlock)
                        {
                            GameObject go_newBlock0 = Instantiate(RespawnManager.instance.p_Documentbox, go_TutorialSpwan.transform.GetChild(i).position, Quaternion.Euler(0, 0, 0));
                            go_newBlock0.SetActive(false);
                            go_newBlock0.transform.SetParent(RespawnManager.instance.G_enemy.transform.GetChild(BlockNum));
                            RespawnManager.instance.AllBlock.Add(go_newBlock0);
                        }
                        break;

                    case 2: // 캐비넷
                        for (int j = 0; j < RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).childCount; j++)
                        {
                            if (RespawnManager.instance.AllBlock.Contains(RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).GetChild(j).gameObject)) // 이미 목록에 들어가 있다.
                            {
                                count++;
                                if (count >= RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).childCount) // 여유분이 없으므로 새로 생성한다.
                                {
                                    newBlock = true;
                                }
                            }
                            else // 목록에 없는 새로운 녀석이다.
                            {
                                RespawnManager.instance.AllBlock.Add(RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).GetChild(j).gameObject);
                                break;
                            }
                        }
                        // newBlock이 true면 추가생성
                        if (newBlock)
                        {
                            GameObject go_newBlock0 = Instantiate(RespawnManager.instance.p_Cabinet, go_TutorialSpwan.transform.GetChild(i).position, Quaternion.Euler(0, 0, 0));
                            go_newBlock0.SetActive(false);
                            go_newBlock0.transform.SetParent(RespawnManager.instance.G_enemy.transform.GetChild(BlockNum));
                            RespawnManager.instance.AllBlock.Add(go_newBlock0);
                        }
                        break;


                    default:
                        break;
                }
            }
            else
            {
                bool newBlock = false;
                int count = 0;

                // Mob 블럭 중 하나 선택
                int BlockNum = Random.Range(5, 9);

                // 선별된 블럭이 생성된 것이 있다면 그걸 선택하고 여유분이 없으면 새로 생성
                switch (BlockNum)
                {
                    case 5: // 주임
                        for (int j = 0; j < RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).childCount; j++)
                        {
                            if (RespawnManager.instance.AllBlock.Contains(RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).GetChild(j).gameObject)) // 이미 목록에 들어가 있다.
                            {
                                count++;
                                if (count >= RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).childCount) // 여유분이 없으므로 새로 생성한다.
                                {
                                    newBlock = true;
                                }
                            }
                            else // 목록에 없는 새로운 녀석이다.
                            {
                                RespawnManager.instance.AllBlock.Add(RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).GetChild(j).gameObject);
                                break;
                            }
                        }
                        // newBlock이 true면 추가생성
                        if (newBlock)
                        {
                            GameObject go_newBlock0 = Instantiate(RespawnManager.instance.p_Middle_0[ChangeResource.instance.ResourceNum], go_TutorialSpwan.transform.GetChild(i).position, Quaternion.Euler(0, 0, 0));
                            go_newBlock0.SetActive(false);
                            go_newBlock0.transform.SetParent(RespawnManager.instance.G_enemy.transform.GetChild(BlockNum));
                            RespawnManager.instance.AllBlock.Add(go_newBlock0);
                        }
                        break;

                    case 6: // 대리
                        for (int j = 0; j < RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).childCount; j++)
                        {
                            if (RespawnManager.instance.AllBlock.Contains(RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).GetChild(j).gameObject)) // 이미 목록에 들어가 있다.
                            {
                                count++;
                                if (count >= RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).childCount) // 여유분이 없으므로 새로 생성한다.
                                {
                                    newBlock = true;
                                }
                            }
                            else // 목록에 없는 새로운 녀석이다.
                            {
                                RespawnManager.instance.AllBlock.Add(RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).GetChild(j).gameObject);
                                break;
                            }
                        }
                        // newBlock이 true면 추가생성
                        if (newBlock)
                        {
                            GameObject go_newBlock0 = Instantiate(RespawnManager.instance.p_Middle_1[ChangeResource.instance.ResourceNum], go_TutorialSpwan.transform.GetChild(i).position, Quaternion.Euler(0, 0, 0));
                            go_newBlock0.SetActive(false);
                            go_newBlock0.transform.SetParent(RespawnManager.instance.G_enemy.transform.GetChild(BlockNum));
                            RespawnManager.instance.AllBlock.Add(go_newBlock0);
                        }
                        break;

                    case 7: // 과장
                        for (int j = 0; j < RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).childCount; j++)
                        {
                            if (RespawnManager.instance.AllBlock.Contains(RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).GetChild(j).gameObject)) // 이미 목록에 들어가 있다.
                            {
                                count++;
                                if (count >= RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).childCount) // 여유분이 없으므로 새로 생성한다.
                                {
                                    newBlock = true;
                                }
                            }
                            else // 목록에 없는 새로운 녀석이다.
                            {
                                RespawnManager.instance.AllBlock.Add(RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).GetChild(j).gameObject);
                                break;
                            }
                        }
                        // newBlock이 true면 추가생성
                        if (newBlock)
                        {
                            GameObject go_newBlock0 = Instantiate(RespawnManager.instance.p_Middle_2[ChangeResource.instance.ResourceNum], go_TutorialSpwan.transform.GetChild(i).position, Quaternion.Euler(0, 0, 0));
                            go_newBlock0.SetActive(false);
                            go_newBlock0.transform.SetParent(RespawnManager.instance.G_enemy.transform.GetChild(BlockNum));
                            RespawnManager.instance.AllBlock.Add(go_newBlock0);
                        }
                        break;

                    case 8: // 부장
                        for (int j = 0; j < RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).childCount; j++)
                        {
                            if (RespawnManager.instance.AllBlock.Contains(RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).GetChild(j).gameObject)) // 이미 목록에 들어가 있다.
                            {
                                count++;
                                if (count >= RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).childCount) // 여유분이 없으므로 새로 생성한다.
                                {
                                    newBlock = true;
                                }
                            }
                            else // 목록에 없는 새로운 녀석이다.
                            {
                                RespawnManager.instance.AllBlock.Add(RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).GetChild(j).gameObject);
                                break;
                            }
                        }
                        // newBlock이 true면 추가생성
                        if (newBlock)
                        {
                            GameObject go_newBlock0 = Instantiate(RespawnManager.instance.p_Middle_3[ChangeResource.instance.ResourceNum], go_TutorialSpwan.transform.GetChild(i).position, Quaternion.Euler(0, 0, 0));
                            go_newBlock0.SetActive(false);
                            go_newBlock0.transform.SetParent(RespawnManager.instance.G_enemy.transform.GetChild(BlockNum));
                            RespawnManager.instance.AllBlock.Add(go_newBlock0);
                        }
                        break;

                    default:
                        break;
                }
            }

            // 블럭 생성
            for (int j = 0; j < RespawnManager.instance.AllBlock.Count; j++)
            {
                if (RespawnManager.instance.AllBlock[j].activeSelf == false)
                {
                    BlockControl Script = RespawnManager.instance.AllBlock[j].transform.GetComponent<BlockControl>();

                    Script.setinfo();
                    RespawnManager.instance.AllBlock[j].transform.position = go_TutorialSpwan.transform.GetChild(i).position;
                    Script.ResetVar(false);
                    RespawnManager.instance.AllBlock[j].SetActive(true);
                    Script.PlaySwpanParticle();
                    RespawnManager.instance.ActiveBlock.Add(RespawnManager.instance.AllBlock[j]);
                    if(Script.b_SpeechBlock)
                        RespawnManager.instance.SpeechBlock.Add(RespawnManager.instance.AllBlock[j]);
                }
            }
        }
        // 스킬 사용시 보스 블럭 소환
        if (i_TutorialCount == 2)
        {
            // Boss 블럭 중 하나 선택
            int BlockNum = Random.Range(10, 14);
            GameObject go_BossBlock = RespawnManager.instance.G_enemy.transform.GetChild(BlockNum).gameObject;

            RespawnManager.instance.AllBlock.Add(go_BossBlock);

            // 보스 생성
            go_BossBlock.SetActive(true);
            go_BossBlock.transform.position = go_TutorialSpwan.transform.GetChild(go_TutorialSpwan.transform.childCount - 1).position;
            BlockControl BossScript = go_BossBlock.transform.GetComponent<BlockControl>();
            BossScript.ResetVar(false);
            BossScript.setinfo();
        }
    }

    void Video_Charater()
    {
        Time.timeScale = 1;
        i_CharaterCount = 0;
        Invoke("Invoke_Charater", 1f);
    }

    void Invoke_Charater()
    {
        Player_Input.instance.ActiveCharater(i_CharaterCount);

        i_CharaterCount++;
        if (i_CharaterCount > 5)
        {
            i_CharaterCount = 0;
        }
        Invoke("Invoke_Charater", 2f);
    }

    void Video_Skill()
    {
        Time.timeScale = 1;
        Invoke("Invoke_Cteatebloack", 2f);

        Invoke("Invoke_Skill1", 4f);

        Invoke("Invoke_Skill2", 6f);
    }

    void Invoke_Cteatebloack()
    {
        CreateBlock_Tutorial();
    }
    void Invoke_Skill1()
    {
        Player_Input.instance.useSkill(3, true);
    }

    void Invoke_Skill2()
    {
        Player_Input.instance.useSkill(0, true);
        Player_Input.instance.useSkill(1, true);
        Player_Input.instance.useSkill(2, true);
    }

    void Invoke_ManPower()
    {
        if (i_TutorialCount <= 3)
            Player_Input.instance.TutorialLaunch(new Vector3(3.2f, 0, 4.4f));
    }



    public void Video_VendingMachine()
    {

        for (int i = 0; i < MachineSelectManager_Scr.instance.go_VendingM.Length; i++)
        {
            MachineSelectManager_Scr.instance.go_VendingM[i].SetActive(true);
        }
        Invoke("Invoke_Cteatebloack", 2f);

        Invoke("Invoke_VendingMachine", 4f);
    }

    void Invoke_VendingMachine()
    {
        for (int i = 0; i < MachineSelectManager_Scr.instance.go_VendingM.Length; i++)
        {
            MachineSelectManager_Scr.instance.go_VendingM[i].transform.GetComponent<LaunchCan>().Tutorial_LaunchCan();
        }

        //Player_Input.instance.TutorialLaunch(new Vector3(3.2f, 0, 4.4f));

    }
}
