# SongsStats
This MVC application returns statistics about an artist's songs. 

## Installation requirements
- .Net Core 3.1 SDK
- Docker set to Linux containers (NOTE: This is optional if you want to run the application via dotnet cli)
- Visual Studio 2019 (NOTE: This is optional if you want to run the application using docker or dotnet cli)

## Running the app
### Docker
- After you installed .NetCore 3.1 SDK and Docker navigate to the location of the Dockerfile (repository root folder)
- Run the following docker command in the command line ```docker build -t songsstats:latest .```
- Once the build is completed, run ```docker run -p 8080:80 -d songsstats```
- Navigate to ```localhost:8080```
- The page should display a search box where you can enter an artist's name to get statistics about their songs
  
### CLI
To run it using the CLI, navigate to the location of the SongsStats project folder and run: ```dotnet run --project SongsStats.csproj```
	
### Visual Studio 2019 
Open the sln file to load the solution and run the application via IISExpress
   
## Unit tests
Unit tests can be ran either via: 
- Visual Studio 2019 (Test Runner)
- Dotnet cli ( In the command line navigate to the SongsStats.Tests folder and run the following command: ```dotnet test .\SongsStats.Tests.csproj```)
