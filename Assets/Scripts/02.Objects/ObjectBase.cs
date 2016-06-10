using UnityEngine;
using System.Collections;

public abstract class ObjectBase : MonoBehaviour {

	protected Transform m_MyTrans = null;
	protected Rigidbody2D m_MyRigid = null;
	protected CircleCollider2D m_MyCircleColl = null;
	protected SkeletonAnimation m_Skeleton = null;
	protected AudioSource m_Audio = null;

	protected GameSystem m_GameSys = null;

	// Use this for initialization
	protected void Initialize () {
	
		m_MyTrans = transform;
		m_MyRigid = GetComponent<Rigidbody2D> ();
		m_MyCircleColl = GetComponent<CircleCollider2D> ();
		m_Skeleton = GetComponent<SkeletonAnimation> ();
		m_Audio = GetComponent<AudioSource> ();


		if (Application.loadedLevelName == "02_Game")
		m_GameSys = GameSystem.GetInstance ();
	}
}
