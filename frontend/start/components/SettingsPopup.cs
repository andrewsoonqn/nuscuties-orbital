using Godot;
using nuscutiesapp.tools;

public partial class SettingsPopup : Control
{
    [Export] private VBoxContainer _settingsContainer;

    [Signal]
    public delegate void LogoutRequestedEventHandler();

    private LogoutButton _logoutButtonInstance;
    public override void _Ready()
    {
        base._Ready();
        var profileComponent = (ProfileCustomization)_settingsContainer.FindChild("ProfileCustomization");
        profileComponent.LogoutRequested += OnLogoutRequested;
    }

    public void OnLogoutRequested()
    {
        EmitSignal(SignalName.LogoutRequested);
        Hide();
    }
}
