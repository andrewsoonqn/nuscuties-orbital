using Godot;
using nuscutiesapp.tools;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class DailyQuestUi : Control
{
    [Export]
    private VBoxContainer _questList;

    [Export]
    private Button _backToHomeButton;

    [Export]
    private Button _addQuestButton;

    private PackedScene _questEditPanel = ResourceLoader.Load<PackedScene>(Paths.QuestEditPanel);

    private Godot.Collections.Dictionary<int, CompletableQuestComponent> _completableQuestComponents = new Godot.Collections.Dictionary<int, CompletableQuestComponent>();

    private QuestManager _questManager;
    private BaseNumberManager _baseNumberManager;
    private QuestLogManager _questLogManager;

    public override void _Ready()
    {
        _backToHomeButton.Pressed += OnBackToHomeButtonPressed;
        _addQuestButton.Pressed += OnAddQuestButtonPressed;
        _questManager = this.GetNode<QuestManager>("/root/QuestManager");
        _baseNumberManager = this.GetNode<BaseNumberManager>("/root/BaseNumberManager");
        _questLogManager = this.GetNode<QuestLogManager>("/root/QuestLogManager");
        LoadQuests();


        this.ConnectSignals();
    }

    private void OnRowEditRequested(int id)
    {
        QuestEditPanel questEditPanelInstance = (QuestEditPanel)_questEditPanel.Instantiate();
        GetTree().GetRoot().AddChild(questEditPanelInstance);
        questEditPanelInstance.Initialize(id);
    }

    private void OnQuestCompleted(int coinReward, Vector2 position)
    {
        _baseNumberManager.ShowCoinGain(coinReward, position, this, 6.0f);
    }

    private void OnManagerQuestAdded(int id)
    {
        CompletableQuestComponent newComp = (CompletableQuestComponent)ResourceLoader.Load<PackedScene>(Paths.CompletableQuestComponent).Instantiate<HBoxContainer>();
        newComp.Initialize(_questManager.Get(id));
        newComp.QuestRowEditRequested += OnRowEditRequested;
        newComp.QuestCompleted += OnQuestCompleted;
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
        toRemove.QueueFree();
        this.RemoveChild(toRemove);
        this._completableQuestComponents.Remove(id);
    }

    private void OnBackToHomeButtonPressed()
    {
        DisconnectSignals();
        GetTree().ChangeSceneToFile(Paths.Home);
    }

    private void OnAddQuestButtonPressed()
    {
        QuestEditPanel questEditPanelInstance = (QuestEditPanel)_questEditPanel.Instantiate();
        GetTree().GetRoot().AddChild(questEditPanelInstance);
        questEditPanelInstance.InitializeForNewQuest();
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
        foreach (Node child in _questList.GetChildren())
        {
            child.QueueFree();
        }
        _completableQuestComponents.Clear();
        foreach (var quest in _questManager.GetQuests().Values)
        {
            CompletableQuestComponent newComp = (CompletableQuestComponent)ResourceLoader
                .Load<PackedScene>(Paths.CompletableQuestComponent).Instantiate<HBoxContainer>();
            newComp.Initialize(quest);
            newComp.QuestRowEditRequested += OnRowEditRequested;
            newComp.QuestCompleted += OnQuestCompleted;
            this._completableQuestComponents[quest.Id] = newComp;
            this._questList.AddChild(newComp);
        }
    }
}