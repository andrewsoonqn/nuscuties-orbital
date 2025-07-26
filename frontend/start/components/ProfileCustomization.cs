using Godot;
using nuscutiesapp.tools;

public partial class ProfileCustomization : Control
{
    [Export] private Label _currentUserLabel;
    [Export] private Button _changeUserButton;
    [Export] private Button _editProfileButton;

    private UserManager _userManager;

    public override void _Ready()
    {
        _userManager = GetNode<UserManager>("/root/UserManager");

        ConnectSignals();
        UpdateDisplay();
    }

    private void ConnectSignals()
    {
        _changeUserButton.Pressed += OnChangeUserPressed;
        _editProfileButton.Pressed += OnEditProfilePressed;
    }

    private void UpdateDisplay()
    {
        if (_userManager != null)
        {
            string currentUser = _userManager.GetCurrentUser();
            _currentUserLabel.Text = $"Current User: {currentUser}";
        }
    }

    [Signal]
    public delegate void LogoutRequestedEventHandler();
    private void OnChangeUserPressed()
    { 
        EmitSignalLogoutRequested();
    }

    private void OnEditProfilePressed()
    {
        GD.Print("Edit profile functionality - to be implemented");
    }
}
