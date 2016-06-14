using UnityEngine;
using System.Collections;
public class LogoScene : MonoBehaviour {

	private float m_fTimer = 0.0f;
	public float m_fTerm  = 2.5f;

	// Use this for initialization
	void Start () {

		GooglePlayConnection.Instance.Connect ();
		GoogleAdManager.Instance.ShowBanner ();
	
	}
	
	// Update is called once per frame
	void Update () {

		if(m_fTerm <= m_fTimer)
			Application.LoadLevel ("00_MAIN");

		m_fTimer += Time.deltaTime;

	}
}
