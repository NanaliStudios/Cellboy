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

		if (m_PlayerData.m_Gamedata.m_PlayerInfo [(int)m_PlayerData.m_PlayerID].bIsLock == true) {
			m_MyText.text = m_strLockInfo;
			return;
		} else
			m_MyText.text = m_PlayerData.m_strPlayerInfo;
	}

}
