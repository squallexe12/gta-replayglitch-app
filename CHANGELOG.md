# Changelog

All notable changes to this project are documented here.

## 1.2.0 - 2026-04-13

### Added
- First-run wizard that explains how to create the missing firewall rule step by step.
- Details view that explains exactly what the managed rule does.
- Toast-style success notifications using a tray icon balloon tip.
- GitHub Actions release workflow with checksum and release asset packaging.
- Versioning policy and code-signing guidance docs.

### Changed
- Refactored the application into multiple source files for easier maintenance.
- Improved the UI with clearer status colors and a stronger visual state system.
- Bumped product version metadata to `1.2.0`.

### Fixed
- Reduced the chance of confusing rule state presentation by pairing text with color-coded status feedback.

## 1.1.0 - 2026-04-12

### Added
- English and Turkish UI support with live language switching.
- Application icon and version metadata.
- Build script, README, license, and GitHub-ready repository metadata.

### Changed
- Reworked the layout for a cleaner fixed action area.

### Fixed
- Improved release readiness of the repository structure.

## 1.0.0 - 2026-04-12

### Added
- Initial Windows Firewall rule manager release.
- Support for creating the outbound block rule if it is missing.
- Enable, disable, toggle, and refresh actions.
