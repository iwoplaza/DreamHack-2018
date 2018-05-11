using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Game.Environment;

namespace Game.Environment
{
	public class MeshChunk : MonoBehaviour {
		public struct MeshCombine
		{
			public Mesh ToAdd { get; set; }
			public TilePosition Position { get; set; }
			public Quaternion Rotation { get; set; }

			public MeshCombine(Mesh _toAdd, TilePosition _position, Vector3 _rotation)
			{
				ToAdd = _toAdd;
				Position = _position;
				Rotation = Quaternion.Euler(_rotation);
			}

			public MeshCombine(Mesh _toAdd, TilePosition _position, Quaternion _rotation)
			{
				ToAdd = _toAdd;
				Position = _position;
				Rotation = _rotation;
			}
		}

		public struct Neighbors
		{			
			public bool XPos { get; set; }
			public bool XPosZPos { get; set; }
			public bool XPosZNeg { get; set; }
			public bool XNeg { get; set; }
			public bool XNegZPos { get; set; }
			public bool XNegZNeg { get; set; }
			public bool ZPos { get; set; }
			public bool ZNeg { get; set; }

			public int NeighborCount { get; set; }
		}

		public GameEnvironment Owner { get; private set; }
		public TilePosition ChunkPosition { get; private set; }
		public TilePosition ChunkSize { get; private set; }
		public bool[,] CliffMap { get; set; }
		public List<MeshCombine> MeshesToAdd { get; private set; }
		public Material MeshMaterial { get; private set; }

		public MeshChunk(GameEnvironment _owner, TilePosition _chunkPosition, TilePosition _chunkSize, Material _material)
		{
			Owner = _owner;
			ChunkPosition = _chunkPosition;
			ChunkSize = _chunkSize;
			CliffMap = new bool[_chunkSize.X,_chunkSize.Z];
			MeshMaterial = _material;
			gameObject.AddComponent<MeshFilter>();
			gameObject.AddComponent<MeshRenderer>();
			transform.position = ChunkSize.Vector3 + new Vector3(0.5f,0,0.5f);
		}

