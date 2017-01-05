using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandleOK : MonoBehaviour
{

    private bool state;
    private bool isAnimOver;
    private bool isAnimAtStart;
    private string animName;
    private bool isActivated;

    public Color col;
    private Color lastCol;
    private float duration = 5;
    private float t;

    // Use this for initialization
    void Start()
    {
        state = false;
        isAnimOver = false;
        isActivated = false;
        animName = "circle_button";

        //transform.gameObject.GetComponentInChildren<SpriteRenderer>().color = col;

        foreach (Transform child in transform)
        {
            if (child.tag == "circle")
            {
                child.GetComponent<SpriteRenderer>().color = col;
            }
        }
    }

    public void setState(bool b)
    {
        state = b;
    }

    public bool getActivated()
    {
        return (isActivated);
    }

    public void setActivated(bool b)
    {
        isActivated = b;
    }

    private void action()
    {
        // only if Patient prefab contained in the scene is the local player
        if (GameObject.FindGameObjectWithTag("Therapist").GetComponent<TherapistController>().isLocalPlayer) // only triggers scene change on patient side
        {
            GameObject.Find("NetworkManager").GetComponent<NetworkManagerCustom>().ServerChangeScene("RelaxingEnv1");
        }
		
    }

    // Update is called once per frame
    void Update()
    {

        //print(isAnimOver);
        foreach (Transform child in transform)
        {
            if (child.tag == "square")
            {
                
                if (state) // button is focused
                {
                    if (!child.gameObject.GetComponent<Animation>().IsPlaying(animName) && !isAnimOver) // we play animation if it is not already playing
                    {
                        Animation anim = child.gameObject.GetComponent<Animation>();
                        anim[animName].speed = 1f;
                        child.gameObject.GetComponent<Animation>().Play();
                    }
                }
                else if (!isActivated)
                {
                    Animation anim = child.gameObject.GetComponent<Animation>();
                    anim[animName].speed = -1f;
                    //anim["circle_button"].time = anim["circle_button"].length;
                    child.gameObject.GetComponent<Animation>().Play();
                }

                // check if anim is over
                Animation anim2 = child.gameObject.GetComponent<Animation>();
                //print(isActivated);
                if (anim2[animName].time >= anim2[animName].length && !isAnimOver) // animation over (called once)
                {
                    isAnimOver = true;
                    child.gameObject.GetComponent<Animation>().Stop();
                    anim2[animName].time = anim2[animName].length;
                    isActivated = true;
                    Camera.main.GetComponent<UIRaycast>().activatedButton = transform.gameObject;
                    action();
                }

                if (anim2[animName].time < anim2[animName].length)
                {
                    isAnimOver = false;
                }

                if (anim2[animName].time < 0) // button re-initialized
                {
                    isAnimAtStart = true;
                    child.gameObject.GetComponent<Animation>().Stop();
                    anim2[animName].time = 0;
                    t = 0;
                }

                if (isActivated) // If isActivated
                {
                    
                }
            }
        }
    }
}
