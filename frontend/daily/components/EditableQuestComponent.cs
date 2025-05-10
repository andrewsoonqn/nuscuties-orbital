using Godot;
using System;

public partial class EditableQuestComponent : HBoxContainer
{
    [Export] private LineEdit _title;
    [Export] private LineEdit _description;
    [Export] private Button _saveButton;
    [Export] private Button _deleteButton;
    
    private Quest _quest;

    public void Initialize(Quest quest)
    {
        _quest = quest;
        
        _title.Text = quest.Title;
        _description.Text = quest.Description; 
    }

    public void Update(string title, string description)
    {
        this._quest.Title = title;
        this._quest.Description = description;
    }

    public Quest Get()
    {
        return _quest;
    }
}
