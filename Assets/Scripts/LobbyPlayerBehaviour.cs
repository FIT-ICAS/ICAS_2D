using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyPlayerBehaviour : NetworkBehaviour {


	[SerializeField] 
	private Camera thisCam;

	[SerializeField]
	private AudioListener thisAudioListener;

	public GameObject thisCanv;

	void Update()
	{
		if (!isLocalPlayer) {

			thisCam.enabled = false;
			thisAudioListener.enabled = false;
			thisCanv.SetActive (false);
			return;
		}
	}
}
