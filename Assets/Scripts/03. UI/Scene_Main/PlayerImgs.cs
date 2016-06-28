using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerImgs : MonoBehaviour {


	public int m_iDist = 100;
	
	private Transform[] m_TransChilds = null;

	public GameObject MyCanvas = null;

	void Start()
	{

		StartCoroutine ("SwipeMove");
	} 

	void FixedUpdate()
	{

	}

	public IEnumerator SwipeMove()
	{

		Vector2 ClickMousePos = Vector2.zero;
		Vector2 CurMousePos = Vector2.zero;
		
		float fPressTime = 0f;
		float fSpeed = 0f;
		bool bSwipeStart = false;
		Transform CamTrans = Camera.main.transform;
		
		
		do{
			CurMousePos = Vector2.zero;
				if(Input.GetMouseButtonDown(0))
				{
					ClickMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					fPressTime = 0f;
					bSwipeStart = true;
				}else if(bSwipeStart && Input.GetMouseButton(0))
				{
				//Debug.Log("Clicking");
					CurMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					fPressTime += Time.deltaTime;
					
					fSpeed = Vector2.Distance(new Vector2(CurMousePos.x,0) , new Vector2(ClickMousePos.x,0))  * 10f * Time.deltaTime;
				}
				
				fSpeed -= Time.deltaTime * 0.5f;
				if(fSpeed < 0f)
					fSpeed = 0f;
				
				if(CurMousePos.x > ClickMousePos.x)
			{
				//Debug.Log("CurMousePosX" + CurMousePos.x + "ClickMousePosX" + ClickMousePos.x);
				transform.position = new Vector3(transform.position.x + fSpeed, transform.position.y, -10f);
			}
				else if(CurMousePos.x < ClickMousePos.x)
				transform.position = new Vector3(transform.position.x - fSpeed, transform.position.y, -10f);
			
			yield return null;
			
			
		}while(true);

		
	}

}
