call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\Tools\VsDevCmd.bat"

call msbuild NanoFramework.Adafruit_LED_Backpack /property:Configuration=Release
call "Resources/BuildScripts/Post/nuget.exe" pack NanoFramework.Adafruit_LED_Backpack.nuspec