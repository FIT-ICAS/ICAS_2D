using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SyncPlayerMovement : NetworkBehaviour {

	private float lerpRate = 30f;
	[SerializeField] 
	private Camera thisCam;

	[SerializeField]
	private AudioListener thisAudioListener;

	[ClientRpc]
	void RpcLerpToPosAndRot(Vector3 pos, Quaternion rot) // This is correct, server not updating though
	{
		if (!isLocalPlayer) {
			this.transform.position = Vector3.Lerp (this.transform.position, pos, lerpRate * Time.deltaTime);
			this.transform.rotation = Quaternion.Lerp (this.transform.rotation, rot, lerpRate * Time.deltaTime);
		}
		//CmdGiveNewPositionAndRot (this.transform.position, this.transform.rotation);
	}

	[Command]
	void CmdGiveNewPositionAndRot( Vector3 pos, Quaternion rot){ // I want the client to give back the new position of the other player
		this.transform.position = Vector3.Lerp (this.transform.position, pos, lerpRate * Time.deltaTime);
		this.transform.rotation = Quaternion.Lerp (this.transform.rotation, rot, lerpRate * Time.deltaTime);
	}

	[ServerCallback]
	void FixedUpdate(){
		RpcLerpToPosAndRot (this.transform.position, this.transform.rotation); // give client server position of other player

	}

	void Update()
	{
		if (!isLocalPlayer) { // Don't change

			thisCam.enabled = false;
			thisAudioListener.enabled = false;
			return;
		}
	}

}
