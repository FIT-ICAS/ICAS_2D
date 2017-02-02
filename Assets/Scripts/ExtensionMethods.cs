using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class ExtensionMethods {
	public static void v_StartHost(this NetworkManager netMgmt){
		netMgmt.StartHost ();
	}

	public static void v_StartClient(this NetworkManager netMgmt, string address, int port){
		netMgmt.networkAddress = address;
		netMgmt.networkPort = port;
		netMgmt.StartClient ();
	}
}
