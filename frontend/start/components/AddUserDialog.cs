using Godot;

public partial class AddUserDialog : AcceptDialog
{
    [Export] private LineEdit _usernameInput;
    [Export] private Button _createButton;
    [Export] private Label _errorLabel;

    [Signal]
    public delegate void UserCreatedEventHandler(string username);

    private UserManager _userManager;

    public override void _Ready()
    {
        base._Ready();
        _userManager = GetNode<UserManager>("/root/UserManager");

        _createButton.Pressed += OnCreateButtonPressed;
        _usernameInput.TextSubmitted += OnUsernameSubmitted;

        AboutToPopup += OnAboutToPopup;

        Title = "Create New User";
    }

    private void OnAboutToPopup()
    {
        _usernameInput.Text = "";
        _errorLabel.Text = "";
        _usernameInput.GrabFocus();
    }

    private void OnUsernameSubmitted(string text)
    {
        OnCreateButtonPressed();
    }

    private void OnCreateButtonPressed()
    {
        string username = _usernameInput.Text.Trim();

        if (string.IsNullOrWhiteSpace(username))
        {
            ShowError("Username cannot be empty");
            return;
        }

        if (username.Length < 3)
        {
            ShowError("Username must be at least 3 characters");
            return;
        }

        if (username.Length > 20)
        {
            ShowError("Username must be 20 characters or less");
            return;
        }

        try
        {
            _userManager.CreateNewUser(username);
            EmitSignal(SignalName.UserCreated, username);
            Hide();
        }
        catch (System.ArgumentException ex)
        {
            ShowError(ex.Message);
        }
    }

    private void ShowError(string message)
    {
        _errorLabel.Text = message;
        _errorLabel.Modulate = Colors.Red;
    }
}
