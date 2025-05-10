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
    
    private Dictionary<
        int, 
        EditableQuestComponent> _editableQuestComponents = 
        new Dictionary<int, EditableQuestComponent>();

    private QuestManager _questManager;

    public override void _Ready()
    {
        _goBackButton.Pressed += QueueFree;

        _addQuestButton.Pressed += OnAddQuestButtonPressed;

        _questManager = this.GetNode<QuestManager>("/root/QuestManager");

        // TODO
        _questManager.ManagerQuestAdded += OnManagerQuestAdded;
        // _questManager.ManagerQuestEdited += OnManagerQuestEdited;
        // _questManager.ManagerQuestRemoved += OnManagerQuestRemoved;
    }

    private void OnAddQuestButtonPressed()
    {
        _questManager.Submit(_titleInput.Text, _descriptionInput.Text);
        _titleInput.Clear();
        _descriptionInput.Clear();
    }
    
    private void OnManagerQuestAdded(int id)
    {
        EditableQuestComponent newComp = (EditableQuestComponent)ResourceLoader.Load<PackedScene>("res://daily/components/editable_quest.tscn").Instantiate<HBoxContainer>();
        newComp.Initialize(_questManager.Get(id));
        this._editableQuestComponents.Add(id, newComp);
        
        this._questList.AddChild(newComp);
    }
    
    private void OnManagerQuestEdited(int id)
    {
        EditableQuestComponent edited = this._editableQuestComponents.GetValueOrDefault(id);
        edited.Update(
            _questManager.Get(id).Title, 
            _questManager.Get(id).Description);
    }
    
    private void OnManagerQuestRemoved(int id)
    {
        EditableQuestComponent toRemove = this._editableQuestComponents.GetValueOrDefault(id);
        this.RemoveChild(toRemove);
        this._editableQuestComponents.Remove(id);
    }
    
    private void ConnectSignals()
    {
        _questManager.ManagerQuestAdded += OnManagerQuestAdded;
        _questManager.ManagerQuestEdited += OnManagerQuestEdited;
        _questManager.ManagerQuestRemoved += OnManagerQuestRemoved;
    }
    
    private void DisconnectSignals()
    {
        _questManager.ManagerQuestAdded -= OnManagerQuestAdded;
        _questManager.ManagerQuestEdited -= OnManagerQuestEdited;
        _questManager.ManagerQuestRemoved -= OnManagerQuestRemoved;
    }
}
