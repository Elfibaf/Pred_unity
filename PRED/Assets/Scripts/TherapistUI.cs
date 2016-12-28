using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;

public class TherapistUI : MonoBehaviour {

	void OnGUI()
	{
		GUILayout.Window(2, new Rect(Screen.width - Screen.width/8 - 10, Screen.height - Screen.height/16 - 10, Screen.width/8, Screen.height/16), Window, "", GUIStyle.none);
	}

	void Window(int id)
	{
		GUI.Box (new Rect (0, 0, Screen.width - 260, Screen.height - 20), "");

		if (GUILayout.Button ("Phase Ganzfeld")) 
		{
			print ("Got a click");
			GetComponent<NetworkManagerCustom> ().ServerChangeScene ("Ganzfeld");
            GameObject.FindGameObjectWithTag("Patient").GetComponent<MapBehaviour>().enabled = false;
		}
	}
}
