# ThirdPerson

A third-person action game prototype built in Unity, featuring a robust state-machine-based character controller, combat system, and immersive fantasy environment.

## Prerequisites

- **Unity Version**: 2022.3.62f2
- **Render Pipeline**: Universal Render Pipeline (URP)

## Key Features

- **State Machine Architecture**: Clean and extensible state pattern implementation for both Player and Enemy logic (`PlayerStateMachine`, `EnemyStateMachine`).
- **Combat System**:
  - Attack, Block, and Dodge mechanics.
  - Target Locking system (`Targeter`, `Target`).
  - Hitbox detection and damage handling (`WeaponDamage`, `Health`).
- **Cinematic Camera**: Third-person camera control using Unity's Cinemachine.
- **Movement**:
  - Jumping mechanics (`PlayerJumpingState`).
  - Ledge climbing and hanging (`PlayerHangingState`, `PlayerPullUpState`).
- **Modern Input**: Fully integrated with Unity's new Input System (`Controls.inputactions`).
- **Environment**: Includes fantasy environment assets and skyboxes.

## Getting Started

1. **Open the Project**:

   - Open Unity Hub.
   - Add the project directory.
   - Open the project with Unity 2022.3.62f2.

2. **Run the Game**:
   - Navigate to `Assets/Scenes` in the Project window.
   - Open `SampleScene.unity` or `Sandbox.unity` (if available).
   - Press the **Play** button in the Editor.

## Project Structure

The core logic is located in `Assets/Scripts`:

- **StateMachines/**: Contains the base `StateMachine` and `State` classes, along with specific implementations for Player and Enemy.
  - `Player/`: States like `PlayerAttackingState`, `PlayerDodgingState`, `PlayerFreeLookState`.
  - `Enemy/`: States like `EnemyChasingState`, `EnemyAttackingState`, `EnemyIdleState`.
- **Combat/**: Scripts handling damage, health, and targeting (`Attack`, `Health`, `Targeting/`).
- **Controls.cs**: Generated C# class from the Input System asset.
- **ForceReceiver.cs**: Handles external forces (e.g., knockback) on characters.
- **InputReader.cs**: Intermediary between Input System events and game logic.

## Dependencies

- `com.unity.cinemachine`: 2.10.5
- `com.unity.inputsystem`: 1.14.2
- `com.unity.textmeshpro`: 3.0.9
- `com.unity.ai.navigation`: 1.1.7
