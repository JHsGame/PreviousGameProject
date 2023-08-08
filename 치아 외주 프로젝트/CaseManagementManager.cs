using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CaseManagementManager : MonoBehaviour
{
    private bool _addMenuOn; // 좌측 상단의 부가 메뉴 중 횡단면 관련 설정
    private bool[] _managementMenuOn = new bool[3]; // 우측 상단 3개의 부가 메뉴  0-케이스 상세, 1-케이스 저장, 2-케이스 나가기

    // 미리보기의 정보 기입부분
    [SerializeField]
    private TextMeshProUGUI _patientName;
    [SerializeField]
    private GameObject _infoToothsetup;
    [SerializeField]
    private GameObject _infoCasetype;
    [SerializeField]
    private GameObject _infoDesignOption;
    [SerializeField]
    private GameObject _infoKit;
    [SerializeField]
    private GameObject[] _infoData;

    [SerializeField]
    private GameObject[] _saveList;   // 세이브 버튼을 눌렀을 때 나타나는 리스트

    [SerializeField]
    private GameObject _drawLineObj;

    public GameObject DrawLineObj
    {
        get
        {
            return _drawLineObj;
        }
    }

    public bool AddMenuOn
    {
        set
        {
            _addMenuOn = value;
        }
        get
        {
            return _addMenuOn;
        }
    }
    public bool[] ManagementMenuOn
    {
        set
        {
            _managementMenuOn = value;
        }
        get
        {
            return _managementMenuOn;
        }
    }
    public GameObject InfoToothSetup
    {
        get => _infoToothsetup;
    }
    public GameObject InfoCaseType
    {
        get => _infoCasetype;
    }
    public GameObject InfoDesignOption 
    {
        get => _infoDesignOption;
    }
    public GameObject InfoKit
    {
        get => _infoKit;
    }
    public GameObject[] InfoData
    {
        get => _infoData;
    }

    public GameObject[] SaveList
    {
        get => _saveList;
    }
    public TextMeshProUGUI PatientName
    {
        get => _patientName;
    }
}