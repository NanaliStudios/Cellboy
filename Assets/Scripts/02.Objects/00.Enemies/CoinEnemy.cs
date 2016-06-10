using UnityEngine;
using System.Collections;

public class CoinEnemy : EnemyBase {

	void Start () {
		base.Initialize ();
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
