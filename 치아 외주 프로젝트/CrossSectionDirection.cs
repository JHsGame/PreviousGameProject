using Min_Max_Slider;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSectionDirection : MonoBehaviour
{
    private MinMaxSlider _minmaxSlider;
    [SerializeField] private bool _isUpNDown;
    [SerializeField] private bool _isFrontNBack;
    [SerializeField] private bool _isLeftNRight;

    private void OnEnable()
    {
        Invoke("DelayStart", 0.1f);
    }

    private void DelayStart()
    {
        _minmaxSlider = this.GetComponent<MinMaxSlider>();
        StartCoroutine(sliderDirection());
    }

    IEnumerator sliderDirection()
    {
        Dictionary<string, GameObject> Planes = StaticManager.instance.CrossSectionPlane;
        Vector3[] vectors = StaticManager.instance.CrossSectionVector;

        Vector3 up = vectors[0];
        Vector3 down = vectors[1];
        Vector3 front = vectors[2];
        Vector3 back = vectors[3];
        Vector3 left = vectors[4];
        Vector3 right = vectors[5];

        while (true)
        {

            float min = (50 - _minmaxSlider.minValue) * 0.01f;
            float max = (_minmaxSlider.maxValue - 50) * 0.01f;

            print(min + ", " + max);

            if (_isUpNDown)
            {
                AddCaseManager.instance.VolumeObj.GetComponent<MeshRenderer>().material.SetFloat("_CutMinZ", -min);
                AddCaseManager.instance.VolumeObj.GetComponent<MeshRenderer>().material.SetFloat("_CutMaxZ", max);
            }
            else if (_isFrontNBack)
            {
                AddCaseManager.instance.VolumeObj.GetComponent<MeshRenderer>().material.SetFloat("_CutMinY", -min);
                AddCaseManager.instance.VolumeObj.GetComponent<MeshRenderer>().material.SetFloat("_CutMaxY", max);
            }
            else if (_isLeftNRight)
            {
                AddCaseManager.instance.VolumeObj.GetComponent<MeshRenderer>().material.SetFloat("_CutMinX", -min);
                AddCaseManager.instance.VolumeObj.GetComponent<MeshRenderer>().material.SetFloat("_CutMaxX", max);
            }
            
           /*
           if (_isUpNDown)
           {
               Planes["Up"].gameObject.transform.localPosition = up + new Vector3(960f, 540f - min, 0f);
               Planes["Down"].gameObject.transform.localPosition = down + new Vector3(960f, 540f + max, 0f);
           }
           else if (_isFrontNBack)
           {
               Planes["Front"].gameObject.transform.localPosition = front + new Vector3(960f, 540f, - min);
               Planes["Back"].gameObject.transform.localPosition = back + new Vector3(960f, 540f, max);
           }
           else if (_isLeftNRight)
           {
               Planes["Left"].gameObject.transform.localPosition = left + new Vector3(960f - min, 540f, 0f);
               Planes["Right"].gameObject.transform.localPosition = right + new Vector3(960f + max, 540f, 0f);
           }*/

           yield return CashingCoroutine_Scr.WaitForFixedUpdate;
        }
    }

    private void OnDisable()
    {
        StopCoroutine(sliderDirection());
    }
}