using Godot;
using System;

namespace nuscutiesapp.active.characters
{
    public partial class ColorManager : Node
    {
        [Export] private Character _parent;
        [Export] private AnimatedSprite2D _sprite;

        private Color _originalBaseColor = Colors.White;
        private Color _currentBaseColor = Colors.White;
        private Color _currentColor = Colors.White;

        private bool _isPulsing = false;
        private float _pulseTimer = 0f;
        private float _pulseDuration = 0f;
        private Color _pulseColor = Colors.White;

        public override void _Ready()
        {
            if (_sprite != null)
            {
                _originalBaseColor = _sprite.Modulate;
                _currentBaseColor = _originalBaseColor;
                _currentColor = _originalBaseColor;
            }
        }

        public void SetBaseColor(Color color)
        {
            _currentBaseColor = color;
            if (!_isPulsing)
            {
                _currentColor = color;
                ApplyColor();
            }
        }

        public void Pulse(Color color, float duration)
        {
            _pulseColor = color;
            _pulseDuration = duration;
            _pulseTimer = duration;
            _isPulsing = true;
            _currentColor = color;
            ApplyColor();
        }

        public void ResetToOriginal()
        {
            _currentBaseColor = _originalBaseColor;
            _currentColor = _originalBaseColor;
            _isPulsing = false;
            ApplyColor();
        }

        public override void _Process(double delta)
        {
            if (_isPulsing)
            {
                _pulseTimer -= (float)delta;

                if (_pulseTimer <= 0f)
                {
                    _isPulsing = false;
                    _currentColor = _currentBaseColor;
                    ApplyColor();
                }
            }
        }

        private void ApplyColor()
        {
            if (_sprite != null)
            {
                _sprite.Modulate = _currentColor;
            }
        }

        public Color GetCurrentColor()
        {
            return _currentColor;
        }

        public Color GetBaseColor()
        {
            return _currentBaseColor;
        }

        public bool IsPulsing()
        {
            return _isPulsing;
        }
    }
}
