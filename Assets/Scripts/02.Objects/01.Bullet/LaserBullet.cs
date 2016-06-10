using UnityEngine;
using System.Collections;

public class LaserBullet : Bullet {

	public float m_fStayTerm = 0.2f;
	private float m_fStayTimer = 0.0f;

	// Use this for initialization
	void Start () {
		Initialize ();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
			
		m_fStayTimer += Time.fixedDeltaTime;

		if (m_fStayTimer >= m_fStayTerm) {
				Destroy(m_MyTrans.gameObject);
		}

		m_MyTrans.position = new Vector3 (m_GameSys.Get_PlayerPos ().x, m_MyTrans.position.y);
	}

	protected void OnTriggerStay2D(Collider2D Coll)
	{
		if (Coll.gameObject.tag == "Enemy") {
			Coll.gameObject.GetComponent<EnemyBase> ().ActiveDamage(m_iBulletDmg);
		}
	}
	override protected void OnTriggerEnter2D(Collider2D coll)
	{
	}
}
