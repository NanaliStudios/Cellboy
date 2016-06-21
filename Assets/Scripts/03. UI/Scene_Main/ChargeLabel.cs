using UnityEngine;
using System.Collections;

public class ChargeLabel : TextBase {
	
	private PlayerData m_PlayerData = null;
	public string strInfo = "";

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
		m_MyText.text = strInfo + " " + m_PlayerData.m_strPlayerName + "?";
	}
	

}
