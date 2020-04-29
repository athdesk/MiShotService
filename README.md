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
After building th program, or downloading a binary release, you can install and start the service by doing this in an elevated command shell:

```
installutil MiShotService.exe
net start mishot
```

## How to uninstall

```
net stop mishot
sc delete MiShotService
```

<hr>

Thanks to [Justin Murray](https://github.com/murrayju) for [CreateProcessAsUser](https://github.com/murrayju/CreateProcessAsUser). <br>
Thanks to Xiaomi for the original MiOSD software.
