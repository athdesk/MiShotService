# MiProShotService
Lightweight windows service meant to allow the use of the dedicated screenshot key without the proprietary MiOSD app, for Xiaomi Mi Notebook Pro users.

# How to build and install
Open in Visual Studio, make sure to build MiShotHelper first, and then MiShotService.
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
