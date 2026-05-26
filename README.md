# Candy Runner

A 2D endless runner mobile game built in Unity where players collect candies, avoid obstacles, and use powerups to survive as long as possible.


# Features

- Endless runner gameplay
- Candy collection and score tracking
- Obstacle avoidance system
- Powerup system
  - Shield
  - Speed Boost
- Object Pooling for optimized performance
- AssetBundle-based dynamic content loading
- Multiple player profiles
- Highscore tracking
- Leaderboard system
- Restart and Game Over system

---

# Project Structure

## Scripts

### Managers
- GameManager
- PoolManager
- AssetBundleManager

### Gameplay
- PlayerController
- MoveLeft
- ParallaxLayer

### Spawners
- ObjectSpawner

### UI
- GameOverScreen
- HomeScreenPlayButton
- ProfileUICard

### Data
- PlayerData
- LeaderPlayer

---

# AI-Assisted Improvements

This project was improved using AI-assisted development tools while preserving the original gameplay experience.

Implemented improvements include:
- Project structure cleanup and reorganization
- Startup/runtime safety validation
- Null reference prevention checks
- Improved restart flow handling
- Runtime robustness improvements
- Documentation and verification cleanup

Additional details are documented in:
- `AI_USAGE.md`

---

# Runtime Robustness Improvements

The following runtime stability improvements were added:

- Singleton safety checks
- Improved coroutine handling
- Restart state cleanup
- Safer AssetBundle loading checks
- Improved object pooling stability

---

# Restart Verification

The restart flow was verified for:
- Score reset
- Powerup reset
- UI reset
- Player state reset
- Spawn system restart
- Object pooling cleanup
- Game over state cleanup

---

# Powerup Verification

Verified powerups:

## Shield
- Activation and expiration
- Restart cleanup

## Speed Boost
- Temporary speed increase
- Proper timer reset

# Technologies Used

- Unity Engine
- C#
- TextMeshPro
- Unity AssetBundles

---

# How to Run

1. Open the project in Unity.
2. Open the main gameplay scene.
3. Press Play in the Unity Editor.

---

# Repository Notes

This repository contains an additional AI-assisted improvement branch:

- `ai-improvements`

This branch includes:
- Refactoring commits
- Runtime robustness improvements
- Verification updates
- AI usage documentation

---
