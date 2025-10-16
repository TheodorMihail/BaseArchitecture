<h1>BaseArchitecture</h1>
<h3>A clean and scalable Unity project designed to showcase a modular code foundation that follows industry best practices — built for long-term maintainability and team scalability.</h3>

---
<h2>🧱 Overview</h2>

It includes a few placeholder scenes on the surface, but under the hood it brings together:

- ✅ **SOLID principles** — to ensure clear responsibilities and maintainable code
- 🎮 **Flexible MVC pattern** — with optional typed parameters and results
- 📺 **Dual UI System** — Screens for blocking flows and HUDs for persistent UI
- 🔁 **State Machines** — for predictable, extensible gameplay flow
- 🧠 **Zenject (Dependency Injection)** — for decoupling and testability
- 🔄 **UniTask** — for clean async/await operations
- 📦 **Addressables** — for efficient asset management
- ⚡ **Assembly Definitions** — for faster compile times

---

<h2>🧰 What's inside</h2>

- **UIComponent Abstraction** — Unified base for both Screens and HUDs
- **Screen System** — Blocking UI flows with async/await support
- **HUD System** — Persistent UI elements that don't block execution
- **Simplified MVC Architecture** — Single base classes with interface-based features
- **Type-Safe UI Flow** — Strongly-typed screen parameters and results
- **Error Handling** — Built-in error management system
- **Utility Extensions** — Helper methods for common patterns
- **Assembly Definitions** — Clean boundaries and faster compilation
- Example usage of installers, managers, controllers, and views

---

<h2>📦 Why this matters</h2>

This project is ideal as a starting point for new Unity games, offering a battle-tested architectural foundation. Whether you're prototyping or preparing for production, this structure helps avoid spaghetti code and scaling issues from the start.

After years of working in Unity on everything from mobile games to large-scale multiplayer titles, I wanted a solid base I could build anything on — fast, clean, and scalable. This repo is a result of that thinking.

---

<h2>🏗️ Architecture Guide</h2>

### MVC Pattern

The framework provides **Screens** and **HUDs** as predefined MVC containers:

- **Screens** — Block execution and wait for user interaction (e.g., dialogs, menus)
- **HUDs** — Remain visible without blocking user interaction

Both control their MVC lifecycle automatically:

**Models** are data containers:
- Can implement interfaces for automatic parameter initialization

**Views** are Unity MonoBehaviours:
- Automatically instantiated from prefabs using attributes
- Expose events for user interactions

**Controllers** orchestrate interactions:
- Subscribe to View events
- Update Models and Views
- Work with both Screens and HUDs

**Parameters & Results:**
- Screens can accept typed parameters for initialization
- Screens can return typed results when closed

### Scene Management

Each scene is organized with:

- **Scene Installer** — Zenject bindings for scene-specific dependencies
- **State Machine** — Controls scene flow through multiple states
- **States** — Individual scene phases (e.g., loading, gameplay, pause)

### Core Managers

**ScenesManager** — Handles scene transitions with async operations

**Factory** — Creates objects and instantiates prefabs with dependency injection

**AddressablesManager** — Loads assets asynchronously via Addressables

**ErrorManager** — Handles error logging and displays error-specific screens

---

<h2>🔧 Using as Git Submodule</h2>

This repository is designed to be used as a **git submodule** in your Unity game projects, allowing you to share the core framework across multiple games while keeping game-specific code separate.
