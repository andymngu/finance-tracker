# ClearWealth — Personal Finance Tracker

A full-stack application for tracking and managing personal finances with real-time market data and transaction insights.

## Tech Stack

- **Backend**: .NET 10 (C# 14)
- **Frontend**: React 19 (TypeScript, Vite)
- **Authentication**: JWT (JSON Web Tokens)
- **Data**: Currently using in-memory stubs; migrating to EF Core + SQL in Phase 1

## Project Structure

```
finance-tracker/
├── src/
│   ├── ClearWealth.Api/          # ASP.NET Core API
│   ├── ClearWealth.Application/  # Business logic & services
│   ├── ClearWealth.Domain/       # Domain entities
│   └── frontend/                 # React application
├── README.md
└── .gitignore
```

## Development Environment Setup

### Prerequisites

- **.NET 10 SDK** ([Download](https://dotnet.microsoft.com/download/dotnet/10.0))
- **Node.js 20+** & **npm** ([Download](https://nodejs.org/))
- **Visual Studio 2022+** or **VS Code**

### Backend Setup

1. **Restore .NET packages:**
   ```powershell
   dotnet restore
   ```

2. **Configure JWT Secret using User Secrets:**

   ```powershell
   # Navigate to the API project
   cd src/ClearWealth.Api

   # Set the JWT secret (generates a strong one)
   dotnet user-secrets set "Jwt:Secret" "your-super-secret-key-at-least-32-characters-long-change-this"
   ```

   **⚠️ IMPORTANT:**
   - User Secrets are stored securely in your user profile, **never** in version control
   - Generate a strong secret (at least 32 characters)
   - Example: `openssl rand -base64 32` (on macOS/Linux) or use [this generator](https://www.lastpass.com/features/password-generator)
   - Each developer sets their own local secret
   - Production uses secure environment variables/Key Vault

3. **Run the API:**
   ```powershell
   dotnet run --project src/ClearWealth.Api
   ```

   The API will start on `https://localhost:5001` and `http://localhost:5000`
   - Swagger UI: `https://localhost:5001/swagger`

### Frontend Setup

1. **Navigate to frontend directory:**
   ```powershell
   cd src/frontend
   ```

2. **Install dependencies:**
   ```powershell
   npm install
   ```

3. **Start dev server:**
   ```powershell
   npm run dev
   ```

   The frontend will start on `http://localhost:3000`

## Authentication

### Demo Credentials

```
Email: demo@clearwealth.com
Password: password
```

### Login Flow

1. **POST to `/api/auth/login`** with credentials
2. **Receive JWT token** (valid for 7 days)
3. **Include token in subsequent requests:**
   ```
   Authorization: Bearer <your-jwt-token>
   ```

### Testing with Swagger

1. Navigate to `https://localhost:5001/swagger`
2. Click the lock icon (🔒) and paste your JWT token
3. Make authenticated requests

## API Endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `POST` | `/api/auth/login` | ❌ | Authenticate and receive JWT |
| `GET` | `/api/accounts` | ✅ | Get user's accounts |
| `POST` | `/api/accounts` | ✅ | Add new account |
| `GET` | `/api/transactions` | ✅ | Get user's transactions |

## Development Notes

### Phase 1 Roadmap

- [ ] Replace in-memory stubs with EF Core
- [ ] Implement SQL Server database
- [ ] Add real user registration
- [ ] Implement password hashing (BCrypt)
- [ ] Add Plaid API integration
- [ ] Database migrations & seeding

### Running Tests

```powershell
dotnet test
```

### Build for Release

```powershell
dotnet publish -c Release
```

## Troubleshooting

**CORS errors when calling API from frontend?**
- Ensure the backend is running on `https://localhost:5001` or `http://localhost:5000`
- Check `src/ClearWealth.Api/Program.cs` CORS policy allows `http://localhost:3000`

**JWT Secret not configured?**
- Set it using: `dotnet user-secrets set "Jwt:Secret" "your-secret-key"`
- Make sure you're in the `src/ClearWealth.Api` directory
- User Secrets are stored securely in your local user profile, not in the repo

**Port already in use?**
- Backend: Change port in `src/ClearWealth.Api/Properties/launchSettings.json`
- Frontend: `npm run dev -- --port 3001`

**Login returns Unauthorized?**
- Verify email is `demo@clearwealth.com` and password is `password`
- Check `StubUserRepository` for valid test users

## License

MIT