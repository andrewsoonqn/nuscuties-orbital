using Godot;

public partial class UserCard : Control
{
    [Export] private Label _usernameLabel;
    [Export] private Button _selectButton;
    [Export] private Button _editButton;
    [Export] private Button _deleteButton;

    [Signal]
    public delegate void UserSelectedEventHandler(string username);
    [Signal]
    public delegate void UserEditRequestedEventHandler(string username);
    [Signal]
    public delegate void UserDeleteRequestedEventHandler(string username);

    private string _username;

    public override void _Ready()
    {
        _selectButton.Pressed += OnSelectButtonPressed;
        _editButton.Pressed += OnEditButtonPressed;
        _deleteButton.Pressed += OnDeleteButtonPressed;
    }

    public void Initialize(string username)
    {
        _username = username;
        _usernameLabel.Text = username;
    }

    private void OnSelectButtonPressed()
    {
        EmitSignal(SignalName.UserSelected, _username);
    }

    private void OnEditButtonPressed()
    {
        EmitSignal(SignalName.UserEditRequested, _username);
    }

    private void OnDeleteButtonPressed()
    {
        EmitSignal(SignalName.UserDeleteRequested, _username);
    }
}