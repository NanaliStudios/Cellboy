using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using GooglePlayGames;
//using TapjoyUnity;
//using ChartboostSDK;

public class PlayerData : MonoBehaviour {

	public PLAYER_ID m_PlayerID = PLAYER_ID.NORMAL;
	public string m_strPlayerName = "";
	public string m_strPlayerInfo = "";
	public int m_iChargePrice = 0;
	public int m_iBuyPrice = 0;


	public int m_iCurrentScore = 0;
	public int m_iCurrentCoin = 0;
	public int m_iCurrentPlayNum = 0;
	public bool m_bWinHighScore = false;
	public Color m_BackColor = Color.white;
	
	public int m_iPlayCountForAd = 0;

	public float m_fPlayer0_ChargeTime = 3;
	public float m_fPlayer1_ChargeTime = 10;
	public float m_fPlayer2_ChargeTime = 60;
	public float m_fPlayer3_ChargeTime = 240;
	public float m_fPlayer4_ChargeTime = 360;

	void Awake()
	{
		GooglePlayGames.PlayGamesPlatform.Activate ();
	}

	// Use this for initialization
	void Start () {

		Advertisement.Initialize ("1077035", true);
		Application.targetFrameRate = 60;
	
		//fordebug
		//PlayerPrefs.DeleteAll ();	

		if (PlayerPrefs.GetInt ("PlayNum") == 0) {
			PlayerPrefs.DeleteAll ();	
			PlayerPrefs.SetInt ("PlayNum", 0);
			
			PlayerPrefs.SetInt ("HighScore", 0);
			PlayerPrefs.SetInt ("HaveCoin", 0);
			
			PlayerPrefs.SetFloat ("fPlayer0Tired", 100.0f);
			PlayerPrefs.SetFloat ("fPlayer1Tired", 100.0f);
			PlayerPrefs.SetFloat ("fPlayer2Tired", 100.0f);
			PlayerPrefs.SetFloat ("fPlayer3Tired", 100.0f);
			PlayerPrefs.SetFloat ("fPlayer4Tired", 100.0f);

			PlayerPrefs.SetFloat ("Player0ChargeTime", m_fPlayer0_ChargeTime);
			PlayerPrefs.SetFloat ("Player1ChargeTime", m_fPlayer1_ChargeTime);
			PlayerPrefs.SetFloat ("Player2ChargeTime", m_fPlayer2_ChargeTime);
			PlayerPrefs.SetFloat ("Player3ChargeTime", m_fPlayer3_ChargeTime);
			PlayerPrefs.SetFloat ("Player4ChargeTime", m_fPlayer4_ChargeTime);

			//
			PlayerPrefs.SetInt ("IsCharging0", 0);
			PlayerPrefs.SetInt ("IsCharging1", 0);
			PlayerPrefs.SetInt ("IsCharging2", 0);
			PlayerPrefs.SetInt ("IsCharging3", 0);
			PlayerPrefs.SetInt ("IsCharging4", 0);

			PlayerPrefs.SetInt ("Player0Lock", 0);
			PlayerPrefs.SetInt ("Player1Lock", 1);
			PlayerPrefs.SetInt ("Player2Lock", 1);
			PlayerPrefs.SetInt ("Player3Lock", 1);
			PlayerPrefs.SetInt ("Player4Lock", 1);

			Debug.Log("PlayerPrefs Initialize");
		} 

		//Chartboost.cacheInterstitial (CBLocation.Default);
		Social.localUser.Authenticate ((bool bSuccess) => {});

		m_strPlayerName = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_strName;
		m_iChargePrice = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_iChargePrice;
		m_iBuyPrice = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_iBuyPrice;

		DontDestroyOnLoad (this);
	}
}
