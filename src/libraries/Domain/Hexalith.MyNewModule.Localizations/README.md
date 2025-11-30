# Hexalith.MyNewModule.Localizations

This project contains localization resource files for the MyToDo bounded context.

## Overview

The Localizations project provides:
- Resource files for multiple languages
- Strongly-typed resource accessors
- Labels and messages for UI
- Menu item labels

## Directory Structure

```
Hexalith.MyNewModule.Localizations/
├── MyToDo.resx                 # English (default)
├── MyToDo.fr.resx              # French
├── MyToDo.Designer.cs          # Auto-generated accessor
├── MyNewModuleMenu.resx            # Menu labels (English)
├── MyNewModuleMenu.fr.resx         # Menu labels (French)
├── MyNewModuleMenu.Designer.cs     # Auto-generated accessor
└── Hexalith.MyNewModule.Localizations.csproj
```

## Resource Files

### MyToDo Resources

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

### MyNewModuleMenu Resources

Menu labels:

| Key | English | French |
|-----|---------|--------|
| `MyNewModuleMenuItem` | New Module | Nouveau Module |
| `MyToDoMenuItem` | Module | Module |

## Generated Accessors

### MyToDo Class

```csharp
public class MyToDo
{
    public static string Title => ResourceManager.GetString("Title", Culture);
    public static string AddTitle => ResourceManager.GetString("AddTitle", Culture);
    public static string ListTitle => ResourceManager.GetString("ListTitle", Culture);
    // ... more properties
}
```

### MyNewModuleMenu Class

```csharp
public class MyNewModuleMenu
{
    public static string MyNewModuleMenuItem => 
        ResourceManager.GetString("MyNewModuleMenuItem", Culture);
    public static string MyToDoMenuItem => 
        ResourceManager.GetString("MyToDoMenuItem", Culture);
}
```

## Adding New Resources

### 1. Add to Default Resource File

Edit `MyToDo.resx`:

```xml
<data name="NewLabel" xml:space="preserve">
  <value>New Value</value>
</data>
```

### 2. Add Translations

Edit `MyToDo.fr.resx`:

```xml
<data name="NewLabel" xml:space="preserve">
  <value>Nouvelle Valeur</value>
</data>
```

### 3. Regenerate Designer File

The designer file is auto-generated when you save the `.resx` file in Visual Studio.

## Adding New Languages

1. Copy the default `.resx` file
2. Rename with culture code: `MyToDo.{culture}.resx`
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
using Hexalith.MyNewModule.Localizations;

var title = MyToDo.Title;
var menuItem = MyNewModuleMenu.MyNewModuleMenuItem;
```

### In Blazor Pages

```razor
@using Hexalith.MyNewModule.Localizations

<FluentLabel>@MyToDo.Title</FluentLabel>
```

### In Labels Class

```csharp
internal static class Labels
{
    public static string Title => MyToDo.Title;
    public static string AddTitle => MyToDo.AddTitle;
    public static string ListTitle => MyToDo.ListTitle;
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
@inject IStringLocalizer<MyToDo> Localizer

<FluentLabel>@Localizer["Title"]</FluentLabel>
```

## Best Practices

1. **Use meaningful keys** - Descriptive and consistent
2. **Keep values short** - UI space is limited
3. **Provide context** - Add comments for translators
4. **Test all cultures** - Verify translations display correctly
5. **Handle missing** - Provide fallback to default culture
6. **Format strings** - Use placeholders: `"Hello, {0}!"`
