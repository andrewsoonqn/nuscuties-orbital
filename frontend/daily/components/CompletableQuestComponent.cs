using Godot;
using nuscutiesapp.tools;
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

    [Signal]
    public delegate void QuestCompletedEventHandler(int coinReward, Vector2 position);

    private Quest _quest; // TODO: only id needed, quest manager takes care of the quest storing
    private QuestManager _questManager;
    private ProgressionManager _expManager;
    private PlayerInventoryManager _inventoryManager;
    [Export] private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        _questManager = GetNode<QuestManager>("/root/QuestManager");
        _expManager = GetNode<ProgressionManager>("/root/ProgressionManager");
        _inventoryManager = GetNode<PlayerInventoryManager>("/root/PlayerInventoryManager");

        _checkbox.Toggled += CheckboxOnToggled;
        _editButton.Pressed += OnEditButtonPressed;

        // Connect animation finished signal
        _animationPlayer.AnimationFinished += OnAnimationFinished;
    }

    private void CheckboxOnToggled(bool toggledOn)
    {
        _questManager.ToggleCompletion(_quest.Id);
        if (toggledOn)
        {
            _expManager.AddExp(100);
            int coinReward = _inventoryManager.CalculateDailyQuestCoinReward();
            _inventoryManager.AddCoins(coinReward);

            _animationPlayer.Play("fade_out");

            GD.Print($"Daily quest completed! Gained {coinReward} coins");

            EmitSignal(SignalName.QuestCompleted, coinReward, _checkbox.GlobalPosition);
        }
        else
        {
            _expManager.AddExp(-100);
            int coinPenalty = _inventoryManager.CalculateDailyQuestCoinReward();
            bool success = _inventoryManager.SpendCoins(coinPenalty);
            if (success)
            {
                GD.Print($"Daily quest uncompleted! Lost {coinPenalty} coins");
            }
            else
            {
                GD.Print($"Daily quest uncompleted! Not enough coins to deduct {coinPenalty}");
            }
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

    private void OnFadeOutFinished()
    {
        _questManager.Remove(_quest.Id);
    }

    private void OnAnimationFinished(StringName animName)
    {
        if (animName == "fade_out")
        {
            OnFadeOutFinished();
        }
    }
}
