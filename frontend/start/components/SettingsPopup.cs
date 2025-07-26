using Godot;
using nuscutiesapp.tools;

public partial class SettingsPopup : Control
{
    [Export] private VBoxContainer _settingsContainer;
    [Export] private Button _backButton;

    [Signal]
    public delegate void LogoutRequestedEventHandler();

    private LogoutButton _logoutButtonInstance;
    public override void _Ready()
    {
        base._Ready();
        var profileComponent = (ProfileCustomization)_settingsContainer.FindChild("ProfileCustomization");
        profileComponent.LogoutRequested += OnLogoutRequested;
        _backButton.Pressed += BackButtonOnPressed;
    }

    private void BackButtonOnPressed()
    {
        Hide();
    }

    public void OnLogoutRequested()
    {
        EmitSignal(SignalName.LogoutRequested);
        Hide();
    }
}
