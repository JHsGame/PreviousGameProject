using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotCaching_Scr : MonoBehaviour
{
    private ItemSlot_Scr slotScr;
    private ItemSlotButton_Scr buttonScr;

    [SerializeField]
    private Image i_BG;
    [SerializeField]
    private Image i_itemImage;  // �̹����� Ŭ���ϸ� ��ư �г��� ��������.
    [SerializeField]
    private GameObject g_CheckBox; // üũ�ڽ� �̹���
    [SerializeField]
    private Image i_CheckImage; // ����, �Ǹ� ��� ���� �� üũ �̹����� ������ �ϸ� �� �� ����. -> üũ �̹����� ����ĳ���� üũ ����. ���� ������ ģ���鸸 ���, �������� ��Ӱ� ������.
    [SerializeField]
    TextMeshProUGUI t_Text;

    public TextMeshProUGUI itemName { get => t_Text; }
    public Image itemBG { get => i_BG; }
    public Image itemImage { get => i_itemImage; set => i_itemImage = value; }
    public GameObject CheckBox { get => g_CheckBox; }
    public Image CheckImage { get => i_CheckImage; set => i_CheckImage = value; }

    public void SetScript(ItemSlot_Scr scr1, ItemSlotButton_Scr scr2)
    {
        slotScr = scr1;
        buttonScr = scr2;
    }
}