using QttEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityVolumeRendering;

public class TestController : MonoBehaviour
{
    private static TestController Instance;
    public Transform tmp;

    [SerializeField]
    private GameObject myVolume;
    [SerializeField]
    private GameObject myMainCaptureVolume;
    [SerializeField]
    private GameObject mySubCaptureVolume;
    [SerializeField]
    private GameObject myCaseListPreviewVolume;
    [SerializeField]
    private GameObject mySTL;

    LineRenderer line;
    List<Vector3> curveResult;
    public List<Transform> bezierPoint = new List<Transform>();

    private TFRenderMode tfRenderMode;
    public Texture3D texture;
    public GameObject TexturePlaneParent;
    public GameObject CaptureTexturePlaneParent;
    public GameObject CaseListPreviewTexturePlaneParent;
    public GameObject WorkstepPreviewTexturePlaneParent;
    public TransferFunction transferFunction;
    public TransferFunction2D transferFunction2D;
    public VolumeRenderedObject myVolumeObj;
    public VolumeRenderedObject myMainCaptureVolumeObj;
    public VolumeRenderedObject mySubCaptureVolumeObj;
    public VolumeRenderedObject myCaseListPreviewVolumeObj;
    [SerializeField]
    private VolumeRenderedObject Slice_Canvas_Volume;
    [SerializeField]
    private GameObject Slice_Canvas_TexturePlaneParent;

    public static TestController instance { get => Instance; }

    public void VolumeObjsActive(bool _active)
    {
        myVolumeObj.gameObject.SetActive(_active);
        myMainCaptureVolumeObj.gameObject.SetActive(_active);
        mySubCaptureVolumeObj.gameObject.SetActive(_active);
        myCaseListPreviewVolumeObj.gameObject.SetActive(_active);
        Slice_Canvas_Volume.gameObject.SetActive(_active);
    }

    private void Start()
    {
        if (Instance != null)
            return;
        else
        {
            Instance = this;
            tfRenderMode = TFRenderMode.TF1D;

            line = GetComponent<LineRenderer>();
            //string test = "C:\\Users\\solu0\\Downloads\\gouda_CT\\gouda_CT\\DCT0000.dcm";
            //string[] split = test.Split('\\');
            //print(split[split.Length - 1]);
        }
    }


    private void Update()
    {
        if (StaticManager.instance.WorkStep_Canvas.activeSelf || StaticManager.instance.SubData_Canvas.activeSelf)
        {
            /*
             * 현재 문제점 : UI 레이캐스팅하기.
            print("check");
            RaycastHit hit;
            int layerMask = 1 << LayerMask.NameToLayer("bezierArea");

            if (Input.GetMouseButton(0))
            {
                print(Input.mousePosition);
                print(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity));
                Debug.DrawRay(CameraManager_Scr.instance.mainCam.transform.position, Vector3.forward, Color.blue, Mathf.Infinity);
            }

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask))
            {
                print(hit.transform.name);
                //bezierPoint.Add(hit.transform);
            }
            */
            if (bezierPoint.Count > 0)
            {
                for (int i = 0; i < bezierPoint.Count; ++i)
                {
                    print(bezierPoint[i].position);
                }

                curveResult = Engine.SplineCurve(bezierPoint, 0.01f);

                line.positionCount = curveResult.Count;

                int index = 0;
                foreach (var p in curveResult)
                {
                    line.SetPosition(index, p);
                    index++;
                }
            }
        }
    }

    public void bezierPointAdd()
    {
        tmp.position = Input.mousePosition;
        bezierPoint.Add(tmp);
    }

