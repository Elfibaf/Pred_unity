using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Ports;
using System;

public class ReceiveMindwave : MonoBehaviour {

	public string port;
	private SerialPort sp;
	private float meditationValue;


	// Use this for initialization
	void Start () {
		
		Debug.Log ("coucou");
		sp = new SerialPort (port, 115200);
		
		
	}
		
	// Update is called once per frame
	void Update () {

		/*List<int> bytes = new List<int> ();
		bytes.Add (8);
		bytes.Add (5);
		bytes.Add (3);
		bytes.Add (4);
		bytes.Add (2);
		bytes.Add (7);
		bytes.Add (9);*/

		int packet_code;
		int packet_length;
		int vlength;

		byte[] buffer = new byte[1000];
		sp.Read (buffer, 0, 1000);
		System.Collections.IEnumerator bytes = buffer.GetEnumerator ();

		// Possible besoin de parser les bytes pour les conditions, à voir pendant les tests.

		while (true) {
			if (bytes.MoveNext ()) {
				if ((int)bytes.Current == 0xaa) {
					if (bytes.MoveNext ()) {
						if ((int)bytes.Current == 0xaa) {
							// packet synced by 0xaa 0xaa
							if (bytes.MoveNext ()) {
								packet_length = (int)bytes.Current;
							} else {
								break;
							}
							if (bytes.MoveNext ()) {
								packet_code = (int)bytes.Current;
							} else {
								break;
							}
							if ((packet_code != 0xd4) && (packet_code != 0xd0)) { // Sending data
								int left = packet_length - 2;

								while (left > 0) {
									if (packet_code == 0x80) { // Raw value
										if ((bytes.MoveNext ()) && (bytes.MoveNext ()) && (bytes.MoveNext ())) {
											left -= 2;
										} else {
											break;
										}
									} else if (packet_code == 0x05) { // MEDITATION
										// TODO : UNPACK VALUE 
										if (bytes.MoveNext ()) {
											meditationValue = (float)bytes.Current;
										} else {
											break;
										}
										left -= 1;
									} else if (packet_code == 0x83) {
										if (bytes.MoveNext ()) {
											vlength = (int)bytes.Current;
										} else {
											break;
										}
										for (int i = 0; i < 24; i++) {
											bytes.MoveNext ();
										}
										left -= vlength;
									} 
									else {
										if (bytes.MoveNext ()) {
											left -= 1;
										} else {
											break;
										}
									}
									if (bytes.MoveNext ()) {
										packet_code = (int)bytes.Current;
									} else {
										break;
									}

								}
							}
						}
					} else {
						break;
					}

				} 
			}
			else {
				break;
			}

		}

		Debug.Log ("fini");

	}

	/*public static void Main() {

		ReceiveMindwave coucou = new ReceiveMindwave();

		coucou.Start();
		coucou.Update();

	}*/

}

