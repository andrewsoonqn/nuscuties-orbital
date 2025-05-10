using Godot;
using System;

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
    
    private QuestManager _questManager = new QuestManager();

    public override void _Ready()
    {
        GD.Print("QuestEditor");
        _addQuestButton.Pressed += OnAddQuestButtonPressed;
    }

    private void OnAddQuestButtonPressed()
    {
        _questManager.Submit(_titleInput.Text, _descriptionInput.Text);
        _titleInput.Clear();
        _descriptionInput.Clear();
    }
}
