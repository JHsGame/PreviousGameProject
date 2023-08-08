using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillPopUp : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject g_Skill;

    public int i_SkillNum;

    public bool b_isClick;

    float Target;
    private void Start()
    {
        i_SkillNum = g_Skill.transform.GetSiblingIndex();
    }

    private void Update()
    {
        if (b_isClick)
        {
            if (Time.unscaledTime - Target >= 1f)
            {
                // 스킬 팝업창 띄우기
                Button_Option.instance.SkillPopUpOn(i_SkillNum);
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        b_isClick = true;
        Target = Time.unscaledTime;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        b_isClick = false;
        Button_Option.instance.OnSkillPopUpOff(Button_Option.instance.i_PopUpSkillNum);
    }
}
