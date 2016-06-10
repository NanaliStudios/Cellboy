using UnityEngine;
using System.Collections;

public class Point : Item {


	public int m_iAddPoint = 1;
	private AudioClip m_GetSound;

	// Use this for initialization
	void Start () {
		Initialize ();

		m_GetSound = Resources.Load ("Sounds/ogg(96k)/get_exp_01") as AudioClip;
	}
	
	// Update is called once per frame
	void Update () {

		Follow_Player ();
	

		if (m_GameSys.m_GameOver.activeSelf == true)
			Destroy (gameObject);

		if (m_bIsGet == true) {
			m_GameSys.m_iCurrent_Point +=  m_iAddPoint;
			Destroy(gameObject);
		}
	}


	void OnTriggerEnter2D(Collider2D Coll)
	{
		if (Coll.gameObject.tag == "Player") {

			m_Audio.clip = m_GetSound;
			m_Audio.Play();

			m_Skeleton.state.SetAnimation(0, "die", false);
		}
	}
}
