using Godot;

public partial class LogoutButton : Control
{
    [Export] private Button _logoutButton;

    [Signal]
    public delegate void LogoutRequestedEventHandler();

    public override void _Ready()
    {
        _logoutButton.Pressed += OnLogoutPressed;
    }

    private void OnLogoutPressed()
    {
        EmitSignal(SignalName.LogoutRequested);
    }
}
