using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;
using VoiceChat.Networking;
using VoiceChat.Demo;


public class NetworkManagerCustom : NetworkManager {

	public int chosenCharacter = 0;


	//subclass for sending network messages
	public class NetworkMessage : MessageBase {
		public int chosenClass;
	}

    // triggered only on the server app
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader) {

		// Read client message and receive index
		var stream = extraMessageReader.ReadMessage<IntegerMessage> ();
		chosenCharacter = stream.value;

		//Select the prefab from the spawnable objects list
		var playerPrefab = spawnPrefabs[chosenCharacter];

        if (chosenCharacter == 1) print("patient instanciation");
        if (chosenCharacter == 0) print("therapist instanciation");

		// Create player object with prefab
		var player = Instantiate(playerPrefab, playerPrefab.transform.position, Quaternion.identity) as GameObject;        

		// Add player object for connection
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}

	public override void OnClientConnect(NetworkConnection conn) { // triggered when client is connecting (on Server or Client app)
		// Create message to set the player
		IntegerMessage msg = new IntegerMessage(chosenCharacter);

        if (chosenCharacter == 1) print("patient connection");
        if (chosenCharacter == 0) print("therapist connection");

		// Call Add player and pass the message
		ClientScene.AddPlayer(conn, 0, msg);
		// VOICE CHAT
		VoiceChatNetworkProxy.OnManagerClientConnect(conn);
	}


	public override void OnClientSceneChanged(NetworkConnection conn) {
		var player = conn.playerControllers [0].gameObject;
		//VoiceChatNetworkProxy.OnManagerClientConnect(conn);
        print("scene changed !");
		if (player.tag == "Therapist") 
		{
			player.GetComponent<TherapistController> ().OnStartLocalPlayer ();
		}
		else if (player.tag == "Patient") {
			print ("tag = patient");
			player.GetComponent<PatientController> ().OnStartLocalPlayer ();
		}
		if (!ClientScene.ready) {
			ClientScene.Ready(conn);
		}

		print(ClientScene.ready);
	
	}

	/*public override void OnServerSceneChanged(string sceneName) {
		
	}*/

	// Therapist's button
	public void StartupHost()
	{
		SetPort ();
		NetworkManager.singleton.StartHost ();
		chosenCharacter = 0;
	}

	// Patient's button
	public void JoinGame()
	{
		SetIPAddress ();
		SetPort ();
		NetworkManager.singleton.StartClient ();
		chosenCharacter = 1;
	}

	void SetIPAddress()
	{
		string ipAdress = GameObject.Find ("InputFieldIPAddress").transform.FindChild ("Text").GetComponent<Text> ().text;
        NetworkManager.singleton.networkAddress = ipAdress;
        print(ipAdress);
	}

	void SetPort()
	{
		NetworkManager.singleton.networkPort = 7777;
	}

	void OnLevelWasLoaded(int level)
	{
		if (level == 0) {
			SetupMenuSceneButtons ();
		} 
		else 
		{
			SetupOtherSceneButtons ();
		}
	}

	// To recreate button listeners when scene is changed
	void SetupMenuSceneButtons() 
	{
		GameObject.Find ("ButtonStartHost").GetComponent<Button>().onClick.RemoveAllListeners ();
		GameObject.Find ("ButtonStartHost").GetComponent<Button>().onClick.AddListener (StartupHost);

		GameObject.Find ("ButtonJoinGame").GetComponent<Button>().onClick.RemoveAllListeners ();
		GameObject.Find ("ButtonJoinGame").GetComponent<Button>().onClick.AddListener (JoinGame);
	}

	void SetupOtherSceneButtons()
	{	
		/*GameObject.Find ("ButtonDisconnect").GetComponent<Button>().onClick.RemoveAllListeners ();
		GameObject.Find ("ButtonDisconnect").GetComponent<Button>().onClick.AddListener (NetworkManager.singleton.StopHost);*/
	}

	/***** VOICE CHAT *****/

	public override void OnStartClient(NetworkClient client)
	{

		VoiceChatNetworkProxy.OnManagerStartClient (client);

		//gameObject.AddComponent<VoiceChatUi> ();
	
	}

	public override void OnStopClient()
	{
		//VoiceChatNetworkProxy.OnManagerStopClient();

		//if (client != null)
		//	Destroy(GetComponent<VoiceChatUi>());
	}

	public override void OnServerDisconnect(NetworkConnection conn)
	{
		base.OnServerDisconnect(conn);

		//VoiceChatNetworkProxy.OnManagerServerDisconnect(conn);
	}

	public override void OnStartServer()
	{
		VoiceChatNetworkProxy.OnManagerStartServer();

		gameObject.AddComponent<VoiceChatServerUi>();

		gameObject.AddComponent<VoiceChatUi> ();

		gameObject.AddComponent<TherapistUI> ();
	}

	public override void OnStopServer()
	{
		VoiceChatNetworkProxy.OnManagerStopServer();

		Destroy(GetComponent<VoiceChatServerUi>());

		Destroy (GetComponent<TherapistUI> ());

		if (client != null)
			Destroy(GetComponent<VoiceChatUi>());
	}



}
