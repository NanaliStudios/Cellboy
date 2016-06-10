using UnityEngine;
using System.Collections;

public class SplitBullet : Bullet {

	// Use this for initialization
	void Start () {

		base.Start ();
	}

	void OnTriggerEnter2D(Collider2D Coll)
	{
		if (Coll.gameObject.tag == "Enemy") {
			Coll.gameObject.GetComponent<EnemyBase> ().ActiveDamage(m_iBulletDmg);


			m_GameSys.m_PrefapMgr.CreateBullet(m_MyTrans.position, PLAYER_ID.SPREAD, BULLET_ID.LV1);

			Destroy (gameObject);
		}
	}
}
