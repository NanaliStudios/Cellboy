using UnityEngine;
using System.Collections;
public class LogoScene : MonoBehaviour {

	private float m_fTimer = 0.0f;
	public float m_fTerm  = 2.5f;

	void Awake()
	{
		if ((Screen.width / 3) * 4 == Screen.height)
			Camera.main.orthographicSize = 4.5f;
	}

	// Use this for initialization
	void Start () {

		AdFuctions.Initialize ();
		GameSDK_Funcs.Initialize ();
	
	}
	
	// Update is called once per frame
	void Update () {

			if (m_fTerm <= m_fTimer)
			{
#if UNITY_EDITOR_OSX
			Application.LoadLevel ("00_MAIN");
			return;
#endif

				GameSDK_Funcs.Show_CloudSaveUI();
				AdFuctions.Show_GoogleAD();
				Application.LoadLevel ("00_MAIN");
			}

		m_fTimer += Time.deltaTime;

	}
}
