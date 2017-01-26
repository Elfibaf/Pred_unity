using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PatientController : NetworkBehaviour {

	public float h_speed = 2.0f;
	public float v_speed = 2.0f;

    private float yaw = -100.5f;
	private float pitch = 0.0f;

    public Object[] whiteNoiseArray;

    public bool MouseControl = false;
    public Color chosenColor = Color.red;
    public int chosenSound = -1;
    public float chosenVolume = 0.5f;
	public float chosenIntensity = 0.75f;


    [SyncVar(hook = "OnChangeFadeDir")]
    public int fadeDir = 0;


	void Awake()
	{
		DontDestroyOnLoad (this);
	}

	void Start () {
		// Loading resources in advance to prevent lag in Ganzfeld phase
		whiteNoiseArray = Resources.LoadAll ("Audio");
		if (GameObject.Find ("Audio Source") != null) {
			GameObject.Find ("Audio Source").GetComponent<AudioController> ().SetClipArray (whiteNoiseArray);
		}
		print ("Audio chargé");
	}
	
	// Update is called once per frame
	void Update () {

		if (!isLocalPlayer) 
		{
			return;
		}

		//var x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
		//var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
        yaw += h_speed * Input.GetAxis("Mouse X");
        pitch -= v_speed * Input.GetAxis("Mouse Y");

        if(MouseControl)
        {
            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
            //transform.Translate(0, x, z);
        }
		
		/*if (Input.GetKeyDown(KeyCode.Space))
		{
			CmdChangeAgitation();
			print (GetComponent<MapBehaviour> ().agitation);
		}*/
		/*if (Input.GetKeyDown (KeyCode.A)) {
			//chosenRange -= 0.5f;
			GameObject.Find ("Point light").GetComponent<Light> ().range -= 0.5f;
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			//chosenRange += 0.5f;
			GameObject.Find ("Point light").GetComponent<Light> ().range += 0.5f;
		}
		if (Input.GetKeyDown (KeyCode.Q)) {
			GameObject.Find ("Point light").GetComponent<Light> ().intensity -= 0.1f;
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			GameObject.Find ("Point light").GetComponent<Light> ().intensity += 0.1f;
		}*/
	}

	/*[Command] // calls the function only server-side
	public void CmdChangeAgitation()
	{
		if (GetComponent<MapBehaviour> ().agitation < 0.5f) {
			//GetComponent<MapBehaviour> ().setAgitation (0.5f);
			GetComponent<MapBehaviour> ().agitation = 0.5f;
		} else if (GetComponent<MapBehaviour> ().agitation == 0.5f) {
			GetComponent<MapBehaviour> ().setAgitation (1.0f);
		} else 
		{
			GetComponent<MapBehaviour> ().setAgitation (0.0f);
		}

	}*/
		
	public void OnChangeFadeDir(int direction)
	{
		fadeDir = direction;
		if(Camera.main.GetComponent<Fading>() != null)
		{
			if (direction == 1)
			{
				Camera.main.GetComponent<Fading>().FadeOut();
			}
			else if (direction == -1)
			{
				Camera.main.GetComponent<Fading>().FadeIn();
			}
		}
		print("ONCHANGEFADEDIR: " + direction);
		if (SceneManager.GetActiveScene ().name == "HomeRoom") {
			Camera.main.GetComponent<Fading> ().exitingHomeRoom = true;
		}
	}

	public override void OnStartLocalPlayer() // ONLY CLIENT SIDE
	{

        gameObject.transform.position = new Vector3(0, 0, 0);

		GetComponent<CameraFollow>().setTarget(Camera.main.transform);

		print (SceneManager.GetActiveScene ().name);

		if (SceneManager.GetActiveScene ().name == "RelaxingEnv1") {
            // setting up fading tex
            //OnChangeFadeDir(-1);
            if (isLocalPlayer)
            {
                Camera.main.GetComponent<Fading>().FadeIn();
            }
           
            //fadeDir = -1;
            Camera.main.GetComponent<Fading>().setFadingColor(chosenColor);
            StartCoroutine(SwitchFadingTex(1));

			if (GameObject.FindGameObjectWithTag ("Therapist").GetComponent<TherapistController> ().fromGanzfeld) {
                GameObject.Find("Background").SetActive(false);
				Camera.main.GetComponent<Fading>().setCurrentFadingTex(1);
                StartCoroutine(SwitchFadingTex(0));
			}
            
			GetComponent<MapBehaviour>().enabled = true;

			print ("Mapbehaviour active");
		}
        else
        {
            GetComponent<MapBehaviour>().enabled = false;
			print ("Mapbehaviour desactive");
        }

        if (SceneManager.GetActiveScene().name == "HomeRoom")
        {
            Camera.main.GetComponent<Fading>().FadeIn();

            if (GameObject.FindGameObjectWithTag ("Therapist").GetComponent<TherapistController> ().fromGanzfeld) {
                
            }
        }

        if (SceneManager.GetActiveScene().name == "Ganzfeld")
        {
            Camera.main.GetComponent<Fading>().setFadingColor(chosenColor);
            Camera.main.GetComponent<Fading>().setCurrentFadingTex(1);
			//StartCoroutine(SwitchFadingTex(1));
            OnChangeFadeDir(-1); // start fadeOut after loading Ganzfeld scene

            GameObject.Find("Point light").GetComponent<Light>().color = chosenColor;
            GameObject.Find("Point light").GetComponent<Light>().intensity = chosenIntensity*8.0f;
        }      
	}

    IEnumerator SwitchFadingTex(int index)
    {
        yield return new WaitForSeconds(10.0f);
        Camera.main.GetComponent<Fading>().setCurrentFadingTex(index);
        fadeDir = -1; // car fadeDir est reset à 1 (au lieu d'être -1) à la transition vers  RelaxationEnv1
    }

    IEnumerator delayedDisabling(string name, float delay)
    {
        yield return new WaitForSeconds(delay);
        print("source delayed");
        GameObject.Find(name).SetActive(false);
    }
}
