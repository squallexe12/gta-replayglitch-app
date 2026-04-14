# Replay Firewall Tool

A Windows desktop utility for managing a specific outbound Windows Defender Firewall rule used in GTA 5 replay-related setups.

The application is built as a native WinForms `.exe`, includes bilingual UI support, and requires administrator privileges because it reads and updates firewall rules.

## Preview

### Screenshot

![Replay Firewall Tool screenshot](docs/media/screenshot-main.png)

### Quick Preview GIF

![Replay Firewall Tool quick preview](docs/media/demo.gif)

## What it does

- Detects whether the target outbound firewall rule exists
- Shows current rule status
- Adds the rule if it is missing
- Enables the rule
- Disables the rule
- Toggles the rule state
- Refreshes the current status
- Shows a first-run wizard when the rule is missing
- Explains the rule in a dedicated details view
- Uses color-coded status feedback for quick understanding
- Shows small success notifications after actions
- Supports both English and Turkish from the UI

## Rule details

The tool manages this exact rule configuration:

- Rule name: `GTA 5 REPLAY GLITCH`
- Direction: `Outbound`
- Action: `Block`
- Remote IP: `192.81.241.171`
- Default state when newly created: `Disabled`

## Language support

The application now includes built-in language switching:

- English
- Turkish (`Turkce`)

The default language is selected automatically from the Windows UI culture, and the user can switch languages at runtime from the app window.

## Why the app requires administrator rights

Windows firewall rule operations are privileged actions.  
The executable includes an application manifest with `requireAdministrator`, so Windows will show a UAC prompt before the UI opens.

## Requirements

- Windows 10 or Windows 11
- .NET Framework compiler available for building from source
- Administrator rights to run the app

The built executable itself targets the Windows .NET Framework runtime that is normally present on supported Windows systems.

## User experience highlights

- First-run wizard for missing-rule setup
- Clear status colors for `Missing`, `Disabled`, `Enabled`, and `Mixed`
- Rule explanation panel so users understand exactly what the tool is doing
- Success notifications without interrupting flow
- Language switching from inside the app

## Build from source

Run the included PowerShell build script:

```powershell
.\build.ps1
```

By default, the script writes the build output to:

```text
ReplayFirewallTool.exe
```

You can also choose a custom output folder:

```powershell
.\build.ps1 -OutputDirectory release
```

## Repository structure

```text
.
|-- ReplayFirewallTool.cs
|-- ReplayFirewallTool.exe
|-- ReplayFirewallTool.ico
|-- ReplayFirewallTool.manifest
|-- KULLANIM.txt
|-- build.ps1
|-- README.md
|-- CHANGELOG.md
|-- LICENSE
|-- docs/
|-- .github/
`-- .gitignore
```

## Release guidance

For a clean GitHub repository:

1. Commit source files, docs, and assets
2. Commit the root-level `ReplayFirewallTool.exe` if you want the binary directly visible in the repository
3. Publish the compiled `.exe` and checksum as GitHub Release assets
4. Tag releases with the version from the changelog
5. Use the `Added / Changed / Fixed` release note structure

Current release tag:

```text
v1.2.0
```

## Release workflow

The repository now includes a GitHub Actions workflow for tagged releases:

- Builds the executable on `windows-latest`
- Optionally signs the executable if signing secrets are configured
- Generates a SHA-256 checksum file
- Packages a release zip
- Publishes release assets automatically

Workflow file:

- [release.yml](.github/workflows/release.yml)

Release notes template:

- [RELEASE_TEMPLATE.md](.github/RELEASE_TEMPLATE.md)

## Versioning and release discipline

- [Versioning policy](docs/VERSIONING.md)
- [Code signing guidance](docs/CODE_SIGNING.md)
- [Changelog](CHANGELOG.md)

## Security note

This project manages a very specific firewall rule by design.  
If you fork or adapt it for another target, update these values carefully:

- Rule name
- Remote IP
- Default enabled/disabled behavior
- User-facing README notes

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE).
