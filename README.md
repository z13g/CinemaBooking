<h1 align="center" id="title">CinemaBooking Webapp - Angular Frontend &amp; ASP.NET Core API Backend</h1>

<p align="center"><img src="https://github.com/z13g/CinemaBooking/blob/main/ProjectDisplay/cinema-booking-logo.png?raw=true" alt="project-image"></p>

<p id="description">This project is a collaborative web application for booking movie tickets featuring an Angular frontend and an ASP.NET Core API backend. It prioritizes security and scalability while offering a user-friendly booking experience.</p>

<h2>🚀 Demo</h2>

[https://github.com/z13g/CinemaBooking/blob/main/ProjectDisplay/demo.mp4](https://github.com/z13g/CinemaBooking/blob/main/ProjectDisplay/demo.mp4)

<h2>Project Screenshots:</h2>

<img src="https://github.com/z13g/CinemaBooking/blob/main/ProjectDisplay/cinema-booking-frontpage.png?raw=true" alt="project-screenshot" width="800" height="400/">

<img src="https://github.com/z13g/CinemaBooking/blob/main/ProjectDisplay/cinema-booking-login.png?raw=true" alt="project-screenshot" width="800" height="400/">

<img src="https://github.com/z13g/CinemaBooking/blob/main/ProjectDisplay/cinema-booking-signup.png?raw=true" alt="project-screenshot" width="800" height="400/">

<img src="https://github.com/z13g/CinemaBooking/blob/main/ProjectDisplay/cinema-booking-show-pick.png?raw=true" alt="project-screenshot" width="800" height="400/">

<img src="https://github.com/z13g/CinemaBooking/blob/main/ProjectDisplay/cinema-booking-seat-pick.png?raw=true" alt="project-screenshot" width="800" height="400/">

  
  
<h2>🧐 Features</h2>

Here're some of the project's best features:

*   Area selection logic for a personalized experience.
*   Dynamic frontpage content based on the chosen area (movies cinemas).
*   Interactive cinema selection with up-to-date movie listings.
*   User-friendly seat reservation and booking system with temporary holds (released if not confirmed).
*   Secure site access with guard protection using JWTs.
*   HTTP interceptor for consistent API communication.
*   Admin page for management tasks (details can be added based on your implementation).
*   Attractive and user-friendly UI.
*   Robust API with well-structured controllers services and repositories.
*   Unit tests for controllers and repositories to ensure code quality.
*   Data Transfer Objects (DTOs) to prevent data leakage in API responses.
*   Secure JWT-based authentication for authorized access to endpoints.
*   Dependency injection for loose coupling and testability.
*   Secure password storage using salting and hashing.
*   Pre-built DbContext.cs with sample data

<h2>🛠️ Installation Steps:</h2>

<h3>1 Getting Started Prerequisites</h3>

```
.NET 8 SDK (download from https://dotnet.microsoft.com/en-us/download)
```

<h3>2 Backend (ASP.NET Core API)</h3>

<p>2.1 Clone the repository:</p>

```
git clone https://github.com/z13g/CinemaBooking/
```

<p>2.2 Open the solution</p>

```
Open the .sln file in Visual Studio or your preferred IDE.
```

<h4>2.3 Configure Database:</h4>

<p>2.3.1 Connection String</p>

```
Replace the connection string in appsettings.json with your desired database connection details (avoid using master database).
```

<p>2.3.2 Migrations Folder</p>

```
Delete Migrations Folder (Optional): If the Migrations folder already exists you can delete it to start fresh with the database schema.
```

<h4>2.4 Make Migrations:</h4>

<p>2.4.1 Open NuGet PMC</p>

```
Open the NuGet Package Manager Console in Visual Studio (Tools > NuGet Package Manager > Package Manager Console).
```

<p>2.4.2 Add New Migration</p>

```
Run the command add-migration [Migration name] (replace [Migration name] with a descriptive name for your initial migration). This creates migration files defining the database schema changes.
```

<p>2.5 Update Database</p>

```
After the migration files are generated run the command update-database to apply the migrations and create the database tables. This might take some time depending on the complexity of your database schema.
```

<h4>2.6 Run the application:</h4>

<p>2.6.1 Startup Project</p>

```
Set the startup project to H3CinemaBooking.API in Visual Studio.
```

<p>2.6.2 Run</p>

```
Press F5 or use the "Start Debugging" option to run the backend application.
```

<h3>3 Frontend (Angular)</h3>

<p>3.1 Navigate to Frontend Folder</p>

```
Use your terminal or command prompt to navigate to the directory containing the Angular frontend code.
```

<p>3.2 Install Dependencies</p>

```
npm install
```

<p>3.3 Start Development Server</p>

```
ng serve
```
  
<h2>💻 Built with</h2>

Technologies used in the project:

*   Angular
*   ASP.NET Core API
