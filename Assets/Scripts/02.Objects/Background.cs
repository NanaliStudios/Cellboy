using UnityEngine;
using System.Collections;

public class Background : ObjectBase {

	public float m_fScrollSpeed = 0.01f;

	private Transform m_TransUpBack = null;
	private Transform m_TransDownBack = null;

	private float m_fBackOffset = 27.26f;

	private Player m_Player = null;
	// Use this for initialization
	void Start () {
		Initialize ();
	
		m_TransUpBack = transform.GetChild(0).transform;
		m_TransDownBack = transform.GetChild(1).transform;

		if (m_GameSys != null)
		m_Player = m_GameSys.m_objPlayer.GetComponent<Player> ();

	}

	void FixedUpdate()
	{
		if (m_GameSys != null) {
			if (m_GameSys.CheckGameStart () == false)
				return;

			m_TransDownBack.localPosition = new Vector3 (0.0f, m_TransDownBack.localPosition.y - (m_fScrollSpeed + m_GameSys.Get_GlobalSpeed()));
		}
		else
		m_TransDownBack.localPosition = new Vector3 (0.0f, m_TransDownBack.localPosition.y - (m_fScrollSpeed));

		m_TransUpBack.localPosition = new Vector3(0.0f, m_TransDownBack.localPosition.y + m_fBackOffset);

		if (m_TransDownBack.localPosition.y <= -28.26f) {
			m_TransDownBack.localPosition = new Vector3 (0.0f, 26.26f);
			m_fBackOffset = -m_fBackOffset;
		}
		if (m_TransUpBack.localPosition.y <= -28.26f) {
			m_TransUpBack.localPosition = new Vector3 (0.0f, 26.26f);
			m_fBackOffset = -m_fBackOffset;
		}

	}

//	void BackScroll(Transform m_TransBack)
//	{
//		if (m_TransBack.localPosition.y <= -39.64f)
//			m_TransBack.localPosition = new Vector3 (0.0f, 37.64f);
//	}

}
