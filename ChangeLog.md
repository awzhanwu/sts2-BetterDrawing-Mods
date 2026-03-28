# Changelog

### V0.1.0
- Initial release.

### V0.1.1
- Fixed:
  - Erasing caused display errors in highlight the player.

### V0.1.2
- Reworked for the game v0.99.1.
- Removed unwanted lib `Steamworks.NET`.

### V0.2.0 
- Display:
  - Added `WidthLabel` to show the currently selected width.
  - Added a pen preview on the cursor when using the drawing tool, eraser, or holding **Ctrl** (for width adjustment preview).
- Features:
  - Added a button in the multiplayer status panel to hide a player's drawings.
    - This button is only visible in the map menu.
    - It will support the official option `Disable map drawings in multiplayer` once the beta feature is merged into the main release.
  - Added a UI button for the existing undo operation *(Ctrl+Z)*.
  - Press **Ctrl+ Mouse Wheel** can change width quickly now.
- Other:
  - Refactored **a large** portion of the codebase.
  - Buttons now support controller input.
  - Updated the `images` on the GitHub page.