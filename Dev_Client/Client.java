/*
Implements the the Client API, designed for custom low level communication as
outlined in Hololens "Communication V1.png".
*/

import java.io.BufferedReader;
import java.io.IOException;
import java.io.PrintWriter;
import java.io.InputStreamReader;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.net.Socket;

enum State {
    IDLE,                 // - Device socket not set yet
    GREET,                // - Send greet message to Hololens until response,
                          // send connected message to Hololens once received.
    IDLE_IP,              // If there is anything in the buffer it tries to send
                          // it repeatedly until it gets a mirrored response.
}

public class Client {
    private String greetMessage;
    private String connectedMessage;
    private String serverAddress;
    private State state;
    Socket s;
    BufferedReader input;
    PrintWriter out;
    long delayedTime;

    // First item in the buffer is treated as the head
    ArrayList<String> buffer;

    // Sets up the class, defining the strings used for communication.
    Client(String greetMessage, String connectedMessage) throws Exception {
        this.greetMessage = greetMessage;
        this.connectedMessage = connectedMessage;
        this.state = State.IDLE;

        // Having this here makes timing logic more concise.
        this.delayedTime = System.currentTimeMillis();
    }

    public void Connect(String serverAddress, int port) throws Exception {
        this.buffer = new ArrayList<String>();
        this.serverAddress = serverAddress;
        System.out.println("Trying to open socket.");
        this.s = new Socket(serverAddress, port);
        System.out.println("Socket opened.");
        this.state = State.GREET;
        this.out = new PrintWriter(s.getOutputStream(), true);
        this.input = new BufferedReader(new InputStreamReader(s.getInputStream()));
        System.out.println("Client communication socket established. Swapping to state 'GREET'.");
        out.println(greetMessage);
    }

    public Boolean IsRunning() throws Exception {
        if (state != State.IDLE) { return true; }
        return false;
    }

    public void AddItemToBuffer(String Item) {
        if (state == State.IDLE_IP) {
            buffer.add(Item);
            System.out.println("Item " + "added to buffer");
        }
        else {
            System.out.println("Connection not established yet; not adding item to buffer.");
        }
    }

    // Should be called frequently, ideally on an asynchronous thread.
    public void Update() throws Exception {
        String response = input.readLine();
        long currentTime = System.currentTimeMillis();

        // Useful for debugging
        if (response != null) { System.out.println("Received " + response); }

        switch(state) {
            case IDLE:
                // Nothing to do :)
                break;
            case GREET:
                // If the device has already responded correctly then there
                // is no need to send more requests.
                if (greetMessage.equals(response)) {
                    System.out.println("Greeting established! Swapping to state 'IDLE_IP'.");
                    // out.println(connectedMessage);
                    delayedTime = System.currentTimeMillis();
                    state = State.IDLE_IP;
                    return;
                }
                if (currentTime >= delayedTime) {
                    System.out.println("Sending greeting message.");
                    out.println(greetMessage);
                    delayedTime = System.currentTimeMillis() + 1000;
                }
                break;
            case IDLE_IP:
                if (buffer.size() > 0) {
                    String object = buffer.get(0);
                    if (object.equals(response)) {
                        System.out.println("Object display acknowledged for " + object +"!");
                        buffer.remove(object);
                        delayedTime = System.currentTimeMillis();
                        if (buffer.size() == 0) {
                            System.out.println("Buffer empty; Swapping to state 'IDLE_IP'.");
                            state = State.IDLE_IP;
                            return;
                        }
                    }
                    if (currentTime >= delayedTime) {
                        System.out.println("Attempting to send object " + object);
                        out.println(object);
                        delayedTime = System.currentTimeMillis() + 1000;
                    }
                }
                break;
        }
    }
}
