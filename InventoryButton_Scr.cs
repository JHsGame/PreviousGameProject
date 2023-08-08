using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton_Scr : MonoBehaviour
{
    private static InventoryButton_Scr Instance;

    public static InventoryButton_Scr instance { get => Instance; }

    void Start()
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

    public void OnClickInventoryLevelUp()
    {
        // 일정 가격 이상일 때에만 구매 가능.
        if (GoldManager_Scr.instance.i_Gem >= Inventory_Scr.instance.InventoryPrice)
        {
            GoldManager_Scr.instance.i_Gem -= Inventory_Scr.instance.InventoryPrice;
            Inventory_Scr.instance.InventoryLevel++;

            Inventory_Scr.instance.InitInventory();
        }
    }

    public void OnClickSell()
    {
        if (InventoryCaching_Scr.instance.SellMode)
        {
            Inventory_Scr.instance.SellSlots();
            SoundManager_sfx.instance.PlaySE("Take2", false);
        }

        Inventory_Scr.instance.CalculateStats();
    }

    public void OnClickSellModeOnOff()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);
        if (!InventoryCaching_Scr.instance.SellMode)
        {
            Inventory_Scr.instance.ClearList();
            InventoryCaching_Scr.instance.SellMode = true;
            InventoryCaching_Scr.instance.SoldUI.SetActive(true);
            InventoryCaching_Scr.instance.SellButton.SetActive(true);
            InventoryCaching_Scr.instance.OKButton.SetActive(false);
            InventoryCaching_Scr.instance.SellOrBackImage.sprite = InventoryCaching_Scr.instance.SellOrBackIcons[1];
        }
        else
        {
            Inventory_Scr.instance.ClearList();
            InventoryCaching_Scr.instance.SellMode = false;
            InventoryCaching_Scr.instance.SoldUI.SetActive(false);
            InventoryCaching_Scr.instance.SellButton.SetActive(false);
            InventoryCaching_Scr.instance.OKButton.SetActive(true);
        }
    }

    public void OnClickInfoOK()
    {
        SoundManager_sfx.instance.PlaySE("UI_Click1", false);
        InventoryCaching_Scr.instance.infoUI.SetActive(false);
    }
}