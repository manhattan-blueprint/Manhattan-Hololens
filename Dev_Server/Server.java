import java.io.IOException;
import java.io.PrintWriter;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.InetAddress;

public class Server {
    public static void main(String[] args) throws IOException {
        System.out.println("Server IP is " + InetAddress.getLocalHost().getHostAddress());
        ServerSocket listener = new ServerSocket(9050);
        try {
            while (true) {
                Socket socket = listener.accept();
                try {
                    PrintWriter out =
                        new PrintWriter(socket.getOutputStream(), true);
                    out.println("Hello, World");
                } finally {
                    socket.close();
                }
            }
        } finally {
            listener.close();
        }
    }
}
