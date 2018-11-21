# Manhattan-Hololens
Note: the readme for this branch is written as a progression of thought. A proper manual will be made once a basic communication patter has been established.

Hololens implementation for the Manhattan Games Project, Blueprint.

## What Networking Options are Available for Hololens
Hololens specs - https://www.windowscentral.com/hololens-hardware-specs

## Unity Network Transport Low Level API
Regardless, a low level API will be required.
Unity docs - https://docs.unity3d.com/Manual/UNetUsingTransport.html
Informative video - https://www.youtube.com/watch?v=qGkkaNkq8co

Better to use serializer? - https://docs.unity3d.com/Manual/UNetReaderWriter.html

## Building a UI
A UI will be best for displaying info on connection while debugging.
Building a UI - https://unity3d.com/learn/tutorials/s/user-interface-ui

## Testing
The final solution requires a Hololens to talk to an Android phone. Therefore if a way of using a custom UDP connection with the simulator is viable for development then that will be done; otherwise the Hololens application will have to be built every time and redeployed in order to attempt speaking to the phone directly.
