using Godot;
using nuscutiesapp.tools;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class UserManager : Node
{
    private string _currentUsername = "";

    public override void _Ready()
    {
        var users = GetAllUsers();
        if (users.Count > 0)
        {
            SetCurrentUser(users[0]);
        }

    }

    public List<string> GetAllUsers()
    {
        var users = new List<string>();
        var dir = DirAccess.Open("user://");

        if (dir == null)
        {
            GD.PrintErr("Failed to open user directory");
            return users;
        }

        dir.ListDirBegin();
        string fileName = dir.GetNext();

        while (fileName != "")
        {
            if (fileName.EndsWith("_player_progression.json"))
            {
                string username = fileName.Replace("_player_progression.json", "");
                if (!users.Contains(username))
                {
                    users.Add(username);
                }
            }
            fileName = dir.GetNext();
        }

        dir.ListDirEnd();
        return users.OrderBy(u => u).ToList();
    }

    public void CreateNewUser(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be empty");
        }

        if (UserExists(username))
        {
            throw new ArgumentException($"User '{username}' already exists");
        }

        if (username.Contains("_") || username.Contains("/") || username.Contains("\\"))
        {
            throw new ArgumentException("Username cannot contain special characters (_ / \\)");
        }

        SetCurrentUser(username);

        var progressionManager = GetNode<ProgressionManager>("/root/ProgressionManager");
        var playerStatManager = GetNode<PlayerStatManager>("/root/PlayerStatManager");
        var inventoryManager = GetNode<PlayerInventoryManager>("/root/PlayerInventoryManager");

        progressionManager?.NotifyDataChanged();
        playerStatManager?.NotifyDataChanged();
        inventoryManager?.NotifyDataChanged();

        GD.Print($"Created new user: {username}");
    }

    public void SetCurrentUser(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be empty");
        }

        _currentUsername = username;

        ReloadAllManagerData();
        GD.Print($"Current user set to: {username}");
    }

    private void ReloadAllManagerData()
    {
        var progressionManager = GetNodeOrNull<ProgressionManager>("/root/ProgressionManager");
        var playerStatManager = GetNodeOrNull<PlayerStatManager>("/root/PlayerStatManager");
        var inventoryManager = GetNodeOrNull<PlayerInventoryManager>("/root/PlayerInventoryManager");

        progressionManager?.ReloadData();
        playerStatManager?.ReloadData();
        inventoryManager?.ReloadData();
    }

    public string GetCurrentUser()
    {
        return _currentUsername;
    }

    public bool UserExists(string username)
    {
        return GetAllUsers().Contains(username);
    }


}
