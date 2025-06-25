# 🔐 Secure Microservices Architecture

A comprehensive microservices solution demonstrating secure authentication and authorization using IdentityServer4 (Duende IdentityServer), Ocelot API Gateway, and JWT tokens.

## 🏗️ Architecture Overview

This project implements a secure microservices architecture with the following components:

┌─────────────────┐ ┌─────────────────┐ ┌─────────────────┐
│ Movies.Client │ │ Api Gateway │ │ Movies.API │
│ (MVC Client) │───▶│ (Ocelot) │───▶│ (Protected) │
└─────────────────┘ └─────────────────┘ └─────────────────┘
│ │ │
│ │ │
▼ ▼ ▼
┌─────────────────┐ ┌─────────────────┐ ┌─────────────────┐
│ IdentityServer │ │ JWT Token │ │ Database │
│ (Auth Server) │ │ Validation │ │ (Movies) │
└─────────────────┘ └─────────────────┘ └─────────────────┘


### 🔄 Authentication Flow

1. **User Login**: User authenticates through IdentityServer
2. **Token Issuance**: IdentityServer issues JWT tokens
3. **API Access**: Client uses JWT tokens to access protected APIs through the Gateway
4. **Token Validation**: API Gateway validates tokens before forwarding requests

## 📋 Prerequisites

- **.NET 9.0 SDK**
- **PostgreSQL** (for database storage)
- **Visual Studio 2022** or **VS Code**
- **Git**

## �� Quick Start

### 1. Clone the Repository

```bash
git clone <repository-url>
cd SecureMicroservices
```

### 2. Database Setup

#### Install PostgreSQL
- Download and install PostgreSQL from [postgresql.org](https://www.postgresql.org/download/)
- Create a database named `IdentityDB`

#### Update Connection Strings
Update the connection strings in the following files:

**IdentityServer/appsettings.json:**
```json
{
  "ConnectionStrings": {
    "IdentityDBConnectionString": "Server=localhost;Port=5432;Database=IdentityDB;Userid=postgres;Password=YOUR_PASSWORD;Trust Server Certificate=true"
  }
}
```

**Movies.API/appsettings.json:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=5432;Database=MoviesDB;Userid=postgres;Password=YOUR_PASSWORD;Trust Server Certificate=true"
  }
}
```

### 3. Apply Database Migrations

```bash
# IdentityServer migrations
cd IdentityServer
dotnet ef database update

# Movies.API migrations
cd ../Movies.API
dotnet ef database update
```

### 4. Trust Development Certificate

```bash
dotnet dev-certs https --trust
```

### 5. Run the Application

#### Option A: Visual Studio
1. Open `SecureMicroservices.sln` in Visual Studio
2. Right-click on the solution → "Set Startup Projects..."
3. Select "Multiple startup projects"
4. Set the following projects to "Start":
   - `IdentityServer`
   - `Movies.API`
   - `ApiGateaway`
   - `Movies.Client`
5. Press F5 to run

#### Option B: Command Line
Open **4 separate terminal windows** and run:

**Terminal 1 - IdentityServer:**
```bash
cd IdentityServer
dotnet run
```

**Terminal 2 - Movies.API:**
```bash
cd Movies.API
dotnet run
```

**Terminal 3 - API Gateway:**
```bash
cd ApiGateaway
dotnet run
```

**Terminal 4 - Movies.Client:**
```bash
cd Movies.Client
dotnet run
```

### 6. Access the Application

| Service | URL | Description |
|---------|-----|-------------|
| Movies.Client | https://localhost:5002 | Main web application |
| IdentityServer | https://localhost:5005 | Authentication server |
| Movies.API | https://localhost:5001 | Protected API |
| API Gateway | https://localhost:5010 | Reverse proxy |

## �� Configuration

### IdentityServer Configuration

The IdentityServer is pre-configured with:

- **Clients:**
  - `movieClient` (Client Credentials flow)
  - `movies_mvc_client` (Hybrid flow for web app)

- **Scopes:**
  - `movieAPI` (API access)
  - `openid`, `profile`, `email`, `address`, `roles`

- **Users:** Created automatically during seeding

### API Gateway Routes

The API Gateway routes requests as follows:

| Upstream Path | Downstream Path | Service |
|---------------|-----------------|---------|
| `/movies` | `/api/movies` | Movies.API |
| `/movies/{id}` | `/api/movies/{id}` | Movies.API |
| `/identity` | `/api/identity` | Movies.API |

## 🧪 Testing the Application

### 1. Test Authentication Flow

1. Navigate to https://localhost:5002
2. You should be redirected to IdentityServer login
3. Login with test credentials (see User Management section)
4. After successful login, you'll be redirected back to the movies page

### 2. Test API Access

#### Direct API Access (Unauthorized)
```bash
curl -k https://localhost:5001/api/movies
# Should return 401 Unauthorized
```

#### Through Gateway (Unauthorized)
```bash
curl -k https://localhost:5010/movies
# Should return 401 Unauthorized
```

#### With Valid Token
```bash
# Get token from browser (F12 → Application → Cookies)
curl -H "Authorization: Bearer YOUR_TOKEN" https://localhost:5010/movies
# Should return movies data
```

### 3. Test Admin Access

1. Login with an admin user
2. Navigate to the "Admin Only" page
3. You should see user information and claims

## 👥 User Management

### Default Users

The application creates a default user during seeding:

- **Email:** `test@example.com`
- **Password:** `Test123!`
- **Role:** `admin`

### Adding New Users

#### Option 1: Through Code
Add users in `IdentityServer/DataContexts/IdentityServerDbSeeder.cs`:

```csharp
private static void SeedTestUsers(IServiceProvider serviceProvider)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    
    if (!userManager.Users.Any())
    {
        var user = new ApplicationUser
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            EmailConfirmed = true
        };
        
        var result = userManager.CreateAsync(user, "Admin123!").Result;
        
        if (result.Succeeded)
        {
            userManager.AddToRoleAsync(user, "admin").Wait();
        }
    }
}
```

#### Option 2: Through IdentityServer UI
1. Navigate to https://localhost:5005
2. Use the registration page to create new users

## 🔍 Troubleshooting

### Common Issues

#### 1. Database Connection Issues

**Solution:**
- Ensure PostgreSQL is running
- Verify connection string in `appsettings.json`
- Check database exists: `createdb IdentityDB`

#### 2. Port Conflicts

**Solution:**
```bash
# Find processes using the port
netstat -ano | findstr :5005

