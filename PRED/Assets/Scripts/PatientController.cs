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


    [SyncVar(hook = "OnChangeFadeDir")]
    public int fadeDir = -1;

    void OnChangeFadeDir(int direction)
    {
        fadeDir = direction;
        if(Camera.main.GetComponent<Fading>() != null)
        {
            Camera.main.GetComponent<Fading>().BeginFade(direction);
        }
        print("ONCHANGEFADEDIR: " + direction);
    }

	void Awake()
	{
		DontDestroyOnLoad (this);
	}

	void Start () {
		// Loading resources in advance to prevent lag in Ganzfeld phase
		whiteNoiseArray = Resources.LoadAll ("Audio");
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
		
		if (Input.GetKeyDown(KeyCode.Space))
		{
			CmdChangeAgitation();
			print (GetComponent<MapBehaviour> ().agitation);
		}
	}

	[Command] // calls the function only server-side
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

	}
		

	public override void OnStartLocalPlayer()
	{

        gameObject.transform.position = new Vector3(-115.7f, 26.5f, 113.63f);

		GetComponent<CameraFollow>().setTarget(Camera.main.transform);

		print (SceneManager.GetActiveScene ().name);

		if (SceneManager.GetActiveScene ().name == "RelaxingEnv1") {
			GetComponent<MapBehaviour>().enabled = true;
			print ("Mapbehaviour active");
		}
        else
        {
            GetComponent<MapBehaviour>().enabled = false;
			print ("Mapbehaviour desactive");
        }

        if (SceneManager.GetActiveScene().name == "Ganzfeld")
        {
            GameObject.Find("Point light").GetComponent<Light>().color = chosenColor;
        }

        //gameObject.GetComponent<MapBehaviour>().enabled = false;

        //GameObject.Find("MapBehaviour").GetComponent<MapBehaviour>().setAgitation(0.0f);
        
	}


}
