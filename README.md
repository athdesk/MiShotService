# MiProShotService
Lightweight windows service meant to allow the use of the dedicated screenshot key without the proprietary MiOSD app, for Xiaomi Mi Notebook Pro users.

# How to build and install
You need to add as a reference the dll release of [this library](https://github.com/murrayju/CreateProcessAsUser).

Open in Visual Studio, just make sure to build MiFormHelper first, and then MiShotService.
After building, you can install and start the service by doing this in an elevated command shell:
```
installutil MiShotService.exe
net start mishot
```

# How to uninstall
```
net stop mishot
sc delete MiShotService
```
