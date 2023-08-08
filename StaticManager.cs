using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityVolumeRendering;

public class StaticManager : MonoBehaviour
{
    private static StaticManager Instance;

    public static StaticManager instance { get => Instance; }

    [Header("볼륨렌더링 오브젝트")]
    [SerializeField]
    private VolumeRenderedObject _caseListStepObj;
    [SerializeField]
    private VolumeRenderedObject _workStepObj;
    [SerializeField]
    private VolumeRenderedObject _sliceStepObj;
    [SerializeField]
    private VolumeRenderedObject _subDataStepObj;
    [SerializeField]
    private GameObject _STLObj;

    [Header("캔버스")]
    // 각각의 스텝에 맞는 캔버스들의 모임
    public GameObject Setting_Canvas;
    public GameObject CaseList_Canvas;
    public GameObject AddCase_Canvas;
    public GameObject WorkStep_Canvas;
    public GameObject Slice_Canvas;
    public GameObject SubData_Canvas;
    public GameObject STL_Canvas;
    public GameObject Capture_Canvas;
    public GameObject CaseManagement_Canvas;

    // 다이얼로그 캔버스에 있는 각각의 다이얼로그들.
    [Header("다이얼로그")]
    [SerializeField]
    private GameObject _removeCapture;

    // 캡쳐시 나타나는 뒷 배경과, 캡쳐 항목이 담겨있는 리스트
    [Header("캡쳐")]
    [SerializeField]
    private Transform _captureList;
    // 캡쳐버튼 클릭 시 나타나는 윈도우창
    public GameObject[] captureWindow;

    // 썸네일 사진, 썸네일이 없을 시 나타나는 텍스트
    [Header("썸네일")]
    [SerializeField]
    private Image _thumbnailImage;
    [SerializeField]
    private TextMeshProUGUI _thumbnailText;

    // 프리셋에 있는 트리의 부모 오브젝트.
    [Header("프리셋")]
    [SerializeField]
    private MenuTreeParent[] _presetTreeParents;
    [SerializeField]
    private GameObject _workCanvasOTF;
    [SerializeField]
    private GameObject _subDataCanvasOTF;
    [SerializeField]
    private GameObject _stlCanvasOTF;
    [SerializeField]
    private RuntimeTransferFunctionEditor[] _runtimeOTF;

    [Header("데이터박스")]
    public GameObject AddCase_Databox_InsideBox;    // 데이터 박스에서 쓰이는 UI.
    public GameObject AddCase_Databox_ScrollView;   // 데이터 박스에 들어가는 파일 UI들을 묶어주는 부모 오브젝트.

    [Header("버튼")]
    // 각 버튼들의 모임
    public GameObject[] SettingGeneralButton = new GameObject[6];
    public GameObject[] SettingTemporarycrownButton = new GameObject[5];
    [SerializeField]
    private GameObject[] _caseDetailButton;


    [Header("스크립트")]
    // 스크립트들의 모임
    public AddCaseManager scr_AddCaseManager;
    public CaseListManager scr_CaseListManager;
    public ButtonManager scr_ButtonManager;
    public CaseManagementManager scr_CaseManagementManager;
    public SettingsManager scr_SettingsManager;
    public DrawCaptureArea[] scr_DrawCaptureArea;
    public ScreenCapture scr_ScreenCapture;
    public PreSetManager_Scr scr_PreSet;
    public WorkTools_Scr scr_WorkTools;
    public DialogManager_Scr scr_Dialog;
    public UnityEngine.UI.TableUI.TableUI scr_TableUI;

    [Header("기타")]
    public Sprite[] DataBox_Icons;
    public Transform[] tf_WorkStep_Canvas_2ndDataArea;
    public bool[] b_WorkStep_Canvas_2ndDataArea;
    private Dictionary<string, GameObject> _crossSectionPlane = new Dictionary<string, GameObject>();
    private Vector3[] _crossSectionVector = new Vector3[6];

    // 각 캔버스나 스크립트, 아이콘 이미지 등등 여러 설정값을 담고있는 함수.

    private void Awake()
    {
        if (instance == null)
        {
            Instance = this;
            scr_SettingsManager.delayStart();
            scr_CaseListManager.delayStart2();
            StartCoroutine("ScrennCoroutine");
        }
        else
        {
            return;
        }
    }

    IEnumerator ScrennCoroutine()
    {
        while (true)
        {
            if(STL_Canvas.activeSelf || Slice_Canvas.activeSelf)
            {
                Screen.SetResolution(1920, 1040, true);
            }
            else
            {
                Screen.SetResolution(1920, 1080, true);
            }

            yield return CashingCoroutine_Scr.WaitForFixedUpdate;
        }
    }

    public Dictionary<string, GameObject> CrossSectionPlane
    {
        set => _crossSectionPlane = value;
        get => _crossSectionPlane;
    }

    public Vector3[] CrossSectionVector
    {
        set => _crossSectionVector = value;
        get => _crossSectionVector;
    }
    
    public void BG_1040On(CanvasScaler scaler)
    {
        scaler.referenceResolution = new Vector2(1920f, 1040f);
    }

    public void BG_1080On(CanvasScaler scaler)
    {
        scaler.referenceResolution = new Vector2(1920f, 1080f);
    }

    public RuntimeTransferFunctionEditor[] RuntimeOTF
    {
        get => _runtimeOTF;
    }

    public VolumeRenderedObject CaseListStepVolumeObj
    {
        get => _workStepObj;
    }
    public VolumeRenderedObject WorkStepVolumeObj
    {
        get => _workStepObj;
    }
    public VolumeRenderedObject SliceStepVolumeObj
    {
        get => _sliceStepObj;
    }
    public VolumeRenderedObject SubDataStepVolumeObj
    {
        get => _subDataStepObj;
    }
    public GameObject STLObj
    {
        get => _STLObj;
    }

    public Transform CaptureList
    {
        get => _captureList;
    }

    public Image ThumbnailImage
    {
        get => _thumbnailImage;
    }

    public TextMeshProUGUI ThumbnailText
    {
        get => _thumbnailText;
    }

    public GameObject RemoveCapture
    {
        get => _removeCapture;
    }

    public MenuTreeParent[] TreeParents
    {
        get => _presetTreeParents;
    }

    public GameObject[] CaseDetailButton
    {
        get => _caseDetailButton;
    }

    public GameObject WorkCanvasOTF
    {
        get => _workCanvasOTF;
    }

    public GameObject SubDataCanvasOTF
    {
        get => _subDataCanvasOTF;
    }

    public GameObject STLCanvasOTF
    {
        get => _stlCanvasOTF;
    }
}
