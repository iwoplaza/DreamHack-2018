
using Game.Pathfinding;
using Game;
using System;

namespace Game.Pathfinding.Rules
{
    public class BasicRule : IPathfindingRule
    {
        public bool CanGoIntoFrom(Tile tile, MovementDirection dir)
        {
            return tile != null && tile.CanGoIntoFrom(dir);
        }

        public bool CanComeOutOfTowards(Tile tile, MovementDirection dir)
        {
            return tile != null && tile.CanComeOutOfTowards(dir);
        }

        public bool IsProperEndGoal(Tile tile)
        {
            return tile != null && !tile.IsImpenetrable;
        }
    }
}