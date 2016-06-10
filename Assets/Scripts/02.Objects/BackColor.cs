using UnityEngine;
using System.Collections;

public class BackColor : MonoBehaviour {

	// Use this for initialization
	void Start () {

		//Set Background Color
		float RandColorR = Random.Range (0.0f, 1.0f);
		float RandColorG = Random.Range (0.0f, 1.0f);
		float RandColorB = Random.Range (0.0f, 1.0f);

		int iRandColorKey = Random.Range (0, 14);
		Color BackColor = Color.white;

		if (Application.loadedLevelName == "00_MAIN") {
			switch (iRandColorKey) {
			case 0:
				BackColor = new Color (201.0f / 255.0f, 18.0f / 255.0f, 125.0f / 255.0f);
				break;
			case 1:
				BackColor = new Color (209.0f / 255.0f, 82.0f / 255.0f, 27.0f / 255.0f);
				break;
			case 2:
				BackColor = new Color (3.0f / 255.0f, 95.0f / 255.0f, 194.0f / 255.0f);
				break;
			case 3:
				BackColor = new Color (255.0f / 255.0f, 77.0f / 255.0f, 107.0f / 255.0f);
				break;
			case 4:
				BackColor = new Color (3.0f / 255.0f, 95.0f / 255.0f, 194.0f / 255.0f);
				break;
			case 5:
				BackColor = new Color (200.0f / 255.0f, 18.0f / 255.0f, 18.0f / 255.0f);
				break;
			case 6:
				BackColor = new Color (72.0f / 255.0f, 72.0f / 255.0f, 72.0f / 255.0f);
				break;
			case 7:
				BackColor = new Color (115.0f / 255.0f, 27.0f / 255.0f, 151.0f / 255.0f);
				break;
			case 8:
				BackColor = new Color (45.0f / 255.0f, 199.0f / 255.0f, 154.0f / 255.0f);
				break;
			case 9:
				BackColor = new Color (121.0f / 255.0f, 167.0f / 255.0f, 49.0f / 255.0f);
				break;
			case 10:
				BackColor = new Color (0.0f / 255.0f, 144.0f / 255.0f, 112.0f / 255.0f);
				break;
			case 11:
				BackColor = new Color (165.0f / 255.0f, 60.0f / 255.0f, 190.0f / 255.0f);
				break;
			case 12:
				BackColor = new Color (91.0f / 255.0f, 38.0f / 255.0f, 255.0f / 255.0f);
				break;
			case 13:
				BackColor = new Color (112.0f / 255.0f, 84.0f / 255.0f, 255.0f / 246.0f);
				break;
			}
			GameObject.Find("PlayerData(Clone)").gameObject.GetComponent<PlayerData>().m_BackColor = BackColor;

			gameObject.GetComponent<SpriteRenderer> ().color = BackColor;
		} 
		else {
			gameObject.GetComponent<SpriteRenderer> ().color = GameObject.Find("PlayerData(Clone)").gameObject.GetComponent<PlayerData>().m_BackColor;
		}




	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
