using Godot;
using nuscutiesapp.tools;
using System;

public partial class SettingsManager : BaseStatManager<SettingsManager.SettingsData>
{
    public class SettingsData
    {
        public float BgmVolume { get; set; } = 1.0f;
        public float SfxVolume { get; set; } = 1.0f;
    }

    protected override string BaseSaveFileName => "global_settings.json";

    protected override void OnDataChanged() { }

    public float GetBgmVolume()
    {
        return Data.BgmVolume;
    }

    public void SetBgmVolume(float value)
    {
        Data.BgmVolume = value;
        NotifyDataChanged();
    }

    public float GetSfxVolume()
    {
        return Data.SfxVolume;
    }

    public void SetSfxVolume(float value)
    {
        Data.SfxVolume = value;
        NotifyDataChanged();
    }
}
