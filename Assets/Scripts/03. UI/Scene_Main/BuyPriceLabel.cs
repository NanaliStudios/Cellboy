using UnityEngine;
using System.Collections;

public class BuyPriceLabel : TextBase {

	private PlayerData m_PlayerData = null;
	
	void OnEnable()
	{
		Initialize ();
		m_PlayerData = GameObject.Find ("PlayerData(Clone)").GetComponent<PlayerData>();
		m_MyText.text = m_PlayerData.m_iBuyPrice.ToString();
	}

}
