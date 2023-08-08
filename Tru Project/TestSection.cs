using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 현재는 사용하지 않는 클래스.
public class TestSection : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    public GameObject obj;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        meshRenderer.sharedMaterial.SetMatrix("_parentInverseMat", obj.transform.worldToLocalMatrix);
        meshRenderer.sharedMaterial.SetMatrix("_planeMat", Matrix4x4.TRS(transform.position, transform.rotation, obj.transform.lossyScale));
    }
}
