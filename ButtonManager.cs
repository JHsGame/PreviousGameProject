using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using Newtonsoft.Json;
using System.IO;
using SFB;
using UnityVolumeRendering;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    public TestController controller;


    private static ButtonManager Instance;

    public static ButtonManager instance { get => Instance; }

    private void Awake()
    {
        if (Instance != null)
        {


            return;
        }
        else
        {
            Instance = this;
        }
    }

    // DICOM 파일만을 불러왔거나, STL 파일만을 불러왔거나, 두 형식의 파일을 모두 불러왔거나에 따른 캔버스 활성화 여부를 결정해주는 함수.
    // 추후에 캔버스 로드 관련하여 관리하기 편하도록 만들어 놓은 함수이다.
    public void ChooseStartType()
    {
        if (AddCaseManager.instance.isLoadDicom && !AddCaseManager.instance.isLoadSTL)
        {
            CreateNewCaseAndWork();
        }
        else if (AddCaseManager.instance.isLoadSTL && !AddCaseManager.instance.isLoadDicom)
        {
            LoadSTLFile();
        }
        else if (AddCaseManager.instance.isLoadDicom && AddCaseManager.instance.isLoadSTL)
        {
            LoadCaptureCanvas();
        }
        AddCaseManager.instance.resetCase_info();
    }

    // 새로운 케이스를 만드는 함수로, DICOM 파일만을 따로 불러왔을 때 사용되는 함수이다.
    // 케이스리스트에서 만들어낸 케이스 정보들을 불러오는 역할이다.
    public void CreateNewCase()
    {

        AddCase_DataBox();
        AddCaseManager Script = StaticManager.instance.scr_AddCaseManager;
        Script.getCase_info();
        //  CaseData newData = ScriptableObject.CreateInstance<CaseData>();

        StaticManager.instance.scr_CaseListManager.CaseData._Name = Script.Name;
        StaticManager.instance.scr_CaseListManager.CaseData._Patient_Description = Script.Patient_Description;
        StaticManager.instance.scr_CaseListManager.CaseData._Case_name = Script.Case_name;
        StaticManager.instance.scr_CaseListManager.CaseData._Client = Script.Client;
        StaticManager.instance.scr_CaseListManager.CaseData._Due_date = Script.Due_date;
        StaticManager.instance.scr_CaseListManager.CaseData._Case_type = Script.Case_type;
        StaticManager.instance.scr_CaseListManager.CaseData._Case_Description = Script.Case_Description;

        for (int i = 0; i < Script.Option.Length; i++)
        {
            StaticManager.instance.scr_CaseListManager.CaseData._Option[i] = Script.Option[i];
        }
        for (int i = 0; i < Script.Implant_planning.Length; i++)
        {
            StaticManager.instance.scr_CaseListManager.CaseData._Implant_planning[i] = Script.Implant_planning[i];
        }

        StaticManager.instance.scr_CaseListManager.CaseData._Patient_CT = Script.Patient_CT;
        StaticManager.instance.scr_CaseListManager.CaseData._Upper_CT_or_Scan = Script.Upper_CT_or_Scan;
        StaticManager.instance.scr_CaseListManager.CaseData._Lower_CT_or_Scan = Script.Lower_CT_or_Scan;
        StaticManager.instance.scr_CaseListManager.CaseData._Extra_CT_or_Scan = Script.Extra_CT_or_Scan;
        StaticManager.instance.scr_CaseListManager.CaseData._Kit = Script.Kit;

        for (int i = 0; i < StaticManager.instance.scr_CaseManagementManager.InfoData.Length; i++)
        {
            StaticManager.instance.scr_CaseManagementManager.InfoData[i].transform.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        }
        switch (StaticManager.instance.scr_CaseListManager.CaseData._Case_type)
        {
            case 0:
                StaticManager.instance.scr_CaseManagementManager.InfoCaseType.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Surgical Guide";
                break;

            default:
                break;
        }

        switch (StaticManager.instance.scr_CaseListManager.CaseData._Kit)
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

        if (StaticManager.instance.scr_CaseListManager.CaseData._Patient_CT.Length > 0)
        {
            for (int i = 0; i < StaticManager.instance.scr_CaseListManager.CaseData._Patient_CT.Length; i++)
            {
                if (StaticManager.instance.scr_CaseListManager.CaseData._Patient_CT[i] != null)
                {
                    StaticManager.instance.scr_CaseManagementManager.InfoData[i].transform.GetComponent<Image>().color = new Color(150f / 255f, 150f / 255f, 150f / 255f);
                }
            }
        }

        string Data = JsonConvert.SerializeObject(StaticManager.instance.scr_CaseListManager.CaseData);

        //  string Data = JsonUtility.ToJson(StaticManager.instance.scr_CaseListManager.CaseData);

        File.WriteAllText(StaticManager.instance.scr_SettingsManager.CaseData_Path + "/CaseData_" + StaticManager.instance.scr_CaseListManager.CaseData._Name + ".json", Data);
        //  AssetDatabase.CreateAsset(newData, "Assets/Resources/SO/" + Script.Name + ".asset");

        //CameraManager_Scr.instance.ChangeCameraGroup(CameraManager_Scr.instance.CaseList_CameraGroup);
        StaticManager.instance.CaseList_Canvas.SetActive(true);
        StaticManager.instance.AddCase_Canvas.SetActive(false);
        Script.resetCase_info();
    }


    // 설정 화면에서 데이터베이스 경로 설정창을 켜는 함수이다.
    public void Setting_databasePath()
    {
        StaticManager.instance.scr_SettingsManager.Database_Path_option.SetActive(true);
    }
    // 설정 화면에서 데이터베이스 경로 설정창을 닫는 함수이다.
    public void Setting_databasePath_disable()
    {
        StaticManager.instance.scr_SettingsManager.Database_Path_option.SetActive(false);
    }

    // 선택한 경로상의 데이터베이스 파일을 현재의 경로로 복사하는 함수이다. 
    public void Setting_databasePath_Import()
    {
        string path = "";
        var paths = StandaloneFileBrowser.OpenFolderPanel("Select Folder", "", true);
        foreach (var p in paths)
        {
            path += p;
        }

        var info = new DirectoryInfo(path);
        var fileInfo = info.GetFiles();

        foreach (var file in fileInfo)
        {
            string str_extension = file.Extension;
            string[] words = file.ToString().Split('\\');
            System.IO.File.Move(file.ToString(), StaticManager.instance.scr_SettingsManager.Database_Path + "/" + words[words.Length - 1] + str_extension);

            /*if (file.Extension == ".json" && file.Name.Contains("CaseData_"))
            {
                string[] words = file.ToString().Split('\\');
                System.IO.File.Move(file.ToString(), StaticManager.instance.scr_SettingsManager.Database_Path + "/" + words[words.Length - 1] + str_extension);// efefefe에 파일 이름만 넣으면 됨
            }*/
        }

        AddCase_DataBox();

        StaticManager.instance.scr_SettingsManager.SaveValue();

        string Data = JsonConvert.SerializeObject(StaticManager.instance.scr_SettingsManager.SettingValueData);

        File.WriteAllText(Application.persistentDataPath + "/PYLON_SettingValue.json", Data);
        //  AssetDatabase.CreateAsset(newData, "Assets/Resources/SO/" + Script.Name + ".asset");

        StaticManager.instance.scr_SettingsManager.Database_Path_option.SetActive(false);
    }


    // 현재 경로상의 데이터베이스 파일을 선택한 경로로 복사하는 함수이다.
    public void Setting_databasePath_Export()
    {
        string path = "";
        var paths = StandaloneFileBrowser.OpenFolderPanel("Select Folder", "", true);
        foreach (var p in paths)
        {
            path += p;
        }

        var info = new DirectoryInfo(StaticManager.instance.scr_SettingsManager.Database_Path);
        var fileInfo = info.GetFiles();

        foreach (var file in fileInfo)
        {


            string str_extension = file.Extension;
            string[] words = file.ToString().Split('\\');
            System.IO.File.Copy(file.ToString(), path + "/" + words[words.Length - 1] + str_extension, true);  // efefefe에 파일 이름만 넣으면 됨

            /*if (file.Extension == ".json" && file.Name.Contains("CaseData_"))
            {
                string[] words = file.ToString().Split('\\');
                System.IO.File.Copy(file.ToString(), path + "/"+ words[words.Length-1] + ".json", true);  // efefefe에 파일 이름만 넣으면 됨
            }*/
        }

        AddCase_DataBox();

        StaticManager.instance.scr_SettingsManager.SaveValue();

        string Data = JsonConvert.SerializeObject(StaticManager.instance.scr_SettingsManager.SettingValueData);

        File.WriteAllText(Application.persistentDataPath + "/PYLON_SettingValue.json", Data);
        //  AssetDatabase.CreateAsset(newData, "Assets/Resources/SO/" + Script.Name + ".asset");

        StaticManager.instance.scr_SettingsManager.Database_Path_option.SetActive(false);
    }


    // 현재 경로상의 데이터베이스 파일을 선택한 경로로 이동하는 함수이다.
    public void Setting_databasePath_Move()
    {
        string path = "";
        var paths = StandaloneFileBrowser.OpenFolderPanel("Select Folder", "", true);
        foreach (var p in paths)
        {
            path += p;
        }

        var info = new DirectoryInfo(StaticManager.instance.scr_SettingsManager.Database_Path);
        var fileInfo = info.GetFiles();

        foreach (var file in fileInfo)
        {
            string str_extension = file.Extension;
            string[] words = file.ToString().Split('\\');
            System.IO.File.Move(file.ToString(), path + "/" + words[words.Length - 1] + str_extension); // efefefe에 파일 이름만 넣으면 됨

            /*
            if (file.Extension == ".json" && file.Name.Contains("CaseData_"))
            {
                string[] words = file.ToString().Split('\\');
                System.IO.File.Move(file.ToString(), path + "/" + words[words.Length - 1] + ".json"); // efefefe에 파일 이름만 넣으면 됨
            }*/
        }

        AddCase_DataBox();

        StaticManager.instance.scr_SettingsManager.SaveValue();

        string Data = JsonConvert.SerializeObject(StaticManager.instance.scr_SettingsManager.SettingValueData);

        File.WriteAllText(Application.persistentDataPath + "/PYLON_SettingValue.json", Data);
        //  AssetDatabase.CreateAsset(newData, "Assets/Resources/SO/" + Script.Name + ".asset");

        StaticManager.instance.scr_SettingsManager.Database_Path_option.SetActive(false);
    }


    // 현재 데이터베이스 경로를 새로 지정하는 함수이다.
    public void Setting_databasePath_Link()
    {
        string path = "";
        var paths = StandaloneFileBrowser.OpenFolderPanel("Select Folder", "", true);
        foreach (var p in paths)
        {
            path += p;
        }

        AddCase_DataBox();

        StaticManager.instance.scr_SettingsManager.SaveValue();

        string Data = JsonConvert.SerializeObject(StaticManager.instance.scr_SettingsManager.SettingValueData);

        File.WriteAllText(Application.persistentDataPath + "/PYLON_SettingValue.json", Data);
        //  AssetDatabase.CreateAsset(newData, "Assets/Resources/SO/" + Script.Name + ".asset");

        StaticManager.instance.scr_SettingsManager.Database_Path = path.ToString();
        StaticManager.instance.scr_SettingsManager.text_Database_Path.text = StaticManager.instance.scr_SettingsManager.Database_Path;
        StaticManager.instance.scr_SettingsManager.Database_Path_option.SetActive(false);
    }


    // 디자인 파일을 저장하는 경로 설정하는 함수이다.
    public void Setting_Design_file_path()
    {
        string path = "";
        var paths = StandaloneFileBrowser.OpenFolderPanel("Select Folder", "", true);
        foreach (var p in paths)
        {
            path += p;
        }

        StaticManager.instance.scr_SettingsManager.Designfile_path = path.ToString();
        StaticManager.instance.scr_SettingsManager.text_DesignFile_Path.text = StaticManager.instance.scr_SettingsManager.Designfile_path;
    }


    // 케이스 파일을 저장하는 경로 설정하는 함수이다.
    public void Setting_Case_path()
    {
        string path = "";
        var paths = StandaloneFileBrowser.OpenFolderPanel("Select Folder", "", true);
        foreach (var p in paths)
        {
            path += p;
        }

        StaticManager.instance.scr_SettingsManager.CaseData_Path = path.ToString();
        StaticManager.instance.scr_SettingsManager.text_CaseData_Path.text = StaticManager.instance.scr_SettingsManager.CaseData_Path;
    }


    // 옵션창을 여는 함수이다.
    public void go_Option()
    {
        AddCase_DataBox();
        StaticManager.instance.Setting_Canvas.SetActive(true);
        StaticManager.instance.CaseList_Canvas.SetActive(false);
        StaticManager.instance.AddCase_Canvas.SetActive(false);
        StaticManager.instance.WorkStep_Canvas.SetActive(false);
        StaticManager.instance.Slice_Canvas.SetActive(false);
        StaticManager.instance.STL_Canvas.SetActive(false);
    }

    public void Add_dcmFiletoCase()
    {

    }


    // 케이스 추가화면에서 바로 워크스텝화면으로 넘어가기 위한 함수이다.
    public void CreateNewCaseAndWork()
    {
        controller.myVolumeObj = StaticManager.instance.WorkStepVolumeObj;
        controller.myVolumeObj.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 0f, -10f));

        CreateNewCase();
        StaticManager.instance.CaseList_Canvas.SetActive(false);
        StaticManager.instance.WorkStep_Canvas.SetActive(true);
        //CameraManager_Scr.instance.ChangeCameraGroup(CameraManager_Scr.instance.WorkStep_CameraGroup);
        //CameraManager_Scr.instance.objCam.transform.localPosition = new Vector3(0f, 0f, -0.5f);
        StaticManager.instance.BG_1080On(StaticManager.instance.WorkStep_Canvas.GetComponent<CanvasScaler>());
        StaticManager.instance.scr_PreSet.CTOTF = StaticManager.instance.WorkCanvasOTF;

        // 위, 아래 -> y축 조작
        VolumeObjectFactory.SpawnCrossSectionPlane(TestController.instance.myVolumeObj, new Vector3(0f, 20f, 0f), new Vector3(-90f, 0f, 0f));
        VolumeObjectFactory.SpawnCrossSectionPlane(TestController.instance.myVolumeObj, new Vector3(0f, -20f, 0f), new Vector3(90f, 0f, 0f));

        // 앞, 뒤 -> z축 조작
        VolumeObjectFactory.SpawnCrossSectionPlane(TestController.instance.myVolumeObj, new Vector3(0f, 0f, -20f), new Vector3(180f, 0f, 0f));
        VolumeObjectFactory.SpawnCrossSectionPlane(TestController.instance.myVolumeObj, new Vector3(0f, 0f, 20f), new Vector3(0f, 0f, 0f));

        // 좌, 우 -> x축 조작
        VolumeObjectFactory.SpawnCrossSectionPlane(TestController.instance.myVolumeObj, new Vector3(-20f, 0f, 0f), new Vector3(0f, -90f, 0f));
        VolumeObjectFactory.SpawnCrossSectionPlane(TestController.instance.myVolumeObj, new Vector3(20f, 0f, 0f), new Vector3(0f, 90f, 0f));
    }

    // 케이스 추가화면에서 케이스 추가를 취소할 때 사용하는 함수이다.
    public void CancelCase()
    {
        AddCase_DataBox();
        //CameraManager_Scr.instance.ChangeCameraGroup(CameraManager_Scr.instance.CaseList_CameraGroup);
        StaticManager.instance.CaseList_Canvas.SetActive(true);
        StaticManager.instance.AddCase_Canvas.SetActive(false);

        AddCaseManager Script = StaticManager.instance.scr_AddCaseManager;
        Script.resetCase_info();
    }


    // 새롭게 추가할 케이스 리스트의 정보를 입력하는 캔버스를 활성화시키는 함수.
    public void AddCase()
    {
        AddCase_DataBox();
        StaticManager.instance.CaseList_Canvas.SetActive(false);
        StaticManager.instance.AddCase_Canvas.SetActive(true);
    }


    // 이전에 데이터 박스 영역에 넣었던 파일을 전부 초기화시키는 역할을 하는 함수.
    public void AddCase_DataBox()
    {
        // 데이터 박스에 파일 형식에 따른 이미지, 파일명 등을 표시해주는 각각의 아이템들의 집합.
        Transform Contents = StaticManager.instance.AddCase_Databox_ScrollView.transform.GetChild(0).GetChild(0);
        for (int i = 0; i < Contents.transform.childCount; ++i)
        {
            Contents.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = null;
            Contents.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color(0f / 255f, 0f / 255f, 0f / 255f, 0f / 255f);
            Contents.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text = null;
            Contents.transform.GetChild(i).gameObject.SetActive(false);
        }
        StaticManager.instance.AddCase_Databox_InsideBox.SetActive(true);
        StaticManager.instance.AddCase_Databox_ScrollView.SetActive(false);
    }


    // STL 파일만 불러왔을 때 STL 캔버스를 활성화시키는 함수.
    public void LoadSTLFile()
    {
        AddCase_DataBox();
        StaticManager.instance.AddCase_Canvas.SetActive(false);
        StaticManager.instance.STL_Canvas.SetActive(true);
        //StaticManager.instance.BG.transform.parent.GetComponent<Canvas>().worldCamera = CameraManager_Scr.instance.STL_CameraGroup.transform.GetChild(2).GetComponent<Camera>();
        StaticManager.instance.BG_1040On(StaticManager.instance.STL_Canvas.GetComponent<CanvasScaler>());
        StaticManager.instance.scr_PreSet.CTOTF = StaticManager.instance.STLCanvasOTF;
    }

    // STL 파일과 DICOM 파일을 같이 로드하였을 때 사용되는 함수.
    // 캡쳐 캔버스로, Volume Rendering 오브젝트와 STL 파일의 오브젝트가 동시에 존재한다.
    public void LoadCaptureCanvas()
    {
        AddCase_DataBox();
        controller.myVolumeObj = StaticManager.instance.SubDataStepVolumeObj;
        controller.myVolumeObj.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 0f, -10f));

        CreateNewCase();
        StaticManager.instance.CaseList_Canvas.SetActive(false);
        StaticManager.instance.SubData_Canvas.SetActive(true);
        //StaticManager.instance.BG.transform.parent.GetComponent<Canvas>().worldCamera = CameraManager_Scr.instance.SubData_CameraGroup.transform.GetChild(2).GetComponent<Camera>();
        StaticManager.instance.BG_1080On(StaticManager.instance.SubData_Canvas.GetComponent<CanvasScaler>());
        StaticManager.instance.scr_PreSet.CTOTF = StaticManager.instance.SubDataCanvasOTF;
        //CameraManager_Scr.instance.ChangeCameraGroup(CameraManager_Scr.instance.SubData_CameraGroup);
    }

    // 워크스텝에서 케이스리스트화면으로 나가기 
    public void LeaveStep()
    {
        AddCase_DataBox();
        //CameraManager_Scr.instance.ChangeCameraGroup(CameraManager_Scr.instance.CaseList_CameraGroup);
        StaticManager.instance.CaseList_Canvas.SetActive(true);
        StaticManager.instance.AddCase_Canvas.SetActive(false);
        StaticManager.instance.WorkStep_Canvas.SetActive(false);
        StaticManager.instance.STL_Canvas.SetActive(false);
        StaticManager.instance.SubData_Canvas.SetActive(false);
    }

    // 설정 캔버스의 General 탭에서 사용되고 있는 각각의 설정 탭을 활성화하는 함수.
    // number가 각 버튼의 인덱스 값으로, 인덱스 값에 해당되는 설정 탭을 제외한 나머지 설정 탭들은 비활성화한다.
    public void settingGeneralButton(int number)
    {
        for (int i = 0; i < StaticManager.instance.SettingGeneralButton.Length; i++)
        {

            if (i == number)
            {
                StaticManager.instance.SettingGeneralButton[i].SetActive(true);
            }
            else
            {
                StaticManager.instance.SettingGeneralButton[i].SetActive(false);
            }
        }
    }

    // 설정 캔버스의 PYLON 탭에서 사용되고 있는 각각의 데이터 관련 탭들을 활성화하는 함수.
    // 마찬가지로 number가 인덱스 값으로, 인덱스 값에 해당되는 탭들을 제외한 나머지 탭들은 비활성화한다.
    public void settingTemporarycrownButton(int number)
    {
        for (int i = 0; i < StaticManager.instance.SettingTemporarycrownButton.Length; i++)
        {

            if (i == number)
            {
                StaticManager.instance.SettingTemporarycrownButton[i].SetActive(true);
            }
            else
            {
                StaticManager.instance.SettingTemporarycrownButton[i].SetActive(false);
            }
        }
    }

    // 세팅값 저장
    public void SettingSave()
    {
        AddCase_DataBox();

        StaticManager.instance.scr_SettingsManager.SaveValue();

        string Data = JsonConvert.SerializeObject(StaticManager.instance.scr_SettingsManager.SettingValueData);

        File.WriteAllText(Application.persistentDataPath + "/PYLON_SettingValue.json", Data);
        //  AssetDatabase.CreateAsset(newData, "Assets/Resources/SO/" + Script.Name + ".asset");

        StaticManager.instance.Setting_Canvas.SetActive(false);
        StaticManager.instance.CaseList_Canvas.SetActive(true);
        StaticManager.instance.AddCase_Canvas.SetActive(false);
        StaticManager.instance.WorkStep_Canvas.SetActive(false);
        StaticManager.instance.Slice_Canvas.SetActive(false);
        StaticManager.instance.STL_Canvas.SetActive(false);
    }

    // 세팅화면 나오기 
    public void SettingCancle()
    {
        AddCase_DataBox();
        StaticManager.instance.Setting_Canvas.SetActive(false);
        StaticManager.instance.CaseList_Canvas.SetActive(true);
        StaticManager.instance.AddCase_Canvas.SetActive(false);
        StaticManager.instance.WorkStep_Canvas.SetActive(false);
        StaticManager.instance.Slice_Canvas.SetActive(false);
        StaticManager.instance.STL_Canvas.SetActive(false);
    }

    // 3D Volume 오브젝트를 6방향으로 회전시키기
    public void VolumeRotateSort1() // 좌 앞
    {
        AddCaseManager.instance.VolumeObj.transform.rotation = Quaternion.Euler(new Vector3(90f, 45f, 0f));
    }
    public void VolumeRotateSort2() // 좌 뒤
    {
        AddCaseManager.instance.VolumeObj.transform.rotation = Quaternion.Euler(new Vector3(90f, -45f, 0f));
    }
    public void VolumeRotateSort3() // 위
    {
        AddCaseManager.instance.VolumeObj.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    }
    public void VolumeRotateSort4() // 우 뒤
    {
        AddCaseManager.instance.VolumeObj.transform.rotation = Quaternion.Euler(new Vector3(90f, -135f, 0f));
    }
    public void VolumeRotateSort5() // 우 앞
    {
        AddCaseManager.instance.VolumeObj.transform.rotation = Quaternion.Euler(new Vector3(90f, 135f, 0f));
    }
    public void VolumeRotateSort6() // 밑
    {
        AddCaseManager.instance.VolumeObj.transform.rotation = Quaternion.Euler(new Vector3(180f, 0f, 0f));
    }

    // 워크스텝, 캡쳐캔버스 등 각 캔버스에서 공통적인 부가기능 "캡쳐"버튼.
    // 좌측 상단의 3가지 버튼 중 첫 버튼에 해당.
    public void OnClickCaptureButton()
    {
        for(int i = 0; i < StaticManager.instance.captureWindow.Length; ++i)
        {
            StaticManager.instance.captureWindow[i].SetActive(!StaticManager.instance.captureWindow[i].activeSelf);
        }
    }

    public void OnClickCaptureList()
    {
        controller.VolumeObjsActive(StaticManager.instance.Capture_Canvas.gameObject.activeSelf);
        // 스크린샷을 찍은 리스트들을 보여주는 버튼.
        for (int i = 0; i < StaticManager.instance.captureWindow.Length; ++i)
        {
            StaticManager.instance.captureWindow[i].SetActive(!StaticManager.instance.captureWindow[i].activeSelf);
        }

        StaticManager.instance.Capture_Canvas.GetComponent<Canvas>().worldCamera = CameraManager_Scr.instance.uiCam;
        StaticManager.instance.Capture_Canvas.SetActive(true);
    }

    public void OnClickCaptureArea()
    {
        // 라인 그리는 부분을 막아놓자.
        StaticManager.instance.scr_CaseManagementManager.DrawLineObj.SetActive(false);

        // 사각형으로 그린 부분을 캡쳐하는 기능.
        for (int i = 0; i < StaticManager.instance.scr_DrawCaptureArea.Length; ++i) 
        {
            StaticManager.instance.scr_DrawCaptureArea[i].DelayOn();
        }

        for (int i = 0; i < StaticManager.instance.captureWindow.Length; ++i)
        {
            StaticManager.instance.captureWindow[i].SetActive(!StaticManager.instance.captureWindow[i].activeSelf);
        }
    }

    public void OnClickCpatureAll()
    {
        // 전체 화면을 캡쳐하는 기능.
        for (int i = 0; i < StaticManager.instance.captureWindow.Length; ++i)
        {
            StaticManager.instance.captureWindow[i].SetActive(!StaticManager.instance.captureWindow[i].activeSelf);
        }

        StaticManager.instance.scr_ScreenCapture.CaptureAll();
    }

    public void OnClickRemoveActive()
    {
        StaticManager.instance.RemoveCapture.SetActive(!StaticManager.instance.RemoveCapture.activeSelf);
    }

    public void OnClickRemoveButton()
    {
        Transform tf = StaticManager.instance.CaptureList;

        for (int i = 0; i < tf.childCount; ++i)
        {
            tf.GetChild(i).GetChild(0).GetComponent<Image>().sprite = null;
            tf.GetChild(i).gameObject.SetActive(false);
        }

        var info = new DirectoryInfo(Application.persistentDataPath);
        var fileInfo = info.GetFiles();
        foreach (var file in fileInfo)
        {
            if (file.Extension == ".png")
            {
                System.IO.File.Delete(file.FullName);
            }
        }

        for (int i = 0; i < StaticManager.instance.scr_DrawCaptureArea.Length; ++i)
        {
            StaticManager.instance.scr_DrawCaptureArea[i].ResetThumbnail();
        }

        StaticManager.instance.RemoveCapture.SetActive(!StaticManager.instance.RemoveCapture.activeSelf);
    }

    // 각각의 화면에서 좌측 상단의 3가지 기능 중 횡단면 기능 ON/Off 시키기
    public void OnClickAddTools()
    {
        StaticManager.instance.scr_CaseManagementManager.AddMenuOn = !StaticManager.instance.scr_CaseManagementManager.AddMenuOn;
    }

    // 각각의 화면에서 좌측 상단의 3가지 기능 중 데이터 방향 복원 기능 ON/Off 시키기
    public void OnClickVolumeRenderingResetPosition()
    {
        controller.myVolumeObj.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 0f, -10f));
    }


    // 각각의 화면에서 우측 상단의 3가지 기능 중 케이스 디테일보기 ON/Off 시키기
    public void OnClickCaseDetails()
    {
        StaticManager.instance.scr_CaseManagementManager.ManagementMenuOn[0] = !StaticManager.instance.scr_CaseManagementManager.ManagementMenuOn[0];

        StaticManager.instance.CaseManagement_Canvas.GetComponent<Canvas>().worldCamera = CameraManager_Scr.instance.uiCam;
        StaticManager.instance.CaseManagement_Canvas.SetActive(StaticManager.instance.scr_CaseManagementManager.ManagementMenuOn[0]);

        for (int i = 0; i < StaticManager.instance.CaseDetailButton.Length; ++i) 
        {
            StaticManager.instance.CaseDetailButton[i].SetActive(StaticManager.instance.scr_CaseManagementManager.ManagementMenuOn[0]);
        }
    }

    // 각각의 화면에서 우측 상단의 3가지 기능 중 케이스 정보 덮어쓰기 기능
    public void OnClickCaseSave()
    {
        StaticManager.instance.scr_CaseManagementManager.ManagementMenuOn[1] = !StaticManager.instance.scr_CaseManagementManager.ManagementMenuOn[1];

        for (int i = 0; i < StaticManager.instance.scr_CaseManagementManager.SaveList.Length; ++i)
        {
            StaticManager.instance.scr_CaseManagementManager.SaveList[i].SetActive(StaticManager.instance.scr_CaseManagementManager.ManagementMenuOn[1]);
        }

        StaticManager.instance.CaseManagement_Canvas.GetComponent<Canvas>().worldCamera = CameraManager_Scr.instance.uiCam;
    }

    public void OnClickSaveMessage()
    {
        StaticManager.instance.scr_CaseManagementManager.ManagementMenuOn[1] = !StaticManager.instance.scr_CaseManagementManager.ManagementMenuOn[1];

        for (int i = 0; i < StaticManager.instance.scr_CaseManagementManager.SaveList.Length; ++i)
        {
            StaticManager.instance.scr_CaseManagementManager.SaveList[i].SetActive(StaticManager.instance.scr_CaseManagementManager.ManagementMenuOn[1]);
        }

        print("저장 기능");
        //StaticManager.instance.scr_Dialog.SaveMessage.SetActive(true);
    }

    public void OnClickSaveMessageClose()
    {
        StaticManager.instance.scr_Dialog.SaveMessage.SetActive(false);
    }

    public void OnClickSave()
    {
        print("저장 기능");
    }

    public void OnClickSaveAsMessage()
    {
        StaticManager.instance.scr_CaseManagementManager.ManagementMenuOn[1] = !StaticManager.instance.scr_CaseManagementManager.ManagementMenuOn[1];

        for (int i = 0; i < StaticManager.instance.scr_CaseManagementManager.SaveList.Length; ++i)
        {
            StaticManager.instance.scr_CaseManagementManager.SaveList[i].SetActive(StaticManager.instance.scr_CaseManagementManager.ManagementMenuOn[1]);
        }

        StaticManager.instance.scr_Dialog.SaveAsMessage.SetActive(true);
    }

    public void OnClickSaveAsMessageClose()
    {
        StaticManager.instance.scr_Dialog.SaveAsMessage.SetActive(false);
        StaticManager.instance.scr_Dialog.SaveSuccess.SetActive(false);
    }

    public void OnClickSaveAs()
    {
        print("저장 기능");

        StaticManager.instance.scr_Dialog.SaveSuccess.SetActive(true);
    }

    // 각각의 화면에서 우측 상단의 3가지 기능 중 케이스 리스트로 화면 나가기 기능
    public void OnClickCaseOut()
    {
        StaticManager.instance.scr_CaseManagementManager.ManagementMenuOn[2] = !StaticManager.instance.scr_CaseManagementManager.ManagementMenuOn[2];

        StaticManager.instance.CaseManagement_Canvas.GetComponent<Canvas>().worldCamera = CameraManager_Scr.instance.uiCam;
    }

    public void OnClickCaptureWindowClose()
    {
        controller.VolumeObjsActive(StaticManager.instance.Capture_Canvas.gameObject.activeSelf);
        StaticManager.instance.Capture_Canvas.gameObject.SetActive(false);
    }

    public void OnClickChangePresetMode()
    {
        StaticManager.instance.scr_PreSet.ChangeMode();
    }

    public void OnClickWorkToolsExpand()
    {
        StaticManager.instance.scr_WorkTools.ExpandGroup(true);
    }

    public void OnClickWorkToolsContract()
    {
        StaticManager.instance.scr_WorkTools.ExpandGroup(false);
    }

    public void OnClickROI2DToggle(Transform transform)
    {
        transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
    }

    public void OnClickPreTools()
    {
        StaticManager.instance.scr_WorkTools.Idx--;
        if (StaticManager.instance.scr_WorkTools.Idx <= 0)
        {
            StaticManager.instance.scr_WorkTools.Idx = 0;
        }

        StaticManager.instance.scr_WorkTools.GroupChangeIdx();
    }
    public void OnClickNextTools()
    {
        StaticManager.instance.scr_WorkTools.Idx++;
        if (StaticManager.instance.scr_WorkTools.Idx >= StaticManager.instance.scr_WorkTools.ExpandGroupParent[0].childCount - 1)
        {
            StaticManager.instance.scr_WorkTools.Idx = StaticManager.instance.scr_WorkTools.ExpandGroupParent[0].childCount - 1;
        }

        StaticManager.instance.scr_WorkTools.GroupChangeIdx();
    }


    // 세팅화면에서 수치 조절하는 부분들의 화살표 버튼에 사용되는 기능 
    // 타겟이 되는 textUI 값을 int형 혹은 float형으로 증가하거나 감소시키는 역할의 함수들이다.
    public void ChangeValueToArrow_floatPlus(Text textUI)   // float형 증가
    {
        // if(textUI == StaticManager.instance.scr_SettingsManager.)

        float tmp = float.Parse(textUI.text);
        Range_SettingValue script = textUI.transform.GetComponent<Range_SettingValue>();

        tmp += script.Chage_Value;
        if (script.Range_Value[1] <= tmp)
        {
            tmp = script.Range_Value[1];
        }

        textUI.text = tmp.ToString();
    }
    public void ChangeValueToArrow_floatPlus(TextMeshProUGUI textUI)   // float형 증가
    {
        // if(textUI == StaticManager.instance.scr_SettingsManager.)

        float tmp = float.Parse(textUI.text);
        Range_SettingValue script = textUI.transform.GetComponent<Range_SettingValue>();

        tmp += script.Chage_Value;
        if (script.Range_Value[1] <= tmp)
        {
            tmp = script.Range_Value[1];
        }

        textUI.text = tmp.ToString();
    }
    public void ChangeValueToArrow_intPlus(Text textUI)     // int형 증가
    {
        int tmp = int.Parse(textUI.text);
        Range_SettingValue script = textUI.transform.GetComponent<Range_SettingValue>();
        tmp += 1;
        if (script.Range_Value[1] <= tmp)
        {
            tmp = (int)script.Range_Value[1];
        }
        textUI.text = tmp.ToString();
    }
    public void ChangeValueToArrow_intPlus(TextMeshProUGUI textUI)     // int형 증가
    {
        int tmp = int.Parse(textUI.text);
        Range_SettingValue script = textUI.transform.GetComponent<Range_SettingValue>();
        tmp += 1;
        if (script.Range_Value[1] <= tmp)
        {
            tmp = (int)script.Range_Value[1];
        }
        textUI.text = tmp.ToString();
    }
    public void ChangeValueToArrow_floatMinus(Text textUI)  // float형 감소
    {
        float tmp = float.Parse(textUI.text);
        Range_SettingValue script = textUI.transform.GetComponent<Range_SettingValue>();
        tmp -= script.Chage_Value;
        if (script.Range_Value[0] >= tmp)
        {
            tmp = script.Range_Value[0];
        }
        textUI.text = tmp.ToString();
    }
    public void ChangeValueToArrow_floatMinus(TextMeshProUGUI textUI)  // float형 감소
    {
        float tmp = float.Parse(textUI.text);
        Range_SettingValue script = textUI.transform.GetComponent<Range_SettingValue>();
        tmp -= script.Chage_Value;
        if (script.Range_Value[0] >= tmp)
        {
            tmp = script.Range_Value[0];
        }
        textUI.text = tmp.ToString();
    }
    public void ChangeValueToArrow_intMinus(Text textUI)    // int형 감소
    {
        int tmp = int.Parse(textUI.text);
        Range_SettingValue script = textUI.transform.GetComponent<Range_SettingValue>();
        tmp -= 1;
        if (script.Range_Value[0] >= tmp)
        {
            tmp = (int)script.Range_Value[0];
        }
        textUI.text = tmp.ToString();
    }
    public void ChangeValueToArrow_intMinus(TextMeshProUGUI textUI)    // int형 감소
    {
        int tmp = int.Parse(textUI.text);
        Range_SettingValue script = textUI.transform.GetComponent<Range_SettingValue>();
        tmp -= 1;
        if (script.Range_Value[0] >= tmp)
        {
            tmp = (int)script.Range_Value[0];
        }
        textUI.text = tmp.ToString();
    }


    // 케이스 리스트화면, 워크스텝화면에서 볼 수 있는 케이스 디테일 화면상단의 2가지 버튼(케이스 디테일, 데이터 박스)
    public void Preview_CaseDetail_ON(Transform tf_UI)
    {
        tf_UI.parent.GetChild(2).gameObject.SetActive(true);
        tf_UI.parent.GetChild(3).gameObject.SetActive(false);
    }
    public void Preview_DataBox_ON(Transform tf_UI)
    {
        tf_UI.parent.GetChild(2).gameObject.SetActive(false);
        tf_UI.parent.GetChild(3).gameObject.SetActive(true);
    }


    [SerializeField] GameObject MinimizeArrow;
    [SerializeField] GameObject MaximizeArrow;
    // Worktools 버튼용
    public void MinimizeButtonClick()
    {
        MinimizeArrow.SetActive(false);
        MaximizeArrow.SetActive(true);
    }

    public void MaximizeButtonClick()
    {
        MinimizeArrow.SetActive(true);
        MaximizeArrow.SetActive(false);
    }


    [SerializeField] Transform sliderpage;
    // Worktools change의 image를 클릭하면 슬라이더 창이 나타나게 함
    public void WorktoolsChangepageBackon1()
    {
        int i = 0;
        string ButtonName = EventSystem.current.currentSelectedGameObject.name;

        if (ButtonName == "image1") { i = 0; }
        if (ButtonName == "image2") { i = 1; }
        if (ButtonName == "image3") { i = 2; }
        sliderpage.GetChild(i).gameObject.SetActive(true);
    }

    public void WorktoolsChangepageBackoff1()
    {
        for (int i=0; i<3; i++)
        {
            sliderpage.GetChild(i).gameObject.SetActive(false);
        }
    }

}