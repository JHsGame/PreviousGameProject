using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SynthesisCaching_Scr : MonoBehaviour
{
    private static SynthesisCaching_Scr Instance;
    private SynthesisButton_Scr buttonScr;
    private Synthesis_Scr mainScr;

    [SerializeField]
    private GameObject hintWindow;
    [SerializeField]
    private bool b_isSynthesisMode = false;
    [SerializeField]
    private Sprite[] s_heroIcons;
    [SerializeField]
    private Transform synthesis_Parent;
    [SerializeField]
    private GameObject synthesis_UI;
    [SerializeField]
    private GameObject hero_Tab;
    [SerializeField]
    private GameObject item_Tab;
    [SerializeField]
    private Button synthesis_Button;
    [SerializeField]
    private TextMeshProUGUI t_Title;
    [SerializeField]
    private TextMeshProUGUI t_Explanation;
    [SerializeField]
    private TextMeshProUGUI t_Okbutton;


    [SerializeField]
    private TextMeshProUGUI t_synthesis;
    [SerializeField]
    private TextMeshProUGUI t_heroTab;
    [SerializeField]
    private TextMeshProUGUI t_itemTab;
    [SerializeField]
    private TextMeshProUGUI t_synthesisButton;


    public GameObject Hint { get => hintWindow; }
    public Sprite[] heroIcons { get => s_heroIcons; }
    public Transform synthesisParent { get => synthesis_Parent; }
    public GameObject synthesisUI { get => synthesis_UI;}
    public GameObject heroTab { get => hero_Tab; }
    public GameObject itemTab { get => item_Tab; }
    public Button synthesisButton { get => synthesis_Button; }
    public TextMeshProUGUI Title { get => t_Title; set => t_Title = value; }
    public TextMeshProUGUI Explanation { get => t_Explanation; set => t_Explanation = value; }
    public TextMeshProUGUI Okbutton { get => t_Okbutton; set => t_Okbutton = value; }


    public TextMeshProUGUI synthesis_t { get => t_synthesis; }
    public TextMeshProUGUI heroTab_t { get => t_heroTab; }
    public TextMeshProUGUI itemTab_t { get => t_itemTab; }
    public TextMeshProUGUI synthesisButton_t { get => t_synthesisButton; }


    public bool SynthesisMode { get => b_isSynthesisMode; set => b_isSynthesisMode = value; }
    public static SynthesisCaching_Scr instance { get => Instance; }

    public GameObject SynthesisEffect;
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

    public void SetScript(Synthesis_Scr scr1, SynthesisButton_Scr scr2)
    {
        mainScr = scr1; buttonScr = scr2;
    }
}
