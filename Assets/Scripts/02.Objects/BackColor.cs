using UnityEngine;
using System.Collections;

public class BackColor : MonoBehaviour {

	public Color[] BackColorArray = new Color[14];
	private TweenColor m_TweenColor = null;

	private Color m_PrevColor = Color.white;
	private Color m_FeverColor = Color.black;

	private GameSystem m_GameSys = null;

	// Use this for initialization
	void Start () {
		m_TweenColor = gameObject.GetComponent<TweenColor> ();

		//Set Background Color
		float RandColorR = Random.Range (0.0f, 1.0f);
		float RandColorG = Random.Range (0.0f, 1.0f);
		float RandColorB = Random.Range (0.0f, 1.0f);

		BackColorArray[0] = new Color (201.0f / 255.0f, 18.0f / 255.0f, 125.0f / 255.0f);
		BackColorArray[1] = new Color (209.0f / 255.0f, 82.0f / 255.0f, 27.0f / 255.0f);
		BackColorArray[2] = new Color (3.0f / 255.0f, 95.0f / 255.0f, 194.0f / 255.0f);
		BackColorArray[3] = new Color (255.0f / 255.0f, 77.0f / 255.0f, 107.0f / 255.0f);
		BackColorArray[4] = new Color (3.0f / 255.0f, 95.0f / 255.0f, 194.0f / 255.0f);
		BackColorArray[5] = new Color (200.0f / 255.0f, 18.0f / 255.0f, 18.0f / 255.0f);
		BackColorArray[6] = new Color (72.0f / 255.0f, 72.0f / 255.0f, 72.0f / 255.0f);
		BackColorArray[7] = new Color (115.0f / 255.0f, 27.0f / 255.0f, 151.0f / 255.0f);
		BackColorArray[8] = new Color (45.0f / 255.0f, 199.0f / 255.0f, 154.0f / 255.0f);
		BackColorArray[9] = new Color (121.0f / 255.0f, 167.0f / 255.0f, 49.0f / 255.0f);
		BackColorArray[10] = new Color (0.0f / 255.0f, 144.0f / 255.0f, 112.0f / 255.0f);
		BackColorArray[11] = new Color (165.0f / 255.0f, 60.0f / 255.0f, 190.0f / 255.0f);
		BackColorArray[12] = new Color (91.0f / 255.0f, 38.0f / 255.0f, 255.0f / 255.0f);
		BackColorArray[13] = new Color (112.0f / 255.0f, 84.0f / 255.0f, 255.0f / 246.0f);

		if (Application.loadedLevelName == "00_MAIN") {
		
			int iRandColorKey = Random.Range (0, 14);
			GameObject.Find("PlayerData(Clone)").gameObject.GetComponent<PlayerData>().m_BackColor = BackColorArray[iRandColorKey];
			gameObject.GetComponent<SpriteRenderer> ().color = BackColorArray[iRandColorKey];
		} 
		else {
			gameObject.GetComponent<SpriteRenderer> ().color = GameObject.Find("PlayerData(Clone)").gameObject.GetComponent<PlayerData>().m_BackColor;
			m_GameSys = GameSystem.GetInstance ();
		}




	}

	public void SetRandColor()
	{
		int iRandColorKey = Random.Range (0, 14);
		GameObject.Find("PlayerData(Clone)").gameObject.GetComponent<PlayerData>().m_BackColor = BackColorArray[iRandColorKey];
	
		m_TweenColor.from = gameObject.GetComponent<SpriteRenderer> ().color;
		m_TweenColor.to = BackColorArray[iRandColorKey];
		m_TweenColor.ResetToBeginning ();

		m_TweenColor.enabled = true;
	}

	public void SetFeverColor()
	{
		m_PrevColor = m_TweenColor.from = gameObject.GetComponent<SpriteRenderer> ().color;
		m_TweenColor.to = m_FeverColor;
		m_TweenColor.ResetToBeginning ();
		
		m_TweenColor.enabled = true;
	}

	public void SetPrevColor()
	{
		m_TweenColor.from = gameObject.GetComponent<SpriteRenderer> ().color;
		m_TweenColor.to = m_PrevColor;
		m_TweenColor.ResetToBeginning ();
		
		m_TweenColor.enabled = true;
	}
}
