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
- Chromium

## Project structure

```text
Configuration/  Test configuration and environment settings
Models/         Test data models
Pages/          Page Object Model classes
Tests/          NUnit test classes and test setup
```

## Project structure

- **Page Object Model:** Page interactions are separated from test workflow logic, making the test easier to maintain and read.
- **Reliable locators:** The automation uses stable element IDs, accessible role-based locators, and scoped locators instead of brittle XPath or positional selectors.
- **Web-first assertions:** Playwright assertions automatically wait for the expected UI state, reducing timing-related failures.
- **No fixed delays:** The test uses waits for meaningful application events, such as page navigation, table visibility, and search results, rather than `Thread.Sleep`.
- **Unique test data:** Each test run generates a timestamp-based Asset Tag and Serial Number, preventing clashes in the shared demo environment.
- **Secure configuration:** Credentials are read from environment variables and are not stored in source code.
- **Defensive validation:** Guard clauses validate required input, and missing credentials produce a clear configuration error.
- **Test evidence:** Terminal logs show the created asset and assigned user. Full-page screenshots are captured for the Asset Details and History validations, with an additional screenshot captured if a test fails.
- **Shared-environment safety:** The test is marked non-parallel because it creates and updates data in a public shared demo environment.