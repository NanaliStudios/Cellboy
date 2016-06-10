﻿using UnityEngine;
using System.Collections;

public class EnemyBase : ObjectBase {

	public int	m_iHp = 1;
	public int m_iHaveScore = 1;
	public int	m_iHavePoint = 0;
	public int 	m_iHaveCoin = 0;

	public float m_fRandMinSpeed = 0.5f;
	public float m_fRandMaxSpeed = 0.6f;
	protected float m_fSpeed = 0.0f;

	protected bool m_bDamageAct = false;
	protected float m_fImmTimer = 0.0f;
	protected float m_fImmTerm = 0.1f;

	private bool m_bDead = false;
	private bool m_bGiveScore = true;
	private float m_fGiveTimer = 0.0f;
	private float m_fGiveTime = 0.3f;

	public ENEMY_SIZE m_Size = ENEMY_SIZE.SMALL;
	public ENEMY_ID m_EnemyID = ENEMY_ID.NORMAL_S;

	protected AudioClip m_HitSound = null;
	protected AudioClip m_DieSound = null;

	protected void Initialize()
	{
		base.Initialize ();
	
		m_fSpeed = Random.Range (m_fRandMinSpeed, m_fRandMaxSpeed);

		if (m_Skeleton != null) {
			m_Skeleton.state.Complete += delegate {

//				if(m_Skeleton != null)
//				{
					if(m_Skeleton.AnimationName.Equals("die"))
					{
						m_bDead = true;
					}
				//}
			};
		}

		//Load Sound
		m_HitSound = Resources.Load ("Sounds/ogg(96k)/enemy_hit") as AudioClip;
		m_DieSound = Resources.Load ("Sounds/ogg(96k)/enemy_pop") as AudioClip;
	}

	protected void Progress()
	{
			if (m_bDamageAct == true) {
				m_fImmTimer += Time.fixedDeltaTime;
			}
			
			if (m_fImmTerm <= m_fImmTimer) {
				m_bDamageAct = false;
				m_fImmTimer = 0.0f;
			}

		if(m_EnemyID != ENEMY_ID.CHILD)
		Move ();


	}

	public void ActiveDamage(int iDamage)
	{
		if (m_iHp <= 0)
			return;

		if (m_bDamageAct == false) {

			if(m_Skeleton != null)
			{
				if(m_Skeleton.Skeleton.data.FindAnimation("hit") != null)
				{
					PlaySound("enemy_hit");
					m_Skeleton.state.SetAnimation(0, "hit", false);
				}
			}

			m_iHp -= iDamage;

			if(m_GameSys == null)
			{
				m_bDamageAct = true;
				m_fImmTimer = 0.0f;
			}
			else
			{
				if(m_GameSys.m_PlayerData.m_PlayerID == PLAYER_ID.LASER
				   || m_GameSys.m_PlayerData.m_PlayerID == PLAYER_ID.BOOM)
				{
					m_bDamageAct = true;
					m_fImmTimer = 0.0f;
				}
			}
		}
	}
	protected bool DeadCheck()
	{

		if (m_bDead == true)
			return m_bDead;

		if (m_iHp <= 0) {
			m_fGiveTimer += Time.deltaTime;

			if(m_bGiveScore == true &&
			   m_fGiveTimer >= m_fGiveTime)
			{

				//Create Item & enemy's body has boom 

				PlaySound("enemy_pop");

				m_GameSys.m_PrefapMgr.CreatePoint(transform.position, m_iHavePoint);
				m_GameSys.m_PrefapMgr.CreateCoin(transform.position, m_iHaveCoin);
				m_GameSys.m_iCurrent_GameScore += m_iHaveScore;

				if(m_GameSys.CheckGameStart())
				{
				if(m_EnemyID == ENEMY_ID.SPLIT_S )
				{
					float m_fRandPos = 0.5f;
					for(int i= 0; i <= 2; ++i)
						m_GameSys.m_PrefapMgr.CreateNormalEnemy(new Vector3(m_MyTrans.position.x + Random.Range(-m_fRandPos, m_fRandPos), m_MyTrans.position.y + Random.Range(-m_fRandPos, m_fRandPos)), ENEMY_ID.CHILD);
				}
				else if(m_EnemyID == ENEMY_ID.SPLIT_M )
				{
					float m_fRandPos = 0.5f;
					for(int i= 0; i <= 3; ++i)
						m_GameSys.m_PrefapMgr.CreateNormalEnemy(new Vector3(m_MyTrans.position.x + Random.Range(-m_fRandPos, m_fRandPos), m_MyTrans.position.y + Random.Range(-m_fRandPos, m_fRandPos)), ENEMY_ID.CHILD);
				}
				}

			

				m_bGiveScore = false;
			}

			if(m_Skeleton == null ||
			   m_Skeleton.AnimationName == null)
				return true;
		
			if(m_Skeleton.AnimationName.Equals("die"))
			{
				if(m_MyCircleColl.isActiveAndEnabled == true)
				{
					m_MyCircleColl.enabled = false;
				}

			}
			else
			{
				if(m_Skeleton.Skeleton.data.FindAnimation("die") != null)
				m_Skeleton.state.SetAnimation(0, "die", false);
			}

		
				}

		return m_bDead;

	}


	protected void Move()
	{
		if (m_EnemyID == ENEMY_ID.FOLLOW_S)
			return;

		if (m_iHp > 0) 
		m_MyTrans.Translate(new Vector3(0.0f, -0.1f) * m_fSpeed);
	}


	protected void PlaySound(string SoundName)
	{
		if(m_Audio != null)
		{
			if(m_Audio.clip.name != SoundName)
			{
				switch(SoundName)
				{
				case "enemy_hit":
					m_Audio.clip = m_HitSound;
					break;
				case "enemy_pop":
					m_Audio.clip = m_DieSound;
					break;
				}
			}

			if(SoundName == "enemy_hit")
				m_Audio.Play();
			else
			{
		  		 if(!m_Audio.isPlaying)
					m_Audio.Play();
			}
		}
	}

}


