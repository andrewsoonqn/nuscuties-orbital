using Godot;

public partial class AudioManager : Node
{
    [Export] public AudioStreamPlayer SfxPlayer;
    [Export] public AudioStream ButtonClickSound;

    public override void _Ready()
    {
        SetVolume(GetNode<SettingsManager>("/root/SettingsManager").LoadSettings().SfxVolume);
        GetTree().Root.ChildEnteredTree += OnChildEnteredTree;
        ConnectAllCurrentButtons();
    }

    public float GetVolume()
    {
        return Mathf.DbToLinear(SfxPlayer.VolumeDb);
    }
    public void SetVolume(float percentage)
    {
        SfxPlayer.VolumeDb = Mathf.LinearToDb(percentage);
    }

    private void OnChildEnteredTree(Node node)
    {
        ConnectButtonsInNode(node);
    }

    public void ConnectButtonsInNode(Node node)
    {
        if (node.IsInGroup("sfx_buttons") && node is Button button)
        {
            button.Pressed += PlayButtonClick;
        }
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