using UnityEngine;
using System.Collections;
using GooglePlayGames;
//using ChartboostSDK;

public partial class BtnManager : MonoBehaviour {

	public PlayerData m_PlayerData = null;
	public GameObject m_objParticles = null;

	private SpringPanel m_SpringPanel = null;
	public GameObject m_objScrollView = null;
	public int m_iCurrnetPlayerIndex = 0;

	public GameObject m_objPlayerData = null;
	public GameObject m_objUICam = null;
	//UI
	public GameObject m_objLogo = null;
	public GameObject m_objScore = null;
	public GameObject m_objBadge = null;

	public GameObject m_objTiredBar = null;
	public GameObject m_objChargeBar = null;
	public GameObject m_objPlayerInfo = null;
	public GameObject m_objEndBack = null;

	public GameObject m_objLeftBtn = null;
	public GameObject m_objRightBtn = null;

	public bool m_bTimeInit = true;
	public bool m_bAdsOn = false;

	public MAINMENU_STATE m_MenuStateID = MAINMENU_STATE.MAIN;

	private float m_fCurrentTired = 100.0f;
	private int m_iCurrentLock = 0;

	private AudioSource m_Audio = null;
	private AudioClip m_BtnClip = null;
	private AudioClip m_ScrollClip = null;
	

	private bool m_bStart = true;


	void Awake()
	{
		m_fCurrentTired = 100.0f;
	}

	void Start()
	{
		//Load Audio
		m_Audio = gameObject.GetComponent<AudioSource> ();
		m_BtnClip = Resources.Load ("Sounds/ogg(96k)/button_click") as AudioClip;
		m_ScrollClip = Resources.Load ("Sounds/ogg(96k)/ui_scroll") as AudioClip;

		GameObject objPlayerData = null;
		
		if (GameObject.Find ("PlayerData(Clone)") == null) {
			objPlayerData = Instantiate (m_objPlayerData) as GameObject;
			Debug.Log("Create PlayerData");
		}
		else
			objPlayerData = GameObject.Find ("PlayerData(Clone)").gameObject;

			m_PlayerData = objPlayerData.GetComponent<PlayerData> ();
	
		//Create Particels

		GameObject objParticles = null;

		if (GameObject.Find ("Particles(Clone)") == null) {
			objParticles = Instantiate (m_objParticles) as GameObject;
		}

		//Set BackParticle
		GameObject.Find ("Particles(Clone)").GetComponent<SetBackParticle> ().m_bInit = false;

		if (m_PlayerData.m_iCurrentPlayNum != 0) {
			m_objLogo.SetActive (false);
			m_objScore.gameObject.SetActive (true);

			if (m_PlayerData.m_bWinHighScore == true)
				m_objBadge.SetActive (true);
			else
				m_objBadge.SetActive (false);
		} else {
			//PlayerPrefs.DeleteAll ();		//for debug
		}

		//show chartboost ad
		if (m_PlayerData.m_iPlayCountForAd == 3) {
			//Chartboost.showInterstitial (CBLocation.Default);
			m_PlayerData.m_iPlayCountForAd = 0;
		}
	
	}

