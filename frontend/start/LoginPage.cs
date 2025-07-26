using Godot;
using nuscutiesapp.tools;

public partial class LoginPage : Control
{
    [Export]
    private Button _loginButton;

    public override void _Ready()
    {
        _loginButton.Pressed += LoginButtonOnPressed;
        base._Ready();
    }

    private void LoginButtonOnPressed()
    {
        GetNode<AudioManager>("/root/AudioManager").PlayMainBgm();
        GetTree().ChangeSceneToFile(Paths.UserSelection);
    }
}
