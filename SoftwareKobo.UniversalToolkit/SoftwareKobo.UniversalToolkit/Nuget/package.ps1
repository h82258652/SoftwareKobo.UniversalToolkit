Function GetNuget
{
    return [System.IO.Directory]::GetFiles("..\..\packages","nuget.exe",[System.IO.SearchOption]::AllDirectories)[0]
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