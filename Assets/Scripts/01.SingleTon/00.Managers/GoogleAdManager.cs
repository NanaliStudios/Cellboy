using UnityEngine;
using System.Collections;

public class GoogleAdManager : MonoBehaviour {

	public bool m_bIsDismissPopup  = false;

	private AndroidAdMobController m_AdMob = null;
	private GoogleMobileAdBanner banner = null;

	private string strBannerID = "ca-app-pub-6269735295695961/3500171335";
	private string strPopupID = "ca-app-pub-6269735295695961/9295379334";
	
	public void Initialize()
	{	
		m_AdMob = AndroidAdMobController.Instance;

		#if UNITY_ANDROID
		strBannerID = "ca-app-pub-6269735295695961/3500171335";
		strPopupID = "ca-app-pub-6269735295695961/9295379334";
		#elif UNITY_IOS
		strBannerID = "ca-app-pub-6269735295695961/8969465335";
		#endif

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
