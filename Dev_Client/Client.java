/*
Implements the the Client API, designed for custom low level communication as
outlined in Hololens "Communication V1.png".
*/

import java.io.BufferedReader;
import java.io.IOException;
import java.io.PrintWriter;
import java.io.InputStreamReader;
import java.io.DataOutputStream;
import java.util.ArrayList;
import java.net.Socket;

enum State {
    IDLE,                 // - Device socket not set yet
    GREET,                // - Send greet message to Hololens until response,
                          // send connected message to Hololens once received.
    IDLE_IP,              // - If there is anything in the buffer it tries to send
                          // it repeatedly until it gets a mirrored response.
}

public class Client {
    private String greetMessage;
    private String serverAddress;
    private State state;
    private int port;
    private long delayedTime;

    // First item in the buffer is treated as the head
    ArrayList<String> buffer;

    // Sets up the class, defining the strings used for communication.
    Client(String greetMessage) throws Exception {
        this.greetMessage = greetMessage;
        this.state = State.IDLE;
        buffer = new ArrayList<String>();

        // Having this here makes timing logic more concise.
        this.delayedTime = System.currentTimeMillis();
    }

    public void SetSocket(String serverAddress, int port) throws Exception {
        this.serverAddress = serverAddress;
        this.port = port;
        System.out.println("Socket set to (" + serverAddress + ", " + port + ")");
        this.state = State.IDLE_IP;
    }

    public String SendAndRecv(String message) throws Exception {
        Socket clientSocket = new Socket(serverAddress, 9050);
        DataOutputStream outToServer = new DataOutputStream(clientSocket.getOutputStream());
        BufferedReader inFromServer = new BufferedReader(new InputStreamReader(clientSocket.getInputStream()));
        System.out.println("Sending: " + message);
        outToServer.writeBytes(message);
        outToServer.flush();
        message = new String(inFromServer.readLine());
        System.out.println("Received: " + message);
        clientSocket.close();
        return message;
    }
}
