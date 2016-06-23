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
//		if (GameSDK_Funcs.Check_IsPurchased ("coin_200"))
//			m_PlayerData.m_Gamedata.m_iHaveCoin += 200;
	}

	//
	public void NoAdsBtn_Click()
	{
		if (m_PlayerData.m_Gamedata.m_bAdOff == true)
			return;

		m_PlayerData.m_Gamedata.m_bAdOff = true;
		m_PlayerData.GameData_Save ();
		Application.LoadLevel ("00_Logo");
	}
}
