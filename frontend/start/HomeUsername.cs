using Godot;
using nuscutiesapp.tools;
using System;

public partial class HomeUsername : VBoxContainer
{
	[Export]
	private Label UsernameLabel { get; set; }

	private AccountManager _accountManager;

	public override void _Ready()
	{
		_accountManager = GetNode<AccountManager>("/root/AccountManager");
		UsernameLabel.Text = $"{_accountManager.GetUsername()}";
	}
}
