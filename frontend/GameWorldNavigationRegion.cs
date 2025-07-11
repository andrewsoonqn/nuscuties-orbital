using Godot;
using System;

public partial class GameWorldNavigationRegion : NavigationRegion2D
{
    public override void _Ready()
    {
        BakeNavigationPolygon();
        base._Ready();
    }
}
