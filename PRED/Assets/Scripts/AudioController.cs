using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class AudioController : NetworkBehaviour {
	private AudioSource whiteNoiseSource;
	private AudioClip whiteNoise;
	private Object[] clipArray;



	void Start()
	{
		clipArray = GameObject.FindGameObjectWithTag ("Patient").GetComponent<PatientController> ().whiteNoiseArray;
		whiteNoiseSource = GetComponent<AudioSource> ();
		whiteNoise = (AudioClip)clipArray [0];
		whiteNoiseSource.clip = whiteNoise;
		whiteNoiseSource.Play ();
	}

	[ClientRpc]
	public void RpcPlaySoundFromButton(int soundNum)
	{
		print ("RPC appelé");
		whiteNoise = (AudioClip)clipArray [soundNum];
		whiteNoiseSource.clip = whiteNoise;
		whiteNoiseSource.Play ();

		print ("Musique changée");
	}

	[Command]
	public void CmdPlaySound(int numButton) {
		print ("CMD appelé");
		whiteNoiseSource.GetComponent<AudioController> ().RpcPlaySoundFromButton (numButton);
	}
}
