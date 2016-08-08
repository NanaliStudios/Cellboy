using UnityEngine;
using System.Collections;

public class MyTweenAlpha : MonoBehaviour {
	
	private UISprite m_UISprite = null;

	// Use this for initialization
	void Start () {

		m_UISprite = gameObject.GetComponent<UISprite> ();
	
	}
	
	// Update is called once per frame
	void Update () {

	}
}