	void FixedUpdate()
	{
		//Input
		if (Input.GetKeyDown (KeyCode.Escape)) {
			MainBackBtn_Click();
		}

		if (m_MenuStateID == MAINMENU_STATE.MAIN) {
			m_SpringPanel = GameObject.Find ("ScrollView").gameObject.GetComponent<SpringPanel> ();

			if (m_bStart == true) {
				m_SpringPanel.target = new Vector3 (m_SpringPanel.target.x + (-1200 * (int)m_PlayerData.m_PlayerID), 0.0f);
				
				m_objScrollView.GetComponent<UICenterOnChild> ().centeredObject = m_objScrollView.transform.GetChild ((int)m_PlayerData.m_PlayerID).gameObject;
				m_objScrollView.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_bSelected = true;
				
				m_SpringPanel.enabled = true;
				
				m_bStart = false;
			}

			//wait
			m_PlayerData.m_PlayerID = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_PlayerID;
			m_PlayerData.m_strPlayerName = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_strName;
			m_PlayerData.m_strPlayerInfo = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_strInfo;
			m_PlayerData.m_iChargePrice = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_iChargePrice;
			m_PlayerData.m_iBuyPrice = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_iBuyPrice;
			m_objScrollView.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_bSelected = true;

		}

		m_fCurrentTired = 100.0f;

		switch (m_PlayerData.m_PlayerID) {
		case PLAYER_ID.NORMAL:
			m_fCurrentTired = PlayerPrefs.GetFloat("fPlayer0Tired");
			m_iCurrentLock = PlayerPrefs.GetInt("Player0Lock");

			break;
		case PLAYER_ID.SPREAD:
			m_fCurrentTired = PlayerPrefs.GetFloat("fPlayer1Tired");
			m_iCurrentLock = PlayerPrefs.GetInt("Player1Lock");
			break;
		case PLAYER_ID.LASER:
			m_fCurrentTired = PlayerPrefs.GetFloat("fPlayer2Tired");
			m_iCurrentLock = PlayerPrefs.GetInt("Player2Lock");
			break;
		case PLAYER_ID.HOMING:
			m_fCurrentTired = PlayerPrefs.GetFloat("fPlayer3Tired");
			m_iCurrentLock = PlayerPrefs.GetInt("Player3Lock");
			break;
		case PLAYER_ID.BOOM:
			m_fCurrentTired = PlayerPrefs.GetFloat("fPlayer4Tired");
			m_iCurrentLock = PlayerPrefs.GetInt("Player4Lock");
			break;
		}

		if (m_iCurrentLock == 0) {
			if (m_fCurrentTired <= 0.0f) {
				m_objTiredBar.SetActive (false);
				m_objChargeBar.SetActive (true);
				m_objPlayerInfo.SetActive(false);
			} else {
				m_objTiredBar.SetActive (true);
				m_objChargeBar.SetActive (false);
				m_objPlayerInfo.SetActive(false);
			}

			//TiredTime Setting 
			TiredTime_Setting ();
		} else {
			m_objTiredBar.SetActive (false);
			m_objChargeBar.SetActive (false);
			m_objPlayerInfo.SetActive(true);
		}


		//Arrow Btn Set
		if (m_iCurrnetPlayerIndex == 0)
			m_objLeftBtn.SetActive (false);
		else
			m_objLeftBtn.SetActive (true);

		if (m_iCurrnetPlayerIndex == m_objScrollView.transform.childCount -1)
			m_objRightBtn.SetActive (false);
		else
			m_objRightBtn.SetActive (true);


		//Ads
		Check_AdsReward ();
	
	}

