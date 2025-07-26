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

    public float GetVolume()
    {
        return ConvertDbToPercentage(BgmPlayer.VolumeDb);
    }
    public void SetVolume(float percentage)
    {
        BgmPlayer.VolumeDb = ConvertPercentageToDb(percentage);
    }

    private float ConvertPercentageToDb(float percentage)
    {
        if (percentage <= 0) return -80.0f;
        return Mathf.Log(percentage / 100.0f) * 20.0f;
    }

    private float ConvertDbToPercentage(float db)
    {
        if (db <= -80.0f) return 0.0f;
        return Mathf.Pow(10.0f, db / 20.0f) * 100.0f;
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
