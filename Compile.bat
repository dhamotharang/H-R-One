@echo off

For /f "tokens=1-4 delims=/ " %%a in ('date /t') do (set _mydate=%%c%%b%%a)
For /f "tokens=1-2 delims=/:" %%a in ('time /t') do (set _mytime=%%a%%b)

set _VSPath=%ProgramFiles(x86)%\Microsoft Visual Studio 8\Common7\IDE

IF EXIST "%ProgramFiles%\Microsoft Visual Studio 8\Common7\IDE\devenv.exe" (
	SET _VSPath=%ProgramFiles%\Microsoft Visual Studio 8\Common7\IDE
) ELSE IF NOT EXIST "%_VSPath%\devenv.exe" (
	SET "_VSPath="
)

@echo on

DEL /S /F *.*scc 
"%_VSPath%\devenv.exe" /Rebuild Release HROneWeb.sln
pause