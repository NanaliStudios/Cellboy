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
//	private ArrayList m_ListEnemies = new ArrayList ();
//	private GameObject m_objNormalEnemyS = null;
//	private GameObject m_objNormalEnemyM = null;
	//Obj's Parents----->
	private GameObject BulletParentObj = null;
	private GameObject EnemyParentObj = null;
	private GameObject SleepEnemyParentObj = null;
	private GameObject ItemParentObj = null;
	//
	private GameObject m_obLevelupEffect = null;

	//memory pool
	CGameObjectPool<GameObject>	m_NormalSEnemyObjPool;
	CGameObjectPool<GameObject>	m_NormalMEnemyObjPool;
	CGameObjectPool<GameObject>	m_SpeedEnemyObjPool;
	CGameObjectPool<GameObject>	m_SplitSEnemyObjPool;
	CGameObjectPool<GameObject>	m_SplitMEnemyObjPool;
	CGameObjectPool<GameObject>	m_ChildEnemyObjPool;
	CGameObjectPool<GameObject>	m_MoveEnemyObjPool;
	CGameObjectPool<GameObject>	m_FollowEnemyObjPool;
	CGameObjectPool<GameObject>	m_ImmEnemyObjPool;
	CGameObjectPool<GameObject>	m_CoinSEnemyObjPool;
	CGameObjectPool<GameObject>	m_CoinMEnemyObjPool;

	CGameObjectPool<GameObject> m_PointObjPool;
	GameObject m_objCoin = null;
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

