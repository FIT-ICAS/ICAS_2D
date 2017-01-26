using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour {
	
	private Rigidbody2D carRig;

	private Text currentVelocity;

	void Start(){
		carRig = GetComponent<Rigidbody2D> ();
		currentVelocity = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		currentVelocity.text = carRig.velocity.magnitude + " kpH";
	}


}
