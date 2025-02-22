# MagniseTask
## 1. Clone the Repository
Clone the project repository to your local machine:

    git clone https://github.com/insxmniahauntsme/MagniseTask.git

    cd MagniseTask

## 2. Setting up all dependencies
### 2.1 appsettings.json
Open the project in your IDE

Your appsetting.json file should look like this

    {
        "Logging": {
            "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
            }
        },
        "AllowedHosts": "*",
        "ConnectionStrings": {
            "DefaultConnection": "Server=localhost,YourPort;Database=YourDbName;User ID=YourUserId;Password=YourPassword;TrustServerCertificate=True"
    
        },
        "FintachartsAPI": {
            "Uri": "https://platform.fintacharts.com",
            "WSS_Uri": "wss://platform.fintacharts.com",
            "credentials": {
            "username": "YourUsername",
            "password": "YourPassword"
            }
        }
    }

Update the "ConnectionStrings" and "FintachartsAPI" sections with actual data.

### 2.2 EFCore Migrations

You need to create a migration by typing the following in the terminal:

    dotnet ef migrations add Init

And after that confirm the migration:

    dotnet ef database update Init

This steps should ensure that the database is set up correctly.

## 3. Running the Application with Docker
To build and run the application in a Docker container:

### 3.1 Build the Docker Image
Ensure Docker is installed and running, then run the following command to build the image:

    docker build -t magnisetask .

This will build the Docker image for the project.

### 3.2 Run the Docker Container
Once the image is built, you can run it with:

    docker run -d -p 8080:8080 -p 8081:8081 --name magnisetask magnisetask

This will run the container in detached mode and map ports 8080 and 8081 on the container to port 8080 and 8081 on your machine.

### 3.3 Verify the Application is Running
You can check the logs of the container to confirm the application is running:

    docker logs -f magnisetask

You should see logs indicating the application is listening on http://localhost:8080.

## 4. Testing the API

Once the container is running, you can test the API using Postman or another HTTP client.

Authentication Endpoint:
GET http://localhost:8080/api/authentication

This will check if the authentication endpoint is functioning correctly.
