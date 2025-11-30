# Hexalith.MyNewModule.UI.Pages

This project contains Blazor pages and view models for the MyToDo bounded context.

## Overview

UI Pages provides:
- Routable Blazor pages
- Page-specific view models
- Edit view models with validation
- Page navigation and routing

## Directory Structure

```
Hexalith.MyNewModule.UI.Pages/
├── Modules/
│   └── MyNewModuleMenu.cs
├── MyToDo/
│   ├── MyToDoDetails.razor
│   ├── MyToDoEditValidation.cs
│   ├── MyToDoEditViewModel.cs
│   ├── MyToDoIndex.razor
│   └── Labels.cs
├── Home.razor
├── _Imports.razor
├── wwwroot/
│   └── (static assets)
└── Hexalith.MyNewModule.UI.Pages.csproj
```

## Pages

### Home Page

**Route:** `/MyToDo`

The module's landing page:

```razor
@page "/MyToDo"
@attribute [AllowAnonymous]

<FluentLabel Typo="Typography.PageTitle">MyToDo</FluentLabel>
```

### Index Page (List View)

**Route:** `/MyToDo/MyToDo`

Displays a list of all modules:

```razor
@page "/MyToDo/MyToDo"
@rendermode InteractiveAuto

<HexEntityIndexPage 
    OnLoadData="LoadSummaries"
    OnImport="ImportAsync"
    OnExport="ExportAsync"
    OnDatabaseSynchronize="SynchronizeDatabaseAsync"
    AddPagePath="/MyToDo/Add/MyToDo"
    Title="@Labels.ListTitle">
    <MyToDoSummaryGrid 
        Items="_summariesQuery" 
        EntityDetailsPath="/MyToDo/MyToDo" 
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
- `/MyToDo/Add/MyToDo` - Add new
- `/MyToDo/MyToDo/{Id}` - Edit existing

```razor
@page "/MyToDo/Add/MyToDo"
@page "/MyToDo/MyToDo/{Id}"

<HexEntityDetailsPage 
    AddTitle="@Labels.AddTitle"
    EditTitle="@Labels.Title"
    ViewModel="_data"
    EntityId="@Id"
    IndexPath="/MyToDo/MyToDo"
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

### MyToDoEditViewModel

Edit view model for the details page:

```csharp
public sealed class MyToDoEditViewModel : IIdDescription, IEntityViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Comments { get; set; }
    public bool Disabled { get; set; }
    
    public MyToDoDetailsViewModel Original { get; }
    
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
                new AddMyToDo(Id, Name, Comments), 
                cancellationToken);
        }
        else
        {
            if (DescriptionChanged)
            {
                await commandService.SubmitCommandAsync(user, 
                    new ChangeMyToDoDescription(Id, Name, Comments), 
                    cancellationToken);
            }
            // Handle enable/disable changes...
        }
    }
}
```

### MyToDoEditValidation

FluentValidation validator:

```csharp
public class MyToDoEditValidation : AbstractValidator<MyToDoEditViewModel>
{
    public MyToDoEditValidation()
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

**MyNewModuleMenu.cs:**

```csharp
public static class MyNewModuleMenu
{
    public static MenuItem Menu => new MenuItem
    {
        Id = "MyNewModule",
        Name = MyNewModuleMenuResources.MyNewModuleMenuItem,
        Path = "/MyToDo",
        Icon = Icons.Regular.Size20.Apps,
        Children = [
            new MenuItem
            {
                Id = "mytodo",
                Name = MyNewModuleMenuResources.MyToDoMenuItem,
                Path = "/MyToDo/MyToDo"
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
    public static string Title => MyToDoResources.Title;
    public static string AddTitle => MyToDoResources.AddTitle;
    public static string ListTitle => MyToDoResources.ListTitle;
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
@using Hexalith.MyNewModule
@using Hexalith.MyNewModule.Commands.MyToDo
@using Hexalith.MyNewModule.Localizations
@using Hexalith.MyNewModule.Requests.MyToDo
@using Hexalith.MyNewModule.UI.Components.MyToDo
@using Hexalith.MyNewModule.UI.Pages.MyToDo
@using Hexalith.UI.Components
@using Hexalith.UI.Components.Pages

@inject NavigationManager NavigationManager
@inject IHexalithApplication Application
@inject ICommandService CommandService
@inject IRequestService RequestService
```

## Dependencies

- `Hexalith.UI.Components` - Base UI components
- `Hexalith.MyNewModule.UI.Components` - Module-specific components
- `Hexalith.MyNewModule.Commands` - Command definitions
- `Hexalith.MyNewModule.Requests` - Request/view model definitions
- `Hexalith.MyNewModule.Localizations` - Localization resources
- `FluentValidation` - Form validation
- `Microsoft.FluentUI.AspNetCore.Components` - Fluent UI

## Page Flow

```
                    ┌────────────────────┐
                    │    Home Page       │
                    │   /MyToDo     │
                    └────────────────────┘
                             │
                             ▼
                    ┌────────────────────┐
                    │   Index Page       │
                    │ /MyToDo/      │
                    │   MyToDo      │
                    └────────────────────┘
                      │             │
          ┌───────────┘             └───────────┐
          ▼                                     ▼
┌──────────────────┐               ┌──────────────────┐
│    Add Page      │               │   Edit Page      │
│ /MyToDo/    │               │ /MyToDo/    │
│   Add/MyToDo│               │ MyToDo/{Id} │
└──────────────────┘               └──────────────────┘
```

## Best Practices

1. **Use render modes appropriately** - Choose based on interactivity needs
2. **Validate on client and server** - Don't trust client validation alone
3. **Handle loading states** - Show feedback during async operations
4. **Handle errors gracefully** - Display user-friendly error messages
5. **Use localization** - Support multiple languages
6. **Follow naming conventions** - Consistent page and route naming


