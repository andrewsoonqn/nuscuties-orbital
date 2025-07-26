using Godot;
using nuscutiesapp.tools;

public partial class VolumeControl : Control
{
    [Export] private HSlider _bgmSlider;
    [Export] private HSlider _sfxSlider;
    [Export] private Label _bgmLabel;
    [Export] private Label _sfxLabel;

    private BgmManager _bgmManager;
    private AudioManager _audioManager;

    public override void _Ready()
    {
        _bgmManager = GetNode<BgmManager>("/root/BgmManager");
        _audioManager = GetNode<AudioManager>("/root/AudioManager");

        SetupSliders();
        ConnectSignals();
        LoadCurrentVolumes();
    }

    private void SetupSliders()
    {
        _bgmSlider.MinValue = 0;
        _bgmSlider.MaxValue = 1;
        _bgmSlider.Step = 0.01;

        _sfxSlider.MinValue = 0;
        _sfxSlider.MaxValue = 1;
        _sfxSlider.Step = 0.01;
    }

    private void ConnectSignals()
    {
        _bgmSlider.ValueChanged += OnBgmVolumeChanged;
        _sfxSlider.ValueChanged += OnSfxVolumeChanged;
    }

    private void LoadCurrentVolumes()
    {
        if (_bgmManager != null)
        {
            float volume = _bgmManager.GetVolume();
            _bgmSlider.Value = volume;
            _bgmLabel.Text = $"BGM Volume: {(volume * 100):F0}%";
        }

        if (_audioManager != null)
        {
            float volume = _audioManager.GetVolume();
            _sfxSlider.Value = volume;
            _sfxLabel.Text = $"SFX Volume: {(volume * 100):F0}%";
        }
    }

    private void OnBgmVolumeChanged(double value)
    {
        if (_bgmManager != null)
        {
            _bgmManager.SetVolume((float)value);
            _bgmLabel.Text = $"BGM Volume: {(value * 100):F0}%";
        }
    }

    private void OnSfxVolumeChanged(double value)
    {
        if (_audioManager != null)
        {
            _audioManager.SetVolume((float)value);
            _sfxLabel.Text = $"SFX Volume: {(value * 100):F0}%";
        }
    }
}