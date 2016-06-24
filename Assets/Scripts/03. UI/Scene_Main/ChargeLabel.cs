using UnityEngine;
using System.Collections;

public class ChargeLabel : TextBase {
	
	private PlayerData m_PlayerData = null;
	public string strKEY = "";

	// Use this for initialization
//	void Start () {
//		Initialize ();
//
//		m_PlayerData = GameObject.Find ("PlayerData(Clone)").GetComponent<PlayerData>();
//		m_MyText.text = m_PlayerData.m_strPlayerName + strInfo;
//	}

	void OnEnable()
	{
		Initialize ();
		m_PlayerData = GameObject.Find ("PlayerData(Clone)").GetComponent<PlayerData>();

		string str = Localization.Get (strKEY);
		if(Localization.language == "English")
			m_MyText.text = str + " " + m_PlayerData.m_strPlayerName + "?";
		else
			m_MyText.text = m_PlayerData.m_strPlayerName + " " + str + "?";
	}
	

}
