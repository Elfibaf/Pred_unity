using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.Networking;
using UnityEngine.UI;


public class ReceiveSpire : NetworkBehaviour {

	[SyncVar(hook="OnChangeBreathing")]
	public float breathingRythm;

	public string accesToken = "41d2fadfdc8bf540f4dfe16f1c15e1ddc8db91b2469aaee5cd95fa550c6e4e5e";
	public float delta = 15.0f;


	// Use this for initialization
	void Start () {
		if (!isLocalPlayer) {
			return;
		}
		InvokeRepeating ("GetBreathingRythm", 0.0f, delta);
	}

	public bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
		bool isOk = true;
		// If there are errors in the certificate chain, look at each error to determine the cause.
		if (sslPolicyErrors != SslPolicyErrors.None) {
			for(int i=0; i<chain.ChainStatus.Length; i++) {
				if(chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown) {
					chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
					chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
					chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
					chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
					bool chainIsValid = chain.Build((X509Certificate2)certificate);
					if(!chainIsValid) {
						isOk = false;
					}
				}
			}
		}
		return isOk;
	}


	void GetBreathingRythm() {
		using (WebClient wc = new WebClient())
		{
			ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
			wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler (DownloadStringCallback2);
			wc.DownloadStringAsync(new Uri("https://app.spire.io/api/v2/streaks?access_token=" + accesToken));
		}
	}

	private void DownloadStringCallback2 (object sender, DownloadStringCompletedEventArgs e)
	{
		// If the request was not canceled and did not throw
		// an exception, display the resource.
		if (!e.Cancelled && e.Error == null)
		{
			var json = (string)e.Result;
			var objects = JArray.Parse(json); // parse as array
			foreach (JObject breathe in objects) {
				string type = breathe.Value<string> ("type");
				if ((type == "calm") || (type == "sedentary") || (type == "focus")) {
					CmdChangeBreathing(breathe.Value<float> ("sub_value") + UnityEngine.Random.Range(0.0f,1.0f));
					//CmdChangeBreathing(UnityEngine.Random.Range(6.0f,24.0f));
					CmdChangeAgitation (breathingRythm, 6.0f, 24.0f);
					break;
				}
			}
		}
	}


	void OnChangeBreathing(float breathing) {

		breathingRythm = breathing;
		// updating breathing value on the UI
		if (GameObject.Find ("Breathing") != null) {
			GameObject.Find("Breathing").GetComponent<Text>().text = breathing.ToString();
		}
		print (breathingRythm);
	}

	[Command]
	void CmdChangeBreathing(float breathing) {
		breathingRythm = breathing;

	}

	// Normalizes the breathing between 0-1 and set new agitation
	[Command]
	void CmdChangeAgitation(float value, float min, float max)
	{
		float newAgitation = (float)Math.Round(((value - min) / (max - min)),2);
		//float newAgitation = ((value - min) / (max - min));
		print ("New Agitation" + newAgitation.ToString());
		if (GameObject.FindGameObjectWithTag ("Patient") != null) {
			GameObject.FindGameObjectWithTag ("Patient").GetComponent<MapBehaviour> ().agitation = newAgitation;
			print (GameObject.FindGameObjectWithTag ("Patient").GetComponent<MapBehaviour> ().agitation);
		}
	}



}
