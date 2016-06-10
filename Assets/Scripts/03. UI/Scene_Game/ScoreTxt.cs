using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreTxt : TextBase {
	
	// Use this for initialization
	void Start () {

		Initialize ();
	
		m_MyText.text =  m_GameSys.m_iCurrent_GameScore.ToString();
	}
	
	// Update is called once per frame
	void Update () {

		m_MyText.text =  m_GameSys.m_iCurrent_GameScore.ToString();
	
	}
}
