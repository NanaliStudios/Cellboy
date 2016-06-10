using UnityEngine;
using System.Collections;

public class ChaseRange : MonoBehaviour {

	public GameObject m_objTarget= null;


	void FixedUpdate()
	{
		if (m_objTarget != null) {
			if (m_objTarget.GetComponent<EnemyBase> ().m_iHp <= 0)
				m_objTarget = null;
		}
	}

	public void OnTriggerEnter2D(Collider2D Coll)
	{
		if (Coll.gameObject.tag == "Enemy") {
			m_objTarget = Coll.gameObject;
		}
	}
}
