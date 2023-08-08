using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinerendererTest : MonoBehaviour
{
    private static LinerendererTest Instance;
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private LineRenderer line;
    [SerializeField]
    private Vector3 mousePos;
    [SerializeField]
    private Vector3 startPos;
    [SerializeField]
    private Vector3 endPos;
    [SerializeField]
    private GameObject g_LineParent;
    [SerializeField]
    private Text t_LineONOFF;
    [SerializeField]
    private Text t_RotateDCMOFF;
    [SerializeField]
    private Text t_RotateSTLONOFF;
    [SerializeField]
    private Text t_MoveDCMONOFF;
    [SerializeField]
    private Text t_MoveSTLONOFF;

    [SerializeField]
    private Vector3 v_AngleVec;

    private float dis_cmVer;
    public bool isLineMode = true;
    public bool b_isCreated;
    //public GameObject g_TouchArea;
    public bool isOnUI;

    public static bool isRotateFile_dcm;
    public static bool isRotateFile_stl;
    public static bool isMoveFile_dcm;
    public static bool isMoveFile_stl;
    public static bool isONFunctionBar;
    public bool inLineRendererSpace = false;

    public static LinerendererTest instance { get => Instance; }

    private void Start()
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

    // 마우스 좌클릭으로 내가 원하는 방향으로의 단면을 만들도록 돕는 함수.
    // 첫 번째 클릭지점에서부터 빨간 선이 생기고, 두 번째 클릭 지점에 찍힌 선의 각도에 따른 단면을 생성시켜준다.
    void Update()
    {
        if (!isOnUI && isLineMode && !MoveFunctionBar.onDragFunctionBar && inLineRendererSpace)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!b_isCreated || (b_isCreated && Input.mousePosition.y >= 400f))
                {
                    if (line == null)
                        CreateLine();

                    mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20);
                    mousePos = camera.ScreenToWorldPoint(mousePos);
                    line.SetPosition(0, mousePos);
                    line.SetPosition(1, mousePos);
                    // startPos = mousePos;

                    RaycastHit hit;
                    int layerMask = 1 << LayerMask.NameToLayer("MoveAreaPlane");

                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask))
                    {
                        startPos = hit.point;
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0) && line)
            {
                if (line)
                {
                    mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20);
                    mousePos = camera.ScreenToWorldPoint(mousePos);
                    line.SetPosition(1, mousePos);
                    // endPos = mousePos;

                    RaycastHit hit;
                    int layerMask = 1 << LayerMask.NameToLayer("MoveAreaPlane");

                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask))
                    {

                        endPos = hit.point;
                    }
                    // dis_cmVer는 사용자가 만든 선을 cm로 환산한 변수.
                    dis_cmVer = Mathf.Sqrt(Mathf.Pow(endPos.x - startPos.x, 2) + Mathf.Pow(endPos.y - startPos.y, 2) + Mathf.Pow(endPos.z - startPos.z, 2)) * 0.01f;

                    v_AngleVec = endPos - startPos; // v_AngleVec는 단면을 생성할 때 사용자가 만든 선의 방향대로 생성함.
                    TestController.instance.CreateSlicingPlane(v_AngleVec);
                    Destroy(line);
                    line = null;
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (line)
                {
                    mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20);
                    mousePos = camera.ScreenToWorldPoint(mousePos);
                    line.SetPosition(1, mousePos);
                }
            }
        }
    }


    // 마우스 좌클릭을 처음 누른 지점이 시작 지점이고, 해당 지점으로부터 선을 생성하는 함수이다.
    void CreateLine()
    {
        line = new GameObject("Line").AddComponent<LineRenderer>().GetComponent<LineRenderer>();
        line.transform.parent = g_LineParent.transform; // 선이 그릴 때, 해당 선들을 묶어주는 변수. 현재는 필요가 없음.
        line.material = Resources.Load("Test") as Material;
        line.material.SetColor("_Color", Color.red);
        line.startWidth = 0.2f;
        line.endWidth = 0.2f;
        line.useWorldSpace = true;
        line.GetComponent<LineRenderer>().generateLightingData = true;
    }

    // 선을 그릴 수 있는 영역에 있는지 체크하는 함수.
    public void MouseOnLineRendererSpace()
    {
        inLineRendererSpace = true;
    }

    // 선을 그릴 수 있는 영역에 있는지 체크하는 함수.
    public void MouseLeaveLineRendererSpace()
    {
        inLineRendererSpace = false;
    }

    // 마우스가 UI위에 있는 지 체크하는 함수.
    public void MouseOnUI()
    {
        isOnUI = true;
    }

    // 마우스가 UI위에 있는 지 체크하는 함수.
    public void MouseLeaveUI()
    {
        if (line != null)
            line = null;
        isOnUI = false;
    }

    // 생성한 Volume Rendering 오브젝트나 STL 오브젝트의 회전이나 이동을 초기화시켜주는 함수.
    public void EveryModeOff()
    {
        CameraManager_Scr.MainOBJ_DCM.transform.GetComponent<FollowMouse>().isMove = false;
        if (CameraManager_Scr.MainOBJ_STL != null)
        {
            CameraManager_Scr.MainOBJ_STL.transform.GetComponent<FollowMouse>().isMove = false;
        }

        t_RotateDCMOFF.text = "dcm" + "\n" + "Rotate Off";
        isRotateFile_dcm = false;
        t_RotateSTLONOFF.text = "STL" + "\n" + "Rotate Off";
        isRotateFile_stl = false;
        t_MoveDCMONOFF.text = "dcm" + "\n" + "Move Off";
        isMoveFile_dcm = false;
        t_MoveSTLONOFF.text = "STL" + "\n" + "Move Off";
        isMoveFile_stl = false;
        /*
        t_LineONOFF.text = "Line OFF";
        isLineMode = false;
        */

        t_LineONOFF.text = "Line On";
        isLineMode = true;
    }

    // 선 만들기 모드를 설정해주는 함수. 해당 모드일 때에 선을 그려 단면을 추가할 수 있다.
    public void OnClickCreateLineMode()
    {
        if (isLineMode)
        {
            t_LineONOFF.text = "Line OFF";
            isLineMode = false;
            if (g_LineParent.transform.childCount > 0)
            {
                for (int i = 0; i < g_LineParent.transform.childCount; ++i)
                {
                    Destroy(g_LineParent.transform.GetChild(0).gameObject);
                }
            }
        }
        else
        {
            t_LineONOFF.text = "Line On";
            isLineMode = true;

            t_RotateDCMOFF.text = "dcm" + "\n" + "Rotate Off";
            isRotateFile_dcm = false;
            t_RotateSTLONOFF.text = "STL" + "\n" + "Rotate Off";
            isRotateFile_stl = false;
            t_MoveDCMONOFF.text = "dcm" + "\n" + "Move Off";
            isMoveFile_dcm = false;
            t_MoveSTLONOFF.text = "STL" + "\n" + "Move Off";
            isMoveFile_stl = false;
        }
    }

    // DICOM 파일(Volume Rendering 오브젝트)을 회전시킬 지 여부를 결정해주는 함수.
    public void RotateFile_dcm()
    {
        if (isRotateFile_dcm)
        {
            t_RotateDCMOFF.text = "dcm" + "\n" + "Rotate Off";
            isRotateFile_dcm = false;
        }
        else
        {
            t_RotateDCMOFF.text = "dcm" + "\n" + "Rotate On";
            isRotateFile_dcm = true;

            t_LineONOFF.text = "Line OFF";
            isLineMode = false;
            t_RotateSTLONOFF.text = "STL" + "\n" + "Rotate Off";
            isRotateFile_stl = false;
        }

    }

    // STL 파일을 회전시킬 지 여부를 결정해주는 함수.
    public void RotateFile_stl()
    {
        if (isRotateFile_stl)
        {
            t_RotateSTLONOFF.text = "STL" + "\n" + "Rotate Off";
            isRotateFile_stl = false;
        }
        else
        {
            t_RotateSTLONOFF.text = "STL" + "\n" + "Rotate On";
            isRotateFile_stl = true;

            t_LineONOFF.text = "Line OFF";
            isLineMode = false;
            t_RotateDCMOFF.text = "dcm" + "\n" + "Rotate Off";
            isRotateFile_dcm = false;
        }
    }

    // DICOM 파일(Volume Rendering 오브젝트)을 이동시킬 지 여부를 결정해주는 함수.
    public void MoveFile_dcm()
    {
        if (isMoveFile_dcm)
        {
            CameraManager_Scr.MainOBJ_DCM.transform.GetComponent<FollowMouse>().isMove = false;
            t_MoveDCMONOFF.text = "dcm" + "\n" + "Move Off";
            isMoveFile_dcm = false;
        }
        else
        {
            CameraManager_Scr.MainOBJ_DCM.transform.GetComponent<FollowMouse>().isMove = true;
            t_MoveDCMONOFF.text = "dcm" + "\n" + "Move On";
            isMoveFile_dcm = true;

            if (CameraManager_Scr.MainOBJ_STL != null)
            {
                CameraManager_Scr.MainOBJ_STL.transform.GetComponent<FollowMouse>().isMove = false;
            }

            t_LineONOFF.text = "Line OFF";
            isLineMode = false;
            t_MoveSTLONOFF.text = "STL" + "\n" + "Move Off";
            isMoveFile_stl = false;
        }
    }

    // STL 파일을 이동시킬 지 여부를 결정해주는 함수.
    public void MoveFile_stl()
    {
        if (isMoveFile_stl)
        {
            if (CameraManager_Scr.MainOBJ_STL != null)
            {
                CameraManager_Scr.MainOBJ_STL.transform.GetComponent<FollowMouse>().isMove = false;
            }
            t_MoveSTLONOFF.text = "STL" + "\n" + "Move Off";
            isMoveFile_stl = false;
        }
        else
        {
            if (CameraManager_Scr.MainOBJ_STL != null)
            {
                CameraManager_Scr.MainOBJ_STL.transform.GetComponent<FollowMouse>().isMove = true;
            }
            t_MoveSTLONOFF.text = "STL" + "\n" + "Move On";
            isMoveFile_stl = true;

            CameraManager_Scr.MainOBJ_DCM.transform.GetComponent<FollowMouse>().isMove = false;
            t_MoveDCMONOFF.text = "dcm" + "\n" + "Move Off";
            isMoveFile_dcm = false;
            t_LineONOFF.text = "Line OFF";
            isLineMode = false;
        }
    }

    // FunctionBar UI에 마우스가 있는지 체크하는 함수.
    public void ONFunctionbar()
    {
        isONFunctionBar = true;
    }

    // FunctionBar UI에 마우스가 있는지 체크하는 함수.
    public void OutFunctionbar()
    {
        isONFunctionBar = false;
    }
}