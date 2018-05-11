using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Noise;
using Game;
using Game.TileObjects;

namespace Game.Environment
{
	public class GameEnvironment : MonoBehaviour {
		
		public Vector2Int WorldSize { get; set; }
		public string WorldSeed { get; set; }
		public float CliffThreshold { get; set; }
		
		public MeshChunk[,] Chunks { get; private set; } 

		public TilePosition ChunkCount { get; private set; }

		[SerializeField]FractalChain m_baseMap;
		[SerializeField]FractalChain m_metalMap;

		TileMap m_TileMap;

		public void GenerateMap()
		{
			m_TileMap = WorldController.Instance.MainState.TileMap;
			m_baseMap.GenerateMap(WorldSize, WorldSeed);
			m_metalMap.GenerateMap(WorldSize, WorldSeed);

			for(int x = 0; x < WorldSize.x; x++)
			{
				for(int y = 0; y < WorldSize.y; y++)
				{
					if(m_baseMap.CurrentNoise[x,y] < CliffThreshold)
					{
						m_TileMap.InstallAt(new CliffObject(), new TilePosition(x,y));
					}
				}
			}
		}
	}
}
