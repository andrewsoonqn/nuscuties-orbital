using Godot;

public partial class Passive : Control
{
    [Export]
    private Button _backToHomeButton;
    
    [Export]
    private Button _addExpButton;
    
    [Export]
    private Button _resetExpButton;
    
    private StatsManager _statsManager;

    public override void _Ready()
    {
        _backToHomeButton.Pressed += BackToHomeButtonOnPressed;
        _addExpButton.Pressed += AddExpButtonOnPressed;
        _resetExpButton.Pressed += ResetExpButtonOnPressed;
        _statsManager = GetNode<StatsManager>("/root/StatsManager");
    }

    private void StartGameButtonOnPressed()
    {
        GetTree().ChangeSceneToFile("res://active/active_game.tscn");
    }

    private void AddExpButtonOnPressed()
    {
        _statsManager.AddExp(50);
    }

    private void ResetExpButtonOnPressed()
    {
        _statsManager.ResetExp();
    }
    
    private void BackToHomeButtonOnPressed()
    {
        GetTree().ChangeSceneToFile("res://shared/home.tscn");
    }
}