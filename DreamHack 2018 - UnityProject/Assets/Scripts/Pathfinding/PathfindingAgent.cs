using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using System.Threading;
using Game.Pathfinding.Internal;

namespace Game.Pathfinding
{
	public class PathfindingAgent
	{
		public PathfindingStatus CurrentStatus { get; private set; }

		private PathfindingRule clientRule;

		private List<Tile> m_currentPath;

		private Tile m_currentEndTile;

		private static TileMap currentMap;

		private Thread m_PathfindingThread;

		public PathfindingAgent(PathfindingRule rule, TileMap map)
		{
			currentMap = map;
			clientRule = rule;
			m_currentPath = new List<Tile>();
			map.RegisterEventHandler(TileMapInterruption, TileMapEvent.TILEMAP_MODIFIED);
		}

		public void GeneratePath(TilePosition from, TilePosition to)
		{
			CurrentStatus = PathfindingStatus.GENERATING_PATH;

			if(m_PathfindingThread != null)
			{
				m_PathfindingThread.Abort();
				m_currentPath.Clear();
			}

			m_PathfindingThread = new Thread(() => PathThread(from, to));
			m_PathfindingThread.Start();
		}	

		private void PathThread(TilePosition from, TilePosition to){			
			m_currentPath = Internal.Pathfinding.FindPath(clientRule, currentMap, from, to);
			if(m_currentPath.Count > 0)
			{
				CurrentStatus = PathfindingStatus.HAS_PATH;
			}
			else
			{
				CurrentStatus = PathfindingStatus.PATH_FINISHED;
			}
		}

		public Tile GetNextTile()
		{
			Tile currentTile = m_currentPath[0];
			m_currentPath.RemoveAt(0);
			if(m_currentPath.Count == 0){
				CurrentStatus = PathfindingStatus.PATH_FINISHED;
			}	
			return currentTile;
		}

		public bool CurrentPathContainsTilePos(TilePosition pos)
		{
			foreach(Tile tile in m_currentPath)
			{
				if(tile.Position == pos)
				{
					return true;
				}
			}

			return false;
		}

		public void TileMapInterruption(TilePosition interuptPosition)
		{
			if(m_currentPath == null || m_currentPath.Count == 0)
			{
				return;
			}
			if(CurrentPathContainsTilePos(interuptPosition) && CurrentStatus == PathfindingStatus.HAS_PATH){
				GeneratePath(m_currentPath[0].Position, m_currentEndTile.Position);
			}
		}
	}
}
