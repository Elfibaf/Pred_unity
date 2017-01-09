using UnityEngine;
using System.Collections;
using System.Linq;

namespace VoiceChat.Demo
{
	public class VoiceChatUiClient : MonoBehaviour
	{
		bool recording = false;
		void Awake()
		{
			DontDestroyOnLoad (this);
		}
		void Start()
		{
			Application.RequestUserAuthorization(UserAuthorization.Microphone);
		}

		void OnGUI()
		{
			GUILayout.Window(1, new Rect(10, 10, Screen.width/4, Screen.height/8), Window, "", GUIStyle.none);
		}

		void Window(int id)
		{
			GUI.Box(new Rect(0, 0, Screen.width - 260, Screen.height - 20), "");

			/*if (VoiceChatRecorder.Instance.IsRecording)
			{
				GUILayout.Label(VoiceChatRecorder.Instance.Device);

				if (GUILayout.Button("Arrêter l'enregistrement"))
				{
					VoiceChatRecorder.Instance.StopRecording();
				}
			}
			else
			{
				GUILayout.Label("Sélectionnez un microphone");

				foreach (string device in VoiceChatRecorder.Instance.AvailableDevices)
				{
					if (GUILayout.Button(device))
					{
						VoiceChatRecorder.Instance.Device = device;
						VoiceChatRecorder.Instance.StartRecording();
					}
				}
			}*/

			if (!recording) {
				VoiceChatRecorder.Instance.Device = VoiceChatRecorder.Instance.AvailableDevices [0]; // Assignation du premier micro de la liste
				VoiceChatRecorder.Instance.StartRecording(); //
				recording = true;
			}


			/*if (VoiceChatRecorder.Instance.Device != null)
			{
				//GUILayout.Label("Appuyez pour parler: " + VoiceChatRecorder.Instance.PushToTalkKey);
				//GUILayout.Label("Toggle-to-talk key: " + VoiceChatRecorder.Instance.ToggleToTalkKey);
				GUILayout.Label("Auto detect speech: " + (VoiceChatRecorder.Instance.AutoDetectSpeech ? "On" : "Off"));

				/*if (GUILayout.Button("Toggle Auto Detect"))
                {
                    VoiceChatRecorder.Instance.AutoDetectSpeech = !VoiceChatRecorder.Instance.AutoDetectSpeech;
                }

				/*GUI.color = VoiceChatRecorder.Instance.IsTransmitting ? Color.green : Color.red;
				GUILayout.Label(VoiceChatRecorder.Instance.IsTransmitting ? "Transmitting" : "Silent");
			}*/
		}
	} 
}