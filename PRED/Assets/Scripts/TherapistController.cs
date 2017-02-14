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

	[SyncVar(hook = "OnChangeFromGanzfeld")]
	public bool fromGanzfeld = false;

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
		yaw += h_speed * Input.GetAxis("Mouse X");
		pitch -= v_speed * Input.GetAxis("Mouse Y");

        if(GameObject.FindGameObjectWithTag("Patient") != null && !patientDetected)
        {
			
            patientDetected = true;
            Camera.main.GetComponent<CameraFollow>().setTarget(GameObject.FindGameObjectWithTag("Patient").transform);
            GameObject.FindGameObjectWithTag("Patient").GetComponent<GyroControl>().enabled = false; // we disable the gyroControl component on the host (therapist)
            GameObject.FindGameObjectWithTag("Patient").GetComponent<CameraFollow>().enabled = false;
        }
	}

	public void OnChangeFromGanzfeld(bool fG) {
		fromGanzfeld = fG;
	}

	// Recenters both cameras
	[ClientRpc]
	public void RpcRecenter() {
		if (isServer){
			return;
		}
		UnityEngine.Object.FindObjectOfType<GvrViewer> ().Recenter ();
	}

	[Command]
	public void CmdRecenter() {
		RpcRecenter ();
	}

    public override void OnStartLocalPlayer() // ONLY SERVER SIDE
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
				print ("Mapbehaviour active");
            }
			// Handling fading
			if (fromGanzfeld) {
                GameObject.FindGameObjectWithTag("Patient").GetComponent<PatientController>().fadeDir = -1;
                Camera.main.GetComponent<Fading>().setCurrentFadingTex(1);
			}
		}
        else
        {
            if (GameObject.FindGameObjectWithTag("Patient") != null)
            {
                GameObject.FindGameObjectWithTag("Patient").GetComponent<MapBehaviour>().enabled = false;
				print ("MapBehaviour desactive");
            }
        }
		// Setting fading + Ganzfeld light
        if (SceneManager.GetActiveScene().name == "Ganzfeld")
        {
			GameObject.FindGameObjectWithTag("Patient").GetComponent<PatientController>().fadeDir = -1;
			Camera.main.GetComponent<Fading>().setCurrentFadingTex(1);
            GameObject.Find("Point light").GetComponent<Light>().color = GameObject.FindGameObjectWithTag("Patient").GetComponent<PatientController>().chosenColor;
            GameObject.Find("Point light").GetComponent<Light>().intensity = (0.35f + (GameObject.FindGameObjectWithTag("Patient").GetComponent<PatientController>().chosenIntensity * (1 - 0.35f))) * 8.0f; // 0.35f is the minimalValue from HandleLight scripts
        }

		print (SceneManager.GetActiveScene ().name);

	}
}
