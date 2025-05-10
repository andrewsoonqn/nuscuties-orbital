using Godot;
using System;

public partial class CompletableQuestComponent : HBoxContainer
{
    [Export] private Label _title;
    [Export] private Label _description;
    [Export] private CheckBox _checkbox;
    
    private Quest _quest;

    public void Initialize(Quest quest)
    {
        _quest = quest;
        
        _title.Text = quest.Title;
        _description.Text = quest.Description;
        _checkbox.ToggleMode = quest.Completed;
    }

    public void Update(string title, string description, bool completed)
    {
        this._quest.Title = title;
        this._quest.Description = description;
        this._quest.Completed = completed;
    }

    public Quest Get()
    {
        return _quest;
    }
}
