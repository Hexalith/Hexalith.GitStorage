# Hexalith.GitStorage.UI.Components

This project contains reusable Blazor UI components for the GitStorage bounded context, supporting management of Git storage providers (GitHub and Forgejo), organizations, and repositories.

## Overview

UI Components provides:
- Reusable Blazor components
- Input fields and forms
- Data grids and tables
- Common UI patterns

## Directory Structure

```
Hexalith.GitStorage.UI.Components/
├── Common/
│   └── (shared components)
├── GitStorageAccount/
│   ├── _Imports.razor
│   ├── GitStorageAccountIdField.razor
│   └── GitStorageAccountSummaryGrid.razor
├── wwwroot/
│   └── (static assets)
├── _Imports.razor
└── Hexalith.GitStorage.UI.Components.csproj
```

## Components

### GitStorageAccountIdField

An input field component for selecting or entering a Git storage account ID:

```razor
<GitStorageAccountIdField @bind-Value="@SelectedId" />
```

**Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `Value` | `string?` | The selected ID value |
| `ValueChanged` | `EventCallback<string?>` | Callback when value changes |
| `Disabled` | `bool` | Whether the field is disabled |
| `Required` | `bool` | Whether the field is required |

### GitStorageAccountSummaryGrid

A data grid component for displaying storage account summaries:

```razor
<GitStorageAccountSummaryGrid
    Items="@_summariesQuery"
    EntityDetailsPath="/GitStorage/Account"
    OnDisabledChanged="OnDisabledChangedAsync" />
```

**Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `Items` | `IQueryable<GitStorageAccountSummaryViewModel>?` | Data source |
| `EntityDetailsPath` | `string` | Path to detail page |
| `OnDisabledChanged` | `EventCallback<string>` | Callback when disable toggled |

**Features:**

- Sortable columns
- Filterable data
- Pagination
- Row selection
- Action buttons

### ProviderTypeSelector

A selector component for choosing the Git provider type:

```razor
<ProviderTypeSelector @bind-Value="@ProviderType" />
```

**Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `Value` | `GitProviderType` | The selected provider type |
| `ValueChanged` | `EventCallback<GitProviderType>` | Callback when value changes |

### OrganizationSelector

A component for selecting organizations from a Git provider:

```razor
<OrganizationSelector
    AccountId="@AccountId"
    @bind-SelectedOrganization="@SelectedOrg" />
```

### RepositoryList

A component for displaying repositories in an organization:

```razor
<RepositoryList
    AccountId="@AccountId"
    OrganizationName="@OrgName"
    OnRepositorySelected="HandleSelection" />
```

## Global Imports

**_Imports.razor:**

```razor
@using Microsoft.AspNetCore.Components.Web
@using Microsoft.FluentUI.AspNetCore.Components
@using Hexalith.GitStorage.Requests.GitStorageAccount
@using Hexalith.UI.Components
```

## Styling

Components use the Fluent UI design system:
- Consistent with Microsoft design language
- Accessible by default
- Responsive design
- Theme support (light/dark)

### Custom Styles

Custom styles can be added in `wwwroot/`:

```css
/* Custom component styles */
.GitStorageAccount-grid {
    /* Custom grid styles */
}
```

## Component Guidelines

### Creating New Components

1. Create a new `.razor` file in the appropriate folder
2. Add necessary `@using` directives
3. Define parameters using `[Parameter]`
4. Implement component logic in `@code` block
5. Add XML documentation

**Example:**

```razor
@* MyNewComponent.razor *@

<div class="GitStorageAccount-component">
    <FluentLabel>@Label</FluentLabel>
    <FluentTextField @bind-Value="@Value" />
</div>

@code {
    /// <summary>
    /// The label text.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// The input value.
    /// </summary>
    [Parameter]
    public string? Value { get; set; }

    /// <summary>
    /// Callback when value changes.
    /// </summary>
    [Parameter]
    public EventCallback<string?> ValueChanged { get; set; }
}
```

## Dependencies

- `Microsoft.FluentUI.AspNetCore.Components` - Fluent UI
- `Hexalith.UI.Components` - Base component library
- `Hexalith.GitStorage.Requests` - View models

## Usage in Pages

```razor
@page "/GitStorageAccount/Example"

<GitStorageAccountIdField @bind-Value="@_selectedId" />

<GitStorageAccountSummaryGrid 
    Items="@_items"
    EntityDetailsPath="/GitStorageAccount/GitStorageAccount" />

@code {
    private string? _selectedId;
    private IQueryable<GitStorageAccountSummaryViewModel>? _items;
}
```

## Best Practices

1. **Keep components focused** - Single responsibility
2. **Use parameters** - Make components configurable
3. **Support two-way binding** - Use `EventCallback` for value changes
4. **Handle null values** - Graceful handling of missing data
5. **Add documentation** - XML comments for IntelliSense
6. **Use semantic HTML** - Accessibility first


