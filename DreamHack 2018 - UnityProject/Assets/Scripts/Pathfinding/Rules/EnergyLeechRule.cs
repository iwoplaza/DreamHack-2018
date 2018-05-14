
namespace Game.Pathfinding.Rules
{
    public class EnergyLeechRule : IPathfindingRule
    {
        public bool CanGoIntoFrom(Tile tile, MovementDirection dir)
        {
            return true;
        }

        public bool CanComeOutOfTowards(Tile tile, MovementDirection dir)
        {
            return true;
        }

        public bool CanSkimThrough(Tile tile)
        {
            return tile != null && tile.CanSkimThrough() && !tile.HasCliff;
        }

        public bool IsProperEndGoal(Tile tile)
        {
            TileProp installedObject = tile.GetProp(PropType.OBJECT);

            if (!tile.HasCliff && (installedObject is TileObjects.MainGeneratorTileObject))
                return true;
            else
                return tile != null && !tile.IsImpenetrable && !tile.HasCliff;
        }
    }
}