//		m_ListEnemies.Add (Resources.Load ("Prefaps/00.Objects/Enemies/Normal/NormalEnemyS") as GameObject);
//		m_ListEnemies.Add (Resources.Load ("Prefaps/00.Objects/Enemies/Normal/NormalEnemyM") as GameObject);
//		m_ListEnemies.Add (Resources.Load ("Prefaps/00.Objects/Enemies/Speed/SpeedEnemyS") as GameObject);
//		m_ListEnemies.Add (Resources.Load ("Prefaps/00.Objects/Enemies/Split/SplitEnemyS") as GameObject);
//		m_ListEnemies.Add (Resources.Load ("Prefaps/00.Objects/Enemies/Split/SplitEnemyM") as GameObject);
//		m_ListEnemies.Add (Resources.Load ("Prefaps/00.Objects/Enemies/Split/ChildEnemy") as GameObject);
//		m_ListEnemies.Add (Resources.Load ("Prefaps/00.Objects/Enemies/Follow/FollowEnemyS") as GameObject);
//		m_ListEnemies.Add (Resources.Load ("Prefaps/00.Objects/Enemies/Move/MoveEnemyS") as GameObject);
//		m_ListEnemies.Add (Resources.Load ("Prefaps/00.Objects/Enemies/Imm/ImmEnemyS") as GameObject);
//		m_ListEnemies.Add (Resources.Load ("Prefaps/00.Objects/Enemies/Coin/CoinEnemyS") as GameObject);
//		m_ListEnemies.Add (Resources.Load ("Prefaps/00.Objects/Enemies/Coin/CoinEnemyM") as GameObject);
		//<-----End

		EnemyParentObj = GameObject.Find ("00_Enemies");
		SleepEnemyParentObj = GameObject.Find ("00_SleepEnemies");
		BulletParentObj = GameObject.Find ("01_Bullets");
		ItemParentObj = GameObject.Find ("02_Items");

		//MemoryPool Init----->

		//GameObject ob = Resources.Load ("Prefaps/00.Objects/Enemies/Normal/NormalEnemyS") as GameObject;
		GameObject LoadedObj = Resources.Load ("Prefaps/00.Objects/Enemies/Normal/NormalEnemyS") as GameObject; 
		m_NormalSEnemyObjPool = new CGameObjectPool<GameObject>(30, () =>  {
		                                                 
			GameObject MyObj = GameObject.Instantiate(LoadedObj) as GameObject;
			MyObj.transform.parent = SleepEnemyParentObj.transform;
			MyObj.SetActive(false);
			return MyObj; 
			
		}); 

		LoadedObj = null;
		LoadedObj = Resources.Load ("Prefaps/00.Objects/Enemies/Normal/NormalEnemyM") as GameObject; 
		m_NormalMEnemyObjPool = new CGameObjectPool<GameObject>(30, () =>  {
			
			GameObject MyObj = GameObject.Instantiate(LoadedObj) as GameObject;
			MyObj.transform.parent = SleepEnemyParentObj.transform;
			MyObj.SetActive(false);
			return MyObj; 
			
		}); 

		LoadedObj = null;
		LoadedObj = Resources.Load ("Prefaps/00.Objects/Enemies/Speed/SpeedEnemyS") as GameObject; 
		m_SpeedEnemyObjPool = new CGameObjectPool<GameObject>(30, () =>  {
			
			GameObject MyObj = GameObject.Instantiate(LoadedObj) as GameObject;
			MyObj.transform.parent = SleepEnemyParentObj.transform;
			MyObj.SetActive(false);
			return MyObj; 
			
		}); 

		LoadedObj = null;
		LoadedObj = Resources.Load ("Prefaps/00.Objects/Enemies/Split/SplitEnemyS") as GameObject; 
		m_SplitSEnemyObjPool = new CGameObjectPool<GameObject>(30, () =>  {
			
			GameObject MyObj = GameObject.Instantiate(LoadedObj) as GameObject;
			MyObj.transform.parent = SleepEnemyParentObj.transform;
			MyObj.SetActive(false);
			return MyObj; 
			
		}); 

		LoadedObj = null;
		LoadedObj = Resources.Load ("Prefaps/00.Objects/Enemies/Split/SplitEnemyM") as GameObject; 
		m_SplitMEnemyObjPool = new CGameObjectPool<GameObject>(30, () =>  {
			
			GameObject MyObj = GameObject.Instantiate(LoadedObj) as GameObject;
			MyObj.transform.parent = SleepEnemyParentObj.transform;
			MyObj.SetActive(false);
			return MyObj; 
			
		}); 

		LoadedObj = null;
		LoadedObj = Resources.Load ("Prefaps/00.Objects/Enemies/Split/ChildEnemy") as GameObject; 
		m_ChildEnemyObjPool = new CGameObjectPool<GameObject>(30, () =>  {
			
			GameObject MyObj = GameObject.Instantiate(LoadedObj) as GameObject;
			MyObj.transform.parent = SleepEnemyParentObj.transform;
			MyObj.SetActive(false);
			return MyObj; 
			
		});

		LoadedObj = null;
		LoadedObj = Resources.Load ("Prefaps/00.Objects/Enemies/Follow/FollowEnemyS") as GameObject; 
		m_FollowEnemyObjPool = new CGameObjectPool<GameObject>(30, () =>  {
			
			GameObject MyObj = GameObject.Instantiate(LoadedObj) as GameObject;
			MyObj.transform.parent = SleepEnemyParentObj.transform;
			MyObj.SetActive(false);
			return MyObj; 
			
		});

		LoadedObj = null;
		LoadedObj = Resources.Load ("Prefaps/00.Objects/Enemies/Move/MoveEnemyS") as GameObject; 
		m_MoveEnemyObjPool = new CGameObjectPool<GameObject>(30, () =>  {
			
			GameObject MyObj = GameObject.Instantiate(LoadedObj) as GameObject;
			MyObj.transform.parent = SleepEnemyParentObj.transform;
			MyObj.SetActive(false);
			return MyObj; 
			
		}); 

		LoadedObj = null;
		LoadedObj = Resources.Load ("Prefaps/00.Objects/Enemies/Imm/ImmEnemyS") as GameObject; 
		m_ImmEnemyObjPool = new CGameObjectPool<GameObject>(30, () =>  {
			
			GameObject MyObj = GameObject.Instantiate(LoadedObj) as GameObject;
			MyObj.transform.parent = SleepEnemyParentObj.transform;
			MyObj.SetActive(false);
			return MyObj; 
			
		}); 

		LoadedObj = null;
		LoadedObj = Resources.Load ("Prefaps/00.Objects/Enemies/Coin/CoinEnemyS") as GameObject; 
		m_CoinSEnemyObjPool = new CGameObjectPool<GameObject>(30, () =>  {
			
			GameObject MyObj = GameObject.Instantiate(LoadedObj) as GameObject;
			MyObj.transform.parent = SleepEnemyParentObj.transform;
			MyObj.SetActive(false);
			return MyObj; 
			
		}); 

		LoadedObj = null;
		LoadedObj = Resources.Load ("Prefaps/00.Objects/Enemies/Coin/CoinEnemyM") as GameObject; 
		m_CoinMEnemyObjPool = new CGameObjectPool<GameObject>(30, () =>  {
			
			GameObject MyObj = GameObject.Instantiate(LoadedObj) as GameObject;
			MyObj.transform.parent = SleepEnemyParentObj.transform;
			MyObj.SetActive(false);
			return MyObj; 
			
		}); 

		//Item
		LoadedObj = null;
		LoadedObj = Resources.Load ("Prefaps/00.Objects/Point") as GameObject; 
		m_PointObjPool = new CGameObjectPool<GameObject>(50, () =>  {
			
			GameObject MyObj = GameObject.Instantiate(LoadedObj) as GameObject;
			MyObj.transform.parent = ItemParentObj.transform;
			MyObj.SetActive(false);
			return MyObj; 
			
		}); 

		m_objCoin = Resources.Load ("Prefaps/00.Objects/Coin") as GameObject; 

	//		LoadedObj = null;
	//		LoadedObj = Resources.Load ("Prefaps/00.Objects/Coin") as GameObject; 
	//		m_CoinObjPool = new CGameObjectPool<GameObject>(50, () =>  {
	//			
	//			GameObject MyObj = GameObject.Instantiate(LoadedObj) as GameObject;
	//			MyObj.transform.parent = ItemParentObj.transform;
	//			MyObj.SetActive(false);
	//			return MyObj; 
	//			
	//		}); 

		//<-----End
		m_obLevelupEffect = Resources.Load ("Prefaps/00.Objects/LevelupEffect") as GameObject;

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
		objBoom.transform.parent = BulletParentObj.transform;
		
		return objBoom;
	}

	public GameObject CreateNormalEnemy(Vector3 Vec3Pos, ENEMY_ID EnemyID = ENEMY_ID.NORMAL_S)
	{
		CGameObjectPool<GameObject> ObjPool;

		switch (EnemyID) {
		case ENEMY_ID.NORMAL_S:
			ObjPool = m_NormalSEnemyObjPool;
			break;
		case ENEMY_ID.NORMAL_M:
			ObjPool = m_NormalMEnemyObjPool;
			break;
		case ENEMY_ID.SPEED:
			ObjPool = m_SpeedEnemyObjPool;
			break;
		case ENEMY_ID.SPLIT_S:
			ObjPool = m_SplitSEnemyObjPool;
			break;
		case ENEMY_ID.SPLIT_M:
			ObjPool = m_SplitMEnemyObjPool;
			break;
		case ENEMY_ID.CHILD:
			ObjPool = m_ChildEnemyObjPool;
			break;
		case ENEMY_ID.FOLLOW_S:
			ObjPool = m_FollowEnemyObjPool;
			break;
		case ENEMY_ID.MOVE_S:
			ObjPool = m_MoveEnemyObjPool;
			break;
		case ENEMY_ID.IMM:
			ObjPool = m_ImmEnemyObjPool;
			break;
		case ENEMY_ID.COIN_S:
			ObjPool = m_CoinSEnemyObjPool;
			break;
		case ENEMY_ID.COIN_M:
			ObjPool = m_CoinMEnemyObjPool;
			break;
		default:
			ObjPool = null;
			break;

		}

		GameObject objEnemy = ObjPool.pop();
		objEnemy.transform.position = Vec3Pos;
		objEnemy.transform.parent = EnemyParentObj.transform;
		objEnemy.SetActive (true);
		
		return objEnemy;
	}

	public void DestroyEnemy(GameObject obj, ENEMY_ID EnemyID = ENEMY_ID.NORMAL_S)
	{
		CGameObjectPool<GameObject> ObjPool;
		
		switch (EnemyID) {
		case ENEMY_ID.NORMAL_S:
			ObjPool = m_NormalSEnemyObjPool;
			break;
		case ENEMY_ID.NORMAL_M:
			ObjPool = m_NormalMEnemyObjPool;
			break;
		case ENEMY_ID.SPEED:
			ObjPool = m_SpeedEnemyObjPool;
			break;
		case ENEMY_ID.SPLIT_S:
			ObjPool = m_SplitSEnemyObjPool;
			break;
		case ENEMY_ID.SPLIT_M:
			ObjPool = m_SplitMEnemyObjPool;
			break;
		case ENEMY_ID.CHILD:
			ObjPool = m_ChildEnemyObjPool;
			break;
		case ENEMY_ID.FOLLOW_S:
			ObjPool = m_FollowEnemyObjPool;
			break;
		case ENEMY_ID.MOVE_S:
			ObjPool = m_MoveEnemyObjPool;
			break;
		case ENEMY_ID.IMM:
			ObjPool = m_ImmEnemyObjPool;
			break;
		case ENEMY_ID.COIN_S:
			ObjPool = m_CoinSEnemyObjPool;
			break;
		case ENEMY_ID.COIN_M:
			ObjPool = m_CoinMEnemyObjPool;
			break;
		default:
			ObjPool = null;
			break;
		}

		obj.SetActive (false);
		obj.transform.parent = SleepEnemyParentObj.transform;
		ObjPool.push (obj);

	}


	public void CreatePoint(Vector3 Vec3Pos, int iNum = 1)
	{
		GameObject objPoint;

		for (int i = 0; i < iNum; ++i) {
			objPoint = m_PointObjPool.pop();
			objPoint.transform.position = Vec3Pos;
			objPoint.SetActive(true);
		}
	}

	public void CreateCoin(Vector3 Vec3Pos, int iNum = 1)
	{
		GameObject objCoin;
		
		for (int i = 0; i < iNum; ++i) {
			objCoin = GameObject.Instantiate(m_objCoin, Vec3Pos, Quaternion.identity) as GameObject;
		}
	}

	public void DestoryPoint(GameObject objPoint)
	{
		objPoint.SetActive (false);
		m_PointObjPool.push (objPoint);
	}

	public void DestroyCoin(GameObject objCoin)
	{
		GameObject.Destroy (objCoin);
	}

	public GameObject Create_LevelupEffect(Vector3 Vec3Pos)
	{
		return GameObject.Instantiate (m_obLevelupEffect, Vec3Pos, Quaternion.identity) as GameObject;
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
