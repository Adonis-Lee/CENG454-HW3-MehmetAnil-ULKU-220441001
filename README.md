<div align="center">

# Core Breach

### CENG454 Game Programming · Homework 3

**A 2D top-down arena defense prototype built around design patterns.**

![Unity](https://img.shields.io/badge/Engine-Unity%206-black?style=for-the-badge&logo=unity)
![CSharp](https://img.shields.io/badge/Language-C%23-5B7EF5?style=for-the-badge)
![Patterns](https://img.shields.io/badge/Patterns-5-06B6D4?style=for-the-badge)
![Status](https://img.shields.io/badge/Status-Submitted-22C55E?style=for-the-badge)

</div>

---

## Project Info

| Field | Value |
| --- | --- |
| Student | Mehmet Anıl ÜLKÜ |
| Student ID | 220441001 |
| University | University of Turkish Aeronautical Association |
| Course | CENG454 Game Programming |
| Assignment | Homework 3 |
| Engine | Unity 6 (6000.3.13f1) |
| Language | C# |
| Main Scene | `Assets/_Project/Scenes/Level01.unity` |

## Game Overview

Core Breach is a small 2D top-down arena defense game. The player protects an energy core in the middle of the map while enemies spawn in waves and move toward either the core or the player.

The assignment focuses on software design patterns, so the gameplay systems were built around clear contracts, events, pooled objects, strategy assets, and runtime weapon decorators.

### Controls

| Action | Input |
| --- | --- |
| Move | `W A S D` |
| Aim | Mouse position |
| Shoot | Left mouse button |
| Pick up upgrade | Walk over pickup |

### Win / Lose

| Result | Condition |
| --- | --- |
| Win | Clear all 5 waves while the core is alive |
| Lose | Core HP reaches 0 |

### Enemy Roster

| Enemy | HP | Speed | Target | Strategy Setup |
| --- | ---: | ---: | --- | --- |
| Grunt | 4 | 2.5 | Core | `DirectMove` + `CoreTarget` |
| Hunter | 2 | 4.0 | Player | `ChasePlayerMove` + `PlayerTarget` |
| Tank | 10 | 1.5 | Core | `DirectMove` + `CoreTarget` |

---

## Implemented Design Patterns

| Pattern | Main Files / Classes | Purpose |
| --- | --- | --- |
| Observer | `Core`, `Enemy`, `WaveSpawner`, `WaveTracker`, `CoreHpView`, `WaveView`, `ScreenShake`, `DropSystem`, `GameStateMachine` | Keeps gameplay events separate from UI, VFX, drops, and game state |
| Strategy | `IMovementStrategy`, `ITargetingStrategy`, `DirectMove`, `ChasePlayerMove`, `CoreTarget`, `PlayerTarget`, `EnemyConfig` | Lets enemy types change movement and targeting without changing `Enemy.cs` |
| Object Pool | `IPool<T>`, `IPoolable`, `GenericPool<T>`, `Projectile`, `Enemy`, `Pickup` | Reuses frequently spawned gameplay objects |
| Decorator | `IWeaponBehavior`, `BaseWeapon`, `PierceDecorator`, `DoubleDamageDecorator`, `SpreadDecorator`, `BurstDecorator`, `ChainLightningDecorator` | Stacks weapon upgrades at runtime |
| Interfaces | `IDamageable`, `IWeaponBehavior`, `IMovementStrategy`, `ITargetingStrategy`, `IPool<T>`, `IPoolable` | Creates shared contracts between systems |

<details>
<summary><strong>Observer Pattern</strong></summary>

The project uses C# events to publish gameplay changes.

Events:

- `Core.Damaged`
- `Core.CoreDestroyed`
- `Enemy.Died`
- `WaveSpawner.WaveStarted`
- `WaveTracker.WaveCompleted`
- `WeaponHolder.UpgradeApplied`

Example subscribers:

- `CoreHpView`
- `PlayerHealthView`
- `WaveView`
- `ScreenShake`
- `DropSystem`
- `GameStateMachine`

This means the core does not need to know about the HUD or camera shake. The enemy does not need to know about the wave tracker or pickup system.

</details>

<details>
<summary><strong>Strategy Pattern</strong></summary>

Enemy behavior is split into two strategy families:

- `IMovementStrategy`
- `ITargetingStrategy`

Concrete implementations:

- `DirectMove`
- `ChasePlayerMove`
- `CoreTarget`
- `PlayerTarget`

Each enemy type is configured with an `EnemyConfig` ScriptableObject. Because of this, Grunt, Hunter, and Tank all use the same `Enemy.cs` class.

</details>

<details>
<summary><strong>Object Pool Pattern</strong></summary>

Pooling is handled by:

- `IPool<T>`
- `IPoolable`
- `GenericPool<T>`

Pooled gameplay objects:

- `Projectile`
- `Enemy`
- `Pickup`

`GenericPool<T>` wraps Unity's `ObjectPool<T>` and calls `OnSpawn()` / `OnDespawn()` on pooled objects. This gives every reused object a clear lifecycle.

</details>

<details>
<summary><strong>Decorator Pattern</strong></summary>

The weapon system uses `IWeaponBehavior`.

Base implementation:

- `BaseWeapon`

Runtime decorators:

- `PierceDecorator`
- `DoubleDamageDecorator`
- `SpreadDecorator`
- `BurstDecorator`
- `ChainLightningDecorator`

When the player collects a pickup, `WeaponHolder` wraps the current weapon behavior with a new decorator.

Example:

```csharp
weapon = new PierceDecorator(weapon);
weapon = new DoubleDamageDecorator(weapon);
weapon = new SpreadDecorator(weapon);
```

</details>

<details>
<summary><strong>Interfaces</strong></summary>

Main custom interfaces:

- `IDamageable`
- `IWeaponBehavior`
- `IMovementStrategy`
- `ITargetingStrategy`
- `IPool<T>`
- `IPoolable`

Examples:

- `Core`, `Enemy`, and `PlayerHealth` implement `IDamageable`.
- `BaseWeapon` and all weapon decorators implement `IWeaponBehavior`.
- `Projectile`, `Enemy`, and `Pickup` implement `IPoolable`.

</details>

---

## Project Structure

```text
CoreBreach/
  Assets/
    _Project/
      Runtime/
        Bootstrap/
          LevelInstaller.cs
        Domain/
          Combat/
          Core/
          Enemies/
          GameState/
          Pickups/
          Player/
          Projectiles/
          Waves/
          Weapons/
        Infrastructure/
          Pooling/
        Presentation/
          HUD/
          VFX/
      Scenes/
        Level01.unity
      ScriptableObjects/
```

## Main Runtime Classes

| Class | Role |
| --- | --- |
| `LevelInstaller` | Composition root and scene wiring |
| `PlayerController` | WASD movement, mouse aim, left-click fire |
| `WeaponHolder` | Holds the active `IWeaponBehavior` chain |
| `Enemy` | Damage, movement strategy ticking, death event |
| `WaveSpawner` | Starts and spawns the 5-wave sequence |
| `WaveTracker` | Observes enemy deaths and publishes wave completion |
| `GameStateMachine` | Win/lose state handling |
| `GenericPool<T>` | Pool wrapper for reusable gameplay objects |

## How to Run

1. Clone this repository.
2. Open Unity Hub.
3. Open the Unity project folder:

   ```text
   CoreBreach
   ```

4. Open the scene:

   ```text
   Assets/_Project/Scenes/Level01.unity
   ```

5. Press Play.

## Assets

The project uses imported Unity Asset Store assets for:

- Player sprite
- Zombie enemy sprites
- Grass / ground texture
- Electricity visual effect for the core

All main gameplay and design pattern code is inside:

```text
Assets/_Project
```

## Report

The final homework report is included as:

```text
CENG454_HW3_MehmetAnil_ULKU_220441001.pdf
```

The report contains screenshots, pattern explanations, debugging notes, and pull request workflow evidence.

## Repository Workflow

The project was developed with feature branches and pull requests.

Main work areas:

- Unity project setup
- Player weapon system
- Core damage and observer subscribers
- Enemy strategy system
- Decorator upgrades and pickup drops
- Wave system, UI, and game state
- Visual polish and balance updates

---

<div align="center">

**Core Breach · CENG454 HW3 · Mehmet Anıl ÜLKÜ · 220441001**

</div>

