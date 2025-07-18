using Godot;

public partial class AccountManager : Node
{
    private string Username { get; set; }

    public void SetUsername(string username)
    {
        this.Username = username;
    }

    public string GetUsername()
    {
        return this.Username;
    }
}