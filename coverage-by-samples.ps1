dotnet test ./src/Samples/TestStack.BDDfy.Samples/TestStack.BDDfy.Samples.csproj `
    --collect:"XPlat Code Coverage" `
    --results-directory ./TestResults `
    --settings ./src/default.runsettings

reportgenerator `
    -reports:./TestResults/**/coverage.cobertura.xml `
    -targetdir:./CoverageReport `
    -reporttypes:Html

Remove-Item -Recurse -Force ./TestResults