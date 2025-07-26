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
        _bgmSlider.MaxValue = 100;
        _bgmSlider.Step = 1;

        _sfxSlider.MinValue = 0;
        _sfxSlider.MaxValue = 100;
        _sfxSlider.Step = 1;
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
            _bgmSlider.Value = _bgmManager.GetVolume();
            _bgmLabel.Text = $"BGM Volume: {_bgmSlider.Value}%";
        }

        if (_audioManager != null)
        {
            _sfxSlider.Value = _audioManager.GetVolume();
            _sfxLabel.Text = $"SFX Volume: {_sfxSlider.Value}%";
        }
    }

    private void OnBgmVolumeChanged(double value)
    {
        if (_bgmManager != null)
        {
            _bgmManager.SetVolume((int)value);
            _bgmLabel.Text = $"BGM Volume: {value}%";
        }
    }

    private void OnSfxVolumeChanged(double value)
    {
        if (_audioManager != null)
        {
            _audioManager.SetVolume((int)value);
            _sfxLabel.Text = $"SFX Volume: {value}%";
        }
    }
}
