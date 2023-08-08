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

    // ���� ��� �� ���� �̸� Ȯ�ο�
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

    // �ҷ������� �ϴ� STL������ ��θ� �������µ� ���
    public void OpenSTL()
    {
        WriteSTL(StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false));
        Invoke("STLTransform", 0.1f);
    }

    // �ҷ������� �ϴ� DICOM ���ϵ��� ���ִ� ������ �������µ� ���
    public void OpenDCMFolder()
    {
        var paths = StandaloneFileBrowser.OpenFolderPanel("Select Folder", "", true);
        WriteTexture(paths);
    }

    // paths�� �ҷ������� �ϴ� ������ �ִ� ����̴�.
    // �ҷ����� STL ������ ������� �Ͼ������� �������ִ� ���ҵ� �ϰ� �ִ�.
    // STL ������ �ҷ��� ������Ʈȭ ��Ű��, �� �� STL ������ ������Ʈ�� ���� ������Ʈ�� �����Ǿ� �ִµ�, �ش� ������Ʈ���� �ϳ��� �����ִ� �θ� ������Ʈ�� �� ���� ���̵��� �Ǿ��ִ�.
    // ���������� ������� �Ͼ������� ���� STL ��ġ�� �������ִ� �Լ��� ȣ���Ѵ�.
    public void WriteSTL(string[] paths)
    {
        if (paths.Length == 0)
        {
            return;
        }

        path = "";

        foreach (var p in paths)
        {
            // Ȯ���ڸ���� ����
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

        // �� ĵ������ STL�� �ش��ϴ� ������Ʈ���� ��ġ �ʱ�ȭ
        STLCanvas_objMeshToExport.transform.position = Vector3.zero;
        STLCanvas_objMeshToExport.transform.rotation = Quaternion.identity;
        CaptureCanvas_objMeshToExport.transform.position = Vector3.zero;
        CaptureCanvas_objMeshToExport.transform.rotation = Quaternion.identity;

        // ������ �����Ǿ� �ִ� STL ������Ʈ�� �����Ѵٸ�, �̸� �����ִ� �۾�.
        // sourceArray�� STL�� ������Ʈȭ �� �� ���� ������Ʈ���� ����.
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

    // STL ������Ʈ�� ������� �Ͼ�����, DICOM ���ϵ� ���� �ҷ��������� ���� �޶����� ��ġ�� �������ִ� ������ �ϴ� �Լ��̴�.
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

    // �ҷ������� �ϴ� DICOM ���ϵ��� ��θ� SystemIOFileLoad() �Լ��� �־��ִ� ������ �ϴ� �Լ��̴�.
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

    // �ҷ��� DICOM ���ϵ��� ������ ���� Texture 2Dȭ �� ��, Texture 2D Array�� �����ִ� ������ �ϴ� �Լ��̴�.
    // �߰��� Texture2D ������ ó�����ִ� Convert() �Լ��� ȣ�����ִ� ������ �Ѵ�.
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

    // �����ִ� Texture 2D���� Texture 3Dȭ �Ͽ� Volume Renderingȭ �ϴ� �۾��� �ϴ� �Լ��̴�.
    // Texture 3D�� �ִ� ������ Volume Rendering ������Ʈ�� �ִ� �Լ��� ChangeTexture(tex3d)�� ȣ���Ѵ�.
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

        // ���� ������ ������������ �翭���� �ӵ������� ���� ���� ����
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