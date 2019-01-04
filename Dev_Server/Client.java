import java.io.BufferedReader;
import java.io.IOException;
import java.io.PrintWriter;
import java.io.InputStreamReader;
import java.net.Socket;

public class Client {
    public static void main(String[] args) throws IOException {
        String serverAddress = "192.168.43.91";
        Socket s = new Socket(serverAddress, 9050);
        BufferedReader input = new BufferedReader(new InputStreamReader(s.getInputStream()));
        PrintWriter out = new PrintWriter(s.getOutputStream(), true);
        out.println("Hello, World;");

        String answer = input.readLine();
        System.out.println("Received: " + answer);
        System.exit(0);
    }
}
