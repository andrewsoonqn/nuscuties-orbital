using Godot;
using System;
using System.Linq;

public partial class QuestEditPanel : Control
{
    [Export] private Label _titleLabel;
    [Export] private Label _descriptionLabel;
    [Export] private LineEdit _titleEdit;
    [Export] private LineEdit _descriptionEdit;
    [Export] private Button _editButton;
    [Export] private Button _saveButton;
    [Export] private Button _deleteButton;
    [Export] private Button _doneButton;

    private Quest _quest;
    private QuestManager _questManager;
    private QuestLogManager _questLogManager;
    private bool _isEditMode = false;

    public override void _Ready()
    {
        _questManager = GetNode<QuestManager>("/root/QuestManager");
        _questLogManager = GetNode<QuestLogManager>("/root/QuestLogManager");
        
        _editButton.Pressed += OnEditPressed;
        _saveButton.Pressed += OnSavePressed;
        _deleteButton.Pressed += OnDeletePressed;
        _doneButton.Pressed += OnDonePressed;

        _titleEdit.TextChanged += OnTextChanged;
        _descriptionEdit.TextChanged += OnTextChanged;

    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            if (keyEvent.Keycode == Key.Escape)
            {
                OnDonePressed();
            }
        }
    }

    public void Initialize(int questId)
    {
        _quest = _questManager.Get(questId);
        RefreshUI();
        SetViewMode();
    }

    private void RefreshUI()
    {
        _titleLabel.Text = _quest.Title;
        _descriptionLabel.Text = _quest.Description;
        _titleEdit.Text = _quest.Title;
        _descriptionEdit.Text = _quest.Description;
    }

    private void SetViewMode()
    {
        _isEditMode = false;

        _titleLabel.Visible = true;
        _descriptionLabel.Visible = true;
        _titleEdit.Visible = false;
        _descriptionEdit.Visible = false;

        _editButton.Visible = true;
        _saveButton.Visible = false;
    }

    private void SetEditMode()
    {
        _isEditMode = true;

        _titleLabel.Visible = false;
        _descriptionLabel.Visible = false;
        _titleEdit.Visible = true;
        _descriptionEdit.Visible = true;

        _editButton.Visible = false;
        _saveButton.Visible = true;

        UpdateSaveButtonState();
        _titleEdit.GrabFocus();
    }

    private void OnEditPressed()
    {
        SetEditMode();
    }

    private void OnSavePressed()
    {
        if (ValidateInputs())
        {
            _questManager.Edit(_quest.Id, _titleEdit.Text.Trim(), _descriptionEdit.Text.Trim());
            SaveQuests();
            RefreshUI();
            SetViewMode();
        }
    }

    private void OnDeletePressed()
    {
        _questManager.Remove(_quest.Id);
        SaveQuests();
        QueueFree();
    }

    private void OnDonePressed()
    {
        QueueFree();
    }

    private bool ValidateInputs()
    {
        return !string.IsNullOrWhiteSpace(_titleEdit.Text) &&
               !string.IsNullOrWhiteSpace(_descriptionEdit.Text);
    }

    private void SaveQuests()
    {
        _questLogManager.SaveQuestLog(_questManager.GetQuests().Values.ToList());
    }

    private void OnTextChanged(string newText)
    {
        if (_isEditMode)
        {
            UpdateSaveButtonState();
        }
    }

    private void UpdateSaveButtonState()
    {
        _saveButton.Disabled = !ValidateInputs();
    }
}
