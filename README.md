# RDP Watcher
## What is it ? 
This is a simple C# program to notificate through a Discord WebHook whenether someone log in or out from your Windows VPS.
## How to use
Start by editing the Config/AppConfig.cs file and adding your Discord WebHook URL :
```cs
public string WebHookUrl { get; set; } = "https://discord.com/api/webhooks/XXXXXXX/XXXXXXX";
```
Then open a cmd as admin and then type :
```bat
sc create NameOfTheService start=boot binpath="PathToYourExeIncludingIt"
sc start NameOfTheService
```
\
And now your done !