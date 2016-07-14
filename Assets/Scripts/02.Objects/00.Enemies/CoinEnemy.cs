using UnityEngine;
using System.Collections;

public class CoinEnemy : EnemyBase {

	void Start () {
		base.Initialize ();
	}
	
	void FixedUpdate()
	{
		Progress();

		if(DeadCheck()
		   || (transform.position.y <= -5.8f))	
		{
			m_GameSys.m_PrefapMgr.DestroyEnemy(gameObject, m_EnemyID);
		}
	}
}
