using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dicom;
using Dicom.Imaging;
using Parabox.Stl;
using SFB;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityVolumeRendering;
using VolumeRendering;
using Newtonsoft.Json;
using TMPro;
using QttEngine;

public class AddCaseManager : MonoBehaviour
{
    private static AddCaseManager Instance;

    public static AddCaseManager instance { get => Instance; }

    // 추가한 케이스의 다이콤파일을 다음에 열람시 보다 빠르게 하기위해서 변환한 정보를 저장한다.
    public Dictionary<string, Texture3D> saved_Volume = new Dictionary<string, Texture3D>();    //
    public List<string> saved_VolumePath = new List<string>();

    // DICOM FILE TO VOLUME RENDERING
    public Texture2DArrayToTexture3DConverter converter;
    string error;
    public TestController controller;
    public TestController controller_Caselist;

    // STL FILE
    public GameObject STLCanvas_objMeshToExport;
    public GameObject CaptureCanvas_objMeshToExport;
    public GameObject source;
    public Mesh[] meshs;

    private GameObject VolumeObject;
    private GameObject Upper_STL;
    private GameObject Upper_STL_Capture;
    private GameObject Lower_STL;
    private GameObject Lower_STL_Capture;

    // VolumRender Script


    // Patient info
    public InputField t_Name;
    public string Name;

    public InputField t_Patient_Description;
    public string Patient_Description;

    public List<string> Data_Box = new List<string>();

    // Case info
    public InputField t_Case_name;
    public string Case_name;

    public InputField t_Client;
    public string Client;

    public CalendarController CalendarScript;
    public TextMeshProUGUI t_Due_date;
    public string Due_date;

    public TMP_Dropdown ui_Case_type;
    public int Case_type;

    public InputField t_Case_Description;
    public string Case_Description;

    public bool[] Design_setup = new bool[32];
    public bool[] Option = new bool[2];
    public bool[] Implant_planning = new bool[3];

    // Data
    public string[] Patient_CT;

    public string[] Upper_CT_or_Scan;

    public string[] Lower_CT_or_Scan;

    public string[] Extra_CT_or_Scan;

    public TMP_Dropdown ui_kit;
    public int Kit;

    // bool

    private bool b_isLoadDicom;
    private bool b_isLoadSTL;

    public bool isLoadDicom { get => b_isLoadDicom; }
    public bool isLoadSTL { get => b_isLoadSTL; }

    public bool possibleSave = false;


    public Transform save_startButton;
    public Transform saveButton;
    public Transform tf_TemporaryCrown;
    
    public GameObject VolumeObj
    {
        set => VolumeObj = value;
        get => VolumeObject;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        else
        {
            Instance = this;

            // 저장된 케이스 항목들의 3D Volume 미리 로드해놓기
            /*  string[] SerachFiles = Directory.GetFiles(Application.persistentDataPath, "PYLON_SaveCase3DVolumePath.json");

              for (int i = 0; i < SerachFiles.Length; i++)
              {
                  string data = File.ReadAllText(SerachFiles[i]);

                  List<string> tmp = JsonConvert.DeserializeObject<List<string>>(data);

                  saved_VolumePath = tmp;
              }

              for (int i = 0; i < saved_VolumePath.Count; i++)
              {
                  SystemIOFileLoad_SaveFile(saved_VolumePath[i]);
              }*/
        }
    }

    private void OnEnable()
    {
        Toothsupported_Button(tf_Toothsupported_Button);
    }

    // DICOM 파일 혹은 STL 파일 로드 후 UI쪽 텍스트 및 색깔 변경, 버튼 활성화
    public void Update()
    {
        if (possibleSave)
        {
            save_startButton.GetComponent<Button>().enabled = true;
            save_startButton.GetChild(0).gameObject.SetActive(false);
            save_startButton.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);

            saveButton.GetComponent<Button>().enabled = true;
            saveButton.GetChild(0).gameObject.SetActive(false);
            saveButton.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
        }
        else
        {
            save_startButton.GetComponent<Button>().enabled = false;
            save_startButton.GetChild(0).gameObject.SetActive(true);
            save_startButton.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(66f / 255f, 125f / 255f, 122f / 255f, 255f / 255f);

            saveButton.GetComponent<Button>().enabled = false;
            saveButton.GetChild(0).gameObject.SetActive(true);
            saveButton.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(66f / 255f, 125f / 255f, 122f / 255f, 255f / 255f);
        }

