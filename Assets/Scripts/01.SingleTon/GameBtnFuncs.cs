using UnityEngine;
using System.Collections;

public partial class GameSystem : MonoBehaviour {

	//===Button===
	public void OnClickPause()
	{
		Time.timeScale = 0.0f;
		m_GameMenu.SetActive (false);
		m_PauseMenu.SetActive (true);
		
		m_PauseBtn.SetActive (false);
	}
	
	public void OnClickResume()
	{
		Time.timeScale = 1.0f;
		
		m_GameMenu.SetActive (true);
		m_PauseMenu.SetActive (false);
		m_ContinueMenu.SetActive (false);
		
		m_PauseBtn.SetActive (true);
	}
	
	public void OnClickHome()
	{
		Time.timeScale = 1.0f;
		m_iCurrent_GameScore  = 0;
		SetScore ();
		Application.LoadLevel ("00_MAIN");
	}
	
	public void OnClickNoContinue()
	{
		m_bOnContinue = false;
	}
	
	//unityads
	public void AdsBtnClick()
	{
		if (!AdFunctions.Show_UnityAds ()) {
		}
		m_bAdsOn = true;
		
	}
	
	public void Check_AdsReward()
	{
		if (m_bAdsOn == true) {
			
			if(AdFunctions.m_bAdsComplete == true)
			{
				TapjoyManager.Instance.TrackCustomEvent ("RewardAD", "GameContinue", "PlayerName: " + m_PlayerData.m_strPlayerName, "GameScore: " + m_iCurrent_GameScore.ToString());
				
				//restart
				GameObject EnemyCase = m_PrefapMgr.Get_EnemyParent();
				
				for(int i = 0; i < EnemyCase.transform.childCount; ++i)
				{
					if(EnemyCase.transform.GetChild(i).GetComponent<EnemyBase>().m_EnemyID == ENEMY_ID.IMM)
						Destroy(EnemyCase.transform.GetChild(i).gameObject);
					else
						EnemyCase.transform.GetChild(i).GetComponent<EnemyBase>().m_iHp = 0;
				}
				
				m_objPlayer.transform.position = new Vector3(0.0f, -1.5f);
				m_objPlayer.GetComponent<Player>().Set_AnimIdle();
				
				m_bAdsOn = false;
				m_bGameover = false;
				m_fGameoverTimer = 0.0f;
				m_bIsGameStart = false;
				Physics2D.gravity = new Vector3 (0.0f, 0.0f, 0.0f);
				AdFunctions.m_bAdsComplete = false;
				m_CanRestart = false;
				m_bOnContinue = false;
				gameObject.GetComponent<AudioSource>().enabled = true;
				OnClickResume();
			}
			
		}
	}
	
	//<-----End

}
