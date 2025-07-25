using Godot;
using System;
using System.Linq;

public partial class EditableQuestComponent : HBoxContainer
{
	[Export] private LineEdit _title;
	[Export] private LineEdit _description;
	[Export] private Button _saveButton;
	[Export] private Button _deleteButton;

	private Quest _quest;
	private QuestManager _questManager;

	public override void _Ready()
	{
		_saveButton.Pressed += SaveButtonOnPressed;
		_deleteButton.Pressed += DeleteButtonOnPressed;
		_questManager = this.GetNode<QuestManager>("/root/QuestManager");
	}

	private void DeleteButtonOnPressed()
	{
		_questManager.Remove(_quest.Id);
		new QuestLogManager().SaveQuestLog(_questManager.GetQuests().Values.ToList());
	}

	private void SaveButtonOnPressed()
	{
		// Update(_title.Text, _description.Text);
		_questManager.Edit(_quest.Id, _title.Text, _description.Text);
		new QuestLogManager().SaveQuestLog(_questManager.GetQuests().Values.ToList());
	}

	public void Initialize(Quest quest)
	{
		_quest = quest;

		_title.Text = quest.Title;
		_description.Text = quest.Description;
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
