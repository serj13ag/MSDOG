# Dogzilla

One of the recent game jam projects, built with a focus on clean architecture, dependency injection, and separation of concerns. <br/>
The project showcases my approach to structuring Unity applications for scalability and maintainability. <br/>
👉 Playable build and details available on [itch.io](https://artbooze.itch.io/dgzlmtlcr)

## Key Features

### 🟢Entry Point
The project uses the Bootstrap scene with Bootstrapper script as the single entry point. <br/>
That approach helps you to control application initialization and avoid problems related to Unity Script Execution Order. <br/>
[Bootstrapper.cs](../master/src/MSDOG/Assets/Scripts/Infrastructure/Bootstrapper.cs)

### 🟢Lifetime scopes
Dependency Injection is handled with VContainer as main DI Framework. <br/>
Hierarchical lifetime scopes separate different layers in the application. <br/>
In each of that scope you can see explicit registration of all modules. <br/>
[GlobalLifetimeScope.cs](../master/src/MSDOG/Assets/Scripts/Infrastructure/GlobalLifetimeScope.cs)
[GameplayLifetimeScope.cs](../master/src/MSDOG/Assets/Scripts/Infrastructure/GameplayLifetimeScope.cs)
[GameplayTvHudLifetimeScope.cs](../master/src/MSDOG/Assets/Scripts/Infrastructure/GameplayTvHudLifetimeScope.cs)

### 🟢Game State Machine
State Machine is used to define the core application states (Menu, Gameplay, etc.). <br/>
Each state implements Enter() and Exit() methods where you can control transitions between states. <br/>
This keeps the application flow clear and predictable. <br/>
[GameStateMachine.cs](../master/src/MSDOG/Assets/Scripts/Infrastructure/StateMachine/GameStateMachine.cs)

### 🟢Domain and View Separation
The core game logic is implemented in pure c# classes, independent of MonoBehaviour. <br/>
Views are responsible for rendering, physics and input, while domain classes handle game rules and logic. <br/>
The connection between them is implemented via mediators, presenters or direct reference (when simplicity is enough). <br/>
That makes the project more testable, maintainable and scalable. <br/>
[Projectile.cs](../master/src/MSDOG/Assets/Scripts/Gameplay/Projectiles/Projectile.cs)
[BaseProjectileView.cs](../master/src/MSDOG/Assets/Scripts/Gameplay/Projectiles/Views/BaseProjectileView.cs)
[BaseCooldownAbility.cs](../master/src/MSDOG/Assets/Scripts/Gameplay/Abilities/Core/BaseCooldownAbility.cs)
[OneTimeAbilityPresenter.cs](../master/src/MSDOG/Assets/Scripts/Gameplay/Abilities/View/OneTimeAbilityPresenter.cs)
