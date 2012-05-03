ECHO ready, go!
IF EXIST "C:\Users\knotten\Dropbox\Prog\Release\punch\" GOTO action
GOTO :nonaction

:action
xcopy /Q /R /S /Y "C:\Users\knotten\Dropbox\Prog\Release\punch\*" "C:\release\punch" >> C:\Users\knotten\Dropbox\Prog\Release\release_log.txt 2>&1
rmdir /s /q "C:\Users\knotten\Dropbox\Prog\Release\punch" >> C:\Users\knotten\Dropbox\Prog\Release\release_log.txt 2>&1
ECHO done >>C:\Users\knotten\Dropbox\Prog\Release\release_log.txt 2>&1
GOTO :eof

:nonaction
REM ECHO sleeep... >>C:\Users\knotten\Dropbox\Prog\Release\release_log.txt 2>&1