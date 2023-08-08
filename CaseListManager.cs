using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using TMPro;

// 케이스에 들어가는 변수들을 담은 클래스
[System.Serializable]
public class Case
{
    public string _Name;
    public string _Patient_Description;
    public string _Case_name;
    public string _Client;
    public string _Due_date;
    public int _Case_type;
    public string _Case_Description;
    public bool[] _Option = new bool[2];
    public bool[] _Implant_planning = new bool[3];
    public string[] _Patient_CT;
    public string[] _Upper_CT_or_Scan;
    public string[] _Lower_CT_or_Scan;
    public string[] _Extra_CT_or_Scan;
    public int _Kit;
}

public class CaseListManager : MonoBehaviour
{

    public Case CaseData;
    public List<Case> List_CaseData;

    public Transform TableContents;

    public GameObject G_ProGress;
    public GameObject G_SelectButton;
    public GameObject Info_Toothsetup;
    public GameObject Info_Casetype;
    public GameObject Info_Designoption;
    public GameObject Info_Kit;
    public GameObject[] Info_Data;

    UnityEngine.UI.TableUI.TableUI scr_TableUI;


    private void Awake()
    {
        // 케이스 추가 화면의 표부분 내용 비워주기
        scr_TableUI = transform.GetComponent<UnityEngine.UI.TableUI.TableUI>();
        for (int i = 0; i < scr_TableUI.Rows; i++)
        {
            if (List_CaseData.Count <= i && i < scr_TableUI.Rows - 1)
            {
                for (int j = 0; j < 8; j++)
                {
                    switch (j)
                    {
                        case 0:
                            TableContents.GetChild(i + 1).GetChild(1).GetChild(j).GetComponent<TextMeshProUGUI>().text = "";
                            break;
                        case 1:
                            TableContents.GetChild(i + 1).GetChild(1).GetChild(j).GetComponent<TextMeshProUGUI>().text = "";
                            break;
                        case 2:
                            TableContents.GetChild(i + 1).GetChild(1).GetChild(j).GetComponent<TextMeshProUGUI>().text = "";
                            break;
                        case 3:
                            TableContents.GetChild(i + 1).GetChild(1).GetChild(j).GetComponent<TextMeshProUGUI>().text = "";
                            break;
                        case 4:
                            TableContents.GetChild(i + 1).GetChild(1).GetChild(j).GetComponent<TextMeshProUGUI>().text = "";
                            break;
                        case 5:
                            TableContents.GetChild(i + 1).GetChild(1).GetChild(j).GetComponent<TextMeshProUGUI>().text = "";
                            // GameObject obj = Instantiate(G_ProGress, TableContents.GetChild(i + 1).GetChild(1).GetChild(j).transform);
                            // obj.transform.localPosition = new Vector3(0, 0, 0);
                            break;
                        case 6:
                            TableContents.GetChild(i + 1).GetChild(1).GetChild(j).GetComponent<TextMeshProUGUI>().text = "";
                            break;
                        case 7:
                            TableContents.GetChild(i + 1).GetChild(1).GetChild(j).GetComponent<TextMeshProUGUI>().text = "";
                            break;
                    }
                }
            }
        }
    }

    public void delayStart2()
    {
        // 저장된 케이스 항목들이 있으면 로드하기 
        string[] SerachFiles = Directory.GetFiles(StaticManager.instance.scr_SettingsManager.CaseData_Path, "*.json");


        List_CaseData.Clear();


        for (int i = 0; i < SerachFiles.Length; i++)
        {
            if (SerachFiles[i].Contains("CaseData"))
            {
                string data = File.ReadAllText(SerachFiles[i]);

                Case tmp = JsonConvert.DeserializeObject<Case>(data);

                List_CaseData.Add(tmp);
            }
        }

        if (List_CaseData.Count > 0)
        {
            delayStart();
        }
    }

    // 로드된 케이스 정보값들을 표에 기입
    void delayStart()
    {

        for (int i = 0; i < StaticManager.instance.scr_TableUI.Rows; i++)
        {
            if (List_CaseData.Count > i)
            {
                for (int j = 0; j < 8; j++)
                {
                    switch (j)
                    {
                        case 0:
                            TableContents.GetChild(i + 1).GetChild(1).GetChild(j).GetComponent<TextMeshProUGUI>().text = List_CaseData[i]._Case_name;
                            GameObject obj2 = Instantiate(G_SelectButton, TableContents.GetChild(i + 1).GetChild(1).GetChild(j).transform);
                            obj2.transform.GetComponent<SelectCase>().info = List_CaseData[i];
                            obj2.transform.localPosition = new Vector3(0, 0, 0);

                            // 리스트 화면이 켜질때 마다 최상단의 리스트의 정보가 미리보기창에 기입되도록 한다.
                            if (i == 0)
                            {
                                obj2.transform.GetComponent<SelectCase>().SelectCASE(true);
                            }
                            break;

                        case 1:
                            TableContents.GetChild(i + 1).GetChild(1).GetChild(j).GetComponent<TextMeshProUGUI>().text = List_CaseData[i]._Name;
                            break;
                        case 2:
                            TableContents.GetChild(i + 1).GetChild(1).GetChild(j).GetComponent<TextMeshProUGUI>().text = List_CaseData[i]._Client;
                            break;
                        case 3:
                            TableContents.GetChild(i + 1).GetChild(1).GetChild(j).GetComponent<TextMeshProUGUI>().text = List_CaseData[i]._Case_Description;
                            break;
                        case 4:
                            TableContents.GetChild(i + 1).GetChild(1).GetChild(j).GetComponent<TextMeshProUGUI>().text = List_CaseData[i]._Due_date;
                            break;
                        case 5:
                            TableContents.GetChild(i + 1).GetChild(1).GetChild(j).GetComponent<TextMeshProUGUI>().text = "";
                            GameObject obj = Instantiate(G_ProGress, TableContents.GetChild(i + 1).GetChild(1).GetChild(j).transform);
                            obj.transform.localPosition = new Vector3(0, 0, 0);
                            break;
                        case 6:
                            TableContents.GetChild(i + 1).GetChild(1).GetChild(j).GetComponent<TextMeshProUGUI>().text = "";
                            break;
                        case 7:
                            TableContents.GetChild(i + 1).GetChild(1).GetChild(j).GetComponent<TextMeshProUGUI>().text = "";
                            break;
                    }
                }
            }
        }
    }
}
