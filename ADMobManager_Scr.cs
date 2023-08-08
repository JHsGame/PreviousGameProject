using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using TMPro;

public class ADMobManager_Scr : MonoBehaviour
{
    public static ADMobManager_Scr instance;
    private UnifiedNativeAd adNative;   // 네이티브 광고

    public GameObject RewardCoinStartPos;

    public GameObject g_NativePanel;
    public RawImage i_NativeIcon;
    public RawImage i_NativeChoices;
    public Text t_NativeHeadLine; 
    public Text t_NativeAction;
    public Text t_NativeAdvertiser;

    public bool b_isBannerOn;
    public bool b_isPremiumMode;
    public bool b_isNativeLoad = false;

#if UNITY_ANDROID
    private const string AppID = "ca-app-pub-6214691549388519~7058228130";
    // 5133AAD1CA5B2897

#elif UNITY_IOS
    private const string AppID = "ca-app-pub-6214691549388519~4079839176";

#endif


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
            return;

        MobileAds.Initialize(initStatus => {
            LoadBannerAd();
            LoadFrontAd();
            LoadRewardAd();
            // RequestNativeAD();
        });
        g_NativePanel.SetActive(false);
    }

    public void PremiumMode()
    {
        b_isPremiumMode = true;
        ShopManager_Scr.instance.t_Shop.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        ShopManager_Scr.instance.t_Shop.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Button_Option.instance.b_WindowOn)
        {
            g_NativePanel.transform.GetChild(1).GetComponent<BoxCollider>().enabled = false;
            g_NativePanel.transform.GetChild(2).GetComponent<BoxCollider>().enabled = false;
            g_NativePanel.transform.GetChild(3).GetChild(1).GetComponent<BoxCollider>().enabled = false;
            g_NativePanel.transform.GetChild(4).GetChild(0).GetComponent<BoxCollider>().enabled = false;
            g_NativePanel.transform.GetChild(5).GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            g_NativePanel.transform.GetChild(1).GetComponent<BoxCollider>().enabled = true;
            g_NativePanel.transform.GetChild(2).GetComponent<BoxCollider>().enabled = true;
            g_NativePanel.transform.GetChild(3).GetChild(1).GetComponent<BoxCollider>().enabled = true;
            g_NativePanel.transform.GetChild(4).GetChild(0).GetComponent<BoxCollider>().enabled = true;
            g_NativePanel.transform.GetChild(5).GetComponent<BoxCollider>().enabled = true;
        }
        if (!b_isPremiumMode && b_isNativeLoad)
        {
            b_isNativeLoad = false;
            Texture2D iconTexture2D = this.adNative.GetIconTexture();
            Texture2D iconAdChoices = this.adNative.GetAdChoicesLogoTexture();
            string headLine = this.adNative.GetHeadlineText();
            string cta = this.adNative.GetCallToActionText();
            string advertiser = this.adNative.GetAdvertiserText();

            i_NativeIcon.texture = iconTexture2D;
            i_NativeChoices.texture = iconAdChoices;
            t_NativeHeadLine.text = headLine;
            t_NativeAction.text = cta;
            t_NativeAdvertiser.text = advertiser;

            adNative.RegisterIconImageGameObject(i_NativeIcon.gameObject);
            adNative.RegisterAdChoicesLogoGameObject(i_NativeChoices.gameObject);
            adNative.RegisterHeadlineTextGameObject(t_NativeHeadLine.gameObject);
            adNative.RegisterCallToActionGameObject(t_NativeAction.gameObject);
            adNative.RegisterAdvertiserTextGameObject(t_NativeAdvertiser.gameObject);

            g_NativePanel.SetActive(true);
        }
        else if (b_isPremiumMode)
        {
            g_NativePanel.SetActive(false);

            if (ShopManager_Scr.instance.t_Shop.transform.GetChild(0).GetChild(2).gameObject.activeSelf)
            {
                ShopManager_Scr.instance.t_Shop.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
                ShopManager_Scr.instance.t_Shop.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
            }
        }

        string tmp = TranslateManager_Scr.instance.TranslateContext(56).Replace("N", AttendanceManager_Scr.instance.i_GetCount.ToString());
        /*  주석이 원래 꺼 
        Button_Option.instance.Before_Option.transform.GetChild(1).GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = TranslateManager_Scr.instance.TranslateContext(54) + " 20 " + TranslateManager_Scr.instance.TranslateContext(55) + tmp;
        Button_Option.instance.After_Pause.transform.GetChild(1).GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = TranslateManager_Scr.instance.TranslateContext(54) + " 20 " + TranslateManager_Scr.instance.TranslateContext(55) + tmp;
        */


        Button_Option.instance.Before_Option.transform.GetChild(1).GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = TranslateManager_Scr.instance.TranslateContext(54) + " " + TranslateManager_Scr.instance.TranslateContext(55) + tmp;
        Button_Option.instance.After_Pause.transform.GetChild(1).GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = TranslateManager_Scr.instance.TranslateContext(54) + " " + TranslateManager_Scr.instance.TranslateContext(55) + tmp;

        if (TutorialManager.instance.b_FirstPlaying || AttendanceManager_Scr.instance.i_GetCount <= 0)
        {
            Button_Option.instance.Before_Option.transform.GetChild(1).GetChild(6).GetComponent<Button>().enabled = false;  // Before_Option의 AD버튼
            Button_Option.instance.Before_Option.transform.GetChild(1).GetChild(6).GetComponent<Image>().color = new Vector4(255 / 255, 255 / 255, 255 / 255, 125f / 255f);
            Button_Option.instance.Before_Option.transform.GetChild(1).GetChild(6).GetChild(0).GetComponent<Image>().color = new Vector4(255 / 255, 255 / 255, 255 / 255, 125f / 255f);
            Button_Option.instance.Before_Option.transform.GetChild(1).GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().color = new Vector4(35f / 255f, 35f / 255f, 35f / 255f, 125f / 255f);

            Button_Option.instance.After_Pause.transform.GetChild(1).GetChild(6).GetComponent<Button>().enabled = false;  // After_Option의 AD버튼
            Button_Option.instance.After_Pause.transform.GetChild(1).GetChild(6).GetComponent<Image>().color = new Vector4(255 / 255, 255 / 255, 255 / 255, 125f / 255f);
            Button_Option.instance.After_Pause.transform.GetChild(1).GetChild(6).GetChild(0).GetComponent<Image>().color = new Vector4(255 / 255, 255 / 255, 255 / 255, 125f / 255f);
            Button_Option.instance.After_Pause.transform.GetChild(1).GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().color = new Vector4(35f / 255f, 35f / 255f, 35f / 255f, 125f / 255f);
        }
        else
        {
            Button_Option.instance.Before_Option.transform.GetChild(1).GetChild(6).GetComponent<Button>().enabled = true;  // Before_Option의 AD버튼
            Button_Option.instance.Before_Option.transform.GetChild(1).GetChild(6).GetComponent<Image>().color = new Vector4(255 / 255, 255 / 255, 255 / 255, 255 / 255);
            Button_Option.instance.Before_Option.transform.GetChild(1).GetChild(6).GetChild(0).GetComponent<Image>().color = new Vector4(255 / 255, 255 / 255, 255 / 255, 255 / 255);
            Button_Option.instance.Before_Option.transform.GetChild(1).GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().color = new Vector4(35f / 255f, 35f / 255f, 35f / 255f, 255f / 255f);

            Button_Option.instance.After_Pause.transform.GetChild(1).GetChild(6).GetComponent<Button>().enabled = true;  // After_Option의 AD버튼
            Button_Option.instance.After_Pause.transform.GetChild(1).GetChild(6).GetComponent<Image>().color = new Vector4(255 / 255, 255 / 255, 255 / 255, 255 / 255);
            Button_Option.instance.After_Pause.transform.GetChild(1).GetChild(6).GetChild(0).GetComponent<Image>().color = new Vector4(255 / 255, 255 / 255, 255 / 255, 255 / 255);
            Button_Option.instance.After_Pause.transform.GetChild(1).GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().color = new Vector4(35f / 255f, 35f / 255f, 35f / 255f, 255f / 255f);
        }
    }
    public void LogOut()
    {
        GoogleManager_Scr.instance.LogOut();
    }

    AdRequest GetAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    // 나중에 테스트 모드가 끝나고 플레이 스토어 등록 시, TestID를 일반 ID들로 변경해주자.
    #region 배너 광고
    const string bannerTestID = "ca-app-pub-3940256099942544/6300978111";
