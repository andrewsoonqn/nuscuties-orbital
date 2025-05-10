using Godot;
using System;
using System.Collections.Generic;

public partial class DailyQuestUi : Control
{
    [Export]
    private Button _editQuestsButton;

    [Export]
    private VBoxContainer _questList;
    
    private PackedScene QuestEditor = ResourceLoader.Load<PackedScene>("res://daily/quest_editor.tscn");

    private Godot.Collections.Dictionary<int, CompletableQuestComponent> CompletableQuestComponents = new Godot.Collections.Dictionary<int, CompletableQuestComponent>();
    
    private QuestManager _questManager;
    
    public override void _Ready()
    {
        _editQuestsButton.Pressed += OnEditQuestsButtonPressed;
        _questManager = this.GetNode<QuestManager>("/root/QuestManager");
        
        _questManager.ManagerQuestAdded += OnManagerQuestAdded;
        _questManager.ManagerQuestEdited += OnManagerQuestEdited;
        _questManager.ManagerQuestRemoved += OnManagerQuestRemoved;
    }

    private void OnEditQuestsButtonPressed()
    {
        Node QuestEditorInstance = QuestEditor.Instantiate();
        GetTree().GetRoot().AddChild(QuestEditorInstance);
    }
    
    private void OnManagerQuestAdded(int id)
    {
        GD.Print("OnManagerQuestAdded");
        CompletableQuestComponent newComp = (CompletableQuestComponent)ResourceLoader.Load<PackedScene>("res://daily/components/completable_quest.tscn").Instantiate<HBoxContainer>();
        newComp.Initialize(_questManager.Get(id));
        this.CompletableQuestComponents.Add(id, newComp);
        this._questList.AddChild(newComp);
    }
    
    private void OnManagerQuestEdited(int id)
    {
        CompletableQuestComponent edited = this.CompletableQuestComponents.GetValueOrDefault(id);
        edited.Update(
            _questManager.Get(id).Title, 
            _questManager.Get(id).Description, 
            _questManager.Get(id).Completed);
    }
    
    private void OnManagerQuestRemoved(int id)
    {
        CompletableQuestComponent toRemove = this.CompletableQuestComponents.GetValueOrDefault(id);
        this.RemoveChild(toRemove);
        this.CompletableQuestComponents.Remove(id);
    }
}
