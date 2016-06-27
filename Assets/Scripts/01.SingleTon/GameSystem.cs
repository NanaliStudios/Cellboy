using UnityEngine;
using System.Collections;

public partial class GameSystem : MonoBehaviour {


	[HideInInspector] public FileSystem m_FileSys = null;
	[HideInInspector] public GameData m_GameData = null;
	[HideInInspector] public PrefapManager m_PrefapMgr  = null;
	[HideInInspector] public LevelManager m_lvMgr = null;

	//GameObject----->
	public GameObject m_objPlayer = null;
	public PlayerData m_PlayerData;

	//===UI===
	public GameObject m_PauseMenu = null;
	public GameObject m_PauseBtn = null;
	public GameObject m_GameMenu = null;
	public GameObject m_ContinueMenu = null;
	public GameObject m_GameOver = null;
	public GameObject m_Tutorial = null;
	public GameObject m_CoinnumLabel = null;

	public GameObject m_objBackColor = null;

	public GameObject Admob_Back = null;

	//==UI_Label==
	public GameObject m_NetworkFail_Label = null;

	//===Ads===
	public bool m_bAdsOn = false;
	public bool m_CanRestart = true;

	//<-----End

	//Current Game Data----->
	public int m_iCurrent_GameScore = 0;
	public int m_iCurrent_Point = 0;
	public int m_iCurrent_Coin = 0;

	private bool m_bGameover = false;
	private bool m_bOnContinue = false;
	private float m_fGameoverWaitTime = 2.0f;
	private float m_fGameoverTimer = 0.0f;

	private bool m_bIsGameStart = false;

	private float m_fGlobalSpeed = 0.0f;
	public float m_fCurrentGlobalSpeed = 0.0f;

//	public StageInfo m_ArrayStageInfo;
	//<-----End

	void Awake()
	{
		if ((Screen.width / 3) * 4 == Screen.height)
			Camera.main.orthographicSize = 4.5f;
	}

	void Start()
	{
		//Player Set----->
		m_PlayerData = GameObject.Find ("PlayerData(Clone)").GetComponent<PlayerData> ();
		PLAYER_ID PlayerID = m_PlayerData.m_PlayerID;

		string strPlayerName = "NormalPlayer";

		switch (PlayerID) {
		case PLAYER_ID.NORMAL:
			strPlayerName = "NormalPlayer";
				break;

		case PLAYER_ID.SPREAD:
			strPlayerName = "SpreadPlayer";
			break;

		case PLAYER_ID.LASER:
			strPlayerName = "LaserPlayer"; 
			break;

		case PLAYER_ID.HOMING:
			strPlayerName = "HomingPlayer"; 
			break;

		case PLAYER_ID.BOOM:
			strPlayerName = "BoomPlayer"; 
			break;

		default:
			strPlayerName = "NormalPlayer";
			break;

		}

		m_objPlayer = GameObject.Instantiate((Resources.Load(string.Format("Prefaps/00.Objects/Players/{0}", strPlayerName)) as GameObject), new Vector3(0.0f, -2.0f), Quaternion.identity) as GameObject; 
		//<-----End

		//Class Initialize----->
		m_PrefapMgr = new PrefapManager ();
		m_PrefapMgr.Initialize ();
		m_lvMgr = new LevelManager ();
		m_lvMgr.Initialize (this ,m_PrefapMgr, m_objPlayer.GetComponent<Rigidbody2D>());
		//<-----End

		//GameData Load----->

		m_PlayerData.m_bWinHighScore = false;


		//ads
		if (PlayerPrefs.GetInt("Adoff") == 0)
			Admob_Back.SetActive (true);
		else
			Admob_Back.SetActive (false);

		//<-----End

	}

