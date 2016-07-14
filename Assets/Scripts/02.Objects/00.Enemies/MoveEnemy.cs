using UnityEngine;
using System.Collections;

public class MoveEnemy : EnemyBase {

	public float m_fMoveSpeedMin = 0.05f;
	public float m_fMoveSpeedMax = 0.1f;
	private float m_fMoveSpeed = 0.0f;
	private int m_iDir = 1;

	void Start () {
		base.Initialize ();
	
		m_fMoveSpeed = Random.Range (m_fMoveSpeedMin, m_fMoveSpeedMax);
		m_iDir = Random.Range (0, 2);

		if (m_iDir == 0)
			m_iDir = -1;
	}
	
	void FixedUpdate()
	{
		Progress();

		if (m_MyTrans.position.x >= 2.6f
			|| m_MyTrans.position.x <= -2.6)
			m_iDir = -m_iDir;

		m_MyTrans.Translate(new Vector3(m_iDir, 0.0f) * m_fMoveSpeed);
		
		if(DeadCheck()
		   || (transform.position.y <= -5.8f))	
		{
			m_GameSys.m_PrefapMgr.DestroyEnemy(gameObject, m_EnemyID);
		}
	}

}
