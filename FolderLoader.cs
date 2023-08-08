using Dicom;
using Dicom.Imaging;
using Parabox.Stl;
using SFB;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityVolumeRendering;
using VolumeRendering;
using Newtonsoft.Json;
using Dicom.Imaging.Mathematics;

public class FolderLoader : MonoBehaviour
{
    public Texture2DArrayToTexture3DConverter converter;
    string error;
    public TestController controller;
    public GameObject STLCanvas_objMeshToExport;
    public GameObject CaptureCanvas_objMeshToExport;
    public GameObject source;
    string path;
    public Mesh[] meshs;

    // 파일 경로 및 파일 이름 확인용
    public string test;
    public string s;

    private bool b_isUpper = false;
    /*
    public void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(new Vector3(50, 50, 1), Quaternion.identity, new Vector3(3, 2.5f, 1));

        if (GUILayout.Button("Open STL File"))
        {
            WriteSTL(StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false));
        }

        GUI.matrix = Matrix4x4.TRS(new Vector3(50, 50, 1), Quaternion.identity, new Vector3(3, 2.5f, 1));

        if (GUILayout.Button("Open .dcm Folder"))
        {
            var paths = StandaloneFileBrowser.OpenFolderPanel("Select Folder", "", true);
            WriteTexture(paths);
        }

        GUILayout.Space(15);
    }*/

