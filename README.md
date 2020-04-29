# MiProShotService

<div style="text-align: center">
    <img src="https://hitcounter.pythonanywhere.com/count/tag.svg?url=https%3A%2F%2Fgithub.com%2Fathdesk%2FMiShotService" alt="Hits">
</div>

A lightweight windows service meant to allow the use of the dedicated screenshot key without the proprietary MiOSD app, for Xiaomi Mi Notebook Pro users.

## How does it work

### Interfacing with the firmware

The program basically sets an event watcher on a WMI event, by using a WQL query `SELECT * FROM INVHK7_Event` .
When the event watcher fires, custom events are invoked. <br>
This method was discovered by looking at the original MiOSD service behaviour.

### Opening the screenshot tool

An helper form is shown whenever the screenshot event is triggered. <br>
The invisible form steals focus from the user so that it can work even if elevated windows were first in focus; then it emulates the Win+Shift+S keystroke, to open the tool.

## How to build and install

Open in Visual Studio and build MiShotService. No libraries or resources are needed anymore. <br>
After building the program, or downloading a binary release, you can install the service by opening its executable and checking the autostart checkbox. <br>
Note that the service is registered by it's current path, so if you want to install it for the long term, copying it first in the wanted target directory is recommended. I personally use `C:\MiShot\MiShotService.exe` , but anything will work. You can change the registered path by moving the executable in the new target directory, running it and re-checking the autostart checkbox.

## How to uninstall

You can use the same GUI you used to install it. Just open the executable and uncheck the same checkbox. The file will not be removed, only the service will be unregistered.

<hr>

Thanks to Xiaomi for the original MiOSD software. <br>
