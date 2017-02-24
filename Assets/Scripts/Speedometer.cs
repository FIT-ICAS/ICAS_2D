using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Speedometer : NetworkBehaviour {
	
	public GameObject ICASObject;
	private float curSpd = 0, desSpd = 0;
	private bool ICASCtrl = false;
	public GameObject CurFirstDigit, CurSecondDigit, DesFirstDigit, DesSecondDigit, Car, GreenIndicator, RedIndicator;
	private Digit d_CurFirstDigit, d_CurSecondDigit, d_DesFirstDigit, d_DesSecondDigit;
	private bool leftBlinker = false, rightBlinker = false, leftOn = false, rightOn = false;
	public GameObject lBlinker, rBlinker;

	void Start(){
		d_CurFirstDigit = CurFirstDigit.GetComponent<Digit> ();
		d_CurSecondDigit = CurSecondDigit.GetComponent<Digit> ();
		d_DesFirstDigit = DesFirstDigit.GetComponent<Digit> ();
		d_DesSecondDigit = DesSecondDigit.GetComponent<Digit> ();
		d_CurFirstDigit.displayZero = true;
		RedIndicator.SetActive (false);
		renderBlinker (lBlinker, leftOn);
		renderBlinker (rBlinker, rightOn);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if(!isLocalPlayer)
			return;

		curSpd = Car.GetComponent<Rigidbody2D> ().velocity.magnitude*3.6f;
		//Debug.Log (curSpd);
		DisplayCurSpeed (curSpd);
		if (ICASCtrl) {
			RedIndicator.SetActive (true);
			GreenIndicator.SetActive (false);
		} else {
			RedIndicator.SetActive (false);
			GreenIndicator.SetActive (true);
		}
	}

	void Update(){
		if (Input.GetKeyDown ("Q")) {
			leftBlink ();
		}
		if (Input.GetKeyDown ("E")) {
			rightBlink ();
		}
	}

	void DisplayCurSpeed(float spd){
		int first = (int)spd % 10;
		Debug.Log (first);
		int second = (int)(spd / 10) % 10;
		d_CurFirstDigit.number = first;
		d_CurSecondDigit.number = second;
	}
	void DisplayDesSpeed(float spd){
		int first = (int)spd % 10;
		int second = (int)(spd / 10) % 10;
		d_DesFirstDigit.number = first;
		d_DesSecondDigit.number = second;
	}
<<<<<<< HEAD
	void leftBlink(){
		leftBlinker = leftBlinker ^ true;
		rightBlinker = false;
		LeftBlinker ();
	}
	void rightBlink(){
		rightBlinker = rightBlinker ^ true;
		leftBlinker = false;
		RightBlinker ();
	}
	void renderBlinker(GameObject blinker, bool state){
		blinker.SetActive (state);
	}
	IEnumerator LeftBlinker(){
		while (leftBlinker) {
			leftOn = leftOn ^ true;
			renderBlinker (lBlinker, leftOn);
			yield return null;
		}
		leftOn = false;
		renderBlinker (lBlinker, leftOn);
	}

	IEnumerator RightBlinker(){
		while (rightBlinker) {
			rightOn = rightOn ^ true;
			renderBlinker (rBlinker, rightOn);
			yield return null;
		}
		rightOn = false;
		renderBlinker (rBlinker, rightOn);
	}
=======



>>>>>>> origin/Hud
}
