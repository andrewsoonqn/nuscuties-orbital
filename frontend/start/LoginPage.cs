using Godot;
using nuscutiesapp.tools;

public partial class LoginPage : Control
{
    [Export]
    private LineEdit Username { get; set; }

    [Export]
    private Button SubmitLogin { get; set; }

    private UserManager _userManager;

    public override void _Ready()
    {
        Username.TextSubmitted += OnUsernameSubmitted;
        SubmitLogin.Pressed += () => OnUsernameSubmitted(Username.Text);
        _userManager = GetNode<UserManager>("/root/UserManager");
    }

    private void OnUsernameSubmitted(string username)
    {
        _userManager.SetCurrentUser(username);
        var transition = GetNode("/root/TransitionLoginToHome") as Node;
        transition.Connect("on_transition_finished", Callable.From(() =>
        {
            this.GetTree().ChangeSceneToFile(Paths.Home);
        }));
        transition.Call("transition");
    }
}
