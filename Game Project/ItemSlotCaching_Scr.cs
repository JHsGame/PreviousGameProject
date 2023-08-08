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
    private Image i_itemImage;  // 이미지를 클릭하면 버튼 패널이 나오도록.
    [SerializeField]
    private GameObject g_CheckBox; // 체크박스 이미지
    [SerializeField]
    private Image i_CheckImage; // 조합, 판매 모드 선택 시 체크 이미지만 나오게 하면 될 것 같음. -> 체크 이미지는 레이캐스팅 체크 해제. 선택 가능한 친구들만 밝게, 나머지는 어둡게 해주자.
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