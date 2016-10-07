using UnityEngine;
using System.Collections;
using TapjoyUnity;
public class LogoScene : MonoBehaviour {

	private float m_fTimer = 0.0f;
	public float m_fTerm  = 3.0f;

	public GameObject m_objPlayerData = null;
	public GameObject m_objSDKMgr = null;
	private PlayerData m_PlayerData = null;
	private GameSDKManager m_SdkMgr = null;

	public bool m_bLateInit = false;

	string m_strNextSceneName = "00_Main";

	void Awake()
	{
		if ((Screen.width / 3) * 4 == Screen.height)
			Camera.main.orthographicSize = 4.5f;

		if (Application.systemLanguage == SystemLanguage.Korean)
			Localization.language = "한국어";
		else
			Localization.language = "English";
	}

	// Use this for initialization
	void Start () {

		#if UNITY_EDITOR_OSX
		PlayerPrefs.DeleteAll();
		#endif

		if (PlayerPrefs.HasKey ("CurrentPlayNum") == false) {
			PlayerPrefs.SetInt ("CurrentPlayNum", 0);
			PlayerPrefs.SetInt ("Adoff", 0);

		}

		if(PlayerPrefs.GetInt("CurrentPlayNum").Equals(0))
			m_strNextSceneName = "Comic";
		else
			m_strNextSceneName = "00_Main";
		
		if (PlayerPrefs.HasKey ("GameVolume") == false) {
			PlayerPrefs.SetFloat ("GameVolume", 3.0f);
		} else {
			if(PlayerPrefs.GetFloat("GameVolume") == 0.0f)
				AudioListener.volume = 0.0f;
		}

		if (GameObject.Find ("GameSDKManager(Clone)") == null) {
			GameObject objSdkMgr = Instantiate (m_objSDKMgr) as GameObject;
			m_SdkMgr = objSdkMgr.GetComponent<GameSDKManager> ();
		} else
			m_SdkMgr = GameObject.Find ("GameSDKManager(Clone)").GetComponent<GameSDKManager>();

		if (GameObject.Find ("PlayerData(Clone)") == null) {
			GameObject objPlayerData = Instantiate (m_objPlayerData) as GameObject;
			m_PlayerData = objPlayerData.GetComponent<PlayerData> ();
			//Debug.Log ("Create PlayerData");
		} else
			m_PlayerData = GameObject.Find ("PlayerData(Clone)").GetComponent<PlayerData>();

		if(!m_SdkMgr.isInitialized())
			m_SdkMgr.Initialize ();



		if(!AdFunctions.isInitialized())
		AdFunctions.Initialize ();
	
	}

	// Update is called once per frame
	void Update () {

			if (m_bLateInit == false) {
				//if(PlayerPrefs.GetInt("CurrentPlayNum") != 0)

				Debug.Log("PlayerPrefs.GetInt(CurrentPlayNum):" + PlayerPrefs.GetInt("CurrentPlayNum"));

#if UNITY_IOS
				if(PlayerPrefs.GetInt("CurrentPlayNum") == 0)
					m_SdkMgr.Do_CloudLoad ();
				else
					m_SdkMgr.m_bIsLoadedData = true;
#elif UNITY_ANDROID
				if(PlayerPrefs.GetInt("CurrentPlayNum") != 0)
					m_SdkMgr.m_bIsLoadedData = true;
#endif



				m_bLateInit = true;
		}

			if (m_fTerm <= m_fTimer) {
#if UNITY_EDITOR_OSX || UNITY_IOS
			m_PlayerData.GameData_Load();
			Application.LoadLevel (m_strNextSceneName);

			return;
#endif

			if(Application.internetReachability == NetworkReachability.NotReachable)
			{
				m_PlayerData.GameData_Load();
				Application.LoadLevel (m_strNextSceneName);
			}
			else
			{
				if(m_SdkMgr.m_bIsLoadedData == true)
				{
					m_PlayerData.GameData_Load();
					Application.LoadLevel (m_strNextSceneName);
				}
			}
			return;
		}
		m_fTimer += Time.deltaTime;

	}
}
