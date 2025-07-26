using Godot;
using System;

public partial class UserAddPanel : Control
{
    [Export] private LineEdit _usernameEdit;
    [Export] private Button _createButton;
    [Export] private Button _cancelButton;
    [Export] private Label _headerLabel;
    [Export] private Label _errorLabel;

    [Signal]
    public delegate void UserCreatedEventHandler(string username);

    private UserManager _userManager;

    public override void _Ready()
    {
        _userManager = GetNode<UserManager>("/root/UserManager");

        _createButton.Pressed += OnCreatePressed;
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

    private void OnUsernameSubmitted(string text)
    {
        OnCreatePressed();
    }

    private void OnCreatePressed()
    {
        string username = _usernameEdit.Text.Trim();

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

        if (username.Length > 15)
        {
            ShowError("Username must be 15 characters or less");
            return;
        }

        try
        {
            _userManager.CreateNewUser(username);
            EmitSignal(SignalName.UserCreated, username);
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
