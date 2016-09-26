using UnityEngine;
using System.Collections;

public partial class GameSystem : MonoBehaviour {

	//===Button===
	public void OnClickPause()
	{
		m_fResumeTerm = 3.0f;
		m_fResumeTimer = 0.0f;

		m_bPause = true;
		m_bResume = false;
		Time.timeScale = 0.0f;
		m_GameMenu.SetActive (false);
		m_PauseMenu.SetActive (true);
		m_PauseBtn.SetActive (false);

		m_WaitLabel.SetActive (false);
	}
	
	public void OnClickResume()
	{
		m_GameMenu.SetActive (true);
		m_PauseMenu.SetActive (false);
		m_ContinueMenu.SetActive (false);
		
		m_PauseBtn.SetActive (true);
		m_WaitLabel.SetActive (true);
		m_bResume = true;
	}

	public void ToResumeState()
	{
		Time.timeScale = 1.0f;
		m_WaitLabel.SetActive (false);

		m_bResume = false;
		m_bPause = false;
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
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			m_NetworkFail_Label.GetComponent<TweenAlpha> ().ResetToBeginning ();
			m_NetworkFail_Label.GetComponent<TweenAlpha> ().enabled = true;
			m_NetworkFail_Label.SetActive (true);

			return;
		}
		if (!AdFunctions.Show_UnityAds ()) {
				MobileNativeMessage msg = new MobileNativeMessage ("Show Ads Fail", Localization.Get("ADS_OUT"));

				return;
			}
		m_bAdsOn = true;
		
	}
	
	public void Check_AdsReward()
	{
		if (m_bAdsOn == true) {
			
			if(AdFunctions.m_bAdsComplete == true)
			{
				string strPlayerTrackID = "";

				switch (m_PlayerData.m_PlayerID) {

				case PLAYER_ID.NORMAL:
					strPlayerTrackID = "NORMAL";
					break;
				case PLAYER_ID.SPREAD:
					strPlayerTrackID = "SPREAD";
					break;
				case PLAYER_ID.LASER:
					strPlayerTrackID = "LASER";
					break;
				case PLAYER_ID.HOMING:
					strPlayerTrackID = "HOMING";
					break;
				case PLAYER_ID.BOOM:
					strPlayerTrackID = "BOOM";
					break;

				}

				TapjoyManager.Instance.TrackCustomEvent ("Continue_RewardAD", strPlayerTrackID, "Gamescore :" + m_iCurrent_GameScore + " PlayNum :" + m_PlayerData.m_Gamedata.m_iTotalPlayerNum, "");
				
				//restart
				Delete_AllEnemy ();
				
				m_objPlayer.transform.position = new Vector3(0.0f, -2.0f);
				m_objPlayer.GetComponent<Player>().Set_AnimIdle();
				
				m_bAdsOn = false;
				m_bGameover = false;
				m_fGameoverTimer = 0.0f;
				m_bIsGameStart = false;
				Physics2D.gravity = new Vector3 (0.0f, 0.0f, 0.0f);
				AdFunctions.m_bAdsComplete = false;
				m_CanRestart = false;
				//m_bOnContinue = false;
				gameObject.GetComponent<AudioSource>().enabled = true;
				OnClickResume();
			}
			
		}
	}
	
	//<-----End

}