	public void OnStartBtnClick()
	{

		Play_BtnSound ();

		//m_PlayerData.m_PlayerID = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild>().centeredObject.GetComponent<UI_Playerimg>().m_PlayerID;
		//TiredVal Change----->
		if (m_PlayerData.m_PlayerID == PLAYER_ID.NORMAL) {

			//Buy
			if (m_iCurrentLock == 1) {
				if (m_PlayerData.m_iBuyPrice <= PlayerPrefs.GetInt ("HaveCoin")) {
					OnBuyMenu();
				}
				else
					OnNoMoneyMenu();

				return;
			}
			else
			//Charge
			if(PlayerPrefs.GetFloat("fPlayer0Tired") <= 0.0f)
			{
				OnChargeMenu();
				return;
			}

			PlayerPrefs.SetFloat("fPlayer0Tired", PlayerPrefs.GetFloat("fPlayer0Tired") - 10.0f);
		}
		else if (m_PlayerData.m_PlayerID == PLAYER_ID.SPREAD) {

			//Buy
			if (m_iCurrentLock == 1) {
				if (m_PlayerData.m_iBuyPrice <= PlayerPrefs.GetInt ("HaveCoin")) {
					OnBuyMenu();
				}
				else
					OnNoMoneyMenu();

				return;
			}
			//Charge
			if(PlayerPrefs.GetFloat("fPlayer1Tired") <= 0.0f)
			{
				OnChargeMenu();
				return;
			}
			
			PlayerPrefs.SetFloat("fPlayer1Tired", PlayerPrefs.GetFloat("fPlayer1Tired") - 15.0f);
		}
		else if (m_PlayerData.m_PlayerID == PLAYER_ID.LASER) {

			//Buy
			if (m_iCurrentLock == 1) {

				if(PlayerPrefs.GetInt("Player1Lock") != 0)
				{
					//Lock
					return;
				}

				if (m_PlayerData.m_iBuyPrice <= PlayerPrefs.GetInt ("HaveCoin")) {
					OnBuyMenu();
				}
				else
					OnNoMoneyMenu();

				return;
			}

			//Charge
			if(PlayerPrefs.GetFloat("fPlayer2Tired") <= 0.0f)
			{
				OnChargeMenu();
				return;
			}
			
			PlayerPrefs.SetFloat("fPlayer2Tired", PlayerPrefs.GetFloat("fPlayer2Tired") - 18.0f);
		}
		else if (m_PlayerData.m_PlayerID == PLAYER_ID.HOMING) {
			//Buy
			if (m_iCurrentLock == 1) {
				
				if (m_PlayerData.m_iBuyPrice <= PlayerPrefs.GetInt ("HaveCoin")) {
					OnBuyMenu();
				}
				else
					OnNoMoneyMenu();

				return;
			}

			//Charge
			if(PlayerPrefs.GetFloat("fPlayer3Tired") <= 0.0f)
			{
				OnChargeMenu();
				return;
			}
			
			PlayerPrefs.SetFloat("fPlayer3Tired", PlayerPrefs.GetFloat("fPlayer3Tired") - 20.0f);
		}else if (m_PlayerData.m_PlayerID == PLAYER_ID.BOOM) {
			//Buy
			if (m_iCurrentLock == 1) {
				if (m_PlayerData.m_iBuyPrice <= PlayerPrefs.GetInt ("HaveCoin")) {
					OnBuyMenu();
				}
				else
					OnNoMoneyMenu();

				return;
			}

			//Charge
			if(PlayerPrefs.GetFloat("fPlayer4Tired") <= 0.0f)
			{
				OnChargeMenu();
				return;
			}
			
			PlayerPrefs.SetFloat("fPlayer4Tired", PlayerPrefs.GetFloat("fPlayer4Tired") - 25.0f);
		}
		//<-----End

		//Game Start 
		m_objScrollView.SetActive (false);
		m_objEndBack.SetActive (true);
		m_PlayerData.m_iCurrentPlayNum += 1;
		PlayerPrefs.SetInt ("PlayNum", PlayerPrefs.GetInt("PlayNum") + 1);
		Application.LoadLevel ("02_Game");
		m_bTimeInit = false;

	}

	public void LeftBtnClick() 
	{
		Play_ScrollSound ();
		if (m_iCurrnetPlayerIndex == 0)
			return;

		m_SpringPanel.target = new Vector3(m_SpringPanel.target.x + 1200.0f, 0.0f);

		m_objScrollView.GetComponent<UICenterOnChild> ().centeredObject = m_objScrollView.transform.GetChild (m_iCurrnetPlayerIndex - 1).gameObject;
		m_PlayerData.m_PlayerID = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_PlayerID;
		m_PlayerData.m_strPlayerName = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_strName;
		m_PlayerData.m_strPlayerInfo = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_strInfo;
		m_PlayerData.m_iChargePrice = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_iChargePrice;
		m_PlayerData.m_iBuyPrice = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_iBuyPrice;
		m_objScrollView.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_bSelected = true;
			
		m_SpringPanel.enabled = true;
		//TiredTime_Setting ();
	}
	public void RightBtnClick()
	{
		Play_ScrollSound ();
		if (m_iCurrnetPlayerIndex + 1== m_objScrollView.transform.childCount)
			return;

		m_SpringPanel.target = new Vector3(m_SpringPanel.target.x - 1200.0f, 0.0f);

		m_objScrollView.GetComponent<UICenterOnChild> ().centeredObject = m_objScrollView.transform.GetChild (m_iCurrnetPlayerIndex + 1).gameObject;
		m_PlayerData.m_PlayerID = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_PlayerID;
		m_PlayerData.m_strPlayerName = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_strName;
		m_PlayerData.m_strPlayerInfo = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_strInfo;
		m_PlayerData.m_iChargePrice = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_iChargePrice;
		m_PlayerData.m_iBuyPrice = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_iBuyPrice;
		m_objScrollView.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_bSelected = true;

		m_SpringPanel.enabled = true;
		//TiredTime_Setting ();
	}


