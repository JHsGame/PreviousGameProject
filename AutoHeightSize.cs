using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoHeightSize : MonoBehaviour
{
    [SerializeField]
    private RectTransform _targetObj;

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(320f, _targetObj.sizeDelta.y);
    }
}