using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ChartboostSDK;
using TapjoyUnity;

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

	public GameObject m_NetworkFail_Label = null;

	public MAINMENU_STATE m_MenuStateID = MAINMENU_STATE.MAIN;

	private float m_fCurrentTired = 100.0f;
	private bool m_bCurrentLock = false;

	private AudioSource m_Audio = null;
	private AudioClip m_BtnClip = null;
	private AudioClip m_BuyBtnClip = null;
	private AudioClip m_ScrollClip = null;

	public GameObject m_objSoundBtn = null;
	public GameObject m_objSdkMgr = null;
	private GameSDKManager m_SdkMgr = null;

	private bool m_bSelectChk = false;


	//ad
	public bool m_bIsDismissAD = false;
	public bool m_bVideoAdsOn = false;
	public bool m_bPopupAdsOn = false;
	
	private bool m_bStart = true;
	private bool m_bInfoUpdate = true;

	private float m_fRewardCheckTimer = 0.0f;
	private float m_fRewardCheckTerm = 3.0f;

	private bool m_bIsclickArrowBtn = false;
	private float m_SelectTerm = 0.01f;
	private float m_SelectTimer = 0.0f;


	void Awake()
	{
		m_fCurrentTired = 100.0f;

			if ((Screen.width / 3) * 4 == Screen.height)
				Camera.main.orthographicSize = 4.5f;
	}

	void Start()
	{
		//Debug.Log ("BtnManager");
		//Load Audio
		m_Audio = gameObject.GetComponent<AudioSource> ();
		m_BtnClip = Resources.Load ("Sounds/ogg(96k)/button_click") as AudioClip;
		m_BuyBtnClip = Resources.Load ("Sounds/ogg(96k)/button_buy") as AudioClip;
		m_ScrollClip = Resources.Load ("Sounds/ogg(96k)/ui_scroll") as AudioClip;


		//Create Singleton or Set Getter
		if (GameObject.Find ("GameSDKManager(Clone)") == null) {
			GameObject objSdkMgr = Instantiate (m_objSdkMgr) as GameObject;
			m_SdkMgr = m_objSdkMgr.GetComponent<GameSDKManager> ();
		} else
			m_SdkMgr = GameObject.Find ("GameSDKManager(Clone)").GetComponent<GameSDKManager>();

		GameObject objPlayerData = null;
		
		if (GameObject.Find ("PlayerData(Clone)") == null) {
			objPlayerData = Instantiate (m_objPlayerData) as GameObject;
			//Debug.Log("Create PlayerData");
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

		//Main Logo Change --> Scoreboard 
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


		//Main Menu Initialize here (Srry)\

		//Sound Btn Set
		if (AudioListener.volume == 0) {
			m_objSoundBtn.transform.GetChild (0).gameObject.SetActive (false);
			m_objSoundBtn.transform.GetChild (1).gameObject.SetActive (true);
		} else {
			m_objSoundBtn.transform.GetChild (0).gameObject.SetActive (true);
			m_objSoundBtn.transform.GetChild (1).gameObject.SetActive (false);
		}

		//ads

		#if !UNITY_EDITOR_OSX
		if (PlayerPrefs.GetInt("Adoff") == 0) {
			AdFunctions.Show_GoogleADBanner ();
		}
		else {
			AdFunctions.Hide_GoogleADBanner ();
		}
		#endif

		//starscore, facebook likehttps://www.youtube.com/watch?v=PZbkF-15Obu

		if (m_PlayerData.m_bCanShowRate == false) {

			Debug.Log(PlayerPrefs.GetInt ("CurrentPlayNum"));
			if ((PlayerPrefs.GetInt ("CurrentPlayNum") == 5
			    ||PlayerPrefs.GetInt ("CurrentPlayNum") == 15
			    ||PlayerPrefs.GetInt ("CurrentPlayNum") == 50)) {
				m_PlayerData.m_bCanShowRate = true;
			} 
		} else
			OnRateMenu ();

		if (m_PlayerData.m_bCanShowFB == false) {
			if ((PlayerPrefs.GetInt ("CurrentPlayNum") == 10
				|| PlayerPrefs.GetInt ("CurrentPlayNum") == 20
				|| PlayerPrefs.GetInt ("CurrentPlayNum") == 55)) {
				m_PlayerData.m_bCanShowFB = true;
			}
		} else
			OnFBLikeMenu ();

		//Game Save
		m_PlayerData.GameData_Save ();
		m_bInfoUpdate = true;
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

				if(m_PlayerData.m_PlayerID == PLAYER_ID.NORMAL)
					m_objLeftBtn.SetActive(false);

				m_bStart = false;
			}

			//wait
			if(m_SpringPanel.isActiveAndEnabled == true)
				m_bSelectChk = true;


			if((m_bSelectChk == true &&
			   Mathf.Abs (m_objScrollView.transform.localPosition.x - m_SpringPanel.target.x) > 50.0f)
			   || m_bInfoUpdate == true)
			{
				m_bSelectChk = false;

				m_PlayerData.m_PlayerID = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_PlayerID;
				m_PlayerData.m_strPlayerName = Localization.Get(GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_strName_KEY);
				m_PlayerData.m_strPlayerInfo = Localization.Get(GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_strInfo_KEY);
				m_PlayerData.m_iChargePrice = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_iChargePrice;
				m_PlayerData.m_iBuyPrice = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_iBuyPrice;
				m_objScrollView.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_bSelected = true;
				
				
				//Arrow Btn Set
				if (m_iCurrnetPlayerIndex == 0)
					m_objLeftBtn.SetActive (false);
				else
					m_objLeftBtn.SetActive (true);
				
				if (m_iCurrnetPlayerIndex == m_objScrollView.transform.childCount -1)
					m_objRightBtn.SetActive (false);
				else
					m_objRightBtn.SetActive (true);

				m_bInfoUpdate = false;
			}

			m_fCurrentTired = 100.0f;
			//
			m_fCurrentTired = m_PlayerData.m_Gamedata.m_PlayerInfo[(int)m_PlayerData.m_PlayerID].fTiredPercent;
			m_bCurrentLock = m_PlayerData.m_Gamedata.m_PlayerInfo[(int)m_PlayerData.m_PlayerID].bIsLock;
			
			if (m_bCurrentLock == false) {
				if (m_fCurrentTired <= 0.0f) {
					m_objTiredBar.SetActive (false);
					m_objChargeBar.SetActive (true);
					m_objPlayerInfo.SetActive(false);
				} else {
					m_objTiredBar.SetActive (true);
					m_objChargeBar.SetActive (false);
					m_objPlayerInfo.SetActive(false);
				}
			} else {
				m_objTiredBar.SetActive (false);
				m_objChargeBar.SetActive (false);
				m_objPlayerInfo.SetActive(true);
			}
			
		}

		//Check TireTIme
		TiredTime_Setting ();
		
		
		//Ads
		Check_AdsReward ();
		Chartboost.didDismissInterstitial += delegate {
			m_bIsDismissAD = true;
		};

		//Tapjoy Reward Check
		if (TapjoyManager.Instance.m_bCheckreward == true) {
			Tapjoy.GetCurrencyBalance ();

			if(m_fRewardCheckTimer >= m_fRewardCheckTerm)
			{
				m_fRewardCheckTimer = 0.0f;
				TapjoyManager.Instance.m_bCheckreward = false;
			}
			else
			m_fRewardCheckTimer += Time.deltaTime;

		}

		int iTjCoin = TapjoyManager.Instance.TakeTjCurrency ();
		if (iTjCoin != 0) {
			Play_BuyBtnSound();
			m_PlayerData.m_Gamedata.m_iHaveCoin += iTjCoin;
		}

		#if !UNITY_EDITOR_OSX
		if (m_PlayerData.m_iCurrentPlayNum == 1)
		if (AdFunctions.m_bTjNoticeDismiss) {
			AdFunctions.m_bTjNoticeDismiss = false;
			m_PlayerData.m_Gamedata.Spend_TiredVal (m_PlayerData.m_PlayerID);
			GameStart ();
		}

		if (m_bIsDismissAD == true
		    || AdFunctions.Check_IsClose_GooglePopup () == true) {

			m_bIsDismissAD = false;
			AdFunctions.Set_IsClose_GooglePopup();


			m_PlayerData.m_iPlayCountForAd = 0;
			m_PlayerData.m_Gamedata.Spend_TiredVal (m_PlayerData.m_PlayerID);
			GameStart ();
		}
		#endif

		//for debug
		if (Input.GetKeyDown (KeyCode.LeftArrow))
			LeftBtnClick ();
		if (Input.GetKeyDown (KeyCode.RightArrow))
			RightBtnClick ();

		if (m_bIsclickArrowBtn == true) {
			m_SelectTimer += Time.deltaTime;
			
			if(m_SelectTimer >= m_SelectTerm)
			{
				m_bIsclickArrowBtn = false;
				m_SelectTimer = 0.0f;
			}
		}

