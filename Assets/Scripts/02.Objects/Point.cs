﻿using UnityEngine;
using System.Collections;

public class Point : Item {


	public int m_iAddPoint = 1;
	private AudioClip[] m_GetSound = new AudioClip[3];

	// Use this for initialization
	void Start () {
		Initialize ();

		for(int i = 0; i < 3; ++i)
			m_GetSound[i] = Resources.Load ("Sounds/" + string.Format("get_ex_0{0}", i+1)) as AudioClip;
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

			int iRandVal = Random.Range(0, 3);
			m_Audio.clip = m_GetSound[iRandVal];
			m_Audio.Play();

			m_Skeleton.state.SetAnimation(0, "die", false);
		}
	}
}
