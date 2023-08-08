using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroSlotCaching_Scr : MonoBehaviour
{
    private HeroSlot_Scr slotScr;
    private HeroSlotButton_Scr buttonScr;

    [SerializeField]
    private Image i_BG;
    [SerializeField]
    private Image i_heroImage;  // 이미지를 클릭하면 버튼 패널이 나오도록.
    [SerializeField]
    TextMeshProUGUI t_Text;

    public TextMeshProUGUI itemName { get => t_Text; }
    public Image itemBG { get => i_BG; }
    public Image heroImage { get => i_heroImage; set => i_heroImage = value; }

    public void SetScript(HeroSlot_Scr scr1, HeroSlotButton_Scr scr2)
    {
        slotScr = scr1;
        buttonScr = scr2;
    }
}
