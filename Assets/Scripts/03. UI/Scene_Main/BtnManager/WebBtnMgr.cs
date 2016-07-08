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
		Application.OpenURL ("http://facebook.com/nanalistudios");
	}

	public void ConnectTwitter()
	{
		Application.OpenURL ("https://twitter.com/nanalistudios");
	}

	public void ConnectNanali()
	{
		Application.OpenURL ("http://www.nanali.net");
	}

	public void ConnectMail()
	{
		Application.OpenURL ("http://www.nanali.net/bbs/write/bbs_qna");
	}

	public void RateBtn()
	{
		Application.OpenURL ("http://www.nanali.net");
	}

	public void RestoreProduct()
	{
		if (!AdFunctions.isInitialized())
			return;
#if UNITY_ANDROID
		AndroidInAppPurchaseManager.Client.RetrieveProducDetails();
#elif UNITY_IOS
		m_SdkMgr.RestoreItem();
#endif
	}

	public void SNS_ShareBtn()
	{
		string strMessage = string.Format ("My highscore is {0} in Cellboy!\n->http://www.nanali.net/", m_PlayerData.m_Gamedata.m_iHighScore);

#if UNITY_ANDROID
		AndroidSocialGate.StartShareIntent("Share Cellboy to SNS", strMessage);
#elif UNITY_IOS
		NotiManagerClientX_openActionSheetWithMessage(strMessage);
#endif
	}


	public void Connect_Store_forStarscroe()
	{

		m_PlayerData.m_bCanShowRate = false;
	}

	public void Connect_Facebook_forLike()
	{

		m_PlayerData.m_bCanShowFB = false;
	}
}
