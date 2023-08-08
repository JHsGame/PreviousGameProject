using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityVolumeRendering;

public class CameraManager_Scr : MonoBehaviour
{
    private static CameraManager_Scr Instance;

    public GameObject sliceRenderingPlane;

    //private GraphicRaycaster gr;

    private float _scrollSpeed = 5.0f;
    private float _volumeObjZ;

    public Vector2 turn;
    public Vector2 mainXY;
    public Vector2 subXY;
    public Vector2 mainWH;
    public Vector2 subWH;

    public Camera mainCam;
    public Camera objCam;
    public Camera uiCam;
    public Camera captureCam;

    public List<Camera> subCam;
    public List<Camera> subUICam;

    public bool b_isRemoving = false;

    public GameObject CameraGroup;
    //public GameObject CaseList_CameraGroup;
    //public GameObject WorkStep_CameraGroup;
    //public GameObject SubData_CameraGroup;
    //public GameObject Slice_CameraGroup;
    //public GameObject STL_CameraGroup;

    public GameObject MainOBJ_DCM_;
    public GameObject Capture_SlicingPlane;
    public GameObject Capture_PlaneUIPos;
    public static GameObject MainOBJ_DCM;
    public static GameObject MainOBJ_STL;

    public static CameraManager_Scr instance { get => Instance; }

    private void Start()
    {
        if (Instance != null)
        {
            return;
        }
        else
        {
            //gr = GetComponent<GraphicRaycaster>();
            // MainOBJ_DCM -> 3D Volume을 보여주는 카메라 
            MainOBJ_DCM = MainOBJ_DCM_;
            Instance = this;
            mainCam = Camera.main;

            _volumeObjZ = 0f;
        }
    }

