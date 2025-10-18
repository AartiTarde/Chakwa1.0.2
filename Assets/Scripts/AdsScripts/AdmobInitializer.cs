//using UnityEngine;
//using GoogleMobileAds.Api;
//using System;


////google ads version v9.5.0
//public class AdmobInitializer : MonoBehaviour
//{
//    private static bool isInitialized = false;
//    public static AdmobInitializer instance;
//    //InterstitialAd
//    private InterstitialAd interstitialAd;
//    private string adUnitId = "ca-app-pub-1066929415605370/6656161198"; //InterstitialAd unit id

//    //RewardedAds
//    private RewardedAd rewardedAd;
//    private string radUnitId = "ca-app-pub-1066929415605370/9260744437"; //RewardedAd unit id


//    //Banner
//    private BannerView bannerView;
//    private string badUnitId = "ca-app-pub-1066929415605370/1357686761";//BannerView unit id
//    void Awake()
//    {

//        if (instance == null)
//        {
//            instance = this;
//        }
//        else
//        {
//            Destroy(gameObject);
//            return;
//        }

//        if (!isInitialized)
//        {
//            InitializeAdmob();
//            isInitialized = true;
//        }
//    }

//    private void InitializeAdmob()
//    {
//        Debug.Log("Initializing Google Mobile Ads SDK...");

//        MobileAds.Initialize(initStatus =>
//        {
//            Debug.Log("Google Mobile Ads SDK initialized.");
//            LoadInterstitialAd();
//            LoadRewardedAd();
//            LoadBanner();
//            ShowBanner();
//        });
//    }


//    private void LoadInterstitialAd()
//    {
//        Debug.Log("Loading Interstitial Ad...");

//        AdRequest request = new AdRequest();

//        InterstitialAd.Load(adUnitId, request, (InterstitialAd ad, LoadAdError error) =>
//        {
//            if (error != null || ad == null)
//            {
//                Debug.LogError("Interstitial ad failed to load: " + error);
//                return;
//            }

//            Debug.Log("Interstitial ad loaded.");
//            interstitialAd = ad;


//            interstitialAd.OnAdFullScreenContentFailed += (AdError adError) =>
//            {
//                Debug.LogError("Interstitial ad failed to show: " + adError);
//            };

//            interstitialAd.OnAdFullScreenContentClosed += () =>
//            {
//                Debug.Log("Interstitial ad closed. Reloading...");
//                LoadInterstitialAd();
//            };

//            interstitialAd.OnAdFullScreenContentOpened += () =>
//            {
//                Debug.Log("Interstitial ad opened.");
//            };

//            interstitialAd.OnAdImpressionRecorded += () =>
//            {
//                Debug.Log("Interstitial ad impression recorded.");
//            };

//            interstitialAd.OnAdPaid += (AdValue adValue) =>
//            {
//                Debug.Log($"Interstitial Ad paid: {adValue.Value} {adValue.CurrencyCode}");
//            };
//        });
//    }

//    public void ShowInterstitialAd()
//    {
//        if (interstitialAd != null && interstitialAd.CanShowAd())
//        {
//            interstitialAd.Show();
//        }
//        else
//        {
//            Debug.Log("Interstitial ad not ready yet. Loading...");
//            LoadInterstitialAd();
//        }
//    }

//    //Rewarded Ads
//    private void LoadRewardedAd()
//    {
//        Debug.Log("Loading Rewarded Ad...");

//        AdRequest request = new AdRequest();


//        RewardedAd.Load(radUnitId, request, (RewardedAd ad, LoadAdError error) =>
//        {
//            if (error != null || ad == null)
//            {
//                Debug.LogError("Rewarded ad failed to load: " + error);
//                return;
//            }

//            Debug.Log("Rewarded ad loaded.");
//            rewardedAd = ad;

//            // Register event handlers
//            rewardedAd.OnAdFullScreenContentFailed += (AdError adError) =>
//            {
//                Debug.LogError("Rewarded ad failed to show: " + adError);
//            };

//            rewardedAd.OnAdFullScreenContentClosed += () =>
//            {
//                Debug.Log("Rewarded ad closed. Reloading...");
//                LoadRewardedAd();
//            };

//            rewardedAd.OnAdFullScreenContentOpened += () =>
//            {
//                Debug.Log("Rewarded ad opened.");
//            };

//            rewardedAd.OnAdImpressionRecorded += () =>
//            {
//                Debug.Log("Rewarded ad impression recorded.");
//            };

//            rewardedAd.OnAdPaid += (AdValue adValue) =>
//            {
//                Debug.Log($"Ad paid: {adValue.Value} {adValue.CurrencyCode}");
//            };
//        });

//    }

//    //BannerAds
//    private void LoadBanner()
//    {
//        Debug.Log("Loading Banner Ad...");

//        if (bannerView != null)
//        {
//            bannerView.Destroy();
//        }

//        bannerView = new BannerView(badUnitId, AdSize.Banner, AdPosition.Bottom);
//        bannerView.OnBannerAdLoaded += () =>
//        {
//            Debug.Log("Banner loaded successfully.");
//        };

//        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
//        {
//            Debug.LogError("Banner failed to load: " + error);
//        };

//        bannerView.OnAdImpressionRecorded += () =>
//        {
//            Debug.Log("Banner impression recorded.");
//        };

//        bannerView.OnAdClicked += () =>
//        {
//            Debug.Log("Banner clicked.");
//        };

//        bannerView.OnAdPaid += (AdValue adValue) =>
//        {
//            Debug.Log($"Banner ad paid: {adValue.Value} {adValue.CurrencyCode}");
//        };


