using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTexture : MonoBehaviour
{
    public bool b_isNotWorkStep;
    public Camera UICam;

    public GameObject obj;
    public GameObject ZoomInImage;
    public GameObject OnImage;
    public GameObject objImage;
    public GameObject SliceImage;

    Material material;

    // 잘린 단면을 보여주는 이미지 UI
    private void OnEnable()
    {
        if (!b_isNotWorkStep)
        {
            // 단면을 잘랐을 때, 단면의 이미지 오브젝트를 순서대로 일정 UI 위치로 이동을 처리.
            for (int i = 0; i < StaticManager.instance.tf_WorkStep_Canvas_2ndDataArea.Length; i++)
            {
                if (!StaticManager.instance.b_WorkStep_Canvas_2ndDataArea[i])
                {
                    SliceImage.transform.SetParent(StaticManager.instance.tf_WorkStep_Canvas_2ndDataArea[i]);
                    StaticManager.instance.b_WorkStep_Canvas_2ndDataArea[i] = true;

                    SliceImage.transform.localPosition = new Vector3(0, 0, 0);
                    SliceImage.transform.localScale = new Vector3(0.03f, 0.03f, 0);
                    break;
                }
            }
        }
    }

    // Volume Rendering 오브젝트의 단면 이미지 정보를 해당 이미지 UI 오브젝트에 정보를 넣음.
    void Update()
    {
        objImage.GetComponent<RawImage>().material = obj.GetComponent<MeshRenderer>().material;

        //this.GetComponent<MeshRenderer>().material = obj.GetComponent<MeshRenderer>().material;
        /*
        Vector3 tmpPos = UICam.ScreenToViewportPoint(ZoomInImage.transform.GetComponent<RectTransform>().anchoredPosition);
        int k = TestController.instance.TexturePlaneParent.transform.childCount;
        tmpPos = new Vector3((1080f * k) + (400f * (k - 1)), 4100f, 1f);
        */

        //ZoomInImage.transform.GetComponent<RectTransform>().anchoredPosition = UICam.ViewportToWorldPoint(tmpPos);
        //OnImage.transform.GetComponent<RectTransform>().anchoredPosition = UICam.ViewportToWorldPoint(new Vector3(tmpPos.x + 5f, tmpPos.y - 1200f, tmpPos.z));
    }
}
