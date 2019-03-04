
#if NETFX_CORE
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.Diagnostics;
using System.Net.Sockets;
using System;
using System.Text;
using System.IO;
#else
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
#endif
using UnityEngine;

namespace Server
{
    public class SocketListener
    {
        private ServerState serverState;
        private LocalIP localIP;
        private string localIPAddress;
        private int port;
        private GameObject blueprintServer;

        // Incoming data from the client.  
        public static string data = null;

        public SocketListener(LocalIP localIP, ServerState serverState)
        {
            this.serverState = serverState;
            this.localIP = localIP;
            port = localIP.Port();

            // May be different when emulating due to multiple network cards
            localIPAddress = localIP.Address();

            blueprintServer = GameObject.Find("server");
        }

#if NETFX_CORE
        private StreamSocketListener listener;

        public async void StartListening()
        {
            UnityEngine.Debug.Log("Starting Listener");
            listener = new StreamSocketListener();
            try {
                listener.ConnectionReceived += Listener_ConnectionReceived;
                await listener.BindServiceNameAsync("9050");
            }

            catch (Exception e) {
                UnityEngine.Debug.Log("Listener: " + e.ToString());
            }
        }

        private async void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args) {
            UnityEngine.Debug.Log("Server: New connection");

            string input;
            string response;

            using (var dr = new DataReader(args.Socket.InputStream))
            {
                //dr.InputStreamOptions = InputStreamOptions.Partial;

                dr.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                dr.ByteOrder = ByteOrder.LittleEndian;

                await dr.LoadAsync(32);

                input = dr.ReadString(32) + "\n";

                UnityEngine.Debug.Log("Server: Received '" + input + "'");

                //string dataReceived = Encoding.ASCII.GetString(input, 2, bytesRead - 2);
                response = serverState.ProcessInstruction(input);
            }

            using (var dw = new DataWriter(args.Socket.OutputStream)) 
            {
                UnityEngine.Debug.Log("Server: Sending '" + response + "'");
                dw.WriteString(response);
                await dw.StoreAsync();
                dw.DetachStream();
            }
        }
#else
        public void StartListening()
        {
            while (true)
            {
                try
                {
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

                    string response;
                    if (bytesRead > 2)
                    {
                        response = serverState.ProcessInstruction(dataReceived);
                    }

                    //---write back the text to the client---
                    Debug.Log("Listener: Sending back : " + dataReceived);
                    nwStream.Write(buffer, 0, bytesRead);

                    client.Close();
                    listener.Stop();
                }

                catch (Exception e)
                {
                    Debug.Log("Listener: " + e.ToString());
                }
            }
        }
#endif
    }
}