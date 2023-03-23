set /p Username=Please provide your e-mail for XperiCad organization: 
set /p Password=Please provide your passwrod for XperiCad organization: 

nuget sources Remove -Name "XperiCad" -Source "https://pkgs.dev.azure.com/xpericad/XperiCad.Common/_packaging/XperiCad.Common/nuget/v3/index.json" -configfile %AppData%\NuGet\NuGet.Config
nuget sources Add -Name "XperiCad" -Source "https://pkgs.dev.azure.com/xpericad/XperiCad.Common/_packaging/XperiCad.Common/nuget/v3/index.json" -UserName %Username% -Password %Password% -configfile %AppData%\NuGet\NuGet.Config
pause