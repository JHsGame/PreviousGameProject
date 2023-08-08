using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MeasurementProperty : MonoBehaviour
{

    [SerializeField] GameObject g_MeasurementProperty;

    [SerializeField] TextMeshProUGUI tmpro_fontsize;
    [SerializeField] Slider slider_fontsize;

    [SerializeField] Dropdown dropdown_color;
    [SerializeField] Image image_dropdownColor;

    [SerializeField] Image image_colorPicker;
    [SerializeField] TMP_InputField tmpro_colorPickerR;
    [SerializeField] TMP_InputField tmpro_colorPickerG;
    [SerializeField] TMP_InputField tmpro_colorPickerB;
    [SerializeField] GameObject g_colorPicker;

    private void Update()
    {
        tmpro_fontsize.text = Mathf.Round(slider_fontsize.value).ToString();

        switch (dropdown_color.value)
        {
            case 0:// 검정
                image_dropdownColor.color = new Color(0 / 255, 0 / 255, 0 / 255);
                image_colorPicker.color = new Color(0 / 255, 0 / 255, 0 / 255);
                tmpro_colorPickerR.text = "0";
                tmpro_colorPickerG.text = "0";
                tmpro_colorPickerB.text = "0";
                g_colorPicker.SetActive(false);
                break;
            case 1:// 빨강
                image_dropdownColor.color = new Color(255f / 255f, 0 / 255f, 0 / 255f);
                image_colorPicker.color = new Color(255f / 255f, 0 / 255f, 0 / 255f);
                tmpro_colorPickerR.text = "255";
                tmpro_colorPickerG.text = "0";
                tmpro_colorPickerB.text = "0";
                g_colorPicker.SetActive(false);
                break;
            case 2:// 그레이
                image_dropdownColor.color = new Color(128f / 255f, 128f / 255f, 128f / 255f);
                image_colorPicker.color = new Color(128f / 255f, 128f / 255f, 128f / 255f);
                tmpro_colorPickerR.text = "128";
                tmpro_colorPickerG.text = "128";
                tmpro_colorPickerB.text = "128";
                g_colorPicker.SetActive(false);
                break;
            case 3:// 블루
                image_dropdownColor.color = new Color(0 / 255, 0 / 255, 255 / 255);
                image_colorPicker.color = new Color(0 / 255, 0 / 255, 255 / 255);
                tmpro_colorPickerR.text = "0";
                tmpro_colorPickerG.text = "0";
                tmpro_colorPickerB.text = "255";
                g_colorPicker.SetActive(false);
                break;
            case 4:// 핑크
                image_dropdownColor.color = new Color(255f / 255f, 51f / 255f, 153f / 255f);
                image_colorPicker.color = new Color(255f / 255f, 51f / 255f, 153f / 255f);
                tmpro_colorPickerR.text = "255";
                tmpro_colorPickerG.text = "51";
                tmpro_colorPickerB.text = "153";
                g_colorPicker.SetActive(false);
                break;
            case 5:// 커스텀
                g_colorPicker.SetActive(true);
                break;
        }
    }

    public void button_apply()
    {
        g_MeasurementProperty.SetActive(false);
    }
    public void button_applyAll()
    {
        g_MeasurementProperty.SetActive(false);
    }
    public void button_saveDefault()
    {
        g_MeasurementProperty.SetActive(false);
    }
    public void button_cancel()
    {
        g_MeasurementProperty.SetActive(false);
    }

    // 커스텀 색상 고르는 버튼의 Use 사용
    public void colorPickerUse()
    {
        float.TryParse(tmpro_colorPickerR.text, out float R);
        float.TryParse(tmpro_colorPickerG.text, out float G);
        float.TryParse(tmpro_colorPickerB.text, out float B);
        image_colorPicker.color = new Color(R / 255f, G / 255f, B / 255f);
        image_dropdownColor.color = new Color(R / 255f, G / 255f, B / 255f);

        g_colorPicker.SetActive(false);
    }
}
