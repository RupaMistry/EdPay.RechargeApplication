***Instructions for NUGET Pack of [EdPay.Common] class library.**

1. Nuget Package Manager window and at the top right side, click on settings button.

2. Add new Package source
	- Enter Package Name as "EdPay.Packages"
	- Enter Package location pointing to "**[EdPay.Backend\Packages]**" folder

3. Right click project and select 'Open In Terminal'

4. In the Powershell window enter the below command to create a nuget package.
	dotnet pack -p:PackageVersion=1.0.0 -o ..\..\..\Packages\

5. 'EdPay.Common.1.0.0.nupkg' file must be created under "**[EdPay.Backend\Packages]**" folder.