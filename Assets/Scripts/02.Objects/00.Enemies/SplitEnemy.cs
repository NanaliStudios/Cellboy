using UnityEngine;
using System.Collections;

public class SplitEnemy : EnemyBase {

	public int m_iChildNum = 1;

	// Use this for initialization
	void Start () {
		Initialize ();
	}
	
	void FixedUpdate()
	{
		Progress();

		if(transform.position.y <= -5.8f)
			Destroy(gameObject);
		
		if(DeadCheck())	
		{
			Destroy(gameObject);
		}
	}
}
