using Godot;

public partial class Active : Control
{
    [Export]
    private Button _backToHomeButton;

    [Export] private Button _addExpButton;
    
    private StatsManager _statsManager;

    public override void _Ready()
    {
        _backToHomeButton.Pressed += BackToHomeButtonOnPressed;
        _addExpButton.Pressed += AddExpButtonOnPressed;
        _statsManager = GetNode<StatsManager>("/root/StatsManager");
    }

    private void AddExpButtonOnPressed()
    {
        _statsManager.AddExp(50);
    }

    private void BackToHomeButtonOnPressed()
    {
        GetTree().ChangeSceneToFile("res://shared/home.tscn");
    }
}