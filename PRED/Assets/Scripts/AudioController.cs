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
		clipArray = Resources.LoadAll ("Audio");
		whiteNoiseSource = GetComponent<AudioSource> ();
		whiteNoise = (AudioClip)clipArray [0];
		whiteNoiseSource.clip = whiteNoise;
		whiteNoiseSource.Play ();
	}

	[ClientRpc]
	public void RpcPlaySoundFromButton(int soundNum)
	{
		whiteNoise = (AudioClip)clipArray [soundNum];
		whiteNoiseSource.clip = whiteNoise;
		whiteNoiseSource.Play ();
	}
}
