using Godot;
using nuscutiesapp.active;
using System;

public partial class ActiveGame : Node2D
{
    private ActiveDungeonEventManager _eventManager;
    public override void _Ready()
    {
        var camera = new FollowTargetCamera();
        camera.Name = "FollowPlayerCamera";
        camera.Target = this.GetNode<CharacterBody2D>("Player");
        camera.Zoom = new Vector2(10, 10);
        this.AddChild(camera);

        _eventManager = GetNode<ActiveDungeonEventManager>("/root/ActiveDungeonEventManager");
        _eventManager.GameStarted();
        
        base._Ready();
    }

    public int GetMaxWaves()
    {
        return GetNode<EnemySpawner>("EnemySpawner").Waves;
    }
}