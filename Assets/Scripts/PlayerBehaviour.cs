using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerBehaviour : MonoBehaviour {

	/*
	[SyncVar]
	private Vector3 playerPos;

	[SyncVar]
	private Quaternion playerRot;
	*/

    void Update()
	{
		
		// Drew, this is what you would change {										<-------------- Change

		var x = Input.GetAxis ("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis ("Vertical") * Time.deltaTime * 10.0f;

		transform.Rotate (0, 0, -x); // these you might keep; not sure, but remember this: if you are going to rotate it must be in the negative z axis or nagate the input (I 	got funny results with posative axis)
        transform.Translate(0, z, 0); // this is fine, change it or don't

		// }		end of change


    }


}
