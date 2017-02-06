using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleVolume : MonoBehaviour {

    private bool state = false; // true if the button is hovered
    private GameObject patient;
    private GameObject text;
    private bool patientDetected = false;
    public float changeSpeed;

    public float raiseVolume(int dir)
    {
        float chosenVolume = patient.GetComponent<PatientController>().chosenVolume;
        chosenVolume += dir * changeSpeed * Time.deltaTime;
        chosenVolume = Mathf.Clamp01(chosenVolume);
        if (chosenVolume < 0.20f) chosenVolume = 0.20f;
        patient.GetComponent<PatientController>().chosenVolume = chosenVolume;
		GameObject.Find ("Audio Source").GetComponent<AudioController> ().changeVolume (chosenVolume);
        text.GetComponent<TextMesh>().text = Mathf.FloorToInt(chosenVolume * 100).ToString(); ;
        return (chosenVolume);
    }

    public void setState(bool b)
    {
        state = b;
    }


	// Use this for initialization
	void Start () {
        text = GameObject.Find("volume_txt");
	}
	
	// Update is called once per frame
	void Update () {

        if(!patientDetected && GameObject.FindGameObjectWithTag("Patient") != null)
        {
            patient = GameObject.FindGameObjectWithTag("Patient");
            patientDetected = !patientDetected;
        }


		if(state)
        {
            if (gameObject.name == "button_plus")
                raiseVolume(1);
            else if (gameObject.name == "button_minus")
                raiseVolume(-1);
        }
	}
}
