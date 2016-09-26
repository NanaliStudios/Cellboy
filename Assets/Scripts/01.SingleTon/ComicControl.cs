using UnityEngine;
using System.Collections;

public class ComicControl : MonoBehaviour {

	public GameObject[] m_objWindow = new GameObject[4];	
	public GameObject m_objPge2Wdw = null;
	public GameObject m_objPge1NextBtn = null;
	public GameObject m_objPge2NextBtn = null;

	public GameObject m_objPge1 = null;
	public GameObject m_objPge2 = null;




	public int m_iCurrentNum = 0;
	public int m_iCurrentPage = 1;

	void Start()
	{
		m_objWindow [m_iCurrentNum].gameObject.SetActive (true);
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0)) {

			if(m_iCurrentPage ==1)
			{
				m_objWindow [m_iCurrentNum].gameObject.GetComponent<TweenAlpha>().enabled = false;
				m_objWindow [m_iCurrentNum].gameObject.GetComponent<UISprite>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
				Change_CurrentNum();
			}
			else
			{
				m_objPge2Wdw.gameObject.GetComponent<TweenAlpha>().enabled = false;
				m_objPge2Wdw.gameObject.GetComponent<UISprite>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
				m_objPge2NextBtn.SetActive(true);
			}
		}
	}

	public void Change_CurrentNum()
	{
		if (m_iCurrentNum == 3) {
			//
			m_objPge1NextBtn.SetActive(true);
		}
		else
		{

		m_iCurrentNum += 1;
		m_objWindow [m_iCurrentNum].gameObject.SetActive (true);
		}
	}

	public void Pge2AlphaDone()
	{
		m_objPge2NextBtn.SetActive (true);
	}

	public void Page1End()
	{
		m_objPge1.SetActive (false);
		m_objPge2.SetActive (true);

		m_iCurrentPage += 1;
	}

	public void Page2End()
	{
		Application.LoadLevel ("00_Main");
	}
}
