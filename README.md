# ![RocketModFix][rocketmodfix_logo]

The **RocketModFix** is a fork of RocketMod for Unturned maintained by the Unturned plugin devs, this fork don't have plans for any major changes to the RocketMod, only fixes and new features that doesn't break any backward compatibility with API, etc.

## Compatibility

You can still use old RocketMod plugins without any changes/recompilation/updates, however if you want to use new features and bug fixes we recommend to install updated Module and new Rocket. API/Core/Unturned Redistributables (libraries).

## Our plan

- [x] Create Discord Server Community.
- [x] UnityEngine NuGet Package redist.
- [x] Unturned NuGet Package redist.
- [x] Update MSBuild to the `Microsoft.NET.Sdk`, because current MSBuild in RocketMod is outdated and its hard to support and understand what's going on inside.
- [x] RocketMod NuGet Package containing all required libraries for RockeMod API usage.
- [x] CI/CD and nightly builds with RocketMod .dlls.
- [x] Automatic Release on Tag creation (with RocketMod Module).
- [x] Rocket.Unturned NuGet Package.
- [x] Reset changelog.
- [x] For changelog use [Keep a Changelog standard][keep_a_changelog_url].
- [x] For versioning use [SemVer][semver_url].
- [x] Installation guides inside of the Rocket Unturned Module.
- [ ] Keep backward compatibility.
	- [x] Test with RocketMod plugins that uses old RocketMod libraries, and make sure current changes doesn't break anything.
	- [ ] Test with most used Modules:
		- [ ] AviRockets.
		- [ ] uScript.
		- [ ] OpenMod.
