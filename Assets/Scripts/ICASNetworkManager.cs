using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ICASNetworkManager : MonoBehaviour{
	public static string networkAddress;
	public static int networkPort = 7777;
	public static NetworkClient client;
	private static bool isHost;
	public static bool isLocalPlayer = true;

	public string onlineScene;
	public string offlineScene;	
	//public int clientId = 0;				//clientId will be client.connection.connectionId and same for playerId
	//public short playerId = 0;
	private int onlineSceneIndexMsg = 1001;
	//private short SpawnSpectatorMsg = 1002;
	//private short SpawnPlayerMsg = 1003;
	public short clientMsgAddP = 1004; // if using ClientMessage for a custome message type
	public short clientMsgRemP = 1005;
	// known accross

	public GameObject LobbyPlayerPrefab;		// mainly the positions which don't change so known accross
	public GameObject SpectatorPlayerPrefab;
	public List<GameObject> playerPrefabs; // these may or may not be suitable
	public List<GameObject> playerSpawns;
	private List<NetworkConnection> clients;
	public GameObject SpectatorSpawn;
	public GameObject LobbyPlayerSpawn;


	void Awake(){
		DontDestroyOnLoad (transform); // keep the manager
		DontDestroyOnLoad(this);
	}

	void Start(){
		//SceneManager.LoadScene (offlineScene.name);
		NetworkServer.RegisterHandler(MsgType.AddPlayer, OnAddPlayerMessage);    // the messages will be ClientMessages but over the addplayer type
		NetworkServer.RegisterHandler (MsgType.RemovePlayer, OnRemovePlayerMessage);
		NetworkServer.RegisterHandler (MsgType.NotReady, OnClientUnreadyMsg);
		NetworkServer.RegisterHandler (MsgType.Ready, OnClientReadyMsg);
		NetworkServer.RegisterHandler (MsgType.Connect, OnServerConnect);
		ClientScene.RegisterPrefab (LobbyPlayerPrefab);

	}




	public void OnConnected(NetworkMessage msg){ // gets called when the user make a host
		Debug.Log ("Connected to server");
		//StartCoroutine (LoadSceneCor (onlineScene));
		//client.RegisterHandler ((short)onlineSceneIndexMsg, LoadOnlineScene); // register for future message of scene name
//		ClientMessage newMsg = new ClientMessage(); // connection just came in on this port
//		newMsg.clientId = client.connection.connectionId;
//		newMsg.connection = client.connection;
		//IntegerMessage newMsg = new IntegerMessage(client.connection.connectionId);
		//playerId = (short)client.connection.connectionId;//(short)client.connection.connectionId; // this is on the client side Remember playerId is also client Id
		//clientId++; // ? doesn't this need to be synced			This is on the clients machine which will start at 0!!!	<------------------------------			Maybe this and the one other above should be server

	}
		// server gets player Id											// I think server should also issue a new Id this is good for first connection but all other connections need to be at par with server

	// So here is what needs to happen, the client is going to connect then send a message but it will not be client info. It's  going to be a request for an Id (ClientMessage may be unecessary)
	// Server will give the Id to the client then increment the Id. The client will assign itself the Id and also assign the playerId with the new clientId. The server (host really) will always be client 0 player 0.
	public void OnServerConnect(NetworkMessage msg){
//		ClientMessage cMsg = msg.ReadMessage<ClientMessage> ();
//		clients.Add (cMsg.connection);
		StringMessage newmsg = new StringMessage (onlineScene); // give client the scene to load
		NetworkServer.SendToClient (msg.conn.connectionId, (short)onlineSceneIndexMsg, newmsg);

	}

	public void LoadOnlineScene(NetworkMessage msg){
		Debug.Log ("sadfsdaf");
		string scene = msg.ReadMessage<StringMessage>().value;
		StartCoroutine (LoadSceneCor (scene));

	}



	public void Host(){
		NetworkServer.Listen (networkPort);
		client = new NetworkClient ();
		//client.RegisterHandler (MsgType.Connect, OnConnected);
		//client = ClientScene.ConnectLocalServer();
		client.RegisterHandler ((short)onlineSceneIndexMsg, LoadOnlineScene); // register for future message of scene name
		client.Connect ("localhost", networkPort);
		//SceneManager.LoadScene (onlineScene);
		//LobbyPlayerSpawn = GameObject.FindGameObjectWithTag ("Lobby");

		//Instantiate (LobbyPlayerPrefab, LobbyPlayerSpawn.transform.position, LobbyPlayerSpawn.transform.rotation);
		//client.RegisterHandler (MsgType.Connect, OnConnected);
		isHost = true;
		//ClientScene.AddPlayer (0);
		//client = StartHost();
		//ClientAddPlayer ();

	}

	public void SetClientAddress(Text txt){
		networkAddress = txt.text;
	}

	public void SetClientPort (int port){
		networkPort = port;
	}

	public void ConnectToClient(){
		client = new NetworkClient ();
		//client.RegisterHandler (MsgType.Connect, OnConnected);
		NetworkServer.RegisterHandler ((short)networkPort, OnServerConnect); // for when anyone connects over the port
		client.Connect (networkAddress, networkPort);
		//ClientAddPlayer (); // this may be a bad spot

	}
	public void SpawnSpectator(GameObject prefab){
		NetworkServer.ReplacePlayerForConnection (client.connection, prefab, 0); // this will need a message
	}
		
	public void Stop(){
		Debug.Log (isHost);
		//Unready ();
		if (isHost) { // server side
			//ClientScene.RemovePlayer (0);
			//NetworkServer.Shutdown ();
			//client.Disconnect ();
			isHost = false;
			NetworkServer.Shutdown ();
		} else {
			//client.Disconnect (); // client
		}
		NetworkClient.ShutdownAll();
		SceneManager.LoadScene (offlineScene);
	}

	//server side
	public void OnAddPlayerMessage(NetworkMessage netMsg ){ // this will be the lobby player then for all other player spawns the player object will be removed and the player for connection will be replaced

		//int id = netMsg.ReadMessage<IntegerMessage> ().value;

		//NetworkConnection conn = NetworkServer.connections [id]; // network connections is read-only but is indexed based on connectionId

		LobbyPlayerSpawn = GameObject.FindGameObjectWithTag ("Lobby");
		GameObject player = Instantiate (LobbyPlayerPrefab, LobbyPlayerSpawn.transform.position, LobbyPlayerSpawn.transform.rotation) as GameObject;
		//GameObject player = new GameObject();
		//player = LobbyPlayerPrefab;

		//Debug.Log (player.name);

		//NetworkServer.Spawn (player);
		NetworkServer.AddPlayerForConnection (netMsg.conn, LobbyPlayerPrefab, 0); // found out that playerId is always zero less we are split screening
		if (isHost) {
			player.SetActive (false);
		}

	}


	//server side
	void OnRemovePlayerMessage(NetworkMessage netMsg){ 
		int id = netMsg.ReadMessage<IntegerMessage> ().value;
		NetworkConnection connection = NetworkServer.connections [id];
		NetworkServer.DestroyPlayersForConnection (connection);
	}


	void ClientAddPlayer(){
		IntegerMessage msg = new IntegerMessage (client.connection.connectionId);
		client.Send (MsgType.AddPlayer, msg);
	}

	void ClientRemovePlayer(){
		IntegerMessage msg = new IntegerMessage (client.connection.connectionId);
		client.Send (MsgType.RemovePlayer, msg);
	}

	IEnumerator LoadSceneCor(string name){
		AsyncOperation async = SceneManager.LoadSceneAsync (name);
		while (!async.isDone) {
			yield return null;
		}
		//ClientScene.Ready (client.connection); // ok so this fixed a problem with the playerPrefab was nullreference

		Ready();
		ClientAddPlayer ();
	}

	public void OnClientUnreadyMsg(NetworkMessage msg){
		NetworkServer.SetClientNotReady (msg.conn);
	}
	public void OnClientReadyMsg(NetworkMessage msg){
		NetworkServer.SetClientReady (msg.conn);
	}

	public void Unready(){
		IntegerMessage msg = new IntegerMessage ();
		client.Send (MsgType.NotReady, msg);
	}

	public void Ready(){
		IntegerMessage msg = new IntegerMessage ();
		client.Send (MsgType.Ready, msg);
	}
}

