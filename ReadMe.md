#WindowsPhoneTestFramework

There is an introduction video on http://bit.ly/wp7-test

There are some Wiki Pages now on https://github.com/Expensify/WindowsPhoneTestFramework/wiki


##General setup

Important note - there are several changes going through the tip of the source tree at present as we add Android and iOS support - please be patient as we make these big changes!

Before you try the general steps, consider using NuGet!

For adding BDD to a class library project, see https://github.com/Expensify/WindowsPhoneTestFramework/wiki/Writing-a-new-SpecFlow-test-project-for-WindowsPhoneTestFramework
			
For adding the test client to a WP7 project, see https://github.com/Expensify/WindowsPhoneTestFramework/wiki/Adding-testing-to-an-application

			
##NuGet setup - Windows Phone app

Use the "App - Windows Phone Test Framework" installer on nuget.org/List/Packages/WP7TestClient

Once you have installed from NuGet into your WP App, then:

1. in the App.xaml.cs constructor, add

	```
            #if DEBUG
            WindowsPhoneTestFramework.Client.AutomationClient.Automation.Instance.Initialise();
            #endif // DEBUG
	```
	
##NuGet setup - Class Library test project (BDD)

Use the "BDD - Windows Phone Test Framework" installer on nuget.org/List/Packages/WP7Test

Once you have installed from NuGet into your test class library, then:

1. Change the project Build from "Any CPU" to "x86" only

2. Edit app.config to provide the necessary configuration values for your app

    - Be especially careful about the paths
	
	- For finding the ApplicationId, see ProductId inside the WMAppManifest.xml file for your app.
	
	- For WP8 projects, some further changes to the app.config are required, the following section needs to be present:

	```
    <dependentAssembly>
		<!-- Microsoft.SmartDevice.Connectivity, Version=11.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a -->
		<assemblyIdentity name="Microsoft.SmartDevice.Connectivity" publicKeyToken="b03f5f7f11d50a3a" culture="neutral" />	
		<bindingRedirect oldVersion="10.0.0.0" newVersion="11.0.0.0" />
    </dependentAssembly>
	```
	 
	And the values for WindowsName / WindowClassName need to be this:
	 
	```
	<add key="EmuSteps.Application.WindowsPhone.WindowName" value="XDE" />
    <add key="EmuSteps.Application.WindowsPhone.WindowClassName" value="WindowsForms10.Window.8.app.0.30d38e8_r14_ad1" />
	```
	
3. Add a new feature:

    ```
        Feature: App Test
            In order to test my app
            As a WP7 Developer
            I want to see it start and take a picture of it
    ```
    ```
        Scenario: Start the app
            Given my app is uninstalled
            And my app is installed
            And my app is running
            Then I wait 5 seconds
            Then take a picture
    ```

4. Run the tests

#Prerequisites

To get this to work, you need to install:

- windows phone 7.1 mango dev tools or wp8 dev tools

- nunit (specflow will use nunit by default)

- specflow


#Some possible problems:

- For non-English setups (outside of US, UK, AU, etc) you may find you are not able to create the emulator - this is due to internationalised emulator device names (currently I'm looking for a list).

	- There is now some code to help work around this - but if the emulator does not start in your SDK, then please get in touch.
	
	- If you want to try fixing this yourselves, take a look at the device name in DriverBase.cs and EmulatorDriver.cs

- For some script runners, then you may need to change script runner to have the 32-big flag set - try to find a 32-bit alternative (e.g. nunit-console-x86.exe) - or (at worst) use CorFlags.exe to change your test-runner.

	```
		"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\CorFlags.exe" "your target.exe" /32BIT+
	```

- The server part of the code opens a WCF service on http://localhost:8085 - it needs permission to do this - use:

	```
		 netsh http add urlacl url=http://+:8085/ user=<domain>\<user>
	```
	 
#Source code build

To start:

1. Open and build the whole solution - Debug configuration

2. Open a command prompt and run "cscript runspec.js" inside the Example directory - this will run all the specflow features

3. Try running the emuHost command line tool, then try commands like:

	```
        help
		install
		launch
		click Go!
		setText TextBoxInput=hello world
		getText TextBoxOutput
		doSwipe LeftToRight
	```

	Example command line arguments for EmuHost are:
	
		for WP7
		```
		EmuHost.exe /controller wp /wppid {e33eb75b-7811-4343-a3ab-da5dd6df7572} /wpicon ../../clientbin/debug/ApplicationIcon.png /wpname ExampleApp /wpxap ../../clientbin/debug/ExampleApp.xap 
		```
		
		for Android:
		```
		/controller Android /init Android.AvdName=BigScreen;Android.ConsolePort=6001;Android.AdbPort=6002;Android.SdkPath=D:\android-sdk-windows\ /andpackage com.cirrious.exampleApp /andapk ../../Example/ExampleApp.Android/bin/ExampleApp.apk /andtestpackage com.cirrious.exampleApp.test /andtestapk ../../Example/ExampleApp.TestStub/bin/ExampleApp.TestStub.apk
		```
	
#Source code - using the test platform

To work out how to use the test platform in your own apps:

1. Try looking at the code for ExampleApp - there's only one line that's added to enable testing - `Automation.Instance.Initialise();` in App.xaml.cs

2. Try looking at the gherkin code in the ExampleApp.Spec features

#Bonus Feature

The framework has the capability to run more than one emulator at the same time.
This is done by changing the TargetDevice from 
    
    "Emulator" 

to a list of Device, port number pairs like so: 
    
    "Emulator WVGA,8085;Emulator WVGA 512MB,8086;Emulator WXGA,8087"

This means if you have a tool that can support running tests in parrallel (like SpecRun) you can take advantage of this feature. It is up to your test runner to handle running the tests in parallel.

Please note, if you do this, you will be running multiple instances of the service on different ports, and will need to run the command mentioned earlier for each port.

#Questions

Please ask them on http://www.stackoverflow.com

#Contributing

Please do dive on in and help :)