using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlicingTexturePlane : MonoBehaviour
{
    public GameObject obj;
    public GameObject objImage;

    // Volume Rendering 오브젝트에서 잘린 단면을 설정하는 영역.
    void Update()
    {
        objImage.GetComponent<RawImage>().material = obj.GetComponent<MeshRenderer>().material;

        int k = TestController.instance.TexturePlaneParent.transform.childCount;    // k는 Volume Renderimng 오브젝트로부터 잘린 단면 중 몇 번째 단면인지 체크해주는 변수.



        //this.GetComponent<MeshRenderer>().material = obj.GetComponent<MeshRenderer>().material;
        //ZoomInImage.transform.GetComponent<RectTransform>().anchoredPosition = UICam.ViewportToWorldPoint(tmpPos);
        //OnImage.transform.GetComponent<RectTransform>().anchoredPosition = UICam.ViewportToWorldPoint(new Vector3(tmpPos.x + 5f, tmpPos.y - 1200f, tmpPos.z));
    }
}
