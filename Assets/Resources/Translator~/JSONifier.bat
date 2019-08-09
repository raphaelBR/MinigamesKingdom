title: JSONify
@echo off
set /p menu="Enter directory: "
set resources=%~dp0..\%menu%\%menu%
set list=en fr es it de
for %%a in (%list%) do (
	copy /y %~dp0%menu%\%%a\translations.json %resources%_%%a.json
	powershell -Command "(gc %resources%_%%a.json) -replace ':', '#COMMA""value"""":' | Out-File %resources%_%%a.json -Encoding UTF8
	powershell -Command "(gc %resources%_%%a.json) -replace '{', '{""items"""":[{""key"""":' | Out-File %resources%_%%a.json -Encoding UTF8
	powershell -Command "(gc %resources%_%%a.json) -replace '}', '}]}' | Out-File %resources%_%%a.json -Encoding UTF8
	powershell -Command "(gc %resources%_%%a.json) -replace ',', '},{""key"""":' | Out-File %resources%_%%a.json -Encoding UTF8
	powershell -Command "(gc %resources%_%%a.json) -replace '#COMMA', ',' | Out-File %resources%_%%a.json -Encoding UTF8
)
pause