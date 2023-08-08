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

    // ���콺 ��Ŭ������ ���� ���ϴ� ���������� �ܸ��� ���鵵�� ���� �Լ�.
    // ù ��° Ŭ�������������� ���� ���� �����, �� ��° Ŭ�� ������ ���� ���� ������ ���� �ܸ��� ���������ش�.
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
                    // dis_cmVer�� ����ڰ� ���� ���� cm�� ȯ���� ����.
                    dis_cmVer = Mathf.Sqrt(Mathf.Pow(endPos.x - startPos.x, 2) + Mathf.Pow(endPos.y - startPos.y, 2) + Mathf.Pow(endPos.z - startPos.z, 2)) * 0.01f;

                    v_AngleVec = endPos - startPos; // v_AngleVec�� �ܸ��� ������ �� ����ڰ� ���� ���� ������ ������.
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


    // ���콺 ��Ŭ���� ó�� ���� ������ ���� �����̰�, �ش� �������κ��� ���� �����ϴ� �Լ��̴�.
    void CreateLine()
    {
        line = new GameObject("Line").AddComponent<LineRenderer>().GetComponent<LineRenderer>();
        line.transform.parent = g_LineParent.transform; // ���� �׸� ��, �ش� ������ �����ִ� ����. ����� �ʿ䰡 ����.
        line.material = Resources.Load("Test") as Material;
        line.material.SetColor("_Color", Color.red);
        line.startWidth = 0.2f;
        line.endWidth = 0.2f;
        line.useWorldSpace = true;
        line.GetComponent<LineRenderer>().generateLightingData = true;
    }

    // ���� �׸� �� �ִ� ������ �ִ��� üũ�ϴ� �Լ�.
    public void MouseOnLineRendererSpace()
    {
        inLineRendererSpace = true;
    }

    // ���� �׸� �� �ִ� ������ �ִ��� üũ�ϴ� �Լ�.
    public void MouseLeaveLineRendererSpace()
    {
        inLineRendererSpace = false;
    }

    // ���콺�� UI���� �ִ� �� üũ�ϴ� �Լ�.
    public void MouseOnUI()
    {
        isOnUI = true;
    }

    // ���콺�� UI���� �ִ� �� üũ�ϴ� �Լ�.
    public void MouseLeaveUI()
    {
        if (line != null)
            line = null;
        isOnUI = false;
    }

    // ������ Volume Rendering ������Ʈ�� STL ������Ʈ�� ȸ���̳� �̵��� �ʱ�ȭ�����ִ� �Լ�.
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

    // �� ����� ��带 �������ִ� �Լ�. �ش� ����� ���� ���� �׷� �ܸ��� �߰��� �� �ִ�.
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

    // DICOM ����(Volume Rendering ������Ʈ)�� ȸ����ų �� ���θ� �������ִ� �Լ�.
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

    // STL ������ ȸ����ų �� ���θ� �������ִ� �Լ�.
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

    // DICOM ����(Volume Rendering ������Ʈ)�� �̵���ų �� ���θ� �������ִ� �Լ�.
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

    // STL ������ �̵���ų �� ���θ� �������ִ� �Լ�.
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

    // FunctionBar UI�� ���콺�� �ִ��� üũ�ϴ� �Լ�.
    public void ONFunctionbar()
    {
        isONFunctionBar = true;
    }

    // FunctionBar UI�� ���콺�� �ִ��� üũ�ϴ� �Լ�.
    public void OutFunctionbar()
    {
        isONFunctionBar = false;
    }
}