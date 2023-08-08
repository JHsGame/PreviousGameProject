using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScreenCapture : MonoBehaviour
{
    private Camera _mainCam, _uiCam;
    private Vector2 _firstPos, _secondPos;

    string[] _cameraMainLayer = new string[] { "Default", "TransparentFX", "Ignore Raycast", "Water" };
    string[] _captureAllLayer = new string[] { "Default", "TransparentFX", "Ignore Raycast", "Water", "UI" };
    public void CaptureArea(Vector2 _initPos, Vector2 _curPos)
    {
        // 영역 캡쳐에서의 문제점
        // 볼륨 렌더링한 오브젝트가 쉐이더로 나타나는데, 항상 메인 카메라의 정중앙에 나타나도록 되어있다.
        // 원하는 영역으로 캡쳐를 하여도 캡쳐 영역의 중앙에 나타나기 때문에 이를 해결해야 한다.
        _mainCam = CameraManager_Scr.instance.mainCam;
        _uiCam = CameraManager_Scr.instance.uiCam;

        _firstPos = _initPos;
        _secondPos = _curPos;

        float width = Mathf.Abs(_secondPos.x - _firstPos.x) * 47f;
        float height = Mathf.Abs(_secondPos.y - _firstPos.y) * 47f;

        print(_firstPos.x + ", " + _firstPos.y);
        _mainCam.cullingMask = LayerMask.GetMask(_captureAllLayer);

        RenderTexture texture = new RenderTexture(Screen.width, Screen.height, 24);

        _mainCam.targetTexture = texture;

        Texture2D image = new Texture2D((int)width, (int)height, TextureFormat.RGB24, false);
        _mainCam.Render();
        RenderTexture.active = texture;
        image.ReadPixels(new Rect(_initPos.x + 657f, _initPos.y + 215.5f, width, height), 0, 0);
        image.Apply();

        byte[] bytes = image.EncodeToPNG();
        File.WriteAllBytes(name, bytes);
        Destroy(image);

        for (int i = 0; i <= 50; ++i)
        {
            string path = Application.persistentDataPath + "\\Capture" + i + ".png";

            if (!File.Exists(path))
            {
                File.WriteAllBytes(path, bytes);

                _mainCam.targetTexture = null;
                _mainCam.cullingMask = LayerMask.GetMask(_cameraMainLayer);

                // 라인 그리기를 다시 활성화
                //StaticManager.instance.scr_CaseManagementManager.DrawLineObj.SetActive(true);

                for (int j = 0; j < StaticManager.instance.scr_DrawCaptureArea.Length; ++j)
                {
                    StaticManager.instance.scr_DrawCaptureArea[j].captureMode = false;
                }
                print("부분 화면 스크린샷");

                break;
            }
        }

        StaticManager.instance.scr_SettingsManager.LoadImage();
    }

    public void CaptureAll()
    {
        _mainCam = CameraManager_Scr.instance.mainCam;
        _uiCam = CameraManager_Scr.instance.uiCam;

        _mainCam.cullingMask = LayerMask.GetMask(_captureAllLayer);
        StaticManager.instance.WorkStep_Canvas.GetComponent<Canvas>().worldCamera = _mainCam;

        RenderTexture texture = new RenderTexture(Screen.width, Screen.height, 24);

        _mainCam.targetTexture = texture;

        Texture2D image = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        _mainCam.Render();
        RenderTexture.active = texture;
        image.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        image.Apply();

        byte[] bytes = image.EncodeToPNG();
        File.WriteAllBytes(name, bytes);
        Destroy(image);

        for (int i = 0; i <= 50; ++i)
        {
            string path = Application.persistentDataPath + "\\Capture" + i + ".png";

            if (!File.Exists(path))
            {
                File.WriteAllBytes(path, bytes);
                _mainCam.targetTexture = null;
                _mainCam.cullingMask = LayerMask.GetMask(_cameraMainLayer);
                StaticManager.instance.WorkStep_Canvas.GetComponent<Canvas>().worldCamera = _uiCam;
                print("전체 화면 스크린샷");

                break;
            }
        }

        StaticManager.instance.scr_SettingsManager.LoadImage();
    }
}