using Godot;
using System;
using System.Collections.Generic;

public partial class DailyQuestUi : Control
{
    [Export]
    private Button _editQuestsButton;

    [Export]
    private VBoxContainer _questList;
    
    [Export]
    private Button _backToHomeButton;
    
    private PackedScene _questEditor = ResourceLoader.Load<PackedScene>("res://daily/quest_editor.tscn");

    private Godot.Collections.Dictionary<int, CompletableQuestComponent> _completableQuestComponents = new Godot.Collections.Dictionary<int, CompletableQuestComponent>();
    
    private QuestManager _questManager;
    
    public override void _Ready()
    {
        LoadQuests();
        
        _editQuestsButton.Pressed += OnEditQuestsButtonPressed;
        _backToHomeButton.Pressed += OnBackToHomeButtonPressed;
        _questManager = this.GetNode<QuestManager>("/root/QuestManager");
        
        this.ConnectSignals();
    }

    private void OnEditQuestsButtonPressed()
    {
        Node questEditorInstance = _questEditor.Instantiate();
        GetTree().GetRoot().AddChild(questEditorInstance);
    }
    
    private void OnManagerQuestAdded(int id)
    {
        CompletableQuestComponent newComp = (CompletableQuestComponent)ResourceLoader.Load<PackedScene>("res://daily/components/completable_quest.tscn").Instantiate<HBoxContainer>();
        newComp.Initialize(_questManager.Get(id));
        this._completableQuestComponents[id] = newComp;
        
        this._questList.AddChild(newComp);
    }
    
    private void OnManagerQuestEdited(int id)
    {
        CompletableQuestComponent edited = this._completableQuestComponents.GetValueOrDefault(id);
        edited.Update(
            _questManager.Get(id).Title, 
            _questManager.Get(id).Description, 
            _questManager.Get(id).Completed);
    }
    
    private void OnManagerQuestRemoved(int id)
    {
        CompletableQuestComponent toRemove = this._completableQuestComponents.GetValueOrDefault(id);
        this.RemoveChild(toRemove);
        this._completableQuestComponents.Remove(id);
    }

    private void OnBackToHomeButtonPressed()
    {
        DisconnectSignals();
        GetTree().ChangeSceneToFile("res://shared/home.tscn");
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

    private void LoadQuests()
    {
        List<Quest> quests = new QuestLogManager().LoadQuestLog();
        foreach (Quest quest in quests)
        {
            CompletableQuestComponent newComp = (CompletableQuestComponent)ResourceLoader
                .Load<PackedScene>("res://daily/components/completable_quest.tscn").Instantiate<HBoxContainer>();
            newComp.Initialize(quest);
            this._completableQuestComponents[quest.Id] = newComp;
            this._questList.AddChild(newComp);
        }
    }
}
