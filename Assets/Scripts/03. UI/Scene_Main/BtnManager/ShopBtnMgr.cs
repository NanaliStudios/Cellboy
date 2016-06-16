using UnityEngine;
using System.Collections;
using TapjoyUnity;

public partial class BtnManager : MonoBehaviour {

	public void OnFreeCharge1_Click()
	{
		PlayerPrefs.SetInt ("HaveCoin", PlayerPrefs.GetInt("HaveCoin") + 1000);
		TapjoyManager.Instance.ContentsReady ("getfreecoin1");
	}

	public void Buy200CoinBtn_Click()
	{
		GameSDK_Funcs.Purcahse_Item ("coin_200");

		if (GameSDK_Funcs.Check_IsPurchased ("coin_200"))
			PlayerPrefs.SetInt ("HaveCoin", PlayerPrefs.GetInt("HaveCoin") + 200);
	}

	//
	public void NoAdsBtn_Click()
	{
		PlayerPrefs.SetInt ("ADS_Key", 1);
		Application.LoadLevel ("00_Logo");
	}
}
