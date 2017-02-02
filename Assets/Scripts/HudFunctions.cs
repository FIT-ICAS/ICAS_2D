using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking.NetworkSystem;
public class HudFunctions : MonoBehaviour{

	//public static NetworkManager mgmt;
	public static SpawnMgmt spwn;			// may not actually need this
	private static NetworkClient client;
	private static string netAddr;
	private static int netPort = 7777;
	private static string offline = "Start Menu";
	private static string online = "Simulation";
	private static short spectate = 1001;
	private static short play = 1002;

	//private static bool isHost = false;
	void Awake(){
		DontDestroyOnLoad (transform);
	}
	void Start(){
		//mgmt = GetComponent<NetworkManager> ();
		spwn = GetComponent<SpawnMgmt> ();
		NetworkServer.RegisterHandler (MsgType.AddPlayer, OnAddPlayerMsg);
		NetworkServer.RegisterHandler (spectate, OnSpectateMsg);
		NetworkServer.RegisterHandler (play, OnPlayMessage);

	}

	public new void Host(){
		//client = mgmt.StartHost ();
		NetworkServer.Listen (netPort);
		client = ClientScene.ConnectLocalServer ();
		//client = new NetworkClient();
		//client.Connect ("localhost", netPort);
		StartCoroutine (C_LoadScene (online));
		//isHost = true;
	}

	public void SetClientAddress(Text txt){
		netAddr = txt.text;
	}
		

	public void ConnectToClient(){
		client = new NetworkClient ();
		client.Connect (netAddr, netPort);
		StartCoroutine (C_LoadScene (online));

	}

	public void SpawnSpectator(){
		ClientScene.RemovePlayer ((short)client.connection.connectionId);
		Spectate ();
	}

	public void SpawnPlayer(){
		ClientScene.RemovePlayer ((short)client.connection.connectionId);
		Spawn ();
	}

	public void Stop(){
		client.Shutdown();
		SceneManager.LoadScene (offline);
	}

	IEnumerator C_LoadScene(string name){
		AsyncOperation async = SceneManager.LoadSceneAsync (name);
		while (!async.isDone) {
			yield return null;
		}
		Debug.Log ("Scene loaded");
		Debug.Log (NetworkServer.localConnections.Count);
		ClientScene.AddPlayer (client.connection, (short)client.connection.connectionId);
	}

	public void OnAddPlayerMsg(NetworkMessage msg){

		GameObject lobby = GameObject.Find ("SpectatorSpawn");
		GameObject player = Instantiate (spwn.lobbyPrefab, lobby.transform.position, lobby.transform.rotation);
		player.name = player.name + msg.conn.connectionId;
		NetworkServer.AddPlayerForConnection (msg.conn, player, (short)msg.conn.connectionId);
	}

	public void OnSpectateMsg(NetworkMessage msg){
		GameObject specSpwn = GameObject.Find ("SpectatorSpawn");
		GameObject spec = (GameObject)Instantiate (spwn.specPrefab);
		spec.transform.position = specSpwn.transform.position;
		spec.transform.rotation = specSpwn.transform.rotation;
		NetworkServer.ReplacePlayerForConnection (msg.conn, spec, (short)msg.conn.connectionId);
	}

	public void OnPlayMessage(NetworkMessage msg){
		GameObject[] spawns = GameObject.FindGameObjectsWithTag ("Respawn");
		GameObject mySpawn = spawns [Random.Range (0, spawns.Length)];
		GameObject player = Instantiate (spwn.playerPrefab, mySpawn.transform.position, mySpawn.transform.rotation);
		player.name = player.name + msg.conn.connectionId;
		NetworkServer.ReplacePlayerForConnection (msg.conn, player, (short)msg.conn.connectionId);
	}

	public void Spectate(){
		IntegerMessage msg = new IntegerMessage ();
		client.Send (spectate, msg);
	}

	public void Spawn(){
		IntegerMessage msg = new IntegerMessage ();
		client.Send (play, msg);
	}
}
