using UnityEngine;
using System.Collections;

public class SetCenterScale : MonoBehaviour {
	
	private UI_Playerimg m_CenterScript = null;
	private bool m_bIsDrag = false;

	// Use this for initialization

	void FixedUpdate()
	{
		m_CenterScript = gameObject.GetComponent<UICenterOnChild> ().centeredObject.GetComponent<UI_Playerimg>();
		m_CenterScript.m_bSelected = true;

	
		if (gameObject.GetComponent<UIScrollView> ().isDragging)
			m_bIsDrag = true;
		else 
		{
			if(m_bIsDrag == true)
			{
			AudioSource Audio = gameObject.GetComponent<AudioSource>();
			if(!Audio.isPlaying)
				Audio.Play();

				m_bIsDrag = false;
			}
		}

	}

}
