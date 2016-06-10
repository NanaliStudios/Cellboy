using UnityEngine;
using System.Collections;

public class MainLabel : TextBase {

	PlayerData m_PlayerData = null;
	public string m_strStartText = "";
	public string m_strChargeText = "";
	public string m_strLockText = "";
	public string m_strBuy = "";

	private UISprite m_BtnUISprite = null;
	public GameObject m_objCoinIcon = null;
	public GameObject m_objLockIcon = null;

	private bool m_bInit = false;
	private int m_iCurrentLock = 0;

	// Use this for initialization
	void Start () {
	
		Initialize ();

		m_BtnUISprite = gameObject.transform.parent.gameObject.GetComponent<UISprite> ();
		MakeStartText();
	}
	
	// Update is called once per frame
	void LateUpdate () {

		if (m_bInit == false) {
			m_PlayerData = GameObject.Find ("PlayerData(Clone)").GetComponent<PlayerData>();
			m_bInit = true;
		}
	
		//TiredVal Change----->
		if (m_PlayerData.m_PlayerID == PLAYER_ID.NORMAL) {

			m_iCurrentLock = PlayerPrefs.GetInt ("Player0Lock");
			
			if (PlayerPrefs.GetFloat ("fPlayer0Tired") <= 0.0f) {
				MakeChargeText ();
			} else 
				MakeStartText ();
		} else if (m_PlayerData.m_PlayerID == PLAYER_ID.SPREAD) {

			m_iCurrentLock = PlayerPrefs.GetInt ("Player1Lock");
			
			if (PlayerPrefs.GetFloat ("fPlayer1Tired") <= 0.0f) {
				MakeChargeText ();
			} else 
				MakeStartText ();
		} else if (m_PlayerData.m_PlayerID == PLAYER_ID.LASER) {

			m_iCurrentLock = PlayerPrefs.GetInt ("Player2Lock");

			if (PlayerPrefs.GetInt ("Player1Lock") == 1)
			{
				MakeLockText ();
				return;
			}
			else {
				if (PlayerPrefs.GetFloat ("fPlayer2Tired") <= 0.0f) 
					MakeChargeText ();
				else
					MakeStartText ();
			}
		}
		else if (m_PlayerData.m_PlayerID == PLAYER_ID.HOMING) {

			m_iCurrentLock = PlayerPrefs.GetInt("Player3Lock");

			if (PlayerPrefs.GetInt ("Player2Lock") == 1)
			{
				MakeLockText ();
				return;
			}
			else {
			
			if (PlayerPrefs.GetFloat ("fPlayer3Tired") <= 0.0f) {
				MakeChargeText();
			}
			else 
				MakeStartText();
			}

		} else if (m_PlayerData.m_PlayerID == PLAYER_ID.BOOM) {

			m_iCurrentLock = PlayerPrefs.GetInt("Player4Lock");


			if (PlayerPrefs.GetInt ("Player3Lock") == 1)
			{
				MakeLockText ();
				return;
			}
			else {

			if (PlayerPrefs.GetFloat ("fPlayer4Tired") <= 0.0f) {
				MakeChargeText();
			}
			else 
				MakeStartText();
			}

		} 
		//<-----End

		if (m_iCurrentLock == 1) {
			m_strBuy = m_PlayerData.m_iBuyPrice.ToString();
			MakeBuyText ();
		}


	}

	void MakeStartText()
	{
		m_MyText.text = m_strStartText;
		m_MyText.color = Color.white;

		m_BtnUISprite.enabled = true;
		m_objCoinIcon.SetActive (false);
		m_objLockIcon.SetActive (false);
	}

	void MakeChargeText()
	{
		m_MyText.text = m_strChargeText;
		m_MyText.color = Color.yellow;
	
		m_BtnUISprite.enabled = true;
		m_objCoinIcon.SetActive (false);
		m_objLockIcon.SetActive (false);
	}

	void MakeBuyText()
	{
		m_MyText.text = m_strBuy;
		m_MyText.color = Color.yellow;

		m_BtnUISprite.enabled = true;
		m_objCoinIcon.SetActive (true);
		m_objLockIcon.SetActive (false);
	}

	void MakeLockText()
	{
		m_MyText.text = m_strLockText;
		m_MyText.color = Color.black;

		m_BtnUISprite.enabled = false;
		m_objCoinIcon.SetActive (false);
		m_objLockIcon.SetActive (true);
	}
}
