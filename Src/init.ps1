param($installPath, $toolsPath, $package, $project)
#Get Default browser
$DefaultSettingPath = 'HKCU:\SOFTWARE\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice'
$DefaultBrowserName = (Get-Item $DefaultSettingPath | Get-ItemProperty).ProgId

if($DefaultBrowserName -eq 'AppXq0fevzme2pys62n3e0fbqa7peapykr8v')
{
    #Open url in edge
    start Microsoft-edge:$packageKeyForm
}
else
{
    try
    {
        #Create PSDrive to HKEY_CLASSES_ROOT
        $null = New-PSDrive -PSProvider registry -Root 'HKEY_CLASSES_ROOT' -Name 'HKCR'

        #Get the default browser executable command/path
        $DefaultBrowserOpenCommand = (Get-Item "HKCR:\$DefaultBrowserName\shell\open\command" | Get-ItemProperty).'(default)'
        $DefaultBrowserPath = [regex]::Match($DefaultBrowserOpenCommand,'\".+?\"')
        
        #Open URL in browser
        Start-Process -FilePath $DefaultBrowserPath ('--new-window',  "https://xceed.com/fluent-assertions/")
    }
    catch
    {
        Throw $_.Exception
    }
    finally
    {
        #Clean up PSDrive for 'HKEY_CLASSES_ROOT
        Remove-PSDrive -Name 'HKCR'
    }
}