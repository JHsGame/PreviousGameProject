using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// 케이스 리스트 화면 중 케이스 항목을 선택하면 발생하는 이벤트를 위한 클래스
public class SelectCase : MonoBehaviour
{
    public Case info;

    // 케이스 항목을 선택하면 실행되는 함수 
    public void SelectCASE()
    {
        for (int i = 0; i < StaticManager.instance.scr_CaseListManager.Info_Data.Length; i++)
        {
            StaticManager.instance.scr_CaseListManager.Info_Data[i].transform.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        }

        // 선택된 옵션으로 텍스트 변경
        switch (info._Case_type)
        {
            case 0:
                StaticManager.instance.scr_CaseListManager.Info_Casetype.transform.GetChild(1).GetComponent<Text>().text = "Surgical Guide";
                break;

            default:
                break;
        }

        // 선택된 옵션으로 텍스트 변경
        switch (info._Kit)
        {
            case 0:
                StaticManager.instance.scr_CaseListManager.Info_Kit.transform.GetChild(1).GetComponent<Text>().text = "Non-Kit";
                break;

            case 1:
                StaticManager.instance.scr_CaseListManager.Info_Kit.transform.GetChild(1).GetComponent<Text>().text = "PYLON Kit";
                break;

            case 2:
                StaticManager.instance.scr_CaseListManager.Info_Kit.transform.GetChild(1).GetComponent<Text>().text = "PYLON Sinus Kit";
                break;

            case 3:
                StaticManager.instance.scr_CaseListManager.Info_Kit.transform.GetChild(1).GetComponent<Text>().text = "PYLON Tapered Kit";
                break;

            default:
                break;
        }

        // 선택된 항목의 이미지 색상 변경
        if (info._Patient_CT.Length > 0)
        {
            for (int i = 0; i < info._Patient_CT.Length; i++)
            {
                if (info._Patient_CT[i] != null)
                {
                    StaticManager.instance.scr_CaseListManager.Info_Data[i].transform.GetComponent<Image>().color = new Color(150f / 255f, 150f / 255f, 150f / 255f);
                }
            }
        }

        // 3D Volume 변경
        StaticManager.instance.scr_AddCaseManager.controller.ChangeTexture(StaticManager.instance.scr_AddCaseManager.saved_Volume[info._Patient_CT[info._Patient_CT.Length - 1]]);
    }
}
