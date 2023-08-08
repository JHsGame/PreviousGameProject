using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarController : MonoBehaviour
{
    public GameObject _calendarPanel;
    public Text _yearNumText;
    public Text _monthNumText;

    public GameObject _item;

    public List<GameObject> _dateItems = new List<GameObject>();
    const int _totalDateNum = 42;

    private DateTime _dateTime;
    public static CalendarController _calendarInstance;

    void Start()
    {
        _calendarInstance = this;
        Vector3 startPos = _item.transform.localPosition;   // 날짜에 해당되는 요소들의 시작 위치를 정의.
        _dateItems.Clear();
        _dateItems.Add(_item);  // 날짜에 해당되는 요소들을 리스트에 추가.

        for (int i = 1; i < _totalDateNum; i++)
        {
            GameObject item = GameObject.Instantiate(_item) as GameObject;  // 캘린더에 각 월별 표시해야하는 일수들에 해당하는 UI들을 생성.
            item.name = "Item" + (i + 1).ToString();
            item.transform.SetParent(_item.transform.parent);
            item.transform.localScale = Vector3.one;
            item.transform.localRotation = Quaternion.identity;
            item.transform.localPosition = new Vector3((i % 7) * 36 + startPos.x, startPos.y - (i / 7) * 30, startPos.z);

            _dateItems.Add(item);
        }

        _dateTime = DateTime.Now;

        CreateCalendar();

        _calendarPanel.SetActive(false);
    }

    // 케이스리스트에서 일정을 만드는 함수이다.
    void CreateCalendar()
    {
        DateTime firstDay = _dateTime.AddDays(-(_dateTime.Day - 1));
        int index = GetDays(firstDay.DayOfWeek);

        int date = 0;
        for (int i = 0; i < _totalDateNum; i++)
        {
            Text label = _dateItems[i].GetComponentInChildren<Text>();
            _dateItems[i].SetActive(false);

            if (i >= index)
            {
                DateTime thatDay = firstDay.AddDays(date);
                if (thatDay.Month == firstDay.Month)
                {
                    _dateItems[i].SetActive(true);

                    label.text = (date + 1).ToString();
                    date++;
                }
            }
        }
        _yearNumText.text = _dateTime.Year.ToString();
        _monthNumText.text = _dateTime.Month.ToString("D2");
    }

    // 요일을 가져오는 함수이다.
    // CreateCalendar 함수에서 요일값을 넣어주는 데 쓰인다.
    int GetDays(DayOfWeek day)
    {
        switch (day)
        {
            case DayOfWeek.Monday: return 1;
            case DayOfWeek.Tuesday: return 2;
            case DayOfWeek.Wednesday: return 3;
            case DayOfWeek.Thursday: return 4;
            case DayOfWeek.Friday: return 5;
            case DayOfWeek.Saturday: return 6;
            case DayOfWeek.Sunday: return 0;
        }

        return 0;
    }

    // 현재 연도로부터 1년 전 연도로 캘린더를 만들어주는 역할을 하는 함수이다.
    public void YearPrev()
    {
        _dateTime = _dateTime.AddYears(-1);
        CreateCalendar();
    }

    // 현재 연도로부터 1년 뒤 연도로 캘린더를 만들어주는 역할을 하는 함수이다.
    public void YearNext()
    {
        _dateTime = _dateTime.AddYears(1);
        CreateCalendar();
    }

    // 현재 월로부터 1개월 전의 캘린더를 만들어주는 역할을 하는 함수이다.
    public void MonthPrev()
    {
        _dateTime = _dateTime.AddMonths(-1);
        CreateCalendar();
    }

    // 현재 월로부터 1개월 후의 캘린더를 만들어주는 역할을 하는 함수이다.
    public void MonthNext()
    {
        _dateTime = _dateTime.AddMonths(1);
        CreateCalendar();
    }

    // 생성되어있는 캘린더를 보여주는 역할을 한다.
    // target으로 설정된 Text는 내가 선택한 캘린더의 날짜를 보여주는 역할을 한다.
    public void ShowCalendar(Text target)
    {
        _calendarPanel.SetActive(true);
        _target = target;   // target은 사용자가 설정한 날짜를 보여주는 Text UI 이다.
        //_calendarPanel.transform.position = new Vector3(965, 475, 0);//Input.mousePosition-new Vector3(0,120,0);
    }

    Text _target;

    //Item 클릭했을 경우 Text에 표시.
    public void OnDateItemClick(string day)
    {
        _target.text = _yearNumText.text + "-" + _monthNumText.text + "-" + int.Parse(day).ToString("D2");
        _target.color = new Color(90 / 255f, 90 / 255f, 90 / 255f);
        _calendarPanel.SetActive(false);
    }

    // 캘린더에서 사용자가 지정했던 연도 및 월일을 표시해주는 텍스트를 초기해주는 함수이다.
    public void resetDate()
    {
        if (_target != null)
        {
            _target.text = "yyyy/MM/dd";
            _target.color = new Color(160 / 255f, 160 / 255f, 160 / 255f);
            _calendarPanel.SetActive(false);
        }
    }
}
