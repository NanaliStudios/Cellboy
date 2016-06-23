using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : ObjectBase {
	
	public float m_fGrav = -9.8f;
	//Bullet Var----->
	private int m_iHaveBullet = 1;
	private int m_iCurrentBullet = 1;
	private int m_iCurrentLv = 1;
	private float m_fFireTerm = 0.1f;
	private float m_fFireTimer = 0.0f;

	private bool m_bSwitchDir = false;
	private float m_fSwitchTimer = 0.0f;
	private float m_fSwitchTerm = 0f;

	//Level Var----->

	public int m_iLv1MaxPoint = 5;
	public int m_iLv2MaxPoint = 10;
	public int m_iLv3MaxPoint = 15;

	public int m_iCurrentMaxPoint = 20;

	public UISprite m_ScoreBoard = null;

	private float m_fInputWaitTimer = 0.0f;
	private float m_fInputWaitTerm = 0.2f;

	//Homing 
	private float m_fHomingTerm = 0.01f;
	private float m_fHomingTimer = 0.0f;


	//audio clip list
	private AudioClip[] m_FireSound = new AudioClip[3];
	private AudioClip m_LvUpSound;
	private AudioClip m_DieSound;

	private GameObject m_objParticle = null;
	//<-----End
	

	public PLAYER_ID m_PlayerID = PLAYER_ID.NORMAL;

	// Use this for initialization
	void Start () {
	
		Initialize ();
		m_GameSys.m_PrefapMgr.SetBullet(m_PlayerID, BULLET_ID.LV1);
		Physics2D.gravity = new Vector3 (0.0f, 0.0f, 0.0f);

		m_iCurrentMaxPoint = m_iLv1MaxPoint;
		m_ScoreBoard = GameObject.Find ("ScoreFill").GetComponent<UISprite>();

		m_Skeleton.state.SetAnimation(0, "idle", true);

		m_objParticle = transform.GetChild (0).GetChild (0).GetChild (0).GetChild (0).gameObject;


		if (m_PlayerID == PLAYER_ID.LASER)
			m_FireSound [0] = Resources.Load ("Sounds/shoot_laser") as AudioClip;
		else {
			for (int i = 0; i < 3; ++i)
				m_FireSound [i] = Resources.Load ("Sounds/" + string.Format ("shoot_bullet_0{0}", i + 1)) as AudioClip;
		}

		m_LvUpSound = Resources.Load ("Sounds/ogg(96k)/cellboy_levelup") as AudioClip;
		m_DieSound = Resources.Load ("Sounds/ogg(96k)/score_reset") as AudioClip;


		StartCoroutine ("Excute");
	}
	void FixedUpdate()
	{
		if(m_iCurrentBullet == 0
		   && m_bSwitchDir == false)
			m_fFireTimer += Time.fixedDeltaTime;

		if (m_bSwitchDir == true)
			m_fSwitchTimer += Time.fixedDeltaTime;

		m_ScoreBoard.fillAmount = (float)m_GameSys.m_iCurrent_Point/(float)m_iCurrentMaxPoint;
	
	}

	// Update is called once per frame
	IEnumerator Excute () {

		do {
			if(m_GameSys.CheckGameStart() == false)
				m_fInputWaitTimer += Time.deltaTime;
			if(m_fInputWaitTerm <= m_fInputWaitTimer)
			{
			if ((Input.GetMouseButtonDown (0) || Input.GetKeyDown (KeyCode.Space))
			    &&  (m_GameSys.m_GameOver.activeSelf == false && m_GameSys.m_ContinueMenu.activeSelf == false
				    &&  m_GameSys.m_PauseMenu.activeSelf == false))
			{

				if(m_GameSys.CheckGameStart() == false
				   && m_GameSys.m_PauseMenu.activeSelf == false)
				{
					m_GameSys.GameStart();
					Switch_Dir ();
				}
				else
				{
					Switch_Dir ();
					
					//Fire Bullet----->
					if(m_iCurrentBullet > 0)
					{
						//PlaySound

						if(m_PlayerID == PLAYER_ID.LASER)
								m_Audio.clip = m_FireSound[0];
						else
						{
							int iRandVal = Random.Range(0, 3);
							m_Audio.clip = m_FireSound[iRandVal];
						}

						m_Audio.Play();

						if(m_PlayerID == PLAYER_ID.LASER)
						{
							if(m_GameSys.m_PrefapMgr.Get_BulletParent().transform.childCount == 0)
								m_GameSys.m_PrefapMgr.CreateBullet(transform.position);
						}
						else
							m_GameSys.m_PrefapMgr.CreateBullet(transform.position);
						m_iCurrentBullet -=1;
					}
					//<-----End
				}

			}
			}

			//Bullet Charge----->
			if(m_fFireTimer >= m_fFireTerm)
			{
				m_iCurrentBullet = m_iHaveBullet;
				m_fFireTimer = 0.0f;
			}
			//<-----End

			//Bulletupgrade----->
			if(m_GameSys.m_iCurrent_Point > m_iLv1MaxPoint
			   && m_iCurrentLv == 1)
			{
				Player_LevelUp(BULLET_ID.LV2, 2, "01", m_iLv2MaxPoint);
			}
			if(m_GameSys.m_iCurrent_Point > m_iLv2MaxPoint
			   && m_iCurrentLv == 2)
			{
				Player_LevelUp(BULLET_ID.LV3, 3, "02", m_iLv3MaxPoint);
			}
			if(m_GameSys.m_iCurrent_Point > m_iLv3MaxPoint
			   && m_iCurrentLv == 3)
			{
				Player_LevelUp(BULLET_ID.LV4, 4, "03", m_iLv3MaxPoint);
			}

			if(m_GameSys.m_iCurrent_Point > m_iLv3MaxPoint
			   && m_iCurrentLv == 4)
			{
				m_GameSys.Start_FeverTime (10.0f);
				m_GameSys.m_PrefapMgr.Create_LevelupEffect (m_MyTrans.position);

				m_iCurrentMaxPoint = m_iLv3MaxPoint;
				m_GameSys.m_iCurrent_Point = 0;

				m_Audio.clip = m_LvUpSound;
				m_Audio.Play();
			}
			//<-----End



			if(m_bSwitchDir == true && 
			   (m_fSwitchTimer >= m_fSwitchTerm))
			{
				m_bSwitchDir = false;
				m_fSwitchTimer = 0.0f;

			}

//			//Homing Bullet
//			if(m_PlayerID == PLAYER_ID.HOMING &&
//			   m_iCurrentLv == 4)
//			{
//				m_fHomingTimer += Time.deltaTime;
//
//				if(m_fHomingTerm <= m_fHomingTimer)
//				{   
//					m_GameSys.m_PrefapMgr.Create_HomingBullet(m_MyTrans.position);
//					m_fHomingTimer = 0.0f;
//				}
//			}

			if(Time.timeScale == 0)
				m_Skeleton.Update (Time.unscaledDeltaTime);



			yield return null;
		} while(true);
	}

	void Switch_Dir()
	{
		m_MyRigid.Sleep();
		
		m_fGrav = -m_fGrav;

		if(m_fGrav > 0.0f)
			m_Skeleton.state.SetAnimation(0, "right", true);
		else
			m_Skeleton.state.SetAnimation(0, "left", true);

		Physics2D.gravity = new Vector3 (m_fGrav, 0.0f, 0.0f);

		m_bSwitchDir = true;
		m_fSwitchTimer = 0.0f;
		
		m_MyRigid.WakeUp();
	}

	
	void OnTriggerEnter2D(Collider2D Coll)
	{
		//Game over 
		if (Coll.gameObject.tag == "Enemy"
		    || Coll.gameObject.tag == "Wall"
		    || Coll.gameObject.tag == "Stone") {

			m_Audio.clip = m_DieSound;

			if(AudioListener.volume != 0.0f)
			m_Audio.volume = 0.6f;

			m_Audio.Play();

			m_Skeleton.state.SetAnimation(0, "die", false);
			m_objParticle.SetActive(false);
			m_fInputWaitTimer = 0.0f;
			m_MyRigid.Sleep();

			if(m_GameSys.m_PlayerData.m_Gamedata.m_iHaveCoin < m_GameSys.m_iCurrent_GameScore)
			{
				m_GameSys.WinHighScore();
				m_GameSys.m_PlayerData.m_Gamedata.m_iHighScore = m_GameSys.m_iCurrent_GameScore;
			}

			if(m_GameSys.m_iCurrent_GameScore >= 15
			   && m_GameSys.m_CanRestart == true)
			{
				m_GameSys.CanContinue();
				m_GameSys.GameOver();
			}
			else
				m_GameSys.GameOver();
		}
	}

	public void Set_AnimIdle()
	{
		m_Skeleton.state.SetAnimation(0, "idle", true);
		m_objParticle.SetActive(true);
		if(AudioListener.volume != 0.0f)
		m_Audio.volume = 1.0f;
	}

	public void Player_LevelUp(BULLET_ID BulletID, int iLevel, string strSpineSkinIdx, int iLvMaxPoint)
	{
		//Fever Start
		m_GameSys.Start_FeverTime (5 + ((iLevel - 2) * 2));
		m_GameSys.m_PrefapMgr.Create_LevelupEffect (m_MyTrans.position);

			m_GameSys.m_PrefapMgr.SetBullet(m_PlayerID, BulletID);
			m_iCurrentMaxPoint = iLvMaxPoint;
			m_GameSys.m_iCurrent_Point = 0;
			m_iCurrentLv = iLevel;
			
			m_Skeleton.skeleton.SetSkin (strSpineSkinIdx);
			m_Audio.clip = m_LvUpSound;
			m_Audio.Play();
	}
}
