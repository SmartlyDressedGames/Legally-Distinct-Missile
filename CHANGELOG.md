# Changelog

All notable changes should be documented in this file. The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## 4.9.3.1 - 2020-05-15

### Changed
- `AutomaticSaveWatchdog` checks the timer during `Update` rather than `FixedUpdate` because the latter is for physics code. Reported by @rube200 in PR #7.
- `UnturnedChat` internally uses `ChatManager.serverSendMessage` rather than manually invoking the `tellChat` RPC. 
- Marked Observatory obsolete and deleted the implementation because it is no longer maintained. Reported by DiFFoZ in PR #4.

### Fixed
- `UnturnedPlayer.Ban` correctly calls `Provider.requestBanPlayer` rather than `Provider.ban`. The distinction is that `requestBanPlayer` allows plugins to override the ban handling and saves the ban information, whereas `ban` is a poorly named internal callback. Reported by @Kr4ken-9 in PR #1.
- `UnturnedPlayer` overrides `Object.Equals` and `Object.GetHashCode` in order to work properly with standard containers. Reported by @CyberAndrii in PR #2.
- `UnturnedPlayerComponent` no longer calls `DontDestroyOnLoad` on itself. This component exists per-player and should be destroyed during level load. Reported by @rube200 in PR #7.
- `UnturnedChat` was ignoring the `rich` parameter. Reported by @Tortellio and @CyberAndrii.
