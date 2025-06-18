using GdUnit4;
using Godot;
using nuscutiesapp.tools;
using System.Threading.Tasks;
using static GdUnit4.Assertions;

[TestSuite]
public partial class ActiveGameUiTest
{
    private ActiveGameUi _activeGameUi;
    private PanelContainer _pauseLabel;
    private PanelContainer _pauseMenu;
    private Button _resumeButton;
    private Button _quitButton;
    private Node2D _gameWorld;
    private Node _parentNode;
    private ISceneRunner _runner;

    [BeforeTest]
    public void Setup()
    {
        // Create a scene runner to get access to the scene tree
        _runner = ISceneRunner.Load(Paths.ActiveGame);

        // Create all needed components
        _activeGameUi = AutoFree(new ActiveGameUi());
        _pauseLabel = AutoFree(new PanelContainer());
        _pauseMenu = AutoFree(new PanelContainer());
        _resumeButton = AutoFree(new Button { Text = "Resume" });
        _quitButton = AutoFree(new Button { Text = "Quit Game" });
        _gameWorld = AutoFree(new Node2D { Name = "GameWorld" });
        _parentNode = AutoFree(new Node());

        // Add to scene tree
        _runner.Scene().GetTree().Root.AddChild(_parentNode);
        _parentNode.AddChild(_gameWorld);
        _parentNode.AddChild(_activeGameUi);

        // Assign exported fields manually for ActiveGameUi
        SetPrivateField(_activeGameUi, "_pauseLabel", _pauseLabel);
        SetPrivateField(_activeGameUi, "_pauseMenu", _pauseMenu);
        SetPrivateField(_activeGameUi, "_resumeButton", _resumeButton);
        SetPrivateField(_activeGameUi, "_quitButton", _quitButton);

        _activeGameUi._Ready();
    }

    [AfterTest]
    public void Cleanup()
    {
        (Engine.GetMainLoop() as SceneTree).SetPause(false); // Unpause scene tree if paused
    }

    private async Task _pauseTrigger()
    {
        InputEventKey trigger = new InputEventKey { Keycode = Key.Escape, Pressed = true };
        _activeGameUi._Input(trigger);
        await ISceneRunner.SyncProcessFrame;
    }

    private async Task _pressResumeButton()
    {
        _resumeButton.EmitSignal(Button.SignalName.Pressed);
        await ISceneRunner.SyncProcessFrame;
    }

    private async Task _pressQuitButton()
    {
        _quitButton.EmitSignal(Button.SignalName.Pressed);
        await ISceneRunner.SyncProcessFrame;
    }

    [TestCase]
    public void InitialMenuHidden()
    {
        AssertThat(_pauseMenu.Visible).IsFalse();
    }

    [TestCase]
    public void InitialLabelVisible()
    {
        AssertThat(_pauseLabel.Visible).IsTrue();
    }

    [TestCase]
    public void InitialNotPaused()
    {
        AssertThat(_runner.Scene().GetTree().IsPaused()).IsFalse();
    }

    [TestCase]
    public async Task PausePressedMenuVisible()
    {
        await _pauseTrigger();
        AssertThat(_pauseMenu.Visible).IsTrue();
    }

    [TestCase]
    public async Task PausePressedLabelHidden()
    {
        await _pauseTrigger();
        AssertThat(_pauseLabel.Visible).IsFalse();
    }

    [TestCase]
    public async Task PausePressedScenePaused()
    {
        await _pauseTrigger();
        AssertThat(_runner.Scene().GetTree().IsPaused()).IsTrue();
    }

    [TestCase]
    public async Task MultiplePausePressedScenePaused()
    {
        await _pauseTrigger();
        await _pauseTrigger();
        AssertThat(_runner.Scene().GetTree().IsPaused()).IsTrue();
    }

    [TestCase]
    public async Task NonPausePressedSceneNotPaused()
    {
        InputEventKey incorrectTrigger = new InputEventKey { Keycode = Key.Q, Pressed = true };
        _activeGameUi._Input(incorrectTrigger);
        await ISceneRunner.SyncProcessFrame;
        AssertThat(_pauseMenu.Visible).IsFalse();
        AssertThat(_pauseLabel.Visible).IsTrue();
        AssertThat(_runner.Scene().GetTree().IsPaused()).IsFalse();
    }

    [TestCase]
    public async Task ResumePressedMenuHidden()
    {
        await _pauseTrigger();
        await _pressResumeButton();
        AssertThat(_pauseMenu.Visible).IsFalse();
    }

    [TestCase]
    public async Task ResumePressedLabelVisible()
    {
        await _pauseTrigger();
        await _pressResumeButton();
        AssertThat(_pauseLabel.Visible).IsTrue();
    }

    [TestCase]
    public async Task ResumePressedSceneUnpaused()
    {
        await _pauseTrigger();
        await _pressResumeButton();
        AssertThat(_runner.Scene().GetTree().IsPaused()).IsFalse();
    }

    [TestCase]
    public async Task QuitPressedSceneUnpaused()
    {
        await _pauseTrigger();
        await _pressQuitButton();
        AssertThat(_runner.Scene().GetTree().IsPaused()).IsFalse();
    }

    // Helper method to set private fields
    private void SetPrivateField(object obj, string fieldName, object value)
    {
        var field = obj.GetType().GetField(fieldName,
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance);
        field?.SetValue(obj, value);
    }
}