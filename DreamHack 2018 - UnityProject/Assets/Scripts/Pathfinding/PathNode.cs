using System.Collections.Generic;
using Game;

namespace Game.Pathfinding.Internal
{
    public class PathNode
    {
        public PathNode ParentNode { get; private set; }
        public float Weight { get; private set; }
        public TilePosition Location { get; private set; }

        public PathNode(PathNode parent, float weight, TilePosition loc)
        {
            ParentNode = parent;
            Location = loc;
        }

        public PathNode(float weight, TilePosition loc) : this(null, weight, loc) {}
    }
}