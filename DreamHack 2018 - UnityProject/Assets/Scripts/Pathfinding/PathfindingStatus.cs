namespace Game.Pathfinding
{
    /// <summary>
    /// Don't change the order, since it's serialized into integers and back.
    /// </summary>
    public enum PathfindingStatus
    {
		GENERATING_PATH,
		HAS_PATH,
		PATH_FINISHED
	}
}