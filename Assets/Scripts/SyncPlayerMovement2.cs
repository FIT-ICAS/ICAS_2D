using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SyncPlayerMovement2 : NetworkBehaviour {

	//private float lerpRate = 30f;
	[SerializeField] 
	private Camera thisCam;

	[SerializeField]
	private AudioListener thisAudioListener;

	[SerializeField]
	private Rigidbody2D carRigidBody;

	[Command]
	void CmdLerpPositionAndRotation(Vector3 pos, Quaternion rot){ // on server machine each object runs this script
		
		transform.position = pos;// + (Vector3)vel*(t);//Vector3.Lerp (transform.position, pos, lerpRate * Time.deltaTime); // the object on the server lerps to position
		transform.rotation = rot;//Quaternion.Lerp (transform.rotation, rot, lerpRate * Time.deltaTime);
		RpcGiveBackNewPositionAndRotation (transform.position, transform.rotation); //
	}
	[ClientCallback]
	void FixedUpdate(){
		CmdLerpPositionAndRotation (transform.position, transform.rotation);

	}
	[ClientRpc]
	void RpcGiveBackNewPositionAndRotation(Vector3 pos, Quaternion rot){ // on client machine each object runs this script
		if (!isLocalPlayer) {	// car clones run script but we want the non-local player object to update
			transform.position = pos;
			transform.rotation = rot;
			//carRigidBody.velocity = vel;
		}
		
	}

	void Update()
	{
		if (!isLocalPlayer) { // Don't change

			thisCam.enabled = false;
			thisAudioListener.enabled = false;
			carRigidBody.velocity = new Vector2 (0f, 0f);
			return;
		}
	}

}
