using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Noise;
using Game;
using Game.TileObjects;

namespace Game.Environment
{
	public class GameEnvironment : MonoBehaviour
    {
		public Vector2Int WorldSize { get; set; }
		public string WorldSeed { get; set; }
		public float CliffThreshold { get; set; }
		
		public MeshChunk[,] Chunks { get; private set; } 

		public Vector2Int ChunkCount { get; private set; }
		public Vector2Int ChunkSize { get; set; }

		[SerializeField] FractalChain m_baseMap;
		[SerializeField] FractalChain m_metalMap;
		[SerializeField] FractalChain m_vegetationMap;

		[SerializeField] Material m_mapMaterial;

		TileMap m_tileMap;

		public void GenerateMap()
		{
			m_tileMap = WorldController.Instance.MainState.TileMap;
			m_baseMap.GenerateMap(WorldSize, WorldSeed);
			m_metalMap.GenerateMap(WorldSize, WorldSeed);
			m_vegetationMap.GenerateMap(WorldSize, WorldSeed);

			ChunkCount = new Vector2Int(Mathf.CeilToInt((float)WorldSize.x/ChunkSize.x), Mathf.CeilToInt((float)WorldSize.y/ChunkSize.y));
			
			Chunks = new MeshChunk[ChunkCount.x, ChunkCount.y];			

			for(int x = 0; x < WorldSize.x; x++)
			{
				for(int y = 0; y < WorldSize.y; y++)
				{
					if(x % ChunkSize.x == 0)
					{
						if(y % ChunkSize.y == 0)
						{
                            Vector2Int _chunkSize = new Vector2Int(ChunkSize.x, ChunkSize.y);
							_chunkSize.x = (x + ChunkSize.x < WorldSize.x) ? ChunkSize.x : (ushort)(WorldSize.x - x);
							_chunkSize.y = (y + ChunkSize.y < WorldSize.y) ? ChunkSize.y : (ushort)(WorldSize.y - y);
							GameObject chunkObj = Instantiate(Resources.ChunkPrefab);
							chunkObj.GetComponent<MeshChunk>().Initialize(this, new TilePosition(x,y), new TilePosition(x/ChunkSize.x, y/ChunkSize.y), _chunkSize, m_mapMaterial);
							Chunks[x/ChunkSize.y, y/ChunkSize.y] = chunkObj.GetComponent<MeshChunk>();
						}
					}
					if(m_baseMap.CurrentNoise[x,y] > CliffThreshold)
					{
						Chunks[Mathf.FloorToInt((float)x/ChunkSize.x),Mathf.FloorToInt((float)y/ChunkSize.y)]
						.CliffMap[x%ChunkSize.x, y%ChunkSize.y] = true;
					}
				}
			}
			for(int x = 0; x < ChunkCount.x; x++)
			{
				for(int y = 0; y < ChunkCount.y; y++)
				{
					Chunks[x, y].GenerateMeshMap();
					Chunks[x, y].CombineMeshes();
				}
			}
		}

		public float GetMetalAvailability(TilePosition position)
		{
			return m_metalMap.CurrentNoise[position.X, position.Z];
		}
	}
}