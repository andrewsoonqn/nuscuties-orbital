using GdUnit4;
using Godot;
using nuscutiesapp.tools;
using System.Threading.Tasks;
using static GdUnit4.Assertions;

[TestSuite]
public partial class FollowPlayerCameraIntegrationTest
{
    private const string PlayerPath = "Player";
    private const string CameraPath = "FollowPlayerCamera";

    private ISceneRunner _runner;
    [BeforeTest]
    public void Setup()
    {
        this._runner = ISceneRunner.Load(Paths.ActiveGame);
    }

    [TestCase]
    public async Task CameraFollowsPlayerInScene()
    {
        CharacterBody2D player = (CharacterBody2D)_runner.FindChild(PlayerPath);
        FollowTargetCamera camera = (FollowTargetCamera)_runner.FindChild(CameraPath);

        AssertThat(player).IsNotNull();
        AssertThat(camera).IsNotNull();

        camera.Target = player;

        player.GlobalPosition = new Vector2(100, 100);

        await ISceneRunner.SyncProcessFrame;
        await ISceneRunner.SyncProcessFrame;
        await ISceneRunner.SyncProcessFrame;
        await ISceneRunner.SyncProcessFrame;

        AssertThat(camera.GlobalPosition).IsEqual(player.GlobalPosition);

        player.GlobalPosition = new Vector2(250, 300);

        await ISceneRunner.SyncProcessFrame;

        AssertThat(camera.GlobalPosition).IsEqual(player.GlobalPosition);
    }
}