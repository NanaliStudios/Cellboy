using UnityEngine;
using System.Collections;

public class CountLabel : TextBase {

	public float m_fRemainTime = 10.0f;

	// Use this for initialization
	void Start () {
	
		Initialize ();

		int iTime = (int)m_fRemainTime;
		m_MyText.text = iTime.ToString();
	}

	void OnEnable()
	{
		m_fRemainTime = 10.0f;
	}
	
	// Update is called once per frame
	void Update() {
	
		if (m_fRemainTime <= 0)
			m_GameSys.OnClickNoContinue ();

		if(m_GameSys.m_bAdsOn == false)
		m_fRemainTime -= Time.unscaledDeltaTime;

		int iTime = (int)m_fRemainTime;
		m_MyText.text = iTime.ToString();
	}
}
