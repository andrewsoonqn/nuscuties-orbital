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
        [Export] private Button _closeButton;

        private ProgressionManager _progressionManager;
        private PlayerStatManager _playerStatManager;

        public override void _Ready()
        {
            _progressionManager = GetNode<ProgressionManager>("/root/ProgressionManager");
            _playerStatManager = GetNode<PlayerStatManager>("/root/PlayerStatManager");

            ConnectSignals();
            InitializeUI();

            base._Ready();
        }

        private void InitializeUI()
        {
            _strengthLabel.Text = _playerStatManager.GetStrength().ToString();
            _staminaLabel.Text = _playerStatManager.GetStamina().ToString();
            UpdateRemaining();
        }

        private void ConnectSignals()
        {
            _addStrengthButton.Pressed += AddStrengthButtonOnPressed;
            _decrStrengthButton.Pressed += DecrStrengthButtonOnPressed;
            _addStaminaButton.Pressed += AddStaminaButtonOnPressed;
            _decrStaminaButton.Pressed += DecrStaminaButtonOnPressed;

            _playerStatManager.StrengthChanged += PlayerStatManagerOnStrengthChanged;
            _playerStatManager.StaminaChanged += PlayerStatManagerOnStaminaChanged;
            _playerStatManager.StatPointsChanged += PlayerStatManagerOnStatPointsChanged;

            _closeButton.Pressed += CloseButtonOnPressed;
        }

        private void CloseButtonOnPressed()
        {
            DisconnectSignals();
            QueueFree();
        }

        private void DisconnectSignals()
        {
            _addStrengthButton.Pressed -= AddStrengthButtonOnPressed;
            _decrStrengthButton.Pressed -= DecrStrengthButtonOnPressed;
            _addStaminaButton.Pressed -= AddStaminaButtonOnPressed;
            _decrStaminaButton.Pressed -= DecrStaminaButtonOnPressed;

            _playerStatManager.StrengthChanged -= PlayerStatManagerOnStrengthChanged;
            _playerStatManager.StaminaChanged -= PlayerStatManagerOnStaminaChanged;
            _playerStatManager.StatPointsChanged -= PlayerStatManagerOnStatPointsChanged;

            _closeButton.Pressed -= CloseButtonOnPressed;
        }

        private void DecrStaminaButtonOnPressed()
        {
            _playerStatManager.AddStamina(-1);
        }

        private void AddStaminaButtonOnPressed()
        {
            _playerStatManager.AddStamina(1);
        }

        private void DecrStrengthButtonOnPressed()
        {
            _playerStatManager.AddStrength(-1);
        }

        private void AddStrengthButtonOnPressed()
        {
            _playerStatManager.AddStrength(1);
        }


        private void PlayerStatManagerOnStatPointsChanged(int statPoints)
        {
            UpdateRemaining();
        }

        private void PlayerStatManagerOnStaminaChanged(int stamina)
        {
            _staminaLabel.Text = stamina.ToString();
            UpdateRemaining();
        }

        private void PlayerStatManagerOnStrengthChanged(int strength)
        {
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