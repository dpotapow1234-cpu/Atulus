# Atulus

**Atulus** is a lightweight automation framework and **Custom Macro System (CMS)** for Windows applications. It provides a simple and extensible environment for launching applications, executing automation scripts, managing Windows processes, and interacting with application windows.

The project is designed to automate repetitive tasks, streamline application startup, and simplify workflow automation through reusable macro components.

## Features

* Launch applications directly or through supported launchers
* Steam launcher integration
* Windows window management
* AutoIt script execution
* PowerShell script execution
* Built-in message dialogs
* Modular and extensible architecture
* Support for custom automation workflows

## Components

### Launchers

The launcher system provides multiple ways to start applications. Currently supported launchers include Direct and Steam, while the architecture allows additional integrations such as Epic Games Launcher, Ubisoft Connect, Battle.net, EA App, and GOG Galaxy.

### Custom Macro System (CMS)

CMS is the core of the framework and provides reusable automation actions that can be combined into custom workflows.

Included modules:

* **WindowManager** – Find, wait for, activate, move, resize, minimize, and restore application windows.
* **AutoItMacro** – Execute AutoIt (`.au3`) scripts for advanced UI automation.
* **MessageBox** – Display standardized notification, confirmation, and error dialogs.

### PowerShell Runner

Execute PowerShell commands and scripts for environment preparation, dependency management, or Windows automation tasks.

### Scripts

The `Scripts` directory contains AutoIt automation scripts used by the framework.

Example:

```text
Scripts/
    steam.au3
```

## Example

Launch an application using the Steam launcher:

```csharp
var launcher = new SteamLauncher();

launcher.Run(appId);
```

Execute an AutoIt macro:

```csharp
AutoItMacro.Run("Scripts/steam.au3");
```

## Extensibility

Atulus was designed with extensibility in mind. New launchers, macro types, automation modules, script engines, and workflow components can be added with minimal effort, making it suitable as a foundation for custom automation systems.

## Requirements

* .NET
* Windows
* AutoIt
* PowerShell

## Roadmap

* Epic Games Launcher support
* Ubisoft Connect support
* Battle.net support
* EA App support
* GOG Galaxy support
* Plugin system
* JSON-based macro configuration
* Workflow execution queues
* Logging system
* Asynchronous task execution

## License

This project is licensed under the **LGPL-2.1** License.
