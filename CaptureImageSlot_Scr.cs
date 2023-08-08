using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureImageSlot_Scr : MonoBehaviour
{
    private string _imagePath;

    [SerializeField]
    Image _captureImage;

    [SerializeField]
    GameObject _exitButton;
    
    public void OnClickImageSlot()
    {
        for(int i = 0; i < StaticManager.instance.scr_DrawCaptureArea.Length; ++i)
        {
            StaticManager.instance.scr_DrawCaptureArea[i].ChangeThumbnail(_captureImage.sprite);
        }
    }

    public void ExitButtonOn()
    {
        if (_captureImage.sprite != null)
        {
            _exitButton.SetActive(true);
        }
    }

    public void ExitButtonOff()
    {
        if (_captureImage.sprite != null)
        {
            _exitButton.SetActive(false);
        }
    }

    public void OnClickRemoveFile()
    {
        System.IO.File.Delete(_imagePath);

        StaticManager.instance.scr_SettingsManager.LoadImage();
    }

    public string ImagePath
    {
        set
        {
            _imagePath = value;
        }
        get
        {
            return _imagePath;
        }
    }
}