#if UNITY_ANDROID
    const string bannerID = "ca-app-pub-6214691549388519/2197903257";
#elif UNITY_IOS
    const string bannerID = "ca-app-pub-6214691549388519/2806764886";

#endif

    BannerView bannerAd;

    void LoadBannerAd()
    {
        //bannerAd = new BannerView(b_isTestMode ? bannerTestID : bannerID,
        //    AdSize.SmartBanner, AdPosition.Bottom);
        bannerAd = new BannerView(bannerTestID,
            AdSize.SmartBanner, 0, -1500);
        bannerAd.LoadAd(GetAdRequest());
        ToggleBannerAd(false);
    }

    public void ToggleBannerAd(bool b)
    {
        b_isBannerOn = b;
        if (!b_isPremiumMode)
        {
            if (b)
            {
                bannerAd.Show();
            }
            else
            {
                bannerAd.Hide();
            }
        }
        else
            bannerAd.Hide();
    }
    #endregion

    #region 네이티브 광고
    const string nativeTestID = "ca-app-pub-3940256099942544/2247696110";
#if UNITY_ANDROID
    const string nativeID = "ca-app-pub-6214691549388519/9559976813";
#elif UNITY_IOS
    const string nativeID = "ca-app-pub-6214691549388519/2865446962";

