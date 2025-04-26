@echo off
setlocal enabledelayedexpansion

echo Rodando testes e gerando cobertura...
dotnet test --collect:"XPlat Code Coverage"

echo Aguardando cobertura ser gerada...

:: Pequeno delay para dar tempo do dotnet finalizar o arquivo (5 segundos)
timeout /t 2 >nul

echo Procurando arquivo de cobertura...

set "coverageFile="
set "coverageFolder="

for /r "%cd%\TestResults" %%f in (coverage.cobertura.xml) do (
    if exist "%%f" (
        set "coverageFile=%%f"
        set "coverageFolder=%%~dpf" 
        echo Arquivo de cobertura encontrado: !coverageFile!
        echo Gerando relatório baseado em: !coverageFile!
        reportgenerator -reports:"!coverageFile!" -targetdir:"coverage-report" -reporttypes:Html
        echo Relatório gerado em ./coverage-report/index.html
        goto :apagar
    )
)

echo ERRO: Arquivo coverage.cobertura.xml não encontrado!
goto :fim

:apagar
:: Pegando apenas a pasta (sem o coverage.cobertura.xml no final)
for %%a in ("!coverageFolder!") do set "currentFolder=%%~dpa"

echo Limpando pastas antigas em TestResults...

for /d %%d in ("%cd%\TestResults\*") do (
    if /i not "%%~fd"=="!currentFolder:~0,-1!" (
        echo Deletando pasta: %%~fd
        rmdir /s /q "%%~fd"
    )
)

:fim
pause
