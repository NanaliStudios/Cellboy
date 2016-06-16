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

	void Awake()
	{
	}

	
	private void ActionAvailableGameSavesLoaded (GooglePlayResult res) {
		GP_SnapshotMeta s =  GooglePlaySavedGamesManager.instance.AvailableGameSaves[0];
		GooglePlaySavedGamesManager.instance.LoadSpanshotByName(s.Title);
	}

	private void ActionGameSaveLoaded (GP_SpanshotLoadResult result) {

		byte[] Data = result.Snapshot.bytes;
		BinaryFormatter b = new BinaryFormatter ();
		MemoryStream m = new MemoryStream ();
	 	m.Write(Data, 0, Data.Length); 
		m_Gamedata = b.Deserialize (m) as GameData;

	}

	// Use this for initialization
	void Start () {

		Application.targetFrameRate = 60;
		GooglePlaySavedGamesManager.ActionAvailableGameSavesLoaded += ActionAvailableGameSavesLoaded;
		GooglePlaySavedGamesManager.ActionGameSaveLoaded += ActionGameSaveLoaded;
		//data for cloud 
		//if(GameSDK_Funcs.cloud)
		//m_FileSystem.
	
		//fordebug
		//PlayerPrefs.DeleteAll ();	

		if (PlayerPrefs.GetInt ("CurrentPlayNum") == 0) {

			//for cloud data
			Create_SaveData();

			Debug.Log("GameData Loaded");
		}
		else
		GameData_Load ();

		m_strPlayerName = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_strName;
		m_iChargePrice = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_iChargePrice;
		m_iBuyPrice = GameObject.Find ("ScrollView").gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg> ().m_iBuyPrice;

		TapjoyManager.Instance.ContentsReady ("Notice");


		DontDestroyOnLoad (this);
	}

	public void Create_SaveData()
	{
		GameData MyGameData = new GameData();
		MyGameData.Initialize();
		FileSystem.WriteGameDataFromFile(MyGameData, "SaveData");
		
		m_Gamedata = MyGameData;
	}


	public void GameData_Save()
	{
		byte[] byteData = FileSystem.WriteGameDataFromFile(m_Gamedata, "SaveData");

		if (GameSDK_Funcs.isInitialized ()) {
			BinaryFormatter b = new BinaryFormatter();
			MemoryStream m = new MemoryStream();

			b.Serialize(m, m_Gamedata);


			GameSDK_Funcs.Do_CloudSave (m.GetBuffer());
		}
	}

	public void GameData_Load()
	{
		if (GameSDK_Funcs.isInitialized ())
			GameSDK_Funcs.Do_CloudLoad ();
//		else
//		
//		m_Gamedata = FileSystem.ReadGameDataFromFile ("SaveData");
//
//
//		if (m_Gamedata == null)
//			Create_SaveData ();

	}

	void OnApplicationQuit() {
		GameData_Save ();
		Debug.Log ("Game Quit");
	}
}
