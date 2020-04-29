# Changelog

All notable changes should be documented in this file. The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## Unreleased

### Fixed
- `UnturnedPlayer.Ban` correctly calls `Provider.requestBanPlayer` rather than `Provider.ban`. The distinction is that `requestBanPlayer` allows plugins to override the ban handling and saves the ban information, whereas `ban` is a poorly named internal callback. Reported by @Kr4ken-9 in issue #1.
