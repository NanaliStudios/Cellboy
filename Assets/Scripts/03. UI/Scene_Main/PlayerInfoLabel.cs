using UnityEngine;
using System.Collections;

public class PlayerInfoLabel : TextBase {

	private PlayerData m_PlayerData = null;

	void Awake()
	{
		Initialize ();
		m_PlayerData = GameObject.Find ("PlayerData(Clone)").GetComponent<PlayerData>();
	}

	// Use this for initialization
	void FixedUpdate() {

		bool m_bCurrentLock = m_PlayerData.m_Gamedata.m_PlayerInfo [(int)m_PlayerData.m_PlayerID].bIsLock;
		
		if (m_bCurrentLock == true) {
			
			int iPlayerIdx = ((int)m_PlayerData.m_PlayerID) -1;
			
			if(iPlayerIdx > 0)
			{
				
				if(m_PlayerData.m_Gamedata.m_PlayerInfo [((int)m_PlayerData.m_PlayerID) -1].bIsLock == true)
				{
					m_MyText.text = Localization.Get("LOCK");
					return;
				}
			}
			
			m_MyText.text = m_PlayerData.m_strPlayerInfo;
			return;
		}
	}

}
