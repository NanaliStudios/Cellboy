using UnityEngine;
using System.Collections;

public class CurrentScoreTxt : TextBase {

	public PlayerData m_PlayerData = null;

	// Use this for initialization
	void Start () {
		
		Initialize ();

		m_PlayerData =  GameObject.Find ("PlayerData(Clone)").GetComponent<PlayerData> ();
		
		m_MyText.text =  m_PlayerData.m_iCurrentScore.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
		m_MyText.text =  m_PlayerData.m_iCurrentScore.ToString();
		
	}
}
