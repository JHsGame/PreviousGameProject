using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityVolumeRendering;

public class testScript : MonoBehaviour
{
    public void tt(VolumeRenderedObject target)
    {
        //UnityVolumeRendering.
        //TransferFunctionEditorWindow.ShowWindow();
    }

    public void move()
    {
        AddCaseManager.instance.VolumeObj.transform.position = Input.mousePosition;
    }
}