	//Option
	public void OptionBtnClick()
	{
		Play_BtnSound ();
		m_objUICam.transform.FindChild ("Main").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("Option").gameObject.SetActive(true);

		m_MenuStateID = MAINMENU_STATE.OPTION;
	}
	//Shop
	public void ShopBtnClick()
	{
		Play_BtnSound ();
		m_objUICam.transform.FindChild ("Main").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("NoMoney").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("Shop").gameObject.SetActive(true);
		
		m_MenuStateID = MAINMENU_STATE.SHOP;
	}

	public void LeaderboardBtn_Click()
	{
		((PlayGamesPlatform)Social.Active).ShowLeaderboardUI ("Scroe");
	}

	public void OnBuyMenu()
	{
		Play_BtnSound ();
		m_objUICam.transform.FindChild ("Main").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("Buy").gameObject.SetActive(true);
		
		m_MenuStateID = MAINMENU_STATE.CHARGE;
	}

	public void OnNoMoneyMenu()
	{
		Play_BtnSound ();
		m_objUICam.transform.FindChild ("Main").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("Charge").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("NoMoney").gameObject.SetActive(true);
		
		m_MenuStateID = MAINMENU_STATE.CHARGE;
	}

	public void MainBackBtn_Click()
	{
		Play_BtnSound ();
		m_objUICam.transform.FindChild ("Option").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("Shop").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("Charge").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("Buy").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("NoMoney").gameObject.SetActive(false);

		m_objUICam.transform.FindChild ("Main").gameObject.SetActive(true);

		Time.timeScale = 1;
		m_MenuStateID = MAINMENU_STATE.MAIN;
	}

	public void OnChargeMenu()
	{
		Play_BtnSound ();
		m_objUICam.transform.FindChild ("Main").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("Charge").gameObject.SetActive(true);
		m_MenuStateID = MAINMENU_STATE.CHARGE;
	}

	public void OnMoneyChargeBtn()
	{
		Play_BtnSound ();
		if (m_PlayerData.m_iChargePrice <= PlayerPrefs.GetInt ("HaveCoin")) {

			PlayerPrefs.SetInt ("HaveCoin", PlayerPrefs.GetInt ("HaveCoin") - m_PlayerData.m_iChargePrice);

			switch (m_PlayerData.m_PlayerID) {
			case PLAYER_ID.NORMAL:
				PlayerPrefs.SetFloat ("fPlayer0Tired", 100.0f);
				PlayerPrefs.SetInt ("IsCharging0", 0);
				break;
			case PLAYER_ID.SPREAD:
				PlayerPrefs.SetFloat ("fPlayer1Tired", 100.0f);
				PlayerPrefs.SetInt ("IsCharging1", 0);
				break;
			case PLAYER_ID.LASER:
				PlayerPrefs.SetFloat ("fPlayer2Tired", 100.0f);
				PlayerPrefs.SetInt ("IsCharging2", 0);
				break;
			case PLAYER_ID.HOMING:
				PlayerPrefs.SetFloat ("fPlayer3Tired", 100.0f);
				PlayerPrefs.SetInt ("IsCharging3", 0);
				break;
			case PLAYER_ID.BOOM:
				PlayerPrefs.SetFloat ("fPlayer4Tired", 100.0f);
				PlayerPrefs.SetInt ("IsCharging4", 0);
				break;
			}

			MainBackBtn_Click ();
		} else
			OnNoMoneyMenu ();
	}

