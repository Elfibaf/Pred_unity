using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBehaviour : MonoBehaviour {

    private float agitation;

    private float lastIntensity;
    private float lastCloudRotationSpeed;
    private float duration = 5;
    private float t;

	// Use this for initialization
	void Start () {
        agitation = 0.0f;
        if (transform.tag == "tree")
        {
            this.GetComponent<Animator>().ForceStateNormalizedTime(UnityEngine.Random.Range(0.0f, 1.0f));
        }

        setAgitation(1.0f);
	}

    public void setAgitation(float a)
    {
        agitation = a;
        if (agitation > 1.0f) agitation = 1.0f;
        if (agitation < 0.0f) agitation = 0.0f;

        if(transform.tag == "tree")
        {
            Animator anim = transform.gameObject.GetComponent<Animator>();
            anim.speed = 0.8f + agitation * 2.0f;
        }

        if (transform.tag == "sea")
        {
            Animator anim = transform.gameObject.GetComponent<Animator>();
            anim.speed = 0.2f + agitation * 0.5f;
        }

        t = 0.0f;
        lastIntensity = GameObject.FindGameObjectWithTag("DirectionalLight").GetComponent<Light>().intensity;
        lastCloudRotationSpeed = GameObject.FindGameObjectWithTag("DayNight").GetComponent<DayNightController>().cloudRotationSpeed;
    }
	
	// Update is called once per frame
	void Update () {

        GameObject.FindGameObjectWithTag("DirectionalLight").GetComponent<Light>().intensity = Mathf.Lerp(lastIntensity, (0.35f + (1.0f - agitation) * 0.65f), t);
        GameObject.FindGameObjectWithTag("DayNight").GetComponent<DayNightController>().cloudRotationSpeed = Mathf.Lerp(lastCloudRotationSpeed, (10.0f + agitation * 100.0f), t);
        t += Time.deltaTime / duration; 
	}
}
