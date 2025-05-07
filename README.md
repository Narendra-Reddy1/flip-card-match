# psdsdnpskaqw

Welcome to **psdsdnpskaqw**, a fun and visually appealing card-matching memory game.
This document outlines the structure, functionality, and core logic of the project for the people who want to understand the project.

---

## Folder Structure

```
MatchCards/
├── _Game/
│   ├── _Scripts/          # All game-related scripts
│   ├── Art/               # All art assets
│   ├── Audio/             # Audio assets
│   ├── Fonts/             # Fonts used in the game
│   ├── Prefabs/           # Prefab files
│   ├── Scenes/            # Unity scene files
│   ├── SO/                # ScriptableObject assets
├── Thirdparty/              # External third-party packages
```

---

## Core Gameplay Classes

### `BaseCard`

* Base implementation of a card.
* Provides the foundation for card interaction.

### `FlipCard`

* Handles user input for card interactions.
* Manages card states like `Revealed`, `Hidden`, and `Matched`.
* Executes card flipping animations.

### `CardsManager`

* Spawns cards based on `LevelDataSO` configuration.
* Dynamically sizes cards according to grid dimensions.
* Tracks all active cards during gameplay.

### `MatchHandler`

* Core class responsible for matching logic.
* Supports matching for any number of cards (`N-card matching`) using the `MIN_CARDS_TO_MATCH` constant.
* Ensure consistency in level setup if the constant is changed (update SOs accordingly).

---

## Data Persistence

### `PlayerDataManager.cs`

* Project-independent logic for saving/loading player data.
* Handles first-time setup and session data.

### `CardGame.PlayerDataManager.cs`

* Project-specific data handling.
* Stores game-specific persistent data (like level info, score, card states).

---

## Utilities

### `Konstants`

* Centralized place for configuration constants like `MIN_CARDS_TO_MATCH`, etc.

### `GlobalVariables`

* Contains helper variables accessible across the game globally.

---

## Tracking Systems

### `ScoreHandler`

* Handles player score tracking and updates.

### `StreakHandler`

* Tracks consecutive successful matches (streaks).

---

## ScriptableObjects

### `LevelDataSO`

* Contains configuration for an individual level (e.g., grid size, unique pairs).

### `LevelDatabaseSO`

* Maintains a collection of all `LevelDataSO` instances.

### `SpriteDatabaseSO`

* Holds all card icons and other in-game sprites.

### `AudioAsset`

* Maps `AudioID` enum values to corresponding audio clips.

---

## Managers

### `AudioManager`

* Controls audio playback, stopping, and toggling.
* Centralized sound system for UI and gameplay.

### `UIManager`

* Handles UI transitions, panels, and popups.
* Manages level and home UI switching.

### `GameManager`

* Main coordinator for gameplay initialization.
* Interfaces between `PlayerDataManager` and other core gameplay components.

---

## Getting Started

1. Open the project in Unity.
2. Check the scenes in `_Game/Scenes`.
3. Configure levels via `LevelDataSO` and `LevelDatabaseSO`.
4. Run the `HomeScene` and begin matching cards!

---

## Notes

* Modify `MIN_CARDS_TO_MATCH` carefully; it affects gameplay logic.
* Ensure icons in `SpriteDatabaseSO` cover the number of unique sets required.
* Use `GlobalVariables` for any shared runtime flags.

---

For any questions or support, feel free to reach out to me.

Happy Matching! ✨🏋️
