using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 현재는 쓰이지 않는 클래스.
public class CameraIndex : MonoBehaviour
{
    public int idx; // 추가로 생긴 카메라가 몇 번째 카메라인지 체크해주는 변수.
    public bool isMain; // 메인 카메라인지 체크해주는 변수

    // Start is called before the first frame update
    void Update()
    {
        if (!isMain)
            idx = transform.GetSiblingIndex();
        else
            idx = -1;
    }

    // 메인이 되는 카메라 화면을 선택해주는 함수이다. 현재는 사용하지 않는 함수.
    public void OnClickPlusButton()
    {
        CameraManager_Scr.instance.ChangeMainCamera(idx);
    }

    // 자른 화면을 보여주는 서브 카메라들을 제거하는 함수이다. 현재는 사용하지 않는 함수.
    public void OnClickCloseButton()
    {
        CameraManager_Scr.instance.b_isRemoving = true;

        CameraManager_Scr.instance.RemoveSubCam(idx);
        Destroy(TestController.instance.myVolumeObj.transform.GetChild(idx + 1).gameObject);

        Destroy(gameObject);
    }
}
