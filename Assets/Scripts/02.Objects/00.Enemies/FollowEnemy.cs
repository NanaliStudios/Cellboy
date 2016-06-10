using UnityEngine;
using System.Collections;

public class FollowEnemy : EnemyBase {

	// Use this for initialization
	void Start () {
	
		base.Initialize ();
	}

	void FixedUpdate()
	{
		Progress();
		//Rotate

		float fAngle = 0.0f;
		if (m_GameSys != null) {
			Vector3 Vec3PlayerPos = m_GameSys.Get_PlayerPos ();
			Vector3 dir = Vec3PlayerPos - transform.position;
			fAngle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
		}


		m_MyTrans.rotation = Quaternion.Euler (new Vector3(0.0f, 0.0f, fAngle + 90.0f));
		//
		m_MyTrans.position = Vector3.MoveTowards(m_MyTrans.transform.position, m_GameSys.Get_PlayerPos(), m_fSpeed);
		
		if(transform.position.y <= -5.8f)
			Destroy(gameObject);
		
		if(DeadCheck())	
		{
			Destroy(gameObject);
		}
	}
}
