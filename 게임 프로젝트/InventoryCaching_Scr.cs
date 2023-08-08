using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCaching_Scr : MonoBehaviour
{
    private static InventoryCaching_Scr Instance;

    [SerializeField]
    private bool b_isSellMode = false;

    [SerializeField]
    private Transform itemListView;
    [SerializeField]
    private Transform heroListView;
    [SerializeField]
    private Transform itemSlotsParent;
    [SerializeField]
    private Transform heroSlotsParent;
    [SerializeField]
    private Transform statIconsParent;
    [SerializeField]
    private Transform statInfoParent;
    [SerializeField]
    private Sprite[] icons;
    [SerializeField]
    private Sprite[] stat_icons;
    [SerializeField]
    private Sprite[] bag_icons;
    [SerializeField]
    private Sprite[] Hero_icons;
    [SerializeField]
    private Sprite[] Tier_icons;
    [SerializeField]
    private Sprite[] SellOrBack_Icons;
    [SerializeField]
    private Sprite[] BoximageHero_icons;
    [SerializeField]
    private Image tier_Image;
    [SerializeField]
    private Image infoTier_Image;
    [SerializeField]
    private Image info_Image;
    [SerializeField]
    private Image SellOrBack_Image;
    [SerializeField]
    private TextMeshProUGUI info_Title;
    [SerializeField]
    private GameObject inventory_UI;    // 하단에 있는 인벤토리 아이콘.
    [SerializeField]
    private GameObject sold_UI;
    [SerializeField]
    private GameObject info_UI;    // 정보창
    [SerializeField]
    private GameObject UI_Title;
    [SerializeField]
    private GameObject Button_CombinationMode;
    [SerializeField]
    private GameObject Button_SellMode;
    [SerializeField]
    private GameObject Button_CombinationSell;
    [SerializeField]
    private GameObject Button_OK;
    [SerializeField]
    private GameObject Button_Sell;
    [SerializeField]
    private GameObject g_Warninginventory;
    [SerializeField]
    private GameObject g_WarninginventoryGem;

    public bool SellMode { get => b_isSellMode; set => b_isSellMode = value; }
    public Transform itemList { get => itemListView; }
    public Transform heroList { get => heroListView; }
    public Transform itemSlotParent { get => itemSlotsParent; }
    public Transform heroSlotParent { get => heroSlotsParent; }
    public Transform statParent { get => statIconsParent; }
    public Transform InfoParent { get => statInfoParent; }
    public Sprite[] itemIcons { get => icons; }
    public Sprite[] statIcons { get => stat_icons; }
    public Sprite[] bagIcons { get => bag_icons; }
    public Sprite[] Heroicons { get => Hero_icons; }
    public Sprite[] TierIcons { get => Tier_icons; }
    public Sprite[] SellOrBackIcons { get => SellOrBack_Icons; }
    public Sprite[] BoximageHeroicons { get => BoximageHero_icons; }
    public Image tierImage { get => tier_Image; }
    public Image infoImage { get => info_Image; }
    public Image infoTierImage { get => infoTier_Image; }
    public Image SellOrBackImage { get => SellOrBack_Image; }
    public TextMeshProUGUI infoTitle { get => info_Title; }
    public GameObject invenUI { get => inventory_UI; }
    public GameObject SoldUI { get => sold_UI; }
    public GameObject infoUI { get => info_UI; }
    public GameObject Title { get => UI_Title; }
    public GameObject CombinationModeButton { get => Button_CombinationMode; }
    public GameObject SellModeButton { get => Button_SellMode; }
    public GameObject DecideButton { get => Button_CombinationSell; }
    public GameObject SellButton { get => Button_Sell; }
    public GameObject OKButton { get => Button_OK; }
    public static InventoryCaching_Scr instance { get => Instance; }
    public GameObject warninginventory { get => g_Warninginventory; }
    public GameObject warninginventoryGem { get => g_WarninginventoryGem; }


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
}