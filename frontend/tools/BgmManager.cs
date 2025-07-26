using Godot;

public partial class BgmManager : Node
{
    [Export] public AudioStreamPlayer BgmPlayer;
    [Export] public AudioStream MainBgm;
    [Export] public AudioStream FightBgm;

    public override void _Ready()
    {
        SetVolume(GetNode<SettingsManager>("/root/SettingsManager").LoadSettings().BgmVolume);
    }

    public float GetVolume()
    {
        return Mathf.DbToLinear(BgmPlayer.VolumeDb);
    }
    public void SetVolume(float percentage)
    {
        BgmPlayer.VolumeDb = Mathf.LinearToDb(percentage);
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