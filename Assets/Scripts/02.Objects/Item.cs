using UnityEngine;
using System.Collections;

public class Item: ObjectBase {
	protected GameData m_GameData = null;

	public float m_fRandSpeedMin = 0.01f;
	public float m_fRandSpeedMax = 0.05f;
	protected float m_fSpeed = 0.0f;
	protected bool m_bIsGet = false;


	void OnEnable()
	{
		if (m_Skeleton != null) {
			m_Skeleton.state.SetAnimation(0, "start", false);
			m_MyCircleColl.enabled = true;
			m_bIsGet = false;
			m_fSpeed = Random.Range (m_fRandSpeedMin, m_fRandSpeedMax);
		}
	}

	protected void Initialize()
	{
		base.Initialize ();
		m_GameData = m_GameSys.m_GameData;

		m_fSpeed = Random.Range (m_fRandSpeedMin, m_fRandSpeedMax);

		if (m_Skeleton != null) {
			m_Skeleton.state.Complete += delegate {

				if(m_Skeleton.AnimationName.Equals("start"))
					m_Skeleton.state.SetAnimation(0, "idle", true);

				if(m_Skeleton.AnimationName.Equals("die"))
				{
					m_bIsGet = true;
				}
				
				
			};
		}

	}

	protected void Follow_Player()
	{
		m_MyTrans.transform.position = Vector3.MoveTowards (m_MyTrans.transform.position , m_GameSys.Get_PlayerPos (), m_fSpeed);
		m_fSpeed += 0.01f;
	}
}
