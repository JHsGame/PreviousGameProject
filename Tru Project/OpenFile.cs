using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenFile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string filePath;
    public bool b_isClick;
    float f_Time;

    // 데이터 박스 영역에 있는 파일 클릭을 체크하는 함수.
    private void Update()
    {
        if (b_isClick)
        {
            if (Time.unscaledTime - f_Time >= 0.1f)
            {
                OnClick_OpenFile();
            }
        }
    }

    // 클릭한 파일을 열어주는 함수.
    public void OnClick_OpenFile()
    {
        Application.OpenURL(filePath);
        //File.Open(filePath, FileMode.Open);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        b_isClick = true;
        f_Time = Time.unscaledTime;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        b_isClick = false;
    }
}
