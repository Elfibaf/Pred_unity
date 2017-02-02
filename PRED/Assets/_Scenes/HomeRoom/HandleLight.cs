using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleLight : MonoBehaviour
{

    private bool state = false; // true if the button is hovered
    private GameObject patient;
    private GameObject text;
    private bool patientDetected = false;
    public float changeSpeed;

    public float raiseLight(int dir)
    {
        float chosenIntensity = patient.GetComponent<PatientController>().chosenIntensity;
        chosenIntensity += dir * changeSpeed * Time.deltaTime;
        chosenIntensity = Mathf.Clamp01(chosenIntensity);
        if (chosenIntensity < 0.2f) chosenIntensity = 0.2f;
        patient.GetComponent<PatientController>().chosenIntensity = chosenIntensity;
        GameObject.Find("Point light").GetComponent<Light>().intensity = chosenIntensity * 8.0f;
        text.GetComponent<TextMesh>().text = Mathf.FloorToInt(chosenIntensity * 100).ToString(); ;
        return (chosenIntensity);
    }

    public void setState(bool b)
    {
        state = b;
    }


    // Use this for initialization
    void Start()
    {
        text = GameObject.Find("light_txt");
        text.GetComponent<TextMesh>().text = Mathf.FloorToInt(patient.GetComponent<PatientController>().chosenIntensity * 100).ToString();
        GameObject.Find("Point light").GetComponent<Light>().intensity = patient.GetComponent<PatientController>().chosenIntensity * 8.0f;
    }

    // Update is called once per frame
    void Update()
    {

        if (!patientDetected && GameObject.FindGameObjectWithTag("Patient") != null)
        {
            patient = GameObject.FindGameObjectWithTag("Patient");
            patientDetected = !patientDetected;
        }


        if (state)
        {
            if (gameObject.name == "button_plus")
                raiseLight(1);
            else if (gameObject.name == "button_minus")
                raiseLight(-1);
        }
    }
}
