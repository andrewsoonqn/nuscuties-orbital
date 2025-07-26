# Daily-Quest Refactor – Design Decision Record

## Context

The original daily quest system displayed a list of completable quests and offered a separate, global “Edit Quests” overlay to add, edit, or delete them. Although functional, this user experience felt disconnected. Editing a quest required a significant context switch away from the main view, and the overlay duplicated information already visible on the screen, creating a clunky and unintuitive workflow.

This design led to several pain-points, most notably a high cognitive load on the user who had to locate a quest, remember its details, open the editor, and find the quest again in a separate list. This UI was not only redundant but also suffered from poor discoverability, as users often expected to edit a quest directly where it appeared. Initial attempts to remedy this within the existing model—by adjusting button aesthetics or pre-scrolling the list—proved insufficient, indicating a fundamental structural issue. The problem wasn't the components themselves, but the disjointed interaction model.

## The Refactored Solution

The chosen solution was to introduce per-quest inline editing. Now, each quest component in the main list features an "Edit" button that spawns a dedicated, modal `QuestEditPanel` for that single quest. This approach provides a more localized and intuitive interaction, allowing users to manipulate exactly the quest they are looking at. It also streamlines the UI by eliminating the redundant bulk editor, progressively disclosing the edit controls only when they are needed. Crucially, this was achieved while reusing the existing `QuestManager` signals, ensuring the underlying data flow remained consistent and minimizing regression risk.

## Architectural Principles

This refactor was explicitly shaped by several SOLID principles, each applied concretely in the new architecture. For the **Single Responsibility Principle**, we split responsibilities so that `CompletableQuestComponent` is only responsible for rendering quest information and handling user interactions like clicking "Edit", while `QuestEditPanel` is solely responsible for editing a single quest’s details—this is enforced by the fact that the edit panel is instantiated only when needed and receives the quest data as input, with no knowledge of the quest list or other UI. The `QuestManager` is the only class that manages quest data, emitting signals when quests are updated, added, or deleted, and does not handle any UI logic.

For the **Open/Closed Principle**, we introduced the `QuestEditPanel` as a new scene and connected it to the existing system entirely through signals and method calls, without changing the internal logic of `QuestManager` or `QuestLogManager`. For example, the process of editing a quest now involves emitting a signal from the edit panel, which is already handled by the `QuestManager`’s existing API, demonstrating that the system was extended (not modified) to support new UI behavior.

Applying the **Dependency Inversion Principle**, all UI components—including both the quest list and the edit panel—interact with the quest system exclusively through the `QuestManager`’s public methods and signals. For instance, when a quest is edited, the edit panel calls `QuestManager.UpdateQuest(questId, newData)`, and listens for the `QuestUpdated` signal to update the UI, without ever accessing the underlying data storage or file format. This abstraction means that if we later replace the quest storage backend (e.g., from JSON to a database), no UI code will need to change, as it depends only on the stable API and signals of `QuestManager`.

The implementation also makes extensive use of established design patterns to ensure maintainability and clarity. At the core is the **Observer** pattern, realized through Godot's signal system: the `QuestManager` emits signals whenever quest data changes—such as when a quest is added, updated, or deleted—and all relevant UI components (like the quest list and edit panels) subscribe to these signals. This ensures that any change in the underlying quest data is immediately reflected across the interface, keeping all views synchronized without requiring manual refreshes or tight coupling between components.

Within the `QuestEditPanel`, the **State** pattern is employed to manage the panel's behavior as it transitions between different modes. For example, when a user first opens the panel, it may be in a read-only "view" state, displaying the quest details. If the user clicks "Edit," the panel transitions into an "edit" state, enabling input fields and showing save/cancel controls. This separation of states within the panel helps keep the logic organized and makes it easy to extend or modify the panel's behavior in the future.

Additionally, the **Factory** pattern is used in the `DailyQuestUi` to dynamically create new instances of the `QuestEditPanel` as needed. Rather than having a persistent edit panel in the scene, the UI uses Godot's `PackedScene.Instantiate` method to generate a fresh panel each time the user chooses to edit a quest. This approach not only conserves resources but also ensures that each edit session is isolated, reducing the risk of state leakage or unintended interactions between different quests' edit panels.

## Rationale and Risk

Other alternatives, such as building editing controls directly into each quest row, were rejected to avoid cluttering the main list and complicating the layout, especially for mobile views. Retaining the bulk editor with better highlighting was also dismissed as it failed to address the core UX problem. The chosen approach carries minimal risk because the data schema remains unchanged, ensuring backward compatibility with existing user data stored in JSON. To further mitigate risk, a fix was implemented to prevent quest ID collisions across sessions, and the plan includes adding new integration tests for the edit/delete pathways.

The expected impact of this refactor is a more intuitive and efficient user experience for quest management, along with a simplified and more maintainable codebase due to the removal of the redundant editor scene.
