using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class TherapistUI : MonoBehaviour {

	public float rectWidth = Screen.width / 2;
	public float rectHeight = Screen.height / 8;
	//private AudioSource whiteNoiseSource;

	public int prevSelGridInt = 0;
	public int selGridInt = 0;
	public Object[] clipArray;
	public List<string> clipNames = new List<string>();
	public string[] clipNamesArray;

	public float agitation = 0;
	public float breathing = 0;
	public bool toggleAgitation = false;
	public float vSliderValue = 0.0f;


	void Start()
	{
		/*clipArray = Resources.LoadAll ("Audio");
		foreach (AudioClip clip in clipArray) {
			clipNames.Add (clip.name);
		}
		clipNamesArray = clipNames.ToArray();*/
	}

    void Update()
    {

    }

	void OnGUI()
	{
		GUILayout.Window(4, new Rect(Screen.width - Screen.width/8 - 10, Screen.height/16 + 10, Screen.width/8, Screen.height/16), BiofeedbackWindow, "", GUIStyle.none);

		if (SceneManager.GetActiveScene ().name == "RelaxingEnv1") {
			GUILayout.Window(2, new Rect(Screen.width - Screen.width/8 - 10, Screen.height - Screen.height/16 - 10, Screen.width/8, Screen.height/16), Window, "", GUIStyle.none);
		}

		if (SceneManager.GetActiveScene ().name == "Ganzfeld") {
			GUILayout.Window(2, new Rect(Screen.width - Screen.width/8 - 10, Screen.height - Screen.height/16 - 10, Screen.width/8, Screen.height/16), GanzfeldWindow, "", GUIStyle.none);
		}
	}

	void Window(int id)
	{
		//GUI.Box (new Rect (0, 0, Screen.width - 260, Screen.height - 20), "");


		if (GUILayout.Button ("Phase Ganzfeld")) 
		{
            Camera.main.GetComponent<Fading>().setFadingColor(GameObject.FindGameObjectWithTag("Patient").GetComponent<PatientController>().chosenColor);
            Camera.main.GetComponent<Fading>().setCurrentFadingTex(1);
            StartCoroutine(Fading(1, "Ganzfeld"));
		}
	}

    IEnumerator Fading(int direction, string sceneName)
    {
        // fade in
        print("fade " + direction + " " + sceneName);
        
        GameObject.FindGameObjectWithTag("Patient").GetComponent<PatientController>().fadeDir = direction;
        float fadeTime = Camera.main.GetComponent<Fading>().BeginFade(direction);
        

        //GameObject.FindGameObjectWithTag("Patient").GetComponent<MapBehaviour>().enabled = false;
        if(sceneName != "")
        {
            yield return new WaitForSeconds(1.0f / fadeTime);
            GetComponent<NetworkManagerCustom>().ServerChangeScene(sceneName);
        }  
    }


	void BiofeedbackWindow(int id)
	{
		GUILayout.Label ("Breathing : " + breathing);
		GUILayout.Label("Agitation : " + agitation);
		if (SceneManager.GetActiveScene ().name == "RelaxingEnv1") {
			toggleAgitation = GUILayout.Toggle (toggleAgitation, "Contrôle manuel de l'agitation");
			if (toggleAgitation) {
				vSliderValue = GameObject.FindGameObjectWithTag ("Patient").GetComponent<MapBehaviour> ().agitation;
				if (GUILayout.Button ("+", GUILayout.MaxWidth(50))) {
					print ("+ clicked");
					//GameObject.FindGameObjectWithTag ("Patient").GetComponent<MapBehaviour> ().CmdAgitationUp (0.05f);
					vSliderValue += 0.05f;
				}
				if (GUILayout.Button ("-", GUILayout.MaxWidth(50))) {
					print ("- clicked");
					//GameObject.FindGameObjectWithTag ("Patient").GetComponent<MapBehaviour> ().CmdAgitationDown (0.05f);
					vSliderValue -= 0.05f;
				}
				vSliderValue = GUILayout.VerticalSlider (vSliderValue, 1.0f, 0.0f);
				GameObject.FindGameObjectWithTag ("Patient").GetComponent<MapBehaviour> ().agitation = vSliderValue;

			}
		}

	}

	void GanzfeldWindow(int id) {
		if (GUILayout.Button ("Phase Relaxation")) 
		{
			Camera.main.GetComponent<Fading>().setFadingColor(GameObject.FindGameObjectWithTag("Patient").GetComponent<PatientController>().chosenColor);
			Camera.main.GetComponent<Fading>().setCurrentFadingTex(1);
			StartCoroutine(Fading(1, "RelaxingEnv1"));
			GameObject.FindGameObjectWithTag ("Therapist").GetComponent<TherapistController> ().fromGanzfeld = true;
		}
	}

	/*void WhiteNoiseWindow(int id)
	{
		GUILayout.Label("Choix du bruit blanc");
		selGridInt = GUILayout.SelectionGrid (selGridInt, clipNamesArray, 2);

		if (prevSelGridInt != selGridInt) {
			prevSelGridInt = selGridInt;
			print("Changement de musique " + clipNamesArray[selGridInt]);
			whiteNoiseSource.GetComponent<AudioController> ().RpcPlaySoundFromButton (selGridInt);
			PlaySoundFromButton(selGridInt);
			//whiteNoiseSource.GetComponent<AudioController> ().CmdPlaySound (selGridInt);

		}

		/*if (GUILayout.Button ("Bruit 1")) 
		{
			print ("Got a click");
			whiteNoiseSource.GetComponent<AudioController>().RpcPlaySound("15 Highway to Hell");
		}
		if (GUILayout.Button ("Bruit 2")) {
			whiteNoiseSource.GetComponent<AudioController>().RpcPlaySound("Make It Bun Dem - Skrillex _ Damian Jr. Gong Marley");
		}

	}*/

	// Using this function because RPC won't get called on host (when running in standalone build)
	/*void PlaySoundFromButton(int soundNum)
	{
		whiteNoiseSource.clip = (AudioClip)clipArray [soundNum];
		whiteNoiseSource.Play ();
	}*/

}
