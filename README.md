# Manhattan-Hololens
Note: the readme for this branch is written as a progression of thought. A proper manual will be made once a basic communication patter has been established.

Hololens implementation for the Manhattan Games Project, Blueprint.

# Set Up Hololens from Scratch with Unity
* Install latest version of unity and Windows.
* Add this package: https://github.com/Microsoft/MixedRealityToolkit-Unity/releases/tag/2017.4.2.0
* Set build options to Windows Universal Platform
* Follow this set up: https://docs.microsoft.com/en-us/windows/mixed-reality/holograms-210
* Should probably work

## What Networking Options are Available for Hololens
Hololens specs - https://www.windowscentral.com/hololens-hardware-specs

## Current networking implementation
Custom asynchronous server socket interface running on a separate thread, based off of: https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example

### Gaze
https://docs.microsoft.com/en-us/windows/mixed-reality/gaze-in-unity
