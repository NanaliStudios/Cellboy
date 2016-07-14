using UnityEngine;
using System.Collections;

public class ChildEnemy : EnemyBase {

	private float m_fStayTimer = 0.0f;
	public float m_fStayTerm = 0.5f;

	private bool m_bStay = true;


	void OnEnable()
	{
		base.OnEnable ();
		m_fStayTimer = 0.0f;
		m_bStay = true;
	}

	void Start () {
		base.Initialize ();
	}
	
	void FixedUpdate()
	{
		Progress();

		if (m_bStay == true) {
			if (m_fStayTimer >= m_fStayTerm) {
				m_bStay = false;

			} else
				m_fStayTimer += Time.fixedDeltaTime;
		} else
			Move ();
		
		if(DeadCheck()
		   || (transform.position.y <= -5.8f))	
		{
			m_GameSys.m_PrefapMgr.DestroyEnemy(gameObject, m_EnemyID);
		}
	}
}
