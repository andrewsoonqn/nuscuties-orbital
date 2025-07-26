using Godot;

public partial class AudioManager : Node
{
    [Export] public AudioStreamPlayer BgmPlayer;
    [Export] public AudioStreamPlayer SfxPlayer;
    [Export] public AudioStream ButtonClickSound;

    public override void _Ready()
    {
        CallDeferred(nameof(ConnectButtonSignals));
    }

    private void ConnectButtonSignals()
    {
        var buttons = GetTree().GetNodesInGroup("sfx_buttons");
        foreach (var node in buttons)
        {
            if (node is Button button)
                button.Pressed += () => PlayButtonClick();
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
