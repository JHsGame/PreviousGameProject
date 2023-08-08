using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAniEvent : MonoBehaviour
{
    public static CamAniEvent instance;

    public GameObject CM_vcam1;
    public GameObject CM_vcam3;
    public GameObject Canvas;
    public bool Test = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            return;
    }

    public void EndCamAnim(int i) // i==0 start가 end i==1 end가 end
    {
        if (i == 0)
        {
            Canvas.transform.GetComponent<Button_Option>().CamAniStart_end();

        }
        else if(i == 1)
        {
            Canvas.transform.GetComponent<Button_Option>().CamAniEnd_end();
        }
        else if(i == 2)
        {
            Canvas.transform.GetComponent<Button_Option>().CamAniBoss_end();
        }
    }
    public void StartCamAnim()
    {
        Player_Input.instance.b_Unaiming = true;

        if (Button_Option.instance.Cam.transform.GetChild(3).gameObject != null && !Button_Option.instance.Cam.transform.GetChild(3).gameObject.activeSelf)
        {
            Button_Option.instance.Cam.transform.GetChild(3).gameObject.SetActive(true);
        }
        string[] MainCamlayer = new string[] { "Default", "TransparentFX", "Ignore Raycast", "Water", "Ball", "Block", "Piece", "Skill_SCV", "Skill_Police", "Skill_Tax", "Player", "test", "Wall", "Skill_Computer", "Can", "WarningLine", "CanLine", "BossSkillCol" };
        if (Button_Option.instance.Cam.GetComponent<Camera>().cullingMask != LayerMask.GetMask(MainCamlayer))
        {
            Button_Option.instance.Cam.GetComponent<Camera>().cullingMask = LayerMask.GetMask(MainCamlayer);
        }
    }

    public void CameraAniEnd()
    {
        Button_Option.instance.Cam.GetComponent<Animator>().enabled = true;
        //CM_vcam1.SetActive(false);
        // CM_vcam3.SetActive(true);
        //if (Button_Option.instance.b_InGame)
        //{
        //    Button_Option.instance.Cam.GetComponent<Animator>().enabled = false;
        //}
        //else
        //{
        //    Button_Option.instance.Cam.GetComponent<Animator>().enabled = true;
        //}
    }

    public void BossSceneEnd()
    {
        QuitManager_Scr.instance.b_BossScene = false;
        //Button_Option.instance.Warning();
        string[] UICamlayer = new string[] { "UI", "WorldCanvas", "EventMark", "BoxUI" };
        string[] MainCamlayer = new string[] { "Default", "TransparentFX", "Ignore Raycast", "Water", "Ball", "Block", "Piece", "Skill_SCV", "Skill_Police", "Skill_Tax", "Player", "test", "Wall", "Skill_Computer", "Can", "WarningLine", "CanLine", "BossSkillCol" };

        RespawnManager.instance.UICam.cullingMask = LayerMask.GetMask(UICamlayer);
        Button_Option.instance.Cam.GetComponent<Camera>().cullingMask = LayerMask.GetMask(MainCamlayer);
    }

    public void EndCamZoomIn()
    {
        string[] MainCamlayer = new string[] { "Nothing" };
        Button_Option.instance.Cam.GetComponent<Camera>().cullingMask = LayerMask.GetMask(MainCamlayer);
        Button_Option.instance.Cam.transform.GetChild(3).gameObject.SetActive(false);
    }

    public void EndCamZoomOut()
    {
        string[] MainCamlayer = new string[] { "Default", "TransparentFX", "Ignore Raycast", "Water", "Ball", "Block", "Piece", "Skill_SCV", "Skill_Police", "Skill_Tax", "Player", "test", "Wall", "Skill_Computer", "Can", "WarningLine", "CanLine", "BossSkillCol" };
        Button_Option.instance.Cam.GetComponent<Camera>().cullingMask = LayerMask.GetMask(MainCamlayer);
        Button_Option.instance.Cam.transform.GetChild(3).gameObject.SetActive(true);
    }

    public void AniTest()
    {
        Test = false;
    }
}
