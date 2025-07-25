using Godot;
using System;

public partial class UserDeletePanel : Control
{
    [Export] private Label _messageLabel;
    [Export] private Button _okButton;
    [Export] private Button _cancelButton;
    [Export] private Label _headerLabel;

    [Signal]
    public delegate void UserDeleteConfirmedEventHandler(string username);

    private UserManager _userManager;
    private string _username;

    public override void _Ready()
    {
        _userManager = GetNode<UserManager>("/root/UserManager");

        _okButton.Pressed += OnOkPressed;
        _cancelButton.Pressed += OnCancelPressed;
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

    public void Initialize(string username)
    {
        _username = username;
        _headerLabel.Text = "Delete User";
        _messageLabel.Text = $"Are you sure you want to delete user '{username}'?\n\nThis will permanently delete all save data for this user.\n\nThis action cannot be undone.";
    }

    private void OnOkPressed()
    {
        try
        {
            _userManager.DeleteUser(_username);
            EmitSignal(SignalName.UserDeleteConfirmed, _username);
            QueueFree();
        }
        catch (System.ArgumentException ex)
        {
            GD.PrintErr($"Error deleting user: {ex.Message}");
            QueueFree();
        }
    }

    private void OnCancelPressed()
    {
        QueueFree();
    }
}
