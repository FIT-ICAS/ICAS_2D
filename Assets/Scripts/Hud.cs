using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Hud : NetworkBehaviour {

	[SerializeField]
	private Text currentVelocity;

	[SerializeField]
	private Rigidbody2D playerRig;


	
	// Update is called once per frame
	void FixedUpdate () {
		if (!isLocalPlayer)
			return;
		currentVelocity.text = playerRig.velocity.magnitude + "kpH";
	}



}
