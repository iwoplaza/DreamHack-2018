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

		private PathfindingRule m_clientRule;
		private List<TilePosition> m_currentPath;
		private TilePosition m_currentEndTile;
		private TileMap m_currentMap;

		private Thread m_pathfindingThread;

		public PathfindingAgent(PathfindingRule rule, TileMap map)
		{
            CurrentStatus = PathfindingStatus.PATH_FINISHED;

            m_currentMap = map;
			m_clientRule = rule;
			m_currentPath = new List<TilePosition>();
			map.RegisterEventHandler(TileMapInterruption, TileMapEvent.TILEMAP_MODIFIED);
		}

		public void GeneratePath(TilePosition from, TilePosition to)
		{
			CurrentStatus = PathfindingStatus.GENERATING_PATH;

			if(m_pathfindingThread != null)
			{
				m_pathfindingThread.Abort();
				m_currentPath.Clear();
			}

			m_pathfindingThread = new Thread(() => PathThread(from, to));
			m_pathfindingThread.Start();
		}	

		private void PathThread(TilePosition from, TilePosition to)
        {			
			m_currentPath = Internal.Pathfinding.FindPath(m_clientRule, m_currentMap, from, to);
			if(m_currentPath.Count > 0)
			{
				CurrentStatus = PathfindingStatus.HAS_PATH;
			}
			else
			{
				CurrentStatus = PathfindingStatus.PATH_FINISHED;
			}
		}

		public TilePosition GetNextTile()
		{
			if(m_currentPath.Count > 0)
			    return m_currentPath[0];
            return null;
		}

        public TilePosition PopTile()
        {
            TilePosition currentTile = m_currentPath[0];
            m_currentPath.RemoveAt(0);
            if (m_currentPath.Count == 0)
            {
                CurrentStatus = PathfindingStatus.PATH_FINISHED;
            }
            return currentTile;
        }

		public bool CurrentPathContainsTilePos(TilePosition pos)
		{
			foreach(TilePosition position in m_currentPath)
			{
				if(position == pos)
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
			if(CurrentPathContainsTilePos(interuptPosition) && CurrentStatus == PathfindingStatus.HAS_PATH)
            {
				GeneratePath(m_currentPath[0], m_currentEndTile);
			}
		}
	}
}