# Kill the process
taskkill /PID <PID> /F
```

#### 3. HTTPS Certificate Issues

**Solution:**
```bash
dotnet dev-certs https --trust
```

#### 4. JWT Token Issues

**Solution:**
- Check if IdentityServer is running on port 5005
- Verify client configurations in IdentityServer
- Check token expiration
- Ensure proper scope is requested

#### 5. Migration Issues

**Solution:**
```bash
# Remove existing database
dropdb IdentityDB
createdb IdentityDB

# Reapply migrations
dotnet ef database update
```

### Debug Mode

Enable detailed logging by updating `appsettings.Development.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information",
      "Duende.IdentityServer": "Debug",
      "Ocelot": "Debug"
    }
  }
}
```

## 📁 Project Structure
SecureMicroservices/
├── IdentityServer/ # Authentication server
│ ├── DataContexts/ # Database contexts
│ ├── Entities/ # User entities
│ ├── Migrations/ # Database migrations
│ └── Pages/ # IdentityServer UI
├── Movies.API/ # Protected API
│ ├── Controllers/ # API controllers
│ ├── DataContexts/ # Database contexts
│ ├── Entities/ # Movie entities
│ └── Models/ # API models
├── Movies.Client/ # Web client
│ ├── Controllers/ # MVC controllers
│ ├── Infrastructure/ # HTTP clients
│ ├── Services/ # API services
│ └── Views/ # Razor views
├── ApiGateaway/ # API Gateway (Ocelot)
│ └── ocelot.json # Gateway configuration
└── README.md # This file


## 🔐 Security Features

- **JWT Token Authentication**: Secure token-based authentication
- **OAuth2/OpenID Connect**: Industry-standard protocols
- **Scope-Based Authorization**: Fine-grained access control
- **HTTPS Enforcement**: All communications encrypted
- **Token Validation**: Automatic token validation at gateway
- **Role-Based Access**: User roles and permissions

## 🚀 Production Deployment

### Security Considerations

1. **Replace Development Certificate:**
   ```csharp
   // Replace in IdentityServer/Program.cs
   .AddDeveloperSigningCredential()
   // With:
   .AddSigningCredential("your-production-certificate")
   ```

2. **Update Connection Strings:**
   - Use production database
   - Use strong passwords
   - Enable SSL

3. **Configure HTTPS:**
   - Use proper SSL certificates
   - Configure reverse proxy (nginx, IIS)

4. **Environment Variables:**
   - Store sensitive data in environment variables
   - Use Azure Key Vault or similar for secrets

### Docker Support

To containerize the application:

```dockerfile
# Example Dockerfile for Movies.API
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Movies.API/Movies.API.csproj", "Movies.API/"]
RUN dotnet restore "Movies.API/Movies.API.csproj"
COPY . .
WORKDIR "/src/Movies.API"
RUN dotnet build "Movies.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Movies.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Movies.API.dll"]
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🆘 Support

If you encounter any issues:

1. Check the troubleshooting section above
2. Review the logs in debug mode
3. Create an issue with detailed error information
4. Include your environment details (OS, .NET version, etc.)

## 🔗 Useful Links

- [Duende IdentityServer Documentation](https://docs.duendesoftware.com/)
- [Ocelot Documentation](https://ocelot.readthedocs.io/)
- [ASP.NET Core Authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/)
- [JWT.io](https://jwt.io/) - JWT token decoder
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)

---

**Happy Coding! 🚀**