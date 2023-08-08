using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MachineSelectManager_Scr : MonoBehaviour
{
    public static MachineSelectManager_Scr instance;

    public Transform t_AllCanList;
    public Transform t_ParentMachine;

    public GameObject[] go_VendingM;

    public int i_NowBoughtMachine;

    public GameObject g_SelectBuyButton;
    public GameObject g_TutorialShield;
    public TextMeshProUGUI t_MachinePrice;
    //public GameObject g_BuyCan;
    public int i_MachineSelected;
    public int i_LastSelected = -1;  // 마지막으로 선택한 자판기 
    public int i_CancelPurchaseSelected = -1;  // 구매 취소시 마지막으로 선택됬던 자판기를 택하기 위한 임시변수
    public TextAsset myTxt;
    public List<Dictionary<string, object>> MachineData;

    public int i_BuyPrice = 100;

    public bool b_isSoundOn = false;

    private Coroutine c_UpdateCoroutine;

    public Transform[] t_CanList;     // 전체 캔 리스트에서 내 리스트 찾기
    public List<GameObject> canList = new List<GameObject>();   // 캔 리스트


    public TextAsset CanTxt;
    public List<Dictionary<string, object>> CanData;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            CanData = CSVReader.Read(CanTxt);
        }
        else
            return;

    }

    // Start is called before the first frame update
    void Start()
    {

        MachineData = CSVReader.Read(myTxt);

        for (int i = 0; i < t_ParentMachine.childCount; ++i)
        {
            t_ParentMachine.GetChild(i).GetComponent<MachineStat_Scr>().i_ListNum = i;
        }

        Invoke("TurnOffList", 1.0f);
    }

    public void readytoDelay()
    {
        Invoke("StartingCoroutine", 1f);
    }

    void StartingCoroutine()
    {
        c_UpdateCoroutine = StartCoroutine(Cou_Update());
    }

    public void TurnOffList()
    {
        OffMachine();
    }

    IEnumerator Cou_Update()
    {
        if (i_LastSelected != -1)
        {
            t_ParentMachine.GetChild(i_LastSelected).GetComponent<MachineStat_Scr>().OnClickMyMachine();
        }
        while (true)
        {
            if (Button_Option.instance.G_VendingMachine.activeSelf)
            {
                t_MachinePrice.text = i_BuyPrice.ToString();
                for (int i = 0; i < t_ParentMachine.childCount; ++i)
                {
                    if (!t_ParentMachine.GetChild(i).GetComponent<MachineStat_Scr>().b_Bought)
                    {
                        t_ParentMachine.GetChild(i).GetComponent<MachineStat_Scr>().SetPlusPrice((int)MachineData[i_NowBoughtMachine]["Buy"]);
                    }
                }

                for (int i = 0; i < t_AllCanList.childCount; ++i)
                {
                    if (t_AllCanList.GetChild(i).gameObject.activeSelf)
                        t_ParentMachine.GetChild(i).GetComponent<MachineStat_Scr>().SetSelectedMachine();
                    else
                    {
                        t_ParentMachine.GetChild(i).GetComponent<MachineStat_Scr>().SetDeselectMachine();
                    }
                }
                //g_BuyCan.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = i_BuyPrice.ToString();
            }
            else
            {
                for (int i = 0; i < t_ParentMachine.childCount; ++i)
                {
                    if (t_ParentMachine.GetChild(i).GetComponent<MachineStat_Scr>().b_Bought)
                    {
                        canList.Clear();
                        for (int k = 0; k < t_CanList[i].childCount; ++k)
                        {
                            GameObject obj = t_CanList[i].GetChild(k).gameObject;
                            if (obj.activeSelf && obj.GetComponent<CanStat_Scr>().b_IsBought)
                                canList.Add(obj);
                        }

                        canList.Sort(delegate (GameObject A, GameObject B)
                        {
                            if (A.GetComponent<CanStat_Scr>().f_NowAtk > B.GetComponent<CanStat_Scr>().f_NowAtk) return 1;
                            else if (A.GetComponent<CanStat_Scr>().f_NowAtk < B.GetComponent<CanStat_Scr>().f_NowAtk) return -1;
                            else return 0;
                        });
                        for (int k = 0; k < canList.Count; ++k)
                        {
                            canList[k].transform.SetSiblingIndex(k);
                        }
                        for (int k = 0; k < transform.childCount - 2; ++k)
                        {
                            transform.GetChild(k).GetComponent<LaunchCan>().Refreash();
                        }
                    }
                }
            }
            yield return CoroutineManager_Scr.WaitForEndOfFrame;
        }
    }

    public void OffMachine()
    {
        for (int i = 0; i < t_ParentMachine.childCount; ++i)
        {
            if(i != i_LastSelected) // 마지막으로 선택한 자판기는 꺼지지 않게 설정
                t_ParentMachine.GetChild(i).GetComponent<MachineStat_Scr>().SetDeselectMachine();
            else if(!t_ParentMachine.GetChild(i).GetComponent<MachineStat_Scr>().b_Bought && i == i_LastSelected)
                t_ParentMachine.GetChild(i).GetComponent<MachineStat_Scr>().SetDeselectMachine();
        }
        g_SelectBuyButton.SetActive(false);
        //g_BuyCan.SetActive(false);
    }

    public void OnLoadMachineOnOff(int _Num, bool _OnOff)
    {
        t_ParentMachine.GetChild(_Num).GetComponent<MachineStat_Scr>().OnLoadMachine(_OnOff);
        go_VendingM[_Num].SetActive(_OnOff);
    }

    public bool OnSaveMachineOnOff(int _Num)
    {
        return t_ParentMachine.GetChild(_Num).GetComponent<MachineStat_Scr>().b_Bought;
    }

    public void buyMachine()
    {
        i_NowBoughtMachine++;
        MachineAchievmentCheck();
        for (int i = 0; i < t_ParentMachine.childCount; ++i)
        {
            if (!t_ParentMachine.GetChild(i).GetComponent<MachineStat_Scr>().b_Bought)
                t_ParentMachine.GetChild(i).GetComponent<MachineStat_Scr>().SetPlusPrice((int)MachineData[i_NowBoughtMachine]["Buy"]);
            else if(go_VendingM[i].activeSelf == false)
            {
                go_VendingM[i].SetActive(true);
                //g_BuyCan.SetActive(true);
            }
        }
    }

    public void MachineAchievmentCheck()
    {

#if UNITY_ANDROID
        if (GoogleManager_Scr.instance != null && GoogleManager_Scr.instance.b_isLogin)
        {
            if (i_NowBoughtMachine == 1)
            {
                GoogleManager_Scr.instance.GetAchievementMachineOne();
            }
            else if (i_NowBoughtMachine == 2)
            {
                GoogleManager_Scr.instance.GetAchievementMachineTwo();
            }
            else if (i_NowBoughtMachine == 3)
            {
                GoogleManager_Scr.instance.GetAchievementMachineThree();
            }
            else if (i_NowBoughtMachine == 4)
            {
                GoogleManager_Scr.instance.GetAchievementMachineFour();
            }
            else if (i_NowBoughtMachine == 5)
            {
                GoogleManager_Scr.instance.GetAchievementMachineFive();
            }
            else if (i_NowBoughtMachine == 6)
            {
                GoogleManager_Scr.instance.GetAchievementMachineSix();
            }
        }

#elif UNITY_IOS

        if (AppleManager_Scr.instance != null && AppleManager_Scr.instance.b_isLogin)
        {
            if (i_NowBoughtMachine == 1)
            {
                AppleManager_Scr.instance.GetAchievementMachineOne();
            }
            else if (i_NowBoughtMachine == 2)
            {
                AppleManager_Scr.instance.GetAchievementMachineTwo();
            }
            else if (i_NowBoughtMachine == 3)
            {
                AppleManager_Scr.instance.GetAchievementMachineThree();
            }
            else if (i_NowBoughtMachine == 4)
            {
                AppleManager_Scr.instance.GetAchievementMachineFour();
            }
            else if (i_NowBoughtMachine == 5)
            {
                AppleManager_Scr.instance.GetAchievementMachineFive();
            }
            else if (i_NowBoughtMachine == 6)
            {
                AppleManager_Scr.instance.GetAchievementMachineSix();
            }
        }

#endif
    }

    public void OnClickMachineBuyButton()
    {
        t_ParentMachine.GetChild(i_MachineSelected).GetComponent<MachineStat_Scr>().OnClickBuyMachine();
    }

    public void OnClickMachineCancleBuy()
    {
        i_LastSelected = i_CancelPurchaseSelected;
        if (!b_isSoundOn)
            SoundManager_sfx.instance.PlaySE("UI_Click1", false);
        b_isSoundOn = false;
        g_SelectBuyButton.SetActive(false);
    }
}
