dotnet test ./src `
    --collect:"XPlat Code Coverage" `
    --results-directory ./TestResults

reportgenerator `
    -reports:./TestResults/**/coverage.cobertura.xml `
    -targetdir:./CoverageReport `
    -reporttypes:Html

Remove-Item -Recurse -Force ./TestResults