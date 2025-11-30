# Hexalith.GitStorage.UI.Pages

This project contains Blazor pages and view models for the GitStorageAccount bounded context.

## Overview

UI Pages provides:
- Routable Blazor pages
- Page-specific view models
- Edit view models with validation
- Page navigation and routing

## Directory Structure

```
Hexalith.GitStorage.UI.Pages/
├── Modules/
│   └── GitStorageMenu.cs
├── GitStorageAccount/
│   ├── GitStorageAccountDetails.razor
│   ├── GitStorageAccountEditValidation.cs
│   ├── GitStorageAccountEditViewModel.cs
│   ├── GitStorageAccountIndex.razor
│   └── Labels.cs
├── Home.razor
├── _Imports.razor
├── wwwroot/
│   └── (static assets)
└── Hexalith.GitStorage.UI.Pages.csproj
```

## Pages

### Home Page

**Route:** `/GitStorageAccount`

The module's landing page:

```razor
@page "/GitStorageAccount"
@attribute [AllowAnonymous]

<FluentLabel Typo="Typography.PageTitle">GitStorageAccount</FluentLabel>
```

### Index Page (List View)

**Route:** `/GitStorageAccount/GitStorageAccount`

Displays a list of all modules:

```razor
@page "/GitStorageAccount/GitStorageAccount"
@rendermode InteractiveAuto

<HexEntityIndexPage 
    OnLoadData="LoadSummaries"
    OnImport="ImportAsync"
    OnExport="ExportAsync"
    OnDatabaseSynchronize="SynchronizeDatabaseAsync"
    AddPagePath="/GitStorageAccount/Add/GitStorageAccount"
    Title="@Labels.ListTitle">
    <GitStorageAccountSummaryGrid 
        Items="_summariesQuery" 
        EntityDetailsPath="/GitStorageAccount/GitStorageAccount" 
        OnDisabledChanged="OnDisabledChangedAsync" />
</HexEntityIndexPage>
```

**Features:**
- Data grid with sorting/filtering
- Add new item button
- Import/Export functionality
- Database synchronization
- Disable/Enable toggle

### Details Page (Add/Edit)

**Routes:**
- `/GitStorageAccount/Add/GitStorageAccount` - Add new
- `/GitStorageAccount/GitStorageAccount/{Id}` - Edit existing

```razor
@page "/GitStorageAccount/Add/GitStorageAccount"
@page "/GitStorageAccount/GitStorageAccount/{Id}"

<HexEntityDetailsPage 
    AddTitle="@Labels.AddTitle"
    EditTitle="@Labels.Title"
    ViewModel="_data"
    EntityId="@Id"
    IndexPath="/GitStorageAccount/GitStorageAccount"
    ValidationResult="_validationResult"
    OnSave="OnSave"
    OnLoadData="OnLoadDataAsync">
</HexEntityDetailsPage>
```

**Features:**
- Add or edit mode
- Form validation
- Save functionality
- Navigation back to list

## View Models

### GitStorageAccountEditViewModel

Edit view model for the details page:

```csharp
public sealed class GitStorageAccountEditViewModel : IIdDescription, IEntityViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Comments { get; set; }
    public bool Disabled { get; set; }
    
    public GitStorageAccountDetailsViewModel Original { get; }
    
    public bool HasChanges => 
        Id != Original.Id ||
        DescriptionChanged ||
        Disabled != Original.Disabled;
        
    public bool DescriptionChanged => 
        Comments != Original.Comments || 
        Name != Original.Name;

    internal async Task SaveAsync(
        ClaimsPrincipal user, 
        ICommandService commandService, 
        bool create, 
        CancellationToken cancellationToken)
    {
        if (create)
        {
            await commandService.SubmitCommandAsync(user, 
                new AddGitStorageAccount(Id, Name, Comments), 
                cancellationToken);
        }
        else
        {
            if (DescriptionChanged)
            {
                await commandService.SubmitCommandAsync(user, 
                    new ChangeGitStorageAccountDescription(Id, Name, Comments), 
                    cancellationToken);
            }
            // Handle enable/disable changes...
        }
    }
}
```

