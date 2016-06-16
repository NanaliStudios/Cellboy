using UnityEngine;
using System.Collections;

public class CoinTxt : TextBase {

	private PlayerData m_PlayerData = null;
	private bool m_bInit = false;

	// Use this for initialization
	void Start () {
		
		Initialize ();
	}
	
	// Update is called once per frame
	void Update () {
		if (m_bInit == false) {
			m_PlayerData = GameObject.Find ("PlayerData(Clone)").GetComponent<PlayerData>();
			m_bInit = true;
		}
		
		m_MyText.text = m_PlayerData.m_Gamedata.m_iHaveCoin.ToString();
		
	}
}
