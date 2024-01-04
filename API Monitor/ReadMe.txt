Notes for Installation

1047_DanaMonitorAPI: 
Actual source code for the Windows Service to be used in the HMI.

Automation Service: 
Windows service installer.

APITest.ACD: 
Example tag program that worked with the API.

Installing changes:
After building the C# program that resides in 1047_DanaMonitorAPI, copy and paste the AutomationAPI.exe and AutomationAPI.exe.config files in ...\Automation Service\Contents\
These two files can be found under ...\1047_DanaMonitorAPI\AutomationAPI\bin\Debug or \Release (depending on what it was built to).
Edit the contents of AutomationAPI.exe.config.  Only change the connectionString information.  Do not modify the name fields.
In the folder, double click Install.bat.
This will run the installer and install any necessary dependencies.
After installation, check that the Windows Service is Running and will start Automatically.  You can find the services on your computer by locating Services.msc.
Installation creates a folder located in C:\Automation Service.
A log file inside the folder is useful for troubleshooting.

To uninstall the service, click on C:\Automation Service\Contents\Uninstall.bat