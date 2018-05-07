using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Game.Pathfinding.Internal;

namespace Game.Pathfinding
{
	public class PathfindingAgent
	{
		private Queue<Tile> m_currentPath;

		public bool HasTarget { get { return m_currentPath.Count > 0; } }

		private PathfindingRule clientRule;

		private static TileMap currentMap;

		public PathfindingAgent(PathfindingRule rule, TileMap map)
		{
			currentMap = map;
			clientRule = rule;
			m_currentPath = new Queue<Tile>();
		}

		public void GeneratePath(TilePosition from, TilePosition to)
		{
			m_currentPath = Internal.Pathfinding.FindPath(clientRule, currentMap, from, to);
		}

		public Tile GetNextTile()
		{
			return m_currentPath.Dequeue();
		}
	}
}
