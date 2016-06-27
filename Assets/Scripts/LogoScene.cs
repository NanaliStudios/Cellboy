using UnityEngine;
using System.Collections;
using TapjoyUnity;
public class LogoScene : MonoBehaviour {

	private float m_fTimer = 0.0f;
	public float m_fTerm  = 5.0f;

	public GameObject m_objPlayerData = null;
	private PlayerData m_PlayerData = null;

	public bool m_bLateInit = false;

	void Awake()
	{
		if ((Screen.width / 3) * 4 == Screen.height)
			Camera.main.orthographicSize = 4.5f;

		Localization.language = "한국어";
	}

	// Use this for initialization
	void Start () {

		#if UNITY_EDITOR_OSX
		//PlayerPrefs.DeleteAll();
		#endif

		if (GameObject.Find ("PlayerData(Clone)") == null) {
			GameObject objPlayerData = Instantiate (m_objPlayerData) as GameObject;
			m_PlayerData = objPlayerData.GetComponent<PlayerData> ();
			Debug.Log ("Create PlayerData");
		} else
			m_PlayerData = GameObject.Find ("PlayerData(Clone)").GetComponent<PlayerData>();

		if (PlayerPrefs.HasKey ("CurrentPlayNum") == false) {
			PlayerPrefs.SetInt ("CurrentPlayNum", 0);
			PlayerPrefs.SetInt ("Adoff", 0);
			m_PlayerData.Create_SaveData();
		}

		if(!GameSDK_Funcs.isInitialized())
		GameSDK_Funcs.Initialize ();

		if(!AdFunctions.isInitialized())
		AdFunctions.Initialize ();

		if (PlayerPrefs.HasKey ("GameVolume") == false) {
			PlayerPrefs.SetFloat ("GameVolume", 3.0f);
		} else {
			if(PlayerPrefs.GetFloat("GameVolume") == 0.0f)
				AudioListener.volume = 0.0f;
		}
	
	}

	// Update is called once per frame
	void Update () {

			if (m_bLateInit == false) {
			if(GameSDK_Funcs.isInitialized())
			{
				if(PlayerPrefs.GetInt("CurrentPlayNum") != 0)
				GameSDK_Funcs.Do_CloudLoad ();

				m_bLateInit = true;
			}
		}

			if (m_fTerm <= m_fTimer) {
#if UNITY_EDITOR_OSX
			if(PlayerPrefs.GetInt ("CurrentPlayNum") != 0)
			m_PlayerData.GameData_Load();
			Application.LoadLevel ("00_MAIN");

			return;
#endif
			if(PlayerPrefs.GetInt ("CurrentPlayNum") != 0)
			m_PlayerData.GameData_Load();
			  
			Application.LoadLevel ("00_MAIN");
			return;
		}
		m_fTimer += Time.deltaTime;

	}
}
