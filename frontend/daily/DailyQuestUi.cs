using Godot;
using System;

public partial class DailyQuestUi : Control
{
    [Export]
    Button _editQuestsButton;

    [Export]
    VBoxContainer _questList;
    
    PackedScene QuestEditor = ResourceLoader.Load<PackedScene>("res://daily/quest_editor.tscn");
    
    public override void _Ready()
    {
        _editQuestsButton.Pressed += OnEditQuestsButtonPressed;
    }

    private void OnEditQuestsButtonPressed()
    {
        Node QuestEditorInstance = QuestEditor.Instantiate();
        GetTree().GetRoot().AddChild(QuestEditorInstance);
    }
}
