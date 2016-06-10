using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomingBullet : Bullet {

	private GameObject m_objEnemyParent = null;
	private ChaseRange m_ChaseRange = null;

	private bool m_bBackMove = true;
	private Vector3 m_Vec3Move = Vector3.zero;

	// Use this for initialization
	void Start () {
	
		base.Start ();

		m_ChaseRange = gameObject.transform.GetChild (0).gameObject.GetComponent<ChaseRange>();

	}

	override protected void FixedUpdate()
	{

		if(transform.position.y > 5.8f)
			Destroy(gameObject);

		if (m_bDead == true) {
			Destroy (gameObject);
		}
		

		if (m_ChaseRange.m_objTarget== null) {
			if (m_fAngle != 0) {
				Quaternion v3Rotation = Quaternion.Euler (0f, 0f, m_fAngle);  // Rotate Val
				Vector3 v3Direction = Vector2.up; // Rotate Vector
				Vector3 v3RotatedDirection = v3Rotation * v3Direction; 
				v3RotatedDirection = Vector3.Normalize (v3RotatedDirection);
				m_MyTrans.rotation = v3Rotation;
				Move (v3RotatedDirection * m_fBulletSpeed);
			} else
				Move (new Vector3 (0.0f, 0.1f) * m_fBulletSpeed);

		} else {
			m_Vec3Move = Vector3.MoveTowards(m_MyTrans.transform.position,m_ChaseRange.m_objTarget.transform.position, m_fBulletSpeed * 0.05f);
			m_MyTrans.transform.position = m_Vec3Move;
		}


//		if(transform.position.y <= -2.5f)
//			m_bBackMove = false;
//		
//		if(m_bBackMove == true)
//			m_MyTrans.Translate(new Vector3(0.0f, -m_fBulletSpeed));
//		else
//		{
//			if(m_objTarget != null)
//			{
//				m_Vec3Move = Vector3.MoveTowards(m_MyTrans.transform.position, m_objTarget.transform.position, m_fBulletSpeed);
//				m_MyTrans.transform.position = m_Vec3Move;
//			}
//			else
//			{
//				Move(new Vector3(0.0f, 0.1f) * m_fBulletSpeed);
//				
//				//					if(m_Vec3Move == Vector3.zero)
//				//						m_Vec3Move = new Vector3(m_MyTrans.position.x, 10.0f);
//				//					m_MyTrans.transform.position = m_Vec3Move;
//				
//			}
//		}
//		
//		if(transform.position.y > 5.8f)
//			Destroy(gameObject);
//
//		if (m_bDead == true)
//			Destroy (gameObject);

	}

}
