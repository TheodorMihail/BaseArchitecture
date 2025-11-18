<h1>BaseArchitecture</h1>
<h3>A clean and scalable Unity project designed to showcase a modular code foundation that follows industry best practices — built for long-term maintainability and team scalability.</h3>

---
<h2>🧱 Overview</h2>

It includes a few placeholder scenes on the surface, but under the hood it brings together:

- ✅ **SOLID principles** — to ensure clear responsibilities and maintainable code
- 🎮 **Flexible MVC pattern** — with optional typed parameters and results
- 🔁 **State Machines** — for predictable, extensible gameplay flow
- 🔄 **UniTask** — for clean async/await operations
- 📦 **Addressables** — for efficient asset management
- ♻️ **Object Pooling** — for optimized object reuse and reduced GC pressure
- 🗂️ **Repository Pattern** — for centralized configuration and data management
- 📬 **Message Bus** — for decoupled pub/sub communication between systems
- 🛠️ **Extension Methods** — for cleaner code and reusable utilities
- ⚡ **Assembly Definitions** — for faster compile times
- 🧪 **Comprehensive test coverage** — with EditMode and PlayMode tests

---

<h2>📦 Why this matters</h2>

This project is ideal as a starting point for new Unity games, offering a battle-tested architectural foundation. Whether you're prototyping or preparing for production, this structure helps avoid spaghetti code and scaling issues from the start.

After years of working in Unity on everything from mobile games to large-scale multiplayer titles, I wanted a solid base I could build anything on — fast, clean, and scalable. This repo is a result of that thinking.

---

<h2>🏗️ Architecture Guide</h2>

### 🎮 MVC Pattern

The framework provides **Screens** and **HUDs** as predefined MVC containers:

- **Screens** — Block execution and wait for user interaction (e.g., dialogs, menus)
- **HUDs** — Remain visible without blocking user interaction

Both control their MVC lifecycle automatically:

- **Models** — Data containers with optional auto-initialization from parameters
- **Views** — Unity MonoBehaviours instantiated from prefabs using attributes
- **Controllers** — Orchestrate interactions between Models and Views
- **Parameters & Results** — Type-safe screen communication

### 🎬 Scene Management

Each scene is organized with:

- **Scene Installer** — Zenject bindings for scene-specific dependencies
- **State Machine** — Controls scene flow through multiple states
- **States** — Individual scene phases (e.g., loading, gameplay, pause)

### 🛠️ Core Systems

- **ScenesManager** — Handles scene transitions with async operations
- **Factory** — Creates objects and instantiates prefabs with dependency injection
- **AddressablesManager** — Loads assets asynchronously via Addressables
- **ErrorManager** — Handles error logging and displays error-specific screens
- **ObjectPooling** — Reuses objects for better performance via `IPoolableObject` interface
- **Repository** — Centralizes configuration objects for easy access via `IRepositoryObject` interface
- **MessageBus** — Publish/subscribe pattern for decoupled system communication via `IMessageObject` interface

### 🧰 Extension Methods

- **LoggingExtensions** — Type-aware logging with `this.Log()`, `this.LogWarning()`, `this.LogError()`
- **UIExtensions** — Async UI animations with `FadeToAsync()` and `CountdownAsync()` for smooth transitions
- **CancellationTokenSourceExtensions** — Safe cancellation with `CancelAndDispose()` helper method

### 🧪 Testing

Comprehensive test coverage using Unity Test Framework, NUnit, and Zenject Test Framework:

- **EditMode** — MessageBus, Repository
- **PlayMode** — CustomFactory, ObjectPooling

Tests are editor-only and excluded from player builds.

---

<h2>🔧 Using as Git Submodule</h2>

This repository is designed to be used as a **git submodule** in your Unity game projects, allowing you to share the core framework across multiple games while keeping game-specific code separate.
