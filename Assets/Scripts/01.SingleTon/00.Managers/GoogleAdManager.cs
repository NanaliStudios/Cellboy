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

	}

	public bool isInitialized()
	{
		banner = m_AdMob.CreateAdBanner (TextAnchor.LowerCenter, GADBannerSize.SMART_BANNER);
		return m_AdMob.IsInited;
	}

	public void ShowBanner()
	{
	 	if (banner == null) {
			banner = m_AdMob.CreateAdBanner (TextAnchor.LowerCenter, GADBannerSize.SMART_BANNER);
			banner.Show();
		}
	 	else
	 		banner.Show();
	}

	public void ShowPopup()
	{
		m_AdMob.StartInterstitialAd();
	}
	
	public void BannerHide()
	{
		if(banner.IsLoaded && banner.IsOnScreen)
			banner.Hide();
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
