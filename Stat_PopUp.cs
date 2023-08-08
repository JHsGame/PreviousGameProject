using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Stat_PopUp : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Transform t_Stats;
    public bool b_isClick;
    float f_Time;
    public void OnPointerDown(PointerEventData eventData)
    {
        b_isClick = true;
        f_Time = Time.unscaledTime;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        b_isClick = false;
        f_Time = Time.unscaledTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (b_isClick)
        {
            if (Time.unscaledTime - f_Time >= 1f)
            {
                print(1);
                for (int i = 0; i < t_Stats.childCount; ++i)
                {
                    t_Stats.GetChild(i).GetChild(1).gameObject.SetActive(true);
                }
            }
        }
        else
        {
            if (Time.unscaledTime - f_Time >= 1f)
            {
                for (int i = 0; i < t_Stats.childCount; ++i)
                {
                    t_Stats.GetChild(i).GetChild(1).gameObject.SetActive(false);
                }
            }
        }
    }
}
