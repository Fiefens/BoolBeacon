# Bool Beacon

A unity game of stacking various true, false, or faulty statements.  

# Bool Beacon High Score System

The high score system uses Unity's built-in PlayerPrefs and JSON serialization.

## Overview

When a game ends, the player's name and score are stored locally in a high score list. The list is:
- Limited to the **top 5 scores**.
- Stored as a **JSON string** in `PlayerPrefs` under the key `"HighScores"`.
- Loaded, updated, and saved every time the `UpdateScoreText` component runs (e.g., on a game over screen).