using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.VR;

public class TherapistController : NetworkBehaviour {

	public float h_speed = 2.0f;
	public float v_speed = 2.0f;

	private float yaw = -100.5f;
	private float pitch = 0.0f;
    private bool patientDetected = false;

	void Awake()
	{
		DontDestroyOnLoad (this);
	}

	// Update is called once per frame
	void Update () {

		if (!isLocalPlayer) 
		{
			return;
		}

		/*var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 150.0f;*/

		yaw += h_speed * Input.GetAxis("Mouse X");
		pitch -= v_speed * Input.GetAxis("Mouse Y");


		//transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
		//transform.Translate(x, 0, z);

        if(GameObject.FindGameObjectWithTag("Patient") != null && !patientDetected)
        {
			
            patientDetected = true;
            Camera.main.GetComponent<CameraFollow>().setTarget(GameObject.FindGameObjectWithTag("Patient").transform);
            GameObject.FindGameObjectWithTag("Patient").GetComponent<GyroControl>().enabled = false; // we disable the gyroControl component on the host (therapist)
            GameObject.FindGameObjectWithTag("Patient").GetComponent<CameraFollow>().enabled = false;
        }

	}

	public override void OnStartLocalPlayer()
	{
        gameObject.transform.position = new Vector3(-115.7f, 26.5f, 113.63f);
		patientDetected = false;

        // On therapist side : the main camera follows the Patient Prefab
        Camera.main.GetComponent<CameraFollow>().enabled = true;
        Camera.main.GetComponent<CameraFollow>().setTarget(gameObject.transform);
        if(Camera.main.GetComponent<GvrHead>() != null)
        {
            Camera.main.GetComponent<GvrHead>().enabled = false;
        }

        if (GameObject.Find("GvrViewerMain") != null)
        {
            GameObject.Find("GvrViewerMain").GetComponent<GvrViewer>().VRModeEnabled = false;
        }
        VRSettings.enabled = false;

        if (SceneManager.GetActiveScene().name == "RelaxingEnv1")
        {
            if (GameObject.FindGameObjectWithTag("Patient") != null)
            {
                GameObject.FindGameObjectWithTag("Patient").GetComponent<MapBehaviour>().enabled = true;
            }
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("Patient") != null)
            {
                GameObject.FindGameObjectWithTag("Patient").GetComponent<MapBehaviour>().enabled = false;
            }
        }

        if (SceneManager.GetActiveScene().name == "Ganzfeld")
        {
            GameObject.Find("Point light").GetComponent<Light>().color = GameObject.FindGameObjectWithTag("Patient").GetComponent<PatientController>().chosenColor;
        }

		print (SceneManager.GetActiveScene ().name);

	}
}
