using UnityEngine;
using System.Collections;

public partial class BtnManager : MonoBehaviour {

	public void OnFreeCharge1_Click()
	{
		PlayerPrefs.SetInt ("HaveCoin", PlayerPrefs.GetInt("HaveCoin") + 1000);
		TapjoyManager.Instance.ContentsReady ("getfreecoin1");
	}

	public void Buy200CoinBtn_Click()
	{
		AndroidInAppPurchaseManager.Client.Purchase ("coin_200");


		if (AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased ("coin_200")) {
			PlayerPrefs.SetInt ("HaveCoin", PlayerPrefs.GetInt("HaveCoin") + 200);
		}
	}
}
