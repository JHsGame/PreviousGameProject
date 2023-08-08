using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems; //지시문 추가
public class FollowMouse : MonoBehaviour
{
    Vector3 mousePosition;

    public bool isMove = false;

    // STL 파일 혹은 Volume Rendering 오브젝트를 마우스 좌클릭을 한 위치로 이동시키도록 하는 함수.
    void Update()
    {
        /*if (!EventSystem.current.IsPointerOverGameObject() && isMove && Input.GetMouseButton(0))
        {
            
            mousePosition = Input.mousePosition;
            float dist = Vector3.Dot(mousePosition - CameraManager_Scr.instance.mainCam.transform.position, CameraManager_Scr.instance.mainCam.transform.up);
            mousePosition.z = dist / 100;

            transform.position = CameraManager_Scr.instance.mainCam.ScreenToWorldPoint(mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            print(mousePosition);
            

            RaycastHit hit;
            int layerMask = 1 << LayerMask.NameToLayer("MoveAreaPlane");

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask))
            {

                transform.position = hit.point;
            }
        }*/
    }
}
