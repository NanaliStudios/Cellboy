using UnityEngine;
using System.Collections;

public class GameSystem : MonoBehaviour {


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

//	public StageInfo m_ArrayStageInfo;
	//<-----End

	void Start()
	{
		//Player Set----->
		m_PlayerData = GameObject.Find ("PlayerData(Clone)").GetComponent<PlayerData> ();
		PLAYER_ID PlayerID = m_PlayerData.m_PlayerID;

		switch (PlayerID) {
		case PLAYER_ID.NORMAL:
			m_objPlayer = GameObject.Instantiate((Resources.Load("Prefaps/00.Objects/Players/NormalPlayer") as GameObject)) as GameObject; 
				break;

		case PLAYER_ID.SPREAD:
			m_objPlayer = GameObject.Instantiate((Resources.Load("Prefaps/00.Objects/Players/SpreadPlayer") as GameObject)) as GameObject; 
			break;

		case PLAYER_ID.LASER:
			m_objPlayer = GameObject.Instantiate((Resources.Load("Prefaps/00.Objects/Players/LaserPlayer") as GameObject)) as GameObject; 
			break;

		case PLAYER_ID.HOMING:
			m_objPlayer = GameObject.Instantiate((Resources.Load("Prefaps/00.Objects/Players/HomingPlayer") as GameObject)) as GameObject; 
			break;

		case PLAYER_ID.BOOM:
			m_objPlayer = GameObject.Instantiate((Resources.Load("Prefaps/00.Objects/Players/BoomPlayer") as GameObject)) as GameObject; 
			break;

		default:
			m_objPlayer = GameObject.Instantiate((Resources.Load("Prefaps/00.Objects/Players/NormalPlayer") as GameObject)) as GameObject; 
			break;

		}
		//<-----End

		//Class Initialize----->
		m_FileSys = new FileSystem ();
		m_GameData = new GameData ();
		m_PrefapMgr = new PrefapManager ();
		m_PrefapMgr.Initialize ();
		m_lvMgr = new LevelManager ();
		m_lvMgr.Initialize (this ,m_PrefapMgr, m_objPlayer.GetComponent<Rigidbody2D>());
		//<-----End

		//GameData Load----->

		//for debug
		//PlayerPrefs.DeleteAll ();

		m_PlayerData.m_bWinHighScore = false;

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

	//GameDATA
	public void SetScore()
	{
		m_PlayerData.m_iCurrentScore = m_iCurrent_GameScore;
	}

	public void WinHighScore()
	{
		m_PlayerData.m_bWinHighScore = true;
		//google play
		GooglePlayManager.Instance.SubmitScoreById ("CgkI-5Pv_oYcEAIQAQ", m_iCurrent_GameScore);
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
	//===Button===
	public void OnClickPause()
	{
		Time.timeScale = 0.0f;
		m_GameMenu.SetActive (false);
		m_PauseMenu.SetActive (true);

		m_PauseBtn.SetActive (false);
	}

	public void OnClickResume()
	{
		Time.timeScale = 1.0f;

		m_GameMenu.SetActive (true);
		m_PauseMenu.SetActive (false);
		m_ContinueMenu.SetActive (false);

		m_PauseBtn.SetActive (true);
	}

	public void OnClickHome()
	{
		Time.timeScale = 1.0f;
		m_iCurrent_GameScore  = 0;
		SetScore ();
		Application.LoadLevel ("00_MAIN");
	}

	public void OnClickNoContinue()
	{
		m_bOnContinue = false;
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
				
				//restart
				GameObject EnemyCase = GameObject.Find("00_Enemies");

				for(int i = 0; i < EnemyCase.transform.childCount; ++i)
				{
					if(EnemyCase.transform.GetChild(i).GetComponent<EnemyBase>().m_EnemyID == ENEMY_ID.IMM)
						Destroy(EnemyCase.transform.GetChild(i).gameObject);
					else
					EnemyCase.transform.GetChild(i).GetComponent<EnemyBase>().m_iHp = 0;
				}

				m_objPlayer.transform.position = new Vector3(0.0f, -1.5f);
				m_objPlayer.GetComponent<Player>().Set_AnimIdle();

				m_bAdsOn = false;
				m_bGameover = false;
				m_fGameoverTimer = 0.0f;
				m_bIsGameStart = false;
				Physics2D.gravity = new Vector3 (0.0f, 0.0f, 0.0f);
				AdFuctions.m_bAdsComplete = false;
				m_CanRestart = false;
				m_bOnContinue = false;
				gameObject.GetComponent<AudioSource>().enabled = true;
				OnClickResume();
			}
			
		}
	}

	//<-----End


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