#if UNITY_IOS
		//Purchase
		if(m_SdkMgr.GetIsPurchasing())
			m_objWaitback.SetActive(true);
		else
			m_objWaitback.SetActive(false);
#endif
			
	}

	//Startbtn
	public void OnStartBtnClick()
	{

		Play_BtnSound ();

		if (Mathf.Abs (m_objScrollView.transform.localPosition.x - m_SpringPanel.target.x) > 200.0f) {
				Debug.Log(string.Format("ScrollViewX : {0}, SprinTargetX : {1}", m_objScrollView.transform.position.x, m_SpringPanel.target.x));

			return;
		}

			//Buy
		if (m_bCurrentLock == true) {

			if (m_PlayerData.m_iBuyPrice <= m_PlayerData.m_Gamedata.m_iHaveCoin) 
				OnBuyMenu ();
			else
				OnNoMoneyMenu ();

			return;
		} else {
			//Charge
			if (m_PlayerData.m_Gamedata.m_PlayerInfo [(int)m_PlayerData.m_PlayerID].fTiredPercent <= 0.0f) {
				OnChargeMenu ();
				return;
			}
		}

		#if UNITY_EDITOR_OSX
		m_PlayerData.m_Gamedata.Spend_TiredVal (m_PlayerData.m_PlayerID);
		GameStart ();
		return;
		#endif

		if (TapjoyManager.Instance.m_bCheckreward == true) {
			Debug.Log("TapjoyManager.Instance.m_bCheckreward == true");
			return;
		}

		//show chartboost ad
		if (PlayerPrefs.GetInt("Adoff") == 1) {

			m_PlayerData.m_Gamedata.Spend_TiredVal (m_PlayerData.m_PlayerID);
			GameStart ();
		} 
		else {
			if (m_PlayerData.m_iPlayCountForAd == 3
			   && (Application.internetReachability != NetworkReachability.NotReachable)) {
			
				if (m_bPopupAdsOn == false) {
					if (Chartboost.hasInterstitial (CBLocation.Default))
						Chartboost.showInterstitial (CBLocation.Default);
					else if(AdFunctions.IsLoadedPopup())
						AdFunctions.Show_GoogleADPopup();
					else
						GameStart();
				}
			
				m_bPopupAdsOn = true;
			} else {
				if (m_PlayerData.m_iCurrentPlayNum == 1
				   && (Application.internetReachability != NetworkReachability.NotReachable))
					TapjoyManager.Instance.m_TjNotice.ShowContent ();
				else {
					m_PlayerData.m_Gamedata.Spend_TiredVal (m_PlayerData.m_PlayerID);
					GameStart ();
				}
			}
		}
		//Game Start 
	
	}

	public void LeftBtnClick() 
	{
		Play_ScrollSound ();
		if (m_iCurrnetPlayerIndex == 0 ||
		    m_bIsclickArrowBtn == true)
			return;

		//m_SpringPanel.target = new Vector3 (m_SpringPanel.target.x + (1200 * m_iCurrnetPlayerIndex -1), 0.0f);
		m_SpringPanel.target = new Vector3(m_SpringPanel.target.x + 1200.0f, 0.0f);

		m_objScrollView.GetComponent<UICenterOnChild> ().centeredObject = m_objScrollView.transform.GetChild (m_iCurrnetPlayerIndex - 1).gameObject;
		m_PlayerData.m_PlayerID = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_PlayerID;
		m_PlayerData.m_strPlayerName = Localization.Get(GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_strName_KEY);
		m_PlayerData.m_strPlayerInfo = Localization.Get(GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_strInfo_KEY);
		m_PlayerData.m_iChargePrice = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_iChargePrice;
		m_PlayerData.m_iBuyPrice = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_iBuyPrice;
		m_objScrollView.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_bSelected = true;
			
		m_SpringPanel.enabled = true;
		m_bIsclickArrowBtn = true;
		//TiredTime_Setting ();
	}
	public void RightBtnClick()
	{
		Play_ScrollSound ();
		if (m_iCurrnetPlayerIndex + 1== m_objScrollView.transform.childCount
		    || m_bIsclickArrowBtn == true)
			return;

		//m_SpringPanel.target = new Vector3 (m_SpringPanel.target.x - (1200 * m_iCurrnetPlayerIndex +1), 0.0f);
		m_SpringPanel.target = new Vector3(m_SpringPanel.target.x - 1200.0f, 0.0f);

		m_objScrollView.GetComponent<UICenterOnChild> ().centeredObject = m_objScrollView.transform.GetChild (m_iCurrnetPlayerIndex + 1).gameObject;
		m_PlayerData.m_PlayerID = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_PlayerID;
		m_PlayerData.m_strPlayerName = Localization.Get(GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_strName_KEY);
		m_PlayerData.m_strPlayerInfo = Localization.Get(GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_strInfo_KEY);
		m_PlayerData.m_iChargePrice = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_iChargePrice;
		m_PlayerData.m_iBuyPrice = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_iBuyPrice;
		m_objScrollView.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_bSelected = true;

		m_SpringPanel.enabled = true;
		m_bIsclickArrowBtn = true;
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
		if (m_bPopupAdsOn == true)
			return;

		Play_BtnSound ();
		m_objUICam.transform.FindChild ("Main").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("NoMoney").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("Shop").gameObject.SetActive(true);
		
		m_MenuStateID = MAINMENU_STATE.SHOP;
	}

	public void LeaderboardBtn_Click()
	{
		m_SdkMgr.Show_LeaderBoard ();

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

	public void OnRateMenu()
	{
		if (m_PlayerData.m_bCanShowRate == false
		    || m_PlayerData.m_iCurrentPlayNum == 0)
			return;


		m_objUICam.transform.FindChild ("Main").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("Charge").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("NoMoney").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("Starscore").gameObject.SetActive(true);

		m_MenuStateID = MAINMENU_STATE.CHARGE;

		m_PlayerData.m_bCanShowRate = false;
	}

	public void OnFBLikeMenu()
	{
		if (m_PlayerData.m_bCanShowFB == false
		    || m_PlayerData.m_iCurrentPlayNum == 0)
			return;

		m_objUICam.transform.FindChild ("Main").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("Charge").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("NoMoney").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("FBLike").gameObject.SetActive(true);

		m_MenuStateID = MAINMENU_STATE.CHARGE;

		m_PlayerData.m_bCanShowFB = false;
	}

	public void MainBackBtn_Click()
	{
		Play_BtnSound ();
		m_objUICam.transform.FindChild ("Option").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("Shop").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("Charge").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("Buy").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("NoMoney").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("Starscore").gameObject.SetActive(false);
		m_objUICam.transform.FindChild ("FBLike").gameObject.SetActive(false);

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
		Play_BuyBtnSound ();
		if (m_PlayerData.m_iChargePrice <= m_PlayerData.m_Gamedata.m_iHaveCoin) {

			string strPlayerTrackID = "";

			switch (m_PlayerData.m_PlayerID) {

			case PLAYER_ID.NORMAL:
				strPlayerTrackID = "NORMAL";
				break;
			case PLAYER_ID.SPREAD:
				strPlayerTrackID = "SPREAD";
				break;
			case PLAYER_ID.LASER:
				strPlayerTrackID = "LASER";
				break;
			case PLAYER_ID.HOMING:
				strPlayerTrackID = "HOMING";
				break;
			case PLAYER_ID.BOOM:
				strPlayerTrackID = "BOOM";
				break;

			}

			TapjoyManager.Instance.TrackCustomEvent ("Buy_Playercharge", strPlayerTrackID, "Coin :" + m_PlayerData.m_Gamedata.m_iHaveCoin, "");

			m_PlayerData.m_Gamedata.m_iHaveCoin -= m_PlayerData.m_iChargePrice;

			m_PlayerData.m_Gamedata.m_PlayerInfo[(int)m_PlayerData.m_PlayerID].fTiredPercent = 100.0f;
			m_PlayerData.m_Gamedata.m_PlayerInfo[(int)m_PlayerData.m_PlayerID].bIsSleep = false;

			m_PlayerData.GameData_Save ();
			m_bInfoUpdate = true;
			MainBackBtn_Click ();
		} else
			OnNoMoneyMenu ();
	}

	public void OnClickBuyBtn()
	{
		Play_BuyBtnSound ();
		if (m_PlayerData.m_iBuyPrice <= m_PlayerData.m_Gamedata.m_iHaveCoin) {

			string strPlayerTrackID = "";

			switch (m_PlayerData.m_PlayerID) {

			case PLAYER_ID.NORMAL:
				strPlayerTrackID = "NORMAL";
				break;
			case PLAYER_ID.SPREAD:
				strPlayerTrackID = "SPREAD";
				break;
			case PLAYER_ID.LASER:
				strPlayerTrackID = "LASER";
				break;
			case PLAYER_ID.HOMING:
				strPlayerTrackID = "HOMING";
				break;
			case PLAYER_ID.BOOM:
				strPlayerTrackID = "BOOM";
				break;

			}

			TapjoyManager.Instance.TrackCustomEvent ("Buy_Player", strPlayerTrackID, "PlayerNum :" + m_PlayerData.m_Gamedata.m_iTotalPlayerNum + " Coin :" + m_PlayerData.m_Gamedata.m_iHaveCoin, "");
		
			m_PlayerData.m_Gamedata.m_iHaveCoin -= m_PlayerData.m_iBuyPrice;
			m_PlayerData.m_Gamedata.m_PlayerInfo[(int)m_PlayerData.m_PlayerID].bIsLock = false;

			m_PlayerData.GameData_Save ();
			m_bInfoUpdate = true;
			MainBackBtn_Click();
		}
	}

	public void ChargeTired()
	{
		m_PlayerData.m_Gamedata.m_PlayerInfo [(int)m_PlayerData.m_PlayerID].fTiredPercent = 100.0f;
		m_PlayerData.m_Gamedata.m_PlayerInfo [(int)m_PlayerData.m_PlayerID].bIsSleep = false;


		MainBackBtn_Click();
	}


	public void TiredTime_Setting()
	{
		if (!m_bTimeInit)
			return;

		System.DateTime CurrentTime = System.DateTime.Now;


		//player change state to SLEEP 

		for (int i = 0; i < m_PlayerData.m_Gamedata.m_iTotalPlayerNum; ++i) {
			if (m_PlayerData.m_Gamedata.m_PlayerInfo [i].fTiredPercent <= 0.0f) {

				if (m_PlayerData.m_Gamedata.m_PlayerInfo [i].bIsSleep == false) {		//Time Setting

					m_PlayerData.m_Gamedata.m_PlayerInfo [i].bIsSleep = true;
					CurrentTime = CurrentTime.AddMinutes (m_PlayerData.m_Gamedata.m_PlayerInfo [i].iSleepMin);
					m_PlayerData.m_Gamedata.m_PlayerInfo [i].SleepEnd_Time = CurrentTime;
					Debug.Log("TiredTime_Setting");
					m_PlayerData.GameData_Save ();
				}

				if (m_PlayerData.m_Gamedata.m_PlayerInfo [i].SleepEnd_Time <= System.DateTime.Now) {
					m_PlayerData.m_Gamedata.m_PlayerInfo [i].bIsSleep = false;
					Debug.Log("ChargeTired");
					ChargeTired ();
				}

			}
		}


	}

	//unityads
	public void AdsBtnClick()
	{
		string strPlayerTrackID = "";

		switch (m_PlayerData.m_PlayerID) {

		case PLAYER_ID.NORMAL:
			strPlayerTrackID = "NORMAL";
			break;
		case PLAYER_ID.SPREAD:
			strPlayerTrackID = "SPREAD";
			break;
		case PLAYER_ID.LASER:
			strPlayerTrackID = "LASER";
			break;
		case PLAYER_ID.HOMING:
			strPlayerTrackID = "HOMING";
			break;
		case PLAYER_ID.BOOM:
			strPlayerTrackID = "BOOM";
			break;

		}

		TapjoyManager.Instance.TrackCustomEvent ("Charge_RewardAD", strPlayerTrackID, "Coin :" + m_PlayerData.m_Gamedata.m_iHaveCoin, "");

		if (!AdFunctions.Show_UnityAds ()) {
			if (!AdFunctions.Show_VungleAds ()) {
				m_NetworkFail_Label.GetComponent<TweenAlpha> ().ResetToBeginning ();
				m_NetworkFail_Label.GetComponent<TweenAlpha> ().enabled = true;
				m_NetworkFail_Label.SetActive (true);

				return;
			}
		}
		m_bVideoAdsOn = true;
		
	}

	public void SoundOnOffBtn_Click()
	{
		if (AudioListener.volume == 0) {
			m_objSoundBtn.transform.GetChild (0).gameObject.SetActive (true);
			m_objSoundBtn.transform.GetChild (1).gameObject.SetActive (false);
			PlayerPrefs.SetFloat("GameVolume", 3.0f);
			AudioListener.volume = 	3.0f;
		} else {
			m_objSoundBtn.transform.GetChild(0).gameObject.SetActive(false);
			m_objSoundBtn.transform.GetChild(1).gameObject.SetActive(true);
			PlayerPrefs.SetFloat("GameVolume", 0.0f);
			AudioListener.volume = 0.0f;
		}
	}

	public void Check_AdsReward()
	{
		if (m_bVideoAdsOn == true) {

			if(AdFunctions.m_bAdsComplete == true)
			{

			int iPlayerId = (int)m_PlayerData.m_PlayerID;

			m_PlayerData.m_Gamedata.m_PlayerInfo[iPlayerId].fTiredPercent = 100.0f;
			m_PlayerData.m_Gamedata.m_PlayerInfo[iPlayerId].bIsSleep = false;


				AdFunctions.m_bAdsComplete = false;
				m_bVideoAdsOn = false;

				m_PlayerData.GameData_Save();
				m_bInfoUpdate = true;
				MainBackBtn_Click();
			}
	
		}
	}

	public void GameStart()
	{
		m_objScrollView.SetActive (false);
		m_objEndBack.SetActive (true);
		PlayerPrefs.SetInt ("CurrentPlayNum", PlayerPrefs.GetInt("CurrentPlayNum") + 1);
		m_PlayerData.m_iCurrentPlayNum += 1;
		Application.LoadLevel ("02_Game");
		m_bTimeInit = false;
	}


	public void Play_BtnSound()
	{
		m_Audio.clip = m_BtnClip;
		m_Audio.Play();
	}

	public void Play_BuyBtnSound()
	{
		m_Audio.clip = m_BuyBtnClip;
		m_Audio.Play();
	}

	public void Play_ScrollSound()
	{
		m_Audio.clip = m_ScrollClip;
		m_Audio.Play();
	}

	public void Set_ProgressBar()
	{

	}

}
