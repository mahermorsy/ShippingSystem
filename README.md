# Shipping Management System

Shipping Management System is a portfolio web application for creating, managing, and tracking shipments. It combines an ASP.NET Core MVC frontend with a separate ASP.NET Core Web API backend, SQL Server persistence, authentication, and PayPal Sandbox checkout integration.

## Tech Stack

- ASP.NET Core MVC and ASP.NET Core Web API
- Entity Framework Core with SQL Server
- ASP.NET Core Identity, cookies, and JWT authentication
- Layered architecture with Domains, DataAccessLayer, BusinessLayer, UI, and MVC frontend projects
- PayPal Sandbox Orders API integration
- Bootstrap, jQuery, SweetAlert2, and Razor views

## Features

- User registration, login, authorization, and token refresh flow
- Shipment creation workflow with sender, receiver, package, shipping, review, and payment steps
- Shipment listing, details, editing, and deletion flows
- Admin/service modules for countries, cities, carriers, shipping types, packaging, payment methods, settings, and subscriptions
- Standalone PayPal Sandbox payment test page for isolating payment issues
- Swagger-enabled Web API for endpoint exploration

## Local Setup

1. Install prerequisites:
   - .NET SDK compatible with `net10.0`
   - SQL Server or SQL Server Express
   - Node.js only if you want to restore frontend package dependencies

2. Restore and build:

   ```powershell
   dotnet restore ShippingSystem.slnx
   dotnet build ShippingSystem.slnx -m:1
   ```

3. Configure local secrets. Do not commit real secrets to GitHub.

   Update local development settings or use user secrets/environment variables for:

   ```json
   {
     "ConnectionStrings": {
       "Shipping": "Server=.;Database=Project_ShippingDatabase;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;"
     },
     "Jwt": {
       "Key": "YOUR_LOCAL_LONG_JWT_SIGNING_KEY"
     },
     "PayPal": {
       "ClientId": "YOUR_PAYPAL_SANDBOX_CLIENT_ID",
       "Secret": "YOUR_PAYPAL_SANDBOX_SECRET",
       "Environment": "sandbox"
     }
   }
   ```

4. Run the API:

   ```powershell
   dotnet run --project UI\WebApi.csproj --launch-profile https
   ```

   Default Swagger URL:

   ```text
   https://localhost:7007/swagger
   ```

5. Run the MVC frontend:

   ```powershell
   dotnet run --project ShippingSystem\UI.csproj --launch-profile https
   ```

   Default frontend URL:

   ```text
   https://localhost:7279
   ```

## PayPal Sandbox Notes

- This project is configured for PayPal Sandbox only.
- Use a sandbox Business account for the app credentials.
- Use a sandbox Personal account or PayPal test card for checkout testing.
- If PayPal returns `COMPLIANCE_VIOLATION`, create matching US/USD sandbox buyer and business accounts and retry with a small amount.

## Screenshots

### Admin Dashboard
![Admin Work Flow ]([https://github.com/mahermorsy/ShippingSystem/blob/main/images/%D9%84%D9%82%D8%B7%D8%A9%20%D8%B4%D8%A7%D8%B4%D8%A9%202026-05-18%20160829.png?raw=true](https://github.com/mahermorsy/ShippingSystem/blob/1e4ebd637c12d9ddf0a4f770d72ecf6f9527045d/images/%D9%84%D9%82%D8%B7%D8%A9%20%D8%B4%D8%A7%D8%B4%D8%A9%202026-05-20%20215550.png))

### Admin Dashboard
![Admin Dashboard](https://github.com/mahermorsy/ShippingSystem/blob/1e4ebd637c12d9ddf0a4f770d72ecf6f9527045d/images/%D9%84%D9%82%D8%B7%D8%A9%20%D8%B4%D8%A7%D8%B4%D8%A9%202026-05-20%20215557.png)

### Shipment History
![Shipment History](https://github.com/mahermorsy/ShippingSystem/blob/1e4ebd637c12d9ddf0a4f770d72ecf6f9527045d/images/%D9%84%D9%82%D8%B7%D8%A9%20%D8%B4%D8%A7%D8%B4%D8%A9%202026-05-20%20215628.png)


### Shipment History
![Shipment History](https://github.com/mahermorsy/ShippingSystem/blob/1e4ebd637c12d9ddf0a4f770d72ecf6f9527045d/images/%D9%84%D9%82%D8%B7%D8%A9%20%D8%B4%D8%A7%D8%B4%D8%A9%202026-05-20%20215628.png)

Add updated screenshots before publishing the repository:

- Shipment creation flow
- Standalone PayPal payment test page
- Swagger API page
- Shipment history/details page

Suggested folder:

```text
docs/screenshots/
```

## Portfolio / LinkedIn Summary

Built a shipping management web application using ASP.NET Core MVC and Web API with SQL Server, authentication, shipment workflows, and PayPal Sandbox payment integration.

Key points to highlight:

- Layered architecture
- REST APIs
- EF Core and SQL Server
- Authentication and authorization
- PayPal order and capture flow debugging
- Shipment management workflows

## Repository Hygiene

The root `.gitignore` excludes generated and local-only files such as `.vs/`, `bin/`, `obj/`, `node_modules/`, `*.user`, and local development settings.

The `Utlalities/standard_html_dotnet` directory contains PayPal sample/reference material. Keep it only if you want to show the integration reference; otherwise remove it before publishing for a smaller portfolio repository.