		public void GenerateMeshMap()
		{
			MeshesToAdd = new List<MeshCombine>();
			for(int x = 0; x < ChunkSize.X; x++)
			{
				for(int z = 0; z < ChunkSize.Z; z++)
				{
					TilePosition addPosition = new TilePosition(x,z);
					if(!CliffMap[x,z])
					{
						MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.GROUND)
										, addPosition, Vector3.zero));
					}
					else
					{
						Neighbors tileNeighbor = GetNeighboringTile(addPosition);
						if(tileNeighbor.NeighborCount == 8)
						{
							MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.GROUND_CLIFF)
										, addPosition, Vector3.zero));
						}
						else if(tileNeighbor.NeighborCount == 7)
						{

							// CASE:
							//	101
							//	1P1
							//	111

							if(!tileNeighbor.XNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT)
										, addPosition, new Vector3(0,270,0)));
							}
							else if(!tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT)
										, addPosition, new Vector3(0,90,0)));
							}
							else if(!tileNeighbor.ZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT)
										, addPosition, new Vector3(0,180,0)));
							}
							else if(!tileNeighbor.ZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT)
										, addPosition, new Vector3(0,0,0)));
							}

							// CASE:
							//	011
							//	1P1
							//	111

							else if(!tileNeighbor.XNegZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_CORNER_CASE1)
										, addPosition, new Vector3(0,180,0)));
							}
							else if(!tileNeighbor.XNegZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_CORNER_CASE1)
										, addPosition, new Vector3(0,270,0)));
							}
							else if(!tileNeighbor.XPosZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_CORNER_CASE1)
										, addPosition, new Vector3(0,90,0)));
							}
							else if(!tileNeighbor.XPosZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_CORNER_CASE1)
										, addPosition, new Vector3(0,0,0)));
							}
						}
						else if(tileNeighbor.NeighborCount == 6)
						{

							// CASE:
							//	110
							//	1P1
							//	011

							if(!tileNeighbor.XNegZNeg && !tileNeighbor.XPosZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_CORNER_CASE22)
										, addPosition, new Vector3(0,0,0)));
							}
							else if(!tileNeighbor.XNegZPos && !tileNeighbor.XPosZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_CORNER_CASE22)
										, addPosition, new Vector3(0,90,0)));
							}

							//	CASE:
							//	001
							//	1P1
							//	111

							else if ((!tileNeighbor.XNegZPos || !tileNeighbor.XPosZPos) && !tileNeighbor.ZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT)
										, addPosition, new Vector3(0,0,0)));
							}
							else if ((!tileNeighbor.XNegZNeg || !tileNeighbor.XPosZNeg) && !tileNeighbor.ZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT)
										, addPosition, new Vector3(0,180,0)));
							}
							else if ((!tileNeighbor.XPosZNeg || !tileNeighbor.XPosZPos) && !tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT)
										, addPosition, new Vector3(0,90,0)));
							}
							else if ((!tileNeighbor.XNegZPos || !tileNeighbor.XNegZNeg) && !tileNeighbor.XNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT)
										, addPosition, new Vector3(0,270,0)));
							}
							
							// CASE:
							//	010
							//	1P1
							//	111
							
							else if (!tileNeighbor.XNegZPos && !tileNeighbor.XPosZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_CORNER_CASE21)
										, addPosition, new Vector3(0,0,0)));
							}
							else if (!tileNeighbor.XNegZNeg && !tileNeighbor.XPosZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_CORNER_CASE21)
										, addPosition, new Vector3(0,180,0)));
							}
							else if (!tileNeighbor.XPosZNeg && !tileNeighbor.XPosZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_CORNER_CASE21)
										, addPosition, new Vector3(0,90,0)));
							}
							else if (!tileNeighbor.XNegZPos && !tileNeighbor.XNegZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_CORNER_CASE21)
										, addPosition, new Vector3(0,270,0)));
							}

							// CASE:
							//	011
							//	1P0
							//	111
							
							else if (!tileNeighbor.ZPos && !tileNeighbor.XNegZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE1)
										, addPosition, new Vector3(0,0,0)));
							}
							else if (!tileNeighbor.ZPos && !tileNeighbor.XPosZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE2)
										, addPosition, new Vector3(0,0,0)));
							}
							else if (!tileNeighbor.ZNeg && !tileNeighbor.XPosZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE1)
										, addPosition, new Vector3(0,180,0)));
							}
							else if (!tileNeighbor.ZNeg && !tileNeighbor.XNegZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE2)
										, addPosition, new Vector3(0,180,0)));
							}
							else if (!tileNeighbor.XPos && !tileNeighbor.XNegZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE1)
										, addPosition, new Vector3(0,90,0)));
							}
							else if (!tileNeighbor.XPos && !tileNeighbor.XNegZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE2)
										, addPosition, new Vector3(0,90,0)));
							}
							else if (!tileNeighbor.XNeg && !tileNeighbor.XPosZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE1)
										, addPosition, new Vector3(0,270,0)));
							}
							else if (!tileNeighbor.XNeg && !tileNeighbor.XPosZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE2)
										, addPosition, new Vector3(0,270,0)));
							}
						}
						else if(tileNeighbor.NeighborCount == 5)
						{

							// CASE:
							//	000
							//	1P1
							//	111

							if(!tileNeighbor.XNegZPos && !tileNeighbor.XPosZPos && !tileNeighbor.ZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT)
										, addPosition, new Vector3(0,0,0)));
							}
							else if(!tileNeighbor.XPosZNeg && !tileNeighbor.XPosZPos && !tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT)
										, addPosition, new Vector3(0,90,0)));
							}
							else if(!tileNeighbor.XNegZNeg && !tileNeighbor.XPosZNeg && !tileNeighbor.ZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT)
										, addPosition, new Vector3(0,180,0)));
							}
							else if(!tileNeighbor.XNegZPos && !tileNeighbor.XNegZNeg && !tileNeighbor.XNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT)
										, addPosition, new Vector3(0,270,0)));
							}

							// CASE:
							//	001
							//	1P0
							//	111

							if((!tileNeighbor.XNegZPos || !tileNeighbor.XPosZNeg) && !tileNeighbor.ZPos && !tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_SHARPCORNER)
										, addPosition, new Vector3(0,0,0)));
							}
							else if((!tileNeighbor.XPosZPos || !tileNeighbor.XNegZNeg) && !tileNeighbor.ZNeg && !tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_SHARPCORNER)
										, addPosition, new Vector3(0,90,0)));
							}
							else if((!tileNeighbor.XNegZPos || !tileNeighbor.XPosZNeg) && !tileNeighbor.ZNeg && !tileNeighbor.XNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_SHARPCORNER)
										, addPosition, new Vector3(0,180,0)));
							}
							else if((!tileNeighbor.XPosZPos || !tileNeighbor.XNegZNeg) && !tileNeighbor.ZPos && !tileNeighbor.XNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_SHARPCORNER)
										, addPosition, new Vector3(0,270,0)));
							}

							// CASE:
							//	001
							//	111
							//	110

							if(!tileNeighbor.ZPos && !tileNeighbor.XNegZPos && !tileNeighbor.XPosZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE1)
										, addPosition, new Vector3(0,0,0)));
							}
							if(!tileNeighbor.ZPos && !tileNeighbor.XPosZPos && !tileNeighbor.XNegZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE2)
										, addPosition, new Vector3(0,0,0)));
							}
							if(!tileNeighbor.XPos && !tileNeighbor.XPosZPos && !tileNeighbor.XNegZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE1)
										, addPosition, new Vector3(0,90,0)));
							}
							if(!tileNeighbor.XPos && !tileNeighbor.XPosZNeg && !tileNeighbor.XNegZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE2)
										, addPosition, new Vector3(0,90,0)));
							}
							if(!tileNeighbor.ZNeg && !tileNeighbor.XPosZNeg && !tileNeighbor.XNegZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE1)
										, addPosition, new Vector3(0,180,0)));
							}
							if(!tileNeighbor.ZNeg && !tileNeighbor.XPosZPos && !tileNeighbor.XNegZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE2)
										, addPosition, new Vector3(0,180,0)));
							}
							if(!tileNeighbor.XNeg && !tileNeighbor.XNegZPos && !tileNeighbor.XPosZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE1)
										, addPosition, new Vector3(0,270,0)));
							}
							if(!tileNeighbor.XNeg && !tileNeighbor.XPosZPos && !tileNeighbor.XNegZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE2)
										, addPosition, new Vector3(0,270,0)));
							}						
						}
					}
				}
			}
		}

		public void CombineMeshes()
		{
			CombineInstance[] combineInstance = new CombineInstance[MeshesToAdd.Count];
			for(int i = 0; i < MeshesToAdd.Count; i++)
			{
				combineInstance[i].mesh = MeshesToAdd[i].ToAdd;
				Vector3 position = MeshesToAdd[i].Position.Vector3 + new Vector3(1,0,1) * 0.5f;
				Quaternion rotation = MeshesToAdd[i].Rotation;

				combineInstance[i].transform = Matrix4x4.TRS(position,rotation,Vector3.one);
			}
			MeshFilter myFilter = GetComponent<MeshFilter>();
			myFilter.mesh.CombineMeshes(combineInstance,true,true);
		}

		Neighbors GetNeighboringTile(TilePosition _position)
		{
			Neighbors tileNeighbor = new Neighbors();
			tileNeighbor.NeighborCount = 0;

			if(_position.X == 0)
			{
				if(ChunkPosition.X > 0)
				{
					MeshChunk neighboringChunk = Owner.Chunks[ChunkPosition.X-1,ChunkPosition.Z];
					if(neighboringChunk.CliffMap[neighboringChunk.ChunkSize.X-1,_position.Z])
					{
						tileNeighbor.XNeg = true;
						tileNeighbor.NeighborCount++;
					}

					if(_position.Z == 0)
					{
						if(ChunkPosition.Z > 0)
						{
							neighboringChunk = Owner.Chunks[ChunkPosition.X-1,ChunkPosition.Z-1];
							if(neighboringChunk.CliffMap[neighboringChunk.ChunkSize.X-1,neighboringChunk.ChunkSize.Z-1])
							{
								tileNeighbor.XNegZNeg = true;
								tileNeighbor.NeighborCount++;
							}
						}
					}

					if(_position.Z == ChunkSize.Z-1)
					{
						if(ChunkPosition.Z < Owner.ChunkCount.Z-1)
						{
							neighboringChunk = Owner.Chunks[ChunkPosition.X-1,ChunkPosition.Z+1];
							if(neighboringChunk.CliffMap[neighboringChunk.ChunkSize.X-1,0])
							{
								tileNeighbor.XNegZPos = true;
								tileNeighbor.NeighborCount++;
							}
						}
					}
				}				
			}			
			if(_position.X == ChunkSize.X-1)
			{
				if(ChunkPosition.X < Owner.ChunkCount.X-1)
				{
					MeshChunk neighboringChunk = Owner.Chunks[ChunkPosition.X+1,ChunkPosition.Z];
					if(neighboringChunk.CliffMap[0,_position.Z])
					{
						tileNeighbor.XPos = true;
						tileNeighbor.NeighborCount++;
					}
					if(_position.Z == ChunkSize.Z-1)
					{
						if(ChunkPosition.Z < Owner.ChunkCount.Z-1)
						{
							neighboringChunk = Owner.Chunks[ChunkPosition.X+1,ChunkPosition.Z+1];
							if(neighboringChunk.CliffMap[0,0])
							{
								tileNeighbor.ZPos = true;
								tileNeighbor.NeighborCount++;
							}
						}
					}
					if(_position.Z == 0)
					{
						if(ChunkPosition.Z > 0)
						{
							neighboringChunk = Owner.Chunks[ChunkPosition.X+1,ChunkPosition.Z-1];
							if(neighboringChunk.CliffMap[0,neighboringChunk.ChunkSize.Z-1])
							{
								tileNeighbor.XNegZNeg = true;
								tileNeighbor.NeighborCount++;
							}
						}
					}
				}
			}
			if(_position.Z == 0)
			{
				if(ChunkPosition.Z > 0)
				{
					MeshChunk neighboringChunk = Owner.Chunks[ChunkPosition.X,ChunkPosition.Z-1];
					if(neighboringChunk.CliffMap[_position.X,neighboringChunk.ChunkSize.Z-1])
					{
						tileNeighbor.ZNeg = true;
						tileNeighbor.NeighborCount++;
					}
				}
			}
			if(_position.Z == ChunkSize.Z-1)
			{
				if(ChunkPosition.Z < Owner.ChunkCount.Z-1)
				{
					MeshChunk neighboringChunk = Owner.Chunks[ChunkPosition.X,ChunkPosition.Z+1];
					if(neighboringChunk.CliffMap[_position.X,0])
					{
						tileNeighbor.ZPos = true;
						tileNeighbor.NeighborCount++;
					}
				}
			}

			if(_position.X > 0 && _position.X < ChunkSize.X - 1
				&& _position.Z > 0 && _position.Z < ChunkSize.Z - 1)
			{
				if(CliffMap[_position.X-1,_position.Z])
				{
					tileNeighbor.XNeg = true;
					tileNeighbor.NeighborCount++;
				}
				if(CliffMap[_position.X+1,_position.Z])
				{
					tileNeighbor.XPos = true;
					tileNeighbor.NeighborCount++;
				}
				if(CliffMap[_position.X,_position.Z-1])
				{
					tileNeighbor.ZNeg = true;
					tileNeighbor.NeighborCount++;
				}
				if(CliffMap[_position.X,_position.Z+1])
				{
					tileNeighbor.ZPos = true;
					tileNeighbor.NeighborCount++;
				}
				if(CliffMap[_position.X-1,_position.Z-1])
				{
					tileNeighbor.XNegZNeg = true;
					tileNeighbor.NeighborCount++;
				}
				if(CliffMap[_position.X-1,_position.Z+1])
				{
					tileNeighbor.XNegZPos = true;
					tileNeighbor.NeighborCount++;
				}
				if(CliffMap[_position.X+1,_position.Z-1])
				{
					tileNeighbor.NeighborCount++;
					tileNeighbor.XPosZNeg = true;
				}
				if(CliffMap[_position.X+1,_position.Z+1])
				{
					tileNeighbor.XPosZPos = true;
					tileNeighbor.NeighborCount++;
				}
			}

			return tileNeighbor;
		}
	}
}
