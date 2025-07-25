using Godot;
using nuscutiesapp.active.ui;

namespace nuscutiesapp.tools
{
    public partial class BaseNumberManager : Node
    {
        private PackedScene _damageNumberScene;
        private PackedScene _healthGainNumberScene;
        private PackedScene _coinGainNumberScene;

        public override void _Ready()
        {
            _damageNumberScene = GD.Load<PackedScene>("res://active/ui/damage_number.tscn");
            _healthGainNumberScene = GD.Load<PackedScene>("res://active/ui/health_gain_number.tscn");
            _coinGainNumberScene = GD.Load<PackedScene>("res://active/ui/coin_gain_number.tscn");
        }

        public void ShowDamage(float amount, Vector2 worldPosition, Node world)
        {
            var damageNumber = _damageNumberScene.Instantiate<DamageNumber>();
            world.AddChild(damageNumber);
            damageNumber.Position = worldPosition;
            damageNumber.Show(amount.ToString());
        }

        public void ShowHealthGain(float amount, Vector2 worldPosition, Node world)
        {
            var healthNumber = _healthGainNumberScene.Instantiate<HealthGainNumber>();
            world.AddChild(healthNumber);
            healthNumber.Position = worldPosition;
            healthNumber.Show(amount.ToString());
        }

        public void ShowCoinGain(int amount, Vector2 worldPosition, Node world)
        {
            ShowCoinGain(amount, worldPosition, world, 1.0f);
        }

        public void ShowCoinGain(int amount, Vector2 worldPosition, Node world, float scale)
        {
            var coinNumber = _coinGainNumberScene.Instantiate<CoinGainNumber>();
            world.AddChild(coinNumber);
            coinNumber.Position = worldPosition;
            coinNumber.Scale = Vector2.One * scale;
            coinNumber.Show(amount.ToString());
        }

        public void Show(float amount, Vector2 worldPosition, Node world)
        {
            ShowDamage(amount, worldPosition, world);
        }
    }
}
