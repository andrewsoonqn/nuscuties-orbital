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

    private PackedScene _questEditPanel = ResourceLoader.Load<PackedScene>(Paths.QuestEditPanel);

    private Godot.Collections.Dictionary<int, CompletableQuestComponent> _completableQuestComponents = new Godot.Collections.Dictionary<int, CompletableQuestComponent>();

    private QuestManager _questManager;

    public override void _Ready()
    {
        LoadQuests();

        _backToHomeButton.Pressed += OnBackToHomeButtonPressed;
        _questManager = this.GetNode<QuestManager>("/root/QuestManager");

        this.ConnectSignals();
    }

    private void OnRowEditRequested(int id)
    {
        QuestEditPanel questEditPanelInstance = (QuestEditPanel)_questEditPanel.Instantiate();
        GetTree().GetRoot().AddChild(questEditPanelInstance);
        questEditPanelInstance.Initialize(id);
    }

    private void OnManagerQuestAdded(int id)
    {
        CompletableQuestComponent newComp = (CompletableQuestComponent)ResourceLoader.Load<PackedScene>(Paths.CompletableQuestComponent).Instantiate<HBoxContainer>();
        newComp.Initialize(_questManager.Get(id));
        newComp.QuestRowEditRequested += OnRowEditRequested;
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
        // this.RemoveChild(toRemove);
        this._completableQuestComponents.Remove(id);
    }

    private void OnBackToHomeButtonPressed()
    {
        DisconnectSignals();
        GetTree().ChangeSceneToFile(Paths.Home);
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
                .Load<PackedScene>(Paths.CompletableQuestComponent).Instantiate<HBoxContainer>();
            newComp.Initialize(quest);
            newComp.QuestRowEditRequested += OnRowEditRequested;
            this._completableQuestComponents[quest.Id] = newComp;
            this._questList.AddChild(newComp);
        }
    }
}
