using UnityEngine;
using System.Collections;
using System.IO;


public class GameData {

	public struct PlayerInfo
	{
		public float[] fIsSleep;
		public float[] fChargeTime;
		public int[] iIsCharging;
		public int[] iIsLock;
	};

	
	public int m_iPlayNum = 0;
	public int m_iHighScore = 0;
	public int m_iHaveCoin = 0;
	public PlayerInfo m_PlayerInfo = new PlayerInfo();


	public void Initialize()
	{
		int iPlayerNum = 5;
	
		m_PlayerInfo.fIsSleep = new float[iPlayerNum];
		m_PlayerInfo.fChargeTime = new float[iPlayerNum];
		m_PlayerInfo.iIsCharging = new int[iPlayerNum];
		m_PlayerInfo.iIsLock = new int[iPlayerNum];
	}


}
