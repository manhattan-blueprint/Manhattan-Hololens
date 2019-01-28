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
            client = new Client("hello_blueprint", "connected_blueprint");
            client.Connect("192.168.43.91", 9050);
            client.Update();
            // client.Update();
            client.AddItemToBuffer("I;test");
        }
        catch(Exception e) {
            System.out.println("Error initializing server; " + e.getMessage());
            return;
        }

        if (client.IsRunning()) {
            while (true) {
                try {
                    client.Update();
                }
                catch(Exception e) {
                    System.out.println("Error during server running; " + e.getMessage());
                    return;
                }
            }
        }
    }
}
