using Godot;
using nuscutiesapp.active.characters.DamageSystem;

public partial class EnemyHealthBar : Control
{
    [Export] private TextureProgressBar _healthBar;
    [Export] private Timer _hideTimer;

    private Character _targetCharacter;
    private bool _isVisible = false;

    public override void _Ready()
    {
        Visible = false;
        _isVisible = false;

        if (_hideTimer == null)
            _hideTimer = GetNode<Timer>("HideTimer");

        if (_healthBar == null)
            _healthBar = GetNode<TextureProgressBar>("HealthBar");

        _hideTimer.WaitTime = 3.0f;
        _hideTimer.OneShot = true;
        _hideTimer.Timeout += OnHideTimerTimeout;
    }

    public void Initialize(Character character)
    {
        _targetCharacter = character;

        if (_targetCharacter?.Health != null)
        {
            _targetCharacter.Health.Damaged += OnCharacterDamaged;
            _targetCharacter.Health.Died += OnCharacterDied;

            UpdateHealthBar();
        }
    }

    private void OnCharacterDamaged(float currentHP, DamageInfo damageInfo)
    {
        UpdateHealthBar();
        ShowHealthBar();

        _hideTimer.Stop();
        _hideTimer.Start();
    }

    private void OnCharacterDied(DamageInfo damageInfo)
    {
        HideHealthBar();
        _hideTimer.Stop();
    }

    private void UpdateHealthBar()
    {
        if (_targetCharacter?.Health != null && _healthBar != null)
        {
            _healthBar.MaxValue = _targetCharacter.Health.MaxHP;
            _healthBar.Value = _targetCharacter.Health.CurrentHP;
        }
    }

    private void ShowHealthBar()
    {
        if (!_isVisible)
        {
            _isVisible = true;
            Visible = true;
        }
    }

    private void HideHealthBar()
    {
        if (_isVisible)
        {
            _isVisible = false;
            Visible = false;
        }
    }

    private void OnHideTimerTimeout()
    {
        HideHealthBar();
    }

    public override void _ExitTree()
    {
        if (_targetCharacter?.Health != null)
        {
            _targetCharacter.Health.Damaged -= OnCharacterDamaged;
            _targetCharacter.Health.Died -= OnCharacterDied;
        }
        base._ExitTree();
    }
}
