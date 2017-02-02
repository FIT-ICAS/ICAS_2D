using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnMgmt : MonoBehaviour { // NetworkManager and SpawnManager both have the same assetId's for their prefabs
	public static NetworkManager mgmt;
	public GameObject lobbyPrefab, specPrefab, playerPrefab;
	public NetworkHash128 lobbyPlayerAssetId { get; set; }
	public NetworkHash128 spectatorPlayerAssetId{ get; set; }
	public NetworkHash128 playerAssetId { get; set; }

	public delegate GameObject SpawnDelegate (Vector3 pos, NetworkHash128 assetId);
	public delegate void UnspawnDelegate(GameObject spawned);
	// Use this for initialization
	void Start () {
		mgmt = GetComponent<NetworkManager> ();
		lobbyPlayerAssetId = lobbyPrefab.GetComponent<NetworkIdentity> ().assetId;
		spectatorPlayerAssetId = specPrefab.GetComponent<NetworkIdentity> ().assetId;
		playerAssetId = playerPrefab.GetComponent<NetworkIdentity> ().assetId;
		ClientScene.RegisterSpawnHandler (lobbyPlayerAssetId, SpawnLobby, UnspawnLobby);
		ClientScene.RegisterSpawnHandler (spectatorPlayerAssetId, SpawnSpec, UnspawnSpec);
		ClientScene.RegisterSpawnHandler (playerAssetId, SpawnPlayer, UnspawnPlayer);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	// this occurs client side not server side: server creates objects directly
	// I want to spawn in predefined locations mostly
	public GameObject SpawnLobby(Vector3 pos, NetworkHash128 assetId){ // must match delegate signitures above
		return (GameObject)Instantiate (lobbyPrefab, pos, Quaternion.identity);
	}
	public GameObject SpawnSpec(Vector3 pos, NetworkHash128 assetId){
		return (GameObject)Instantiate (lobbyPrefab, pos, Quaternion.identity);
	}
	public GameObject SpawnPlayer(Vector3 pos, NetworkHash128 assetId){
		return (GameObject)Instantiate (lobbyPrefab, pos, Quaternion.identity);
	}
	public void UnspawnLobby(GameObject spawned){ // remove like normal
		Destroy (spawned);
	}
	public void UnspawnSpec(GameObject spawned){
		Destroy (spawned);
	}
	public void UnspawnPlayer(GameObject spawned){
		Destroy (spawned);
	}
}
