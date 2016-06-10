using UnityEngine;
using System.Collections;

public class HighScoreTxt : TextBase {

	// Use this for initialization
	void Start () {
		
		Initialize ();
		
		m_MyText.text =  "Best " + PlayerPrefs.GetInt("HighScore").ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
		m_MyText.text =  "Best " + PlayerPrefs.GetInt("HighScore").ToString();
		
	}
}
