# Clean Architecture Solution

## Project Structure
- `CleanArchitecture.Core` - Domain models and interfaces
- `CleanArchitecture.Application` - Application logic and use cases
- `CleanArchitecture.Infrastructure` - External dependencies and implementations
- `CleanArchitecture.Web` - API layer (ASP.NET Core) and 
- `CleanArchitecture.Web.Client` - Angular frontend
```
CleanArchitectureSolution/
├── CleanArchitecture.Core/           # Domain layer
│   ├── Entities/
│   ├── Interfaces/
│   └── Exceptions/
├── CleanArchitecture.Application/    # Application layer
│   ├── DTOs/
│   ├── Interfaces/
│   └── Services/
├── CleanArchitecture.Infrastructure/ # Infrastructure layer  
│   ├── Data/
│   └── Repositories/
├── CleanArchitecture.Web/            # API Layer (ASP.NET Core)
│   ├── Controllers/
│   ├── Program.cs
│   └── Properties/
└── CleanArchitecture.Web.Client/     # Angular Frontend (separate project)
    ├── src/
    ├── package.json
    └── angular.json
```


# Act Testing
