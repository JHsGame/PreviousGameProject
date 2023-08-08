using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureCam : MonoBehaviour
{
    Vector3 vec;

    void Update()
    {
        if (AddCaseManager.instance.VolumeObj != null)
        {
            vec = AddCaseManager.instance.VolumeObj.transform.position;
            transform.localPosition = new Vector3(vec.x, vec.y, vec.z - 1.5f);
        }
    }
}
