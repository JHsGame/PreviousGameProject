using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpEft_Scr : MonoBehaviour
{
    Animator ani;
    public TextMeshProUGUI t_Level;

    string Hp;
    string s_HpString;
    private void Start()
    {
        ani = this.GetComponent<Animator>();
    }
    // 이전 레벨 텍스트 표기
    public void BeforeLevelText()
    {
        t_Level.text = (ManPowerManager_Scr.instance.i_Level - 1).ToString();
    }
    // 레벨업 후 텍스트 표기
    public void LevelTextUp()
    {
        t_Level.text = ManPowerManager_Scr.instance.i_Level.ToString();
        if(ManPowerManager_Scr.instance.i_Level > 1)
            Hp = ((int)HPManager_Scr.instance.HpData[ManPowerManager_Scr.instance.i_Level - 1]["HP"] - (int)HPManager_Scr.instance.HpData[ManPowerManager_Scr.instance.i_Level - 2]["HP"]).ToString();
        string str = TranslateManager_Scr.instance.TranslateContext(43);
        string[] strs = str.Split('+');
        str = strs[0];
        str += ". +";
        s_HpString = string.Concat(str + Hp);
        //s_HpString = string.Concat("체력. +", Hp);
        if (!ManPowerManager_Scr.instance.b_isUp)
        {
            ManPowerManager_Scr.instance.t_LevelUpStat.text = s_HpString;
        }
        else
        {
            ManPowerManager_Scr.instance.t_LevelUpStat.text = TranslateManager_Scr.instance.TranslateContext(64);
            ManPowerManager_Scr.instance.t_LevelUpStat.text += string.Concat(". +1 / ", s_HpString);
            ManPowerManager_Scr.instance.b_isUp = false;
        }
        Button_Option.instance.b_isLvUp = false;
    }

    private void OnDisable()
    {
        Button_Option.instance.b_isLvUp = false;
    }
}
