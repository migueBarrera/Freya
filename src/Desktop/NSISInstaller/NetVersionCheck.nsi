; NetVersionCheck.nsh

!include LogicLib.nsh
!include x64.nsh

!define /IfNDef NET_RUNTIME_VERSION "7.0.5"
!define NETInstaller86 "windowsdesktop-runtime-7.0.5-win-x86.exe"
!define NETInstaller64 "windowsdesktop-runtime-7.0.5-win-x64.exe"


Function CheckNetVersionAndInstall
  DetailPrint "[Installing required components] Checking if net7 is installed..."
  nsExec::ExecToStack /OEM 'cmd /c dir "%windir%\system32" | dotnet --list-runtimes | find /c /i "Microsoft.WindowsDesktop.App ${NET_RUNTIME_VERSION}"'
  Pop $0
  Pop $1
  StrCpy $2 $1 1
  StrCmp $2 "1" 0 version_not_found
    goto version_found
  version_not_found:
      DetailPrint "[Installing required components] net7 will be installed..."
      Call InstallNET7DependSystemOS
      DetailPrint "[Installing required components] net7 is now installed correctly"
  version_found:
      DetailPrint "[Installing required components] net7 is now installed correctly"
FunctionEnd

Function InstallNET7DependSystemOS
   ${If} ${RunningX64}
       Call InstallNET764
   ${Else}
        Call InstallNET732
   ${EndIf}
FunctionEnd

Function InstallNET764
    NSISdl::download "https://download.visualstudio.microsoft.com/download/pr/dffb1939-cef1-4db3-a579-5475a3061cdd/578b208733c914c7b7357f6baa4ecfd6/windowsdesktop-runtime-7.0.5-win-x64.exe" "$TEMP\${NETInstaller64}"
    ExecWait '"$TEMP\${NETInstaller64}" /install /quiet /l /norestart'
FunctionEnd

Function InstallNET732
    NSISdl::download "https://download.visualstudio.microsoft.com/download/pr/eb64dcd1-d277-4798-ada1-600805c9e2dc/fc73c843d66f3996e7ef22468f4902e6/windowsdesktop-runtime-7.0.5-win-x86.exe" "$TEMP\${NETInstaller64}"
    ExecWait '"$TEMP\${NETInstaller86}" /install /quiet /l /norestart'
FunctionEnd
