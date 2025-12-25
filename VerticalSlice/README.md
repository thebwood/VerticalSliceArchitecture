# Vertical Slice Architecture with CQRS

This project implements **Vertical Slice Architecture** with **CQRS** (Command Query Responsibility Segregation) using **Minimal APIs** in .NET 10.

## ?? Architecture Overview

### Vertical Slice Architecture
Each feature is organized in its own folder with everything it needs:
- Endpoint (HTTP mapping)
- Query/Command (request definition)
- Handler (business logic)
- Validator (FluentValidation rules)
- Response DTOs (data transfer objects)

### CQRS Pattern
- **Queries**: Read operations (GET) - return data
- **Commands**: Write operations (POST, PUT, DELETE) - modify data

### Result Pattern
All handlers return `Result<T>` or `Result` which encapsulates:
- Success/Failure state
- Value (on success)
- Error information (on failure with error type: NotFound, Validation, Conflict, Failure)

### FluentValidation Integration
- **Automatic validation** via MediatR pipeline behavior
- **Validators live with their commands** in the same slice
- **Validation errors** automatically converted to Result.Failure
- **Clean separation** - handlers only contain business logic

## ?? Project Structure

```
VerticalSlice.Api/
??? Domain/
?   ??? Address.cs                    # Domain entity
??? Common/
?   ??? AddressDbContext.cs           # EF Core DbContext
?   ??? IEndpoint.cs                  # Endpoint interface
?   ??? Result.cs                     # Result pattern implementation
?   ??? Error.cs                      # Error types and factory methods
??? Contracts/
?   ??? AddressResponse.cs            # Shared DTOs
??? Behaviors/
?   ??? ValidationBehavior.cs         # MediatR pipeline for FluentValidation
??? Extensions/
?   ??? EndpointExtensions.cs         # Auto-discovery of endpoints
?   ??? ResultExtensions.cs           # Convert Result to HTTP responses
??? Middleware/
?   ??? ExceptionHandlingMiddleware.cs # Global exception handling
?   ??? RequestLoggingMiddleware.cs    # Request/response logging
??? Features/
    ??? Addresses/
        ??? GetAllAddresses/
        ?   ??? GetAllAddressesEndpoint.cs
        ?   ??? GetAllAddressesQuery.cs
        ?   ??? GetAllAddressesHandler.cs
        ??? GetAddressById/
        ?   ??? GetAddressByIdEndpoint.cs
        ?   ??? GetAddressByIdQuery.cs
        ?   ??? GetAddressByIdHandler.cs
        ??? CreateAddress/
        ?   ??? CreateAddressEndpoint.cs
        ?   ??? CreateAddressCommand.cs
        ?   ??? CreateAddressHandler.cs
        ?   ??? CreateAddressValidator.cs      ? FluentValidation
        ??? UpdateAddress/
        ?   ??? UpdateAddressEndpoint.cs
        ?   ??? UpdateAddressCommand.cs
        ?   ??? UpdateAddressHandler.cs
        ?   ??? UpdateAddressValidator.cs      ? FluentValidation
        ??? DeleteAddress/
            ??? DeleteAddressEndpoint.cs
            ??? DeleteAddressCommand.cs
            ??? DeleteAddressHandler.cs
```

## ?? Key Features

### 1. Result Pattern
Provides consistent error handling across the application:

```csharp
// Success
return Result<AddressResponse>.Success(address);

// Failure with specific error type
return Result<AddressResponse>.Failure(
    Error.NotFound("Address.NotFound", $"Address with ID {id} was not found"));
```

### 2. FluentValidation
Each command has its own validator in the same slice:

```csharp
public class CreateAddressValidator : AbstractValidator<CreateAddressCommand>
{
    public CreateAddressValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street is required")
            .MaximumLength(200);

        RuleFor(x => x.ZipCode)
            .Matches(@"^\d{5}(-\d{4})?$")
            .WithMessage("Zip code must be in format 12345 or 12345-6789");
    }
}
```

**Benefits:**
- ? Automatic validation via MediatR pipeline
- ? Validators co-located with commands
- ? No validation logic in handlers
- ? Consistent error responses

### 3. Automatic Error Mapping
Results are automatically converted to appropriate HTTP responses:
- `NotFound` ? 404
- `Validation` ? 400
- `Conflict` ? 409
- `Failure` ? 500

