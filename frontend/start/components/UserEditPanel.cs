using Godot;
using System;

public partial class UserEditPanel : Control
{
    [Export] private LineEdit _usernameEdit;
    [Export] private Button _confirmButton;
    [Export] private Button _cancelButton;
    [Export] private Label _headerLabel;
    [Export] private Label _errorLabel;

    [Signal]
    public delegate void UserEditedEventHandler(string oldUsername, string newUsername);

    private UserManager _userManager;
    private string _oldUsername;

    public override void _Ready()
    {
        _userManager = GetNode<UserManager>("/root/UserManager");

        _confirmButton.Pressed += OnConfirmPressed;
        _cancelButton.Pressed += OnCancelPressed;
        _usernameEdit.TextSubmitted += OnUsernameSubmitted;
        _usernameEdit.TextChanged += OnTextChanged;

        _usernameEdit.GrabFocus();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            if (keyEvent.Keycode == Key.Escape)
            {
                OnCancelPressed();
            }
        }
    }

    public void Initialize(string oldUsername)
    {
        _oldUsername = oldUsername;
        _usernameEdit.Text = oldUsername;
        _headerLabel.Text = $"Edit User";
        _errorLabel.Text = "";
    }

    private void OnUsernameSubmitted(string text)
    {
        OnConfirmPressed();
    }

    private void OnConfirmPressed()
    {
        string newUsername = _usernameEdit.Text.Trim();

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

        if (newUsername.Length > 15)
        {
            ShowError("Username must be 15 characters or less");
            return;
        }

        if (newUsername == _oldUsername)
        {
            QueueFree();
            return;
        }

        try
        {
            _userManager.RenameUser(_oldUsername, newUsername);
            EmitSignal(SignalName.UserEdited, _oldUsername, newUsername);
            QueueFree();
        }
        catch (System.ArgumentException ex)
        {
            ShowError(ex.Message);
        }
    }

    private void OnCancelPressed()
    {
        QueueFree();
    }

    private void OnTextChanged(string newText)
    {
        _errorLabel.Text = "";
    }

    private void ShowError(string message)
    {
        _errorLabel.Text = message;
        _errorLabel.Modulate = Colors.Red;
    }
}
