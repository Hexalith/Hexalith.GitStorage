# Hexalith.GitStorage.Localizations

This project contains localization resource files for the GitStorageAccount bounded context.

## Overview

The Localizations project provides:
- Resource files for multiple languages
- Strongly-typed resource accessors
- Labels and messages for UI
- Menu item labels

## Directory Structure

```
Hexalith.GitStorage.Localizations/
├── GitStorageAccount.resx                 # English (default)
├── GitStorageAccount.fr.resx              # French
├── GitStorageAccount.Designer.cs          # Auto-generated accessor
├── GitStorageMenu.resx            # Menu labels (English)
├── GitStorageMenu.fr.resx         # Menu labels (French)
├── GitStorageMenu.Designer.cs     # Auto-generated accessor
└── Hexalith.GitStorage.Localizations.csproj
```

## Resource Files

### GitStorageAccount Resources

Labels and messages for the module:

| Key | English | French |
|-----|---------|--------|
| `Title` | Module | Module |
| `AddTitle` | Add Module | Ajouter un Module |
| `ListTitle` | Modules | Modules |
| `IdLabel` | ID | Identifiant |
| `NameLabel` | Name | Nom |
| `CommentsLabel` | Comments | Commentaires |
| `DisabledLabel` | Disabled | Désactivé |

### GitStorageMenu Resources

Menu labels:

| Key | English | French |
|-----|---------|--------|
| `GitStorageMenuItem` | New Module | Nouveau Module |
| `GitStorageAccountMenuItem` | Module | Module |

## Generated Accessors

### GitStorageAccount Class

```csharp
public class GitStorageAccount
{
    public static string Title => ResourceManager.GetString("Title", Culture);
    public static string AddTitle => ResourceManager.GetString("AddTitle", Culture);
    public static string ListTitle => ResourceManager.GetString("ListTitle", Culture);
    // ... more properties
}
```

### GitStorageMenu Class

```csharp
public class GitStorageMenu
{
    public static string GitStorageMenuItem => 
        ResourceManager.GetString("GitStorageMenuItem", Culture);
    public static string GitStorageAccountMenuItem => 
        ResourceManager.GetString("GitStorageAccountMenuItem", Culture);
}
```

## Adding New Resources

### 1. Add to Default Resource File

Edit `GitStorageAccount.resx`:

```xml
<data name="NewLabel" xml:space="preserve">
  <value>New Value</value>
</data>
```

### 2. Add Translations

Edit `GitStorageAccount.fr.resx`:

```xml
<data name="NewLabel" xml:space="preserve">
  <value>Nouvelle Valeur</value>
</data>
```

### 3. Regenerate Designer File

The designer file is auto-generated when you save the `.resx` file in Visual Studio.

## Adding New Languages

1. Copy the default `.resx` file
2. Rename with culture code: `GitStorageAccount.{culture}.resx`
3. Translate all values
4. Rebuild the project

**Culture codes:**
- `fr` - French
- `de` - German
- `es` - Spanish
- `it` - Italian
- `pt` - Portuguese
- `ja` - Japanese
- `zh` - Chinese

## Usage in Code

### In C# Code

```csharp
using Hexalith.GitStorage.Localizations;

var title = GitStorageAccount.Title;
var menuItem = GitStorageMenu.GitStorageMenuItem;
```

### In Blazor Pages

```razor
@using Hexalith.GitStorage.Localizations

<FluentLabel>@GitStorageAccount.Title</FluentLabel>
```

### In Labels Class

```csharp
internal static class Labels
{
    public static string Title => GitStorageAccount.Title;
    public static string AddTitle => GitStorageAccount.AddTitle;
    public static string ListTitle => GitStorageAccount.ListTitle;
}
```

## Setting Culture

### In Blazor Server

```csharp
// Startup.cs
services.AddLocalization(options => options.ResourcesPath = "Resources");
services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en", "fr" };
    options.SetDefaultCulture("en")
           .AddSupportedCultures(supportedCultures)
           .AddSupportedUICultures(supportedCultures);
});
```

### In Components

```razor
@inject IStringLocalizer<GitStorageAccount> Localizer

<FluentLabel>@Localizer["Title"]</FluentLabel>
```

## Best Practices

1. **Use meaningful keys** - Descriptive and consistent
2. **Keep values short** - UI space is limited
3. **Provide context** - Add comments for translators
4. **Test all cultures** - Verify translations display correctly
5. **Handle missing** - Provide fallback to default culture
6. **Format strings** - Use placeholders: `"Hello, {0}!"`
