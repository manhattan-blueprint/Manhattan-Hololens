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
            client.SetSocket("192.168.43.160", 9050);
            // client.AddItemToBuffer("I;0.0;0.5;3.5;wood\r\n");
            // client.AddItemToBuffer("18");
            client.AddItemToBuffer("I;0.0;0.5;3.5;wood\r\n");
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

/* TODO:
 * Achieve some server communication.
 * Update to display nearest objects whenever displayed (if newest nearest
   spawn, retain old one but update with new one).
 * Fix IP to properly follow camera.
 * Use same script to create cursor to show direction to object.
 * Update to remove object if too far away.
 * Use depth sensor camera to hide object if behind walls etc.
   * (?) Determine if feasible to notify user of obstruction.
 * Make similar games to those of on the phone.
   * Spawn multiple of resource in quantity requested by phone.
   * Update communication schema.
  */


/* BUGS
Multiple entries in the retrieving IP bit?
*/
