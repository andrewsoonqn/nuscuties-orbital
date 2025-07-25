using Godot;
using System;
using System.Linq;

public partial class CompletableQuestComponent : HBoxContainer
{
	[Export] private Label _title;
	[Export] private Label _description;
	[Export] private CheckBox _checkbox;
	[Export] private Button _editButton;

	[Signal]
	public delegate void QuestRowEditRequestedEventHandler(int id);

	private Quest _quest; // TODO: only id needed, quest manager takes care of the quest storing
	private QuestManager _questManager;
	private ProgressionManager _expManager;

	public override void _Ready()
	{
		_questManager = GetNode<QuestManager>("/root/QuestManager");
		_expManager = GetNode<ProgressionManager>("/root/ProgressionManager");
		_checkbox.Toggled += CheckboxOnToggled;
		_editButton.Pressed += OnEditButtonPressed;
	}

	private void CheckboxOnToggled(bool toggledOn)
	{
		_questManager.ToggleCompletion(_quest.Id);
		if (toggledOn)
		{
			_expManager.AddExp(100);
		}
		else
		{
			_expManager.AddExp(-100);
		}
		new QuestLogManager().SaveQuestLog(_questManager.GetQuests().Values.ToList());
	}

	private void OnEditButtonPressed()
	{
		EmitSignal(SignalName.QuestRowEditRequested, _quest.Id);
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
