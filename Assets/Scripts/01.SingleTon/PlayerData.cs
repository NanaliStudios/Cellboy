using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using TapjoyUnity;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
//using ChartboostSDK;

public class PlayerData : MonoBehaviour {

	public PLAYER_ID m_PlayerID = PLAYER_ID.NORMAL;
	public string m_strPlayerName = "";
	public string m_strPlayerInfo = "";
	public int m_iChargePrice = 0;
	public int m_iBuyPrice = 0;


	public int m_iCurrentScore = 0;
	public int m_iCurrentCoin = 0;
	public int m_iCurrentPlayNum = 0;
	public bool m_bWinHighScore = false;
	public Color m_BackColor = Color.white;
	
	public int m_iPlayCountForAd = 0;

	public float m_fPlayer0_ChargeTime = 3;
	public float m_fPlayer1_ChargeTime = 10;
	public float m_fPlayer2_ChargeTime = 60;
	public float m_fPlayer3_ChargeTime = 240;
	public float m_fPlayer4_ChargeTime = 360;
	
	public GameData m_Gamedata = null;
	private byte[] m_ByteGameData = null;

	// Use this for initialization
	void Start () {

		Application.targetFrameRate = 60;
		//data for cloud 

		DontDestroyOnLoad (this);
	}

	public void Create_SaveData()
	{
		GameData MyGameData = new GameData();
		MyGameData.Initialize();
		FileSystem.WriteGameDataFromFile(MyGameData, "SaveData");
		
		m_Gamedata = MyGameData;

		Debug.Log ("Create Save Data Complete");
	}


	public void GameData_Save()
	{
		FileSystem.WriteGameDataFromFile(m_Gamedata, "SaveData");
		Debug.Log ("Save GameData Complete");

		if (GameSDK_Funcs.isInitialized ()) {
			BinaryFormatter b = new BinaryFormatter();
			MemoryStream m = new MemoryStream();

			b.Serialize(m, m_Gamedata);


			Debug.Log(m.GetBuffer().Length);
			GameSDK_Funcs.Do_CloudSave (m.GetBuffer());
		}
	}

	public void GameData_Load()
	{
		Debug.Log ("Try Gamedata Load");
		m_ByteGameData = GameSDK_Funcs.CurrentSaveDAta;

		if (m_ByteGameData != null) {
			BinaryFormatter b = new BinaryFormatter ();
			MemoryStream m = new MemoryStream (m_ByteGameData);

			Debug.Log(m.ToArray().Length);
			m_Gamedata = b.Deserialize (m) as GameData;
			Debug.Log ("Move CloudData to CurrentSaveData");
		} else {
			Debug.Log("LocalSave");
			m_Gamedata = FileSystem.ReadGameDataFromFile ("SaveData");
			if(m_Gamedata == null)
				Create_SaveData();
		}
	}


	void OnApplicationPause(bool pauseStatus) {
		if (pauseStatus)
			Vungle.onPause ();
		else
			Vungle.onResume ();
	}
}
