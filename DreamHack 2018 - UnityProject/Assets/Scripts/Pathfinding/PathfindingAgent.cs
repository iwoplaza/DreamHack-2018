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
        private PathfindingStatus m_currentStatus;

        public PathfindingStatus CurrentStatus
        {
            get { return m_currentStatus; }
            private set
            {
                m_currentStatus = value;
                if (m_statusChangeHandlers != null)
                    m_statusChangeHandlers(m_currentStatus);
            }
        }

		private PathfindingRule m_clientRule;
		private List<TilePosition> m_currentPath;
		private TilePosition m_currentEndTile;
		private TileMap m_currentMap;
		private Thread m_pathfindingThread;

        public delegate void StatusChangeHandler(PathfindingStatus newStatus);
        private StatusChangeHandler m_statusChangeHandlers;

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
			m_currentEndTile = to;
			
			CurrentStatus = PathfindingStatus.GENERATING_PATH;

			if(m_pathfindingThread != null)
			{
				m_pathfindingThread.Abort();
			}

			m_pathfindingThread = new Thread(() => PathThread(from, to));
			m_pathfindingThread.Start();
		}

        public void CancelPath()
        {
            m_currentPath.Clear();
            CurrentStatus = PathfindingStatus.PATH_FINISHED;
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

		private bool CurrentPathIsNearTilePos(TilePosition pos)
		{
			foreach(TilePosition position in m_currentPath)
			{
				if(position == pos || TileDistance(pos, position) < 4)
				{
					return true;
				}
			}

			return false;
		}

		private int TileDistance(TilePosition pos1, TilePosition pos2)
		{
			return Mathf.Max(Mathf.Abs(pos1.X - pos2.X),Mathf.Abs(pos1.Z - pos2.Z));
		}

		public void TileMapInterruption(TilePosition interuptPosition)
		{
			if(m_currentPath == null || m_currentPath.Count == 0)
			{
				return;
			}
			if(CurrentPathIsNearTilePos(interuptPosition) && CurrentStatus == PathfindingStatus.HAS_PATH)
            {				
				GeneratePath(GetNextTile(), m_currentEndTile);
			}
		}

        public void RegisterStatusChangeHandler(StatusChangeHandler handler)
        {
            m_statusChangeHandlers += handler;
        }
	}
}
