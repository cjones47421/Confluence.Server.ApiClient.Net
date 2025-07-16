# How to Create and Publish a NuGet Package to nuget.org

This guide explains how to build, pack, and publish your .NET library as a NuGet package to [nuget.org](https://www.nuget.org/).

---

## Prerequisites

- **NuGet.org account:** Register at [nuget.org](https://www.nuget.org/users/account/LogOn).
- **API Key:** Generate an API key from your nuget.org account (see below).
- **.NET SDK:** Ensure the .NET 8 SDK is installed (`dotnet --version`).

---

## 1. Check Project Metadata

Ensure your `.csproj` file contains the following properties:
<PackageId>Your.Package.Id</PackageId>
<Version>1.0.0</Version>
<Authors>Your Name</Authors>
<Company>Your Company</Company>
<Description>A short description of your package</Description>
<PackageLicense>MIT</PackageLicense>
<PackageProjectUrl>https://github.com/your/repo</PackageProjectUrl>
<RepositoryUrl>https://github.com/your/repo</RepositoryUrl>
## 2. Build and Pack the Project

Open a terminal in your project directory and run:
dotnet pack --configuration Release
This command builds your project and creates a NuGet package (`.nupkg` file) in the `bin/Release` folder.

This creates a `.nupkg` file in the `./nupkg` directory.

---

## 3. Get Your NuGet API Key

1. Log in to [nuget.org](https://www.nuget.org/).
2. Click your user icon → **API Keys**.
3. Click **Create** to generate a new key.
4. Set permissions (at minimum: **Push new packages and package versions**).
5. Copy the generated API key.

---

## 4. Publish the Package

Navigate to the `nupkg` directory and run:
dotnet nuget push Your.Package.Id.1.0.0.nupkg -k YourApiKey -s https://api.nuget.org/v3/index.json
Replace `YourApiKey` with the API key you generated from your nuget.org account.

---

## Troubleshooting Common Issues

- **401 Unauthorized Error:** Ensure your API key is correct and has push permissions.
- **404 Not Found Error:** Check that the package ID and version number are correct.
- **Package Already Exists Error:** Update the version number in your `.csproj` file and rebuild the package.

---

## Helpful Links

- [nuget.org Documentation](https://docs.nuget.org/)
- [.NET CLI Reference](https://docs.microsoft.com/en-us/dotnet/core/tools/)

---

Happy Publishing!
Replace `YOUR_API_KEY` with your actual key.

---

## 5. Verify Publication

- Visit [nuget.org/packages/Confluence.Server.ApiClient.Net](https://www.nuget.org/packages/Confluence.Server.ApiClient.Net)
- It may take a few minutes for the package to appear.

---

## 6. Versioning for Updates

- Update the `<Version>` in your `.csproj` for each new release.
- Follow [Semantic Versioning](https://semver.org/).

---

## Security Tips

- **Never commit API keys** to source control.
- Use environment variables or CI/CD secrets for automation.

---

## Optional: Automate with GitHub Actions

You can automate publishing using GitHub Actions. See [NuGet docs](https://docs.microsoft.com/en-us/nuget/nuget-org/publish-a-package) for details.

---
