using UnityEngine;
using System.Collections;

public class TiredLabel : TextBase {
	
	private PLAYER_ID m_CurrentPlayerID = PLAYER_ID.NORMAL;
	private float m_fCurrentTired = 0.0f;
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
		
		switch (m_CurrentPlayerID) {
		case PLAYER_ID.NORMAL:
			m_fCurrentTired = PlayerPrefs.GetFloat("fPlayer0Tired");
			break;
		case PLAYER_ID.SPREAD:
			m_fCurrentTired = PlayerPrefs.GetFloat("fPlayer1Tired");
			break;
		case PLAYER_ID.LASER:
			m_fCurrentTired = PlayerPrefs.GetFloat("fPlayer2Tired");
			break;
		case PLAYER_ID.HOMING:
			m_fCurrentTired = PlayerPrefs.GetFloat("fPlayer3Tired");
			break;
		case PLAYER_ID.BOOM:
			m_fCurrentTired = PlayerPrefs.GetFloat("fPlayer4Tired");
			break;
		}
		m_MyText.text = m_fCurrentTired + "%";
		
	}
}
