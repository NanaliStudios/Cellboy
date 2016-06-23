using UnityEngine;
using System.Collections;
using TapjoyUnity;

public partial class BtnManager : MonoBehaviour {

	public void OnFreeCharge1_Click()
	{
		m_PlayerData.m_Gamedata.m_iHaveCoin += 1000;
	}

	public void OnFreeCharge2_Click()
	{
		TapjoyManager.Instance.m_TjOfferwall.ShowContent();
	}

	public void Buy200CoinBtn_Click()
	{
		GameSDK_Funcs.Purcahse_Item ("coin_200");
	}

	public void Buy500CoinBtn_Click()
	{
		GameSDK_Funcs.Purcahse_Item ("coin_500");
	}

	public void Buy1000CoinBtn_Click()
	{
		GameSDK_Funcs.Purcahse_Item ("coin_1000");
	}

	//
	public void NoAdsBtn_Click()
	{
		if (m_PlayerData.m_Gamedata.m_bAdOff == true)
			return;
			
		GameSDK_Funcs.Purcahse_Item ("adoff");
	}
}
