﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TherapistController : NetworkBehaviour {

	public float h_speed = 2.0f;
	public float v_speed = 2.0f;

	private float yaw = -100.5f;
	private float pitch = 0.0f;
    private bool patientDetected = false;

	// Update is called once per frame
	void Update () {

		if (!isLocalPlayer) 
		{
			return;
		}

		var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 150.0f;

		yaw += h_speed * Input.GetAxis("Mouse X");
		pitch -= v_speed * Input.GetAxis("Mouse Y");


		transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
		transform.Translate(x, 0, z);

        if(GameObject.FindGameObjectWithTag("Patient") != null && !patientDetected)
        {
            patientDetected = true;
            Camera.main.GetComponent<CameraFollow>().setTarget(GameObject.FindGameObjectWithTag("Patient").transform);
        }
	}

	public override void OnStartLocalPlayer()
	{
        gameObject.transform.position = new Vector3(-115.7f, 26.5f, 113.63f);
       
		Camera.main.GetComponent<CameraFollow>().setTarget(gameObject.transform);
	}
}
