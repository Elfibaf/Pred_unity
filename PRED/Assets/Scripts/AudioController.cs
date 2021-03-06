﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class AudioController : NetworkBehaviour {
	private AudioSource whiteNoiseSource;
	private AudioClip whiteNoise;
	private Object[] clipArray;
	private GameObject patient;



	void Start()
	{
		patient = GameObject.FindGameObjectWithTag ("Patient");
		whiteNoiseSource = GetComponent<AudioSource> ();
		clipArray = patient.GetComponent<PatientController> ().whiteNoiseArray;
		// Triggers the chosen white noise when entering Ganzfeld phase
        if (patient.GetComponent<PatientController>().chosenSound != -1 && !GameObject.FindGameObjectWithTag("Therapist").GetComponent<TherapistController>().fromGanzfeld)
        {
			whiteNoise = (AudioClip)clipArray [patient.GetComponent<PatientController>().chosenSound];
			whiteNoiseSource.clip = whiteNoise;
			whiteNoiseSource.Play ();
		}
	}

	// Called by the patientController when audio is loaded
	public void SetClipArray(Object[] patientClipArray){
		clipArray = patientClipArray;
	}

	/*[ClientRpc]
	public void RpcPlaySoundFromButton(int soundNum)
	{
		whiteNoise = (AudioClip)clipArray [soundNum];
		whiteNoiseSource.clip = whiteNoise;
		whiteNoiseSource.Play ();
	}

	[Command]
	public void CmdPlaySound(int numButton) {
		whiteNoiseSource.GetComponent<AudioController> ().RpcPlaySoundFromButton (numButton);
	}*/

	public void PlaySoundFromButton(int soundNum, float vol)
	{
		whiteNoiseSource.volume = vol;
		whiteNoise = (AudioClip)clipArray [soundNum];
		whiteNoiseSource.clip = whiteNoise;
		whiteNoiseSource.Play ();
	}

	public void changeVolume(float vol){
		whiteNoiseSource.volume = vol;
		print (vol);
	}
}
