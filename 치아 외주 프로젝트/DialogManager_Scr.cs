using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager_Scr : MonoBehaviour
{
    [SerializeField]
    private GameObject _saveMessage;
    [SerializeField]
    private GameObject _saveAsMessage;
    [SerializeField]
    private GameObject _saveSuccessMessage;

    public GameObject SaveMessage
    {
        get => _saveMessage;
    }

    public GameObject SaveAsMessage
    {
        get => _saveAsMessage;
    }

    public GameObject SaveSuccess 
    {
        get => _saveSuccessMessage;
    }
}
