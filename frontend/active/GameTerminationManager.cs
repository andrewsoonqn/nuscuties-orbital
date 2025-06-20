using Godot;
using nuscutiesapp.tools;
using System;
using System.Threading;
using System.Threading.Tasks;

public partial class GameTerminationManager : Node
{
    private ActiveDungeonEventManager _eventManager;

    public override void _Ready()
    {
        _eventManager = GetNode<ActiveDungeonEventManager>("/root/ActiveDungeonEventManager");
        _eventManager.GameWonEvent += EventManagerOnGameEndEvent;
        _eventManager.GameLostEvent += EventManagerOnGameEndEvent;
    }

    private async void EventManagerOnGameEndEvent()
    {
        await Task.Delay(1000);
        GetTree().ChangeSceneToFile(Paths.ActiveEndScene);
    }
}