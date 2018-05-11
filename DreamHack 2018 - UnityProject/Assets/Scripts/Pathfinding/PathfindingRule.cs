using Game;

namespace Game.Pathfinding
{
    public interface IPathfindingRule
    {
        bool CanGoIntoFrom(Tile tile, Pathfinding.MovementDirection dir);
        bool CanComeOutOfTowards(Tile tile, Pathfinding.MovementDirection dir);
        bool CanSkimThrough(Tile tile);
        bool IsProperEndGoal(Tile tile);
    }
}