import java.io.BufferedReader;
import java.io.IOException;
import java.io.PrintWriter;
import java.io.InputStreamReader;
import java.net.Socket;

enum State {
    IDLE,                 // Hololens IP not set
    GREET,                // Send "hello_blueprint\0 to Hololens until response"
    GREET_RESPOND,        // Send "connected_blueprint\0 to Hololens"
    IDLE_IP,              // Connected but no instructions
    ADD_ITEM_TO_BUFFER,   // Adding items to buffer
    SEND_TOP_OF_BUFFER,   // Send item at top of buffer
    POP_BUFFER            // Receive mirrored item ID and pop it from the buffer
}

public class Client {
    private String greetMessage;
    private String connectedMessage;
    private String serverAddress;
    private State state;

    public void initialize(String greetMessage, String connectedMessage, String serverAddress) {
        this.greetMessage = greetMessage;
        this.connectedMessage = connectedMessage;
        this.serverAddress = serverAddress;
        this.state = State.IDLE;
    }

    public void update() {
        
    }


    // public static void main(String[] args) throws IOException {
    //     String serverAddress = "192.168.43.91";
    //     Socket s = new Socket(serverAddress, 9050);
    //     BufferedReader input = new BufferedReader(new InputStreamReader(s.getInputStream()));
    //     PrintWriter out = new PrintWriter(s.getOutputStream(), true);
    //     out.println("Hello, World;");
    //
    //     String answer = input.readLine();
    //     System.out.println("Received: " + answer);
    //     System.exit(0);
    // }
}
