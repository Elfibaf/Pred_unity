using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform playerTransform;
	public int depth = 0;

	// Update is called once per frame
	void Update()
	{
		if(playerTransform != null)
		{
			transform.position = playerTransform.position + new Vector3(0,0.5f,depth);
			transform.rotation = playerTransform.rotation;
		}
	}

	public void setTarget(Transform target)
	{
		playerTransform = target;
	}
}
