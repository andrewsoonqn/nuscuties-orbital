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
	[Export] private Button _deleteButton;
	[Export] private Button _doneButton;

	[Export] private Button _okButton;
	[Export] private Button _cancelButton;

	[Export] private Label _headerLabel;

	private Quest _quest;
	private QuestManager _questManager;
	private QuestLogManager _questLogManager;
	private bool _isEditMode = false;
	private bool _isNewQuest = false;
	private string _originalTitle;
	private string _originalDescription;

	public override void _Ready()
	{
		_questManager = GetNode<QuestManager>("/root/QuestManager");
		_questLogManager = GetNode<QuestLogManager>("/root/QuestLogManager");
		_headerLabel = GetNode<Label>("CenterContainer/PanelContainer/MarginContainer/VBoxContainer/HeaderLabel");

		_editButton.Pressed += OnEditPressed;
		_okButton.Pressed += OnOkPressed;
		_cancelButton.Pressed += OnCancelPressed;
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
				if (_isEditMode)
				{
					OnCancelPressed();
				}
				else
				{
					OnDonePressed();
				}
			}
		}
	}

	public void Initialize(int questId)
	{
		_quest = _questManager.Get(questId);
		_headerLabel.Text = "Quest Details";
		RefreshUI();
		SetViewMode();
	}

	public void InitializeForNewQuest()
	{
		if (_questManager == null)
		{
			_questManager = GetNode<QuestManager>("/root/QuestManager");
		}
		if (_questLogManager == null)
		{
			_questLogManager = GetNode<QuestLogManager>("/root/QuestLogManager");
		}
		if (_headerLabel == null)
		{
			_headerLabel = GetNode<Label>("CenterContainer/PanelContainer/MarginContainer/VBoxContainer/HeaderLabel");
		}

		_isNewQuest = true;
		_quest = new Quest("", "");
		_headerLabel.Text = "Add New Quest";

		RefreshUI();
		SetEditMode();
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
		_deleteButton.Visible = true;
		_doneButton.Visible = true;
		_okButton.Visible = false;
		_cancelButton.Visible = false;
	}

	private void SetEditMode()
	{
		_isEditMode = true;

		_originalTitle = _quest.Title;
		_originalDescription = _quest.Description;

		_titleLabel.Visible = false;
		_descriptionLabel.Visible = false;
		_titleEdit.Visible = true;
		_descriptionEdit.Visible = true;

		_editButton.Visible = false;
		_deleteButton.Visible = false;
		_doneButton.Visible = false;
		_okButton.Visible = true;
		_cancelButton.Visible = true;

		UpdateOkButtonState();
		_titleEdit.GrabFocus();
	}

	private void OnEditPressed()
	{
		SetEditMode();
	}

	private void OnOkPressed()
	{
		if (ValidateInputs())
		{
			if (_isNewQuest)
			{
				_questManager.Submit(_titleEdit.Text.Trim(), _descriptionEdit.Text.Trim());
				SaveQuests();
				QueueFree();
			}
			else
			{
				_questManager.Edit(_quest.Id, _titleEdit.Text.Trim(), _descriptionEdit.Text.Trim());
				SaveQuests();
				RefreshUI();
				SetViewMode();
			}
		}
	}

	private void OnCancelPressed()
	{
		if (_isNewQuest)
		{
			QueueFree();
		}
		else
		{
			_titleEdit.Text = _originalTitle;
			_descriptionEdit.Text = _originalDescription;
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
			UpdateOkButtonState();
		}
	}

	private void UpdateOkButtonState()
	{
		_okButton.Disabled = !ValidateInputs();
	}
}
