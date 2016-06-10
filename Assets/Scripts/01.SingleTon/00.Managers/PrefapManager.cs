using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefapManager {

	//Bullets----->
	private GameObject m_objCurrentBullet = null;
	private ArrayList m_ListBullets = new ArrayList();
	private GameObject m_objHomingBullet = null;
	private GameObject m_objBoom = null;
	//Enemies----->
	private ArrayList m_ListEnemies = new ArrayList ();
//	private GameObject m_objNormalEnemyS = null;
//	private GameObject m_objNormalEnemyM = null;
	//Items----->
	private GameObject m_objPoint = null;
	private GameObject m_objCoin = null;
	//Obj's Parents----->
	private GameObject BulletParentObj = null;
	private GameObject EnemyParentObj = null;
	private GameObject ItemParentObj = null;
	//<-----End

	public void Initialize () {
		//Data Initialize----->

		//-----Set Player1's Bullets-----
		ArrayList ListNormalBullets = new ArrayList ();
		//Normal
		for(int i = 0; i < (int)BULLET_ID.END; ++i)
			ListNormalBullets.Add (Resources.Load (string.Format("Prefaps/00.Objects/Bullets/Normal/NormalBullet_Lv{0}", i+1)) as GameObject);

		m_ListBullets.Add (ListNormalBullets);

		ListNormalBullets = new ArrayList();
		//Spread
		for(int i = 0; i < (int)BULLET_ID.END; ++i)
			ListNormalBullets.Add (Resources.Load (string.Format("Prefaps/00.Objects/Bullets/Spread/SpreadBullet_Lv{0}", i+1)) as GameObject);
		
		m_ListBullets.Add (ListNormalBullets);

		ListNormalBullets = new ArrayList();
		//Laser
		for(int i = 0; i < (int)BULLET_ID.END; ++i)
			ListNormalBullets.Add (Resources.Load (string.Format("Prefaps/00.Objects/Bullets/Laser/LaserBullet_Lv{0}", i+1)) as GameObject);
		
		m_ListBullets.Add (ListNormalBullets);

		ListNormalBullets = new ArrayList();
		//Homing
		for(int i = 0; i < (int)BULLET_ID.END; ++i)
			ListNormalBullets.Add (Resources.Load (string.Format("Prefaps/00.Objects/Bullets/Homing/HomingBullet_Lv{0}", i+1)) as GameObject);
		
		m_ListBullets.Add (ListNormalBullets);

		ListNormalBullets = new ArrayList();
		//Homing
		for(int i = 0; i < (int)BULLET_ID.END; ++i)
			ListNormalBullets.Add (Resources.Load (string.Format("Prefaps/00.Objects/Bullets/Boom/BoomBullet_Lv{0}", i+1)) as GameObject);
		
		m_ListBullets.Add (ListNormalBullets);


		m_objHomingBullet = Resources.Load ("Prefaps/00.Objects/Bullets/Homing/HomingBullet") as GameObject;
		m_objBoom = Resources.Load ("Prefaps/00.Objects/Bullets/Boom/Boom") as GameObject;
		//<-----End

		//Load Enemy----->
		m_ListEnemies.Add (Resources.Load ("Prefaps/00.Objects/Enemies/Normal/NormalEnemyS") as GameObject);
		m_ListEnemies.Add (Resources.Load ("Prefaps/00.Objects/Enemies/Normal/NormalEnemyM") as GameObject);
		m_ListEnemies.Add (Resources.Load ("Prefaps/00.Objects/Enemies/Speed/SpeedEnemyS") as GameObject);
		m_ListEnemies.Add (Resources.Load ("Prefaps/00.Objects/Enemies/Split/SplitEnemyS") as GameObject);
		m_ListEnemies.Add (Resources.Load ("Prefaps/00.Objects/Enemies/Split/SplitEnemyM") as GameObject);
		m_ListEnemies.Add (Resources.Load ("Prefaps/00.Objects/Enemies/Split/ChildEnemy") as GameObject);
		m_ListEnemies.Add (Resources.Load ("Prefaps/00.Objects/Enemies/Follow/FollowEnemyS") as GameObject);
		m_ListEnemies.Add (Resources.Load ("Prefaps/00.Objects/Enemies/Move/MoveEnemyS") as GameObject);
		m_ListEnemies.Add (Resources.Load ("Prefaps/00.Objects/Enemies/Imm/ImmEnemyS") as GameObject);
		m_ListEnemies.Add (Resources.Load ("Prefaps/00.Objects/Enemies/Coin/CoinEnemyS") as GameObject);
		m_ListEnemies.Add (Resources.Load ("Prefaps/00.Objects/Enemies/Coin/CoinEnemyM") as GameObject);
		//<-----End

		m_objPoint = Resources.Load ("Prefaps/00.Objects/Point") as GameObject;
		m_objCoin = Resources.Load ("Prefaps/00.Objects/Coin") as GameObject;

		EnemyParentObj = GameObject.Find ("00_Enemies");
		BulletParentObj = GameObject.Find ("01_Bullets");
		ItemParentObj = GameObject.Find ("02_Items");
		//Debug.Log ("Prefap Manager Initialize Complete");
	}

	public GameObject SetBullet( PLAYER_ID PlayerID, BULLET_ID BulletID)
	{
		ArrayList ListBullets = m_ListBullets [(int)PlayerID] as ArrayList;
		m_objCurrentBullet = ListBullets[(int)BulletID] as GameObject;

		return m_objCurrentBullet;
	}
	public GameObject CreateBullet(Vector3 Vec3Pos, PLAYER_ID PlayerID = PLAYER_ID.NORMAL ,BULLET_ID BulletID = BULLET_ID.LV1)
	{
		GameObject objBullet = GameObject.Instantiate (m_objCurrentBullet, new Vector3 (Vec3Pos.x, Vec3Pos.y + 0.5f), Quaternion.identity) as GameObject;
		objBullet.transform.parent = BulletParentObj.transform;

		return objBullet;
	}

	public GameObject Create_HomingBullet(Vector3 Vec3Pos)
	{
		GameObject objBullet = GameObject.Instantiate (m_objHomingBullet, Vec3Pos, Quaternion.identity) as GameObject;
		objBullet.transform.parent = BulletParentObj.transform;
		
		return objBullet;
	}
	
	public GameObject CreateBoom(Vector3 Vec3Pos, float m_fBoomRadius, int m_iBoomDmg)
	{
		GameObject objBoom = GameObject.Instantiate (m_objBoom, Vec3Pos, Quaternion.identity) as GameObject;
		Boom BoomScript = objBoom.GetComponent<Boom> ();
		BoomScript.m_iBoomDamage = m_iBoomDmg;
		objBoom.transform.localScale = new Vector3(objBoom.transform.localScale.x * m_fBoomRadius, objBoom.transform.localScale.y * m_fBoomRadius);
		objBoom.transform.parent = EnemyParentObj.transform;
		
		return objBoom;
	}

	public GameObject CreateNormalEnemy(Vector3 Vec3Pos, ENEMY_ID EnemyID = ENEMY_ID.NORMAL_S)
	{
		GameObject objEnemy = GameObject.Instantiate (m_ListEnemies[(int)EnemyID] as GameObject, Vec3Pos, Quaternion.identity) as GameObject;
		objEnemy.transform.parent = EnemyParentObj.transform;
		
		return objEnemy;
	}


	public void CreatePoint(Vector3 Vec3Pos, int iNum = 1)
	{
		GameObject objPoint;

		for (int i = 0; i < iNum; ++i) {
			objPoint = GameObject.Instantiate (m_objPoint, Vec3Pos, Quaternion.identity) as GameObject;
			objPoint.transform.parent = ItemParentObj.transform;
		}
	}

	public void CreateCoin(Vector3 Vec3Pos, int iNum = 1)
	{
		GameObject objCoin;
		
		for (int i = 0; i < iNum; ++i) {
			objCoin = GameObject.Instantiate (m_objCoin, Vec3Pos, Quaternion.identity) as GameObject;
			objCoin.transform.parent = ItemParentObj.transform;
		}
	}


	public GameObject Get_EnemyParent()
	{
		return EnemyParentObj;
	}

	public GameObject Get_BulletParent()
	{
		return BulletParentObj;
	}

	public GameObject Get_ItemParent()
	{
		return ItemParentObj;
	}
}
