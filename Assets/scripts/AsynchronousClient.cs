using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

public class AsynchronousClient {
    // The port number for the remote device.
    private int port;

    private string LocalIPAddress = (new LocalIP()).Address();

    // ManualResetEvent instances signal completion.
    private ManualResetEvent connectDone =
        new ManualResetEvent(false);
    private ManualResetEvent sendDone =
        new ManualResetEvent(false);
    private ManualResetEvent receiveDone =
        new ManualResetEvent(false);

    // The response from the remote device.
    private String response = String.Empty;

    public void StartClient() {
        // Connect to a remote device.
        Debug.Log("Client: start called.");
        try {
            // Establish the remote endpoint for the socket.
            // The name of the
            // remote device is "host.contoso.com".
            //IPHostEntry ipHostInfo = Dns.GetHostEntry("10.72.1.88");
            //IPAddress ipAddress    = ipHostInfo.AddressList[0];
            //IPEndPoint remoteEP    = new IPEndPoint(ipAddress, port);            
            //IPHostEntry ipHostInfo = Dns.GetHostEntry("10.72.1.88");
            IPAddress ipAddress = IPAddress.Parse(LocalIPAddress);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 9050);

            // Create a TCP/IP socket.
            Socket client = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect to the remote endpoint.
            client.BeginConnect( remoteEP,
                new AsyncCallback(ConnectCallback), client);
            connectDone.WaitOne();

            // Send test data to the remote device.
            Send(client,"This is a test<EOF>");
            sendDone.WaitOne();

            // Receive the response from the remote device.
            Receive(client);
            receiveDone.WaitOne();

            // Write the response to the unity debug log.
            Debug.Log("Client: Response received:" + response);

            // Release the socket.
            client.Shutdown(SocketShutdown.Both);
            client.Close();

        } catch (Exception e) {
            Debug.Log("Client: " + e.ToString());
        }
    }

    private void ConnectCallback(IAsyncResult ar) {
        try {
            // Retrieve the socket from the state object.
            Socket client = (Socket) ar.AsyncState;

            // Complete the connection.
            client.EndConnect(ar);

            Debug.Log("Client: Socket connected to " +
                client.RemoteEndPoint.ToString());

            // Signal that the connection has been made.
            connectDone.Set();
        } catch (Exception e) {
            Debug.Log("Client: " + e.ToString());
        }
    }

    private void Receive(Socket client) {
        try {
            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = client;

            // Begin receiving the data from the remote device.
            client.BeginReceive( state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReceiveCallback), state);
        } catch (Exception e) {
						Debug.Log(e.ToString());
        }
    }

    private void ReceiveCallback( IAsyncResult ar ) {
        try {
            // Retrieve the state object and the client socket
            // from the asynchronous state object.
            StateObject state = (StateObject) ar.AsyncState;
            Socket client = state.workSocket;

            // Read data from the remote device.
            int bytesRead = client.EndReceive(ar);

            if (bytesRead > 0) {
                // There might be more data, so store the data received so far.
            state.sb.Append(Encoding.ASCII.GetString(state.buffer,0,bytesRead));

                // Get the rest of the data.
                client.BeginReceive(state.buffer,0,StateObject.BufferSize,0,
                    new AsyncCallback(ReceiveCallback), state);
            } else {
                // All the data has arrived; put it in response.
                if (state.sb.Length > 1) {
                    response = state.sb.ToString();
                }
                // Signal that all bytes have been received.
                receiveDone.Set();
            }
        } catch (Exception e) {
            Debug.Log("Client: " + e.ToString());
        }
    }

    private void Send(Socket client, String data) {
				Debug.Log("Client: sending " + data);
        // Convert the string data to byte data using ASCII encoding.
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        // Begin sending the data to the remote device.
        client.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(SendCallback), client);
    }

    private void SendCallback(IAsyncResult ar) {
        try {
            // Retrieve the socket from the state object.
            Socket client = (Socket) ar.AsyncState;

            // Complete sending the data to the remote device.
            int bytesSent = client.EndSend(ar);
            Debug.Log("Client: Sent " + bytesSent + " bytes to server.");

            // Signal that all bytes have been sent.
            sendDone.Set();
        } catch (Exception e) {
            Debug.Log("Client: " + e.ToString());
        }
    }
}
