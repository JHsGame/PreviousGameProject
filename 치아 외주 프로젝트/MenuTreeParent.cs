using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTreeParent : MonoBehaviour
{
    List<MenuTree_Scr> _myTree = new List<MenuTree_Scr>();

    private void OnEnable()
    {
        if (_myTree.Count <= 0)
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                _myTree.Add(transform.GetChild(i).GetComponent<MenuTree_Scr>());
            }
        }
    }

    private void Start()
    {
        if (_myTree.Count <= 0)
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                _myTree.Add(transform.GetChild(i).GetComponent<MenuTree_Scr>());
            }
        }
    }

    public void TreeChangeMode()
    {
        for(int i = 0; i < _myTree.Count; ++i)
        {
            _myTree[i].PanelOnOff();
        }
    }
}