    // 불러오는 DICOM 파일을 Volume Rendering 오브젝트에 넣어주는 작업.
    // 쉐이더를 통하여 Texture3D 영역을 교체해주는 작업을 하는 함수이다.
    // 변경된 DICOM 파일에 맞게 잘린 단면들도 변경을 해준다.
    public void ChangeTexture(Texture3D _texture)
    {
        Material material = myVolume.GetComponent<MeshRenderer>().material;
        texture = _texture;
        material.SetTexture("_DataTex", texture);
        myVolumeObj.dataset.SetDataTexture(texture);
        Slice_Canvas_Volume.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetTexture("_DataTex", texture);
        Slice_Canvas_Volume.dataset.SetDataTexture(texture);
        mySubCaptureVolume.GetComponent<MeshRenderer>().material.SetTexture("_DataTex", texture);
        mySubCaptureVolumeObj.dataset.SetDataTexture(texture);

        myMainCaptureVolume.GetComponent<MeshRenderer>().material.SetTexture("_DataTex", texture);
        myMainCaptureVolumeObj.dataset.SetDataTexture(texture);

        myCaseListPreviewVolume.GetComponent<MeshRenderer>().material.SetTexture("_DataTex", texture);
        myCaseListPreviewVolumeObj.dataset.SetDataTexture(texture);

        //  myWorkstepPreviewVolume.GetComponent<MeshRenderer>().material.SetTexture("_DataTex", texture);
        //  myWorkstepPreviewVolumeObj.dataset.SetDataTexture(texture);

        // Volume Rendering 오브젝트의 단면들의 이미지를 DICOM 파일 이미지에 맞게 설정해주는 작업.
        if (TexturePlaneParent.transform.childCount > 0)
        {
            for (int i = 0; i < TexturePlaneParent.transform.childCount; ++i)
            {
                TestTexture texture = TexturePlaneParent.transform.GetChild(i).gameObject.GetComponent<TestTexture>();
                GameObject sliceObj = texture.obj; // 단면 오브젝트
                MeshRenderer sliceMesh = sliceObj.GetComponent<MeshRenderer>();
                Material sliceMaterial = sliceMesh.sharedMaterial;

                // 잘려진 단면의 쉐이더를 설정하는 작업. Volume Rendering 오브젝트에 맞게 텍스쳐 등을 변경한다.
                sliceMaterial.SetTexture("_DataTex", myVolumeObj.dataset.GetDataTexture());
                sliceMaterial.SetTexture("_TFTex", myVolumeObj.transferFunction.GetTexture());
                sliceMaterial.SetMatrix("_parentInverseMat", myVolume.transform.parent.worldToLocalMatrix);
                sliceMaterial.SetMatrix("_planeMat", Matrix4x4.TRS(sliceObj.transform.position, sliceObj.transform.rotation, myVolume.transform.parent.lossyScale));

            }
        }

        if (Slice_Canvas_TexturePlaneParent.transform.childCount > 0)
        {
            for (int i = 0; i < Slice_Canvas_TexturePlaneParent.transform.childCount; ++i)
            {
                TestTexture texture = Slice_Canvas_TexturePlaneParent.transform.GetChild(i).gameObject.GetComponent<TestTexture>();
                GameObject sliceObj = texture.obj;
                MeshRenderer sliceMesh = sliceObj.GetComponent<MeshRenderer>();
                Material sliceMaterial = sliceMesh.sharedMaterial;

                sliceMaterial.SetTexture("_DataTex", Slice_Canvas_Volume.dataset.GetDataTexture());
                sliceMaterial.SetTexture("_TFTex", Slice_Canvas_Volume.transferFunction.GetTexture());
                sliceMaterial.SetMatrix("_parentInverseMat", Slice_Canvas_Volume.transform.GetChild(0).gameObject.transform.parent.worldToLocalMatrix);
                sliceMaterial.SetMatrix("_planeMat", Matrix4x4.TRS(sliceObj.transform.position, sliceObj.transform.rotation, Slice_Canvas_Volume.transform.GetChild(0).gameObject.transform.parent.lossyScale));

            }
        }

        if (CaptureTexturePlaneParent.transform.childCount > 0)
        {
            for (int i = 0; i < CaptureTexturePlaneParent.transform.childCount; ++i)
            {
                TestTexture texture = CaptureTexturePlaneParent.transform.GetChild(i).gameObject.GetComponent<TestTexture>();
                GameObject sliceObj = texture.obj;
                MeshRenderer sliceMesh = sliceObj.GetComponent<MeshRenderer>();
                Material sliceMaterial = sliceMesh.sharedMaterial;

                sliceMaterial.SetTexture("_DataTex", mySubCaptureVolumeObj.dataset.GetDataTexture());
                sliceMaterial.SetTexture("_TFTex", mySubCaptureVolumeObj.transferFunction.GetTexture());
                sliceMaterial.SetMatrix("_parentInverseMat", mySubCaptureVolume.transform.parent.worldToLocalMatrix);
                sliceMaterial.SetMatrix("_planeMat", Matrix4x4.TRS(sliceObj.transform.position, sliceObj.transform.rotation, mySubCaptureVolume.transform.parent.lossyScale));

            }
        }
        //texture = AssetDatabase.LoadAssetAtPath(path, typeof(Texture3D)) as Texture3D;
        //texture.UpdateExternalTexture(Application.persistentDataPath + @path);
        //texture = (Texture3D)((Application.persistentDataPath + @path) as object);
    }

