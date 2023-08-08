using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityVolumeRendering;
using QttEngine;

public class ProGrassButton : MonoBehaviour
{

    // 워크스텝 캔버스를 활성화 하고, Volume Rendering 오브젝트의 기본 회전값을 설정해주는 함수. 카메라 그룹 또한 변경해준다. 
    // 또한 해당 케이스의 정보를 가져와서 워크스텝의 미리보기 정보란에 기입하고, 3D 볼륨을 바꿔준다.
    public void WorkStep_Canvas()
    {
        ButtonManager.instance.controller.myVolumeObj.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 0f, -10f));

        StaticManager.instance.scr_PreSet.CTOTF = StaticManager.instance.WorkCanvasOTF;
        //CameraManager_Scr.instance.ChangeCameraGroup(CameraManager_Scr.instance.WorkStep_CameraGroup);
        //CameraManager_Scr.instance.objCam.transform.localPosition = new Vector3(-9.360003f, -0.06f, 97f);
        StaticManager.instance.BG_1080On(StaticManager.instance.WorkStep_Canvas.GetComponent<CanvasScaler>());

        /*
        // 위, 아래 -> y축 조작
        VolumeObjectFactory.SpawnCrossSectionPlane(TestController.instance.myVolumeObj, new Vector3(0f, 20f, 0f), new Vector3(-90f, 0f, 0f));
        VolumeObjectFactory.SpawnCrossSectionPlane(TestController.instance.myVolumeObj, new Vector3(0f, -20f, 0f), new Vector3(90f, 0f, 0f));

        // 앞, 뒤 -> z축 조작
        VolumeObjectFactory.SpawnCrossSectionPlane(TestController.instance.myVolumeObj, new Vector3(0f, 0f, -20f), new Vector3(180f, 0f, 0f));
        VolumeObjectFactory.SpawnCrossSectionPlane(TestController.instance.myVolumeObj, new Vector3(0f, 0f, 20f), new Vector3(0f, 0f, 0f));

        // 좌, 우 -> x축 조작
        VolumeObjectFactory.SpawnCrossSectionPlane(TestController.instance.myVolumeObj, new Vector3(-20f, 0f, 0f), new Vector3(0f, -90f, 0f));
        VolumeObjectFactory.SpawnCrossSectionPlane(TestController.instance.myVolumeObj, new Vector3(20f, 0f, 0f), new Vector3(0f, 90f, 0f));
        */

        Case info = transform.parent.parent.GetChild(0).GetChild(0).transform.GetComponent<SelectCase>().info;


        // 환자 이름 표기 
        StaticManager.instance.scr_CaseManagementManager.PatientName.text = info._Name;


        for (int i = 0; i < StaticManager.instance.scr_CaseManagementManager.InfoData.Length; i++)
        {
            StaticManager.instance.scr_CaseManagementManager.InfoData[i].transform.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        }

        // 선택된 옵션으로 텍스트 변경
        switch (info._Case_type)
        {
            case 0:
                StaticManager.instance.scr_CaseManagementManager.InfoCaseType.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Surgical Guide";
                break;

            default:
                break;
        }

        // 선택된 옵션으로 텍스트 변경
        switch (info._Kit)
        {
            case 0:
                StaticManager.instance.scr_CaseManagementManager.InfoKit.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Non-Kit";
                break;

            case 1:
                StaticManager.instance.scr_CaseManagementManager.InfoKit.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "PYLON Kit";
                break;

            case 2:
                StaticManager.instance.scr_CaseManagementManager.InfoKit.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "PYLON Sinus Kit";
                break;

            case 3:
                StaticManager.instance.scr_CaseManagementManager.InfoKit.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "PYLON Tapered Kit";
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
                    StaticManager.instance.scr_CaseManagementManager.InfoData[i].transform.GetComponent<Image>().color = new Color(150f / 255f, 150f / 255f, 150f / 255f);
                }
            }
        }

        // 3D Volume 변경
        // 여기서 저장되어있는 Data의 Path 부분을 Engine.LoadDicom으로 불러오자.
        string path = "";
        if (info._Patient_CT.Length > 0)
        {
            foreach (var p in info._Patient_CT)
            {
                path += p;
            }
        }

        AddCaseManager.instance.Add_DCMFolder_Paths(path);

        StaticManager.instance.CaseList_Canvas.SetActive(false);
        StaticManager.instance.AddCase_Canvas.SetActive(false);
        StaticManager.instance.WorkStep_Canvas.SetActive(true);
        /*
        StaticManager.instance.scr_AddCaseManager.VolumeObj = Engine.LoadDicom("DICOM", path);

        QttEngine.CrossSectionObject cs_Up = Engine.CreateCrossSection("CS_Up");
        Engine.SetDicomToCrossSection("CS_Up", "DICOM");
        AddCaseManager.instance.CrossSectionPlaneGroup(cs_Up.gameObject, new Vector3(-90f, 0f, 0f), "Up");

        QttEngine.CrossSectionObject cs_Down = Engine.CreateCrossSection("CS_Down");
        Engine.SetDicomToCrossSection("CS_Down", "DICOM");
        AddCaseManager.instance.CrossSectionPlaneGroup(cs_Down.gameObject, new Vector3(90f, 0f, 0f), "Down");

        QttEngine.CrossSectionObject cs_Left = Engine.CreateCrossSection("CS_Left");
        Engine.SetDicomToCrossSection("CS_Left", "DICOM");
        AddCaseManager.instance.CrossSectionPlaneGroup(cs_Left.gameObject, new Vector3(0f, -90f, 0f), "Left");

        QttEngine.CrossSectionObject cs_Right = Engine.CreateCrossSection("CS_Right");
        Engine.SetDicomToCrossSection("CS_Right", "DICOM");
        AddCaseManager.instance.CrossSectionPlaneGroup(cs_Right.gameObject, new Vector3(0f, 90f, 0f), "Right");

        QttEngine.CrossSectionObject cs_Front = Engine.CreateCrossSection("CS_Front");
        Engine.SetDicomToCrossSection("CS_Front", "DICOM");
        AddCaseManager.instance.CrossSectionPlaneGroup(cs_Front.gameObject, new Vector3(0f, 180f, 0f), "Front");

        QttEngine.CrossSectionObject cs_Behind = Engine.CreateCrossSection("CS_Back");
        Engine.SetDicomToCrossSection("CS_Back", "DICOM");
        AddCaseManager.instance.CrossSectionPlaneGroup(cs_Behind.gameObject, new Vector3(0f, 0f, 0f), "Back");

        StaticManager.instance.scr_AddCaseManager.controller.ChangeTexture(StaticManager.instance.scr_AddCaseManager.saved_Volume[info._Patient_CT[info._Patient_CT.Length - 1]]);
        */
    }
}
