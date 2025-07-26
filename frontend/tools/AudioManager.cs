using Godot;

public partial class AudioManager : Node
{
    [Export] public AudioStreamPlayer BgmPlayer;
    [Export] public AudioStreamPlayer SfxPlayer;
    [Export] public AudioStream ButtonClickSound;

    public override void _Ready()
    {
        GetTree().Root.ChildEnteredTree += OnChildEnteredTree;
        ConnectAllCurrentButtons();
    }

    private void OnChildEnteredTree(Node node)
    {
        ConnectButtonsInNode(node);
    }

    public void ConnectButtonsInNode(Node node)
    {
        // Check if this node itself is a button in the group
        if (node.IsInGroup("sfx_buttons") && node is Button button)
        {
            button.Pressed += PlayButtonClick;
        }
    
        // Recursively check all children
        foreach (Node child in node.GetChildren())
        {
            ConnectButtonsInNode(child);
        }
    }

    private void ConnectAllCurrentButtons()
    {
        var buttons = GetTree().GetNodesInGroup("sfx_buttons");
        foreach (var node in buttons)
        {
            if (node is Button button)
                button.Pressed += PlayButtonClick;
        }
    }
    
    public void PlayBgm(AudioStream stream)
    {
        if (BgmPlayer.Stream != stream)
            BgmPlayer.Stream = stream;
        if (!BgmPlayer.Playing)
            BgmPlayer.Play();
    }

    public void StopBgm()
    {
        BgmPlayer.Stop();
    }

    public void PlaySfx(AudioStream stream)
    {
        SfxPlayer.Stream = stream;
        SfxPlayer.Play();
    }

    public void PlayButtonClick()
    {
        PlaySfx(ButtonClickSound);
    }

    public void ConnectButtonToClickSound(Button button)
    {
        button.AddToGroup("sfx_buttons");
        button.Pressed += () => PlayButtonClick();
    }
}
