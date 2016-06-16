using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

class AdFuctions
{
	private static GoogleAdManager m_GoogleAD = null;
	public static bool m_bAdsComplete = false;

	public static void Initialize()
	{
		//unityad init
		Advertisement.Initialize ("1077035", true);
	
		//google admob init
		m_GoogleAD = new GoogleAdManager();
		m_GoogleAD.Initialize ();
	
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
	public static void Show_GoogleAD()
	{
		if(PlayerPrefs.GetInt ("ADS_Key") == 0)
			m_GoogleAD.ShowBanner ();
	}

	public static bool Show_UnityAds()
	{
		if (!Advertisement.isInitialized)
			return false;

		if (Check_UnityAdsRdy ()) { //동영상이 준비 되었으면
			ShowOptions opt = new ShowOptions ();
			opt.resultCallback = OnShowResult;
			Advertisement.Show (null, opt);
		} else
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