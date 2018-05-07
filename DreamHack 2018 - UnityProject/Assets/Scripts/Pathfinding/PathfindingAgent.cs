using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Game.Pathfinding.Internal;

namespace Game.Pathfinding
{
	public class LivingPathfindingAgent
	{
		private Queue<TilePosition> m_currentPath;

		public bool HasTarget { get { return m_currentPath.Count > 0; } }

		private Living owner;

		public void MoveTo(Living livingObj, TilePosition start, TilePosition target){
			//m_currentPath = livingObj.Pathfinding.FindPath()
		}
	}
}
