using UnityEngine;
using System.Collections;

public class TimeLabel : TextBase {

	private PLAYER_ID m_CurrentPlayerID = PLAYER_ID.NORMAL;
	private PlayerData m_PlayerData = null;
	
	bool m_bInit = false;
	// Use this for initialization
	void Start () {
		Initialize ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (m_bInit == false) {
			m_PlayerData = GameObject.Find ("PlayerData(Clone)").GetComponent<PlayerData>();
			m_bInit = true;
		}
		
		m_CurrentPlayerID = m_PlayerData.m_PlayerID;
		System.DateTime TargetTime = System.DateTime.Now;


		int iPlayerIdx = (int)m_CurrentPlayerID;

		TargetTime = new System.DateTime(PlayerPrefs.GetInt(string.Format("TargetYear{0}", iPlayerIdx)), PlayerPrefs.GetInt(string.Format("TargetMonth{0}", iPlayerIdx)), 
		                                 PlayerPrefs.GetInt(string.Format("TargetDay{0}", iPlayerIdx)),PlayerPrefs.GetInt(string.Format("TargetHour{0}", iPlayerIdx)),  
		                                 PlayerPrefs.GetInt(string.Format("TargetMin{0}", iPlayerIdx)),  PlayerPrefs.GetInt(string.Format("TargetSec{0}", iPlayerIdx))); 


		System.TimeSpan TimeSpan = TargetTime - System.DateTime.Now;
		m_MyText.text = TimeSpan.Hours + "h" + TimeSpan.Minutes + "m" + TimeSpan.Seconds + "s";
		
	}

}
