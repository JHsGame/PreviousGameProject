using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SynthesisSlot_Scr : MonoBehaviour
{
    public GameObject itemSlot;
    public GameObject heroSlot;

    private int i_csvNum;
    private int i_synthesisLevel;
    [SerializeField]
    private int i_synthesisPoint;
    private bool b_isHero;

    [SerializeField]
    private bool b_isEmpty = true;

    [SerializeField]
    private ItemSlot_Scr myItem;
    [SerializeField]
    private HeroSlot_Scr myHero;



    public int SynthesisPoint { get => i_synthesisPoint; set => i_synthesisPoint = value; }
    public int SynthesisLevel { get => i_synthesisLevel; set => i_synthesisLevel = value; }
    public ItemSlot_Scr item { get => myItem; }
    public HeroSlot_Scr hero { get => myHero; }
    public bool isHero { get => b_isHero; }
    public int CsvNum { get => i_csvNum; }
    public bool isEmpty { get => b_isEmpty; set => b_isEmpty = value; }

    public ParticleSystem par_Particle;
    public Image myBG;
    public Image myHeroBG;
    public Image myItemBG;
    public Image myHeroImage;
    public Image myItemImage;

    private void OnEnable()
    {
        b_isEmpty = true;
        myBG.enabled = true;
        myHeroBG.enabled = true;
        myItemBG.enabled = true;

        myItemImage.enabled = true;
        myHeroImage.enabled = true;
    }

    public void invokeEffect()
    {
        par_Particle.Play();
        delayeffect();
        //Invoke("delayeffect", 1.0f);
    }

    public void delayeffect()
    {
        myBG.enabled = false;
        myHeroBG.enabled = false;
        myItemBG.enabled = false;

        myItemImage.enabled = false;
        myHeroImage.enabled = false;
    }

    public void resultSynthesis(bool isHero, int CSVnum)
    {
        if (isHero)
        {
            itemSlot.SetActive(false);
            heroSlot.SetActive(true);
            myHeroImage.sprite = SynthesisCaching_Scr.instance.heroIcons[CSVnum];
        }
        else
        {
            itemSlot.SetActive(true);
            heroSlot.SetActive(false);
            myItemImage.sprite = InventoryCaching_Scr.instance.itemIcons[CSVnum];
        }
        myItemImage.enabled = true;
        myHeroImage.enabled = true;
    }
    public void SetInfo(bool isHero, ItemSlot_Scr _itemSlot, HeroSlot_Scr _heroSlot)
    {
        if (_itemSlot == null)
        {
            i_csvNum = _heroSlot.csvNum;
        }
        else if(_heroSlot == null)
        {
            i_csvNum = _itemSlot.csvNum;
        }

        b_isHero = isHero;
        if (isHero)
        {
            itemSlot.SetActive(false);
            heroSlot.SetActive(true);
            heroSlot.GetComponent<HeroSlot_Scr>().LoadInfo(i_csvNum, _heroSlot.heroSlot);
            myHero = heroSlot.GetComponent<HeroSlot_Scr>();
        }
        else
        {
            itemSlot.SetActive(true);
            heroSlot.SetActive(false);
            itemSlot.GetComponent<ItemSlot_Scr>().LoadInfo(_itemSlot.csvNum);
            myItem = heroSlot.GetComponent<ItemSlot_Scr>();
        }
        myItemImage.enabled = true;
        myHeroImage.enabled = true;
        b_isEmpty = false;
    }

    public void OnClickReject()
    {
        if (!b_isEmpty)
        {
            Synthesis_Scr.instance.RejectCombination(this, false);
            b_isEmpty = true;
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        i_synthesisPoint = i_synthesisLevel = 0;
        i_csvNum = -1;
        myItem = null; myHero = null;
        b_isHero = false;
        b_isEmpty = true;

        itemSlot.SetActive(false);
        heroSlot.SetActive(false);
    }
}
