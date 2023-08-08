using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SynthesisButton_Scr : MonoBehaviour
{
    private static SynthesisButton_Scr Instance;
    private Synthesis_Scr mainScr;
    private SynthesisCaching_Scr cachingScr;

    public static SynthesisButton_Scr instance { get => Instance; }

    private void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        else
        {
            Instance = this;
        }
    }

    public void SetScript(Synthesis_Scr scr1, SynthesisCaching_Scr scr2)
    {
        mainScr = scr1; cachingScr = scr2;
    }

    public void OnClickSynthesis()
    {
        if (cachingScr.SynthesisMode)
        {
            mainScr.activeFusionEffect();
            Player_Input.instance.g_ButtonShield.SetActive(true);
           // mainScr.ItemSynthesis();
        }

        Inventory_Scr.instance.CalculateStats();
    }

    public void OnClickHintWindowOn()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);

        cachingScr.Hint.SetActive(true);
    }

    public void OnClickHintWindowOff()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);

        cachingScr.Hint.SetActive(false);
    }

    public void OnClickHeroTab()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);
        if (InventoryCaching_Scr.instance.itemList.gameObject.activeSelf)
        {
            mainScr.ClearList(false);
            cachingScr.heroTab.GetComponent<Image>().color = new Color(155f / 255f, 155f / 255f, 155f / 255f, 255f / 255f);
            cachingScr.heroTab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            cachingScr.itemTab.GetComponent<Image>().color = new Color(65f / 255f, 65f / 255f, 65f / 255f, 255f / 255f);
            cachingScr.itemTab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(155f / 255f, 155f / 255f, 155f / 255f, 255f / 255f);

            InventoryCaching_Scr.instance.heroList.GetComponent<RectTransform>().anchoredPosition = new Vector2(60f, -190f);
            InventoryCaching_Scr.instance.heroList.GetComponent<RectTransform>().sizeDelta = new Vector2(700f, 490f);
            InventoryCaching_Scr.instance.heroList.gameObject.SetActive(true);
            InventoryCaching_Scr.instance.itemList.gameObject.SetActive(false);
        }
    }

    public void OnClickItemTab()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);
        if (InventoryCaching_Scr.instance.heroList.gameObject.activeSelf)
        {
            mainScr.ClearList(false);
            cachingScr.itemTab.GetComponent<Image>().color = new Color(155f / 255f, 155f / 255f, 155f / 255f, 255f / 255f);
            cachingScr.itemTab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            cachingScr.heroTab.GetComponent<Image>().color = new Color(65f / 255f, 65f / 255f, 65f / 255f, 255f / 255f);
            cachingScr.heroTab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(155f / 255f, 155f / 255f, 155f / 255f, 255f / 255f);

            InventoryCaching_Scr.instance.itemList.GetComponent<RectTransform>().anchoredPosition = new Vector2(60f, -190f);
            InventoryCaching_Scr.instance.itemList.GetComponent<RectTransform>().sizeDelta = new Vector2(700f, 490f);
            InventoryCaching_Scr.instance.itemList.gameObject.SetActive(true);
            InventoryCaching_Scr.instance.heroList.gameObject.SetActive(false);
        }
    }
}