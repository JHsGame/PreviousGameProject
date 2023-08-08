using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    public int i_SpeechNum = 0;

    public bool b_isSpeech = false;
    public bool b_hitSpeech = false;

    public bool b_forBulldog = false;

    public float f_Changetime = 0.0f;

    public GameObject go_SpeechText;
    public GameObject go_Background;
    public GameObject go_TextBase;
    public GameObject go_TartgetBlockPos;
    public GameObject go_TartgetBlock;
    public bool b_ChangedSize = false;
    public bool b_BossProduction = false;
    public Canvas maincan;
    public TextAsset t_Script_BossCSV;
    public List<Dictionary<string, object>> Script_Boss;
    public TextAsset t_Script_MobCSV;
    public List<Dictionary<string, object>> Script_MobData;
    public TextAsset t_Script_MobDamageCSV;
    public List<Dictionary<string, object>> Script_MobDamageData;
    public TextAsset t_Script_TranslateCSV;
    public List<Dictionary<string, object>> Script_TranslateData;
    public Vector3 vec_FirstPos;
    

    void Awake()
    {
        vec_FirstPos = transform.localPosition;
        if (!b_forBulldog)
        {
            Script_Boss = CSVReader.Read(t_Script_BossCSV);
            Script_MobData = CSVReader.Read(t_Script_MobCSV);
            Script_MobDamageData = CSVReader.Read(t_Script_MobDamageCSV);
            Invoke("DelayStart", 0.1f);
        }
        else
        {
            Script_TranslateData = CSVReader.Read(t_Script_TranslateCSV);
            Invoke("DelayStart", 0.1f);
            Invoke("unable", 0.5f);
        }
    }
    public void Delay()
    {
        Invoke("DelayStart", 0.1f);
    }

    public void DelayStart()
    {
        if (b_hitSpeech)
        {
            int randomScript = Random.Range(0, 30);
            go_SpeechText.GetComponent<TextMeshProUGUI>().text = Script_MobDamageData[randomScript][TranslateManager_Scr.instance.s_Language].ToString();
        }
        else
        {
            if (!b_forBulldog)
            {
                go_SpeechText.GetComponent<TextMeshProUGUI>().text = Script_MobData[RespawnManager.instance.ScriptList[RespawnManager.instance.i_SpeechCount]][TranslateManager_Scr.instance.s_Language].ToString();
                RespawnManager.instance.i_SpeechCount++;
            }
            else if (b_forBulldog)
            {
                go_SpeechText.GetComponent<TextMeshProUGUI>().text = Script_TranslateData[135][TranslateManager_Scr.instance.s_Language].ToString();
            }
        }
    }

    void unable()
    {
        gameObject.SetActive(false);
    }

    public void StartSpeech(int PlaySENUM, int blockKinds)
    {
        if(PlaySENUM != 3)
            SoundManager_Voice.instance.PlaySE_block(PlaySENUM, blockKinds);

        var pos = Vector2.zero;
        Camera uiCamera = Button_Option.instance.Cam.GetComponent<Camera>();
        Camera worldCamera = Button_Option.instance.Cam.GetComponent<Camera>();
        var canvasRect = maincan.GetComponent<RectTransform>();
        var screenPos = RectTransformUtility.WorldToScreenPoint(worldCamera, go_TartgetBlockPos.transform.position);    
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, uiCamera, out pos);
        transform.GetComponent<RectTransform>().localPosition = pos;
        b_isSpeech = true;
        if (!b_hitSpeech)
        {
            Invoke("Timeover", 3f);
        }
        else
        {
            Invoke("Timeover", 2f);
        }
    }

    public void Timeover()
    {
        CancelInvoke("Timeover");
        transform.GetChild(0).GetComponent<SpeechCoillder>().Disable();
        // 쓰이지 않을 때는 안보이는 곳으로 위치 이동
        transform.GetComponent<RectTransform>().anchoredPosition = vec_FirstPos;
        if(go_TartgetBlock != null)
        {
            go_TartgetBlock.transform.GetComponent<BlockControl>().b_isSpeech = false;
        }

        if(go_TartgetBlock != null)
            go_TartgetBlock.transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetBool("isSpeaking", false);

        go_TartgetBlockPos = null;
        go_TartgetBlock = null;
        b_ChangedSize = false;
        go_TextBase.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 25);
        go_Background.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 25);
        go_Background.GetComponent<SpeechCoillder>().go_CoillderBox = null;

        if (!b_forBulldog)
        {
            RespawnManager.instance.i_SpeechCount++;
            if (b_hitSpeech)
            {
                int randomScript = Random.Range(0, 10);

                go_SpeechText.GetComponent<TextMeshProUGUI>().text = Script_MobDamageData[randomScript][TranslateManager_Scr.instance.s_Language].ToString();
            }
            else
            {
                // 여기가 문제뜨네
                go_SpeechText.GetComponent<TextMeshProUGUI>().text = Script_MobData[RespawnManager.instance.ScriptList[RespawnManager.instance.i_SpeechCount]][TranslateManager_Scr.instance.s_Language].ToString();
            }
        }
        if (!b_BossProduction)
            go_Background.GetComponent<RectTransform>().sizeDelta = new Vector2((go_TextBase.transform.GetChild(0).GetComponent<RectTransform>().rect.width / 3), go_Background.GetComponent<RectTransform>().sizeDelta.y);
        else
            go_Background.GetComponent<RectTransform>().sizeDelta = new Vector2((go_TextBase.GetComponent<RectTransform>().rect.width / 3) * 1.7f, go_Background.GetComponent<RectTransform>().sizeDelta.y);
        
        b_isSpeech = false;

        if (b_forBulldog)
        {
            go_SpeechText.GetComponent<TextMeshProUGUI>().text = Script_TranslateData[135][TranslateManager_Scr.instance.s_Language].ToString();
            gameObject.SetActive(false);
        }
    }

    public void setSize()
    {
        if (!b_BossProduction)
            go_Background.GetComponent<RectTransform>().sizeDelta = new Vector2((go_TextBase.transform.GetChild(0).GetComponent<RectTransform>().rect.width / 3), go_Background.GetComponent<RectTransform>().sizeDelta.y);
        else
            go_Background.GetComponent<RectTransform>().sizeDelta = new Vector2((go_TextBase.GetComponent<RectTransform>().rect.width / 3) * 1.7f, go_Background.GetComponent<RectTransform>().sizeDelta.y);

    }

    public void BossText(int num)
    {
        switch (num)
        {
            case 10:
                go_SpeechText.GetComponent<TextMeshProUGUI>().text = Script_Boss[0][TranslateManager_Scr.instance.s_Language].ToString();
                break;

            case 11:
                go_SpeechText.GetComponent<TextMeshProUGUI>().text = Script_Boss[1][TranslateManager_Scr.instance.s_Language].ToString();
                break;

            case 12:
                go_SpeechText.GetComponent<TextMeshProUGUI>().text = Script_Boss[2][TranslateManager_Scr.instance.s_Language].ToString();
                break;

            case 13:
                go_SpeechText.GetComponent<TextMeshProUGUI>().text = Script_Boss[3][TranslateManager_Scr.instance.s_Language].ToString();
                break;
        }
    }

    public void BossSkillText()
    {
        int i_TextNum = Random.Range(4, 16);
        string str = Script_Boss[i_TextNum][TranslateManager_Scr.instance.s_Language].ToString();
        string[] split = str.Split('_');

        if (split.Length > 1)
        {
            str = split[0] + "\n" + split[1];
        }
        go_SpeechText.GetComponent<TextMeshProUGUI>().text = str;
    }

    private void Update()
    {
        if (Button_Option.instance.b_Golobby)
        {
            transform.GetComponent<RectTransform>().localPosition = new Vector2(2000, 0);
        }
        else
        {

            if (go_TartgetBlock == null && !b_isSpeech)
            {
                setSize();
            }

            if ((go_TartgetBlock != null && b_isSpeech) || (b_forBulldog && gameObject.activeSelf))
            {
                var pos = Vector2.zero;
                Camera uiCamera = Button_Option.instance.Cam.GetComponent<Camera>();
                Camera worldCamera = Button_Option.instance.Cam.GetComponent<Camera>();
                var canvasRect = maincan.GetComponent<RectTransform>();
                var screenPos = RectTransformUtility.WorldToScreenPoint(worldCamera, go_TartgetBlockPos.transform.position);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, uiCamera, out pos);
                SpeechCoillder Script = transform.GetChild(0).GetComponent<SpeechCoillder>();
                if (Script.b_MoveDown)
                {
                    transform.GetComponent<RectTransform>().localPosition = new Vector2(pos.x, pos.y - 33);
                }
                else if (Script.b_MoveUp)
                {
                    transform.GetComponent<RectTransform>().localPosition = new Vector2(pos.x, pos.y + 33);
                }
                else
                {
                    transform.GetComponent<RectTransform>().localPosition = pos;
                }
                if (f_Changetime > 0 && Button_Option.instance.b_WindowOn)
                {
                    f_Changetime -= Time.unscaledDeltaTime;
                    if(f_Changetime <= 0)
                    {
                        go_TextBase.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 23.7f);
                        ChangeText();
                    }
                }
                if (!b_ChangedSize)
                    ChangeText();

            }

            if(go_TartgetBlock != null && go_TartgetBlock.activeSelf == false)
            {
                Timeover();
            }
        }
    }

    void ChangeText()
    {
        b_ChangedSize = true;
        float BodyWidth = go_TextBase.GetComponent<RectTransform>().rect.width;
        float PosX = transform.GetComponent<RectTransform>().anchoredPosition.x;


        if (PosX > 0)
        {
            if (PosX + ((BodyWidth * 0.1f) * 0.5f) > 540)
            {
                float X = PosX + ((BodyWidth * 0.1f) * 0.55f) - 540;
                float PosXX = go_TextBase.GetComponent<RectTransform>().anchoredPosition.x - X;
                transform.GetChild(0).GetComponent<SpeechCoillder>().f_ChangePosX = PosXX;
                go_TextBase.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(PosXX, 23.7f);
            }
        }
        else if (PosX < 0)
        {
            if (Mathf.Abs(PosX - ((BodyWidth * 0.1f) * 0.5f)) > 540)
            {
                float X = Mathf.Abs(PosX - ((BodyWidth * 0.1f) * 0.55f)) - 540;
                float PosXX = go_TextBase.GetComponent<RectTransform>().anchoredPosition.x + X;
                transform.GetChild(0).GetComponent<SpeechCoillder>().f_ChangePosX = PosXX;
                go_TextBase.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(PosXX, 23.7f);
            }
        }
        setSize();
    }
}