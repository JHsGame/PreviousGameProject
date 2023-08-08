using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroSlotButton_Scr : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private HeroSlot_Scr slotScr;
    private HeroSlotCaching_Scr cachingScr;
    public bool b_isClick;
    float f_Time;


    public void SetScript(HeroSlot_Scr scr1, HeroSlotCaching_Scr scr2)
    {
        slotScr = scr1;
        cachingScr = scr2;
    }
    private void Update()
    {
        if (b_isClick)
        {
            if (Time.unscaledTime - f_Time >= 1f)
            {
                // ½ºÅ³ ÆË¾÷Ã¢ ¶ç¿ì±â
                slotScr.PopUpInfo();
            }
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        b_isClick = true;
        f_Time = Time.unscaledTime;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        b_isClick = false;
    }

    public void OnSelectSynthesis()
    {
        if (!slotScr.isEmpty)
        {
            SoundManager_sfx.instance.PlaySE("UI_Click1", false);

            if (!Synthesis_Scr.instance.isFull && SynthesisCaching_Scr.instance.SynthesisMode)
            {
                cachingScr.heroImage.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0f / 255f);
                Synthesis_Scr.instance.SetCombinationSlot(slotScr.heroTier, slotScr);
            }
        }
    }
}