	public void OnClickBuyBtn()
	{
		Play_BtnSound ();
		if (m_PlayerData.m_iBuyPrice <= PlayerPrefs.GetInt ("HaveCoin")) {
		
			switch (m_PlayerData.m_PlayerID) {
			case PLAYER_ID.NORMAL:
				PlayerPrefs.SetInt("HaveCoin", PlayerPrefs.GetInt ("HaveCoin") -m_PlayerData.m_iBuyPrice);
				PlayerPrefs.SetInt("Player0Lock", 0);
				break;
			case PLAYER_ID.SPREAD:
				PlayerPrefs.SetInt("HaveCoin", PlayerPrefs.GetInt ("HaveCoin") -m_PlayerData.m_iBuyPrice);
				PlayerPrefs.SetInt("Player1Lock", 0);
				break;
			case PLAYER_ID.LASER:
				PlayerPrefs.SetInt("HaveCoin", PlayerPrefs.GetInt ("HaveCoin") -m_PlayerData.m_iBuyPrice);
				PlayerPrefs.SetInt("Player2Lock", 0);
				break;
			case PLAYER_ID.HOMING:
				PlayerPrefs.SetInt("HaveCoin", PlayerPrefs.GetInt ("HaveCoin") -m_PlayerData.m_iBuyPrice);
				PlayerPrefs.SetInt("Player3Lock", 0);
				break;
			case PLAYER_ID.BOOM:
				PlayerPrefs.SetInt("HaveCoin", PlayerPrefs.GetInt ("HaveCoin") -m_PlayerData.m_iBuyPrice);
				PlayerPrefs.SetInt("Player4Lock", 0);
				break;
			}
			
			MainBackBtn_Click();
		}
	}

	public void ChargeTired()
	{
			switch (m_PlayerData.m_PlayerID) {
		case PLAYER_ID.NORMAL:
			PlayerPrefs.SetFloat ("fPlayer0Tired", 100.0f);
			PlayerPrefs.SetInt ("IsCharging0", 0);
			break;
		case PLAYER_ID.SPREAD:
			PlayerPrefs.SetFloat ("fPlayer1Tired", 100.0f);
			PlayerPrefs.SetInt ("IsCharging1", 0);
			break;
		case PLAYER_ID.LASER:

			Debug.Log("charge");
			PlayerPrefs.SetFloat ("fPlayer2Tired", 100.0f);
			PlayerPrefs.SetInt ("IsCharging2", 0);
			break;
		case PLAYER_ID.HOMING:
			PlayerPrefs.SetFloat ("fPlayer3Tired", 100.0f);
			PlayerPrefs.SetInt ("IsCharging3", 0);
			break;
		case PLAYER_ID.BOOM:
			PlayerPrefs.SetFloat ("fPlayer4Tired", 100.0f);
			PlayerPrefs.SetInt ("IsCharging4", 0);
			break;
		}

			MainBackBtn_Click();
	}


