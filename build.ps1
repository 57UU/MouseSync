# 一键编译 Release AOT 可执行文件到 ./build 目录
# 用法: .\build.ps1

$ErrorActionPreference = "Stop"

# 设置 VS 开发者环境 (AOT 编译需要 link.exe / vswhere.exe)
$vswhere = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
if (Test-Path $vswhere) {
    $env:PATH = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer;$env:PATH"
    $vsPath = & $vswhere -latest -property installationPath
    $vsdevcmd = Join-Path $vsPath "Common7\Tools\VsDevCmd.bat"
    if (Test-Path $vsdevcmd) {
        # 导入 VS 开发环境变量
        $envDump = cmd /c "`"$vsdevcmd`" -no_logo && set"
        foreach ($line in $envDump) {
            if ($line -match '^([^=]+)=(.*)$') {
                [Environment]::SetEnvironmentVariable($Matches[1], $Matches[2], "Process")
            }
        }
        Write-Host "已加载 VS 开发环境: $vsPath" -ForegroundColor DarkGray
    }
}

$root = $PSScriptRoot
$buildDir = Join-Path $root "build"

# 清理并创建 build 目录
if (Test-Path $buildDir) {
    Remove-Item -Recurse -Force $buildDir
}
New-Item -ItemType Directory -Path $buildDir | Out-Null

$projects = @(
    "MouseSyncClientCore\MouseSyncClientCore.csproj",
    "MouseSyncServer\MouseSyncServer.csproj"
)

foreach ($proj in $projects) {
    $projPath = Join-Path $root $proj
    $projDir = Split-Path $projPath -Parent
    $projName = [System.IO.Path]::GetFileNameWithoutExtension($proj)

    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "  编译 $projName" -ForegroundColor Cyan
    Write-Host "========================================" -ForegroundColor Cyan

    dotnet publish $projPath -c Release -r win-x64 --self-contained -o (Join-Path $buildDir $projName)

    if ($LASTEXITCODE -ne 0) {
        Write-Host "编译 $projName 失败!" -ForegroundColor Red
        exit 1
    }

    # 复制 exe 到 build 根目录
    $exe = Get-ChildItem -Path (Join-Path $buildDir $projName) -Filter "*.exe" | Select-Object -First 1
    if ($exe) {
        Copy-Item $exe.FullName -Destination (Join-Path $buildDir $exe.Name)
        Write-Host "  -> $($exe.Name)" -ForegroundColor Green
    }
}

Write-Host ""
Write-Host "编译完成! 产物位于: $buildDir" -ForegroundColor Green
Get-ChildItem $buildDir -Filter "*.exe" | ForEach-Object {
    Write-Host "  $($_.Name)  ($([math]::Round($_.Length / 1MB, 2)) MB)" -ForegroundColor Yellow
}
