using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroSlot_Scr : MonoBehaviour
{
    [SerializeField]
    HeroSlotButton_Scr buttonScr;
    HeroSlotCaching_Scr cachingScr;

    public GameObject heroSlot;

    [SerializeField]
    private bool b_isEmpty = true;
    private int i_slotNum;
    private int i_csvNum;
    [SerializeField]
    private int i_Tier;

    public bool isEmpty { get => b_isEmpty; set => b_isEmpty = value; }
    public int slotNum { get => i_slotNum; }
    public int csvNum { get => i_csvNum; }
    public int heroTier { get => i_Tier; set => i_Tier = value; }


    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        cachingScr = this.GetComponent<HeroSlotCaching_Scr>();

        buttonScr.SetScript(this, cachingScr);
        cachingScr.SetScript(this, buttonScr);
        i_slotNum = transform.GetSiblingIndex() + 1;    // 각 슬롯 번호는 1번부터

        StartCoroutine(UpdateCoroutine());
    }

    IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            if (b_isEmpty)
            {
                buttonScr.enabled = false;
            }
            else
            {
                buttonScr.enabled = true;
            }

            yield return CoroutineManager_Scr.WaitForEndOfFrame;
        }
    }

    public void PopUpInfo()
    {
        //Synthesis_Scr.instance.OpenBoxScr.setInfo_Hero(csvNum, i_Tier, true);
        OpenBox.instance.setInfo_Hero(csvNum, i_Tier - 1, true);
    }

    public void LoadInfo(int num, GameObject obj)
    {
        i_csvNum = num;

        cachingScr.heroImage.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
        cachingScr.heroImage.sprite = SynthesisCaching_Scr.instance.heroIcons[num];
        i_Tier = obj.GetComponent<ManStat_Scr>().i_HeroTier;
        heroSlot = obj;

        b_isEmpty = false;
    }

    public void ResetSlot()
    {
        heroSlot = null;
        i_Tier = 0;
        cachingScr.heroImage.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0f / 255f);
        cachingScr.heroImage.sprite = null;
        b_isEmpty = true;
    }
}
