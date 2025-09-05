# URLShortener API

<img src="https://upload.wikimedia.org/wikipedia/commons/thumb/7/7d/Microsoft_.NET_logo.svg/640px-Microsoft_.NET_logo.svg.png" alt=".Net" width="100"/>
<img src="https://i0.wp.com/alinebhen.org/wp-content/uploads/2023/03/SQLSErver.jpg" alt="MSSQL" width="270">
<img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS1VrymvWgkfMMHx3kOXpwcg9qB9Z2TcRGrxA&s" alt="Docker" width="190">

## Overview
Built a .NET Core REST API to generate short links, handle redirects, support fetching all links, and toggle link status.

## Get Started
### Prerequisites
- .Net 8 or later
- MSSQL SERVER 19 or later
- Docker DeskTop

### Clone Repository:
```   
git clone https://github.com/Diaa0011/urlShortenTask
cd urlShortenTask
```

### 🖥️ Running Locally

1. Install the necessary packages:
```
dotnet restore
```
2. Run the Project:
```
dotnet run
```
3. Adding migrations and create database:
```
dotnet ef migrations add "database setup"
dotnet ef database update
```

### 🐳 Running with Docker
1. Build and Run with Docker Compose:
```
docker-compose up --build
```
2. The API will be available at:
```
http://localhost:5000/(example)
```
3. (Optional) To run in detached mode:
```
docker-compose up -d
```
4. To stop containers:
```
docker-compose down
```