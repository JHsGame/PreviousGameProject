using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TranslateStatPopUp_Scr : MonoBehaviour
{
    public int num;

    // Update is called once per frame
    void Update()
    {
        this.transform.GetComponent<TextMeshProUGUI>().text = TranslateManager_Scr.instance.TranslateContext(num);
    }
}
