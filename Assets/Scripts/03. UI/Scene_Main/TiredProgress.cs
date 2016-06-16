using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TiredProgress : MonoBehaviour {

	private UISlider m_Slider = null;
	private PLAYER_ID m_CurrentPlayerID = PLAYER_ID.NORMAL;
	private float m_fCurrentTired = 0.0f;
	private PlayerData m_PlayerData = null;

	bool m_bInit = false;

	// Use this for initialization
	void Start () {
	
		m_Slider = gameObject.GetComponent<UISlider> ();
	}
	
	// Update is called once per frame
	void Update () {


		if (m_bInit == false) {
			m_PlayerData = GameObject.Find ("PlayerData(Clone)").GetComponent<PlayerData>();
			m_bInit = true;
		}

		m_CurrentPlayerID = m_PlayerData.m_PlayerID;
		//
		m_fCurrentTired = m_PlayerData.m_Gamedata.m_PlayerInfo[(int)m_CurrentPlayerID].fTiredPercent;
		m_Slider.value = m_fCurrentTired / 100.0f;
	
	}
}
