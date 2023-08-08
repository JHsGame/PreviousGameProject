using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawCaptureArea : MonoBehaviour
{
    [SerializeField]
    private Sprite _sprite;

    private LineRenderer lineRenderer;
    private Vector3 mousePos;
    private Vector2 initMousePosition, currentMousePosition;

    private bool _isCapturemode;

    public bool captureMode
    {
        get { return _isCapturemode; }
        set { _isCapturemode = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (captureMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                lineRenderer.enabled = true;
                lineRenderer.positionCount = 4;
                mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20f);
                initMousePosition = CameraManager_Scr.instance.mainCam.ScreenToWorldPoint(mousePos);
                lineRenderer.SetPosition(0, new Vector3(initMousePosition.x, initMousePosition.y, -5f));
                lineRenderer.SetPosition(1, new Vector3(initMousePosition.x, initMousePosition.y, -5f));
                lineRenderer.SetPosition(2, new Vector3(initMousePosition.x, initMousePosition.y, -5f));
                lineRenderer.SetPosition(3, new Vector3(initMousePosition.x, initMousePosition.y, -5f));
            }

            if (Input.GetMouseButton(0))
            {
                mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20f);
                currentMousePosition = CameraManager_Scr.instance.mainCam.ScreenToWorldPoint(mousePos);
                lineRenderer.SetPosition(0, new Vector3(initMousePosition.x, initMousePosition.y, -5f));
                lineRenderer.SetPosition(1, new Vector3(initMousePosition.x, currentMousePosition.y, -5f));
                lineRenderer.SetPosition(2, new Vector3(currentMousePosition.x, currentMousePosition.y, -5f));
                lineRenderer.SetPosition(3, new Vector3(currentMousePosition.x, initMousePosition.y, -5f));
            }

            if (Input.GetMouseButtonUp(0))
            {
                // 사각형을 지우고, 해당 그리기 영역만큼을 스크린샷 찍도록 하자.
                lineRenderer.enabled = false;
                StaticManager.instance.scr_ScreenCapture.CaptureArea(lineRenderer.GetPosition(0), lineRenderer.GetPosition(2));
            }
        }
    }

    // 버튼을 누르자마자 영역을 선택한 판정이 있어서 해당 부분을 막기위한 함수
    public void DelayOn()
    {
        Invoke("CaptureModeOn", 0.1f);
    }

    private void CaptureModeOn()
    {
        _isCapturemode = true;
    }

    public void ChangeThumbnail(Sprite _sprite)
    {
        float width = 0;
        StaticManager.instance.ThumbnailText.gameObject.SetActive(false);
        if (_sprite.texture.width >= 1920)
        {
            width = _sprite.rect.width / 3;
        }
        else
        {
            width = _sprite.rect.width;
        }
        StaticManager.instance.ThumbnailImage.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        StaticManager.instance.ThumbnailImage.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 454f);
        StaticManager.instance.ThumbnailImage.sprite = _sprite;
    }

    public void ResetThumbnail()
    {
        StaticManager.instance.ThumbnailImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(-10.5f, 13.5f);
        StaticManager.instance.ThumbnailImage.GetComponent<RectTransform>().sizeDelta = new Vector2(160f, 161f);
        StaticManager.instance.ThumbnailImage.sprite = _sprite;
        StaticManager.instance.ThumbnailText.gameObject.SetActive(true);
    }
}