using UnityEngine;
using System.Collections;
using TapjoyUnity;

public class Coin : Item {

	public int m_iAddCoin = 1;

	private AudioClip[] m_GetSound = new AudioClip[3];
	
	// Use this for initialization
	void Start () {
		Initialize ();

		for(int i = 0; i < 3; ++i)
			m_GetSound[i] = Resources.Load ("Sounds/ogg(96k)/" + string.Format("get_coin_0{0}", i+1)) as AudioClip;
	}
	
	// Update is called once per frame
	void Update () {
		
		Follow_Player ();

		if (m_GameSys.m_GameOver.activeSelf == true)
			Destroy (gameObject);

		if (m_bIsGet == true) {
			if (Tapjoy.IsConnected)
				Tapjoy.AwardCurrency (m_iAddCoin);
			else
			m_GameSys.m_PlayerData.m_Gamedata.m_iHaveCoin += m_iAddCoin;

			Destroy(gameObject);
		}
	}
	
	
	void OnTriggerEnter2D(Collider2D Coll)
	{
		if (Coll.gameObject.tag == "Player") {

			//Play Rand Sound
			int iRandVal = Random.Range(0, 3);
			m_Audio.clip = m_GetSound[iRandVal];
			m_Audio.Play();

			m_Skeleton.state.SetAnimation(0, "die", false);
		}
	}
}
