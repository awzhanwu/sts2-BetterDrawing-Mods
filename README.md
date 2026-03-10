<div align="center">

# Better Drawing

[**简体中文**](README_ZH.md)

![Version](https://img.shields.io/badge/Version-0.1.0-blue.svg)
![Game](https://img.shields.io/badge/Slay_The_Spire_2-Mod-red.svg)
![Platform](https://img.shields.io/badge/Platform-Windows%20|%20Godot-lightgrey.svg)

*A better drawing mod for Slay the Spire 2.*

</div>

The mod adds more user-defined functions to the map drawing system of the original game, and provides additional undo and highlight player drawings functions.

## ✨ Core Features

- Adds new buttons to the official map drawing toolbar, allowing players to customize **color** and **line width**.
- On the map screen, press **Ctrl+Z** to undo the previous drawing or erasing action.
- In multiplayer mode, hovering the mouse over a player's status bar will **highlight** that player's map drawings.

## 🎮 Installation

1. Download the latest **zip** from the **Releases** page.
2. Extract the archive and copy the inner `BetterDrawing` folder to your game directory: `<Slay the Spire 2>/mods/`.
3. Launch the game and enable the mod in the **Mods** menu.

## 🐞Bugs

- The official eraser may cause errors when erasing colors that are not currently selected. Fixing this would require overriding the original eraser logic, which is not planned at this time.
  - **It is not recommended** to use the eraser while this mod is installed.
  - If you still need to use it, please **use the sampler** from the palette to select **the color you want to erase** before using the eraser.

## ⚠️ Copyright Notice

This mod uses textures from the official game "Slay the Spire". These assets **remain the property of the original game developer**.  
The MIT license of this mod **only applies to the code and resources created by the mod author**.

## ⚙️ Build

If you want to build the project yourself:
- Set `Sts2Dir` in `BetterDrawing.csproj` to the path of your game installation.
- Or build using: `dotnet build -p:Sts2Dir="Your Slay the Spire 2 Path"`
