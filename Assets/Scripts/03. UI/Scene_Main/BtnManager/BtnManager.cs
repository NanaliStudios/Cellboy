using UnityEngine;
using System.Collections;
using ChartboostSDK;

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

	public GameObject m_NetworkFail_Label = null;

	public MAINMENU_STATE m_MenuStateID = MAINMENU_STATE.MAIN;

	private float m_fCurrentTired = 100.0f;
	private bool m_bCurrentLock = false;

	private AudioSource m_Audio = null;
	private AudioClip m_BtnClip = null;
	private AudioClip m_ScrollClip = null;

	public GameObject m_objSoundBtn = null;
	public GameObject Admob_Back = null;
	

	private bool m_bStart = true;


	void Awake()
	{
		m_fCurrentTired = 100.0f;

			if ((Screen.width / 3) * 4 == Screen.height)
				Camera.main.orthographicSize = 4.5f;
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

		//Logo Progress 
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


		//Main Menu Initialize here (Srry)

		//show chartboost ad
		if (m_PlayerData.m_iPlayCountForAd == 3) {
			Chartboost.showInterstitial (CBLocation.Default);
			m_PlayerData.m_iPlayCountForAd = 0;
		}

		if(AudioListener.volume != 0)
			SoundOnOffBtn_Click ();

		//ads
		if (!m_PlayerData.m_Gamedata.m_bAdOff)
			Admob_Back.SetActive (true);
		else
			Admob_Back.SetActive (false);

			//Game Save
			m_PlayerData.GameData_Save ();

	
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

		m_PlayerData.m_Gamedata.Spend_TiredVal (m_PlayerData.m_PlayerID);



		//Game Start 
		m_objScrollView.SetActive (false);
		m_objEndBack.SetActive (true);
		m_PlayerData.m_iCurrentPlayNum += 1;
		PlayerPrefs.SetInt ("CurrentPlayNum", PlayerPrefs.GetInt("CurrentPlayNum") + 1);
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
		GameSDK_Funcs.Show_LeaderBoard ();

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
		if (m_PlayerData.m_iChargePrice <= m_PlayerData.m_Gamedata.m_iHaveCoin) {

			TapjoyManager.Instance.TrackCustomEvent ("UseCoin", "Charge", "PlayerName: " + m_PlayerData.m_strPlayerName, "HaveCoin: " + m_PlayerData.m_Gamedata.m_iHaveCoin.ToString());

			m_PlayerData.m_Gamedata.m_iHaveCoin -= m_PlayerData.m_iChargePrice;

			m_PlayerData.m_Gamedata.m_PlayerInfo[(int)m_PlayerData.m_PlayerID].fTiredPercent = 100.0f;
			m_PlayerData.m_Gamedata.m_PlayerInfo[(int)m_PlayerData.m_PlayerID].bIsSleep = false;

			MainBackBtn_Click ();
		} else
			OnNoMoneyMenu ();
	}

	public void OnClickBuyBtn()
	{
		Play_BtnSound ();
		if (m_PlayerData.m_iBuyPrice <= m_PlayerData.m_Gamedata.m_iHaveCoin) {

			TapjoyManager.Instance.TrackCustomEvent ("UseCoin", "BuyPlayer", "HaveCoin: " + m_PlayerData.m_strPlayerName, "PlayNum: " + m_PlayerData.m_Gamedata.m_iPlayNum.ToString());
		
			m_PlayerData.m_Gamedata.m_iHaveCoin -= m_PlayerData.m_iBuyPrice;
			m_PlayerData.m_Gamedata.m_PlayerInfo[(int)m_PlayerData.m_PlayerID].bIsLock = false;
			
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
				}

				if (m_PlayerData.m_Gamedata.m_PlayerInfo [i].SleepEnd_Time <= System.DateTime.Now) {
					m_PlayerData.m_Gamedata.m_PlayerInfo [i].bIsSleep = false;
					ChargeTired ();
				}

			}
		}


	}

	//unityads
	public void AdsBtnClick()
	{
		TapjoyManager.Instance.TrackCustomEvent ("RewardAD", "Charge", m_PlayerData.m_strPlayerName, m_PlayerData.m_Gamedata.m_iHaveCoin.ToString());

		if (!AdFuctions.Show_UnityAds ()) {
			Color CurrentCol = m_NetworkFail_Label.GetComponent<UISprite>().color;
			m_NetworkFail_Label.GetComponent<UISprite>().color = new Color(CurrentCol.r, CurrentCol.g, CurrentCol.b, CurrentCol.a);
			m_NetworkFail_Label.SetActive (true);
		}
		m_bAdsOn = true;
		
	}

	public void SoundOnOffBtn_Click()
	{
		if (AudioListener.volume == 0) {
			m_objSoundBtn.transform.GetChild (0).gameObject.SetActive (true);
			m_objSoundBtn.transform.GetChild (1).gameObject.SetActive (false);
			AudioListener.volume = 3;
		} else {
			m_objSoundBtn.transform.GetChild(0).gameObject.SetActive(false);
			m_objSoundBtn.transform.GetChild(1).gameObject.SetActive(true);
			AudioListener.volume = 0;
		}
	}

	public void Check_AdsReward()
	{
		if (m_bAdsOn == true) {

			if(AdFuctions.m_bAdsComplete == true)
			{

			int iPlayerId = (int)m_PlayerData.m_PlayerID;

			m_PlayerData.m_Gamedata.m_PlayerInfo[iPlayerId].fTiredPercent = 100.0f;
			m_PlayerData.m_Gamedata.m_PlayerInfo[iPlayerId].bIsSleep = false;


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
