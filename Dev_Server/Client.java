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
    IDLE_IP,              // - Connected but not currently executing instructions
    POP_BUFFER            // - Entered after item/items are added to the buffer.
                          // Sends the item at top of buffer, and pops once
                          // server has acknowledged the send
}

public class Client {
    private String greetMessage;
    private String connectedMessage;
    private String serverAddress;
    private State state;
    Socket s;
    BufferedReader input;
    PrintWriter out;
    long delayedTimer;

    // First item in the buffer is treated as the head
    ArrayList<String> buffer;

    // Sets up the class, defining the strings used for communication.
    Client(String greetMessage, String connectedMessage) throws Exception {
        this.greetMessage = greetMessage;
        this.connectedMessage = connectedMessage;
        this.state = State.IDLE;

        // Having this here makes timing logic more concise.
        this.delayedTimer = System.currentTimeMillis();
    }

    public void SetSocket(String serverAddress, int port) throws Exception {
        this.buffer = new ArrayList<String>();
        this.serverAddress = serverAddress;
        System.out.println("Trying to open socket.");
        this.s = new Socket(serverAddress, port);
        System.out.println("Socket opened.");
        this.state = State.GREET;
        this.out = new PrintWriter(s.getOutputStream(), true);
        this.input = new BufferedReader(new InputStreamReader(s.getInputStream()));
        System.out.println("Client communication socket established. Swapping to state 'GREET'.");
    }

    public Boolean IsRunning() throws Exception {
        if (state != State.IDLE) { return true; }
        return false;
    }

    public void AddItemToBuffer(String Item) {
        if (state == State.IDLE_IP || state == State.POP_BUFFER) {
            buffer.add(Item);
            if (state != State.POP_BUFFER) {
                System.out.println("Swapping to state 'POP_BUFFER'.");
            }
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
                    out.println(connectedMessage);
                    delayedTimer = System.currentTimeMillis();
                    state = State.IDLE_IP;
                    return;
                }
                if (currentTime >= delayedTimer) {
                    out.println(connectedMessage);
                    delayedTimer = System.currentTimeMillis() + 1000;
                }
                break;
            case IDLE_IP:
                // Nothing to do :)
                break;
            case POP_BUFFER:
                String object = buffer.get(0);
                if (object.equals(response)) {
                    System.out.println("Object display acknowledged for " + object +"!");
                    buffer.remove(object);
                    delayedTimer = System.currentTimeMillis();
                    if (buffer.size() == 0) {
                        System.out.println("Buffer empty; Swapping to state 'IDLE_IP'.");
                        state = State.IDLE_IP;
                        return;
                    }
                }
                if (currentTime >= delayedTimer) {
                    out.println(object);
                    delayedTimer = System.currentTimeMillis() + 1000;
                }
                break;
        }
    }
}
