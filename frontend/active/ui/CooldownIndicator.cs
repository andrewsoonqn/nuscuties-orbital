using Godot;
using nuscutiesapp.tools;

namespace nuscutiesapp.active.ui
{
    public partial class CooldownIndicator : Control
    {
        [Export] private Panel _overlayPanel;
        [Export] private TextureProgressBar _progressBar;
        [Export] private Color _loadingColor = new Color(0.5f, 0.5f, 0.5f, 0.7f);
        [Export] private Color _inProgressColor = new Color(0.3f, 0.3f, 0.3f, 0.8f);
        [Export] private Color _cooldownColor = new Color(0.5f, 0.5f, 0.5f, 0.7f);

        private IActiveAbility _activeAbility;
        private bool _isVisible = false;

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

            SetupProgressBar();
            SetVisible(false);
        }

        private void SetupProgressBar()
        {
            if (_progressBar == null) return;

            var circularTexture = GD.Load<Texture2D>("res://assets/white-circle.png");
            if (circularTexture != null)
            {
                _progressBar.TextureProgress = circularTexture;
                _progressBar.FillMode = (int)TextureProgressBar.FillModeEnum.Clockwise;
            }
        }

        public void SetActiveAbility(IActiveAbility ability)
        {
            _activeAbility = ability;
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
                    UpdateProgressBar(completionPercentage, _loadingColor);
                    break;

                case ActiveAbilityPhase.InProgress:
                    SetVisible(true);
                    SetFullOverlay(_inProgressColor);
                    break;

                case ActiveAbilityPhase.Cooldown:
                    SetVisible(true);
                    UpdateProgressBar(completionPercentage, _cooldownColor);
                    break;
            }
        }

        private void UpdateProgressBar(float completionPercentage, Color color)
        {
            if (_progressBar == null) return;

            _progressBar.Visible = true;
            _progressBar.Value = completionPercentage * 100f;
            _progressBar.Modulate = color;

            if (_overlayPanel != null)
            {
                _overlayPanel.Visible = false;
            }
        }

        private void SetFullOverlay(Color color)
        {
            if (_overlayPanel == null) return;

            _overlayPanel.Visible = true;
            var styleBox = new StyleBoxFlat();
            styleBox.BgColor = color;
            styleBox.CornerRadiusTopLeft = 8;
            styleBox.CornerRadiusTopRight = 8;
            styleBox.CornerRadiusBottomLeft = 8;
            styleBox.CornerRadiusBottomRight = 8;

            _overlayPanel.AddThemeStyleboxOverride("panel", styleBox);

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
