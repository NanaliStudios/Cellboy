using UnityEngine;
using System.Collections;

public class Bullet : ObjectBase {

	public float m_fBulletSpeed = 0.05f;
	public int m_iBulletDmg = 5;
	public float m_fAngle = 0.0f;

	public bool m_bDead = false;

	// Use this for initialization
	protected void Start () {
		base.Initialize ();

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

	virtual protected void FixedUpdate()
	{
		if(transform.position.y > 5.8f)
			Destroy(gameObject);

		
		if (m_fAngle != 0) {
			Quaternion v3Rotation = Quaternion.Euler (0f, 0f, m_fAngle);  // 회전각
			Vector3 v3Direction = Vector2.up; // 회전시킬 벡터(테스트용으로 world forward 썼음)
			Vector3 v3RotatedDirection = v3Rotation * v3Direction; 
			v3RotatedDirection = Vector3.Normalize(v3RotatedDirection);
			m_MyTrans.rotation = v3Rotation;
			Move(v3RotatedDirection * m_fBulletSpeed);
		} else
			Move (new Vector3(0.0f, 0.1f) * m_fBulletSpeed);

		if (m_bDead == true) {
			Destroy (gameObject);
		}

	}

	protected void Move(Vector3 Vec3Dir)
	{
		if (m_Skeleton.AnimationName != "die")
			m_MyTrans.Translate (Vec3Dir);
	}



		virtual protected void OnTriggerEnter2D(Collider2D Coll)
		{
			if (Coll.gameObject.tag == "Enemy"
		    || Coll.gameObject.tag == "Stone") {
			Coll.gameObject.GetComponent<EnemyBase> ().ActiveDamage(m_iBulletDmg);
			m_Skeleton.state.SetAnimation(0, "die", false);
			gameObject.GetComponent<BoxCollider2D>().enabled = false;

		}
		}
	
}
