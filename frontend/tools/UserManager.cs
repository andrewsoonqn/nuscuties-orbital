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
        // No automatic user selection - users must explicitly choose a user
    }

    public List<string> GetAllUsers()
    {
        var users = new List<string>();
        var dir = DirAccess.Open("user://saves");

        if (dir == null)
        {
            GD.PrintErr("Failed to open saves directory");
            return users;
        }

        dir.ListDirBegin();
        string entry = dir.GetNext();

        while (entry != "")
        {
            if (dir.CurrentIsDir() && entry != "." && entry != "..")
            {
                users.Add(entry);
            }
            entry = dir.GetNext();
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

    public void RenameUser(string oldName, string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Username cannot be empty");
        if (UserExists(newName))
            throw new ArgumentException($"User '{newName}' already exists");
        if (newName.Contains("_") || newName.Contains("/") || newName.Contains("\\"))
            throw new ArgumentException("Username cannot contain special characters (_ / \\)");
        var oldPath = $"user://saves/{oldName}";
        var newPath = $"user://saves/{newName}";
        if (!DirAccess.DirExistsAbsolute(oldPath))
            throw new ArgumentException($"User '{oldName}' does not exist");
        DirAccess.RenameAbsolute(oldPath, newPath);
        if (_currentUsername == oldName)
            SetCurrentUser(newName);
    }

    public void DeleteUser(string username)
    {
        var userPath = $"user://saves/{username}";
        if (!DirAccess.DirExistsAbsolute(userPath))
            throw new ArgumentException($"User '{username}' does not exist");
        RecursiveDelete(userPath);
        DirAccess.RemoveAbsolute(userPath);
        if (_currentUsername == username)
            SetCurrentUser("");
    }

    private void RecursiveDelete(string path)
    {
        var dir = DirAccess.Open(path);
        if (dir == null) return;
        dir.ListDirBegin();
        string entry = dir.GetNext();
        while (entry != "")
        {
            if (entry == "." || entry == "..")
            {
                entry = dir.GetNext();
                continue;
            }
            string fullPath = path + "/" + entry;
            if (dir.CurrentIsDir())
            {
                RecursiveDelete(fullPath);
                DirAccess.RemoveAbsolute(fullPath);
            }
            else
            {
                DirAccess.RemoveAbsolute(fullPath);
            }
            entry = dir.GetNext();
        }
        dir.ListDirEnd();
    }


}