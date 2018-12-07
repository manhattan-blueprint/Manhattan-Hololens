//Attach this script to a GameObject for debugging.
//Create a Text GameObject (Create>UI>Text) and attach it to the My Text field in the Inspector of your GameObject
//Press the space bar in Play Mode to see the Text change.

using UnityEngine;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;
using System.Threading;


public class UDPclient : MonoBehaviour {
    private string IPAddress;

	ConnectionConfig config;
	HostTopology topology;

    Thread serverThread;
    Thread clientThread;

    AsynchronousSocketListener listener = new AsynchronousSocketListener();
    AsynchronousClient client = new AsynchronousClient();

    private string LocalIPAdress = (new LocalIP()).Address();
    private int Port = (new LocalIP()).Port();

    public void Start() {
        Debug.Log("World initialized with both client and server on IP "
            + LocalIPAdress + " through port " + Port);

        serverThread = new Thread(new ThreadStart(listener.StartListening));
        serverThread.Start();

        //clientThread = new Thread(new ThreadStart(client.StartClient));
        //clientThread.Start();
    }

    //This is the function that serializes the message before sending it
    void sendMessage(string textInput) {

    }

    private void Update() {
    }
}
