# Manhattan-Hololens
Note: the readme for this branch is written as a progression of thought. A proper manual will be made once a basic communication patter has been established.

Hololens implementation for the Manhattan Games Project, Blueprint.

## Set Up Hololens from Scratch with Unity (Windows only)
* Install ~~latest~~ version 2017.2.0f3 of unity.
* Add [this package](https://github.com/Microsoft/MixedRealityToolkit-Unity/releases/tag/2017.4.2.0)
* Set build options to Windows Universal Platform
* Follow [this set up](https://docs.microsoft.com/en-us/windows/mixed-reality/holograms-210)
* Add holograms from [here](https://github.com/Microsoft/HolographicAcademy/tree/Holograms-210-Gaze)
* Should probably work

## Useful Hololens sites
 * https://www.windowscentral.com/hololens-hardware-specs

 * [Implementing a synchronous socket listener](https://docs.microsoft.com/en-us/dotnet/framework/network-programming/synchronous-server-socket-example) (which can then be run on a seperate thread to operate asynchronously)

 * [Hololens development examples and useful code](https://github.com/Microsoft/HolographicAcademy/)

 * [Gaze](https://docs.microsoft.com/en-us/windows/mixed-reality/gaze-in-unity) (click on items and stuff)

## UWP things
 * [Porting Unity games to UWP](https://blogs.windows.com/buildingapps/2016/04/18/intro-to-porting-unity-3d-games-to-uwp-building-and-deploying/)

 * [Submitting an item to the thread pool](https://docs.microsoft.com/en-us/windows/uwp/threading-async/submit-a-work-item-to-the-thread-pool)

 * [Finding the IP address](https://stackoverflow.com/questions/33770429/how-do-i-find-the-local-ip-address-on-a-win-10-uwp-project) (dear god)

 * [Using the Device Portal](https://docs.microsoft.com/en-us/windows/mixed-reality/using-the-windows-device-portal)
   * My method: Connect both computer and Hololens

 * [Install apps on Hololens](https://docs.microsoft.com/en-us/hololens/hololens-install-apps)

 * [Deploy an app to Hololens](https://docs.microsoft.com/en-us/windows/mixed-reality/using-visual-studio)
 * Build for 32 bit

 * [Useful Github with lots of info](https://github.com/Microsoft/Windows-universal-samples)
   * [In particular networking](https://github.com/Microsoft/Windows-universal-samples/tree/master/Samples/StreamSocket)

 * [StreamSocketListener example](https://stackoverflow.com/questions/32665847/cannot-connect-to-streamsocketlistener)

## Other stuff
 * Installing Eduroam certificate: connect hololens to computer with certificate using the device portal and install transfer security certificate between hololens and computer. Afterwards connect back to Eduroam using the Hololens and put in student username and password. The page will say insecure, but proceed anyway. Log in using the Hololens log in details.