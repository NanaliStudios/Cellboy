using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

class AdFuctions
{
	public static bool m_bAdsComplete = false;

	public static bool Init_UnityAds()
	{
		Advertisement.Initialize ("1077035", true);

		return Advertisement.isInitialized;
	}

	public static bool Check_UnityAdsRdy()
	{
		return Advertisement.IsReady();
	}

	public static void ShowUnityAds()
	{
		if (Check_UnityAdsRdy()) //동영상이 준비 되었으면
		{
			ShowOptions opt = new ShowOptions();
			opt.resultCallback = OnShowResult;
			Advertisement.Show(null, opt);
		}
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