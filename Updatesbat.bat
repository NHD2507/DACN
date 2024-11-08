@echo off
echo script lay list de update.
set /p input1= nhap commnit dau: 
set /p input2= nhap commnit cuoi: 
(git diff --diff-filter=ACMRTUXB --name-only %input1% %input2%) > UpdateList.txt
setlocal enabledelayedexpansion
>temp.txt (
    for /f "delims=" %%a in (UpdateList.txt) do (
        set "line=%%a"
        if "!line: =!" neq "%%a" (
            echo "!line!"^
        ) else (
            echo %%a^
        )
    )
)
move /y temp.txt UpdateList.txt
endlocal
for /f "delims=" %%i in (Updatelist.txt) do call set "DIFF=%%DIFF%% %%i"
git archive --format=zip -o archive.zip HEAD %DIFF%
pause
