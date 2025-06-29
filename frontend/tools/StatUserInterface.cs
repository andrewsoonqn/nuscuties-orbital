using Godot;

namespace nuscutiesapp.tools
{
    public partial class StatUserInterface : Control
    {
        [Export] private Button _addStrengthButton;
        [Export] private Button _addStaminaButton;
        [Export] private Button _decrStrengthButton;
        [Export] private Button _decrStaminaButton;
        [Export] private Label _strengthLabel;
        [Export] private Label _staminaLabel;
        [Export] private Label _remainingStatPointsLabel;
        
        private ProgressionManager _progressionManager;
        private PlayerStatManager _playerStatManager;

        public override void _Ready()
        {
            _progressionManager = GetNode<ProgressionManager>("/root/ProgressionManager");
            _playerStatManager = GetNode<PlayerStatManager>("/root/PlayerStatManager");
            _addStrengthButton.Pressed += () => _playerStatManager.AddStrength(1);
            _decrStrengthButton.Pressed += () => _playerStatManager.AddStrength(-1);
            _addStaminaButton.Pressed += () => _playerStatManager.AddStamina(1);
            _decrStaminaButton.Pressed += () => _playerStatManager.AddStamina(-1);
            _playerStatManager.StrengthChanged += PlayerStatManagerOnStrengthChanged;
            _playerStatManager.StaminaChanged += PlayerStatManagerOnStaminaChanged;

            InitializeUI();
            
            base._Ready();
        }

        private void InitializeUI()
        {
            _strengthLabel.Text = _playerStatManager.GetStrength().ToString();
            _staminaLabel.Text = _playerStatManager.GetStamina().ToString();
            UpdateRemaining();
        }

        private void PlayerStatManagerOnStaminaChanged(int stamina)
        {
            _staminaLabel.Text = stamina.ToString();
            UpdateRemaining();
        }

        private void PlayerStatManagerOnStrengthChanged(int strength)
        {
            GD.Print("hihi");
            _strengthLabel.Text = strength.ToString();
            UpdateRemaining();
        }

        private void UpdateRemaining()
        {
            int remaining = _playerStatManager.GetRemainingStatPoints();
            _remainingStatPointsLabel.Text = $"{remaining} stat points remaining";
        }
    }
}