using Godot;
using System;
using System.Collections.Generic;

public partial class ActiveDungeonEventManager : Node
{
    public void PlayerDied()
    {
        GD.Print("dieddddddd");
        EmitSignal(SignalName.PlayerDeathEvent);
    }

    [Signal]
    public delegate void PlayerDeathEventEventHandler();
    
    public void GameWon()
    {
        GD.Print("wonnnnnnn");
        EmitSignal(SignalName.GameWonEvent);
    }

    [Signal]
    public delegate void GameWonEventEventHandler();
}
