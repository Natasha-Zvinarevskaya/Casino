# === CONFIG ===
# Путь к локальному NuGet фиду (не меняется)
$source = "D:\Натаха\Работа\Nuggets"

# === AUTO-DETECT PROJECT FILE ===
$projectFile = Get-ChildItem -Filter *.csproj | Select-Object -First 1

if (-not $projectFile) {
    Write-Host "❌ Не найден .csproj файл!"
    exit 1
}

Write-Host "Проект: $($projectFile.Name)"

# === GET VERSION FROM CSPROJ ===
[xml]$xml = Get-Content $projectFile.FullName
$version = $xml.Project.PropertyGroup.Version

if (-not $version) {
    Write-Host "❌ В .csproj нет <Version>!"
    exit 1
}

Write-Host "Версия пакета: $version"

# === PACK ===
Write-Host "📦 Создание NuGet пакета..."
dotnet pack $projectFile.FullName -c Release --output nupkgs

# === PACKAGE PATH ===
$packageName = "$($projectFile.BaseName).$version.nupkg"
$packagePath = "nupkgs/$packageName"

if (-not (Test-Path $packagePath)) {
    Write-Host "❌ Пакет не найден: $packagePath"
    exit 1
}

Write-Host "🎉 Пакет создан: $packagePath"

# === PUSH ===
Write-Host "⬆️ Отправка пакета в локальный фид..."
dotnet nuget push $packagePath --source $source

Write-Host "✅ Готово!"