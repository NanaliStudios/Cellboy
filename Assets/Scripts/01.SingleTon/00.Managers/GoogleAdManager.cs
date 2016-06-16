using UnityEngine;
using System.Collections;

public class GoogleAdManager : MonoBehaviour {

	private AndroidAdMobController m_AdMob = null;
	private GoogleMobileAdBanner banner = null;

	private string strBannerID = "ca-app-pub-6269735295695961/3500171335";
	
	public void Initialize()
	{	
		m_AdMob = AndroidAdMobController.Instance;

		#if UNITY_ANDROID
		strBannerID = "ca-app-pub-6269735295695961/3500171335";
		#elif UNITY_IOS
		strBannerID = "ca-app-pub-6269735295695961/8969465335";
		#endif

		if(m_AdMob != null)
			m_AdMob.Init (strBannerID);

	}

	public bool isInitialized()
	{
		banner = m_AdMob.CreateAdBanner (TextAnchor.LowerCenter, GADBannerSize.SMART_BANNER);
		return m_AdMob.IsInited;
	}

	public void ShowBanner()
	{
//		if (AndroidAdMobController.Instance.IsInited == false)
//			return;

	 	if(banner == null)
			banner = m_AdMob.CreateAdBanner (TextAnchor.LowerCenter, GADBannerSize.SMART_BANNER);
	 	else
	 		banner.Show();
	}
	
	public void BannerHide()
	{
		if(banner.IsLoaded && banner.IsOnScreen)
			banner.Hide();
	}
}
