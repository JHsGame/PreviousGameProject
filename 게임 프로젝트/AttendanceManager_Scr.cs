using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class AttendanceManager_Scr : MonoBehaviour
{
    public static AttendanceManager_Scr instance;

    public GameObject g_DontSeeObj;
    public GameObject g_CheckImage;
    public Transform t_SlotParent;

    public int i_AccessDay;
    public int i_LastAccessDay;
    public int i_GetCount = 10;
    public DateTimeOffset d_StartDay;  // 시작날짜
    public DateTimeOffset d_EndDay;    // 시작날짜 + 6 -> StartDay == EndDay -> Reset();


    public bool b_isNotFile = false;
    public bool b_isStarted;    // 출석 시작했는지 체크
    public bool b_isLoaded;
    public bool b_DontSee;
    public bool b_isAbsent;
     
    public bool b_AutoPause = false;

    int i_AttendanceDay = 0;
    public void AddCallendars()
    {
        new System.Globalization.GregorianCalendar();
        new System.Globalization.PersianCalendar();
        new System.Globalization.UmAlQuraCalendar();
        new System.Globalization.ThaiBuddhistCalendar();
    }

    private void Awake()
    {
        if (instance == null)
        {
            i_AccessDay = DateTimeOffset.UtcNow.LocalDateTime.Day;
            instance = this;
        }
        else
            return;
    }

    public void readytoDelay()
    {
        Invoke("Setting", 1f);
    }

    public void Setting()
    {
        b_DontSee = Save_Load.instance.s_Data.b_DontSee;
    }

    public void OnClickDontSee()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);
        if (b_DontSee)
        {
            b_DontSee = false;
            g_CheckImage.SetActive(b_DontSee);
        }
        else
        {
            b_DontSee = true;
            g_CheckImage.SetActive(b_DontSee);
        }
        Save_Load.instance.Save();
    }

    // Save_Load 스크립트에서 불러오기
    public void LoadAttendance()
    {
        if (DateTimeOffset.UtcNow.LocalDateTime.Day >= d_EndDay.Day)
        {
            b_isStarted = false;
        }
        else
        {
            b_isStarted = true;
        }


        if (i_AccessDay != i_LastAccessDay)
        {
            i_GetCount = 10;
            b_DontSee = false;
            b_isLoaded = false;
            MissionResetCheck();

            if (MissionManager_Scr.instance.b_DayDiff)
            {
                if (Button_Option.instance.b_isEndIntro && !Button_Option.instance.b_GoAttendanceActive)
                {
                    Button_Option.instance.DelayAttendanceON();
                }
            }
        }
        if (!b_isStarted)
        {
            d_StartDay = DateTimeOffset.UtcNow.LocalDateTime;
            d_EndDay = DateTimeOffset.UtcNow.LocalDateTime.AddDays(5);
            for (int i = 0; i < t_SlotParent.childCount - 1; ++i)
            {
                t_SlotParent.GetChild(i).GetComponent<AttendanceSlot_Scr>().ResetSlot();
                t_SlotParent.GetChild(i).GetComponent<AttendanceSlot_Scr>().d_Day = DateTimeOffset.UtcNow.LocalDateTime.AddDays(i);
            }
            t_SlotParent.GetChild(0).GetComponent<AttendanceSlot_Scr>().Reward(DateTimeOffset.UtcNow.LocalDateTime);

            b_isStarted = true;
        }
        else if (b_isStarted)
        {
            if (d_EndDay != d_StartDay.AddDays(5))
                d_EndDay = d_StartDay.AddDays(5);
            // 데이터 받고 난 후의 행동
            for (int i = 0; i < t_SlotParent.childCount; ++i)
            {
                if (t_SlotParent)
                    t_SlotParent.GetChild(i).GetComponent<AttendanceSlot_Scr>().Reward(DateTimeOffset.UtcNow.LocalDateTime);
            }


            for (int i = 0; i < t_SlotParent.childCount - 1; ++i)
            {
                if (t_SlotParent.GetChild(i).GetComponent<AttendanceSlot_Scr>().b_Attendance)
                    i_AttendanceDay++;
                else if (t_SlotParent.GetChild(i).GetComponent<AttendanceSlot_Scr>().b_Absent)
                    b_isAbsent = true;
            }

            if (!b_isAbsent && i_AttendanceDay >= 5)
                t_SlotParent.GetChild(t_SlotParent.childCount - 1).GetComponent<AttendanceSlot_Scr>().GetGemReward(true);
            else if (b_isAbsent)
                t_SlotParent.GetChild(t_SlotParent.childCount - 1).GetComponent<AttendanceSlot_Scr>().GetGemReward(false);
        }
    }

    public void MissionResetCheck()
    {
        if (i_LastAccessDay != i_AccessDay)
        {
            MissionManager_Scr.instance.b_DayDiff = true;
            MissionManager_Scr.instance.ResetMission();
        }
        else
            b_isLoaded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (b_isLoaded)
        {
            i_LastAccessDay = DateTimeOffset.UtcNow.LocalDateTime.Day;


            if (i_AccessDay != i_LastAccessDay)
            {
                if (MissionManager_Scr.instance != null)
                {
                    MissionManager_Scr.instance.b_DayDiff = true;
                    MissionManager_Scr.instance.ResetMission();
                    LoadAttendance();
                }
            }
        }
    }
}