#endif

    private void RequestNativeAD()
    {
        AdLoader adLoader = new AdLoader.Builder(nativeTestID).ForUnifiedNativeAd().Build();
        adLoader.OnUnifiedNativeAdLoaded += this.HandleUnifiedNativeAdLoaded;
        adLoader.OnAdFailedToLoad += this.HandleNativeAdFailedToLoad;
        adLoader.LoadAd(GetAdRequest());
    }

    private void HandleUnifiedNativeAdLoaded(object sender, UnifiedNativeAdEventArgs args)
    {
        this.adNative = args.nativeAd;
        b_isNativeLoad = true;
    }
    private void HandleNativeAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("Native ad failed to load: " + args.Message);
    }
    public void ShowNativeAD()
    {
        /*
        b_isNativeLoad = true;

        Texture2D iconTexture2D = this.adNative.GetIconTexture();
        Texture2D iconAdChoices = this.adNative.GetAdChoicesLogoTexture();
        string headLine = this.adNative.GetHeadlineText();
        string cta = this.adNative.GetCallToActionText();
        string advertiser = this.adNative.GetAdvertiserText();

        i_NativeIcon.texture = iconTexture2D;
        i_NativeChoices.texture = iconAdChoices;
        t_NativeHeadLine.text = headLine;
        t_NativeAction.text = cta;
        t_NativeAdvertiser.text = advertiser;

        adNative.RegisterIconImageGameObject(i_NativeIcon.gameObject);
        adNative.RegisterAdChoicesLogoGameObject(i_NativeChoices.gameObject);
        adNative.RegisterHeadlineTextGameObject(t_NativeHeadLine.gameObject);
        adNative.RegisterCallToActionGameObject(t_NativeAction.gameObject);
        adNative.RegisterAdvertiserTextGameObject(t_NativeAdvertiser.gameObject);

        g_NativePanel.SetActive(true);
        */
    }
    public void HideNativeAD()
    {
        /*
        if (b_isNativeLoad)
        {
            b_isNativeLoad = false;

            g_NativePanel.SetActive(false);
        }
        */
    }
    #endregion



    #region 전면 광고
    const string frontTestID = "ca-app-pub-3940256099942544/8691691433";
