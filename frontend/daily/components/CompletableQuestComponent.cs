using Godot;
using System;
using System.Linq;

public partial class CompletableQuestComponent : HBoxContainer
{
    [Export] private Label _title;
    [Export] private Label _description;
    [Export] private CheckBox _checkbox;

    private Quest _quest; // TODO: only id needed, quest manager takes care of the quest storing
    private QuestManager _questManager;
    private StatsManager _statsManager;

    public override void _Ready()
    {
        _questManager = GetNode<QuestManager>("/root/QuestManager");
        _statsManager = GetNode<StatsManager>("/root/StatsManager");
        _checkbox.Toggled += CheckboxOnToggled;
    }

    private void CheckboxOnToggled(bool toggledOn)
    {
        _questManager.ToggleCompletion(_quest.Id);
        if (toggledOn)
        {
            _statsManager.AddExp(100);
        }
        else
        {
            _statsManager.DecrExp(100);
        }
        new QuestLogManager().SaveQuestLog(_questManager.GetQuests().Values.ToList());
    }

    public void Initialize(Quest quest)
    {
        _quest = quest;

        _title.Text = quest.Title;
        _description.Text = quest.Description;
        _checkbox.ButtonPressed = quest.Completed;
    }

    public void Update(string title, string description, bool completed)
    {
        this._quest.Title = title;
        this._quest.Description = description;
        this._quest.Completed = completed;
        this._title.Text = title;
        this._description.Text = description;
    }

    public Quest Get()
    {
        return _quest;
    }
}