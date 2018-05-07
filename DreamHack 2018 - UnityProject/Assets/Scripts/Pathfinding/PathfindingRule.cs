using Game;

namespace Game.Pathfinding
{
    public interface PathfindingRule
    {
        bool CanPassThrough(Tile tile, Direction dir);
    }
}