using Godot;
using nuscutiesapp.tools;
using System.Collections.Generic;

public partial class UserSelectionPage : Control
{
    [Export] private VBoxContainer _userContainer;
    [Export] private Button _addUserButton;
    [Export] private Label _titleLabel;
    [Export] private AddUserDialog _addUserDialog;
    [Export] private ConfirmationDialog _deleteUserDialog;
    [Export] private EditUserDialog _editUserDialog;
    private string _pendingDeleteUser;
    private string _pendingEditUser;

    private UserManager _userManager;
    private PackedScene _userCardScene;

    public override void _Ready()
    {
        _userManager = GetNode<UserManager>("/root/UserManager");
        _userCardScene = ResourceLoader.Load<PackedScene>(Paths.UserCard);

        _addUserButton.Pressed += OnAddUserButtonPressed;
        _addUserDialog.UserCreated += OnUserCreated;
        _editUserDialog.UserEdited += OnEditUserConfirmed;
        _deleteUserDialog.Confirmed += OnDeleteUserConfirmed;

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
        _addUserDialog.PopupCentered();
    }

    private void OnUserCreated(string username)
    {
        RefreshUserList();
    }

    private void OnUserEditRequested(string username)
    {
        _pendingEditUser = username;
        _editUserDialog.SetOldUsername(username);
        _editUserDialog.PopupCentered();
    }

    private void OnUserDeleteRequested(string username)
    {
        _pendingDeleteUser = username;
        _deleteUserDialog.DialogText = $"Are you sure you want to delete user '{username}'? This cannot be undone.";
        _deleteUserDialog.PopupCentered();
    }

    private void OnEditUserConfirmed(string newUsername)
    {
        try
        {
            _userManager.RenameUser(_pendingEditUser, newUsername);
            RefreshUserList();
        }
        catch (System.Exception ex)
        {
            _editUserDialog.ShowError(ex.Message);
        }
    }

    private void OnDeleteUserConfirmed()
    {
        try
        {
            _userManager.DeleteUser(_pendingDeleteUser);
            RefreshUserList();
        }
        catch (System.Exception ex)
        {
            GD.PrintErr(ex.Message);
        }
    }
}
