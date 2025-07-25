using Godot;

public partial class UserCard : Control
{
    [Export] private Label _usernameLabel;
    [Export] private Button _selectButton;

    [Signal]
    public delegate void UserSelectedEventHandler(string username);

    private string _username;

    public override void _Ready()
    {
        _selectButton.Pressed += OnSelectButtonPressed;
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
}
