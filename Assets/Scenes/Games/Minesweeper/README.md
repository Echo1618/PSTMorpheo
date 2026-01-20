# Minesweeper

## Game Overview

### Objective

This is a VR implementation of a Minesweeper game developed in Unity. The player will have to guess where the bombs are located on a 3x3 grid. He will get hints on where the bombs are placed using a physical board with a non-newtonian fluid that he can touch in real life.

### How to Play

The parts where the fluid is solid are the parts where there are bombs hidden underneath. The player can mark these parts by putting flags on them in VR. The goal is to flag all the bombs without triggering any of them and then click on the submit button to win the game.

### Controls

- Use the VR hand recognition to take and put down flags on the grid.
- Press the submit button to check if all bombs are correctly flagged.
- Press the restart button to start a new game.

## Game logic

### Bomb Placement

At the start of the game, 3 cases are randomly chosen to have bombs on the 3x3 grid.

### Flagging Cases

The player can flag a case by placing a flag object on top of it. If the case is already flagged, he can remove the flag by taking it off.

### Win/Loss Conditions

When the player presses the submit button:

- If all bombs are correctly flagged, the player wins the game.
- If any bomb is not flagged or if a non-bomb case is flagged, the player loses the game.

### Restarting the Game

The player can restart the game at any time by pressing the restart button, which will reset the grid and randomly place new bombs.

## Assets Overview

### Scripts

- **CaseScript.cs**: Manages individual grid cases, including bomb status and flag placement.
- **PadScript.cs**: Manages the overall grid, including bomb placement, starting & resetting the game.
- **GameManager.cs**: Handles game state, win/loss conditions, and overall game flow.
- **SubmitScript.cs**: Manages the submit button functionality to check win/loss conditions.
- **RestartScript.cs**: Manages the restart button functionality to reset the game.

### Prefabs

- **Grid prefab**: The main game grid containing all cases.
- **Case prefab**: Individual grid case that can be a bomb or safe.
- **Flag prefab**: The flag object that players can place on cases.
- **Submit Button prefab**: The button players press to submit their guesses.
- **Restart Button prefab**: The button players press to restart the game
