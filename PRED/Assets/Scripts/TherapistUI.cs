using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class TherapistUI : MonoBehaviour {

	public float rectWidth = Screen.width / 2;
	public float rectHeight = Screen.height / 8;
	private AudioSource whiteNoiseSource;

	public int prevSelGridInt = 0;
	public int selGridInt = 0;
	public Object[] clipArray;
	public List<string> clipNames = new List<string>();
	public string[] clipNamesArray;

	void Start()
	{
		clipArray = Resources.LoadAll ("Audio");
		foreach (AudioClip clip in clipArray) {
			clipNames.Add (clip.name);
		}
		clipNamesArray = clipNames.ToArray();
	}

	void OnGUI()
	{
		if (SceneManager.GetActiveScene ().name == "RelaxingEnv1") {
			GUILayout.Window(2, new Rect(Screen.width - Screen.width/8 - 10, Screen.height - Screen.height/16 - 10, Screen.width/8, Screen.height/16), Window, "", GUIStyle.none);
		}

		if (SceneManager.GetActiveScene ().name == "Ganzfeld") {
			GUILayout.Window(3, new Rect(rectWidth/2, Screen.height - rectHeight - 10, rectWidth, rectHeight), WhiteNoiseWindow, "", GUIStyle.none);
			whiteNoiseSource = GameObject.Find ("Audio Source").GetComponent<AudioSource> ();
		}
	}

	void Window(int id)
	{
		//GUI.Box (new Rect (0, 0, Screen.width - 260, Screen.height - 20), "");


		if (GUILayout.Button ("Phase Ganzfeld")) 
		{
			GetComponent<NetworkManagerCustom> ().ServerChangeScene ("Ganzfeld");
            GameObject.FindGameObjectWithTag("Patient").GetComponent<MapBehaviour>().enabled = false;
		}
	}

	void WhiteNoiseWindow(int id)
	{
		
		//GUI.Box (new Rect (0, 0, Screen.width - 260, Screen.height - 20), "");
		GUILayout.Label("Choix du bruit blanc");
		selGridInt = GUILayout.SelectionGrid (selGridInt, clipNamesArray, 2);

		if (prevSelGridInt != selGridInt) {
			prevSelGridInt = selGridInt;
			whiteNoiseSource.GetComponent<AudioController> ().RpcPlaySoundFromButton (selGridInt);
		}

		/*if (GUILayout.Button ("Bruit 1")) 
		{
			print ("Got a click");
			whiteNoiseSource.GetComponent<AudioController>().RpcPlaySound("15 Highway to Hell");
		}
		if (GUILayout.Button ("Bruit 2")) {
			whiteNoiseSource.GetComponent<AudioController>().RpcPlaySound("Make It Bun Dem - Skrillex _ Damian Jr. Gong Marley");
		}*/

	}
		


}
