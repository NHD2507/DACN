@echo off
echo script lay list de xoa.
set /p input1= nhap commnit dau: 
set /p input2= nhap commnit cuoi: 
(git diff --diff-filter=D --name-only %input1% %input2%) > DelList.txt
PAUSE