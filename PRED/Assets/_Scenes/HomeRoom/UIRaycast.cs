using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRaycast : MonoBehaviour {

    private GameObject[] buttons_col;
    private GameObject[] buttons_sound;
    public GameObject activatedButtonCol;
    public GameObject activatedButtonSound;

	// Use this for initialization
	void Start () {
        buttons_col = GameObject.FindGameObjectsWithTag("button_col");
        buttons_sound = GameObject.FindGameObjectsWithTag("button_sound");
        activatedButtonCol = null;
        activatedButtonSound = null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, fwd, out hit, 10))
        {        
            
            if (hit.collider.tag == "button_col")
            {
                foreach(GameObject button in buttons_col)
                {
                    if(button.GetInstanceID() == hit.transform.gameObject.GetInstanceID())
                    {
                        button.GetComponent<HandleButtonCol>().setState(true);
                    }
                    else
                    {
                        button.GetComponent<HandleButtonCol>().setState(false);
                    }

                    if(activatedButtonCol != null && (button.GetInstanceID() != activatedButtonCol.GetInstanceID())) // if it's not the last activated button, we deactivate it
                    {
                        button.GetComponent<HandleButtonCol>().setActivated(false);
                    }
                }  
            }
            else if (hit.collider.tag == "button_sound")
            {

                foreach (GameObject button in buttons_sound)
                {
                    if (button.GetInstanceID() == hit.transform.gameObject.GetInstanceID())
                    {
                        button.GetComponent<HandleButtonSound>().setState(true);
                    }
                    else
                    {
                        button.GetComponent<HandleButtonSound>().setState(false);
                    }

                    if (activatedButtonSound != null && (button.GetInstanceID() != activatedButtonSound.GetInstanceID())) // if it's not the last activated button, we deactivate it
                    {
                        button.GetComponent<HandleButtonSound>().setActivated(false);
                    }
                }
            }
            else if(hit.collider.name == "button_OK")
            {
                hit.transform.gameObject.GetComponent<HandleOK>().setState(true);
            }
            else if(hit.collider.name == "button_plus")
            {
                hit.transform.gameObject.GetComponent<HandleVolume>().setState(true);
            }
            else if(hit.collider.name == "button_minus")
            {
                hit.transform.gameObject.GetComponent<HandleVolume>().setState(true);
            }
            else // hit but not on a circle_button
            {
                foreach (GameObject button in buttons_col)
                {
                    button.GetComponent<HandleButtonCol>().setState(false);
                }
                foreach (GameObject button in buttons_sound)
                {
                    button.GetComponent<HandleButtonSound>().setState(false);
                }
                //GameObject.Find("button_OK").GetComponent<HandleOK>().setState(false);
                //GameObject.Find("button_plus").GetComponent<HandleVolume>().setState(false);
                //GameObject.Find("button_minus").GetComponent<HandleVolume>().setState(false);
            }
        }
        else // no collision
        {
            foreach (GameObject button in buttons_col)
            {
                button.GetComponent<HandleButtonCol>().setState(false);
            }
            foreach (GameObject button in buttons_sound)
            {
                button.GetComponent<HandleButtonSound>().setState(false);
            }
            GameObject.Find("button_OK").GetComponent<HandleOK>().setState(false);

            GameObject.Find("button_plus").GetComponent<HandleVolume>().setState(false);
            GameObject.Find("button_minus").GetComponent<HandleVolume>().setState(false); ;
        }


            
    }
}
