using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetManager_Scr : MonoBehaviour
{
    private bool _presetMode = true;   // true면 프리셋의 텍스트 모드, false면 프리셋의 슬라이더 모드.
    [SerializeField]
    private GameObject _ctOTF;

    public bool PresetMode
    {
        set => _presetMode = value;
        get => _presetMode;
    }

    public GameObject CTOTF
    {
        get => _ctOTF;
    }

    public void ChangeMode()
    {
        _presetMode = !_presetMode;

        for(int i =0; i < StaticManager.instance.TreeParents.Length; ++i)
        {
            StaticManager.instance.TreeParents[i].TreeChangeMode();
        }
    }
}
