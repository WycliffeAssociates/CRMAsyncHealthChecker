# CRMAsyncHealthChecker

A utility to check the amount of async operations waiting in Microsoft Dataverse (formerly Dynamics 365/CRM) and alert if they are above a limit.

## Requirements

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- Access to a Microsoft Dataverse environment
- SMTP server credentials for email notifications

## Building

To build the project, run:

```bash
dotnet build CRMAsyncHealthChecker.sln
```

Or to build in Release mode:

```bash
dotnet build CRMAsyncHealthChecker.sln --configuration Release
```

## Running Tests

To run the test suite:

```bash
dotnet test CRMAsyncHealthChecker.sln
```

## Configuration

The application requires a JSON configuration file with the following structure:

```json
{
  "EnvironmentName": "Production",
  "ConnectionString": "AuthType=OAuth;Username=user@contoso.onmicrosoft.com;Password=password;Url=https://contoso.crm.dynamics.com;AppId=51f81489-12ee-4a9e-aaae-a2591f45987d;RedirectUri=app://58145B91-0C36-4500-8554-080854F2AC97;LoginPrompt=Auto",
  "EmailServerAddress": "smtp.office365.com",
  "EmailServerPort": 587,
  "EmailServerUsername": "alerts@contoso.com",
  "EmailServerPassword": "email-password",
  "FromAddress": "alerts@contoso.com",
  "ToAddress": [
    "admin@contoso.com",
    "support@contoso.com"
  ],
  "Limit": 1000
}
```

### Configuration Properties

- **EnvironmentName**: A friendly name for the environment (used in email subject)
- **ConnectionString**: Dataverse connection string ([see connection string documentation](https://learn.microsoft.com/en-us/power-apps/developer/data-platform/xrm-tooling/use-connection-strings-xrm-tooling-connect))
- **EmailServerAddress**: SMTP server hostname
- **EmailServerPort**: SMTP server port (typically 587 for TLS or 25)
- **EmailServerUsername**: SMTP authentication username
- **EmailServerPassword**: SMTP authentication password
- **FromAddress**: Email address to send alerts from
- **ToAddress**: Array of email addresses to receive alerts
- **Limit**: Number of waiting async operations that triggers an alert

## Running

Run the application with a configuration file:

```bash
dotnet run --project CRMAsyncHealthChecker/CRMAsyncHealthChecker.csproj -- /path/to/config.json
```

Or after building, run the executable directly:

```bash
# Windows
CRMAsyncHealthChecker\bin\Release\net8.0\CRMAsyncHealthChecker.exe config.json

# Linux/macOS
./CRMAsyncHealthChecker/bin/Release/net8.0/CRMAsyncHealthChecker config.json
```

## How It Works

1. Connects to the specified Dataverse environment using the provided connection string
2. Queries for async operations with a status of "Waiting" (statuscode = 0)
3. If the count is greater than or equal to the configured limit, sends an email alert
4. Email includes the environment name and the configured limit

## License

See [LICENSE](LICENSE) file for details.
