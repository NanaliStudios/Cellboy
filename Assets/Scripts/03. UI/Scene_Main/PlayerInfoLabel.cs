using UnityEngine;
using System.Collections;

public class PlayerInfoLabel : TextBase {

	private PlayerData m_PlayerData = null;
	public string m_strLockInfo = "";

	void Awake()
	{
		Initialize ();
		m_PlayerData = GameObject.Find ("PlayerData(Clone)").GetComponent<PlayerData>();
	}

	// Use this for initialization
	void FixedUpdate() {

		if (m_PlayerData.m_PlayerID == PLAYER_ID.NORMAL) {
			
			m_MyText.text = m_PlayerData.m_strPlayerInfo;

		} else if (m_PlayerData.m_PlayerID == PLAYER_ID.SPREAD) {
			
			m_MyText.text = m_PlayerData.m_strPlayerInfo;

		} else if (m_PlayerData.m_PlayerID == PLAYER_ID.LASER) {
			
			if (PlayerPrefs.GetInt ("Player1Lock") == 1)
			{
				m_MyText.text = m_strLockInfo;
				return;
			}
			else 
				m_MyText.text = m_PlayerData.m_strPlayerInfo;
		}
		else if (m_PlayerData.m_PlayerID == PLAYER_ID.HOMING) {
			
			if (PlayerPrefs.GetInt ("Player2Lock") == 1)
			{
				m_MyText.text = m_strLockInfo;
				return;
			}
			else 
				m_MyText.text = m_PlayerData.m_strPlayerInfo;
			
		} else if (m_PlayerData.m_PlayerID == PLAYER_ID.BOOM) {

			if (PlayerPrefs.GetInt ("Player3Lock") == 1)
			{
				m_MyText.text = m_strLockInfo;
				return;
			}
			else 
				m_MyText.text = m_PlayerData.m_strPlayerInfo;
		
		} 
	
	}
}
