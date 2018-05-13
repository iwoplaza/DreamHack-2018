using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Noise;
using Game;
using Game.TileObjects;
using System.Threading;
using Game.Utility;
using System.Xml.Linq;

namespace Game.Environment
{
	public class GameEnvironment : MonoBehaviour
    {
		public Vector2Int WorldSize { get; set; }
		public string WorldSeed { get; set; }
		public float CliffThreshold { get; set; }
		
		public MeshChunk[,] Chunks { get; private set; } 

		public float EmptyRadius { get; set; }

		public Vector2Int ChunkCount { get; private set; }
		public Vector2Int ChunkSize { get; set; }

        [SerializeField]
        MetalMap m_metalMap;

        [SerializeField] FractalChain m_baseMap;
		[SerializeField] FractalChain m_vegetationMap;
		[SerializeField] FractalChain m_rockMap;
		[SerializeField] Material m_mapMaterial;

        public MetalMap MetalMap { get { return m_metalMap; } }

		struct GameObjectToAdd
		{
			public GameObject ToAdd { get; private set; }
			public TilePosition AddPosition { get; private set; }

			public GameObjectToAdd(GameObject _ToAdd, TilePosition _AddPosition)
			{
				ToAdd = _ToAdd;
				AddPosition = _AddPosition;
			}
		}

		private Queue<GameObjectToAdd> m_setTransformToChunkQueue;

		public struct CullingStream
		{
			public TilePosition ToCull { get; private set; }
			public bool DisableObject { get; private set; }

			public CullingStream(TilePosition _ToCull, bool _DisableObject)
			{
				ToCull = _ToCull;
				DisableObject = _DisableObject;
			}
		}		

		private ThreadQueue<CullingStream> m_cullQueue;

		private bool m_startCulling;

		TileMap m_tileMap;

		public void AddGameobjectToChunk(GameObject toAdd, TilePosition position)
		{
			if(m_setTransformToChunkQueue == null)
			{
				m_setTransformToChunkQueue = new Queue<GameObjectToAdd>();
			}
			m_setTransformToChunkQueue.Enqueue(new GameObjectToAdd(toAdd,position));
		}

		public void Update()
		{
			if(Chunks == null)
				return;

			if(m_setTransformToChunkQueue != null)
            {
				while(m_setTransformToChunkQueue.Count > 0)
				{
					GameObjectToAdd newObj = m_setTransformToChunkQueue.Dequeue();
					newObj.ToAdd.transform.parent = Chunks[Mathf.FloorToInt((float)newObj.AddPosition.X/ChunkSize.x),
													Mathf.FloorToInt((float)newObj.AddPosition.Z/ChunkSize.y)].gameObject.transform;
				}
			}

			if(m_startCulling)
			{
				while(m_cullQueue.Count > 0)
				{
					
					CullingStream toCull = m_cullQueue.Dequeue();
					if(toCull.ToCull != null){
						if(toCull.DisableObject && Chunks[toCull.ToCull.X,toCull.ToCull.Z].gameObject.activeSelf)
						{
							Chunks[toCull.ToCull.X,toCull.ToCull.Z].gameObject.SetActive(false);
						}
						else
						{
							if(!toCull.DisableObject && !Chunks[toCull.ToCull.X,toCull.ToCull.Z].gameObject.activeSelf)
								Chunks[toCull.ToCull.X,toCull.ToCull.Z].gameObject.SetActive(true);
						}
					}
				}
			}
		}

		public void AddCullQueue(TilePosition _object, bool _disable)
		{
			m_cullQueue.Enqueue(new CullingStream(_object, _disable));
		}

		public void GenerateMap()
		{
			m_tileMap = WorldController.Instance.MainState.TileMap;
			m_baseMap.GenerateMap(WorldSize, WorldSeed);
            MetalMap.GenerateMap(WorldSize, WorldSeed);
			m_vegetationMap.GenerateMap(WorldSize, WorldSeed);
			m_rockMap.GenerateMap(WorldSize, WorldSeed);

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
					if((m_baseMap.CurrentNoise[x,y] > CliffThreshold 
						&& Vector2.Distance(new Vector2((float)WorldSize.x/2,(float)WorldSize.y/2), new Vector2(x,y)) > EmptyRadius )
						|| (x == 0 || y == 0 || x == WorldSize.x - 1 || y == WorldSize.y - 1))
					{
						Chunks[Mathf.FloorToInt((float)x/ChunkSize.x),Mathf.FloorToInt((float)y/ChunkSize.y)]
						.CliffMap[x%ChunkSize.x, y%ChunkSize.y] = true;
						m_tileMap.TileAt(new TilePosition(x,y)).SetHasCliff(true);
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
			m_cullQueue = new ThreadQueue<CullingStream>();
			m_startCulling = true;
		}

		public void PopulateMapForNewWorld()
		{
			for(int x = 0; x < WorldSize.x; x++)
			{
				for(int y = 0; y < WorldSize.y; y++)
				{
					if(m_baseMap.CurrentNoise[x, y] <= CliffThreshold)
					{
						if(Vector2.Distance(new Vector2((float)WorldSize.x/2,(float)WorldSize.y/2), new Vector2(x,y)) > EmptyRadius)
						{
							if(MetalMap.MetalFractalChain.CurrentNoise[x,y] < 0.2F){
								if(Random.Range(0.00f, 1.00f) < m_vegetationMap.CurrentNoise[x,y])
								{						
									m_tileMap.InstallAt(new GreenVegetation(), new TilePosition(x,y));
								}
							}else
							{
								if(Random.Range(0.00f, 1.00f) < m_vegetationMap.CurrentNoise[x,y])
								{						
									m_tileMap.InstallAt(new DesertVegetation(), new TilePosition(x,y));
								}
							}		
						}
						
					}

					if(m_baseMap.CurrentNoise[x, y] <= CliffThreshold)
					{
						if(Random.Range(0.00f,1.00f) < m_rockMap.CurrentNoise[x, y])
						{
							if(Vector2.Distance(new Vector2((float)WorldSize.x / 2, (float)WorldSize.y / 2), new Vector2(x, y)) < EmptyRadius)
							{
								
							}
							else
							{
								if(Random.Range(0.00f, 1.00f) > 0.65f)
								{
									m_tileMap.InstallAt(new LargeRock(), new TilePosition(x,y));
								}
								else
								{
									m_tileMap.InstallAt(new SmallRock(), new TilePosition(x,y));
								}
							}
						}
					}
				}
			}

            MetalMap.PopulateMapForNewWorld();
		}

        /// <summary>
        /// Called after either PopulateMapForNewWorld or Parse
        /// </summary>
        public void AfterSetup()
        {
            m_mapMaterial.SetFloat("_ResolutionX", WorldSize.x);
            m_mapMaterial.SetFloat("_ResolutionY", WorldSize.y);
            m_mapMaterial.SetTexture("_ResourceMap", MetalMap.MetalFractalChain.CurrentTexture);
        }

        public void Parse(XElement element)
        {
            XElement metalMapElement = element.Element("MetalMap");
            if (metalMapElement != null)
                MetalMap.Parse(metalMapElement);
        }

        public void Populate(XElement element)
        {
            XElement metalMapElement = new XElement("MetalMap");
            element.Add(metalMapElement);
            MetalMap.Populate(metalMapElement);
        }

		public float GetMetalAvailability(TilePosition position)
		{
			return MetalMap.MetalFractalChain.CurrentNoise[position.X, position.Z];
		}
	}
}