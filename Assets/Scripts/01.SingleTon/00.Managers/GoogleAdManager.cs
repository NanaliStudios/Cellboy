using UnityEngine;
using System.Collections;

#if UNITY_ANDROID
public class GoogleAdManager : MonoBehaviour {

	public bool m_bIsDismissPopup  = false;

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
			Debug.Log("Popup Closed");
		};

		banner = m_AdMob.CreateAdBanner (TextAnchor.LowerCenter, GADBannerSize.SMART_BANNER);

	}

	public bool isInitialized()
	{
		return m_AdMob.IsInited;
	}

	public void ShowBanner()
	{
		if (banner == null) {
			Debug.Log("banner is null. Create Banner");
			banner = m_AdMob.CreateAdBanner (TextAnchor.LowerCenter, GADBannerSize.SMART_BANNER);
			banner.Show ();
		} else {
			if (!banner.IsOnScreen) {
				Debug.Log("AD ON");
				banner.Show ();
			}
		}
	}

	public void ShowPopup()
	{
		m_AdMob.StartInterstitialAd();
	}
	
	public void BannerHide()
	{
		Debug.Log("AD OFF");

		if (banner != null) {
			banner.Hide ();
			m_AdMob.DestroyBanner (banner.id);
		}
		else
			Debug.Log ("banner is null");
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
		AdRequest request = new AdRequest.Builder().Build();

		bannerView = new BannerView(strBannerID, AdSize.SmartBanner, AdPosition.Bottom);
		bannerView.OnAdLoaded += delegate {
			m_bIsInitialized = true;
		};
		bannerView.LoadAd(request);

		interstitial = new InterstitialAd(strPopupID);
		interstitial.OnAdClosed += delegate {
			m_bIsDismissPopup = true;
		};
		interstitial.LoadAd(request);
	}

	public bool isInitialized()
	{
		return m_bIsInitialized;
	}
	
	public void ShowBanner()
	{
		bannerView.Show ();
	}

	public void ShowBanner()
	{
		bannerView.Hide ();
	}


	
	public void ShowPopup()
	{
		interstitial.Show ();
	}
		
//		public void Request(AdMobState state)
//		{
//			string adUnitId = PluginIDManager.Instance.GetAdMobID(state);
//			// Create an empty ad request.
//			AdRequest request = new AdRequest.Builder().Build();
//			switch(state)
//			{
//			case AdMobState.Interstitial:
//				// Initialize an InterstitialAd.
//				interstitial = new InterstitialAd(adUnitId);
//				// Load the interstitial with the request.
//				interstitial.LoadAd(request);
//				break;
//			case AdMobState.Banner:
//				// Create a 320x50 banner at the top of the screen.
//				bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
//				// Load the banner with the request.
//				bannerView.LoadAd(request);
//				break;
//			}
//		}
//		
//		public void ShowContents(AdMobState state)
//		{
//			string id = PluginIDManager.Instance.GetAdMobID(state);
//			Debug.Log (state + " : " + id);
//			switch(state)
//			{
//			case AdMobState.Interstitial:
//				if (interstitial.IsLoaded())
//				{
//					interstitial.Show();
//					Request(state);
//				}
//				break;
//			case AdMobState.Banner:
//				bannerView.Show();
//				break;
//			}
//		}
//		
//		public void BannerHide()
//		{
//			bannerView.Hide();
//		}
}
#endif
