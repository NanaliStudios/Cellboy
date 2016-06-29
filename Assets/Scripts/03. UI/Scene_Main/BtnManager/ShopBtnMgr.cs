using UnityEngine;
using System.Collections;
using TapjoyUnity;

public partial class BtnManager : MonoBehaviour {

	public void OnFreeCharge1_Click()
	{
		//TapjoyManager.Instance.m_TjOfferwall.ShowContent();
		m_PlayerData.m_Gamedata.m_iHaveCoin += 1000;
	}
		
	public void Buy200CoinBtn_Click()
	{
		m_SdkMgr.Purcahse_Item ("cellboy_coin200");
	}

	public void Buy500CoinBtn_Click()
	{
		m_SdkMgr.Purcahse_Item ("cellboy_coin500");
	}

	public void Buy1000CoinBtn_Click()
	{
		m_SdkMgr.Purcahse_Item ("cellboy_coin5000");
	}

	//
	public void NoAdsBtn_Click()
	{
		if (PlayerPrefs.GetInt("Adoff")== 1)
			return;
			
		m_SdkMgr.Purcahse_Item ("cellboy_adoff");
	}
}
