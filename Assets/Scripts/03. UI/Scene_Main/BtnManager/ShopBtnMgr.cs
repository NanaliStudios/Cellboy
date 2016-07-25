using UnityEngine;
using System.Collections;
using TapjoyUnity;

public partial class BtnManager : MonoBehaviour {

	public GameObject m_objWaitback = null;

	public void OnFreeCharge1_Click()
	{
		if (TapjoyManager.Instance.m_TjOfferwall != null)
			TapjoyManager.Instance.m_TjOfferwall.ShowContent ();
		else {
			MobileNativeMessage msg = new MobileNativeMessage ("error", "Tapjoy Initialize Failed");
		}

		Play_BtnSound ();
//		m_PlayerData.m_Gamedata.m_iHaveCoin += 1000;

	}
		
	public void Buy500CoinBtn_Click()
	{
		Play_BtnSound ();
		m_SdkMgr.Purcahse_Item ("cellboy_500coin");
	}

	public void Buy1000CoinBtn_Click()
	{
		Play_BtnSound ();
		m_SdkMgr.Purcahse_Item ("cellboy_1000coin");
	}

	public void Buy5000CoinBtn_Click()
	{
		Play_BtnSound ();
		m_SdkMgr.Purcahse_Item ("cellboy_5kcoin");
	}

	//
	public void NoAdsBtn_Click()
	{
		if (PlayerPrefs.GetInt ("Adoff") == 1) {
			MobileNativeMessage msg = new MobileNativeMessage ("buy adoff", "You've already purchased 'adoff'");
			return;
		} else {
			Play_BtnSound ();
			m_SdkMgr.Purcahse_Item ("cellboy_adoff");
		}
	}
}
