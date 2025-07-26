using Godot;
using nuscutiesapp.tools;
using System.Collections.Generic;

public partial class UserSelectionPage : Control
{
    [Export] private VBoxContainer _userContainer;
    [Export] private Button _addUserButton;
    [Export] private Label _titleLabel;

    private UserManager _userManager;
    private PackedScene _userCardScene;
    private PackedScene _userAddPanelScene;
    private PackedScene _userEditPanelScene;
    private PackedScene _userDeletePanelScene;

    public override void _Ready()
    {
        _userManager = GetNode<UserManager>("/root/UserManager");
        _userCardScene = ResourceLoader.Load<PackedScene>(Paths.UserCard);
        _userAddPanelScene = ResourceLoader.Load<PackedScene>(Paths.UserAddPanel);
        _userEditPanelScene = ResourceLoader.Load<PackedScene>(Paths.UserEditPanel);
        _userDeletePanelScene = ResourceLoader.Load<PackedScene>(Paths.UserDeletePanel);

        _addUserButton.Pressed += OnAddUserButtonPressed;

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
                userCardInstance.UserEditRequested += OnUserEditRequested;
                userCardInstance.UserDeleteRequested += OnUserDeleteRequested;
                _userContainer.AddChild(userCardInstance);
            }
        }
    }

    private void OnUserSelected(string username)
    {
        _userManager.SetCurrentUser(username);
        GetTree().ChangeSceneToFile(Paths.Home);
    }

    private void OnAddUserButtonPressed()
    {
        var addPanelInstance = _userAddPanelScene.Instantiate<UserAddPanel>();
        addPanelInstance.UserCreated += OnUserCreated;
        GetTree().Root.AddChild(addPanelInstance);
    }

    private void OnUserEditRequested(string username)
    {
        var editPanelInstance = _userEditPanelScene.Instantiate<UserEditPanel>();
        editPanelInstance.Initialize(username);
        editPanelInstance.UserEdited += OnUserEdited;
        GetTree().Root.AddChild(editPanelInstance);
    }

    private void OnUserDeleteRequested(string username)
    {
        var deletePanelInstance = _userDeletePanelScene.Instantiate<UserDeletePanel>();
        deletePanelInstance.Initialize(username);
        deletePanelInstance.UserDeleteConfirmed += OnUserDeleteConfirmed;
        GetTree().Root.AddChild(deletePanelInstance);
    }

    private void OnUserCreated(string username)
    {
        RefreshUserList();
    }

    private void OnUserEdited(string oldUsername, string newUsername)
    {
        RefreshUserList();
    }

    private void OnUserDeleteConfirmed(string username)
    {
        RefreshUserList();
    }
}
