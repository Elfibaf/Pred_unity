using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class TherapistUI : MonoBehaviour {

	public float rectWidth = Screen.width / 4;
	public float rectHeight = Screen.height / 8;

	//public int prevSelGridInt = 0;
	//public int selGridInt = 0;
	//public Object[] clipArray;
	//public List<string> clipNames = new List<string>();
	//public string[] clipNamesArray;

	public float agitation = 0;
	public float breathing = 0;
	public bool toggleAgitation = false;
	public float vSliderValue = 0.0f;


	void Start()
	{
	}

    void Update()
    {
		
    }

	void OnGUI()
	{
		GUI.skin.toggle.wordWrap = true;
		GUILayout.Window(4, new Rect(Screen.width - Screen.width/7 - 10, Screen.height/16 + 10, Screen.width/6, Screen.height/3), BiofeedbackWindow, "", GUIStyle.none);
		GUILayout.Window(3, new Rect(Screen.width/2 - rectWidth/2, Screen.height - rectHeight - 10, rectWidth, rectHeight), RecenterWindow, "", GUIStyle.none); 

		if (SceneManager.GetActiveScene ().name == "RelaxingEnv1") {
			GUILayout.Window(2, new Rect(Screen.width - Screen.width/8 - 10, Screen.height - Screen.height/16 - 10, Screen.width/8, Screen.height/16), Window, "", GUIStyle.none);
		}

		if (SceneManager.GetActiveScene ().name == "Ganzfeld") {
			GUILayout.Window(6, new Rect(10, Screen.height - Screen.height/16 - 10, Screen.width/8, Screen.height/16), GanzfeldWindow2, "", GUIStyle.none);
			GUILayout.Window(5, new Rect(Screen.width - Screen.width/8 - 10, Screen.height - Screen.height/16 - 10, Screen.width/8, Screen.height/16), GanzfeldWindow, "", GUIStyle.none);
		}
	}

	// Displayed in RelaxingEnv1
	void Window(int id)
	{
        if(!GameObject.FindGameObjectWithTag("Therapist").GetComponent<TherapistController>().fromGanzfeld)
        {
            if (GUILayout.Button("Phase Ganzfeld"))
            {
                Camera.main.GetComponent<Fading>().setFadingColor(GameObject.FindGameObjectWithTag("Patient").GetComponent<PatientController>().chosenColor);
                Camera.main.GetComponent<Fading>().setCurrentFadingTex(1);
                StartCoroutine(Fading(1, "Ganzfeld"));
            }
        }
        else
        {
            if (GUILayout.Button("Retour Accueil"))
            {
                Camera.main.GetComponent<Fading>().setCurrentFadingTex(0);
                StartCoroutine(Fading(1, "HomeRoom"));
            }
        }  
	}

    IEnumerator Fading(int direction, string sceneName)
    {
        // fade in
        print("fade " + direction + " " + sceneName);
        
        GameObject.FindGameObjectWithTag("Patient").GetComponent<PatientController>().fadeDir = direction;
        float fadeTime = Camera.main.GetComponent<Fading>().BeginFade(direction);
        
        if(sceneName != "")
        {
            yield return new WaitForSeconds(1.0f / fadeTime);
            GetComponent<NetworkManagerCustom>().ServerChangeScene(sceneName);
        }  
    }

	// Displays breathing and agitation + buttons to control the agitation value
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
					vSliderValue += 0.05f;
				}
				if (GUILayout.Button ("-", GUILayout.MaxWidth(50))) {
					print ("- clicked");
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

	void GanzfeldWindow2(int id) {
		if (GUILayout.Button ("Retour Accueil")) 
		{
			Camera.main.GetComponent<Fading>().setFadingColor(GameObject.FindGameObjectWithTag("Patient").GetComponent<PatientController>().chosenColor);
			Camera.main.GetComponent<Fading>().setCurrentFadingTex(1);
			StartCoroutine(Fading(1, "HomeRoom"));
			GameObject.FindGameObjectWithTag ("Therapist").GetComponent<TherapistController> ().fromGanzfeld = true;
		}
	}

	void RecenterWindow(int id) {
		if (GUILayout.Button ("Recentrer caméra")) {
			GameObject.FindGameObjectWithTag ("Therapist").GetComponent<TherapistController> ().CmdRecenter ();
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
