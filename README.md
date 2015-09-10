# StaticMicVolumeApp
Forces microphone level to a fixed user-set value. App minimizes to tray without a taskbar icon.

### Intention
I created this app as a solution for the gazillion different applications and/or games that change the levels of your microphone without asking.
Results of this are people yelling at you to turn down the volume on your mic because your levels are way too high.

### Features (v1.2.0.0)
- Frontend (Windows Forms)
- Select any Mic from ComboBox
- Set Volume in Percent
- Set Interval for checking in Seconds
- Minimize to Tray without Taskbar icon
- Start by providing command line parameters
 
### Example for starting with command line parameters
```
StaticMicVolumeApp.exe -v 30
StaticMicVolumeApp.exe --volume 30 --interval 10
StaticMicVolumeApp.exe -v 30 -i 10
StaticMicVolumeApp.exe -v 30 --name SubStringOfMyMicName
StaticMicVolumeApp.exe -v 30 -n SubStringOfMyMicName
```


### Dependencies
- NAudio v1.7.3 https://www.nuget.org/packages/NAudio/1.7.3
- Command Line Parser Library v2.0.261-beta https://github.com/gsscoder/commandline
- MoreLinq v1.1.1 https://www.nuget.org/packages/morelinq/1.1.1


