# Changelog

All notable changes should be documented in this file. The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## 4.9.3.16 - 2023-10-13

### Changed
- Bind Unturned's `PreVanillaAssemblyResolvePostRedirects` event to fix assembly resolve consistency.

## 4.9.3.15 - 2023-02-08

### Fixed
- `OnPlayerChatted` overriding `isVisible` when `onChatted` is called beforehand. Thanks @rube200 in PR #58.

## 4.9.3.14 - 2022-04-01

### Changed
- Assembly resolve looks for dependency with requested version number. Thanks @Sl4vP0weR in issue #49.

## 4.9.3.13 - 2021-04-23

### Changed
- Updated `onCheckValid` to `onCheckValidWithExplanation`. "Thanks" @surv0013 in issue #39.

## 4.9.3.12 - 2021-04-09

### Changed
- Replaced `UnturnedPlayer` usage of `PlayerSkills.askSkills`. Thanks @PandahutMiku: [#2537](https://github.com/SmartlyDressedGames/Unturned-3.x-Community/issues/2537)

## 4.9.3.11 - 2021-04-02

### Changed
- Replaced Assembly-CSharp-firstpass.dll with Steamworks.NET.dll.

## 4.9.3.10 - 2021-03-19

### Fixed
- Call OnPlayerDeath (tellDeath) before tellDead to respect old behaviour. Thanks @rube200 in issue #38 and @RestoreMonarchy in issue #37.

## 4.9.3.9 - 2021-03-05

### Changed
- Replaced `tellBleeding` invocation with `serverSetBleeding`.
- Replaced `tellBroken` invocation with `serverSetLegsBroken`.
- Replaced `UnturnedPlayer.TriggerEffect` invocation with `EffectManager.triggerEffect`.
- Replaced `UnturnedEffect` invocation with `EffectManager.triggerEffect`.
- Replaced `UnturnedPlayer.Experience` invocation with `PlayerSkills.ServerSetExperience`.
- Vanish mode teleport no longer needs special handling because the vanilla game skips other clients if canAddSimulationResultsToUpdates is false.
- Updated gesture changed to use value from event.

## 4.9.3.8 - 2021-02-26

### Changed
- Replaced gesture parse with switch statement. Thanks @PandahutMiku: [#2435](https://github.com/SmartlyDressedGames/Unturned-3.x-Community/issues/2435)

## 4.9.3.7 - 2021-02-26

### Changed
- Replaced `OnPlayerUpdateStat` usage of `SteamChannel.onTriggerSend` with `Player.onPlayerStatIncremented`.
- Replaced `OnPlayerWear` usage of `SteamChannel.onTriggerSend` with `PlayerClothing.OnItemChanged_Global`.
- Replaced life stat usage event of `SteamChannel.onTriggerSend` with `PlayerLife.OnTellStat_Changed`.
- Replaced `OnPlayerUpdateGesture` usage of `SteamChannel.onTriggerSend` with `PlayerAnimator.OnGestureChanged_Global`.
- Replaced `OnPlayerUpdateExperience` usage of `SteamChannel.onTriggerSend` with `PlayerSkills.OnExperienceChanged_Global`.
- Replaced `OnPlayerUpdateStance` usage of `SteamChannel.onTriggerSend` with `PlayerStance.OnStanceChanged_Global`.
- Replaced `OnPlayerDead` and `OnPlayerDeath` usage of `SteamChannel.onTriggerSend` with `PlayerLife.onPlayerDied`.
- Replaced `OnPlayerUpdateLife` and `OnPlayerRevive` usage of `SteamChannel.onTriggerSend` with `PlayerLife.OnRevived_Global`.

## 4.9.3.6 - 2020-10-16

### Fixed
- Updated `UnturnedPlayer.IP` and `UnturnedPlayer.Ban` to use address from transport layer refactor.

## 4.9.3.5 - 2020-09-25

### Changed
- Updated to use Unturned's plugin advertising interface rather than directly setting Steam Game Server details, and no longer consume a bot player slot.

## 4.9.3.4 - 2020-09-11

### Fixed
- `Logger` no longer directly writes to Rocket log file because `Rocket.Unturned.U` already writes console output to the log file. Reported by @warren39 in issue #28.

## 4.9.3.3 - 2020-07-10

### Changed
- The command to reload all plugins at once has been disabled by popular request. Many plugins did not support it properly. Reloading individual plugins is still enabled. Read this issue for more details: [#1794](https://github.com/SmartlyDressedGames/Unturned-3.x-Community/issues/1794)
- `Logger` console output is routed through Unturned's `CommandWindow` class, rather than directly using the `System.Console` class. This fixes Rocket output blocking the game thread. Reported by @rube200 in issue #21.

## 4.9.3.2 - 2020-05-18

### Changed
- Home command offsets vertically similar to the vanilla respawn, and warns player when unable to teleport.

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