//        AdRequest request = new AdRequest();
//        bannerView.LoadAd(request);
//    }
//    public void HideBanner()
//    {
//        if (bannerView != null)
//        {
//            bannerView.Hide();
//            Debug.Log("Banner hidden.");
//        }
//    }
//    public void ShowBanner()
//    {
//        if (bannerView != null)
//        {
//            bannerView.Show();
//            Debug.Log("Banner shown.");
//        }
//    }
//    void OnDestroy()
//    {
//        if (bannerView != null)
//        {
//            bannerView.Destroy();
//        }
//    }

//    public void ShowRewardedAd(Action onAdClosed = null)
//    {
//        if (rewardedAd != null)
//        {
//            rewardedAd.OnAdFullScreenContentClosed += () =>
//            {
//                Debug.Log("Rewarded ad closed. Reloading...");
//                LoadRewardedAd();


//                onAdClosed?.Invoke();
//            };

//            rewardedAd.Show((Reward reward) =>
//            {
//                Debug.Log($"User earned reward: {reward.Amount} {reward.Type}");

//            });
//        }
//        else
//        {
//            Debug.Log("Rewarded ad not ready yet. Loading...");
//            LoadRewardedAd();
//        }
//    }

//}
using UnityEngine;
using GoogleMobileAds.Api;
using System;


//google ads version v9.5.0
public class AdmobInitializer : MonoBehaviour
{
    private static bool isInitialized = false;
    public static AdmobInitializer instance;
    //InterstitialAd
    private InterstitialAd interstitialAd;
    private string adUnitId = "ca-app-pub-1066929415605370/6656161198"; //InterstitialAd unit id

    //RewardedAds
    private RewardedAd rewardedAd;
    private string radUnitId = "ca-app-pub-1066929415605370/9260744437"; //RewardedAd unit id


    //Banner
    private BannerView bannerView;
    private string badUnitId = "ca-app-pub-1066929415605370/1357686761";//BannerView unit id
    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (!isInitialized)
        {
            InitializeAdmob();
            isInitialized = true;
        }
    }

    private void InitializeAdmob()
    {
        Debug.Log("Initializing Google Mobile Ads SDK...");

        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("Google Mobile Ads SDK initialized.");
            LoadInterstitialAd();
            LoadRewardedAd();
            LoadBanner();
            ShowBanner();
        });
    }


    private void LoadInterstitialAd()
    {
        Debug.Log("Loading Interstitial Ad...");

        AdRequest request = new AdRequest();

        InterstitialAd.Load(adUnitId, request, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Interstitial ad failed to load: " + error);
                return;
            }

            Debug.Log("Interstitial ad loaded.");
            interstitialAd = ad;


            interstitialAd.OnAdFullScreenContentFailed += (AdError adError) =>
            {
                Debug.LogError("Interstitial ad failed to show: " + adError);
            };

            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Interstitial ad closed. Reloading...");
                LoadInterstitialAd();
            };

            interstitialAd.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Interstitial ad opened.");
            };

            interstitialAd.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Interstitial ad impression recorded.");
            };

            interstitialAd.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log($"Interstitial Ad paid: {adValue.Value} {adValue.CurrencyCode}");
            };
        });
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstitial ad not ready yet. Loading...");
            LoadInterstitialAd();
        }
    }

    //Rewarded Ads
    private void LoadRewardedAd()
    {
        Debug.Log("Loading Rewarded Ad...");

        AdRequest request = new AdRequest();


        RewardedAd.Load(radUnitId, request, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load: " + error);
                return;
            }

            Debug.Log("Rewarded ad loaded.");
            rewardedAd = ad;

            // Register event handlers
            rewardedAd.OnAdFullScreenContentFailed += (AdError adError) =>
            {
                Debug.LogError("Rewarded ad failed to show: " + adError);
            };

            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded ad closed. Reloading...");
                LoadRewardedAd();
            };

            rewardedAd.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Rewarded ad opened.");
            };

            rewardedAd.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Rewarded ad impression recorded.");
            };

            rewardedAd.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log($"Ad paid: {adValue.Value} {adValue.CurrencyCode}");
            };
        });

    }

    //BannerAds
    private void LoadBanner()
    {
        Debug.Log("Loading Banner Ad...");

        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        bannerView = new BannerView(badUnitId, AdSize.Banner, AdPosition.Bottom);
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner loaded successfully.");
        };

        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner failed to load: " + error);
        };

        bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner impression recorded.");
        };

        bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner clicked.");
        };

        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log($"Banner ad paid: {adValue.Value} {adValue.CurrencyCode}");
        };


        AdRequest request = new AdRequest();
        bannerView.LoadAd(request);
    }
    public void HideBanner()
    {
        if (bannerView != null)
        {
            bannerView.Hide();
            Debug.Log("Banner hidden.");
        }
    }
    public void ShowBanner()
    {
        if (bannerView != null)
        {
            bannerView.Show();
            Debug.Log("Banner shown.");
        }
    }
    void OnDestroy()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }

    public void ShowRewardedAd(Action onAdClosed = null)
    {
        if (rewardedAd != null)
        {
            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded ad closed. Reloading...");
                LoadRewardedAd();


                onAdClosed?.Invoke();
            };

            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log($"User earned reward: {reward.Amount} {reward.Type}");

            });
        }
        else
        {
            Debug.Log("Rewarded ad not ready yet. Loading...");
            LoadRewardedAd();
        }
    }

}
