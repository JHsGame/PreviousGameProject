using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager_Scr : MonoBehaviour
{
    public static MissionManager_Scr instance;

    public GameObject[] G_MissionList;


    public bool b_DayDiff = false;    // 날짜가 바뀌었는지

    public int[] i_MissionSlot = new int[3];    // 각 슬롯의 랜덤 미션
    public int[] i_TargetMission = new int[3];  // 각 미션들의 목표수치
    public int[] i_ClearMissionNum = new int[3]; // 각 미션들의 완성수치

    public bool[] b_MissionSuccess = new bool[3];   // 각 슬롯의 미션 성공 여부
    public bool[] b_GetReward = new bool[4];    // 각 슬롯의 미션 보상을 받았는지 체크용

    public bool b_isNoDie;
    public bool b_isNoQuit;
    public bool b_isNoHit; 

    string s_tmpClearNum;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            return;
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
            for (int i = 0; i < G_MissionList.Length - 1; ++i)
            {
                if (G_MissionList[i].GetComponent<MissionSlot_Scr>().b_GetMission)
                {
                    if (i_ClearMissionNum[i] >= i_TargetMission[i])
                        i_ClearMissionNum[i] = i_TargetMission[i];
                    s_tmpClearNum = string.Concat(i_ClearMissionNum[i], " / ");
                    G_MissionList[i].GetComponent<MissionSlot_Scr>().t_ClearNum.text = string.Concat(s_tmpClearNum, i_TargetMission[i]);
                }
            }
            yield return CoroutineManager_Scr.WaitForEndOfFrame;
        }
    }
    public void InitSlot()
    {
        for (int i = 0; i < G_MissionList.Length; ++i)
        {
            G_MissionList[i].GetComponent<MissionSlot_Scr>().ResetSlot();
        }
    }
    public void ResetMission()
    {
        DeliveryBoxManager_Scr.instance.ResetCount();
        b_isNoQuit = false;
        b_isNoHit = false;
        b_isNoDie = false;

        for (int i = 0; i < G_MissionList.Length; ++i)
        {
            G_MissionList[i].GetComponent<MissionSlot_Scr>().ResetSlot();
        }
        for(int i =0; i < i_ClearMissionNum.Length; ++i)
        {
            i_ClearMissionNum[i] = 0;
        }
        for (int i = 0; i < b_MissionSuccess.Length; ++i)
        {
            b_MissionSuccess[i] = false;
        }
        for (int i = 0; i < b_GetReward.Length; ++i)
        {
            b_GetReward[i] = false;
        }

        if (!TutorialManager.instance.b_FirstPlaying)
        {
            for (int i = 0; i < G_MissionList.Length - 1; ++i)
            {
                G_MissionList[i].GetComponent<MissionSlot_Scr>().OnClickGetMission();
            }
        }

        //AttendanceManager_Scr.instance.b_isLoaded = true;
        b_DayDiff = false;
    }
}
