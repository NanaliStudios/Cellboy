using UnityEngine;
using System.Collections;

#if UNITY_ANDROID
public class GoogleAdManager : MonoBehaviour {

	public bool m_bIsDismissPopup  = false;
	private bool m_bIsLoadedPopup = false;

	private AndroidAdMobController m_AdMob = null;
	private GoogleMobileAdBanner banner = null;

	private string strBannerID = "ca-app-pub-6269735295695961/3500171335";
	private string strPopupID = "ca-app-pub-6269735295695961/9295379334";
	
	public void Initialize()
	{

		m_AdMob = AndroidAdMobController.Instance;

		if (m_AdMob != null) {
			m_AdMob.Init (strBannerID, strPopupID);
		}

		m_AdMob.OnInterstitialClosed += delegate {
			m_bIsDismissPopup = true;
			//Debug.Log("Popup Closed");
		};

		m_AdMob.OnInterstitialLoaded += delegate {
			m_bIsLoadedPopup = true;
		};

		//banner = m_AdMob.CreateAdBanner (TextAnchor.LowerCenter, GADBannerSize.SMART_BANNER);

	}

	public void CreateBanner()
	{
		banner = m_AdMob.CreateAdBanner (TextAnchor.LowerCenter, GADBannerSize.SMART_BANNER);
	}

	public bool isInitialized()
	{
		return m_AdMob.IsInited;
	}

	public void ShowBanner()
	{
		if (banner == null) {
			//Debug.Log("banner is null. Create Banner");
			banner = m_AdMob.CreateAdBanner (TextAnchor.LowerCenter, GADBannerSize.SMART_BANNER);
			banner.Show ();
		} else {
			if (!banner.IsOnScreen) {
				//Debug.Log("AD ON");
				banner.Show ();
			}
		}
	}

	public bool IsLoadedPopup()
	{
		return m_bIsLoadedPopup;
	}

	public void ShowPopup()
	{
		m_AdMob.StartInterstitialAd();
	}
	
	public void BannerHide()
	{
		//Debug.Log("AD OFF");

		if (banner != null) {
			banner.Hide ();
			m_AdMob.DestroyBanner (banner.id);
		}
		//else
			//Debug.Log ("banner is null");
	}
}


	//IOS
#elif UNITY_IOS
	using GoogleMobileAds.Api;
public class GoogleAdManager : MonoBehaviour
	{
		
	public bool m_bIsInitialized = false;
		public bool m_bIsDismissPopup  = false;
	
		private string strBannerID = "ca-app-pub-6269735295695961/3500171335";
		private string strPopupID = "ca-app-pub-6269735295695961/9295379334";
		private BannerView bannerView;
		private InterstitialAd interstitial;

	public void Initialize()
	{
		Debug.Log ("GoogleAdManager:Initialize");

		AdRequest request = new AdRequest.Builder().Build();

		bannerView = new BannerView(strBannerID, AdSize.SmartBanner, AdPosition.Bottom);
//		bannerView.OnAdLoaded += delegate {
//		};
		bannerView.LoadAd(request);

		interstitial = new InterstitialAd(strPopupID);
		interstitial.OnAdClosed += delegate {
			m_bIsDismissPopup = true;
	
			interstitial.Destroy();
			interstitial.LoadAd(request);
		};
		interstitial.LoadAd(request);

		m_bIsInitialized = true;
	}

	public bool isInitialized()
	{
		return m_bIsInitialized;
	}
	
	public void ShowBanner()
	{
		Debug.Log ("Show Admob Banner");
		bannerView.Show ();
	}
	
	public void ShowPopup()
	{
		interstitial.Show ();
	}

	public bool IsLoadedPopup()
	{
		return interstitial.IsLoaded ();
	}


	public void BannerHide()
	{
		Debug.Log ("Hide Admob Banner");
		bannerView.Hide ();
	}
		
}
#endif
