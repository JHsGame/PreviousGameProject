using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityVolumeRendering;

public class CameraRay : MonoBehaviour
{
    public Camera camera;

    int layerMask;
    void Start()
    {
        camera = this.transform.GetComponent<Camera>();
    }

    // 제거된 카메라인지 체크하는 bool 변수는 현재 사용하고 있지 않음.
    // 추가된 단면을 비추는 카메라로부터 단면을 쏴, 해당 단면을 선택했는지의 여부를 체크해주는 함수.
    void Update()
    {
        if (!CameraManager_Scr.instance.b_isRemoving)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            layerMask = 1 << LayerMask.NameToLayer("TexturePlane");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    LinerendererTest.instance.EveryModeOff();
                    if (CameraManager_Scr.instance.sliceRenderingPlane != null)
                    {
                        CameraManager_Scr.instance.sliceRenderingPlane.GetComponent<SlicingPlane>().ObjOFF();
                    }
                    CameraManager_Scr.instance.sliceRenderingPlane = hit.collider.gameObject.GetComponent<TestTexture>().obj;
                    CameraManager_Scr.instance.sliceRenderingPlane.GetComponent<SlicingPlane>().ObjON();

                }
            }
        }
    }

    /*public void EnterUI()
    {
        LinerendererTest.instance.MouseOnUI();
    }

    public void OutUI()
    {
        LinerendererTest.instance.MouseLeaveUI();
    }*/
}
