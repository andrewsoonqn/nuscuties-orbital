using Godot;

public partial class EditUserDialog : AcceptDialog
{
    [Export] private LineEdit _usernameInput;
    [Export] private Button _confirmButton;
    [Export] private Label _errorLabel;

    [Signal]
    public delegate void UserEditedEventHandler(string newUsername);

    private string _oldUsername;

    public override void _Ready()
    {
        _confirmButton.Pressed += OnConfirmPressed;
        _usernameInput.TextSubmitted += OnUsernameSubmitted;
        AboutToPopup += OnAboutToPopup;
        Title = "Edit Username";
    }

    public void SetOldUsername(string oldUsername)
    {
        _oldUsername = oldUsername;
        _usernameInput.Text = oldUsername;
        _errorLabel.Text = "";
    }

    private void OnAboutToPopup()
    {
        _errorLabel.Text = "";
        _usernameInput.GrabFocus();
    }

    private void OnUsernameSubmitted(string text)
    {
        OnConfirmPressed();
    }

    private void OnConfirmPressed()
    {
        string newUsername = _usernameInput.Text.Trim();
        if (string.IsNullOrWhiteSpace(newUsername))
        {
            ShowError("Username cannot be empty");
            return;
        }
        if (newUsername.Length < 3)
        {
            ShowError("Username must be at least 3 characters");
            return;
        }
        if (newUsername.Length > 20)
        {
            ShowError("Username must be 20 characters or less");
            return;
        }
        if (newUsername == _oldUsername)
        {
            Hide();
            return;
        }
        EmitSignal(SignalName.UserEdited, newUsername);
        Hide();
    }

    public void ShowError(string message)
    {
        _errorLabel.Text = message;
        _errorLabel.Modulate = Colors.Red;
    }
}
