using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

enum EventState {
    IDLE,                 // - Device socket not set yet, display IP on screen
    GREET,                // - Send greet message to phone until response,
                          // send connected message to Hololens once received.
    IDLE_IP               // - Connected, waiting for instructions
}

// State object for receiving data from remote device.
public class StateObject {
    public Socket workSocket = null;
    public const int BufferSize = 256; // Size of receive buffer.
    public byte[] buffer = new byte[BufferSize];
    public StringBuilder sb = new StringBuilder();
}

public class AsynchronousSocketListener {
    public ManualResetEvent allDone;
    public HoloInteractive holoInter;
    LocalIP localIP;
    private string localIPAddress;
    private int port;
    EventState eventState;

    private String greetMessage;
    private String connectedMessage;

    public AsynchronousSocketListener() {
        localIP = new LocalIP();
        allDone = new ManualResetEvent(false);
        localIPAddress = localIP.Address();
        port = localIP.Port();

        greetMessage = "hello_blueprint";
        connectedMessage = "connected_blueprint";
        eventState = EventState.IDLE;
    }

    public void StartListening() {
        Debug.Log("Server: initialized");
        IPAddress ipAddress = IPAddress.Parse(localIPAddress);
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

        // Create a TCP/IP socket.
        Socket listener = new Socket(ipAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp );

        // Bind the socket to the local endpoint and listen for incoming connections.
        try {
            listener.Bind(localEndPoint);
            listener.Listen(100);

            while (true) {
                // Set the event to nonsignaled state.
                allDone.Reset();

                // Start an asynchronous socket to listen for connections.
                Debug.Log("Server: open to connections...");
                listener.BeginAccept(
                    new AsyncCallback(AcceptCallback),
                    listener );

                // Wait until a connection is made before continuing.
                allDone.WaitOne();
            }

        } catch (Exception e) {
            Debug.Log("Server: " + e.ToString());
        }

				Debug.Log("Server: error caused exit.");
    }

    public void AcceptCallback(IAsyncResult ar) {
        // Signal the main thread to continue.
        allDone.Set();

        // Get the socket that handles the client request.
        Socket listener = (Socket) ar.AsyncState;
        Socket handler = listener.EndAccept(ar);

        // Create the state object.
        StateObject state = new StateObject();
        state.workSocket = handler;
        handler.BeginReceive( state.buffer, 0, StateObject.BufferSize, 0,
            new AsyncCallback(ReadCallback), state);
    }

    public void ReadCallback(IAsyncResult ar) {
        String content = String.Empty;

        // Retrieve the state object and the handler socket
        // from the asynchronous state object.
        StateObject state = (StateObject) ar.AsyncState;
        Socket handler = state.workSocket;

        // Read data from the client socket.
        int bytesRead = handler.EndReceive(ar);

        if (bytesRead > 0) {
            // There  might be more data, so store the data received so far.
            state.sb.Append(Encoding.ASCII.GetString(
                state.buffer, 0, bytesRead));

            // Check for end-of-file tag. If it is not there, read
            // more data.
            content = state.sb.ToString();

            HandleEventState(content, handler);
        }
    }

    private void HandleEventState(String content, Socket handler) {
        Debug.Log("Server: read " + content);
        switch (eventState) {
            case EventState.IDLE:
                if (string.Equals(content, greetMessage + "\n")) {
                    Send(handler, greetMessage);
                    Debug.Log("Server: swapping to state 'IDLE_IP'");
                    eventState = EventState.IDLE_IP;
                }
                break;
            case EventState.IDLE_IP:
                if (content[0] == 'I') {
                    Debug.Log("Server: received instruction");
                }
                else {
                    Debug.Log("Server: unexpected message received");
                }
                break;
            default:
                break;
        }
    }

    private void Send(Socket handler, String data) {
        Debug.Log("Server: sending " + data + " to client");

        // Convert the string data to byte data using ASCII encoding.
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        // Begin sending the data to the remote device.
        handler.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(SendCallback), handler);
    }

    private void SendCallback(IAsyncResult ar) {
        try {
            // Retrieve the socket from the state object.
            Socket handler = (Socket) ar.AsyncState;

            // Complete sending the data to the remote device.
            int bytesSent = handler.EndSend(ar);

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();

        } catch (Exception e) {
            Debug.Log("Server: " + e.ToString());
        }
    }
}
