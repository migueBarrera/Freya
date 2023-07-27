!include LogicLib.nsh
!include x64.nsh
!include NetVersionCheck.nsi

!define PRODUCT_NAME "FreyaManager"
!define PRODUCT_PUBLISHER "FreyaManager"
!define PRODUCT_WEB_SITE "freya.soft.app@gmail.com"

; Following constants you should never change
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

!include "MUI.nsh"
!define MUI_ABORTWARNING
!define MUI_ICON "..\FreyaManager\Resources\favicon.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"


Function .onInit
   ${If} ${RunningX64}
      StrCpy $INSTDIR "$PROGRAMFILES64\FreyaManager"
   ${Else}
      StrCpy $INSTDIR "$PROGRAMFILES\FreyaManager"
   ${EndIf}
FunctionEnd

; Wizard pages
!insertmacro MUI_PAGE_WELCOME
; Note: you should create License.txt in the same folder as this file, or remove following line.
!insertmacro MUI_PAGE_LICENSE "License.txt"
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_LANGUAGE "Spanish"

; Replace the constants bellow to hit suite your project
Name "${PRODUCT_NAME} ${VERSION}"
OutFile "${PRODUCT_NAME}${VERSION}_x64.exe"
InstallDir "$INSTDIR"
ShowInstDetails show
ShowUnInstDetails show

; Following lists the files you want to include, go through this list carefully!
Section "MainSection" SEC01
  RMDir /r "$INSTDIR"
  CreateDirectory "$INSTDIR"

  SetOutPath "$INSTDIR"
  SetOverwrite ifnewer
  File "${BINARIES_FOLDER}\*.*"

  DetailPrint "[Installing required components] Checking if the required components are installed..."
  Call CheckNetVersionAndInstall
  
  CreateShortCut "$DESKTOP\${PRODUCT_NAME}.lnk" "$INSTDIR\FreyaManager.exe" ""
SectionEnd

Section -Post
  ;Following lines will make uninstaller work - do not change anything, unless you really want to.
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
  
  ; COOL STUFF: Following line will add a registry setting that will add the INSTDIR into the list of folders from where
  ; the assemblies are listed in the Add Reference in C# or Visual Studio.
  ; This is super-cool if your installation package contains assemblies that someone will use to build more applications - 
  ; and it doesn't hurt even if it is placed there, it will only make the VS a bit slower to find all assemblies when adding references.
  ;WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "SOFTWARE\Microsoft\.NETFramework\v2.0.50727\AssemblyFoldersEx\ZWare\ZawsCC" "" "$INSTDIR"
SectionEnd

; Replace the following strings to suite your needs
Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "Application was successfully removed from your computer."
FunctionEnd

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "Are you sure you want to completely remove Freya Manager and all of its components?" IDYES +2
  Abort
FunctionEnd

; Remove any file that you have added above - removing uninstallation and folders last.
; Note: if there is any file changed or added to these folders, they will not be removed. Also, parent folder (which in my example 
; is company name ZWare) will not be removed if there is any other application installed in it.
Section Uninstall
  Delete "$INSTDIR\*"
  DetailPrint "Removing $INSTDIR"
  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  ; Change following to be exactly as above
  ; DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "SOFTWARE\Microsoft\.NETFramework\v2.0.50727\AssemblyFoldersEx\ZWare\ZAwsCC"  
  Delete "$DESKTOP\${PRODUCT_NAME}.lnk"
  SetAutoClose true
SectionEnd