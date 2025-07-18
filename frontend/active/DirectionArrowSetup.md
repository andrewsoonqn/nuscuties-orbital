# Direction Arrow Setup Guide

This guide explains how to integrate the direction arrow system into your active dungeon gameplay.

## Components Created

1. **EnemyTracker.cs** - Singleton that tracks all active enemies
2. **DirectionArrow.cs** - UI component that displays the arrow
3. **direction_arrow.tscn** - Scene file for the direction arrow UI

## Setup Instructions

### 1. Add EnemyTracker to Your Main Scene

In your main active game scene (e.g., `active_game.tscn`):

1. Add a new Node and attach the `EnemyTracker.cs` script
2. Name it "EnemyTracker"
3. Make sure it's added early in the scene tree so it initializes before enemies spawn

### 2. Add DirectionArrow to Your UI

1. In your main scene, add the `direction_arrow.tscn` as a child of your UI layer
2. The arrow should be added to a CanvasLayer so it appears on top of gameplay
3. Set the Player reference in the DirectionArrow component

### 3. Customize Arrow Appearance

Replace the default arrow texture:

1. Create or find an arrow sprite (pointing right by default)
2. In `direction_arrow.tscn`, replace the texture resource in ArrowTexture node
3. Adjust the `ArrowDistance` property to control how far from center the arrow appears

### 4. Scene Structure Example

```
Main (Node2D)
├── EnemyTracker (Node) - [EnemyTracker.cs]
├── GameWorld (Node2D)
│   ├── Player (CharacterBody2D)
│   ├── EnemySpawner (Node2D) - [EnemySpawner.cs]
│   └── ...
└── UI (CanvasLayer)
    ├── DirectionArrow (Control) - [DirectionArrow.cs]
    └── ...
```

## Configuration

### DirectionArrow Properties

- **ArrowDistance**: Distance from screen center (default: 100.0)
- **Player**: Reference to the player node
- **ArrowTexture**: The TextureRect showing the arrow sprite

### Arrow Behavior

- Shows only when enemies are present
- Points to the nearest enemy
- Automatically hides when no enemies exist
- Updates position and rotation in real-time

## Custom Arrow Texture

For best results, use an arrow texture that:
- Points to the right (0 degrees)
- Is centered around its pivot point
- Has good contrast for visibility

## Troubleshooting

1. **Arrow not showing**: Ensure EnemyTracker is initialized and Player reference is set
2. **Arrow pointing wrong direction**: Check that your arrow texture points right
3. **Performance issues**: The system automatically cleans up destroyed enemies

## Optional Enhancements

You can extend the system by:
- Adding distance text to show how far the enemy is
- Changing arrow color based on enemy type
- Adding smooth rotation transitions
- Implementing different arrow styles for different enemy priorities