	public void TiredTime_Setting()
	{
		if (!m_bTimeInit)
			return;

		System.DateTime CurrentTime = System.DateTime.Now;
		
		//player0 Chargetime setting
		if (PlayerPrefs.GetFloat ("fPlayer0Tired") <= 0.0f) {

			if(PlayerPrefs.GetInt("IsCharging0") == 0)		//Time Setting
			{

				PlayerPrefs.SetInt ("IsCharging0", 1);
				CurrentTime = CurrentTime.AddMinutes(PlayerPrefs.GetFloat("Player0ChargeTime"));
				
				PlayerPrefs.SetInt("TargetYear0", CurrentTime.Year);
				PlayerPrefs.SetInt("TargetMonth0", CurrentTime.Month);
				PlayerPrefs.SetInt("TargetDay0", CurrentTime.Day);
				PlayerPrefs.SetInt("TargetHour0", CurrentTime.Hour);
				PlayerPrefs.SetInt("TargetMin0", CurrentTime.Minute);
				PlayerPrefs.SetInt("TargetSec0", CurrentTime.Second);
			}

			System.DateTime TargetTime = new System.DateTime(PlayerPrefs.GetInt("TargetYear0"), PlayerPrefs.GetInt("TargetMonth0"), PlayerPrefs.GetInt("TargetDay0"),
			                                                 PlayerPrefs.GetInt("TargetHour0"),  PlayerPrefs.GetInt("TargetMin0"),  PlayerPrefs.GetInt("TargetSec0") ); 
			
			if(TargetTime <= System.DateTime.Now)
			{
				PlayerPrefs.SetInt ("IsCharging0", 0);
				ChargeTired();
			}

		}
		
		//player1 Chargetime setting
		if (PlayerPrefs.GetFloat ("fPlayer1Tired") <= 0.0f) {
			
			CurrentTime = System.DateTime.Now;
			
			if(PlayerPrefs.GetInt("IsCharging1") == 0)		//Time Setting
			{
				PlayerPrefs.SetInt ("IsCharging1", 1);
				CurrentTime = CurrentTime.AddMinutes(PlayerPrefs.GetFloat("Player1ChargeTime"));
				
				PlayerPrefs.SetInt("TargetYear1", CurrentTime.Year);
				PlayerPrefs.SetInt("TargetMonth1", CurrentTime.Month);
				PlayerPrefs.SetInt("TargetDay1", CurrentTime.Day);
				PlayerPrefs.SetInt("TargetHour1", CurrentTime.Hour);
				PlayerPrefs.SetInt("TargetMin1", CurrentTime.Minute);
				PlayerPrefs.SetInt("TargetSec1", CurrentTime.Second);
			}	

			System.DateTime TargetTime = new System.DateTime(PlayerPrefs.GetInt("TargetYear1"), PlayerPrefs.GetInt("TargetMonth1"), PlayerPrefs.GetInt("TargetDay1"),
			                                                 PlayerPrefs.GetInt("TargetHour1"),  PlayerPrefs.GetInt("TargetMin1"),  PlayerPrefs.GetInt("TargetSec1") ); 
			
			if(TargetTime <= System.DateTime.Now)
			{
				PlayerPrefs.SetInt ("IsCharging1", 0);
				ChargeTired();
			}
		}
		
		//player2 Chargetime setting
		if (PlayerPrefs.GetFloat ("fPlayer2Tired") <= 0.0f) {
			
			CurrentTime = System.DateTime.Now;
			
			if(PlayerPrefs.GetInt("IsCharging2") == 0)		//Time Setting
			{
				PlayerPrefs.SetInt ("IsCharging2", 1);
				CurrentTime = CurrentTime.AddMinutes(PlayerPrefs.GetFloat("Player2ChargeTime"));
				
				PlayerPrefs.SetInt("TargetYear2", CurrentTime.Year);
				PlayerPrefs.SetInt("TargetMonth2", CurrentTime.Month);
				PlayerPrefs.SetInt("TargetDay2", CurrentTime.Day);
				PlayerPrefs.SetInt("TargetHour2", CurrentTime.Hour);
				PlayerPrefs.SetInt("TargetMin2", CurrentTime.Minute);
				PlayerPrefs.SetInt("TargetSec2", CurrentTime.Second);
			}

			System.DateTime TargetTime = new System.DateTime(PlayerPrefs.GetInt("TargetYear2"), PlayerPrefs.GetInt("TargetMonth2"), PlayerPrefs.GetInt("TargetDay2"),
			                                                 PlayerPrefs.GetInt("TargetHour2"),  PlayerPrefs.GetInt("TargetMin2"),  PlayerPrefs.GetInt("TargetSec2") ); 
			
			if(TargetTime <= System.DateTime.Now)
			{
				PlayerPrefs.SetInt ("IsCharging2", 0);
				ChargeTired();
			}

		}
		
		//player3 Chargetime setting
		if (PlayerPrefs.GetFloat ("fPlayer3Tired") <= 0.0f) {
			
			CurrentTime = System.DateTime.Now;
			
			if(PlayerPrefs.GetInt("IsCharging3") == 0)		//Time Setting
			{
				PlayerPrefs.SetInt ("IsCharging3", 1);
				CurrentTime = CurrentTime.AddMinutes(PlayerPrefs.GetFloat("Player3ChargeTime"));
				
				PlayerPrefs.SetInt("TargetYear3", CurrentTime.Year);
				PlayerPrefs.SetInt("TargetMonth3", CurrentTime.Month);
				PlayerPrefs.SetInt("TargetDay3", CurrentTime.Day);
				PlayerPrefs.SetInt("TargetHour3", CurrentTime.Hour);
				PlayerPrefs.SetInt("TargetMin3", CurrentTime.Minute);
				PlayerPrefs.SetInt("TargetSec3", CurrentTime.Second);
			}

			System.DateTime TargetTime = new System.DateTime(PlayerPrefs.GetInt("TargetYear3"), PlayerPrefs.GetInt("TargetMonth3"), PlayerPrefs.GetInt("TargetDay3"),
			                                                 PlayerPrefs.GetInt("TargetHour3"),  PlayerPrefs.GetInt("TargetMin3"),  PlayerPrefs.GetInt("TargetSec3") ); 
			
			if(TargetTime <= System.DateTime.Now)
			{
				PlayerPrefs.SetInt ("IsCharging3", 0);
				ChargeTired();
			}
			
		}
		
		//player4 Chargetime setting
		if (PlayerPrefs.GetFloat ("fPlayer4Tired") <= 0.0f) {
			
			CurrentTime = System.DateTime.Now;
			
			if(PlayerPrefs.GetInt("IsCharging4") == 0)		//Time Setting
			{
				PlayerPrefs.SetInt ("IsCharging4", 1);
				CurrentTime = CurrentTime.AddMinutes(PlayerPrefs.GetFloat("Player4ChargeTime"));
				
				PlayerPrefs.SetInt("TargetYear4", CurrentTime.Year);
				PlayerPrefs.SetInt("TargetMonth4", CurrentTime.Month);
				PlayerPrefs.SetInt("TargetDay4", CurrentTime.Day);
				PlayerPrefs.SetInt("TargetHour4", CurrentTime.Hour);
				PlayerPrefs.SetInt("TargetMin4", CurrentTime.Minute);
				PlayerPrefs.SetInt("TargetSec4", CurrentTime.Second);
			}

			System.DateTime TargetTime = new System.DateTime(PlayerPrefs.GetInt("TargetYear4"), PlayerPrefs.GetInt("TargetMonth4"), PlayerPrefs.GetInt("TargetDay4"),
			                                                 PlayerPrefs.GetInt("TargetHour4"),  PlayerPrefs.GetInt("TargetMin4"),  PlayerPrefs.GetInt("TargetSec4") ); 
			
			if(TargetTime <= System.DateTime.Now)
			{
				PlayerPrefs.SetInt ("IsCharging4", 0);
				ChargeTired();
			}
			
		}



	}

	//unityads
	public void AdsBtnClick()
	{
		AdFuctions.ShowUnityAds ();
		m_bAdsOn = true;
		
	}

	public void Check_AdsReward()
	{
		if (m_bAdsOn == true) {

			if(AdFuctions.m_bAdsComplete == true)
			{

			int iPlayerId = (int)m_PlayerData.m_PlayerID;

			PlayerPrefs.SetFloat (string.Format("fPlayer{0}Tired", iPlayerId), 100.0f);
			PlayerPrefs.SetInt (string.Format("IsCharging{0}", iPlayerId), 0);

				AdFuctions.m_bAdsComplete = false;
				m_bAdsOn = false;
				MainBackBtn_Click();
			}
	
		}
	}


	public void Play_BtnSound()
	{
		m_Audio.clip = m_BtnClip;
		m_Audio.Play();
	}

	public void Play_ScrollSound()
	{
		m_Audio.clip = m_ScrollClip;
		m_Audio.Play();
	}

}
