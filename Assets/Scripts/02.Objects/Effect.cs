using UnityEngine;
using System.Collections;

public class Effect : ObjectBase {

	bool m_bDelete = false;

	// Use this for initialization
	void Start () {

		if (m_Skeleton != null) {
			m_Skeleton.state.Complete += delegate {

				m_bDelete = true;
	
			};
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_bDelete)
		Destroy(gameObject);
	}
}
