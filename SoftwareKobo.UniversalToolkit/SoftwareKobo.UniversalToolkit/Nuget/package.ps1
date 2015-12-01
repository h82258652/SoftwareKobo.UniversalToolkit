Function GetNuget
{
	return "C:\Users\%USERNAME%\.nuget\packages\NuGet.CommandLine\3.3.0\tools\nuget.exe"
}

$path = "..\bin\Release\"

$dll = [System.IO.Path]::Combine($path,"SoftwareKobo.UniversalToolkit.dll")
$version = (Get-Item $dll).VersionInfo.FileVersion

$outNuspacName = "SoftwareKobo.UniversalToolkit." + $version + ".nuspec"

$doc = New-Object System.Xml.XmlDocument
$doc.Load("SoftwareKobo.UniversalToolkit.template")
$versionElement = $doc.GetElementsByTagName("version")[0]
$versionElement.InnerText = $version
$doc.Save($outNuspacName)

$nuget = GetNuget
$cmd = $nuget + " pack " + $outNuspacName

cmd /c $cmd

Remove-Item $outNuspacName