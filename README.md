# Snipe-IT Playwright Assessment

End-to-end UI automation for the Snipe-IT demo application, built with .NET, NUnit, and Microsoft Playwright.

## Test coverage

The automated test performs the following workflow:

1. Logs in to the Snipe-IT demo application.
2. Creates a unique `Macbook Pro 13"` asset.
3. Sets the asset status to `Ready to Deploy`.
4. Finds the created asset in the assets list.
5. Checks out the asset to a randomly selected user.
6. Opens the asset details page and validates:
   - Asset tag
   - Serial number
   - Model
   - Status
   - Checkout status
   - Assigned user
   - Notes
7. Opens the History tab and validates the asset creation and checkout events.

## Technology stack

- .NET 10
- C#
- NUnit
- Microsoft Playwright
- Chromium browser

## Project structure

```text
Configuration/  Test configuration and environment settings
Models/         Test data models
Pages/          Page Object Model classes
Tests/          NUnit test classes and test setup
```
## Prerequisites

- .NET 10 SDK
- Git, if cloning the repository
- The Playwright Chromium browser
- Windows Command Prompt or PowerShell

## Setup and execution

The following commands were verified on Windows.

### 1. Clone the repository

```cmd
git clone https://github.com/Viji6/snipeit-playwright-assessment-Vijayalakshmi.git
cd snipeit-playwright-assessment-Vijayalakshmi
```
### 2. Restore packages and build

```cmd
dotnet restore
dotnet build
```
### 3. Install the Playwright Chromium browser

```cmd
powershell -ExecutionPolicy Bypass -File .\bin\Debug\net10.0\playwright.ps1 install chromium
```
### 4. Set Snipe-IT demo credentials

Run the following in **Windows Command Prompt**:

```cmd
set SNIPEIT_USERNAME=admin
set SNIPEIT_PASSWORD=password
```
### 5. Run the test

```cmd
set HEADED=1
dotnet test --settings playwright.runsettings --filter "FullyQualifiedName~AssetLifecycleTests"
```
## Expected result

A successful execution displays terminal logs containing:

- Generated Asset Tag
- Generated Serial Number
- Selected checkout user
- Details validation result
- History validation result

Example result:

```text
Test summary: total: 1, failed: 0, succeeded: 1
```

The test also creates full-page evidence screenshots:

```text
bin\Debug\net10.0\test-results\asset-details-<asset-tag>.png
bin\Debug\net10.0\test-results\asset-history-<asset-tag>.png
```

If a test fails, a failure screenshot is created automatically:

```text
bin\Debug\net10.0\test-results\failure.png
```

## Quality and Design Decisions

- **Page Object Model:** Page interactions are separated from test workflow logic, making the test easier to maintain and read.
- **Reliable locators:** The automation uses stable element IDs, accessible role-based locators, and scoped locators instead of brittle XPath or positional selectors.
- **Web-first assertions:** Playwright assertions automatically wait for the expected UI state, reducing timing-related failures.
- **No fixed delays:** The test uses waits for meaningful application events, such as page navigation, table visibility, and search results, rather than `Thread.Sleep`.
- **Unique test data:** Each test run generates a timestamp-based Asset Tag and Serial Number, preventing clashes in the shared demo environment.
- **Secure configuration:** Credentials are read from environment variables and are not stored in source code.
- **Defensive validation:** Guard clauses validate required input, and missing credentials produce a clear configuration error.
- **Test evidence:** Terminal logs show the created asset and assigned user. Full-page screenshots are captured for the Asset Details and History validations, with an additional screenshot captured if a test fails.
- **Shared-environment safety:** The test is marked non-parallel because it creates and updates data in a public shared demo environment.

## Notes

This project uses the public Snipe-IT demo environment. Each execution creates a new asset and checks it out to a randomly selected existing user. The generated timestamp-based Asset Tag ensures each test run uses unique test data.