        if (b_CrownPlacement && b_SurgicalGuide)
        {
            if (b_TemporaryCrown)
            {
                tf_TemporaryCrown.GetChild(0).gameObject.SetActive(false);
              //  tf_TemporaryCrown.GetComponent<Image>().color = new Color(200f / 255f, 200f / 255f, 200f / 255f, 255f / 255f);
                TextMeshProUGUI tmp_Text = tf_TemporaryCrown.GetChild(0).GetComponent<TextMeshProUGUI>();
               // tmp_Text.color = new Color(0 / 255f, 0 / 255f, 0 / 255f, 255f / 255f);
               // tmp_Text.fontStyle = FontStyle.Bold;
            }
            else
            {
                tf_TemporaryCrown.GetChild(0).gameObject.SetActive(true);
               // tf_TemporaryCrown.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
                TextMeshProUGUI tmp_Text = tf_TemporaryCrown.GetChild(0).GetComponent<TextMeshProUGUI>();
              //  tmp_Text.color = new Color(0 / 255f, 0 / 255f, 0 / 255f, 255f / 255f);
               // tmp_Text.fontStyle = FontStyle.Normal;
            }
            tf_TemporaryCrown.GetComponent<Button>().enabled = true;
        }
        else
        {
            tf_TemporaryCrown.GetChild(0).gameObject.SetActive(true);
            //tf_TemporaryCrown.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 125f / 255f);
         //   tf_TemporaryCrown.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(0 / 255f, 0 / 255f, 0 / 255f, 125f / 255f);
            tf_TemporaryCrown.GetComponent<Button>().enabled = false;
        }
    }

    // 케이스 입력란을 초기화 해주는 함수.
    // 각 UI에 들어가는 초기 입력값들을 null 혹은 기본이 되는 문구들로 처리하였음.
    public void resetCase_info()
    {
        t_Name.text = "Create or search";
        Name = null;
        t_Patient_Description.text = null;
        Patient_Description = null;
        t_Case_name.text = null;
        Case_name = null;
        t_Client.text = null;
        Client = null;
        t_Due_date.text = "yyyy/MM/dd";
        CalendarScript.resetDate();
        Due_date = null;
        Case_type = 0;
        t_Case_Description.text = null;
        Case_Description = null;
        Patient_CT = null;
        Upper_CT_or_Scan = null;
        Lower_CT_or_Scan = null;
        Extra_CT_or_Scan = null;

        b_Toothsupported = false;
        b_Edentulous = false;
        b_CrownPlacement = false;
        b_SurgicalGuide = false;
        b_TemporaryCrown = false;
        b_isLoadSTL = false;
        b_isLoadDicom = false;


        paths_STL = null;

        if (obj_Parent != null)
        {
            obj_Parent.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            obj_Parent.parent.GetChild(1).gameObject.SetActive(true);
            obj_Parent.GetChild(0).gameObject.SetActive(true);
         //   obj_Parent.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
            obj_Parent = null;
        }


        if (obj_Parent_lower != null)
        {
            obj_Parent_lower.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            obj_Parent_lower.parent.GetChild(1).gameObject.SetActive(true);
            obj_Parent_lower.GetChild(0).gameObject.SetActive(true);
            //obj_Parent_lower.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
            obj_Parent_lower = null;
        }

        if (obj_Parent_upper != null)
        {
            obj_Parent_upper.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            obj_Parent_upper.parent.GetChild(1).gameObject.SetActive(true);
            obj_Parent_upper.GetChild(0).gameObject.SetActive(true);
          //  obj_Parent_upper.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
            obj_Parent_upper = null;
        }

        for (int i = 0; i < AddcaseOption.Count; i++)
        {
            AddcaseOption[i].GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
            TextMeshProUGUI tmp_Text = AddcaseOption[i].GetChild(0).GetComponent<TextMeshProUGUI>();
            tmp_Text.color = new Color(50f / 255f, 50f / 255f, 50f / 255f);
           // tmp_Text.fontStyle = FontStyle.Normal;
        }
        AddcaseOption.Clear();
        possibleSave = false;
        Kit = 0;
    }

    // 케이스의 정보들을 가져오는 함수
    public void getCase_info()
    {
        Name = t_Name.text;
        Patient_Description = t_Patient_Description.text;
        Case_name = t_Case_name.text;
        Client = t_Client.text;
        Due_date = t_Due_date.text;
        Case_type = ui_Case_type.value;
        Case_Description = t_Case_Description.text;
        Kit = ui_kit.value;
    }

    // STL 경로
    string[] paths_STL;
    Transform obj_Parent;

    // DICOM 파일들이 모여있는 폴더 경로 지정, 해당 폴더의 경로를 SystemIOFileLoad라는 함수에 넣어줌으로서 경로에 있는 폴더를 처리하기 전의 일을 하는 함수이다.
    // 매개변수 obj는 불러올 경로를 적어주는 Text UI와, 해당 글씨를 포함하는 배경이 되는 UI의 색을 지정해주는 UI오브젝트들의 부모 오브젝트이다.
    public void Add_DCMFolder_Paths(Transform obj)
    {
        var paths = StandaloneFileBrowser.OpenFolderPanel("Select Folder", "", true);
        Patient_CT = paths;

        // obj_parent는 파일 경로를 나타내는 UI들을 묶어주는 부모 오브젝트.
        obj_Parent = obj.parent;

        if (paths.Length > 0)
        {
            string word = paths[paths.Length - 1].ToString();   // world는 파일까지의 경로를 표현.
            string result = new DirectoryInfo(word).Name;       // result는 경로를 C#의 디렉토리형식으로 처리한 결과값 -> UI에 들어갈 내용.

            obj_Parent.GetChild(1).GetComponent<TextMeshProUGUI>().text = result;
            obj_Parent.GetChild(0).gameObject.SetActive(false);
            obj_Parent.parent.GetChild(1).gameObject.SetActive(false);
            obj_Parent.GetComponent<Image>().color = new Color(66 / 255f, 125 / 255f, 122 / 255f);

            string path = "";
            foreach (var p in paths)
            {
                path += p;
            }
            b_isLoadDicom = true;

            //SystemIOFileLoad(path);
            VolumeObject = Engine.LoadDicom("DICOM", path);
            /*
            QttEngine.CrossSectionObject cs_Up = Engine.CreateCrossSection("CS_Up");
            Engine.SetDicomToCrossSection("CS_Up", "DICOM");
            CrossSectionPlaneGroup(cs_Up.gameObject, new Vector3(-90f, 0f, 0f), "Up");

            QttEngine.CrossSectionObject cs_Down = Engine.CreateCrossSection("CS_Down");
            Engine.SetDicomToCrossSection("CS_Down", "DICOM");
            CrossSectionPlaneGroup(cs_Down.gameObject, new Vector3(90f, 0f, 0f), "Down");

            QttEngine.CrossSectionObject cs_Left = Engine.CreateCrossSection("CS_Left");
            Engine.SetDicomToCrossSection("CS_Left", "DICOM");
            CrossSectionPlaneGroup(cs_Left.gameObject, new Vector3(0f, -90f, 0f), "Left");

            QttEngine.CrossSectionObject cs_Right = Engine.CreateCrossSection("CS_Right");
            Engine.SetDicomToCrossSection("CS_Right", "DICOM");
            CrossSectionPlaneGroup(cs_Right.gameObject, new Vector3(0f, 90f, 0f), "Right");

            QttEngine.CrossSectionObject cs_Front = Engine.CreateCrossSection("CS_Front");
            Engine.SetDicomToCrossSection("CS_Front", "DICOM");
            CrossSectionPlaneGroup(cs_Front.gameObject, new Vector3(0f, 180f, 0f), "Front");

            QttEngine.CrossSectionObject cs_Behind = Engine.CreateCrossSection("CS_Back");
            Engine.SetDicomToCrossSection("CS_Back", "DICOM");
            CrossSectionPlaneGroup(cs_Behind.gameObject, new Vector3(0f, 0f, 0f), "Back");
            */
            Invoke("STLTransform", 0.01f);
            Invoke("DicomTransform", 0.01f);

            possibleSave = true;
        }
    }
    public void Add_DCMFolder_Paths(string _dcmPath)
    {
        b_isLoadDicom = true;

        //SystemIOFileLoad(path);
        VolumeObject = Engine.LoadDicom("DICOM", _dcmPath);
        /*
        QttEngine.CrossSectionObject cs_Up = Engine.CreateCrossSection("CS_Up");
        Engine.SetDicomToCrossSection("CS_Up", "DICOM");
        CrossSectionPlaneGroup(cs_Up.gameObject, new Vector3(-90f, 0f, 0f), "Up");

        QttEngine.CrossSectionObject cs_Down = Engine.CreateCrossSection("CS_Down");
        Engine.SetDicomToCrossSection("CS_Down", "DICOM");
        CrossSectionPlaneGroup(cs_Down.gameObject, new Vector3(90f, 0f, 0f), "Down");

        QttEngine.CrossSectionObject cs_Left = Engine.CreateCrossSection("CS_Left");
        Engine.SetDicomToCrossSection("CS_Left", "DICOM");
        CrossSectionPlaneGroup(cs_Left.gameObject, new Vector3(0f, -90f, 0f), "Left");

        QttEngine.CrossSectionObject cs_Right = Engine.CreateCrossSection("CS_Right");
        Engine.SetDicomToCrossSection("CS_Right", "DICOM");
        CrossSectionPlaneGroup(cs_Right.gameObject, new Vector3(0f, 90f, 0f), "Right");

        QttEngine.CrossSectionObject cs_Front = Engine.CreateCrossSection("CS_Front");
        Engine.SetDicomToCrossSection("CS_Front", "DICOM");
        CrossSectionPlaneGroup(cs_Front.gameObject, new Vector3(0f, 180f, 0f), "Front");

        QttEngine.CrossSectionObject cs_Behind = Engine.CreateCrossSection("CS_Back");
        Engine.SetDicomToCrossSection("CS_Back", "DICOM");
        CrossSectionPlaneGroup(cs_Behind.gameObject, new Vector3(0f, 0f, 0f), "Back");
        */
        Invoke("STLTransform", 0.01f);
        Invoke("DicomTransform", 0.01f);

        possibleSave = true;
    }

    public void CrossSectionPlaneGroup(GameObject obj, Vector3 rot, string str)
    {
        if (!StaticManager.instance.CrossSectionPlane.ContainsKey(str))
        {
            obj.layer = 9;
            obj.transform.GetChild(0).gameObject.layer = 9;
            obj.transform.localRotation = Quaternion.Euler(rot);
            StaticManager.instance.CrossSectionPlane.Add(str, obj);
            StaticManager.instance.CrossSectionVector[0] = obj.transform.localPosition;

            if (str == "Up")
            {
                obj.transform.localPosition = new Vector3(960f, 540f - 0.5f, 0f);
            }

            if (str == "Down")
            {
                obj.transform.localPosition = new Vector3(960f, 540f + 0.5f, 0f);
            }

            if (str == "Front")
            {
                obj.transform.localPosition = new Vector3(960f, 540f, -0.5f);
            }

            if (str == "Back")
            {
                obj.transform.localPosition = new Vector3(960f, 540f, 0.5f);
            }

            if (str == "Left")
            {
                obj.transform.localPosition = new Vector3(960f - 0.5f, 540f, 0f);
            }

            if (str == "Right")
            {
                obj.transform.localPosition = new Vector3(960f + 0.5f, 540f, 0f);
            }
        }
    }

    public void DicomTransform()
    {
        // main cam 위치로 옮겼을 때의 포지션
        //VolumeObject.transform.position = new Vector3(872.3f, 540f, 0f);
        VolumeObject.transform.position = new Vector3(960f, 540f, 0f);
        VolumeObject.transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 180f));
        VolumeObject.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    bool b_Toothsupported = false;
    bool b_Edentulous = false;
    bool b_CrownPlacement = false;
    bool b_SurgicalGuide = false;
    bool b_TemporaryCrown = false;
    List<Transform> AddcaseOption = new List<Transform>();
    public Transform tf_Toothsupported_Button;

    // 디자인 설정에서 주 옵션 혹은 치아 지원 부 옵션과 관련된 버튼 클릭과 관련된 함수
    public void Toothsupported_Button(Transform mytf)
    {
        if (b_Toothsupported)
        {
            Option[0] = false;  // 옵션 체크 여부를 나타내는 변수
            b_Toothsupported = false;
            mytf.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
            TextMeshProUGUI tmp_Text = mytf.GetChild(0).GetComponent<TextMeshProUGUI>();
            tmp_Text.color = new Color(66 / 255f, 125 / 255f, 122 / 255f);
            //  tmp_Text.fontStyle = FontStyle.Normal;
        }
        else
        {
            Option[0] = true;
            b_Toothsupported = true;
            mytf.GetComponent<Image>().color = new Color(66f / 255f, 125f / 255f, 122f / 255f);
            TextMeshProUGUI tmp_Text = mytf.GetChild(0).GetComponent<TextMeshProUGUI>();
            tmp_Text.color = new Color(255 / 255, 255 / 255, 255 / 255);
          //  tmp_Text.fontStyle = FontStyle.Bold;
        }
    }

    // 디자인 설정에서 주 옵션 혹은 치아 지원 부 옵션과 관련된 버튼 클릭과 관련된 함수
    public void Edentulous_Button(Transform mytf)
    {
        if (!AddcaseOption.Contains(mytf))
        {
            AddcaseOption.Add(mytf);
        }
        else
        {
            AddcaseOption.Remove(mytf);
        }

        if (b_Edentulous)
        {
            b_Edentulous = false;
            mytf.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
            TextMeshProUGUI tmp_Text = mytf.GetChild(0).GetComponent<TextMeshProUGUI>();
            tmp_Text.color = new Color(50f / 255f, 50f / 255f, 50f / 255f);
         //   tmp_Text.fontStyle = FontStyle.Normal;
        }
        else
        {
            b_Edentulous = true;
            mytf.GetComponent<Image>().color = new Color(200f / 255f, 200f / 255f, 200f / 255f);
            TextMeshProUGUI tmp_Text = mytf.GetChild(0).GetComponent<TextMeshProUGUI>();
            tmp_Text.color = new Color(0 / 255, 0 / 255, 0 / 255);
          //  tmp_Text.fontStyle = FontStyle.Bold;
        }
    }

    // 디자인 설정에서 주 옵션 혹은 치아 지원 부 옵션과 관련된 버튼 클릭과 관련된 함수
    public void CrownPlacement_Button(Transform mytf)
    {
        if (!AddcaseOption.Contains(mytf))
        {
            AddcaseOption.Add(mytf);
        }
        else
        {
            AddcaseOption.Remove(mytf);
        }

        if (b_CrownPlacement)
        {
            Implant_planning[0] = false;    // 임플란트 플래닝 옵션 체크 여부
            b_CrownPlacement = false;

            mytf.GetChild(0).gameObject.SetActive(true);

           // mytf.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
          //  TextMeshProUGUI tmp_Text = mytf.GetChild(0).GetComponent<TextMeshProUGUI>();
          //  tmp_Text.color = new Color(50f / 255f, 50f / 255f, 50f / 255f);
          //  tmp_Text.fontStyle = FontStyle.Normal;
        }
        else
        {
            Implant_planning[0] = true;
            b_CrownPlacement = true;

            mytf.GetChild(0).gameObject.SetActive(false);

          //  mytf.GetComponent<Image>().color = new Color(200f / 255f, 200f / 255f, 200f / 255f);
          //  TextMeshProUGUI tmp_Text = mytf.GetChild(0).GetComponent<TextMeshProUGUI>();
           // tmp_Text.color = new Color(0 / 255, 0 / 255, 0 / 255);
            //tmp_Text.fontStyle = FontStyle.Bold;
        }
    }

    // 디자인 설정에서 주 옵션 혹은 치아 지원 부 옵션과 관련된 버튼 클릭과 관련된 함수
    public void SurgicalGuide_Button(Transform mytf)
    {
        if (!AddcaseOption.Contains(mytf))
        {
            AddcaseOption.Add(mytf);
        }
        else
        {
            AddcaseOption.Remove(mytf);
        }

        if (b_SurgicalGuide)
        {
            Implant_planning[1] = false;
            b_SurgicalGuide = false;
            mytf.GetChild(0).gameObject.SetActive(true);


           // mytf.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
          //  TextMeshProUGUI tmp_Text = mytf.GetChild(0).GetComponent<TextMeshProUGUI>();
           // tmp_Text.color = new Color(50f / 255f, 50f / 255f, 50f / 255f);
           // tmp_Text.fontStyle = FontStyle.Normal;
        }
        else
        {
            Implant_planning[1] = true;
            b_SurgicalGuide = true;
            mytf.GetChild(0).gameObject.SetActive(false);


          //  mytf.GetComponent<Image>().color = new Color(200f / 255f, 200f / 255f, 200f / 255f);
          //  TextMeshProUGUI tmp_Text = mytf.GetChild(0).GetComponent<TextMeshProUGUI>();
          //  tmp_Text.color = new Color(0 / 255, 0 / 255, 0 / 255);
           // tmp_Text.fontStyle = FontStyle.Bold;
        }
    }

    // 디자인 설정에서 주 옵션 혹은 치아 지원 부 옵션과 관련된 버튼 클릭과 관련된 함수
    public void TemporaryCrown_Button(Transform mytf)
    {
        if (!AddcaseOption.Contains(mytf))
        {
            AddcaseOption.Add(mytf);
        }
        else
        {
            AddcaseOption.Remove(mytf);
        }

        if (b_TemporaryCrown)
        {
            Implant_planning[2] = false;
            b_TemporaryCrown = false;
            mytf.GetChild(0).gameObject.SetActive(false);


          //  mytf.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
          //  TextMeshProUGUI tmp_Text = mytf.GetChild(0).GetComponent<TextMeshProUGUI>();
           // tmp_Text.color = new Color(50f / 255f, 50f / 255f, 50f / 255f);
           // tmp_Text.fontStyle = FontStyle.Normal;
        }
        else
        {
            Implant_planning[2] = true;
            b_TemporaryCrown = true;
            mytf.GetChild(0).gameObject.SetActive(true);


          //  mytf.GetComponent<Image>().color = new Color(200f / 255f, 200f / 255f, 200f / 255f);
          //  TextMeshProUGUI tmp_Text = mytf.GetChild(0).GetComponent<TextMeshProUGUI>();
          // tmp_Text.color = new Color(0 / 255, 0 / 255, 0 / 255);
           // tmp_Text.fontStyle = FontStyle.Bold;
        }
    }

    Transform obj_Parent_upper;
    // STL 중 상악에 해당하는 파일이 있는 폴더의 경로를 설정하는 함수, 해당 폴더의 경로를 CreateSTLObj라는 함수에 넣어줌으로서 경로에 있는 폴더를 처리하기 전의 일을 하는 함수이다.
    // 매개변수 obj는 불러올 경로를 적어주는 Text UI와, 해당 글씨를 포함하는 배경이 되는 UI의 색을 지정해주는 UI오브젝트들의 부모 오브젝트이다.
    public void Add_Upper_STLFile_Paths(Transform obj)
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false);
        paths_STL = paths;

        // obj_parent는 파일 경로를 나타내는 UI들을 묶어주는 부모 오브젝트.
        obj_Parent_upper = obj.parent;

        string path = "";
        foreach (var p in paths)
        {
            path += p;
        }

        string str = "Upper";

        if (path.Contains(str))
        {
            // STL 상악 오브젝트 오픈
            string word = paths[paths.Length - 1].ToString();   // world는 파일까지의 경로를 표현.
            string result = new DirectoryInfo(word).Name;       // result는 경로를 C#의 디렉토리형식으로 처리한 결과값 -> UI에 들어갈 내용.

            obj_Parent_upper.GetChild(1).GetComponent<TextMeshProUGUI>().text = result;
            obj_Parent_upper.GetChild(0).gameObject.SetActive(false);
            obj_Parent_upper.parent.GetChild(1).gameObject.SetActive(false);
            obj_Parent_upper.GetComponent<Image>().color = new Color(66 / 255f, 125 / 255f, 122 / 255f);

            b_isLoadSTL = true;
            CreateSTLObj(path, true);
        }
        else
        {
            // 해당 파일은 상악 STL 파일이 아니므로 로드 실패
            return;
        }
    }


    Transform obj_Parent_lower;
    // STL 중 하악에 해당하는 파일이 있는 폴더의 경로를 설정하는 함수, 해당 폴더의 경로를 CreateSTLObj라는 함수에 넣어줌으로서 경로에 있는 폴더를 처리하기 전의 일을 하는 함수이다.
    // 매개변수 obj는 불러올 경로를 적어주는 Text UI와, 해당 글씨를 포함하는 배경이 되는 UI의 색을 지정해주는 UI오브젝트들의 부모 오브젝트이다.
    public void Add_Lower_STLFile_Paths(Transform obj)
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false);
        paths_STL = paths;

        obj_Parent_lower = obj.parent;

        string path = "";
        foreach (var p in paths)
        {
            path += p;
        }

        string str = "Lower";

        if (path.Contains(str))
        {
            // STL 하악 오브젝트 오픈
            string word = paths[paths.Length - 1].ToString();   // world는 파일까지의 경로를 표현.
            string result = new DirectoryInfo(word).Name;       // result는 경로를 C#의 디렉토리형식으로 처리한 결과값 -> UI에 들어갈 내용.

            obj_Parent_lower.GetChild(1).GetComponent<TextMeshProUGUI>().text = result;
            obj_Parent_lower.GetChild(0).gameObject.SetActive(false);
            obj_Parent_lower.parent.GetChild(1).gameObject.SetActive(false);
            obj_Parent_lower.GetComponent<Image>().color = new Color(66 / 255f, 125 / 255f, 122 / 255f);

            b_isLoadSTL = true;
            CreateSTLObj(path, false);
        }
        else
        {
            // 해당 파일은 하악 STL 파일이 아니므로 로드 실패
            return;
        }

    }

    // path는 불러오고자 하는 파일이 있는 경로이고, isUpper은 상악인지 하악인지 구분시켜주는 매개변수이다.
    // 원하는 STL 파일을 불러와 오브젝트화 시키는 함수이다.
    public void CreateSTLObj(string path, bool isUpper)
    {
        // 각 캔버스별 STL에 해당하는 오브젝트들의 위치 초기화
        STLCanvas_objMeshToExport.transform.position = Vector3.zero;
        STLCanvas_objMeshToExport.transform.rotation = Quaternion.identity;
        CaptureCanvas_objMeshToExport.transform.position = Vector3.zero;
        CaptureCanvas_objMeshToExport.transform.rotation = Quaternion.identity;

        // 이전에 생성되어 있는 STL 오브젝트가 존재한다면, 이를 없애주는 작업.
        // sourceArray는 STL을 오브젝트화 할 때 생긴 오브젝트들의 모임.
        /*
        Transform[] sourceArray = STLCanvas_objMeshToExport.GetComponentsInChildren<Transform>();
        foreach (Transform t in sourceArray)
        {
            if (t.name != STLCanvas_objMeshToExport.name)
                Destroy(t.gameObject);
        }

        Transform[] sourceArray2 = CaptureCanvas_objMeshToExport.GetComponentsInChildren<Transform>();
        foreach (Transform t in sourceArray2)
        {
            if (t.name != CaptureCanvas_objMeshToExport.name)
                Destroy(t.gameObject);
        }*/

        // 사용자가 선택한 STL 파일이 있는 경로인 path로부터 mesh 파일들을 임포트 한 것.
        // 해당 메쉬들로 이루어진 오브젝트들의 모임이 STL 오브젝트.
        /*
        Mesh[] mesh = Importer.Import(path);
        meshs = mesh;
        foreach (Mesh m in meshs)
        {
            GameObject obj = Instantiate(source, Vector3.zero, Quaternion.identity);
            obj.GetComponent<MeshFilter>().mesh = m;

            GameObject obj2 = Instantiate(source, Vector3.zero, Quaternion.identity);
            obj2.GetComponent<MeshFilter>().mesh = m;

            obj.transform.SetParent(STLCanvas_objMeshToExport.transform);
            obj2.transform.SetParent(CaptureCanvas_objMeshToExport.transform);
        }*/

        if (isUpper)
        {
            Upper_STL = Engine.LoadScan("Upper", @path);
            Upper_STL_Capture = Engine.LoadScan("Upper_Capture", @path);
            Upper_STL.transform.SetParent(STLCanvas_objMeshToExport.transform);
            Upper_STL_Capture.transform.SetParent(CaptureCanvas_objMeshToExport.transform);
        }
        else
        {
            Lower_STL = Engine.LoadScan("Lower", @path);
            Lower_STL_Capture = Engine.LoadScan("Lower_Capture", @path);
            Lower_STL.transform.SetParent(STLCanvas_objMeshToExport.transform);
            Lower_STL_Capture.transform.SetParent(CaptureCanvas_objMeshToExport.transform);
        }

        Invoke("STLTransform", 0.001f);
        /*
        if (isUpper)
        {
            Invoke("Upper_STL_Transform", 0.001f);
        }
        else
        {
            Invoke("Lower_STL_Transform", 0.001f);
        }
        */

        possibleSave = true;
    }

    // STL의 상악, 하악에 따른 위치를 지정해주는 함수이다.
    public void STLTransform()
    {
        controller.gameObject.GetComponent<FolderLoader>().STLTransform();
    }


    // STL의 상악의 위치를 지정해주는 함수이나 현재는 사용하고 있지 않다.
    public void Upper_STL_Transform()
    {
        STLCanvas_objMeshToExport.transform.localPosition = Vector3.zero;

        STLCanvas_objMeshToExport.transform.localRotation = Quaternion.Euler(new Vector3(-10f, -90f, 0f));

        STLCanvas_objMeshToExport.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

        for (int i = 0; i < STLCanvas_objMeshToExport.transform.childCount; ++i)
        {
            STLCanvas_objMeshToExport.transform.GetChild(i).transform.localScale = new Vector3(1f, 1f, 1f);
        }

        CameraManager_Scr.MainOBJ_STL = STLCanvas_objMeshToExport;
    }

    // STL의 하악의 위치를 지정해주는 함수이나 현재는 사용하고 있지 않다.
    public void Lower_STL_Transform()
    {
        STLCanvas_objMeshToExport.transform.localPosition = Vector3.zero;

        STLCanvas_objMeshToExport.transform.localRotation = Quaternion.Euler(new Vector3(-10f, -90f, 40f));

        STLCanvas_objMeshToExport.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

        for (int i = 0; i < STLCanvas_objMeshToExport.transform.childCount; ++i)
        {
            STLCanvas_objMeshToExport.transform.GetChild(i).transform.localScale = new Vector3(1f, 1f, 1f);
        }

        CameraManager_Scr.MainOBJ_STL = STLCanvas_objMeshToExport;
    }

    // path는 불러오고자 하는 DICOM 파일들이 모여있는 폴더를 지정해주는 경로이다.
    // 원하는 DICOM 파일들을 불러와 Convert() 함수를 통하여 Textrue 2D로 변형 후 Texture 3D로 합쳐주기 전의 함수이다.
    public void SystemIOFileLoad(string path)
    {
        converter.texture2DArray.Clear();
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var info = new DirectoryInfo(path); // 파일이 있는 경로의 디렉토리 정보를 가져오는 변수.
        var fileInfo = info.GetFiles();     // 디렉토리 경로로부터 존재하는 파일들을 모두 가져오는 변수.
        foreach (var file in fileInfo)
        {
            if (file.Extension == ".dcm")
            {
                DicomFile dicomFile = DicomFile.Open(file.FullName);    // DICOM 파일 형식을 가져오는 변수.
                DicomImage image = new DicomImage(dicomFile.Dataset);   // DICOM 파일을 이미지화 하는 변수.
                string test = image.RenderImage().ToString();
                string s = file.DirectoryName;
                var tex2d = image.RenderImage().As<Texture2D>();    // DICOM 파일의 이미지를 Texture 2D화 시킨 변수.
                converter.texture2DArray.Add(tex2d);

            }
        }

        Convert(path);
    }

    // DICOM 파일들을 Texture2D로 변경하고, 변경한 파일들을 Texture3D로 합치는 작업을 한 후 해당 Texture3D를 VolumeRendering화 하는 작업을 한다.
    public void Convert(string path)
    {
        var tex2dArray = converter.texture2DArray;  // DICOM 파일의 이미지를 Texture 2D로 변형시킨 정렬.
        tex2dArray.Reverse();   // 폴더의 처음 파일이 제일 나중에 출력되기에 이를 반대로 수정.
        if (tex2dArray.Count == 0)
        {
            error = "no image";
        }

        var w = tex2dArray[0].width;
        var h = tex2dArray[0].height;
        var d = tex2dArray.Count;
        var format = tex2dArray[0].format;
        var colors = new UnityEngine.Color32[w * h * d];

        for (int i = 0; i < d; ++i)
        {
            var tex2d = tex2dArray[i];
            if (tex2d.width != w || tex2d.height != h)
            {
                error = "texture size error";
            }
            if (tex2d.format != format)
            {
                error = "texture format error";
            }

            tex2d.GetPixels32().CopyTo(colors, w * h * i);
        }

        var tex3d = new Texture3D(w, h, d, format, false);
        tex3d.SetPixels32(colors);
        tex3d.Apply();

        controller.ChangeTexture(tex3d);

        if (!saved_Volume.ContainsKey(path))
        {
            saved_Volume.Add(path, tex3d);
        }

        //   string Data = JsonConvert.SerializeObject(saved_Volume);

        //    File.WriteAllText(Application.persistentDataPath + "/PYLON_SaveCase3DVolume.json", Data);

        //    saved_VolumePath.Add(path);

        //   string Data2 = JsonConvert.SerializeObject(saved_VolumePath);

        //   File.WriteAllText(Application.persistentDataPath + "/PYLON_SaveCase3DVolumePath.json", Data2);

        //controller_Caselist.ChangeTexture(tex3d);
    }

    // 저장된 케이스의 3D Volume을 미리 로드시키는 곳
    public void SystemIOFileLoad_SaveFile(string path)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var info = new DirectoryInfo(path);
        var fileInfo = info.GetFiles();
        foreach (var file in fileInfo)
        {
            if (file.Extension == ".dcm")
            {
                DicomFile dicomFile = DicomFile.Open(file.FullName);
                DicomImage image = new DicomImage(dicomFile.Dataset);
                string test = image.RenderImage().ToString();
                string s = file.DirectoryName;
                var tex2d = image.RenderImage().As<Texture2D>();
                converter.texture2DArray.Add(tex2d);

            }
        }
        var tex2dArray = converter.texture2DArray;
        tex2dArray.Reverse();
        if (tex2dArray.Count == 0)
        {
            error = "no image";
        }

        var w = tex2dArray[0].width;
        var h = tex2dArray[0].height;
        var d = tex2dArray.Count;
        var format = tex2dArray[0].format;
        var colors = new UnityEngine.Color32[w * h * d];

        for (int i = 0; i < d; ++i)
        {
            var tex2d = tex2dArray[i];
            if (tex2d.width != w || tex2d.height != h)
            {
                error = "texture size error";
            }
            if (tex2d.format != format)
            {
                error = "texture format error";
            }
            tex2d.GetPixels32().CopyTo(colors, w * h * i);
        }

        var tex3d = new Texture3D(w, h, d, format, false);
        tex3d.SetPixels32(colors);
        tex3d.Apply();

        controller.ChangeTexture(tex3d);


        if (!saved_Volume.ContainsKey(path))
        {
            saved_Volume.Add(path, tex3d);
        }

        string Data = JsonConvert.SerializeObject(saved_Volume);

        File.WriteAllText(Application.persistentDataPath + "/PYLON_SaveCase3DVolume.json", Data);
        //controller_Caselist.ChangeTexture(tex3d);
    }
}
