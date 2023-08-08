using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitManager_Scr : MonoBehaviour
{ 
    public static QuitManager_Scr instance;

    public GameObject g_QuitPopUpMenu;

    public bool b_PossibleQuit = false;
    public bool b_BossScene = false;
    public bool b_isFade = false;
    public bool b_isPause = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            return;


    }
    // Update is called once per frame
    void Update()
    {
        if (!TutorialManager.instance.go_TutorialRankGroup.activeSelf && TutorialManager.instance.go_TutorialGroup == null || (TutorialManager.instance.go_TutorialGroup != null && !TutorialManager.instance.go_TutorialGroup.activeSelf && TutorialManager.instance.i_TutorialCount > 8)
            || (Button_Option.instance.G_CharacterShop.activeSelf || Button_Option.instance.G_ManPower.activeSelf ||
            Button_Option.instance.G_VendingMachine.activeSelf || Button_Option.instance.G_Skill.activeSelf))
        {
            b_PossibleQuit = true;
        }
        else if (TutorialManager.instance.go_TutorialGroup != null && TutorialManager.instance.i_TutorialCount == 6 && Button_Option.instance.G_Shop.activeSelf)
        {
            b_PossibleQuit = false;
        }
        else if (TutorialManager.instance.go_TutorialGroup != null && TutorialManager.instance.go_TutorialGroup.activeSelf || TutorialManager.instance.go_TutorialRankGroup.activeSelf)
        {
            b_PossibleQuit = false;
        }

        if (!b_PossibleQuit)
        {
            if (!Button_Option.instance.G_Shop.activeSelf)
            {
                if (!TutorialManager.instance.go_TutorialRankGroup.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape) && (TutorialManager.instance.i_TutorialCount == 7 || !TutorialManager.instance.b_StopinputESC))
                    {
                        TutorialManager.instance.TutorialCountPlus(false);
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        TutorialManager.instance.Tutorial_RankButton();
                    }
                }
            }
        }
        else
        {
            if (!b_isFade && !ScenarioEventManager_Scr.instance.b_Eventing && !RespawnManager.instance.b_isMovingBlock)
            {
                if (FailedPurchaseManager_Scr.instance.G_Failed.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        FailedPurchaseManager_Scr.instance.OnClickFailedOff();
                    }
                }
                else if (Button_Option.instance.G_CharacterShop.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        Button_Option.instance.OnClickCharacterOFF();
                    }
                }
                else if (Button_Option.instance.G_Attendance.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        Button_Option.instance.OnClickAttendanceOff();
                    }
                }
                else if (Button_Option.instance.G_Mission.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        Button_Option.instance.OnClickMissionOff();
                    }
                }
                else if (Button_Option.instance.G_CoinShop.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        Button_Option.instance.OnClickCoinShopOff();
                    }
                }
                else if (Button_Option.instance.G_GemShop.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        Button_Option.instance.OnClickCashShopOff();
                    }
                }
                else if (Button_Option.instance.b_InGame && Button_Option.instance.G_LevelUp.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        Button_Option.instance.OnClickLevelUpBtn();
                    }
                }
                else if (Button_Option.instance.b_InGame && Button_Option.instance.G_Skill.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        Button_Option.instance.OnClickSkill();
                    }
                }
                else if (Button_Option.instance.G_Shop.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        if (ShopManager_Scr.instance.g_ChangeWindow.activeSelf && !TutorialManager.instance.b_FirstPlaying)
                        {
                            ShopManager_Scr.instance.Close();
                        }
                        else
                            Button_Option.instance.OnClickShopOFF();
                    }
                }
                else if (Button_Option.instance.G_ManPower.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        Button_Option.instance.OnClickManPowerOFF();
                    }
                }
                else if (Button_Option.instance.G_VendingMachine.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        if (MachineSelectManager_Scr.instance.g_SelectBuyButton.activeSelf)
                            MachineSelectManager_Scr.instance.OnClickMachineCancleBuy();
                        else
                            Button_Option.instance.OnClickVendingMachineOff();
                    }
                }
                else if (DeliveryBoxManager_Scr.instance.openningBox.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        if (!OpenBox.instance.b_isOpen)
                        {
                            if (OpenBox.instance.heroInfo.activeSelf)
                            {
                                Button_Option.instance.Fullinventory();
                            }
                            else if (OpenBox.instance.itemInfo.activeSelf)
                            {
                                Button_Option.instance.Fullinventory();
                            }
                            else if (OpenBox.instance.GoldInfo.activeSelf)
                            {
                                Button_Option.instance.Fullinventory();
                            }
                            else if (InventoryCaching_Scr.instance.warninginventory.activeSelf)
                            {
                                Button_Option.instance.Fullinventory();
                            }
                            else
                            {
                                Button_Option.instance.Fullinventory();
                            }
                        }
                    }
                }
                else if (Button_Option.instance.G_Inventory.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        if (InventoryCaching_Scr.instance.infoUI.activeSelf)
                        {
                            InventoryButton_Scr.instance.OnClickInfoOK();
                        }
                        else
                        {
                            Button_Option.instance.OnClickInventoryOff();
                        }
                    }
                }
                else if (Synthesis_Scr.instance.g_SynthesisEffectChasing.gameObject.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        SynthesisEffectChasing eft = Synthesis_Scr.instance.g_SynthesisEffectChasing;
                        if (eft.g_itemInfo.activeSelf)
                        {
                            Button_Option.instance.OnClickSynthesisEffectOff();
                        }
                        else if (eft.g_heroInfo.activeSelf)
                        {
                            Button_Option.instance.OnClickSynthesisEffectOff();
                        }
                    }
                }
                else if (Button_Option.instance.G_Synthesis.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        if (OpenBox.instance.inven_heroInfo.activeSelf)
                        {
                            OpenBox.instance.inven_heroInfo.SetActive(false);
                        }
                        else if (InventoryCaching_Scr.instance.infoUI.activeSelf)
                        {
                            InventoryButton_Scr.instance.OnClickInfoOK();
                        }
                        else if (SynthesisCaching_Scr.instance.Hint.activeSelf)
                        {
                            SynthesisButton_Scr.instance.OnClickHintWindowOff();
                        }
                        else
                        {
                            Button_Option.instance.OnClickSynthesisOff();
                        }
                    }
                }
                else if (Button_Option.instance.G_Review.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        Button_Option.instance.OnClickReviewOff();
                    }
                }
                else if (StageManager_Scr.instance.Q_Stage.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        StageManager_Scr.instance.OnClickGoLevelNo();
                    }
                }
                else if (Button_Option.instance.G_Stage.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        Button_Option.instance.OnClickStageOff();
                    }
                }
                else if (Button_Option.instance.b_InGame && Button_Option.instance.After_Pause.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        Button_Option.instance.OnClickAfterPauseOFF();
                    }
                }
                else if (Button_Option.instance.b_InGame && !TutorialManager.instance.b_FirstPlaying &&
                    !RewardManager_Scr.instance.b_isEnd && !Button_Option.instance.G_Home.activeSelf &&
                    !Player_Skill_Scr.instance.go_PlayerBase.transform.GetComponent<Player_Input>().b_isSkill && !Button_Option.instance.b_isBoss)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        Button_Option.instance.OnClickAfterPauseHOME();
                    }
                }
                else if (Button_Option.instance.b_InGame && Button_Option.instance.G_Home.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        Button_Option.instance.OnClickAfterPauseHomeOff();
                    }
                }
                else if (RewardManager_Scr.instance.G_Win.activeSelf || RewardManager_Scr.instance.G_Lose.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        RewardManager_Scr.instance.OnClickClosePanel();
                    }
                }
                else if (Button_Option.instance.b_isEndIntro && (AttendanceManager_Scr.instance.b_DontSee || !Button_Option.instance.b_GoAttendanceActive) && !Button_Option.instance.b_InGame && !g_QuitPopUpMenu.activeSelf && !Button_Option.instance.Before_Option.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        ActivePopMenu();
                    }
                }
                else if (!Button_Option.instance.b_InGame && g_QuitPopUpMenu.activeSelf && !Button_Option.instance.Before_Option.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        OnClickQuitNo();
                    }
                }
                else if (!Button_Option.instance.b_InGame && g_QuitPopUpMenu.activeSelf && Button_Option.instance.Before_Option.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        OnClickQuitNo();
                    }
                }
                else if (!Button_Option.instance.b_InGame && !g_QuitPopUpMenu.activeSelf && Button_Option.instance.Before_Option.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        Button_Option.instance.Before_Option.SetActive(false);
                    }

                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape) && Player_Input.instance.g_ButtonShield.activeSelf)
        {
            Player_Input.instance.g_ButtonShield.SetActive(false);
        }
    }

    public void ActivePopMenu()
    {
        g_QuitPopUpMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void OnClickQuitYes()
    {
        if (MissionManager_Scr.instance.b_isNoQuit)
            MissionManager_Scr.instance.i_ClearMissionNum[2] = 0;
        Save_Load.instance.Save();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    public void OnClickQuitNo()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);
        g_QuitPopUpMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
            b_isPause = true;   // 다른 행동을 함으로써 퍼즈가 되었는지 체크
        else
        {
            if (b_isPause)
                b_isPause = false;  // 다시 돌아왔으니 false로 변경
        }
        // Pause는 플레이도중 나가면 true 다시 앱으로 돌아오면 false     < ------ > Focus는 완전 반대
        if (b_isPause) {
            if (AttendanceManager_Scr.instance.b_AutoPause)
            {
                if (Button_Option.instance.b_InGame)
                {
                    Button_Option.instance.OnClickAfterPauseON();
                    /*
                    if (Button_Option.instance.G_Home.activeSelf)
                        Button_Option.instance.OnClickAfterPauseHomeOff();
                    RewardManager_Scr.instance.G_ButtonShield.SetActive(false);
                    */
                }
            }
        }
        else
        {
            // 레벨업이나, 플레이로 넘어가는 도중 FadeIn/Out효과를 일으키는 오브젝트 비활성화
            Button_Option.instance.g_FadeInOut.SetActive(false);
        }

    }
}
