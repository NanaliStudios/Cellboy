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

	public bool m_bCanShowRate = false;
	public bool m_bCanShowFB = false;
	
	public GameData m_Gamedata = null;
	public byte[] m_ByteGameData = null;


	private GameSDKManager m_SdkMgr = null;

	// Use this for initialization
	void Start () {

		Application.targetFrameRate = 60;
		//data for cloud 

		DontDestroyOnLoad (this);
	
		m_SdkMgr = GameObject.Find ("GameSDKManager(Clone)").GetComponent<GameSDKManager>();

		m_ByteGameData = null;
	}

	public void Create_SaveData()
	{
		GameData MyGameData = new GameData();
		MyGameData.Initialize();
		FileSystem.WriteGameDataFromFile(MyGameData, "SaveData");
		
		m_Gamedata = MyGameData;

		Debug.Log ("Create Save Data Complete");

		//m_SdkMgr = GameObject.Find ("GameSDKManager(Clone)").GetComponent<GameSDKManager>();
	}


	public void GameData_Save()
	{
		FileSystem.WriteGameDataFromFile(m_Gamedata, "SaveData");
		Debug.Log ("Save GameData Complete");

		if (m_SdkMgr.isInitialized ()) {
			BinaryFormatter b = new BinaryFormatter();
			MemoryStream m = new MemoryStream();

			Debug.Log(m_Gamedata.m_iHaveCoin);
			b.Serialize(m, m_Gamedata);


			//Debug.Log(m.GetBuffer().Length);
			m_SdkMgr.Do_CloudSave (m.GetBuffer());
		}
	}

	public void GameData_Load()
	{
		Debug.Log ("Try Gamedata Load");
		//m_ByteGameData = m_SdkMgr.CurrentSaveDAta;

//		if (m_ByteGameData == null)
//			Debug.Log ("CloudLoad Failed");
//		if (m_SdkMgr == null)
//			Debug.Log ("gamesdkmgr is null");

		if (m_ByteGameData != null) {
			Debug.Log ("Cloud Load");
			BinaryFormatter b = new BinaryFormatter ();
			MemoryStream m = new MemoryStream (m_ByteGameData);


			m_Gamedata = b.Deserialize (m) as GameData;
			Debug.Log ("Move CloudData to CurrentSaveData");
		} else {
			Debug.Log("LocalLoad");
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
