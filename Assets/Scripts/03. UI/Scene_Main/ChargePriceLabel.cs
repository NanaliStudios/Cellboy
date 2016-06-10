using UnityEngine;
using System.Collections;

public class ChargePriceLabel : TextBase {

	private PlayerData m_PlayerData = null;
	
	// Use this for initialization
	void Start () {
		Initialize ();
		
		m_PlayerData = GameObject.Find ("PlayerData(Clone)").GetComponent<PlayerData>();
		m_MyText.text = m_PlayerData.m_iChargePrice.ToString();
	}

}
