using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityVolumeRendering;

public class MenuTree_Scr : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _childMenus;
    [SerializeField]
    private GameObject[] _panelGroup;   // 0은 부모, 1은 자식 패널
    [SerializeField]
    private GameObject _textModePanel;
    [SerializeField]
    private GameObject _sliderPanel;

    [SerializeField]
    private Image _treeButton;  // 맨 우측에 있는 화살표 이미지
    [SerializeField]
    private Image _iconImage;   // 맨 좌측에 있는 이미지

    [SerializeField]
    private Sprite[] _buttonImages;

    private bool _isParent = false;
    private bool _treeOnOff;

    List<GameObject> _childMenuList = new List<GameObject>();

    public bool IsParent
    {
        set => _isParent = value;
        get => _isParent;
    }

    public List<GameObject> ChildMenuList
    {
        get => _childMenuList;
    }

    private void Start()
    {
        if (_childMenus.Length > 0)
        {
            _isParent = true;
            _treeButton.gameObject.SetActive(true);

            for(int i = 0; i < _childMenus.Length; ++i)
            {
                _childMenuList.Add(_childMenus[i]);
            }
        }

        PanelOnOff();
    }

    public void AddSubMenus(GameObject obj)
    {
        _childMenuList.Add(obj);

        if (!_isParent)
        {
            _isParent = !_isParent;
            PanelOnOff();
        }
    }

    public void PanelOnOff()
    {
        // 0이 부모, 1이 자식.
        _panelGroup[0].SetActive(_isParent);
        _panelGroup[1].SetActive(!_isParent);

        // 프리셋 세팅이 텍스트인지 슬라이더인지.
        _textModePanel.SetActive(StaticManager.instance.scr_PreSet.PresetMode);
        _sliderPanel.SetActive(!StaticManager.instance.scr_PreSet.PresetMode);
    }

    // CT만 로드했을 때의 캔버스에서 해당 OTF를 켜줄 지 말 지를 결정하는 함수.
    public void OnClickCTOTF()
    {
        StaticManager.instance.scr_PreSet.CTOTF.SetActive(!StaticManager.instance.scr_PreSet.CTOTF.activeSelf);

        if (StaticManager.instance.scr_PreSet.CTOTF.activeSelf)
        {
            StaticManager.instance.RuntimeOTF[0].ShowWindow(TestController.instance.myVolumeObj);
        }
        /*
        else
        {
            StaticManager.instance.RuntimeOTF[0].CloseWindow();
        }*/
    }

    public void OnClickSaveOTF()
    {
        RuntimeFileBrowser.ShowSaveFileDialog((RuntimeFileBrowser.DialogResult result) =>
        {
            if (!result.cancelled)
            {
                TransferFunctionDatabase.SaveTransferFunction(StaticManager.instance.RuntimeOTF[0].curTF, result.path);
            }
        });
    }

    public void OnClickTreeOnOff(GameObject obj)
    {   
        _treeOnOff = !_treeOnOff;

        if (_treeOnOff)
        {
            _treeButton.sprite = _buttonImages[0];
        }
        else
        {
            _treeButton.sprite = _buttonImages[1];
        }

        MenuTree_Scr scr = obj.GetComponent<MenuTree_Scr>();

        for (int i = 0; i < scr.ChildMenuList.Count; ++i)
        {
            scr.ChildMenuList[i].SetActive(!_treeOnOff);
        }
    }
}
