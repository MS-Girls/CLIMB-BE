# CLIMB BACKEND

## Steps to Set up the Development Server

This project uses ASP.NET Core (VERSION: 8) for the backend. There are two methods that you could use to set up the development environment. One is to set up the environment locally in your machine which requires certain pre-requisites to be installed on your machine. The Second method is to use docker. The following are the steps outlined to do the same.

## Local Development Environment

### Pre-requisites

1. [**ASP.NET**](https://dotnet.microsoft.com/en-us/download): Framework for building applications with C#

2. [**Git**](https://git-scm.com/downloads): To clone the repository and for Version Control

### Steps

1. First Make sure that the above pre-requisites are installed successfully, by typing the following commands on the command line

```shell
dotnet --version
git --version
```

2. Clone the repository

```shell
git clone https://github.com/MS-Girls/CLIMB-BE
```

3. Navigate to the frontend directory and install the packages

```shell
cd CLIMB-BE
dotnet add package .
```

4. Set up environment variables

Create a ```.env``` file and the add the following environment variables

```shell
AZURE_STORAGE_CONNECTION_STRING=
AZURE_RESUMES_CONTAINER_NAME=
AZURE_PROBLEMS_CONTAINER_NAME=
AZURE_DOCUMENT_INTELLIGENCE_ENDPOINT=
AZURE_DOCUMENT_INTELLIGENCE_API_KEY=
ENDPOINTURL=
KEY=
```

5. To Start the server execute

```shell
dotnet run
```

7. The local URL where your backend is available shall be displayed in the terminal


## Using Docker

### Pre-requisites

1. Just make sure that [**docker**](https://www.docker.com/get-started/) is installed your system by typing the command ```docker -v``` on your command line

2. Also you require [**Git**](https://git-scm.com/downloads) to be installed inorder to clone the repository

### Steps

1. Clone the repository and navigate to the frontend

```shell
git clone https://github.com/MS-Girls/CLIMB-BE
cd CLIMB-BE
```

2. Set up environment variables

Create a ```.env``` file and set up the environment variables

3. To build docker image and start the container execute

```shell
docker build -t CLIMB-backend:latest .
docker run -d -p 8000:80 --env-file .env  --name CLIMB-backend CLIMB-backend:latest
```

4. Now your backend shall be accessible via [```http://localhost:8000```](http://localhost:8000)