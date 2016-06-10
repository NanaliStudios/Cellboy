using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextBase : MonoBehaviour {

	protected UILabel m_MyText = null;
	protected GameSystem m_GameSys = null;

	protected void Initialize()
	{
		m_MyText = gameObject.GetComponent<UILabel> ();

		if (Application.loadedLevelName == "02_Game")
		m_GameSys = GameSystem.GetInstance ();
	}
}
