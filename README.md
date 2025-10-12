<h1>BaseArchitecture</h1>
<h3>A clean and scalable Unity project designed to showcase a modular code foundation that follows industry best practices — built for long-term maintainability and team scalability.</h3>

---
<h2>🧱 Overview</h2>

It includes a few placeholder scenes on the surface, but under the hood it brings together:

- ✅ **SOLID principles** — to ensure clear responsibilities and maintainable code
- 🎮 **MVC pattern** — to separate logic, UI, and data cleanly
- 🔁 **State Machines** — for predictable, extensible gameplay flow
- 🧠 **Zenject (Dependency Injection)** — for decoupling and testability
- 🔄 **UniTask** — for clean async/await operations
- 📦 **Addressables** — for efficient asset management

---

<h2>🧰 What's inside</h2>

- Simple demo scenes to showcase structure (not gameplay-focused)
- Folder structure built for modular feature development
- Interfaces and base classes for consistent extension
- Example usage of installers, managers, controllers, and views
- Minimal dependencies, ready to plug into your own pipeline
- **Assembly definitions** for faster compile times

---

<h2>📦 Why this matters</h2>

This project is ideal as a starting point for new Unity games, offering a battle-tested architectural foundation. Whether you're prototyping or preparing for production, this structure helps avoid spaghetti code and scaling issues from the start.

After years of working in Unity on everything from mobile games to large-scale multiplayer titles, I wanted a solid base I could build anything on — fast, clean, and scalable. This repo is a result of that thinking.

---

<h2>🔧 Using as Git Submodule</h2>

This repository is designed to be used as a **git submodule** in your Unity game projects, allowing you to share the core framework across multiple games while keeping game-specific code separate.

---

<h2>🏗️ Architecture Guide</h2>

### MVC Pattern

**Screens** manage the full MVC lifecycle:
- Can optionally accept **typed parameters** via `IScreenWithParams<TParam>`
- Can optionally return **typed results** via `IScreenWithResult<TResult>`
- Automatically create and manage Model, View, Controller

**Models** handle data and business logic:
- Implement `IModelWithParams<TParam>` for automatic parameter initialization
- Should be framework-agnostic (no Unity dependencies)

**Views** are Unity MonoBehaviours:
- Use `[AddressablePath("path")]` attribute for automatic prefab loading
- Expose events for user interactions
- Update UI based on data

**Controllers** orchestrate Model-View interactions:
- Subscribe to View events in `Initialize()`
- Update Models based on user input
- Update Views based on Model changes
- Use `CloseScreenWithResult()` extension for returning results
