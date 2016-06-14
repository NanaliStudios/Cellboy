using UnityEngine;
using System.Collections;

public class GoogleAdManager : MonoBehaviour {

	private static GoogleAdManager _instance;
	public static GoogleAdManager Instance{
		get{
			if(_instance == null)
				_instance = FindObjectOfType(typeof(GoogleAdManager)) as GoogleAdManager;
			return _instance;
		}
	}
	
	
	private GoogleMobileAdBanner banner;
	
	private void Awake()
	{
		DontDestroyOnLoad (this);

		string bannerID = "ca-app-pub-6269735295695961/3500171335";
		
		AndroidAdMobController AdMob = AndroidAdMobController.instance;
		if(AdMob != null)
		AdMob.Init (bannerID);

	}
	
	public void ShowBanner()
	{
		if (AndroidAdMobController.Instance.IsInited == false)
			return;

	 	if(banner == null)
	 		banner = AndroidAdMobController.instance.CreateAdBanner (TextAnchor.LowerCenter, GADBannerSize.SMART_BANNER);
	 	else
	 		banner.Show();
	}
	
	public void BannerHide()
	{
		if(banner.IsLoaded && banner.IsOnScreen)
			banner.Hide();
	}
}
