using UnityEngine;
using System.Collections;

public class HighScoreTxt : TextBase {

	private PlayerData m_PlayerData = null;

	void Awake()
	{
		m_PlayerData = GameObject.Find ("PlayerData(Clone)").GetComponent<PlayerData>();
	}
	void Start () {
		
		Initialize ();
		
		m_MyText.text =  "Best " + m_PlayerData.m_Gamedata.m_iHighScore.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
		m_MyText.text =  "Best " + m_PlayerData.m_Gamedata.m_iHighScore.ToString();
		
	}
}
