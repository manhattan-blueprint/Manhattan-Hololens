/*
Provides an example of how to use the Client API, designed for custom low
level communication as outlined in Hololens "Communication V1.png".
*/

import java.io.PrintWriter;

public class BlueprintClient {

    public static void main(String[] args) throws Exception {
        Client client = null;
        System.out.println("Starting client.");

        try {
            client = new Client("hello_blueprint\r\n");
            client.SetSocket("192.168.43.74", 9050);
            // client.SetSocket("192.168.43.160", 9050);
            System.out.println("Sending message");
            client.SendAndRecv("I;003;00011.45;00012.31;02;010");
        }
        catch(Exception e) {
            System.out.println("Error sending message from client; " + e.getMessage());
            return;
        }
    }
}