#if UNITY_ANDROID
    const string frontID = "ca-app-pub-6214691549388519/3319413235";
#elif UNITY_IOS
    const string frontID = "ca-app-pub-6214691549388519/9180601548";

#endif
    InterstitialAd frontAd;


    void LoadFrontAd()
    {
        //frontAd = new InterstitialAd(b_isTestMode ? frontTestID : frontID);
        frontAd = new InterstitialAd(frontTestID);
        frontAd.LoadAd(GetAdRequest());
        frontAd.OnAdClosed += (sender, e) => {
            AttendanceManager_Scr.instance.b_AutoPause = true;
        };
    }

    public void ShowFrontAd()
    {
        if (!b_isPremiumMode)
        {
            AttendanceManager_Scr.instance.b_AutoPause = false;
            frontAd.Show();
            LoadFrontAd();
        }
    }
    #endregion



    #region 리워드 광고
    const string rewardTestID = "ca-app-pub-3940256099942544/5224354917";
#if UNITY_ANDROID
    const string rewardID = "ca-app-pub-6214691549388519/5754004888";
#elif UNITY_IOS
    const string rewardID = "ca-app-pub-6214691549388519/6554438200";
    
#endif
    RewardedAd rewardAd;


    void LoadRewardAd()
    {
        //rewardAd = new RewardedAd(b_isTestMode ? rewardTestID : rewardID);
        rewardAd = new RewardedAd(rewardTestID);
        rewardAd.LoadAd(GetAdRequest());
        rewardAd.OnUserEarnedReward += (sender, e) => {
            AttendanceManager_Scr.instance.b_AutoPause = true;
            Time.timeScale = 0;
            if (AttendanceManager_Scr.instance.i_GetCount > 0)
            {
                // 주석 없애기
                //AttendanceManager_Scr.instance.i_GetCount--;

                // 골드 추가 효과
                Vector3 screenPos = Button_Option.instance.Cam.GetComponent<Camera>().WorldToScreenPoint(transform.position);
                GameObject go_TextGold_Dia_Exp = Button_Option.instance.go_TextGold_Dia_Exp;
                GameObject go_Text_Point = go_TextGold_Dia_Exp.transform.GetChild(0).gameObject;
                GameObject go_Text_Dia = go_TextGold_Dia_Exp.transform.GetChild(1).gameObject;

                // 포인트 획득 애니메이션
                for (int i = 0; i < go_Text_Point.transform.childCount; i++)
                {
                    if (go_Text_Point.transform.GetChild(i).gameObject.activeSelf == false)
                    {
                        GameObject T_Point = go_Text_Point.transform.GetChild(i).gameObject;
                        Vector2 PointPos = RewardCoinStartPos.transform.localPosition;
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(T_Point.transform.GetComponent<RectTransform>(), screenPos, Camera.main, out PointPos);
                        T_Point.SetActive(true);
                        T_Point.transform.localPosition = new Vector3(0, -140, -200);
                        T_Point.transform.GetChild(0).GetComponent<Animator>().Play("GetCoin");

                        // 주석 없애기
                        //T_Point.transform.GetChild(0).GetComponent<Icon_GetCoin>().i_getGold = 20;
                        T_Point.transform.GetChild(0).GetComponent<Icon_GetCoin>().i_getGold = 10000;
                        T_Point.transform.GetChild(0).GetComponent<Icon_GetCoin>().i_getCash = 1000;

                        break;
                    }
                    else if (i >= go_Text_Point.transform.childCount - 1)
                    {
                        // 추가 생성하는 곳 
                    }
                }
            }
        };
    }

    public void ShowRewardAd()
    {
        AttendanceManager_Scr.instance.b_AutoPause = false;
        rewardAd.Show();
        LoadRewardAd();
    }
    #endregion
}