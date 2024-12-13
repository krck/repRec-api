# repRec

# Table of Contents

- [1 About](#1-about)
- [2 Local Setup](#1-local-setup)
    - [2.1 Frontend](#21-frontend)
    - [2.2 Backend](#22-backend)
    - [2.3 Docker](#23-docker)
        - [2.3.1 Frontend Container](#231-frontend-container)
        - [2.3.2 Backend Container](#232-backend-container)
        - [2.3.3 All Containers](#233-all-containers)
- [3 Server Setup](#3-server-setup)
    - [3.1 SSH](#31-ssh)
        - [3.1.1 SSH Setup](#311-ssh-setup)
        - [3.1.2 Simple SSH connection](#312-simple-ssh-connection)
        - [3.1.3 SSH Key setup and connection](#313-ssh-key-setup-and-connection)
    - [3.2 Firewall](#32-firewall)
- [4 Development Workflows](#4-development-workflows)
    - [4.1 Database](#41-database)
    - [4.2 CI/CD](#42-cicd)
    - [4.3 Authentication](#43-authentication)
    - [4.4 HTTPS](#44-https)


# 1 About
...


# 2 Local Setup
Setup Frontend and Backend on the development machine, running Debian 12

The project directory structure separates frontend and backend, with both having their own git repository (but the docker configuration is part of the backend git for simplicity):
<pre>
 /repRec
 |── /repRec-frontend
 └── /repRec-api
     └── /_docker
</pre>

## 2.1 Frontend
Using TypeScript and Angular 17
....

## 2.2 Backend
Using C# and .NET 9
- https://learn.microsoft.com/en-us/dotnet/core/install/linux-debian?tabs=dotnet9
- Linux: Add the package key to the apt repo
- Install the SDK: > sudo apt-get update && \ sudo apt-get install -y dotnet-sdk-9.0
- Install the runtime: > sudo apt-get update && \ sudo apt-get install -y aspnetcore-runtime-9.0
- https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-9.0&tabs=visual-studio-code

## 2.3 Docker
Docker will provide the containerization platform needed to run the website in isolated environments.
Docker Compose helps us manage multiple services (frontend, backend, database) in a single configuration.

Verify the installation of docker and docker-compose:
`docker --version`
`docker-compose --version`

All relevant commands to view, stop or clear running docker containers
`docker ps -a`

`docker stop "CONTAINER ID"`

`docker stop $(docker ps -a -q) // Stop all`

`systemctl restart docker.socket docker.service // Restart service`

`docker container prune -f`

`docker builder prune --all`
  
### 2.3.1 Frontend Container
Defined in "_docker/Dockerfile.frontend"
- Use a Node.js (v18) image to install dependencies and build the Angular app
- The app is then served by Nginx, a lightweight web server that efficiently serves static files

Create, test and run the frontend container
> switch to admin in cmd and go the the api main "repRec" parent folder
- build: `docker build -f repRec-api/_docker/Dockerfile.frontend -t reprec-frontend .`
- run: `docker run -d -p 8080:80 reprec-frontend`
- run (custom nginx conf): `docker run -v repRec-api/_docker/nginx.conf:/etc/nginx/nginx.conf:ro -d -p 8080:80 reprec-frontend`

Container will now be accessible at http://localhost:80/

### 2.3.2 Backend Container
Defined in "_docker/Dockerfile.backend"
- Build the .NET app inside the container using the official SDK 9.0 image
- Publish the app to a folder (/app/publish)
- Use a "light" runtime image to run the app and expose port 5205

Create, test and run the backend container
> switch to admin in cmd and go the the api main "repRec" parent folder
- build: `docker build -f repRec-api/_docker/Dockerfile.backend -t reprec-backend .`
- run: `docker run -d -p 5205:8080 reprec-backend`

Container will now be accessible at http://localhost:5205/

### 2.3.3 All Containers
> Switch to admin in cmd and go the the api main "repRec" parent folder

Build the images (if needed) and starts the services defined in the docker-compose.yml file

`docker-compose -f repRec-api/_docker/docker-compose.yml up`

Run the containers in the background (useful for keeping your terminal free)

`docker-compose -f repRec-api/_docker/docker-compose.yml up -d`

Force a rebuild of the Docker images and starts the containers

`docker-compose -f repRec-api/_docker/docker-compose.yml up --build`

Stop the running containers gracefully

`docker-compose -f repRec-api/_docker/docker-compose.yml down`

Stop and remove all containers, networks, and volumes (useful for a clean slate)

`docker-compose -f repRec-api/_docker/docker-compose.yml down -v`

Lists the containers currently managed by the compose setup

`docker-compose -f repRec-api/_docker/docker-compose.yml ps`


# 3 Server Setup
Setup the hosting environment (Hetzner in this case) on a VPS running Debian 12
Server IPv4: 188.245.213.112
Server IPv6: 2a01:4f8:1c1c:d852::/64

Create the main folder for the web hosting
`mkdir -p /var/www/repRec`

> Both git repos will be cloned to that location, so that the folder structure mimics the dev machine.

From this main folder, the docker containers will be run.
`cd /var/www/repRec`
`docker-compose -f repRec-api/_docker/docker-compose.yml up -d`
`docker-compose logs -f`


## 3.1 SSH

### 3.1.1 SSH Setup

Install SSH on the server side (allows the server to accept connections)
`apt install openssh-server`

Install SSH on the client side (allows the client to create connections)
`apt install openssh-client`

Enable SSH services, activate on boot and verify if its running
`systemctl start ssh`
`systemctl enable ssh`
`systemctl status ssh`

### 3.1.2 Simple SSH connection

Simple ssh connect to the server from the client, with username and password
`ssh login-user@server-ip`

### 3.1.3 SSH Key setup and connection

Create a local ssh key
`ssh-keygen -t rsa -b 4096`
> Press Enter to save the key in the default location (~/.ssh/id_rsa).
> Optionally, set a passphrase for additional security.

Copy the public key to the VPS
`ssh-copy-id login-user@server-ip`

Test the connection: This should now work, without requiring a password
`ssh login-user@server-ip`

### 3.1.4 Secure the SSH connection (no more PW login)

Update the Firewall to allow a new port (optional but recommended)
`ufw allow 2222`

Edit the SSH Config File and change these settings
`nano /etc/ssh/sshd_config`

Disable password logins, allow only one user and change the default SSH port (optional but recommended)
> PasswordAuthentication no
> AllowUsers login-user
> Port 2222

Save and close the file (Ctrl+O, Enter, Ctrl+X).
Then restart the SSH service.
`systemctl restart ssh`

Deny the default port (22)
`ufw delete allow ssh`

Test the secure connection
`ssh -p 2222 login-user@server-ip`

To verify that password login is disabled: Try connecting without your SSH key. It should fail.


## 3.2 Firewall

Install the ufw (Uncomplicated Firewall) tool
`apt install ufw -y`

Configure firewall rules for ssh and web-hosting
`ufw allow OpenSSH`
`ufw allow ssh`
`ufw allow 80`
`ufw allow 443`

Activate and check the status
`ufw enable`
`ufw status`


# 4 Development Workflows
Recurring workflows during development, from updating the database to updating the live website

## 4.1 Database

The Entity Framework is used with a Code-First approach, meaning that database changes/updates have to be pushed from the local development machine in this way.

> cmd must be in the main "repRec-api" folder, since relative paths are used

Once database changes have been added to the `RepRecDbContext`, a new migration has to be crated:
`dotnet-ef migrations add MIGRATION_NAME -o Database/Migrations/`

The new migration can be applied to the database by running:
`dotnet-ef database update`

Otherwise, migrations can be removed or checked by running (remove always removes the last migration):
`dotnet-ef migrations remove`
`dotnet-ef migrations list`


## 4.2 CI/CD

There will be no CI/CD setup as of now, but there is a simple server-side refresh script:
- Pulls all repositories (frontend/backend)
    - In case of nasty conflicts use `git pull --rebase` manually
- Shuts down all running containers
- Forces a full rebuild of all containers with the new code
- Starts and runs the updated containers (and outputs a list of all running ones)

Located in the main folder on the server:
`/var/www/repRec/update.sh`

Update script:
> `#!/bin/bash`
>
> `# Pull latest changes`
> `cd /var/www/repRec/repRec-api && git pull`
> `cd /var/www/repRec/repRec-frontend && git pull`
>
> `# Rebuild and restart containers`
> `cd /var/www/repRec`
> `docker-compose -f repRec-api/_docker/docker-compose.yml down -v`
> `docker-compose -f repRec-api/_docker/docker-compose.yml build --no-cache`
> `docker-compose -f repRec-api/_docker/docker-compose.yml up -d`
> `docker-compose -f repRec-api/_docker/docker-compose.yml ps`


## 4.3 Authentication

Authentication is done via Auth0 (free tier)
...

## 4.4 HTTPS

To use Auth0 in a dev/prod environment, HTTPS (SSL) certificate is required.
To configure an SSL certificate, it makes sense to have a fixed domain registered.

Domain certificate setup with `certbot`
> Certificate location
> ssl_certificate /etc/letsencrypt/live/www.reprec.com/fullchain.pem;
> ssl_certificate_key /etc/letsencrypt/live/www.reprec.com/privkey.pem;

Also added `nginx`hosting to the whole VPS, not just within the docker container.
> nginx config location
> /etc/nginx/sites-available/default




<b>TODOS:</b>
- Harden/Secure Server
- Logging
- Error Handling