    // 각종 키입력 혹은 마우스 입력에 따른 처리를 해주는 함수.
    private void Update()
    {
        // 워크스텝 캔버스에서 캡쳐 캔버스를 보여주기 위한 키입력 이벤트.
        if (StaticManager.instance != null)
        {
            if (StaticManager.instance.Slice_Canvas.activeSelf && Input.GetKeyDown(KeyCode.F1))
            {
                //StaticManager.instance.BG.transform.parent.GetComponent<Canvas>().worldCamera = CameraManager_Scr.instance.WorkStep_CameraGroup.transform.GetChild(2).GetComponent<Camera>();
                StaticManager.instance.WorkStep_Canvas.SetActive(true);
                StaticManager.instance.scr_PreSet.CTOTF = StaticManager.instance.WorkCanvasOTF;
                StaticManager.instance.Slice_Canvas.SetActive(false);
                StaticManager.instance.BG_1080On(StaticManager.instance.WorkStep_Canvas.GetComponent<CanvasScaler>());
            }

            if (StaticManager.instance.WorkStep_Canvas.activeSelf && Input.GetKeyDown(KeyCode.F2))
            {
                StaticManager.instance.WorkStep_Canvas.SetActive(false);
                // 수정할거
                //TestController.instance.myVolumeObj = StaticManager.instance.SliceStepVolumeObj;
                StaticManager.instance.Slice_Canvas.SetActive(true);
                StaticManager.instance.BG_1040On(StaticManager.instance.Slice_Canvas.GetComponent<CanvasScaler>());
                //StaticManager.instance.BG.transform.parent.GetComponent<Canvas>().worldCamera = CameraManager_Scr.instance.Slice_CameraGroup.transform.GetChild(2).GetComponent<Camera>();
            }

            // 캡쳐 캔버스에서 마우스 우클릭 상태로 좌우로 움직이면, 단면이 움직이도록 설정.
            if (StaticManager.instance.SubData_Canvas.activeSelf)
            {
                if (Input.GetMouseButton(1))
                {
                    turn.x = Input.GetAxis("Mouse X") * 0.08f;

                    Vector3 movePlane = new Vector3(0, turn.x, 0);
                    Vector3 moveUI = new Vector3(turn.x, 0, 0);
                    Capture_SlicingPlane.transform.Translate(movePlane);
                    Capture_PlaneUIPos.transform.Translate(moveUI);
                }
            }


            // 씬을 다시 시작하는 임시용 이벤트 함수.
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SceneManager.LoadScene(0);
            }

            // 볼륨렌더링 오브젝트를 움직이게 하는 코드
            if (!StaticManager.instance.scr_DrawCaptureArea[0].captureMode && !StaticManager.instance.scr_DrawCaptureArea[1].captureMode && !StaticManager.instance.scr_DrawCaptureArea[2].captureMode)
            {
                if (AddCaseManager.instance.VolumeObj != null && Input.GetMouseButton(0))
                {
                    if (StaticManager.instance.WorkStep_Canvas.activeSelf || StaticManager.instance.SubData_Canvas.activeSelf)
                    {
                        int layerMask = 1 << LayerMask.NameToLayer("MoveAreaPlane");
                        /*var ped = new PointerEventData(null);
                        ped.position = Input.mousePosition;
                        List<RaycastResult> results = new List<RaycastResult>();
                        gr.Raycast(ped, results);

                        if(results.Count > 0)
                        {
                            for(int i = 0; i < results.Count; ++i)
                            {
                                if(results[i].gameObject.layer == layerMask)
                                {
                                    AddCaseManager.instance.VolumeObj.transform.position = new Vector3(results[i].gameObject.transform.position.x, results[i].gameObject.transform.position.y, _volumeObjZ);
                                }
                            }
                        }
                        */
                        RaycastHit hit;

                        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask))
                        {

                            AddCaseManager.instance.VolumeObj.transform.position = new Vector3(hit.point.x, hit.point.y, _volumeObjZ);
                        }

                    }
                    /*GameObject UpVector = TestController.instance.myVolumeObj.gameObject;

                    if (Input.GetMouseButton(2))
                    {
                        turn.x = Input.GetAxis("Mouse X") * 2f;
                        turn.y = Input.GetAxis("Mouse Y") * 2f;
                        //TestController.instance.myVolumeObj.transform.Rotate(Vector3.down, turn.x);
                        //TestController.instance.myVolumeObj.transform.Rotate(Vector3.right, turn.y);

                        TestController.instance.myVolumeObj.GetComponent<Rigidbody>().MovePosition(UpVector.transform.position + UpVector.transform.up * 2 * Time.deltaTime);
                    }

                    if (Input.GetKey(KeyCode.W))
                    {
                        TestController.instance.myVolumeObj.GetComponent<Rigidbody>().MovePosition(UpVector.transform.position + UpVector.transform.up * 2 * Time.deltaTime);
                    }

                    if (Input.GetKey(KeyCode.S))
                    {
                        TestController.instance.myVolumeObj.GetComponent<Rigidbody>().MovePosition(UpVector.transform.position - UpVector.transform.up * 2 * Time.deltaTime);
                    }*/

                }
            }

            if(AddCaseManager.instance.VolumeObj != null)
            {
                float scroll = Input.GetAxis("Mouse ScrollWheel") * _scrollSpeed;
                // 오브젝트 카메라들의 Z축 포지션의 값을 조정하여 확대 & 축소를 조절해줍시다.
                
                CameraGroup.transform.Translate(Vector3.forward * scroll);
                //Slice_CameraGroup.transform.Translate(Vector3.forward * scroll);
                //STL_CameraGroup.transform.Translate(Vector3.forward * scroll);
                //SubData_CameraGroup.transform.Translate(Vector3.forward * scroll);
                //WorkStep_CameraGroup.transform.Translate(Vector3.forward * scroll);
                //AddCaseManager.instance.VolumeObj.transform.Translate(Vector3.up * scroll);
            }

            // DICOM 파일 혹은 STL 파일의 회전에 쓰이는 함수
            if (Input.GetMouseButton(1) && AddCaseManager.instance.VolumeObj != null)
            {

                turn.x = Input.GetAxis("Mouse X") * 2f;
                turn.y = Input.GetAxis("Mouse Y") * 2f;

                AddCaseManager.instance.VolumeObj.transform.Rotate(0f, -Input.GetAxis("Mouse X") * 2f, 0f, Space.World);
                AddCaseManager.instance.VolumeObj.transform.Rotate(-Input.GetAxis("Mouse Y") * 2f, 0f, 0f);

                /*
                if (LinerendererTest.isRotateFile_dcm)
                {
                    //  MainOBJ_DCM.transform.Rotate(Vector3.down, turn.x);
                    // MainOBJ_DCM.transform.Rotate(Vector3.right, turn.y);
                }
                if (MainOBJ_STL != null && LinerendererTest.isRotateFile_stl)
                {
                    TestController.instance.myVolumeObj.transform.Rotate(0f, -Input.GetAxis("Mouse X") * 2f, 0f, Space.World);
                    TestController.instance.myVolumeObj.transform.Rotate(Input.GetAxis("Mouse Y") * 2f, 0f, 0f);
                    //  MainOBJ_STL.transform.Rotate(Vector3.down, turn.x);
                    //  MainOBJ_STL.transform.Rotate(Vector3.right, turn.y);
                }
                */

            }
        }
    }

    // 캔버스를 변경해줌에 따라 각 캔버스를 비춰주는 카메라 그룹을 세팅해주는 함수.
    public void ChangeCameraGroup(GameObject ToCanvas)
    {
        if (ToCanvas.transform.childCount > 2)
        {
            mainCam = ToCanvas.transform.GetChild(0).GetComponent<Camera>();
            objCam = ToCanvas.transform.GetChild(1).GetComponent<Camera>();
            uiCam = ToCanvas.transform.GetChild(2).GetComponent<Camera>();
        }
        else
        {
            mainCam = ToCanvas.transform.GetChild(0).GetComponent<Camera>();
            uiCam = ToCanvas.transform.GetChild(1).GetComponent<Camera>();
        }
    }

    // 단면을 추가함에 따라 생겨난 서브 카메라를 없애주는 함수.
    public void RemoveSubCam(int idx)
    {
        subCam.Remove(subCam[idx]);
        subUICam.Remove(subUICam[idx]);

        /*
        if (subCam.Count > 0)
        {
            subWH = new Vector2(1f / subCam.Count, 0.5f);

            for (int i = 0; i < subCam.Count; ++i)
            {
                subCam[i].rect = new Rect(subWH.x * i, 0, subWH.x, subWH.y);
                subUICam[i].rect = new Rect(subWH.x * i, 0, subWH.x, subWH.y);
            }
        }
        else
        {
            mainXY = new Vector2(0, 0);
            mainWH = new Vector2(1, 1);

            mainCam.rect = new Rect(mainXY.x, mainXY.y, mainWH.x, mainWH.y);
            uiCam.rect = new Rect(mainXY.x, mainXY.y, mainWH.x, mainWH.y);
        }*/

        b_isRemoving = false;
    }

    // 단면을 추가함에 따라 해당 단면을 비춰주는 카메라를 추가하는 함수.
    public void AddSubCam(GameObject obj, GameObject obj2)
    {
        // obj는 OBJ CAM, obj2는 UI CAM

        subCam.Add(obj.GetComponent<Camera>());
        subUICam.Add(obj2.GetComponent<Camera>());

        subWH = new Vector2(1f / subCam.Count, 0.5f);
        mainXY = new Vector2(0, 0.5f);
        mainWH = new Vector2(1, 0.5f);
        /*
        mainCam.rect = new Rect(mainXY.x, mainXY.y, mainWH.x, mainWH.y);
        uiCam.rect = new Rect(mainXY.x, mainXY.y, mainWH.x, mainWH.y);

        for (int i = 0; i < subCam.Count; ++i)
        {
            subCam[i].rect = new Rect(subWH.x * i, 0, subWH.x, subWH.y);
            subUICam[i].rect = new Rect(subWH.x * i, 0, subWH.x, subWH.y);
        }*/
    }

    // 메인이 되는 카메라를 바꿔주는 함수.
    public void ChangeMainCamera(int idx)
    {
        if (!mainCam.transform.parent.GetComponent<CameraIndex>().isMain)
        {
            int tmpIdx = mainCam.transform.parent.GetComponent<CameraIndex>().idx;  // 카메라 순서를 변경시키기 위한 임시 변수
            mainCam.transform.parent.GetComponent<CameraIndex>().idx = idx;
            subCam[idx].transform.parent.GetComponent<CameraIndex>().idx = tmpIdx;
        }
        else
        {
            mainCam.transform.parent.GetComponent<CameraIndex>().idx = idx;
        }

        Camera tmp = subCam[idx];
        Camera tmp2 = subUICam[idx];
        Rect subRect = subCam[idx].rect;

        subCam[idx] = mainCam;
        mainCam = tmp;

        subUICam[idx] = uiCam;
        uiCam = tmp2;

        //mainCam.rect = new Rect(mainXY.x, mainXY.y, mainWH.x, mainWH.y);
        //uiCam.rect = new Rect(mainXY.x, mainXY.y, mainWH.x, mainWH.y);

        // subCam[idx].rect = subRect;
        //  subUICam[idx].rect = subRect;
    }
}
