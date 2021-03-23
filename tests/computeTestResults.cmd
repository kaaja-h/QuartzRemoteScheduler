dotnet tool install --global dotnet-reportgenerator-globaltool
pushd ..\src\
dotnet test /p:CollectCoverage=true /p:CoverletOutput=../../tests/results/ /p:CoverletOutputFormat=opencover
popd
reportgenerator -reports:results/coverage.net5.0.opencover.xml -targetdir:coveragereport -reporttypes:Html -historydir:history