    // 불러오고자 하는 STL파일의 경로를 가져오는데 사용
    public void OpenSTL()
    {
        WriteSTL(StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false));
        Invoke("STLTransform", 0.1f);
    }

    // 불러오고자 하는 DICOM 파일들이 모여있는 폴더를 가져오는데 사용
    public void OpenDCMFolder()
    {
        var paths = StandaloneFileBrowser.OpenFolderPanel("Select Folder", "", true);
        WriteTexture(paths);
    }

    // paths는 불러오고자 하는 파일이 있는 경로이다.
    // 불러오는 STL 파일이 상악인지 하악인지를 구별해주는 역할도 하고 있다.
    // STL 파일을 불러와 오브젝트화 시키고, 이 때 STL 파일의 오브젝트는 여러 오브젝트로 구성되어 있는데, 해당 오브젝트들을 하나로 묶어주는 부모 오브젝트에 다 같이 묶이도록 되어있다.
    // 마찬가지로 상악인지 하악인지에 따라 STL 위치를 지정해주는 함수를 호출한다.
    public void WriteSTL(string[] paths)
    {
        if (paths.Length == 0)
        {
            return;
        }

        path = "";

        foreach (var p in paths)
        {
            // 확장자명까지 포함
            path += p;
        }

        string str = "Upper";

        if (path.Contains(str))
        {
            b_isUpper = true;
        }
        else
        {
            b_isUpper = false;
        }

        // 각 캔버스별 STL에 해당하는 오브젝트들의 위치 초기화
        STLCanvas_objMeshToExport.transform.position = Vector3.zero;
        STLCanvas_objMeshToExport.transform.rotation = Quaternion.identity;
        CaptureCanvas_objMeshToExport.transform.position = Vector3.zero;
        CaptureCanvas_objMeshToExport.transform.rotation = Quaternion.identity;

        // 이전에 생성되어 있는 STL 오브젝트가 존재한다면, 이를 없애주는 작업.
        // sourceArray는 STL을 오브젝트화 할 때 생긴 오브젝트들의 모임.
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
        }

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
        }

        Invoke("STLTransform", 0.001f);
    }

    // STL 오브젝트가 상악인지 하악인지, DICOM 파일도 같이 불러오는지에 따라 달라지는 위치를 지정해주는 역할을 하는 함수이다.
    public void STLTransform()
    {
        STLCanvas_objMeshToExport.transform.position = new Vector3(-0.8f, 0f, 0f);

        if (!StaticManager.instance.scr_AddCaseManager.isLoadDicom)
        {
            if (b_isUpper)
            {
                STLCanvas_objMeshToExport.transform.rotation = Quaternion.Euler(new Vector3(0f, -90f, -60f));
            }
            else
            {
                STLCanvas_objMeshToExport.transform.rotation = Quaternion.Euler(new Vector3(0f, -90f, 0f));
            }
            STLCanvas_objMeshToExport.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

            for (int i = 0; i < STLCanvas_objMeshToExport.transform.childCount; ++i)
            {
                STLCanvas_objMeshToExport.transform.GetChild(i).transform.localScale = new Vector3(1f, 1f, 1f);
            }

            CameraManager_Scr.MainOBJ_STL = STLCanvas_objMeshToExport;
        }
        else
        {
            if (b_isUpper)
            {
                CaptureCanvas_objMeshToExport.transform.localPosition = new Vector3(-2.1f, -0.05f, 0f);
                CaptureCanvas_objMeshToExport.transform.rotation = Quaternion.Euler(new Vector3(-10f, -95f, -5.8f));
            }
            else
            {
                CaptureCanvas_objMeshToExport.transform.localPosition = new Vector3(-2.1f, -0.02f, 0f);
                CaptureCanvas_objMeshToExport.transform.rotation = Quaternion.Euler(new Vector3(-10f, -90f, 30f));
            }
            CaptureCanvas_objMeshToExport.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

            for (int i = 0; i < CaptureCanvas_objMeshToExport.transform.childCount; ++i)
            {
                CaptureCanvas_objMeshToExport.transform.GetChild(i).transform.localScale = new Vector3(1f, 1f, 1f);
            }

            CameraManager_Scr.MainOBJ_STL = CaptureCanvas_objMeshToExport;
        }
    }

    // 불러오고자 하는 DICOM 파일들의 경로를 SystemIOFileLoad() 함수에 넣어주는 역할을 하는 함수이다.
    public void WriteTexture(string[] paths)
    {
        converter.texture2DArray.Clear();
        if (paths.Length == 0)
        {
            return;
        }

        path = "";
        foreach (var p in paths)
        {
            path += p;
        }
        SystemIOFileLoad(path);
    }

    // 불러온 DICOM 파일들의 정보를 토대로 Texture 2D화 한 후, Texture 2D Array에 묶어주는 역할을 하는 함수이다.
    // 추가한 Texture2D 묶음을 처리해주는 Convert() 함수를 호출해주는 역할을 한다.
    private void SystemIOFileLoad(string Path)
    {
        //Dicom.Imaging.Codec.TranscoderManager.SetImplementation(new Dicom.Imaging.NativeCodec.NativeTranscoderManager());
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var info = new DirectoryInfo(path);
        var fileInfo = info.GetFiles();
        foreach (var file in fileInfo)
        {
            if (file.Extension == ".dcm")
            {
                /*
                Point3D ef = null;
                Point3D ef2 = null;
                var fe = ef.Distance(ef2);

                Vector3D efe = new Vector3D(1, 1, 1);
                Vector3D ef222 = new Vector3D(12, 111, 1);
                var few = efe.Distance(ef222);
                Debug.Log(few);*/


                DicomFile dicomFile = DicomFile.Open(file.FullName);
                DicomImage image = new DicomImage(dicomFile.Dataset);
                test = image.RenderImage().ToString();
                s = file.DirectoryName;
                var tex2d = image.RenderImage().As<Texture2D>();
                converter.texture2DArray.Add(tex2d);

            }
        }

        Convert(Path);
    }

    // 묶여있는 Texture 2D들을 Texture 3D화 하여 Volume Rendering화 하는 작업을 하는 함수이다.
    // Texture 3D에 있는 내용을 Volume Rendering 오브젝트에 넣는 함수인 ChangeTexture(tex3d)를 호출한다.
    public void Convert(string Path)
    {
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

        // 새로 열람한 다이콤파일을 재열람시 속도증가를 위해 정보 저장
        if (!StaticManager.instance.scr_AddCaseManager.saved_Volume.ContainsKey(Path))
        {
            StaticManager.instance.scr_AddCaseManager.saved_Volume.Add(Path, tex3d);
            string Data = JsonConvert.SerializeObject(StaticManager.instance.scr_AddCaseManager.saved_Volume, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            File.WriteAllText(Application.persistentDataPath + "/PYLON_SaveCase3DVolume.json", Data);
        }

        if (!StaticManager.instance.scr_AddCaseManager.saved_VolumePath.Contains(Path))
        {
            StaticManager.instance.scr_AddCaseManager.saved_VolumePath.Add(Path);
            string Data2 = JsonConvert.SerializeObject(StaticManager.instance.scr_AddCaseManager.saved_VolumePath, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            File.WriteAllText(Application.persistentDataPath + "/PYLON_SaveCase3DVolumePath.json", Data2);
        }
    }
}