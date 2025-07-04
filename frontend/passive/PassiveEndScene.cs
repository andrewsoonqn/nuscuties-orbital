using Godot;
using nuscutiesapp.tools;

public partial class PassiveEndScene : Node
{
    [Export]
    private Label _expAccumulatedLabel;

    [Export]
    private Label _totalTimeSpentLabel;

    [Export]
    private Button _returnButton;

    private PassiveSessionInfoManager _passiveSessionInfoManager;
    private ProgressionManager _expManager;

    public override void _Ready()
    {
        this._passiveSessionInfoManager = this.GetNode<PassiveSessionInfoManager>("/root/PassiveSessionInfoManager");
        this._expManager = this.GetNode<ProgressionManager>("/root/ProgressionManager");
        int expGained = _passiveSessionInfoManager.getAccumulatedExp();
        _expManager.AddExp(expGained);

        this._expAccumulatedLabel.Text = $"Total EXP Gained: {expGained}";
        this._totalTimeSpentLabel.Text = $"Total Time Spent: {_passiveSessionInfoManager.getTimeSpent():F0} seconds";

        this._returnButton.Pressed += () => this.GetTree().ChangeSceneToFile(Paths.Passive);
    }
}