using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;

public class MapBehaviour : NetworkBehaviour {

    [SyncVar(hook = "OnChangeAgitation")]
	public float agitation;


    public RectTransform agitationBar;

    private float lastIntensity;
    private float lastCloudRotationSpeed;
    private float duration = 5;
    private float t;

	// Use this for initialization
	void Start () {

        foreach(GameObject tree in GameObject.FindGameObjectsWithTag("tree"))
        {
            tree.GetComponent<Animator>().ForceStateNormalizedTime(UnityEngine.Random.Range(0.0f, 1.0f));
			agitation = 0.0f;
        }

        agitationBar = GameObject.Find("Foreground").GetComponent<RectTransform>();

        //setAgitation(0.5f);
	}

    public void setAgitation(float a)
    {
		/*if (!isServer)
		{
			return;
		}*/


        agitation = a;
        if (agitation > 1.0f) agitation = 1.0f;
        if (agitation < 0.0f) agitation = 0.0f;

        foreach (GameObject tree in GameObject.FindGameObjectsWithTag("tree"))
        {
            Animator anim = tree.GetComponent<Animator>();
            anim.speed = 0.8f + agitation * 2.0f;
        }

        Animator anim_s = GameObject.FindGameObjectWithTag("sea").GetComponent<Animator>();
        anim_s.speed = 0.2f + agitation * 0.5f;

        t = 0.0f;
        lastIntensity = GameObject.FindGameObjectWithTag("DirectionalLight").GetComponent<Light>().intensity;
        lastCloudRotationSpeed = GameObject.FindGameObjectWithTag("DayNight").GetComponent<DayNightController>().cloudRotationSpeed;

        // update the agitation value on the UI
        GameObject.Find("AgitationValue").GetComponent<Text>().text = agitation.ToString();
        if(agitationBar != null)
        {
            agitationBar.sizeDelta = new Vector2(agitation*100.0f, agitationBar.sizeDelta.y);
        }
        
    }

    void OnChangeAgitation(float newAgitation) // called on clients
    {
		if (this.enabled) {
			print ("ONCHANGEAGITATION (" + newAgitation + ")");
			// update the agitation value on the UI
			GameObject.Find ("AgitationValue").GetComponent<Text> ().text = newAgitation.ToString ();
			if (agitationBar != null) {
				agitationBar.sizeDelta = new Vector2 (newAgitation * 100.0f, agitationBar.sizeDelta.y);
			}

			setAgitation (newAgitation);
		}
    }
	
	// Update is called once per frame (when a script component is disabled, only the update function is disabled)
	void Update () {

        if(this.enabled) // only if component is enabled
        {
            GameObject.FindGameObjectWithTag("DirectionalLight").GetComponent<Light>().intensity = Mathf.Lerp(lastIntensity, (0.35f + (1.0f - agitation) * 0.65f), t);
            GameObject.FindGameObjectWithTag("DayNight").GetComponent<DayNightController>().cloudRotationSpeed = Mathf.Lerp(lastCloudRotationSpeed, (10.0f + agitation * 100.0f), t);
            t += Time.deltaTime / duration;
        }
	}
		
}
