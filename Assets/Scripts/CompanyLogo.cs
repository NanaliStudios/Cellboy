using UnityEngine;
using System.Collections;

public class CompanyLogo : MonoBehaviour {
	
	private UISprite m_UISprite = null;
	public float m_fAlphaTime = 0.1f;
	public float m_fAlphaVal = 0.1f;

	// Use this for initialization
	void Start () {

		m_UISprite = gameObject.GetComponent<UISprite> ();
		StartCoroutine ("TweenOn");
	
	}
	
	IEnumerator TweenOn()
	{
		while(true)
		{
			yield return new WaitForSeconds(m_fAlphaTime);
			m_UISprite.alpha += m_fAlphaVal;
		}
	}
}