### GitStorageAccountEditValidation

FluentValidation validator:

```csharp
public class GitStorageAccountEditValidation : AbstractValidator<GitStorageAccountEditViewModel>
{
    public GitStorageAccountEditValidation()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ID is required");
            
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Name is required and must be less than 100 characters");
    }
}
```

## Menu Configuration

**GitStorageMenu.cs:**

```csharp
public static class GitStorageMenu
{
    public static MenuItem Menu => new MenuItem
    {
        Id = "GitStorage",
        Name = GitStorageMenuResources.GitStorageMenuItem,
        Path = "/GitStorageAccount",
        Icon = Icons.Regular.Size20.Apps,
        Children = [
            new MenuItem
            {
                Id = "GitStorageAccount",
                Name = GitStorageMenuResources.GitStorageAccountMenuItem,
                Path = "/GitStorageAccount/GitStorageAccount"
            }
        ]
    };
}
```

## Labels and Localization

**Labels.cs:**

```csharp
internal static class Labels
{
    public static string Title => GitStorageAccountResources.Title;
    public static string AddTitle => GitStorageAccountResources.AddTitle;
    public static string ListTitle => GitStorageAccountResources.ListTitle;
}
```

Labels are sourced from localization resources for i18n support.

## Global Imports

**_Imports.razor:**

```razor
@using System.Security.Claims
@using Microsoft.AspNetCore.Authorization
@using Microsoft.AspNetCore.Components.Authorization
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.AspNetCore.Components.Routing
@using Microsoft.AspNetCore.Components.Web
@using Microsoft.FluentUI.AspNetCore.Components
@using FluentValidation.Results
@using Hexalith.Application.Commands
@using Hexalith.Application.Modules.Applications
@using Hexalith.Application.Requests
@using Hexalith.GitStorage
@using Hexalith.GitStorage.Commands.GitStorageAccount
@using Hexalith.GitStorage.Localizations
@using Hexalith.GitStorage.Requests.GitStorageAccount
@using Hexalith.GitStorage.UI.Components.GitStorageAccount
@using Hexalith.GitStorage.UI.Pages.GitStorageAccount
@using Hexalith.UI.Components
@using Hexalith.UI.Components.Pages

@inject NavigationManager NavigationManager
@inject IHexalithApplication Application
@inject ICommandService CommandService
@inject IRequestService RequestService
```

## Dependencies

- `Hexalith.UI.Components` - Base UI components
- `Hexalith.GitStorage.UI.Components` - Module-specific components
- `Hexalith.GitStorage.Commands` - Command definitions
- `Hexalith.GitStorage.Requests` - Request/view model definitions
- `Hexalith.GitStorage.Localizations` - Localization resources
- `FluentValidation` - Form validation
- `Microsoft.FluentUI.AspNetCore.Components` - Fluent UI

## Page Flow

```
                    ┌────────────────────┐
                    │    Home Page       │
                    │   /GitStorageAccount     │
                    └────────────────────┘
                             │
                             ▼
                    ┌────────────────────┐
                    │   Index Page       │
                    │ /GitStorageAccount/      │
                    │   GitStorageAccount      │
                    └────────────────────┘
                      │             │
          ┌───────────┘             └───────────┐
          ▼                                     ▼
┌──────────────────┐               ┌──────────────────┐
│    Add Page      │               │   Edit Page      │
│ /GitStorageAccount/    │               │ /GitStorageAccount/    │
│   Add/GitStorageAccount│               │ GitStorageAccount/{Id} │
└──────────────────┘               └──────────────────┘
```

## Best Practices

1. **Use render modes appropriately** - Choose based on interactivity needs
2. **Validate on client and server** - Don't trust client validation alone
3. **Handle loading states** - Show feedback during async operations
4. **Handle errors gracefully** - Display user-friendly error messages
5. **Use localization** - Support multiple languages
6. **Follow naming conventions** - Consistent page and route naming


