using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStepManager : MonoBehaviour
{
    public bool[] Add_ons; // 왼쪽 상단 3개의 부가 메뉴  0-스크린샷, 1-횡단면 설정, 2-데이터 방향 복원
    public bool[] Add_CaseManagement; // 우측 상단 3개의 부가 메뉴  0-케이스 상세, 1-케이스 저장, 2-케이스 나가기

    public GameObject g_CaseManagement; // 워크스텝에서의 미리보기 최상위 오브젝트 

    // 미리보기의 정보 기입부분
    public GameObject Info_Toothsetup; 
    public GameObject Info_Casetype;
    public GameObject Info_Designoption;
    public GameObject Info_Kit;
    public GameObject[] Info_Data;
}
