using UnityEngine;
using System.Collections;

public class BoomBullet : Bullet {

	public float m_fBoomRadius = 1.0f;
	public int m_iBoomDmg = 5;

	// Use this for initialization
	void Start () {

		base.Start ();
	
	}
	
	override protected void OnTriggerEnter2D(Collider2D Coll)
	{
		if(Coll.gameObject.tag == "Enemy"
		   || Coll.gameObject.tag == "Stone")
		{
			m_GameSys.m_PrefapMgr.CreateBoom(m_MyTrans.transform.position, m_fBoomRadius, m_iBoomDmg);
			Destroy (gameObject);
		}
	}
}
