- [ ] Objective
  - [ ] Implement per-quest inline editing UX and retire global quest editor overlay while preserving existing data and persistence layers.

- [ ] Create QuestEditPanel scene and script
  - [ ] Design floating Control scene centred on viewport with semi-transparent modal background.
  - [ ] Add Label nodes for title and description in view mode.
  - [ ] Add LineEdit nodes bound by visible property for edit mode.
  - [ ] Add buttons: Edit / Save (toggled by mode), Delete, Done.
  - [ ] Implement internal state machine switching between view and edit modes.
  - [ ] Connect button signals to QuestManager.Edit and QuestManager.Remove.
  - [ ] Persist changes via QuestLogManager.SaveQuestLog after each mutation.

- [ ] Update Paths.cs
  - [ ] Add constant string QuestEditPanel = "res://daily/quest_edit_panel.tscn".
  - [ ] Remove obsolete QuestEditor constant.

- [ ] Update CompletableQuestComponent scene & script
  - [ ] Add EditButton to HBox layout, align after checkbox.
  - [ ] Export NodePath for EditButton in script.
  - [ ] Define signal QuestRowEditRequested(int id).
  - [ ] Emit signal when EditButton pressed.
  - [ ] Ensure existing checkbox completion logic remains intact.

- [ ] Modify DailyQuestUi scene & script
  - [ ] Remove EditQuestsButton node and related code paths.
  - [ ] During quest list population connect each row’s QuestRowEditRequested to handler OnRowEditRequested.
  - [ ] Implement OnRowEditRequested(id): instantiate QuestEditPanel scene, pass quest id, add as child of root.
  - [ ] Disconnect obsolete QuestEditor signal logic.

- [ ] Retire legacy QuestEditor assets
  - [ ] Delete quest_editor.tscn and QuestEditor.cs.
  - [ ] Remove reference from Paths and any remaining usages.

- [ ] Enhance QuestManager (optional semantic aliases)
  - [ ] Add EditQuest(id,title,description) wrapper around Edit for clearer API.
  - [ ] Add DeleteQuest(id) wrapper around Remove.

- [ ] Strengthen Quest id generation
  - [ ] On QuestManager.LoadQuests compute max existing id, set Quest._id = maxId + 1 to prevent collisions across sessions.

- [ ] Unit tests
  - [ ] Add test verifying Quest id collision fix.
  - [ ] Add tests for EditQuest / DeleteQuest wrappers invoking original functionality.
  - [ ] Add UI integration test: toggling edit mode updates QuestManager.

- [ ] UI/UX polish
  - [ ] Import pencil icon for edit button sized 24×24.
  - [ ] Disable Save button until both input fields are non-empty.
  - [ ] Support Escape key to close QuestEditPanel.

- [ ] Regression testing
  - [ ] Manual test completing, editing, deleting quests persists correctly after app restart.
  - [ ] Verify EXP gain/loss remains consistent.

- [ ] Documentation & cleanup
  - [ ] Update README user guide screenshots.
  - [ ] Remove obsolete quest editor references from docs.
