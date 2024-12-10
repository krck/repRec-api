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

View, stop or clear running docker containers
> docker ps -a
> docker stop "CONTAINER ID"
> docker stop $(docker ps -a -q)                    // Stop all
> systemctl restart docker.socket docker.service    // Restart service
> docker container prune -f
> docker builder prune --all

### Project Structure
The project directory structure separates frontend and backend, with both having their own git repository (but the docker configuration is part of the backend git for simplicity):
/repRec
|── /repRec-frontend
└── /repRec-api
    └── /_docker

### Frontend Container
Defined in "_docker/Dockerfile.frontend"
- Use a Node.js (v18) image to install dependencies and build the Angular app
- The app is then served by Nginx, a lightweight web server that efficiently serves static files

Create, test and run the frontend container
- switch to admin in cmd and go to the main "repRec" parent folder
- build:    > docker build -f repRec-api/_docker/Dockerfile.frontend -t reprec-frontend .
- run:      > docker run -d -p 8080:80 reprec-frontend
            > docker run -v repRec-api/_docker/nginx.conf:/etc/nginx/nginx.conf:ro -d -p 8080:80 reprec-frontend
Container will now be accessible at http://localhost:8080/

### Backend Container
Defined in "_docker/Dockerfile.backend"
- Build the .NET app inside the container using the official SDK 9.0 image
- Publish the app to a folder (/app/publish)
- Use a "light" runtime image to run the app and expose port 80

Create, test and run the backend container
- switch to admin in cmd and go the the api main "repRec" parent folder
- build:    > docker build -f repRec-api/_docker/Dockerfile.backend -t reprec-backend .
- run:      > docker run -d -p 5205:8080 reprec-backend
Container will now be accessible at http://localhost:5205/

### All Containers
Switch to admin in cmd and go the the api main "repRec" parent folder

Build the images (if needed) and starts the services defined in the docker-compose.yml file
> docker-compose -f repRec-api/_docker/docker-compose.yml up
Run the containers in the background (useful for keeping your terminal free)
> docker-compose -f repRec-api/_docker/docker-compose.yml up -d
Force a rebuild of the Docker images and starts the containers
> docker-compose -f repRec-api/_docker/docker-compose.yml up --build

Stop the running containers gracefully
> docker-compose -f repRec-api/_docker/docker-compose.yml down
Stop and remove all containers, networks, and volumes (useful for a clean slate)
> docker-compose -f repRec-api/_docker/docker-compose.yml down -v

Lists the containers currently managed by the compose setup
> docker-compose -f repRec-api/_docker/docker-compose.yml ps
