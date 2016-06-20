using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using ChartboostSDK;

class AdFunctions
{
	private static GoogleAdManager m_GoogleAD = null;
	public static bool m_bAdsComplete = false;
	public static bool m_bTjNoticeDismiss = false;

	public static void Initialize()
	{
		//unityad init
		Advertisement.Initialize ("1077035", true);
	
		//google admob init
		m_GoogleAD = new GoogleAdManager();
		m_GoogleAD.Initialize ();

		//chartboost init
		Chartboost.cacheInterstitial (CBLocation.Default);
	
	}

	public static bool isInitialized()
	{
		if (Advertisement.isInitialized &&
		    m_GoogleAD.isInitialized ())
			return true;
	
		return false;
	}

	public static bool Check_UnityAdsRdy()
	{
		return Advertisement.IsReady();
	}


	//===Show===
	public static void Show_GoogleADBanner()
	{
		m_GoogleAD.ShowBanner ();
	}

	public static void Show_GoogleADPopup()
	{
		m_GoogleAD.ShowPopup ();
	}

	public static bool Check_IsClose_GooglePopup()
	{
		return m_GoogleAD.m_bIsDismissPopup;
	}

	public static void Set_IsClose_GooglePopup()
	{
		m_GoogleAD.m_bIsDismissPopup = false;
	}

	public static bool Show_UnityAds()
	{
		if (!Advertisement.isInitialized)
			return false;

		if (Check_UnityAdsRdy ()) { //동영상이 준비 되었으면
			ShowOptions opt = new ShowOptions ();
			opt.resultCallback = OnShowResult;
			Advertisement.Show (null, opt);
		}else
			return false;

		return true;

	}
	
	public static void OnShowResult(ShowResult ret)
	{
		switch(ret)
		{
		case ShowResult .Failed:      //동영상 보여주기 실패한경우
			break;
		case ShowResult .Skipped:   //유저가 중간에 동영상을 스킵한경우
			break;
		case ShowResult .Finished:  //유저가 동영상을 끝까지 본경우
			m_bAdsComplete = true;
			break;
		}
	}



	                       
}