# MiProShotService

A lightweight windows service meant to allow the use of the dedicated screenshot key without the proprietary MiOSD app, for Xiaomi Mi Notebook Pro users.

## How does it work

### Interfacing with the firmware

The program basically sets an event watcher on a WMI event, by using a WQL query `SELECT * FROM INVHK7_Event` .
When the event watcher fires, custom events are invoked. <br>
This method was discovered by looking at the original MiOSD service behaviour.

### Opening the screenshot tool

An helper program is executed as the current logged in user (using a modified version of [this library](https://github.com/murrayju/CreateProcessAsUser)). <br>
The helper program spawns an invisible Windows Form, and steals focus from the user so that it can work even if elevated windows were first in focus; then it emulates the Win+Shift+S keystroke, to open the tool.

## How to build and install

Open in Visual Studio, just make sure to build MiFormHelper first, and then MiShotService. <br>
After building the program, or downloading a binary release, you can install and start the service by doing this in an elevated command shell:

```
installutil MiShotService.exe
net start mishot
```
You can now also install the service using it's GUI, just open the executable as you would with any normal program, and press Install. <br> Note that the service is registered by it's current path, so if you want to install it for the long term, copying it first in the wanted target directory is recommended. I personally use `C:\MiShot\MiShotService.exe` , but anything will work.

## How to uninstall

```
net stop mishot
sc delete MiShotService
```

Or you can use the same GUI you used to install it. Just open the executable and click Uninstall. The file will not be removed, only the service will be unregistered.

<hr>

Thanks to [Justin Murray](https://github.com/murrayju) for [CreateProcessAsUser](https://github.com/murrayju/CreateProcessAsUser). <br>
Thanks to Xiaomi for the original MiOSD software.
