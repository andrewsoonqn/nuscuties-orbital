using Godot;
using System;

public partial class HomeUsername : VBoxContainer
{
    [Export]
    private Label UsernameLabel { get; set; }

    private UserManager _userManager;

    public override void _Ready()
    {
        _userManager = GetNode<UserManager>("/root/UserManager");
        UsernameLabel.Text = _userManager.GetCurrentUser();
    }
}
