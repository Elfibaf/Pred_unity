using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class ReceiveSpire : MonoBehaviour {
	

	public string accesToken = "41d2fadfdc8bf540f4dfe16f1c15e1ddc8db91b2469aaee5cd95fa550c6e4e5e";
	public float breathingRythm;


	// Use this for initialization
	void Start () {
		using (WebClient wc = new WebClient())
		{
			ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
			var json = wc.DownloadString("https://app.spire.io/api/v2/streaks?access_token=" + accesToken);
			//Breathe breathe = JsonConvert.DeserializeObject<Breathe> (json);
			var objects = JArray.Parse(json); // parse as array
			foreach (JObject breathe in objects) {
				string type = breathe.Value<string> ("type");
				if ((type == "calm") || (type == "sedentary") || (type == "focus")) {
					breathingRythm = breathe.Value<float> ("sub_value");
					print (breathingRythm);
					break;
				}
			}

		}
	}
	
	// Update is called once per frame
	void Update () {
		
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
}
