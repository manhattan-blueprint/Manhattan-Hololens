//Attach this script to a GameObject for debugging.
//Create a Text GameObject (Create>UI>Text) and attach it to the My Text field in the Inspector of your GameObject
//Press the space bar in Play Mode to see the Text change.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class UDPclient : MonoBehaviour {

	// Debug text on screen.
	public Text debugText;

	// Network variables.
	private const int MAX_CONNECTION = 2;

	private int port = 8888;

	private int hostId;
	private int webHostId;
	private int connectionId;

	private int reliableChannel;
	private int unreliableChannel;

	private bool isConnected = false;
	private byte error;

	// Network variables.
	ConnectionConfig config;
	HostTopology topology;

	public void Connect() {
	}

	private void Start() {
		// Set text of info text.
		debugText.text = "Starting connection.";
		debugText.color = new Color(0.0f, 1.0f, 0.0f);
		debugText.fontSize = 14;

		// Initializing the Transport Layer with no arguments (default settings)
		NetworkTransport.Init();

		// Initialize connection configuration and add a channel
		config = new ConnectionConfig();
		int reliableChannelID  = config.AddChannel(QosType.Reliable);
		int unreliableChannelID  = config.AddChannel(QosType.Unreliable);

		// Maximum of 2 connections
		topology = new HostTopology(config, MAX_CONNECTION);

		// Get a host ID (I.P. address?)
		hostId = NetworkTransport.AddHost(topology, 8888);
		connectionId = NetworkTransport.Connect(hostId, "LOCALHOST", port, 0, out error);

		debugText.text = "Connected.";

		isConnected = true;
	}

	private void Update() {
		if(!isStarted) {
			return;
		}

		int recHostId;
		int connectionId;
		int channelId;
		byte[] recBuffer = new byte[1024];
		int bufferSize = 1024;
		int dataSize;
		byte error;
		NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);

		switch (recData)
		{
			case NetworkEventType.Nothing:					// No data received.
				break;
			case NetworkEventType.ConnectEvent:			// Someone has connected.
				Debug.Log("Connection: " + connectionId + " has connected.")
				break;
			case NetworkEventType.DataEvent:				// Useful data.
				break;
			case NetworkEventType.DisconnectEvent:	// Someone has disconnected.
				break;
			case NetworkEventType.BroadcastEvent:		// ?
				break;
		}


		//Press the space key to change the Text message
		if (Input.GetKey(KeyCode.Space))
		{
		    debugText.text = "Text has changed.";
		}
	}
}
