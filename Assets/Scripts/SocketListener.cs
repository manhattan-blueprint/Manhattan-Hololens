
#if NETFX_CORE
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.Diagnostics;
using System;
using System.IO;
#else
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
#endif

public class SocketListener
{
    private ServerState serverState;
    private LocalIP localIP;
    private string localIPAddress;
    private int port;

    // Incoming data from the client.  
    public static string data = null;

    public SocketListener(LocalIP localIP, ServerState serverState) {
        this.serverState = serverState;
        this.localIP = localIP;
        port = localIP.Port();

        // May be different when emulating due to multiple network cards
        localIPAddress = localIP.Address();
    }

#if NETFX_CORE
    public void StartListening()
    {
        UnityEngine.Debug.Log("Starting Listener");
        StreamSocketListener listener = new StreamSocketListener();
        //while (true) {
            try {
                listener.ConnectionReceived += Listener_ConnectionReceived;
                listener.BindServiceNameAsync("9050").AsTask().Wait();
            }

            catch (Exception e) {
                UnityEngine.Debug.Log("Listener: " + e.ToString());
            }
        //}
    }

    private async void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args) {
        UnityEngine.Debug.Log("Server: New connection");

        string input;

        //using (var sr = new StreamReader(args.Socket.InputStream.AsStreamForRead()))
        //{
        //    input = await sr.ReadLineAsync();
        //    UnityEngine.Debug.Log("ServerA: Received '" + input + "'");
        //    serverState.AddInstruction(input);
        //}

        using (var dr = new DataReader(args.Socket.InputStream))
        {
            dr.InputStreamOptions = InputStreamOptions.Partial;

            await dr.LoadAsync(18);
            input = dr.ReadString(18);
            UnityEngine.Debug.Log("Server: Received '" + input + "'");

            //string dataReceived = Encoding.ASCII.GetString(input, 2, bytesRead - 2);
            serverState.AddInstruction(input);
        }

        using (var dw = new DataWriter(args.Socket.OutputStream)) {
            UnityEngine.Debug.Log("Server: Sending '" + input + "'");
            dw.WriteString(input);
            await dw.StoreAsync();
            dw.DetachStream();
        }
    }
#else
    public void StartListening() {
        while (true) {
            try {
                //---listen at the specified IP and port no.---
                IPAddress localAdd = IPAddress.Parse(localIPAddress);
                TcpListener listener = new TcpListener(localAdd, port);
                Debug.Log("Listener: Listening...");
                listener.Start();

                //---incoming client connected---
                TcpClient client = listener.AcceptTcpClient();

                //---get the incoming data through a network stream---
                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[64];

                //---read incoming stream---
                int bytesRead = nwStream.Read(buffer, 0, 64);
                Debug.Log("Listener: Received " + bytesRead + " of form " + buffer);

                //---convert the data received into a string---
                //string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                string dataReceived = Encoding.ASCII.GetString(buffer, 2, bytesRead - 2);
                Debug.Log("Listener: Received : " + dataReceived);

                if (bytesRead > 2) {
                    serverState.AddInstruction(dataReceived);
                }

                //---write back the text to the client---
                Debug.Log("Listener: Sending back : " + dataReceived);
                nwStream.Write(buffer, 0, bytesRead);

                client.Close();
                listener.Stop();
            }

            catch (Exception e) {
                Debug.Log("Listener: " + e.ToString());
            }
        }
    }
#endif
}