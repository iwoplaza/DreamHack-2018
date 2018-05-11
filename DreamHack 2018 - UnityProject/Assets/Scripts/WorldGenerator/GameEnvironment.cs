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
		public TilePosition ChunkSize { get; set; }

		[SerializeField]FractalChain m_baseMap;
		[SerializeField]FractalChain m_metalMap;

		[SerializeField]Material m_mapMaterial;

		TileMap m_TileMap;

		public void GenerateMap()
		{
			m_TileMap = WorldController.Instance.MainState.TileMap;
			m_baseMap.GenerateMap(WorldSize, WorldSeed);
			m_metalMap.GenerateMap(WorldSize, WorldSeed);

			ChunkCount = new TilePosition(Mathf.CeilToInt((float)WorldSize.x/ChunkSize.X),Mathf.CeilToInt((float)WorldSize.y/ChunkSize.Z));
			
			Chunks = new MeshChunk[ChunkCount.X,ChunkCount.Z];			

			for(int x = 0; x < WorldSize.x; x++)
			{
				for(int y = 0; y < WorldSize.y; y++)
				{
					if(x % ChunkSize.X == 0)
					{
						if(y % ChunkSize.Z == 0)
						{
							TilePosition _chunkSize = new TilePosition(ChunkSize.X,ChunkSize.Z);
							_chunkSize.X = (x + ChunkSize.X < WorldSize.x) ? ChunkSize.X : (ushort)(WorldSize.x - x);
							_chunkSize.Z = (y + ChunkSize.Z < WorldSize.y) ? ChunkSize.Z : (ushort)(WorldSize.y - y);
							GameObject chunkObj = Instantiate(Resources.ChunkPrefab);
							chunkObj.GetComponent<MeshChunk>().Initialize(this, new TilePosition(x,y), new TilePosition(x/ChunkSize.X,y/ChunkSize.Z), _chunkSize, m_mapMaterial);
							Chunks[x/ChunkSize.X,y/ChunkSize.Z] = chunkObj.GetComponent<MeshChunk>();
						}
						
					}
					if(m_baseMap.CurrentNoise[x,y] > CliffThreshold)
					{
						Chunks[Mathf.FloorToInt((float)x/ChunkSize.X),Mathf.FloorToInt((float)y/ChunkSize.Z)]
						.CliffMap[x%ChunkSize.X,y%ChunkSize.Z] = true;
					}
				}
			}
			for(int x = 0; x < ChunkCount.X; x++)
			{
				for(int y = 0; y < ChunkCount.Z; y++)
				{
					Chunks[x,y].GenerateMeshMap();
					Chunks[x,y].CombineMeshes();
				}
			}
		}
	}
}