### 4. Middlewares

**ExceptionHandlingMiddleware**
- Catches unhandled exceptions
- Logs errors
- Returns consistent error responses
- Maps exception types to HTTP status codes

**RequestLoggingMiddleware**
- Logs all incoming requests
- Tracks request duration
- Includes unique request ID for correlation
- Logs response status codes

**ValidationBehavior (MediatR Pipeline)**
- Intercepts all MediatR requests
- Runs FluentValidation validators automatically
- Converts validation failures to Result.Failure
- Executed before handlers

### 5. MediatR Integration
Uses MediatR for CQRS implementation:
- Decouples HTTP layer from business logic
- Pipeline behaviors for cross-cutting concerns
- Clean separation of concerns

### 6. Swagger UI
Interactive API documentation available at the root URL when running in development mode.

## ?? API Endpoints

| Method | Endpoint | Description | Response |
|--------|----------|-------------|----------|
| GET | `/api/addresses` | Get all addresses | 200 OK |
| GET | `/api/addresses/{id}` | Get address by ID | 200 OK / 404 Not Found |
| POST | `/api/addresses` | Create new address | 201 Created / 400 Bad Request |
| PUT | `/api/addresses/{id}` | Update address | 200 OK / 400 Bad Request / 404 Not Found |
| DELETE | `/api/addresses/{id}` | Delete address | 204 No Content / 404 Not Found |

## ?? NuGet Packages

- **MediatR** - CQRS implementation
- **FluentValidation** - Declarative validation rules
- **FluentValidation.DependencyInjectionExtensions** - DI integration
- **Microsoft.EntityFrameworkCore.SqlServer** - Database access
- **Microsoft.EntityFrameworkCore.Design** - EF Core tools
- **Swashbuckle.AspNetCore** - Swagger/OpenAPI documentation

## ?? Configuration

### Database Connection
Update `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\ProjectModels;Database=AddressDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

### Running the Application
1. The application will start with Swagger UI at the root URL (e.g., `https://localhost:5001/`)
2. All API endpoints are documented and testable through the Swagger interface

## ?? Error Response Format

**Validation Error:**
```json
{
  "error": "Validation.Failed",
  "message": "Street is required; Zip code must be in format 12345 or 12345-6789"
}
```

**Not Found Error:**
```json
{
  "error": "Address.NotFound",
  "message": "Address with ID {id} was not found"
}
```

## ??? Adding a New Feature

1. Create a new folder under `Features/`
2. Create endpoint, query/command, handler, and validator files
3. Implement `IEndpoint` interface
4. Validators and handlers are auto-discovered via reflection

Example structure:
```
Features/YourFeature/
??? YourFeatureEndpoint.cs
??? YourFeatureCommand.cs
??? YourFeatureValidator.cs       ? FluentValidation
??? YourFeatureHandler.cs
```

## ?? Best Practices

- Each slice is independent and self-contained
- Commands and Queries are separate
- Result pattern ensures consistent error handling
- Validators are co-located with commands/queries
- Handlers contain only business logic (no validation)
- Endpoints are thin - just HTTP mapping
- DTOs are shared in Contracts folder

## ?? CORS Configuration

CORS is configured to allow all origins in development. Update the policy in `Program.cs` for production.

## ?? Logging

All requests are logged with:
- Request ID (for correlation)
- HTTP method and path
- Response status code
- Request duration

## ?? Middleware Order

1. **ExceptionHandlingMiddleware** - Catches all unhandled exceptions
2. **RequestLoggingMiddleware** - Logs request/response
3. **ValidationBehavior (MediatR Pipeline)** - Validates commands/queries
4. CORS
5. HTTPS Redirection
6. Endpoints
7. Swagger UI (Development only)

## ? Validation Example

Creating an address with validation:

**Request:**
```json
POST /api/addresses
{
  "street": "",
  "city": "New York",
  "state": "NY",
  "zipCode": "invalid",
  "country": "USA"
}
```

**Response (400 Bad Request):**
```json
{
  "error": "Validation.Failed",
  "message": "Street is required; Zip code must be in format 12345 or 12345-6789"
}
```

## ?? Swagger Documentation

When running in development mode:
- Swagger UI is available at the root URL (`/`)
- Interactive API documentation
- Test endpoints directly from the browser
- View request/response schemas
- See validation requirements
