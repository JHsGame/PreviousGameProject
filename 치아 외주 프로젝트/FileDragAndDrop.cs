using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using B83.Win32;
using UnityEngine.UI;
using Dicom;
using Dicom.Imaging;

public class FileDragAndDrop : MonoBehaviour
{
    List<string> log = new List<string>();
    public Text m_text;

    void OnEnable()
    {
        // must be installed on the main thread to get the right thread id.
        UnityDragAndDropHook.InstallHook();
        UnityDragAndDropHook.OnDroppedFiles += OnFiles;
    }
    void OnDisable()
    {
        UnityDragAndDropHook.UninstallHook();
    }

    // ������ ������ ������ �ְ�, �ش� ������ ���Ŀ� ���� �̹����� ���߰�, ���ϸ��� Text UI�� �°� ������ִ� �Լ��̴�.
    void OnFiles(List<string> aFiles, POINT aPos)
    {
        // do something with the dropped file names. aPos will contain the 
        // mouse position within the window where the files has been dropped.
        /*string str = "Dropped " + aFiles.Count + " files at: " + aPos + "\n\t" +
            aFiles.Aggregate((a, b) => a + "\n\t" + b);
        Debug.Log(str);
        log.Add(str);*/


        string[] split_FileName = aFiles[0].Split('\\');    // ���ϸ��� ���� ����.
        string[] split_Extension = aFiles[0].Split('.');    // ������ ������ ���� ����.

        if (m_text.text == "Click to add a file")
        {
            StaticManager.instance.AddCase_Databox_InsideBox.SetActive(false);
            StaticManager.instance.AddCase_Databox_ScrollView.SetActive(true);
        }

        // ������ �ڽ��� ���� ���ϵ��� �����ִ� �θ� ������Ʈ
        Transform Contents = StaticManager.instance.AddCase_Databox_ScrollView.transform.GetChild(0).GetChild(0);
        for (int i = 0; i < Contents.transform.childCount; ++i)
        {
            if (Contents.transform.GetChild(i).gameObject.GetComponent<Image>().sprite == null)
            {
                switch (split_Extension[split_Extension.Length - 1])
                {
                    // ������ ���� ���Ŀ� ���� �����ִ� UI �̹��� �� �ؽ�Ʈ�� �����ϴ� ����.
                    case "dcm":
                        DicomFile dicomFile = DicomFile.Open(aFiles[0]);
                        DicomImage image = new DicomImage(dicomFile.Dataset);
                        var tex2d = image.RenderImage().As<Texture2D>();
                        Rect rect = new Rect(0, 0, tex2d.width, tex2d.height);
                        Contents.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = Sprite.Create(tex2d, rect, new Vector2(0.5f, 0.5f));
                        break;

                    case "pdf":
                        Contents.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = StaticManager.instance.DataBox_Icons[0];
                        break;

                    case "txt":
                        Contents.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = StaticManager.instance.DataBox_Icons[1];
                        break;

                    case "exe":
                        Contents.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = StaticManager.instance.DataBox_Icons[2];
                        break;

                    case "png":
                    case "jpg":
                        Contents.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = StaticManager.instance.DataBox_Icons[3];
                        break;

                    default:
                        Contents.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = StaticManager.instance.DataBox_Icons[4];
                        break;
                }

                Contents.transform.GetChild(i).gameObject.GetComponent<OpenFile>().filePath = aFiles[0];
                Contents.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
                Contents.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text = split_FileName[split_FileName.Length - 1];
                Contents.transform.GetChild(i).gameObject.SetActive(true);
                break;
            }
        }
    }

    /*
    private void OnGUI()
    {
        if (GUILayout.Button("clear log"))
            log.Clear();
        foreach (var s in log)
            GUILayout.Label(s);
    }
    */
}
