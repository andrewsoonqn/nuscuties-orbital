using Godot;
using nuscutiesapp.tools;

namespace nuscutiesapp.active.ui
{
    public partial class CooldownIndicator : Control
    {
        [Export] private Panel _overlayPanel;
        [Export] private TextureProgressBar _progressBar;

        private IActiveAbility _activeAbility;
        private bool _isVisible = false;
        private LoadoutSpawner _loadoutSpawner;

        public override void _Ready()
        {
            if (_overlayPanel == null)
            {
                _overlayPanel = GetNode<Panel>("OverlayPanel");
            }

            if (_progressBar == null)
            {
                _progressBar = GetNode<TextureProgressBar>("ProgressBar");
            }

            ConnectToLoadoutSpawner();
            SetVisible(false);
        }

        private void ConnectToLoadoutSpawner()
        {
            _loadoutSpawner = GetNode<LoadoutSpawner>("/root/LoadoutSpawner");
            if (_loadoutSpawner != null)
            {
                _loadoutSpawner.ActiveAbilityCreated += OnActiveAbilityCreated;
            }
        }

        private void OnActiveAbilityCreated(Node ability)
        {
            _activeAbility = (IActiveAbility)ability;
        }

        public override void _Process(double delta)
        {
            if (_activeAbility == null) return;

            var currentPhase = _activeAbility.GetCurrentPhase();
            var completionPercentage = _activeAbility.GetPhaseCompletionPercentage();

            switch (currentPhase)
            {
                case ActiveAbilityPhase.Ready:
                    SetVisible(false);
                    break;

                case ActiveAbilityPhase.Loading:
                    SetVisible(true);
                    UpdateProgressBar(completionPercentage);
                    break;

                case ActiveAbilityPhase.InProgress:
                    SetVisible(true);
                    SetFullOverlay();
                    break;

                case ActiveAbilityPhase.Cooldown:
                    SetVisible(true);
                    UpdateProgressBar(completionPercentage);
                    break;
            }
        }

        private void UpdateProgressBar(float completionPercentage)
        {
            if (_progressBar == null) return;

            _progressBar.Visible = true;
            _progressBar.Value = 100 - completionPercentage * 100f;

            if (_overlayPanel != null)
            {
                _overlayPanel.Visible = false;
            }
        }

        private void SetFullOverlay()
        {
            if (_overlayPanel == null) return;

            _overlayPanel.Visible = true;
            if (_progressBar != null)
            {
                _progressBar.Visible = false;
            }
        }

        private void SetVisible(bool visible)
        {
            if (_isVisible == visible) return;

            _isVisible = visible;
            Visible = visible;
        }
    }
}