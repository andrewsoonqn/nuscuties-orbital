using Godot;
using nuscutiesapp.tools;

public partial class PassiveEndScene : Node
{
    [Export]
    private Label _expAccumulatedLabel;

    [Export]
    private Label _coinsAccumulatedLabel;

    [Export]
    private Label _totalTimeSpentLabel;

    [Export]
    private Button _returnButton;

    private PassiveSessionInfoManager _passiveSessionInfoManager;
    private ProgressionManager _expManager;
    private PlayerInventoryManager _inventoryManager;

    public override void _Ready()
    {
        this._passiveSessionInfoManager = this.GetNode<PassiveSessionInfoManager>("/root/PassiveSessionInfoManager");
        this._expManager = this.GetNode<ProgressionManager>("/root/ProgressionManager");
        this._inventoryManager = this.GetNode<PlayerInventoryManager>("/root/PlayerInventoryManager");

        int expGained = _passiveSessionInfoManager.getAccumulatedExp();
        int coinsGained = _passiveSessionInfoManager.getAccumulatedCoins();

        _expManager.AddExp(expGained);
        _inventoryManager.AddCoins(coinsGained);

        this._expAccumulatedLabel.Text = $"Total EXP Gained: {expGained}";
        if (_coinsAccumulatedLabel != null)
        {
            this._coinsAccumulatedLabel.Text = $"Total Coins Gained: {coinsGained}";
        }
        this._totalTimeSpentLabel.Text = $"Total Time Spent: {_passiveSessionInfoManager.getTimeSpent():F0} seconds";

        this._returnButton.Pressed += () => this.GetTree().ChangeSceneToFile(Paths.Passive);
    }
}