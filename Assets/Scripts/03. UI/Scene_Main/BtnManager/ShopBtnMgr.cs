using UnityEngine;
using System.Collections;

public partial class BtnManager : MonoBehaviour {

	public void OnFreeCharge1_Click()
	{
		PlayerPrefs.SetInt ("HaveCoin", PlayerPrefs.GetInt("HaveCoin") + 1000);
		TapjoyManager.Instance.ContentsReady ("getfreecoin1");
	}
}
