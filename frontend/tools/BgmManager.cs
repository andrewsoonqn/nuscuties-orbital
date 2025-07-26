using Godot;

public partial class BgmManager : Node
{
    [Export] public AudioStreamPlayer BgmPlayer;
    [Export] public AudioStream MainBgm;
    [Export] public AudioStream FightBgm;

    public override void _Ready()
    {
        // Optionally, play main BGM on startup
        // PlayMainBgm();
    }

    public void PlayBgm(AudioStream stream)
    {
        if (BgmPlayer.Stream != stream)
            BgmPlayer.Stream = stream;
        if (!BgmPlayer.Playing)
            BgmPlayer.Play();
    }

    public void PlayMainBgm()
    {
        PlayBgm(MainBgm);
    }

    public void PlayFightBgm()
    {
        PlayBgm(FightBgm);
    }

    public void StopBgm()
    {
        BgmPlayer.Stop();
    }

    public void PauseBgm()
    {
        if (BgmPlayer.Playing)
            BgmPlayer.StreamPaused = true;
    }

    public void ResumeBgm()
    {
        if (BgmPlayer.StreamPaused)
            BgmPlayer.StreamPaused = false;
    }
}
