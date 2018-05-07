
using Game.Pathfinding;
using Game;

public class BasicRule : PathfindingRule{
    public bool CanPassThrough(Tile tile, Direction dir){
        if(tile.HasObject){
            return false;
        }
        return true;
    }
}