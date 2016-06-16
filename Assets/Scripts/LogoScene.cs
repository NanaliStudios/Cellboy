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


		if(!PlayerPrefs.HasKey("ADS_Key"))
			PlayerPrefs.SetInt ("ADS_Key", 0);

		AdFuctions.Initialize ();
		GameSDK_Funcs.Initialize ();

	
	}
	
	// Update is called once per frame
	void Update () {

			if (m_fTerm <= m_fTimer)
			{
				if(Application.internetReachability == NetworkReachability.NotReachable)
					Application.LoadLevel ("00_MAIN");
				else
				{

					if(AdFuctions.isInitialized ()
				   	&& GameSDK_Funcs.isInitialized ()) 
					{
						AdFuctions.Show_GoogleAD();
						Application.LoadLevel ("00_MAIN");
					}
				}
			}

		m_fTimer += Time.deltaTime;

	}
}
