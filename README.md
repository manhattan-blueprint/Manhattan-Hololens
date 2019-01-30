# Manhattan-Hololens
Note: the readme for this branch is written as a progression of thought. A proper manual will be made once a basic communication patter has been established.

Hololens implementation for the Manhattan Games Project, Blueprint.

## Set Up Hololens from Scratch with Unity (Windows only)
* Install ~~latest~~ version 2017.2.0f3 of unity.
* Add this package: https://github.com/Microsoft/MixedRealityToolkit-Unity/releases/tag/2017.4.2.0
* Set build options to Windows Universal Platform
* Follow this set up: https://docs.microsoft.com/en-us/windows/mixed-reality/holograms-210
* Add holograms from: https://github.com/Microsoft/HolographicAcademy/tree/Holograms-210-Gaze
* Should probably work

## What Networking Options are Available for Hololens
Hololens specs - https://www.windowscentral.com/hololens-hardware-specs

## Current networking implementation
Custom asynchronous server socket interface running on a separate thread, based off of: https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example

## Augmented Reality Customization
Things specific to the Hololens. Useful git repo with demos: https://github.com/Microsoft/HolographicAcademy/

### Gaze
https://docs.microsoft.com/en-us/windows/mixed-reality/gaze-in-unity
