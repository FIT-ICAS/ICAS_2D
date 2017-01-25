using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerBehaviour : NetworkBehaviour {

	[SerializeField] 
	private Camera thisCam;

	[SerializeField]
	private AudioListener thisAudioListener;
	/*
	[SyncVar]
	private Vector3 playerPos;

	[SyncVar]
	private Quaternion playerRot;
	*/

	private float lerpRate = 15f;

    void Update()
	{
		
        if (!isLocalPlayer)
        {
			
			thisCam.enabled = false;
			thisAudioListener.enabled = false;
            return;
        }

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 10.0f;

        transform.Rotate(0, 0, -x);
        transform.Translate(0, z, 0);

    }
	/*
	[ClientCallback]
	void FixedUpdate(){
		if (isLocalPlayer) {
			CmdGetPosAndRot (this.transform.position, this.transform.rotation); // give the position
		}
		lerpToPosAndRot();
	}

	[Command]
	void CmdGetPosAndRot(Vector3 pos, Quaternion rot){
		playerPos = pos;
		playerRot = rot;

	}

	void lerpToPosAndRot(){
		if (!isLocalPlayer) {
			this.transform.position = Vector3.Lerp (this.transform.position, playerPos, lerpRate * Time.deltaTime);
			this.transform.rotation = Quaternion.Lerp (this.transform.rotation, playerRot, lerpRate * Time.deltaTime);
		}
	}

	*/

	[ClientRpc]
	void RpcLerpToPosAndRot(Vector3 pos, Quaternion rot) // This is correct, server not updating though
	{
		if (!isLocalPlayer) {
			this.transform.position = Vector3.Lerp (this.transform.position, pos, lerpRate * Time.deltaTime);
			this.transform.rotation = Quaternion.Lerp (this.transform.rotation, rot, lerpRate * Time.deltaTime);
		}
		CmdGiveNewPositionAndRot (this.transform.position, this.transform.rotation);
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



}
