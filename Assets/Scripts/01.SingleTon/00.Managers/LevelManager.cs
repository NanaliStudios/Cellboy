using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	LEVEL_STATUS m_LevelStatus = LEVEL_STATUS.NORMAL;

	public int m_iCurrentStage = 0;
	private float m_fGameTimer = 0.0f;
	private float m_fChangeStgaeTerm = 15.0f;

	private float m_fEnemySpawnTimer = 0.0f;
	private float m_fEnemySpawnTerm = 0.8f;

	private PrefapManager m_PrefMgr = null;
	private GameSystem m_GameSys = null;
	private Rigidbody2D m_PlayerRigid2D = null;

	//Enemy SpawnChance
	private float m_fNormalSChance = 0.0f;
	private float m_fNormalMChance = 0.0f;
	private float m_fSpeedChance = 0.0f;
	private float m_fSplitSChance = 0.0f;
	private float m_fSplitMChance = 0.0f;
	private float m_fMoveChance = 0.0f;
	private float m_fFollowChance = 0.0f;
	private float m_fImmChance = 0.0f;
	private float m_fCoinSChance = 0.0f;
	private float m_fCoinMChance = 0.0f;
	

	public void Initialize(GameSystem GameSys ,PrefapManager prefMgr, Rigidbody2D PlayerRigid2D)
	{
		m_GameSys = GameSys;
		m_PrefMgr = prefMgr;
		m_PlayerRigid2D = PlayerRigid2D;
		SetStage ();
	}


	public void Progress () {
	

		if (m_fGameTimer >= m_fChangeStgaeTerm) {		//Change Next Stage
			++m_iCurrentStage;
			m_fGameTimer = 0.0f;
			SetStage();
		}

		if (m_fEnemySpawnTimer >= m_fEnemySpawnTerm) {		//Enemy Spawn

			float fRandSpawnVal = Random.Range(0.0f, 100.0f);
			ENEMY_ID SpawnEnemyID = ENEMY_ID.NORMAL_S;

			if(fRandSpawnVal <= m_fNormalSChance)
				SpawnEnemyID = ENEMY_ID.NORMAL_S;
			else if(fRandSpawnVal <= m_fNormalSChance + m_fNormalMChance)
				SpawnEnemyID = ENEMY_ID.NORMAL_M;
			else if(fRandSpawnVal <= m_fNormalSChance + m_fNormalMChance + m_fSpeedChance)
				SpawnEnemyID = ENEMY_ID.SPEED;
			else if(fRandSpawnVal <= m_fNormalSChance + m_fNormalMChance + m_fSpeedChance + m_fSplitSChance)
				SpawnEnemyID = ENEMY_ID.SPLIT_S;
			else if(fRandSpawnVal <= m_fNormalSChance + m_fNormalMChance + m_fSpeedChance + m_fSplitSChance + m_fSplitMChance)
				SpawnEnemyID = ENEMY_ID.SPLIT_M;
			else if(fRandSpawnVal <= m_fNormalSChance + m_fNormalMChance + m_fSpeedChance + m_fSplitSChance + m_fSplitMChance
			        + m_fMoveChance)
				SpawnEnemyID = ENEMY_ID.MOVE_S;
			else if(fRandSpawnVal <= m_fNormalSChance + m_fNormalMChance + m_fSpeedChance + m_fSplitSChance + m_fSplitMChance
			        + m_fMoveChance + m_fFollowChance)
				SpawnEnemyID = ENEMY_ID.FOLLOW_S;
			else if(fRandSpawnVal <= m_fNormalSChance + m_fNormalMChance + m_fSpeedChance + m_fSplitSChance + m_fSplitMChance
			        + m_fImmChance)
				SpawnEnemyID = ENEMY_ID.IMM;
			else if(fRandSpawnVal <= m_fNormalSChance + m_fNormalMChance + m_fSpeedChance + m_fSplitSChance + m_fSplitMChance
			        + m_fMoveChance + m_fFollowChance + m_fImmChance + m_fCoinSChance)
				SpawnEnemyID = ENEMY_ID.COIN_S;
			else if(fRandSpawnVal <= m_fNormalSChance + m_fNormalMChance + m_fSpeedChance + m_fSplitSChance + m_fSplitMChance
			        + m_fMoveChance + m_fFollowChance + m_fImmChance + m_fCoinSChance + m_fCoinMChance)
				SpawnEnemyID = ENEMY_ID.COIN_M;

			m_PrefMgr.CreateNormalEnemy(new Vector3(Random.Range(-2.4f, 2.4f), 5.0f), SpawnEnemyID);

			//m_PrefMgr.CreateNormalEnemy(new Vector3(Random.Range(-2.6f, 2.6f), 5.0f), ENEMY_ID.FOLLOW_S);
			m_fEnemySpawnTimer = 0.0f;
		}

		if (m_GameSys.CheckGameStart ()) {
			m_fEnemySpawnTimer += Time.deltaTime;
			m_fGameTimer += Time.deltaTime;
		}
	}

	void SetStage()
	{
		switch (m_iCurrentStage) {
		case 0:
			m_fNormalSChance = 95.0f;	//small normal
			m_fNormalMChance = 0.0f;	//midium normal

			m_fSpeedChance = 0.0f;		//speed

			m_fSplitSChance = 0.0f;		//small split
			m_fSplitMChance = 0.0f;		//midium split

			m_fMoveChance = 0.0f;		//Move
			m_fFollowChance = 0.0f;		//follow

			m_fImmChance = 0.0f;		//immortal

			m_fCoinSChance = 4.0f;		//small coin	
			m_fCoinMChance = 1.0f;		//midium coin

			m_fEnemySpawnTerm = 1.0f;	//SpawnTerm
			m_PlayerRigid2D.gravityScale = 0.6f; //set player speed
			break;
		case 1:
			m_fNormalSChance = 55.0f;	//small normal
			m_fNormalMChance = 20.0f;	//midium normal
			
			m_fSpeedChance = 10.0f;		//speed
			
			m_fSplitSChance = 0.0f;		//small split
			m_fSplitMChance = 0.0f;		//midium split
			
			m_fMoveChance = 0.0f;		//Move
			m_fFollowChance = 0.0f;		//follow
			
			m_fImmChance = 10.0f;		//immortal
			
			m_fCoinSChance = 4.0f;		//small coin	
			m_fCoinMChance = 1.0f;		//midium coin
			
			m_fEnemySpawnTerm = 1.0f;	//SpawnTerm
			m_PlayerRigid2D.gravityScale = 0.8f; //set player speed
			break;
		case 2:
			m_fNormalSChance = 40.0f;	//small normal
			m_fNormalMChance = 20.0f;	//midium normal
			
			m_fSpeedChance = 10.0f;		//speed
			
			m_fSplitSChance = 5.0f;		//small split
			m_fSplitMChance = 0.0f;		//midium split
			
			m_fMoveChance = 0.0f;		//Move
			m_fFollowChance = 0.0f;		//follow
			
			m_fImmChance = 20.0f;		//immortal
			
			m_fCoinSChance = 4.0f;		//small coin	
			m_fCoinMChance = 1.0f;		//midium coin
			
			m_fEnemySpawnTerm = 0.8f;	//SpawnTerm
			m_PlayerRigid2D.gravityScale = 1.0f; //set player speed
			break;
		case 3:
			m_fNormalSChance = 10.0f;	//small normal
			m_fNormalMChance = 30.0f;	//midium normal
			
			m_fSpeedChance = 10.0f;		//speed
			
			m_fSplitSChance = 15.0f;		//small split
			m_fSplitMChance = 10.0f;		//midium split
			
			m_fMoveChance = 0.0f;		//Move
			m_fFollowChance = 0.0f;		//follow
			
			m_fImmChance = 20.0f;		//immortal
			
			m_fCoinSChance = 4.0f;		//small coin	
			m_fCoinMChance = 1.0f;		//midium coin
			
			m_fEnemySpawnTerm = 0.8f;	//SpawnTerm
			m_PlayerRigid2D.gravityScale = 1.2f; //set player speed
			break;
		case 4:
			m_fNormalSChance = 5.0f;	//small normal
			m_fNormalMChance = 25.0f;	//midium normal
			
			m_fSpeedChance = 10.0f;		//speed
			
			m_fSplitSChance = 20.0f;		//small split
			m_fSplitMChance = 15.0f;		//midium split
			
			m_fMoveChance = 0.0f;		//Move
			m_fFollowChance = 0.0f;		//follow
			
			m_fImmChance = 20.0f;		//immortal
			
			m_fCoinSChance = 4.0f;		//small coin	
			m_fCoinMChance = 1.0f;		//midium coin
			
			m_fEnemySpawnTerm = 0.7f;	//SpawnTerm
			m_PlayerRigid2D.gravityScale = 1.2f; //set player speed
			break;
		case 5:
			m_fNormalSChance = 5.0f;	//small normal
			m_fNormalMChance = 15.0f;	//midium normal
			
			m_fSpeedChance = 10.0f;		//speed
			
			m_fSplitSChance = 20.0f;		//small split
			m_fSplitMChance = 15.0f;		//midium split
			
			m_fMoveChance = 10.0f;		//Move
			m_fFollowChance = 0.0f;		//follow
			
			m_fImmChance = 20.0f;		//immortal
			
			m_fCoinSChance = 4.0f;		//small coin	
			m_fCoinMChance = 1.0f;		//midium coin
			
			m_fEnemySpawnTerm = 0.7f;	//SpawnTerm
			m_PlayerRigid2D.gravityScale = 1.2f; //set player speed
			break;
		case 6:
			m_fNormalSChance = 0.0f;	//small normal
			m_fNormalMChance = 5.0f;	//midium normal
			
			m_fSpeedChance = 10.0f;		//speed
			
			m_fSplitSChance = 25.0f;		//small split
			m_fSplitMChance = 20.0f;		//midium split
			
			m_fMoveChance = 15.0f;		//Move
			m_fFollowChance = 0.0f;		//follow
			
			m_fImmChance = 20.0f;		//immortal
			
			m_fCoinSChance = 4.0f;		//small coin	
			m_fCoinMChance = 1.0f;		//midium coin
			
			m_fEnemySpawnTerm = 0.6f;	//SpawnTerm
			m_PlayerRigid2D.gravityScale = 1.3f; //set player speed
			break;
		case 7:
			m_fNormalSChance = 0.0f;	//small normal
			m_fNormalMChance = 5.0f;	//midium normal
			
			m_fSpeedChance = 10.0f;		//speed
			
			m_fSplitSChance = 15.0f;		//small split
			m_fSplitMChance = 10.0f;		//midium split
			
			m_fMoveChance = 25.0f;		//Move
			m_fFollowChance = 10.0f;		//follow
			
			m_fImmChance = 20.0f;		//immortal
			
			m_fCoinSChance = 4.0f;		//small coin	
			m_fCoinMChance = 1.0f;		//midium coin
			
			m_fEnemySpawnTerm = 0.6f;	//SpawnTerm
			m_PlayerRigid2D.gravityScale = 1.3f; //set player speed
			break;
		case 8:
			m_fNormalSChance = 0.0f;	//small normal
			m_fNormalMChance = 5.0f;	//midium normal
			
			m_fSpeedChance = 5.0f;		//speed
			
			m_fSplitSChance = 10.0f;		//small split
			m_fSplitMChance = 5.0f;		//midium split
			
			m_fMoveChance = 30.0f;		//Move
			m_fFollowChance = 20.0f;		//follow
			
			m_fImmChance = 20.0f;		//immortal
			
			m_fCoinSChance = 4.0f;		//small coin	
			m_fCoinMChance = 1.0f;		//midium coin
			
			m_fEnemySpawnTerm = 0.6f;	//SpawnTerm
			m_PlayerRigid2D.gravityScale = 1.4f; //set player speed
			break;
		case 9:
			m_fNormalSChance = 0.0f;	//small normal
			m_fNormalMChance = 0.0f;	//midium normal
			
			m_fSpeedChance = 5.0f;		//speed
			
			m_fSplitSChance = 10.0f;		//small split
			m_fSplitMChance = 5.0f;		//midium split
			
			m_fMoveChance = 30.0f;		//Move
			m_fFollowChance = 25.0f;		//follow
			
			m_fImmChance = 20.0f;		//immortal
			
			m_fCoinSChance = 4.0f;		//small coin	
			m_fCoinMChance = 1.0f;		//midium coin
			
			m_fEnemySpawnTerm = 0.6f;	//SpawnTerm
			m_PlayerRigid2D.gravityScale = 1.5f; //set player speed
			break;
		case 10:
			m_fNormalSChance = 0.0f;	//small normal
			m_fNormalMChance = 0.0f;	//midium normal
			
			m_fSpeedChance = 5.0f;		//speed
			
			m_fSplitSChance = 10.0f;		//small split
			m_fSplitMChance = 5.0f;		//midium split
			
			m_fMoveChance = 30.0f;		//Move
			m_fFollowChance = 25.0f;		//follow
			
			m_fImmChance = 20.0f;		//immortal
			
			m_fCoinSChance = 4.0f;		//small coin	
			m_fCoinMChance = 1.0f;		//midium coin
			
			m_fEnemySpawnTerm = 0.5f;	//SpawnTerm
			m_PlayerRigid2D.gravityScale = 1.5f; //set player speed
			break;


	}
	}
}
