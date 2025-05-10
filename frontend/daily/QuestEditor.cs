using Godot;
using System;
using System.Collections.Generic;

public partial class QuestEditor : Control
{
    [Export]
    private LineEdit _titleInput;

    [Export]
    private LineEdit _descriptionInput;

    [Export]
    private Button _addQuestButton;

    [Export]
    private VBoxContainer _questList;

    [Export]
    private Button _goBackButton;

    private QuestManager _questManager;

    public override void _Ready()
    {
        _goBackButton.Pressed += QueueFree;

        _addQuestButton.Pressed += OnAddQuestButtonPressed;

        _questManager = this.GetNode<QuestManager>("/root/QuestManager");

        // TODO
        // _questManager.ManagerQuestAdded += OnManagerQuestAdded;
        // _questManager.ManagerQuestEdited += OnManagerQuestEdited;
        // _questManager.ManagerQuestRemoved += OnManagerQuestRemoved;
    }

    private void OnAddQuestButtonPressed()
    {
        _questManager.Submit(_titleInput.Text, _descriptionInput.Text);
        _titleInput.Clear();
        _descriptionInput.Clear();
    }
}
