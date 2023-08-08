using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CalendarDateItem : MonoBehaviour {

    // 사용자가 화면에 나타난 캘린더에서 원하는 날짜를 선택했을 때 사용되는 함수.
    public void OnDateItemClick()
    {
        CalendarController._calendarInstance.OnDateItemClick(gameObject.GetComponentInChildren<Text>().text);
    }
}
