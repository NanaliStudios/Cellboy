using UnityEngine;
using System.Collections;
using System.IO;

[System.Serializable]
public class GameData {

	[System.Serializable]
	public struct PlayerInfo
	{
		public bool bIsSleep;
		public bool bIsLock;
		public float fTiredPercent;
		public float fTiredCost;
		public int iSleepMin;

		public System.DateTime SleepEnd_Time;
	};

	public int m_iPlayNum = 0;
	public int m_iTotalPlayerNum = (int)PLAYER_ID.END;
	public int m_iHighScore = 0;
	public int m_iHaveCoin = 0;
	public PlayerInfo[] m_PlayerInfo = new PlayerInfo[5];
#if UNITY_EDITOR_OSX
	public bool m_bAdOff = false;
#endif


	public void Initialize()
	{
		//m_PlayerInfo = new PlayerInfo [m_iTotalPlayerNum];

		for (int i = 0; i < 5; ++i)
			m_PlayerInfo [i].fTiredPercent = 100.0f;

		m_PlayerInfo [(int)PLAYER_ID.NORMAL].fTiredCost = 10.0f;
		m_PlayerInfo [(int)PLAYER_ID.SPREAD].fTiredCost = 15.0f;
		m_PlayerInfo [(int)PLAYER_ID.LASER].fTiredCost = 18.0f;
		m_PlayerInfo [(int)PLAYER_ID.HOMING].fTiredCost = 20.0f;
		m_PlayerInfo [(int)PLAYER_ID.BOOM].fTiredCost = 25.0f;

		m_PlayerInfo [(int)PLAYER_ID.NORMAL].iSleepMin = 3;
		m_PlayerInfo [(int)PLAYER_ID.SPREAD].iSleepMin = 10;
		m_PlayerInfo [(int)PLAYER_ID.LASER].iSleepMin = 60;
		m_PlayerInfo [(int)PLAYER_ID.HOMING].iSleepMin = 240;
		m_PlayerInfo [(int)PLAYER_ID.BOOM].iSleepMin = 360;

		m_PlayerInfo [(int)PLAYER_ID.NORMAL].bIsLock = false;
		m_PlayerInfo [(int)PLAYER_ID.SPREAD].bIsLock = true;
		m_PlayerInfo [(int)PLAYER_ID.LASER].bIsLock = true;
		m_PlayerInfo [(int)PLAYER_ID.HOMING].bIsLock = true;
		m_PlayerInfo [(int)PLAYER_ID.BOOM].bIsLock = true;
	}

	public void Spend_TiredVal(PLAYER_ID PlayerID)
	{
		m_PlayerInfo [(int)PlayerID].fTiredPercent -= m_PlayerInfo [(int)PlayerID].fTiredCost;
	}


}
