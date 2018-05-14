using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pathfinding.Rules
{
    public class WorkerRule : IPathfindingRule
    {
        public bool CanGoIntoFrom(Tile tile, MovementDirection dir)
        {
            TileProp installedObject = tile.GetProp(PropType.OBJECT);

            if(!tile.HasCliff && installedObject is TileObjects.DoorTileObject)
                return true;
            else
                return tile != null && tile.CanGoIntoFrom(dir) && !tile.HasCliff;
        }

        public bool CanComeOutOfTowards(Tile tile, MovementDirection dir)
        {
            TileProp installedObject = tile.GetProp(PropType.OBJECT);

            if (!tile.HasCliff && installedObject is TileObjects.DoorTileObject)
                return true;
            else
                return tile != null && tile.CanComeOutOfTowards(dir);
        }

        public bool CanSkimThrough(Tile tile)
        {
            return tile != null && tile.CanSkimThrough() && !tile.HasCliff;
        }

        public bool IsProperEndGoal(Tile tile)
        {
            return tile != null && !tile.IsImpenetrable && !tile.HasCliff;
        }
    }
}