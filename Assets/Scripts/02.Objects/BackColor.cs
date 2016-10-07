using UnityEngine;
using System.Collections;

public class BackColor : MonoBehaviour {

	public Color[] BackColorArray = new Color[14];
	private TweenColor m_TweenColor = null;

	private Color m_PrevColor = Color.white;
	private Color m_FeverColor = Color.black;

	private GameSystem m_GameSys = null;

	public int m_CurrentColorKey = 0;

	public GameObject m_objSetBackParticle = null;
	private SetBackParticle m_SetBackParti = null;

	// Use this for initialization
	void Start () {
		m_TweenColor = gameObject.GetComponent<TweenColor> ();
		m_SetBackParti = m_objSetBackParticle.GetComponent<SetBackParticle> ();

		//Set Background Color
		float RandColorR = Random.Range (0.0f, 1.0f);
		float RandColorG = Random.Range (0.0f, 1.0f);
		float RandColorB = Random.Range (0.0f, 1.0f);

		BackColorArray[0] = new Color (201.0f / 255.0f, 18.0f / 255.0f, 125.0f / 255.0f);
		BackColorArray[1] = new Color (235.0f / 255.0f, 71.0f / 255.0f,0.0f / 255.0f);
		BackColorArray[2] = new Color (0.0f / 255.0f, 143.0f / 255.0f, 246.0f / 255.0f);
		BackColorArray[3] = new Color (255.0f / 255.0f, 77.0f / 255.0f, 107.0f / 255.0f);
		BackColorArray[4] = new Color (3.0f / 255.0f, 95.0f / 255.0f, 194.0f / 255.0f);
		BackColorArray[5] = new Color (200.0f / 255.0f, 18.0f / 255.0f, 18.0f / 255.0f);
		BackColorArray[6] = new Color (72.0f / 255.0f, 72.0f / 255.0f, 72.0f / 255.0f);
		BackColorArray[7] = new Color (115.0f / 255.0f, 27.0f / 255.0f, 151.0f / 255.0f);
		BackColorArray[8] = new Color (0.0f / 255.0f, 144.0f / 255.0f, 112.0f / 255.0f);
		BackColorArray[9] = new Color (165.0f / 255.0f, 60.0f / 255.0f, 190.0f / 255.0f);
		BackColorArray[10] = new Color (91.0f / 255.0f, 38.0f / 255.0f, 255.0f / 255.0f);

		if (Application.loadedLevelName == "00_MAIN") {

			SetRandColorKey();

			GameObject.Find("PlayerData(Clone)").gameObject.GetComponent<PlayerData>().m_BackColor = BackColorArray[m_CurrentColorKey];
			gameObject.GetComponent<SpriteRenderer> ().color = BackColorArray[m_CurrentColorKey];
		} 
		else {
			gameObject.GetComponent<SpriteRenderer> ().color = GameObject.Find("PlayerData(Clone)").gameObject.GetComponent<PlayerData>().m_BackColor;
			m_GameSys = GameSystem.GetInstance ();
		}




	}

	public void SetRandColor()
	{
		SetRandColorKey();
		GameObject.Find("PlayerData(Clone)").gameObject.GetComponent<PlayerData>().m_BackColor = BackColorArray[m_CurrentColorKey];
	
		m_TweenColor.from = gameObject.GetComponent<SpriteRenderer> ().color;
		m_TweenColor.to = BackColorArray[m_CurrentColorKey];
		m_TweenColor.ResetToBeginning ();

		m_TweenColor.enabled = true;
	}

	public void SetFeverColor()
	{
		m_PrevColor = m_TweenColor.from = gameObject.GetComponent<SpriteRenderer> ().color;
		m_TweenColor.to = m_FeverColor;
		m_TweenColor.duration = 1.5f;
		m_TweenColor.ResetToBeginning ();
		
		m_TweenColor.enabled = true;
	}

	public void SetPrevColor()
	{
		m_TweenColor.from = gameObject.GetComponent<SpriteRenderer> ().color;
		m_TweenColor.to = m_PrevColor;
		m_TweenColor.duration = 2.5f;
		m_TweenColor.ResetToBeginning ();
		
		m_TweenColor.enabled = true;
	}

	public void SetRandColorKey()
	{
		if(m_SetBackParti.m_iBigIdx == 0
		   || m_SetBackParti.m_iBigIdx == 1)
		{
			m_CurrentColorKey = Random.Range (0, 11);
			
			if(m_CurrentColorKey == 1)
				m_CurrentColorKey = 0;
			else if(m_CurrentColorKey == 2)
				m_CurrentColorKey = 3;
		}
		else if(m_SetBackParti.m_iBigIdx == 2)
			m_CurrentColorKey = Random.Range (0, 11);
		else if(m_SetBackParti.m_iBigIdx == 3)
		{
			m_CurrentColorKey = Random.Range (0, 11);
			
			if(m_CurrentColorKey == 8)
				m_CurrentColorKey = 7;
		}
		else if(m_SetBackParti.m_iBigIdx == 4)
			m_CurrentColorKey = Random.Range (0, 11);
		else
			m_CurrentColorKey = Random.Range (0, 11);
	}
}
