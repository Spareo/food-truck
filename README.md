## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Setup](#setup)
* [API](#api-documentation)
* [Helm](#helm)
* [Docker](#docker)

### General Info
The project provides an API for locating nearby food trucks
	
### Technologies
Project is created with:
* ASP.NET Core 3.1

### Setup
To run this project, clone this repo and open the solution file located in the `src` folder with Visual Studio

### API Documentation
Swagger UI Hosted API Documentation can be found [here](https://spareo.github.io/food-truck/#/)

### Helm
The service can be deployed to kubernetes via helm, please see the helm [README](helm/README.md)

### Docker
Docker images for this service can be pulled from dockerhub
```
docker pull dkhenkin/food-trucks:latest
```