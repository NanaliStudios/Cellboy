using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public partial class BtnManager : MonoBehaviour {

	[DllImport ("__Internal")]
	private static extern void NotiManagerClientX_openActionSheetWithMessage(string message);
	[DllImport ("__Internal")]
	private static extern void NotiManagerClientX_openActionSheetWithImagePath(string path,string message);

	public void ConnectFacebook()
	{
		Play_BtnSound ();
		Application.OpenURL ("http://facebook.com/nanalistudios");
	}

	public void ConnectTwitter()
	{
		Play_BtnSound ();
		Application.OpenURL ("https://twitter.com/nanalistudios");
	}

	public void ConnectNanali()
	{
		Play_BtnSound ();
		Application.OpenURL ("http://www.nanali.net");
	}

	public void ConnectMail()
	{
		Play_BtnSound ();
		Application.OpenURL ("http://www.nanali.net/bbs/write/bbs_qna");
	}

	public void RateBtn()
	{
		Play_BtnSound ();
		
		#if UNITY_ANDROID
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.Nanali.Cellboy");
		#elif UNITY_IOS
		Application.OpenURL("https://itunes.apple.com/app/id1124415214?mt=8");
		#endif
	}

	public void RestoreProduct()
	{
		Play_BtnSound ();
#if UNITY_ANDROID
		AndroidInAppPurchaseManager.Client.RetrieveProducDetails();
#elif UNITY_IOS
		m_SdkMgr.RestoreItem();
#endif
	}

	public void SNS_ShareBtn()
	{
		Play_BtnSound ();

		string strStoreURL;

		#if UNITY_ANDROID
		strStoreURL = "https://goo.gl/csNq8T";
		#elif UNITY_IOS
		strStoreURL = "https://goo.gl/x6zgKh";
		#endif

		string strMessage = string.Format ("My highscore is {0} in Cellboy!\n->" + strStoreURL, m_PlayerData.m_Gamedata.m_iHighScore);

#if UNITY_ANDROID
		AndroidSocialGate.StartShareIntent("Share Cellboy to SNS", strMessage);
#elif UNITY_IOS
		NotiManagerClientX_openActionSheetWithMessage(strMessage);
#endif
	}


	public void Connect_Store_forStarscroe()
	{
		Play_BtnSound ();

		m_PlayerData.m_bCanShowRate = false;
	}

	public void Connect_Facebook_forLike()
	{
		Play_BtnSound ();

		m_PlayerData.m_bCanShowFB = false;
	}
}
