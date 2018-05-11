
using Game.Pathfinding;
using Game;
using System;

public class BasicRule : PathfindingRule
{
    public bool CanPassThrough(Tile tile, Direction dir)
    {
        return tile.CanPassThrough(dir);
    }

    public bool IsProperEndGoal(Tile tile)
    {
        return tile != null && !tile.IsImpenetrable;
    }
}