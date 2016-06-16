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
	private bool m_bCurrentLock = false;

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

		for (int i = 0; i < m_PlayerData.m_Gamedata.m_iTotalPlayerNum; ++i) {

			m_bCurrentLock = m_PlayerData.m_Gamedata.m_PlayerInfo [i].bIsLock;

			if (m_bCurrentLock == true) {
				m_strBuy = m_PlayerData.m_iBuyPrice.ToString();
				MakeBuyText ();
				return;
			}

			if (m_PlayerData.m_Gamedata.m_PlayerInfo [i].fTiredPercent <= 0.0f)
				MakeChargeText ();
			else
				MakeStartText ();
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
