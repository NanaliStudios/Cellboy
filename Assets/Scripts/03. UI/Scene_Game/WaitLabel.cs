using UnityEngine;
using System.Collections;

public class WaitLabel : TextBase {


	void Start () {
		
		Initialize ();

		int iVal = (3 - (int)m_GameSys.m_fResumeTimer);
		m_MyText.text = iVal.ToString();
	}

	// Update is called once per frame
	void Update () {
		int iVal = (3 - (int)m_GameSys.m_fResumeTimer);
		m_MyText.text = iVal.ToString();
	}
}
