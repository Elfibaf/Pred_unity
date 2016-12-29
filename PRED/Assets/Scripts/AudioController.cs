using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class AudioController : NetworkBehaviour {
	private AudioSource whiteNoiseSource;
	private AudioClip whiteNoise;

	void Start()
	{
		whiteNoiseSource = GetComponent<AudioSource> ();
		whiteNoise = whiteNoiseSource.clip;
		print(whiteNoise);
	}

	[ClientRpc]
	public void RpcPlaySound(string soundName)
	{
		whiteNoise = Resources.Load<AudioClip> ("Audio/" + soundName);
		whiteNoiseSource.clip = whiteNoise;
		print (whiteNoise);
		whiteNoiseSource.Play ();
	}
}
