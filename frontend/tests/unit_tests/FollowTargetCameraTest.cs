using GdUnit4;
using Godot;
using nuscutiesapp.tools;
using System;
using static GdUnit4.Assertions;

[TestSuite]
public partial class FollowTargetCameraTest
{
    [BeforeTest]
    public void Setup()
    {
        _camera = AutoFree(new FollowTargetCamera());
        _target = AutoFree(new Node2D());
    }

    private FollowTargetCamera _camera = null!;
    private Node2D _target = null!;

    [TestCase]
    public void FollowsTargetPosition()
    {
        _target.GlobalPosition = new Vector2(100, 200);
        _camera.Target = _target;

        _camera._Process(0);

        AssertThat(_camera.GlobalPosition).IsEqual(_target.GlobalPosition);
    }

    [TestCase]
    public void NullTargetDoesNotMove()
    {
        var initial = new Vector2(50, 50);
        _camera.GlobalPosition = initial;
        _camera.Target = null;

        _camera._Process(0);

        AssertThat(_camera.GlobalPosition).IsEqual(initial);
    }
}