using UnityEngine;
using System.Collections;

public class NormalEnemy : EnemyBase {
	public int m_iMidSkinIdx_Range = 2;
	public int m_iSmallSkinIdx_Range = 4;
	public int m_iSkinNum = 2;

	void Start () {
		base.Initialize ();

		if (m_iSkinNum != 0) {
			int iidx = 0;
			if (m_Size == ENEMY_SIZE.MID) {
				iidx = Random.Range (m_iMidSkinIdx_Range - m_iSkinNum, m_iMidSkinIdx_Range);
				if (iidx < 0)
					iidx = 0;
				m_Skeleton.skeleton.SetSkin (string.Format ("0{0}", iidx));
			} else {
				iidx = Random.Range (m_iSmallSkinIdx_Range - m_iSkinNum, m_iSmallSkinIdx_Range);
				if (iidx < 0)
					iidx = 0;
				m_Skeleton.skeleton.SetSkin (string.Format ("0{0}", iidx));
			}
		}



//		m_Skeleton.skeleton.SetSkin ("02");
//		list<Spine.Skin>() SkinList = m_Skeleton.skeleton.data.skins;
	}

	void FixedUpdate()
	{
		Progress();
		
		if(transform.position.y <= -5.8f)
			Destroy(gameObject);
		
		if(DeadCheck())	
		{
			Destroy(gameObject);
		}
	}

//	void OnTriggerEnter2D(Collider2D Coll)
//	{
//		if (Coll.gameObject.tag == "Bullet") {
//			m_iHp -= Coll.gameObject.GetComponent<Bullet>().m_iBulletDmg;
//			Destroy(Coll.gameObject);
//
//			//if Enemy Dead----->
//			if(DeadCheck())	
//			{
//				m_GameSys.m_PrefapMgr.CreatePoint(transform.position);
//				m_GameSys.m_iCurrent_GameScore += m_iHaveScore;
//				Destroy(gameObject);
//			}
//			//<-----End
//		}
//	}

}
