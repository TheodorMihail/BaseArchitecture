<h1>BaseArchitecture</h1>
<h3>A clean and scalable Unity project designed to showcase a modular code foundation that follows industry best practices, built for long-term maintainability and team scalability.</h3>

---
<h2>🧱 Overview</h2>

It includes a few placeholder scenes on the surface, but under the hood it brings together:

- ✅ **SOLID principles**: to ensure clear responsibilities and maintainable code
- 🎮 **Flexible MVC pattern**: with optional typed parameters and results
- 🔁 **State Machines**: for predictable, extensible gameplay flow
- 🔄 **UniTask**: for clean async/await operations
- 📦 **Addressables**: for efficient asset management
- ♻️ **Object Pooling**: for optimized object reuse and reduced GC pressure
- 🗂️ **Repository Pattern**: for centralized configuration and data management
- 💾 **Persistence**: for saving/loading game data, with a swappable storage implementation
- 📬 **Message Bus**: for decoupled pub/sub communication between systems
- 🔊 **Audio System**: channel-based music/SFX manager with crossfading and persisted volume/mute settings
- 🐞 **Debug Commands**: editor-only hotkey cheats declared by the system that owns the state, compiled out of release builds
- 🛠️ **Extension Methods**: for cleaner code and reusable utilities
- ⚡ **Assembly Definitions**: for faster compile times
- 🧪 **Comprehensive test coverage**: with EditMode and PlayMode tests

---

<h2>📦 Why this matters</h2>

This package is a starting point for new Unity games, providing the groundwork every project needs regardless of genre. Whether prototyping or preparing for production, it establishes structure before the codebase has a chance to accumulate coupling.

It is the result of years of Unity work across mobile games and large-scale multiplayer titles, extracted into a base that can be carried between projects rather than rebuilt each time. It is consumed as a versioned dependency by [Space Invaders](https://github.com/TheodorMihail/SpaceInvaders), which serves as its reference implementation.

### Important notes

These hold throughout the package:

- **No singletons, no static state and no dependency lookups inside the scene.** Every dependency is injected, so nothing has hidden coupling to scene layout
- **Interfaces throughout**, so every system is substitutable, including under test

---

<h2>🏗️ Architecture Guide</h2>

### 🎮 MVC Pattern

The framework provides **Screens** and **HUDs** as predefined MVC containers:

- **Screens**: Block execution and wait for user interaction (e.g., dialogs, menus)
- **HUDs**: Remain visible without blocking user interaction

Both control their MVC lifecycle automatically:

- **Models**: Data containers with optional auto-initialization from parameters
- **Views**: Unity MonoBehaviours instantiated from prefabs using attributes, with an `[AddressablePath]` attribute so the View's prefab is resolved automatically via `IAddressablesManager`
- **Controllers**: Orchestrate interactions between Models and Views
- **Parameters & Results**: Type-safe screen communication
- **Display modes**: `ShowScreen`/`ShowHUD` accept a `UIDisplayTypes` policy (`Queue`, `Parallel`, `Replace`) controlling how a new UI component behaves relative to already-open ones of its category

### 🎬 Scene Management

Each scene is organized with:

- **Scene Installer**: Zenject bindings for scene-specific dependencies
- **State Machine**: Controls scene flow through multiple states
- **States**: Individual scene phases (e.g., loading, gameplay, pause)

### 🛠️ Core Systems

- **ScenesManager**: Handles scene transitions with async operations
- **Factory**: Creates objects and instantiates prefabs with dependency injection
- **AddressablesManager**: Loads assets asynchronously via Addressables
- **ObjectPooling**: Reuses objects for better performance via `IPoolableObject` interface
- **Repository**: Centralizes configuration objects for easy access via `IRepositoryObject` interface
- **PersistenceManager**: Saves/loads typed data via `IPersistenceManager`; ships with a default local (JSON file) implementation and can be swapped for a networked/backend implementation without changing calling code
- **MessageBus**: Publish/subscribe pattern for decoupled system communication via `IMessageObject` interface
- **SoundsManager**: Channel-based audio via `ISoundsManager`: one-shot SFX per channel, looping music/ambience with automatic DOTween-driven crossfading between clips on the same channel, and per-channel volume/mute control persisted automatically through `PersistenceManager`
- **DebugManager**: Editor-only cheat dispatch. Any system implements `IDebugCommandProvider` to declare its own `DebugCommandDTO`s (key, label, action), so the action stays on the class that owns the state instead of widening a production interface. `DebugManager` collects every bound provider, rejects duplicate key bindings with a logged error, logs the resulting keymap on startup, and is the only class polling the keyboard for cheats. Bind it per scene so each instance also picks up that scene's providers. The whole subsystem sits behind `#if UNITY_EDITOR || DEVELOPMENT_BUILD`

### 🧰 Extension Methods

- **LoggingExtensions**: Type-aware logging with `this.Log()`, `this.LogWarning()`, `this.LogError()`
- **UIExtensions**: Async UI animations with `FadeToAsync()` and `CountdownAsync()` for smooth transitions
- **CancellationTokenSourceExtensions**: Safe cancellation with `CancelAndDispose()` helper method
- **ParamsExtensions**: `TryGetParam<T>()` for safely pulling a typed value out of a loosely-typed `object[]` params array with a default fallback

### 🪄 Editor Tools

- **UI Component Creator** (`BaseArchitecture > Create UI Component...`): scaffolds the Model/View/Controller/Screen or HUD scripts for a new UI component.

### 📚 Samples

Example content importable via **Package Manager → Base Architecture → Samples**.

### 🧪 Testing

Comprehensive test coverage using Unity Test Framework, NUnit, and Zenject Test Framework:

- **EditMode**: MessageBus, Repository, PersistenceManager
- **PlayMode**: CustomFactory, ObjectPooling

Tests are editor-only and excluded from player builds.

---

<h2>📥 Installation</h2>

BaseArchitecture is installed as a **Unity package (UPM)** from this Git repo. The package lives in the [`Assets/UnityPackages/BaseArchitecture`](Assets/UnityPackages/BaseArchitecture) folder, so the URL must include `?path=/Assets/UnityPackages/BaseArchitecture`.

**1. Add the OpenUPM scoped registry** to your project's `Packages/manifest.json` (the dependencies below are published on OpenUPM, not the Unity registry):

```json
"scopedRegistries": [
  {
    "name": "OpenUPM",
    "url": "https://package.openupm.com",
    "scopes": [
      "com.svermeulen.extenject",
      "com.cysharp.unitask"
    ]
  }
]
```

**2. Add the package**:

```
https://github.com/TheodorMihail/BaseArchitecture.git?path=/Assets/UnityPackages/BaseArchitecture
```

Dependencies (Extenject, UniTask, Addressables, Newtonsoft Json, Input System) resolve automatically once the registry is set. Append a [release tag](https://github.com/TheodorMihail/BaseArchitecture/tags) such as `#v1.5.0` to the URL to pin a version. **DOTween** must be imported manually from the [Asset Store](http://dotween.demigiant.com/), since it drives the `SoundsManager` crossfade tweens.

**3. Enable the Input System**: set **Project Settings → Player → Active Input Handling** to **Input System Package** or **Both**. `DebugManager` polls `Keyboard.current`, which returns null while the project is on the legacy Input Manager only.

**4. Import samples (optional)** via **Package Manager → Base Architecture → Samples → Error Dialog Screen → Import**.

---

<h2>📄 License</h2>

Licensed under [PolyForm Noncommercial 1.0.0](LICENSE), free for personal use, learning, and other noncommercial purposes. Commercial use requires a separate agreement with the author.
