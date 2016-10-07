using UnityEngine;
using System.Collections;

public class SetBackParticle : MonoBehaviour {

	public GameObject[]	m_objBackPartis = new GameObject[5];
	public GameObject[]	m_objBackSmallPartis = new GameObject[5];

	private ParticleSystem m_BigParticle = null;
	private ParticleSystem m_SmallParticle = null;

	public int m_iBigIdx = 0;



	public bool m_bInit = false;

	// Use this for initialization
	void Start () {

		DontDestroyOnLoad (this);
	}

	void FixedUpdate()
	{
		if (m_bInit == false) {
			if (Application.loadedLevelName == "00_MAIN") {
				for (int i = 0; i < transform.childCount; ++i)
					Destroy (transform.GetChild (i).gameObject);

				m_iBigIdx = Random.Range (0, 5);
				int iSmallIdx = Random.Range (0, 5);
				
				GameObject objParti = GameObject.Instantiate (m_objBackPartis [m_iBigIdx]) as GameObject;
				objParti.transform.parent = gameObject.transform;
				objParti.transform.localPosition = new Vector3 (0.0f, 0.0f);
				m_BigParticle = objParti.GetComponent<ParticleSystem> ();
				
				objParti = GameObject.Instantiate (m_objBackSmallPartis [iSmallIdx]) as GameObject;
				objParti.transform.parent = gameObject.transform;
				objParti.transform.localPosition = new Vector3 (0.0f, 0.0f);
				m_SmallParticle = objParti.GetComponent<ParticleSystem> ();

				m_bInit = true;
			}

		} else {


			if(Application.loadedLevelName == "02_Game")
			{
				m_BigParticle.playbackSpeed = 1 + GameSystem.GetInstance().Get_GlobalSpeed() * 10;
				m_SmallParticle.playbackSpeed = 1 + GameSystem.GetInstance().Get_GlobalSpeed() * 10;
			}
		}

		if (Application.loadedLevelName == "00_Logo")
			Destroy (gameObject);

	}

}
