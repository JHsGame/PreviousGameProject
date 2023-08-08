using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RawImageMove : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public Transform _targetTr; // 이동될 UI

    private Vector2 _startingPoint;
    private Vector2 _moveBegin;
    private Vector2 _moveOffset;
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        _startingPoint = _targetTr.position;
        _moveBegin = eventData.position;
    }

    // 드래그 : 마우스 커서 위치로 이동
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (Input.GetMouseButton(2)) // wheel button  // 이동
        {
            _moveOffset = eventData.position - _moveBegin;
            _targetTr.position = _startingPoint + (_moveOffset * 0.1f);
        }

    }
}
