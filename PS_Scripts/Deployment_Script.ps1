#create Windows user account and add to "Storefront" user group


#debug
$Utf8NoBomEncoding2 = New-Object System.Text.UTF8Encoding($False)
[System.IO.File]::AppendAllText('C:\deploy.txt', "\r\n----------------------------------------------", $Utf8NoBomEncoding2)
[System.IO.File]::AppendAllText('C:\deploy.txt', "USERNAME ##USERNAME## "+ "PASSWORD ##PASSWORD## "+"SUBDOMAIN ##SUBDOMAIN## "+ "DATABASENAME ##DATABASENAME## "+"STORENAME ##STORENAME##", $Utf8NoBomEncoding2)
#debug


NET USER "##USERNAME##" "##PASSWORD##" /ADD
NET LOCALGROUP "Storefront" "##USERNAME##" /ADD

#create IIS application pool with custom properties
Import-Module WebAdministration
$appPool = New-Item ("IIS:\AppPools\##SUBDOMAIN##")
$appPool.processmodel.identityType = "SpecificUser"
$appPool.processmodel.username = "##USERNAME##"
$appPool.processmodel.password = "##PASSWORD##"
$appPool.managedRuntimeVersion = "v4.0"
$appPool | Set-Item

#create website directories
New-Item -ItemType directory -Path ##DEPLOYDIR##\##SUBDOMAIN##
New-Item -ItemType directory -Path ##DEPLOYDIR##\##SUBDOMAIN##\wwwroot
New-Item -ItemType directory -Path ##DEPLOYDIR##\##SUBDOMAIN##\logs

#create website and set website properties
New-Item IIS:\Sites\##SUBDOMAIN## -bindings @{protocol="http";bindingInformation="*:80:##SUBDOMAIN##"} -physicalPath ##DEPLOYDIR##\##SUBDOMAIN##\wwwroot
Set-ItemProperty "IIS:\Sites\##SUBDOMAIN##" ApplicationPool ##SUBDOMAIN##
Set-ItemProperty "IIS:\Sites\##SUBDOMAIN##" -name logFile -value @{directory="##DEPLOYDIR##\##SUBDOMAIN##\logs"}

Copy-Item '##StoreCore##\Web.config' '##DEPLOYDIR##\##SUBDOMAIN##\wwwroot\'
Copy-Item '##StoreCore##\Linking.bat' '##DEPLOYDIR##\##SUBDOMAIN##\wwwroot\'
Copy-Item '##StoreCore##\Global.asax' '##DEPLOYDIR##\##SUBDOMAIN##\wwwroot\'
Copy-Item '##StoreCore##\favicon.ico' '##DEPLOYDIR##\##SUBDOMAIN##\wwwroot\'

#set folder permissions for website root folder
#$Acl = Get-Acl "##DEPLOYDIR##\##SUBDOMAIN##\wwwroot"
#$Ar = New-Object  System.Security.AccessControl.FileSystemAccessRule "##USERNAME##","FullControl","Allow"
#$Acl.SetAccessRule($Ar)
#Set-Acl "##DEPLOYDIR##\##SUBDOMAIN##\wwwroot" $Acl

#unzip package
#$shell = New-Object -com shell.application
#$zipFile = $shell.NameSpace("##DEPLOYDIR##\StorefrontStore.zip")
#foreach($file in $zipFile.items())
#{
#	$shell.Namespace("##DEPLOYDIR##\##SUBDOMAIN##\wwwroot").copyhere($file)
#}

#replace variables in files
(Get-Content '##DEPLOYDIR##\##SUBDOMAIN##\wwwroot\web.config') | Foreach-Object {$_ -replace '##DATABASENAME##','##SUBDOMAIN##'} | Out-File '##DEPLOYDIR##\##SUBDOMAIN##\wwwroot\web.config'

#save file with utf-8 encoding
$File = Get-Content '##DEPLOYDIR##\##SUBDOMAIN##\wwwroot\web.config'
$Utf8NoBomEncoding = New-Object System.Text.UTF8Encoding($False)
[System.IO.File]::WriteAllLines('##DEPLOYDIR##\##SUBDOMAIN##\wwwroot\web.config', $File, $Utf8NoBomEncoding)

#set folder permissions for website root folder
$Acl = Get-Acl "##DEPLOYDIR##\##SUBDOMAIN##\wwwroot"
$Ar = New-Object  System.Security.AccessControl.FileSystemAccessRule "##USERNAME##","FullControl","Allow"
$Acl.SetAccessRule($Ar)
Set-Acl "##DEPLOYDIR##\##SUBDOMAIN##\wwwroot" $Acl

#fix permission

$acl = Get-Acl "##DEPLOYDIR##\##SUBDOMAIN##\wwwroot"
$location = "##DEPLOYDIR##\##SUBDOMAIN##\";

#Search recursivly through location defined;
get-childitem -r $location | foreach{
     $tempLocation = $_.FullName;
     #Get ACL for tempLocation;
     $acl = get-acl $tempLocation;
     #Get SID of explicit ACL;
     $acl.Access | where{
          $_.isinherited -like $false} | foreach{
          #Foreach SID purge the SID from the ACL;
          $acl.purgeaccessrules($_.IdentityReference); 
          #Reapply ACL to file or folder without SID;
          Set-Acl -AclObject $acl -path $tempLocation;
     }
}

#edit web.config to force rebuild application
[System.IO.File]::AppendAllText('##DEPLOYDIR##\##SUBDOMAIN##\wwwroot\web.config', " ", $Utf8NoBomEncoding)

#debug
[System.IO.File]::AppendAllText('C:\deploy.txt', "\r\n Done", $Utf8NoBomEncoding2)
#debug