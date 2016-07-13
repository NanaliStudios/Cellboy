using UnityEngine;
using System.Collections;

public class MainLabel : TextBase {

	PlayerData m_PlayerData = null;
	public string m_strBuy = "";

	private UISprite m_BtnUISprite = null;
	public GameObject m_objCoinIcon = null;
	public GameObject m_objLockIcon = null;

	private bool m_bInit = false;
	private bool m_bCurrentLock = false;

	private UILocalize m_MyLocalize = null;

	// Use this for initialization
	void Start () {
	
		Initialize ();

		m_BtnUISprite = gameObject.transform.parent.gameObject.GetComponent<UISprite> ();
		m_MyLocalize = gameObject.GetComponent<UILocalize> ();
		MakeStartText();
	}
	
	// Update is called once per frame
	void LateUpdate () {

		if (m_bInit == false) {
			m_PlayerData = GameObject.Find ("PlayerData(Clone)").GetComponent<PlayerData>();
			m_bInit = true;
		}
	
		//TiredVal Change----->

			m_bCurrentLock = m_PlayerData.m_Gamedata.m_PlayerInfo [(int)m_PlayerData.m_PlayerID].bIsLock;

			if (m_bCurrentLock == true) {

			int iPlayerIdx = ((int)m_PlayerData.m_PlayerID) -1;

			if(iPlayerIdx > 0)
			{

				if(m_PlayerData.m_Gamedata.m_PlayerInfo [((int)m_PlayerData.m_PlayerID) -1].bIsLock == true)
				{
					MakeLockText();
					return;
				}
			}

			m_strBuy = m_PlayerData.m_iBuyPrice.ToString ();
			MakeBuyText ();
			return;
		}

		if (m_PlayerData.m_Gamedata.m_PlayerInfo [(int)m_PlayerData.m_PlayerID].fTiredPercent <= 0.0f)
				MakeChargeText ();
			else
				MakeStartText ();
	}

	void MakeStartText()
	{
		//Localization.language = "English"
		m_MyText.text = Localization.Get ("START");
		m_MyText.color = Color.white;

		m_BtnUISprite.enabled = true;
		m_objCoinIcon.SetActive (false);
		m_objLockIcon.SetActive (false);
	}

	void MakeChargeText()
	{
		m_MyText.text = Localization.Get ("RECHARGE");
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
		m_MyText.text = "";
		m_MyText.color = Color.black;

		m_BtnUISprite.enabled = false;
		m_objCoinIcon.SetActive (false);
		m_objLockIcon.SetActive (true);
	}
}
