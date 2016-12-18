using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRaycast : MonoBehaviour {

    private GameObject[] circle_buttons;
    public GameObject activatedButton;

	// Use this for initialization
	void Start () {
        circle_buttons = GameObject.FindGameObjectsWithTag("circle_button");
        activatedButton = null;
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
            if (hit.collider.tag == "circle_button")
            {
                //print("button0 is hit!");
                foreach(GameObject button in circle_buttons)
                {
                    if(button.GetInstanceID() == hit.transform.gameObject.GetInstanceID())
                    {
                        button.GetComponent<HandleButton>().setState(true);
                    }
                    else
                    {
                        button.GetComponent<HandleButton>().setState(false);
                    }

                    if(activatedButton != null && (button.GetInstanceID() != activatedButton.GetInstanceID())) // if it's not the last activated button, we deactivate it
                    {
                        button.GetComponent<HandleButton>().setActivated(false);
                    }
                }  
            }
            else // hit but not on a circle_button
            {
                foreach (GameObject button in circle_buttons)
                {
                    button.GetComponent<HandleButton>().setState(false);
                }  
            }
        }
        else // no collision
        {
            foreach (GameObject button in circle_buttons)
            {
                button.GetComponent<HandleButton>().setState(false);
            }
        }


            
    }
}
