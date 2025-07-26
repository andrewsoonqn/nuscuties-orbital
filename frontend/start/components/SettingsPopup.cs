using Godot;
using nuscutiesapp.tools;

public partial class SettingsPopup : Control
{
    [Export] private VBoxContainer _settingsContainer;
    [Export] private Button _backButton;
    private SettingsManager _settingsManager;

    [Signal]
    public delegate void LogoutRequestedEventHandler();

    private LogoutButton _logoutButtonInstance;

    public SettingsManager.SettingsData GetSettingsData()
    {
        var bgmVolume = GetNode<BgmManager>("/root/BgmManager").GetVolume();
        var sfxVolume = GetNode<AudioManager>("/root/AudioManager").GetVolume();
        return new SettingsManager.SettingsData()
        {
            BgmVolume = bgmVolume,
            SfxVolume = sfxVolume
        };
    }

    public override void _Ready()
    {
        base._Ready();
        _settingsManager = GetNode<SettingsManager>("/root/SettingsManager");
        var profileComponent = (ProfileCustomization)_settingsContainer.FindChild("ProfileCustomization");
        profileComponent.LogoutRequested += OnLogoutRequested;
        _backButton.Pressed += BackButtonOnPressed;
    }

    private void BackButtonOnPressed()
    {
        _settingsManager.SaveSettings(GetSettingsData());
        Hide();
    }

    public void OnLogoutRequested()
    {
        _settingsManager.SaveSettings(GetSettingsData());
        EmitSignal(SignalName.LogoutRequested);
        Hide();
    }
}