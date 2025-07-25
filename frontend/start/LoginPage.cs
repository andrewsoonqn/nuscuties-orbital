using Godot;
using nuscutiesapp.tools;

public partial class LoginPage : Control
{
	[Export]
	private LineEdit Username { get; set; }

	[Export]
	private Button SubmitLogin { get; set; }

	private AccountManager _accountManager;

	public override void _Ready()
	{
		Username.TextSubmitted += OnUsernameSubmitted;
		SubmitLogin.Pressed += () => OnUsernameSubmitted(Username.Text);

		this._accountManager = this.GetNode<AccountManager>("/root/AccountManager");
	}

	private void OnUsernameSubmitted(string username)
	{
		_accountManager.SetUsername(username);

		var transition = GetNode("/root/TransitionLoginToHome") as Node;

		transition.Connect("on_transition_finished", Callable.From(() =>
		{
			this.GetTree().ChangeSceneToFile(Paths.Home);
		}));

		transition.Call("transition");
	}
}
