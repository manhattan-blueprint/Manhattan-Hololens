//Attach this script to a GameObject for debugging.
//Create a Text GameObject (Create>UI>Text) and attach it to the My Text field in the Inspector of your GameObject
//Press the space bar in Play Mode to see the Text change.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class UDPtransport : MonoBehaviour {

	// Debug text on screen.
  public Text debugText;

	// Network variables.
	private const int MAX_CONNECTION = 2;

	private int port = 8888;

	private int hostId;
	private int webHostId;

	private int reliableChannel;
	private int unreliableChannel;

	private bool isStarted = false;
	private byte error;

	// Network variables.
  ConnectionConfig config;
	HostTopology topology;

	void Start () {
    // Initializing the Transport Layer with no arguments (default settings)
    NetworkTransport.Init();

    // Initialize connection configuration and add a channel
    config = new ConnectionConfig();
    int reliableChannelID  = config.AddChannel(QosType.Reliable);
    int unreliableChannelID  = config.AddChannel(QosType.Unreliable);

		// Maximum of 2 connections
		topology = new HostTopology(config, MAX_CONNECTION);

		// Get a host ID
		hostId = NetworkTransport.AddHost(topology, 8888);

		isStarted = true;

    // Set text of info text.
    debugText.text = "Debug text.";
		debugText.color = new Color(255f, 255f, 150f);
		debugText.fontSize = 14;
		// debugText.transform.position = new Vector3(50, 50, 50);
  }

	void Update() {
		//Press the space key to change the Text message
		if (Input.GetKey(KeyCode.Space))
		{
		    debugText.text = "Text has changed.";
		}
	}
}
