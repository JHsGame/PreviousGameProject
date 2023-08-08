using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveFunctionBar : MonoBehaviour, IDragHandler
{
    public GameObject Group;
    public static bool onDragFunctionBar;

    void Update()
    {
        /*
        // 입력 포지션이 UI 오브젝트 위에 있는지 체크
        if (EventSystem.current.IsPointerOverGameObject())
        {
            onDragFunctionBar = true;
        }
        else
        {
            onDragFunctionBar = false;
        }*/
    }

    // 현재는 기능을 안하는 함수.
    public void OnDrag(PointerEventData eventData)
    {
        Group.transform.position = eventData.position;
    }
}