- [ ] RocketMod Fixes:
	- [ ] Fix UnturnedPlayer.SteamProfile (cause so many lags). 
	- [ ] Fix UnturnedPlayerComponent is not being added automatically.
	- [ ] Assembly Resolve fixes (don't spam with not found library or make a option to disable it, load all libraries at rocketmod start instead of searching for them only on OnAssemblyResolve)
	- [ ] Commands fixes:
		- [ ] Fix /vanish.
		- [ ] Fix /god.
		- [ ] Fix /p (not readable at all).
	- [ ] Perfomance.
- [ ] New Features:
	- [ ] Commands:
		- [ ] /position /pos (current position of the player).
		- [ ] /tpall (teleport everyone to self or Vector3 point)
- [ ] Gather a Team with a direct access to the repo edit without admins help.
- [ ] RocketModFix Video Installation Guide (could be uploaded on YouTube).

After plan is finished -> Add new plans, keep coding, and don't forget to accept PR or issues.

## Installation

1. Stop the server (if running).
1. Install latest RocketModFix Module [here](https://github.com/RocketModFix/RocketModFix/releases).
2. Open Dropdown button Assets (if its not open).
3. Click on `Rocket.Unturned.Module-v0.0.0.zip` to download (v - version can be a bit different).
4. Keep `Readme.txt` instruction file inside of the installed `Rocket.Unturned.Module-v0.0.0.zip` archive.

## Discord

Feel free to join our [Discord Server][discordserver_url].

## How to Contribute
We're thrilled to have you here! Feel free to create pull requests (PRs) and open issues - your contributions are valuable to us!

### Branching Guidelines
- Avoid Committing to `master`: Please refrain from committing directly to the `master` branch.
- Create a Branch from `dev`: Instead, create a new branch from the `dev` branch.
- Submit PRs to `dev`: Submit your changes by creating a PR into the `dev` branch. This keeps our master branch stable, ensuring a smooth experience for all users.

### Why We Use Issues
Before you dive into making changes, consider creating an [issue][issues_url] or discussions on our [discord server][discordserver_url] first. Here's why:

- Avoid Duplicate Work: Someone might already be working on a similar update. Checking issues prevents duplication of effort.
- Collaborative Problem Solving: Other contributors might have valuable insights or alternative solutions. Discussing changes beforehand can lead to better implementations.
- Save Your Time: Avoid working on updates that might not align with the project's direction. Consult with others to ensure your efforts are fruitful.

## NuGet Packages

### Redist

[![RocketModFix.Unturned.Redist][badge_RocketModFix.Unturned.Redist]][nuget_package_RocketModFix.Unturned.Redist]

[![RocketModFix.UnityEngine.Redist][badge_RocketModFix.UnityEngine.Redist]][nuget_package_RocketModFix.UnityEngine.Redist]

### RocketModFix

[![RocketModFix.Rocket.API][badge_RocketModFix.Rocket.API]][nuget_package_RocketModFix.Rocket.API]

[![RocketModFix.Rocket.Core][badge_RocketModFix.Rocket.Core]][nuget_package_RocketModFix.Rocket.Core]

[![RocketModFix.Rocket.Unturned][badge_RocketModFix.Rocket.Unturned]][nuget_package_RocketModFix.Rocket.Unturned]

## Resources

fr34kyn01535 has listed all of the original plugins in a post to the /r/RocketMod subreddit: [List of plugins from the old repository](https://www.reddit.com/r/rocketmod/comments/ek4i7b/)

Following closure of the original forum the recommended sites for developer discussion are the [/r/UnturnedLDM](https://www.reddit.com/r/UnturnedLDM/) subreddit, [SDG Forum](https://forum.smartlydressedgames.com/c/modding/ldm), or the [Steam Discussions](https://steamcommunity.com/app/304930/discussions/17/).

The RocketMod organization on GitHub hosts several related archived projects: [RocketMod (Abandoned)](https://github.com/RocketMod)

## History

On the 20th of December 2019 Sven Mawby "fr34kyn01535" and Enes Sadık Özbek "Trojaner" officially ceased maintenance of Rocket. They kindly released the source code under the MIT license. [Read their full farewell statement here.](https://github.com/RocketMod/Rocket/blob/master/Farewell.md)

Following their resignation SDG forked the repository to continue maintenance in sync with the game.

On the 2nd of June 2020 fr34kyn01535 requested the fork be rebranded to help distance himself from the project.

## Credits

[OpenMod][openmod_github_repository] for nuget packages ready-to-go actions and workflows.

[keep_a_changelog_url]: https://keepachangelog.com/en/1.1.0/
[semver_url]: https://semver.org/

[rocketmodfix_logo]: https://raw.githubusercontent.com/RocketModFix/RocketModFix/master/resources/RocketModFix.png

[issues_url]: https://github.com/RocketModFix/RocketModFix/issues

[nuget_package_RocketModFix.Unturned.Redist]: https://www.nuget.org/packages/RocketModFix.Unturned.Redist
[badge_RocketModFix.Unturned.Redist]: https://img.shields.io/nuget/v/RocketModFix.Unturned.Redist?label=RocketModFix.Unturned.Redist&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FRocketModFix.Unturned.Redist

[nuget_package_RocketModFix.UnityEngine.Redist]: https://www.nuget.org/packages/RocketModFix.UnityEngine.Redist
[badge_RocketModFix.UnityEngine.Redist]: https://img.shields.io/nuget/v/RocketModFix.UnityEngine.Redist?label=RocketModFix.UnityEngine.Redist&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FRocketModFix.UnityEngine.Redist

[nuget_package_RocketModFix.Rocket.API]: https://www.nuget.org/packages/RocketModFix.Rocket.API
[badge_RocketModFix.Rocket.API]: https://img.shields.io/nuget/v/RocketModFix.Rocket.API?label=RocketModFix.Rocket.API&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FRocketModFix.Rocket.API

[nuget_package_RocketModFix.Rocket.Core]: https://www.nuget.org/packages/RocketModFix.Rocket.Core
[badge_RocketModFix.Rocket.Core]: https://img.shields.io/nuget/v/RocketModFix.Rocket.Core?label=RocketModFix.Rocket.Core&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FRocketModFix.Rocket.Core

[nuget_package_RocketModFix.Rocket.Unturned]: https://www.nuget.org/packages/RocketModFix.Rocket.Unturned
[badge_RocketModFix.Rocket.Unturned]: https://img.shields.io/nuget/v/RocketModFix.Rocket.Unturned?label=RocketModFix.Rocket.Unturned&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FRocketModFix.Rocket.Unturned

[discordserver_url]: https://discord.gg/z6VM7taWeG 

[openmod_github_repository]: https://github.com/openmod/openmod