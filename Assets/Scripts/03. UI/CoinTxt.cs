using UnityEngine;
using System.Collections;

public class CoinTxt : TextBase {

	// Use this for initialization
	void Start () {
		
		Initialize ();
		
		m_MyText.text =  PlayerPrefs.GetInt("HaveCoin").ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
		m_MyText.text =  PlayerPrefs.GetInt("HaveCoin").ToString();
		
	}
}
