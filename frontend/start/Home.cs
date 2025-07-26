using Godot;
using nuscutiesapp.tools;
using System;
using System.Linq;

public partial class Home : Control
{
    [Export] private Label _expLabel;
    [Export] private Label _levelLabel;
    [Export] private ProgressBar _expProgressBar;
    [Export] private Button _dailyButton;
    [Export] private Button _passiveButton;
    [Export] private Button _activeButton;
    [Export] private Button _statsUIButton;
    [Export] private Button _storeButton;
    [Export] private Button _settingsButton;

    private ProgressionManager _expManager;

    private PackedScene _statsUINode;
    private PackedScene _settingsPopupScene;
    private SettingsPopup _settingsPopupInstance;
    public override void _Ready()
    {
        var userManager = GetNode<UserManager>("/root/UserManager");
        if (string.IsNullOrEmpty(userManager.GetCurrentUser()))
        {
            GetTree().ChangeSceneToFile(Paths.UserSelection);
            return;
        }

        _expManager = GetNode<ProgressionManager>("/root/ProgressionManager");
        int exp = _expManager.GetExp();
        int level = _expManager.GetLevel();
        int totalExpNeeded = ProgressionManager.GetTotalExpRequiredForLevel(level + 1);
        _levelLabel.Text = $"Level {level}";
        _expLabel.Text = $"EXP: {exp} / {totalExpNeeded}";
        _expProgressBar.MinValue = 0;
        _expProgressBar.MaxValue = totalExpNeeded;
        _expProgressBar.Value = exp;

        _dailyButton.Pressed += () => switchScene(Paths.Daily);
        _passiveButton.Pressed += () => switchScene(Paths.Passive);
        _activeButton.Pressed += () => switchScene(Paths.Active);
        _storeButton.Pressed += () => switchScene(Paths.Shop);
        _settingsButton.Pressed += OnSettingsButtonPressed;

        _statsUINode = ResourceLoader.Load<PackedScene>(Paths.StatsUI);

        _statsUIButton.Pressed += StatsUIButtonOnPressed;

        _settingsPopupScene = ResourceLoader.Load<PackedScene>(Paths.SettingsPopup);
    }

    private void StatsUIButtonOnPressed()
    {
        if (!GetTree().Root.HasNode("StatUserInterface"))
        {
            GetTree().Root.AddChild(_statsUINode.Instantiate());
        }
    }

    private void OnSettingsButtonPressed()
    {
        if (_settingsPopupInstance == null)
        {
            _settingsPopupInstance = _settingsPopupScene.Instantiate<SettingsPopup>();
            _settingsPopupInstance.LogoutRequested += OnLogoutRequested;
            GetTree().Root.AddChild(_settingsPopupInstance);
        }
        else
        {
            _settingsPopupInstance.Show();
        }
    }

    private void OnLogoutRequested()
    {
        var userManager = GetNode<UserManager>("/root/UserManager");
        var progressionManager = GetNode<ProgressionManager>("/root/ProgressionManager");
        var playerStatManager = GetNode<PlayerStatManager>("/root/PlayerStatManager");
        var inventoryManager = GetNode<PlayerInventoryManager>("/root/PlayerInventoryManager");
        var questLogManager = GetNode<QuestLogManager>("/root/QuestLogManager");

        progressionManager?.NotifyDataChanged();
        playerStatManager?.NotifyDataChanged();
        inventoryManager?.NotifyDataChanged();
        if (questLogManager != null)
        {
            var questManager = GetNode<QuestManager>("/root/QuestManager");
            questLogManager.SaveQuestLog(questManager.GetQuests().Values.ToList());
        }

        userManager.SetCurrentUser("");
        GetTree().ChangeSceneToFile(Paths.UserSelection);
    }

    public void switchScene(string scenePath)
    {
        GetTree().ChangeSceneToFile(scenePath);
    }
}