    public void OnClickCreateSlicingPlane()
    {
        //  CreateSlicingPlane(Vector3.zero);
    }

    // Volume Rendering 오브젝트에서 내가 원하는 방향 혹은 추가하고자 하는 단면이 있을 경우 해당 함수를 통하여 단면을 생성시키는 함수이다.
    // 생성되는 단면은 Volume Rendering 오브젝트에서 잘린 단면 이미지 정보를 담고있다.
    // vec는 단면의 방향을 결정해주는 매개 변수이다.
    public SlicingPlane CreateSlicingPlane(Vector3 vec)
    {
        if (TexturePlaneParent.transform.childCount < 4)
        {
            GameObject sliceRenderingPlane = GameObject.Instantiate(Resources.Load<GameObject>("SlicingPlane"));
            sliceRenderingPlane.transform.parent = myVolume.transform.parent;   // 추가되는 단면을 Volume Rendering 오브젝트의 하위 오브젝트로 묶음.
            sliceRenderingPlane.transform.localPosition = Vector3.zero;
            sliceRenderingPlane.transform.LookAt(-vec); // 뒤집힌 단면이 출력되어 -vec로 설정.
            sliceRenderingPlane.transform.localScale = Vector3.one * 0.1f;
            MeshRenderer sliceMeshRend = sliceRenderingPlane.GetComponent<MeshRenderer>();
            sliceMeshRend.material = new Material(sliceMeshRend.sharedMaterial);
            Material sliceMat = sliceRenderingPlane.GetComponent<MeshRenderer>().sharedMaterial;

            // 추가되는 단면의 쉐이더 형식을 Volume Rendering 오브젝트에 맞게 설정.
            sliceMat.SetTexture("_DataTex", myVolumeObj.dataset.GetDataTexture());
            sliceMat.SetTexture("_TFTex", myVolumeObj.transferFunction.GetTexture());
            sliceMat.SetMatrix("_parentInverseMat", myVolume.transform.parent.worldToLocalMatrix);
            sliceMat.SetMatrix("_planeMat", Matrix4x4.TRS(sliceRenderingPlane.transform.position, sliceRenderingPlane.transform.rotation, myVolume.transform.parent.lossyScale));

            // 잘려진 단면을 보여주는 이미지 형식의 UI 오브젝트.
            GameObject texturePlane = GameObject.Instantiate(Resources.Load<GameObject>("TexturePlane"));
            texturePlane.transform.parent = TexturePlaneParent.transform;
            texturePlane.transform.localPosition = new Vector3(texturePlane.transform.position.x * 3 * TexturePlaneParent.transform.childCount, texturePlane.transform.position.y, texturePlane.transform.position.z);
            texturePlane.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
            texturePlane.GetComponent<TestTexture>().obj = sliceRenderingPlane;

            // 잘려진 단면의 이미지를 보여주는 서브 카메라.
            CameraManager_Scr.instance.AddSubCam(texturePlane.transform.GetChild(0).gameObject, texturePlane.transform.GetChild(1).gameObject);    // OBJ CAM , UI CAM
            sliceRenderingPlane.GetComponent<SlicingPlane>().TexturePlane = texturePlane;

            if (!LinerendererTest.instance.b_isCreated)
            {
                LinerendererTest.instance.b_isCreated = true;
            }

            return sliceRenderingPlane.GetComponent<SlicingPlane>();    // 새로운 단면을 반환시킴.
        }
        else
        {
            return null;
        }
    }

    // 프로그램을 종료할 때 쓰이는 함수이다.
    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnClickVolumeOnOff()
    {
        if (myVolume.activeSelf)
            myVolume.SetActive(false);
        else
            myVolume.SetActive(true);
    }

    public void OnClickSTLOnOff()
    {
        if (mySTL.activeSelf)
            mySTL.SetActive(false);
        else
            mySTL.SetActive(true);
    }

    public void OnClickTF1D()
    {
        tfRenderMode = TFRenderMode.TF1D;
        myVolumeObj.SetTransferFunctionMode(tfRenderMode);
    }

    public void OnClickTF2D()
    {
        tfRenderMode = TFRenderMode.TF2D;
        myVolumeObj.SetTransferFunctionMode(tfRenderMode);
    }
}