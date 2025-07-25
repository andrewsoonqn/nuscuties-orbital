using Godot;
using nuscutiesapp.tools;
using System.Collections.Generic;

public partial class UserSelectionPage : Control
{
    [Export] private VBoxContainer _userContainer;
    [Export] private Button _addUserButton;
    [Export] private Label _titleLabel;
    [Export] private AddUserDialog _addUserDialog;

    private UserManager _userManager;
    private PackedScene _userCardScene;

    public override void _Ready()
    {
        _userManager = GetNode<UserManager>("/root/UserManager");
        _userCardScene = ResourceLoader.Load<PackedScene>(Paths.UserCard);

        _addUserButton.Pressed += OnAddUserButtonPressed;
        _addUserDialog.UserCreated += OnUserCreated;

        RefreshUserList();
    }

    private void RefreshUserList()
    {
        foreach (Node child in _userContainer.GetChildren())
        {
            child.QueueFree();
        }

        var users = _userManager.GetAllUsers();

        if (users.Count == 0)
        {
            var noUsersLabel = new Label();
            noUsersLabel.Text = "No users found. Create your first user!";
            noUsersLabel.HorizontalAlignment = HorizontalAlignment.Center;
            noUsersLabel.AddThemeStyleboxOverride("normal", new StyleBoxEmpty());
            _userContainer.AddChild(noUsersLabel);
        }
        else
        {
            foreach (string username in users)
            {
                var userCardInstance = _userCardScene.Instantiate<UserCard>();
                userCardInstance.Initialize(username);
                userCardInstance.UserSelected += OnUserSelected;
                _userContainer.AddChild(userCardInstance);
            }
        }

        // _userContainer.AddChild(_addUserButton);
    }

    private void OnUserSelected(string username)
    {
        _userManager.SetCurrentUser(username);
        GetTree().ChangeSceneToFile(Paths.Home);
    }

    private void OnAddUserButtonPressed()
    {
        _addUserDialog.PopupCentered();
    }

    private void OnUserCreated(string username)
    {
        RefreshUserList();
    }
}
