using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using System.Threading;
using Game.Pathfinding.Internal;
using Game.Utility;

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

        public TilePosition EndGoal
        {
            get
            {
                if (CurrentStatus == PathfindingStatus.HAS_PATH)
                {
                    return m_currentPath[m_currentPath.Count - 1];
                }
                return null;
            }
        }

		private PathfindingRule m_clientRule;
		private List<TilePosition> m_currentPath;
		private TilePosition m_currentEndTile;
		private TileMap m_currentMap;
		private Thread m_pathfindingThread;
        /// <summary>
        /// Used for setting the CurrentStatus through the PathfindingThread.
        /// </summary>
        private ThreadQueue<PathfindingStatus> m_pathThreadStatusChanges;

        public delegate void StatusChangeHandler(PathfindingStatus newStatus);
        private StatusChangeHandler m_statusChangeHandlers;

        public PathfindingAgent(PathfindingRule rule, TileMap map)
		{
            CurrentStatus = PathfindingStatus.PATH_FINISHED;
            m_pathThreadStatusChanges = new ThreadQueue<PathfindingStatus>();

            m_currentMap = map;
			m_clientRule = rule;
			m_currentPath = new List<TilePosition>();
			map.RegisterEventHandler(TileMapInterruption, TileMapEvent.TILEMAP_MODIFIED);
		}

        public void Update()
        {
            PathfindingStatus newStatus;
            while(m_pathThreadStatusChanges.TryDequeue(out newStatus))
            {
                CurrentStatus = newStatus;
            }
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

        public void GeneratePartialPath(TilePosition from, TilePosition to, int steps)
        {
            m_currentEndTile = to;

            CurrentStatus = PathfindingStatus.GENERATING_PATH;

            if (m_pathfindingThread != null)
            {
                m_pathfindingThread.Abort();
            }

            m_pathfindingThread = new Thread(() => PartialPathThread(from, to, steps));
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
                m_pathThreadStatusChanges.Enqueue(PathfindingStatus.HAS_PATH);
			}
			else
			{
                m_pathThreadStatusChanges.Enqueue(PathfindingStatus.PATH_FINISHED);
            }
		}

        private void PartialPathThread(TilePosition from, TilePosition to, int steps)
        {
            m_currentPath = Internal.Pathfinding.FindPath(m_clientRule, m_currentMap, from, to);
            if (m_currentPath.Count > 0)
            {
                if (steps < m_currentPath.Count)
                {
                    m_currentPath.RemoveRange(steps, m_currentPath.Count - steps);
                }
                m_pathThreadStatusChanges.Enqueue(PathfindingStatus.HAS_PATH);
            }
            else
            {
                m_pathThreadStatusChanges.Enqueue(PathfindingStatus.PATH_FINISHED);
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
