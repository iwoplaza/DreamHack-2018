
namespace Game.Pathfinding.Rules
{
    public class BasicRule : IPathfindingRule
    {
        public bool CanGoIntoFrom(Tile tile, MovementDirection dir)
        {
            return tile != null && tile.CanGoIntoFrom(dir) && !tile.HasCliff;
        }

        public bool CanComeOutOfTowards(Tile tile, MovementDirection dir)
        {
            return tile != null && tile.CanComeOutOfTowards(dir);
        }

        public bool CanSkimThrough(Tile tile)
        {
            return tile != null && tile.CanSkimThrough()  && !tile.HasCliff;
        }

        public bool IsProperEndGoal(Tile tile)
        {
            return tile != null && !tile.IsImpenetrable  && !tile.HasCliff;
        }
    }
}