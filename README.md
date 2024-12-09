# repRec-api

# Setup

### API 
Using .net 9
- https://learn.microsoft.com/en-us/dotnet/core/install/linux-debian?tabs=dotnet9
- Linux: Add the package key to the apt repo
- Install the SDK:      > sudo apt-get update && \ sudo apt-get install -y dotnet-sdk-9.0
- Install the runtime:  > sudo apt-get update && \ sudo apt-get install -y aspnetcore-runtime-9.0
- https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-9.0&tabs=visual-studio-code

### Docker
Docker will provide the containerization platform needed to run the website in isolated environments. 
Docker Compose helps us manage multiple services (frontend, backend, database) in a single configuration.

Verify the installation of docker and docker-compose:
> docker --version
> docker-compose --version

### Project Structure

The project directory structure separates frontend and backend, with both having their own git repository (but the docker configuration is part of the backend git for simplicity):
/repRec
|── /repRec-frontend
└── /repRec-api
    └── /_docker
