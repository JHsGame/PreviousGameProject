using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ItemSlotButton_Scr : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private ItemSlot_Scr slotScr;
    private ItemSlotCaching_Scr cashingScr;
    public bool b_isClick;
    float f_Time;

    //public UnityEvent PointerDown;
    //public UnityEvent PointerUp;

    public void SetScript(ItemSlot_Scr scr1, ItemSlotCaching_Scr scr2)
    {
        slotScr = scr1;
        cashingScr = scr2;
    }

    private void Update()
    {
        if (b_isClick)
        {
            if (Time.unscaledTime - f_Time >= 1f)
            {
                // ��ų �˾�â ����
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

    public void OnSelectCombinationOrSell()
    {
        if (slotScr.item != null)
        {
            SoundManager_sfx.instance.PlaySE("UI_Click1", false);
            // ���� Ȥ�� �Ǹ� ����� ������ ������ �����ϰ�, �ش� ���鿡 ���� �����̳� �Ǹſ� ������ ����.
            if (InventoryCaching_Scr.instance.SellMode || SynthesisCaching_Scr.instance.SynthesisMode)
            {
                if (!slotScr.isSelected)
                {
                    slotScr.isSelected = true;

                    if (!Synthesis_Scr.instance.isFull && SynthesisCaching_Scr.instance.SynthesisMode)
                    {
                        Synthesis_Scr.instance.SetCombinationSlot(slotScr.itemLevel, slotScr);
                    }
                    else if (InventoryCaching_Scr.instance.SellMode)
                    {
                        slotScr.CheckImageOn();
                        Inventory_Scr.instance.SetSellSlot(slotScr);
                        Inventory_Scr.instance.SoldGold += slotScr.soldGold;
                    }
                }
                else
                {
                    slotScr.isSelected = false;

                    if (InventoryCaching_Scr.instance.SellMode)
                    {
                        Inventory_Scr.instance.RejectSell(slotScr);
                        Inventory_Scr.instance.SoldGold -= slotScr.soldGold;
                    }
                }
            }
            else
            {
                if (slotScr.item != null)
                {
                    slotScr.PopUpInfo();
                }
            }
        }
    }
}