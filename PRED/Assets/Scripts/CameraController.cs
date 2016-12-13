using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float h_speed = 2.0f;
	public float v_speed = 2.0f;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float h = h_speed * Input.GetAxis ("Mouse X");
		float v = v_speed * Input.GetAxis ("Mouse Y");

		transform.Rotate (v, h, 0);
	}
}
