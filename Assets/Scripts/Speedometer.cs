using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Speedometer : NetworkBehaviour {

	private float curSpd = 0, desSpd = 0;
	private bool ICASCtrl = false;
	public GameObject CurFirstDigit, CurSecondDigit, DesFirstDigit, DesSecondDigit, Car, GreenIndicator, RedIndicator;
	private Digit d_CurFirstDigit, d_CurSecondDigit, d_DesFirstDigit, d_DesSecondDigit;
	void Start(){
		d_CurFirstDigit = CurFirstDigit.GetComponent<Digit> ();
		d_CurSecondDigit = CurSecondDigit.GetComponent<Digit> ();
		d_DesFirstDigit = DesFirstDigit.GetComponent<Digit> ();
		d_DesSecondDigit = DesSecondDigit.GetComponent<Digit> ();
		d_CurFirstDigit.displayZero = true;
		RedIndicator.SetActive (false);
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
}
