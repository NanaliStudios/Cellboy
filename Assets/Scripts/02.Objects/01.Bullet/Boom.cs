using UnityEngine;
using System.Collections;

public class Boom : ObjectBase {

	public int m_iBoomDamage = 1;
	public float m_fStayTerm = 0.2f;
	private float m_fStayTimer = 0.0f;

	private bool m_bDead = false;
	private bool m_bStay = true;

	// Use this for initialization
	void Start () {
	
		Initialize ();

		if (m_Skeleton != null) {
			m_Skeleton.state.Complete += delegate {
				
				//if(m_Skeleton.AnimationName.Equals("hit"))
				//Debug.Log("Hit");
				
				if(m_Skeleton.AnimationName.Equals("die"))
				{
					m_bDead = true;
				}
				
				
			};
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if(m_bStay == true)
		m_fStayTimer += Time.fixedDeltaTime;
		
		if (m_fStayTimer >= m_fStayTerm) {
			m_MyCircleColl.enabled = false;
			m_Skeleton.state.SetAnimation (0, "die", false);
			m_bStay = false;
			m_fStayTimer = 0.0f;
		}

		if (m_bDead == true)
			Destroy (gameObject);

	
	}

	virtual protected void OnTriggerEnter2D(Collider2D Coll)
	{
		if (Coll.gameObject.tag == "Enemy") {
			Coll.gameObject.GetComponent<EnemyBase> ().ActiveDamage(m_iBoomDamage);
		}
	}
}