	void LateUpdate()
	{
		m_lvMgr.Progress ();
		SetScore ();

		//Gameover
		if (m_bGameover == true) {
			Time.timeScale = 0;
			gameObject.GetComponent<AudioSource>().enabled = false;
			m_fGameoverTimer += Time.unscaledDeltaTime;
		}

		if (m_fGameoverTimer >= m_fGameoverWaitTime) {

			if(m_bOnContinue == true)
			{
				OnContinueMenu();
				Check_AdsReward();
			}
			else{
				//Game Over

				string strPlayerTrackID = "";

				switch (m_objPlayer.GetComponent<Player>().m_PlayerID) {

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
				TapjoyManager.Instance.TrackCustomEvent ("Gameover", strPlayerTrackID , "Gamescore: " + m_iCurrent_GameScore.ToString(), "Stage: " +m_lvMgr.m_iCurrentStage);

				Time.timeScale = 1;
				m_PlayerData.m_iPlayCountForAd += 1;	 
				Application.LoadLevel("00_Main");
			}

		}


		//Esc Input
		if (m_ContinueMenu.activeSelf == false &&
			m_GameOver.activeSelf == false) {

			if (Input.GetKeyDown (KeyCode.Escape)) {
				m_PauseMenu.SetActive(false);
				m_PauseBtn.SetActive(true);
				m_GameMenu.SetActive(true);
				Time.timeScale = 1.0f;
			}
		}

//		if(Input.GetKeyDown(KeyCode.R))
//			Application.LoadLevel("00_Main");

	}


	//

	public void Back_SetRandColor()
	{
		m_objBackColor.gameObject.GetComponent<BackColor> ().SetRandColor ();
	}

	public void Back_SetPrevColor()
	{
		m_objBackColor.gameObject.GetComponent<BackColor> ().SetPrevColor ();
	}

	public void Back_SetFeverColor()
	{
		m_objBackColor.gameObject.GetComponent<BackColor> ().SetFeverColor ();
	}

	//GameDATA
	public void SetScore()
	{
		m_PlayerData.m_iCurrentScore = m_iCurrent_GameScore;
	}

	public void WinHighScore()
	{
		m_PlayerData.m_bWinHighScore = true;
		//google play
		GameSDK_Funcs.SubmitScore_LeaderBoard (m_iCurrent_GameScore);
	}

	public void GameOver()
	{
		m_bGameover = true;
		m_GameOver.SetActive (true);
		m_PauseBtn.SetActive (false);
	}

	public void CanContinue()
	{
		m_bOnContinue = true;
	}

	public bool GetContinue()
	{
		return m_bOnContinue;
	}



	//Functions----->
	public Vector3 Get_PlayerPos()
	{
		return m_objPlayer.transform.position;
	}

	public void GameStart()
	{
		Physics2D.gravity = new Vector3 (9.8f, 0.0f, 0.0f);
		m_bIsGameStart = true;
		m_Tutorial.SetActive (false);
	}

	public bool CheckGameStart()
	{
		return m_bIsGameStart;
	}

	public void Start_FeverTime(float fTime)
	{
		Change_GlobalSpeed (m_fGlobalSpeed + 0.2f);
		m_lvMgr.Start_FeverTime (fTime);

		Delete_AllEnemy ();
	}

	public bool Get_Fever()
	{
		return m_lvMgr.m_bFever;
	}


	public void Delete_AllEnemy()
	{
		GameObject EnemyCase = m_PrefapMgr.Get_EnemyParent();

		for(int i = 0; i < EnemyCase.transform.childCount; ++i)
		{
			GameObject objEnemyChild = EnemyCase.transform.GetChild (i).gameObject;

			if (objEnemyChild.Equals (null))
				return;

			if (objEnemyChild.GetComponent<EnemyBase> ().m_EnemyID == ENEMY_ID.IMM) {
				Destroy (objEnemyChild);
			}
			else
				objEnemyChild.GetComponent<EnemyBase>().m_iHp = 0;

		}
	}

	public void CoinnumLabel_Tweenscale()
	{
		TweenScale tween =	m_CoinnumLabel.GetComponent<TweenScale> ();
		tween.to = new Vector3 (6.0f, 6.0f);
		tween.ResetToBeginning ();
		tween.enabled = true;
	}

	//global speed
	public float Get_GlobalSpeed()
	{
		return m_fGlobalSpeed;
	}
	public void Change_GlobalSpeed(float fSpeed)
	{
		m_fCurrentGlobalSpeed = m_fGlobalSpeed;
		m_fGlobalSpeed = fSpeed;
	}

	public void Return_GlobalSpeed()
	{
		m_fGlobalSpeed = m_fCurrentGlobalSpeed;
	}

	//UI
	public void OnContinueMenu()
	{
		Time.timeScale = 0.0f;
		m_GameMenu.SetActive (false);
		m_PauseMenu.SetActive (false);
		m_GameOver.SetActive (false);
		m_ContinueMenu.SetActive (true);
		m_PauseBtn.SetActive (false);
	}


	//==========SingleTon==========
	private static GameSystem instance;  
	public static GameSystem GetInstance()  
	{  
		if (!instance)  
		{  
			instance = GameObject.FindObjectOfType(typeof(GameSystem)) as GameSystem;  
			if (!instance)  
				Debug.LogError("There needs to be one active GameSystem script on a GameObject in your scene.");  
		}  
		
		return instance;  
	}  
}
