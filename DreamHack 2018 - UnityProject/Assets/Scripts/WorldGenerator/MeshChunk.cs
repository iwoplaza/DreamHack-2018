using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Game.Environment;

namespace Game.Environment
{
	public class Neighbors
	{
		bool m_xPos;
		public bool XPos { get{ return m_xPos; } set{ m_xPos = value; } }
		bool m_xPosZpos;
		public bool XPosZPos { get{ return m_xPosZpos; } set{ m_xPosZpos = value; } }
		bool m_xPosZneg;
		public bool XPosZNeg { get{ return m_xPosZneg; } set{ m_xPosZneg = value; } }
		bool m_xNeg;
		public bool XNeg { get{ return m_xNeg; } set{ m_xNeg = value; } }
		bool m_xNegZPos;
		public bool XNegZPos { get{ return m_xNegZPos; } set{ m_xNegZPos = value; } }
		bool m_xNegZNeg;
		public bool XNegZNeg { get{ return m_xNegZNeg; } set{ m_xNegZNeg = value; } }
		bool m_zPos;
		public bool ZPos { get{ return m_zPos; } set{ m_zPos = value; } }
		bool m_zNeg;
		public bool ZNeg { get{ return m_zNeg; } set{ m_zNeg = value; } }

		int m_neighborCount;
		public int NeighborCount { get { return m_neighborCount; } set{ m_neighborCount = value; } }
	}

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

		public GameEnvironment Owner { get; private set; }
		public TilePosition ChunkBasePosition { get; set; }

		public TilePosition ChunkPosition { get; set; }
		public TilePosition ChunkSize { get; set; }
		public bool[,] CliffMap { get; set; }
		public List<MeshCombine> MeshesToAdd { get; private set; }
		public Material MeshMaterial { get; private set; }

		public void Initialize(GameEnvironment _owner, TilePosition _chunkBasePosition, TilePosition _chunkPosition, TilePosition _chunkSize, Material _material)
		{
			Owner = _owner;
			ChunkBasePosition = _chunkBasePosition;
			ChunkPosition = _chunkPosition;
			ChunkSize = _chunkSize;
			CliffMap = new bool[_chunkSize.X,_chunkSize.Z];
			MeshMaterial = _material;
			gameObject.AddComponent<MeshFilter>();
			gameObject.AddComponent<MeshRenderer>();
		}

		public void GenerateMeshMap()
		{
			MeshesToAdd = new List<MeshCombine>();
			for(int x = 0; x < ChunkSize.X; x++)
			{
				for(int z = 0; z < ChunkSize.Z; z++)
				{
					TilePosition addPosition = new TilePosition(x + ChunkBasePosition.X,z + ChunkBasePosition.Z);
					if(!CliffMap[x,z])
					{
						MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.GROUND)
										, addPosition, Vector3.zero));
					}
					else
					{
						Neighbors tileNeighbor = GetNeighboringTile(new TilePosition(x,z));
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
							else
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.GROUND_CLIFF)
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

							// CASE: BRIDGES
							else if(!tileNeighbor.ZPos && !tileNeighbor.ZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE)
										, addPosition, new Vector3(0,0,0)));
							}
							else if(!tileNeighbor.XPos && !tileNeighbor.XNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE)
										, addPosition, new Vector3(0,90,0)));
							}

							// CASE:
							//	101
							//	1P0
							//	1P1
							else if(!tileNeighbor.ZPos && !tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_DIAGONAL)
										, addPosition, new Vector3(0,0,0)));
							}
							else if(!tileNeighbor.XPos && !tileNeighbor.ZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_DIAGONAL)
										, addPosition, new Vector3(0,90,0)));
							}
							else if(!tileNeighbor.ZNeg && !tileNeighbor.XNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_DIAGONAL)
										, addPosition, new Vector3(0,180,0)));
							}
							else if(!tileNeighbor.XNeg && !tileNeighbor.ZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_DIAGONAL)
										, addPosition, new Vector3(0,270,0)));
							}

							else
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.GROUND_CLIFF)
										, addPosition, new Vector3(0,0,0)));	
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
							//	001 | 100
							//	1P0	| 1P0
							//	111	| 111

							else if((!tileNeighbor.XNegZPos || !tileNeighbor.XPosZNeg || !tileNeighbor.XPosZPos) && !tileNeighbor.ZPos && !tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_DIAGONAL)
										, addPosition, new Vector3(0,0,0)));
							}
							else if((!tileNeighbor.XPosZPos || !tileNeighbor.XNegZNeg || !tileNeighbor.XPosZNeg) && !tileNeighbor.ZNeg && !tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_DIAGONAL)
										, addPosition, new Vector3(0,90,0)));
							}
							else if((!tileNeighbor.XNegZPos || !tileNeighbor.XPosZNeg || !tileNeighbor.XNegZNeg) && !tileNeighbor.ZNeg && !tileNeighbor.XNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_DIAGONAL)
										, addPosition, new Vector3(0,180,0)));
							}
							else if((!tileNeighbor.XPosZPos || !tileNeighbor.XNegZNeg || !tileNeighbor.XNegZPos) && !tileNeighbor.ZPos && !tileNeighbor.XNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_DIAGONAL)
										, addPosition, new Vector3(0,270,0)));
							}

							// CASE:
							//	001	| 010  | 011
							//	1P1	| 1P0  | 1P0
							//	110	| 111  | 110

							else if(!tileNeighbor.ZPos && (!tileNeighbor.XPosZPos || !tileNeighbor.XNegZPos) && !tileNeighbor.XNegZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE1)
										, addPosition, new Vector3(0,0,0)));
							}
							else if(!tileNeighbor.ZPos && (!tileNeighbor.XPosZPos || !tileNeighbor.XNegZPos) && !tileNeighbor.XPosZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE2)
										, addPosition, new Vector3(0,0,0)));
							}
							else if(!tileNeighbor.XPos && (!tileNeighbor.XPosZNeg || !tileNeighbor.XPosZPos) && !tileNeighbor.XNegZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE1)
										, addPosition, new Vector3(0,90,0)));
							}
							else if(!tileNeighbor.XPos && (!tileNeighbor.XPosZNeg || !tileNeighbor.XPosZPos) && !tileNeighbor.XNegZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE2)
										, addPosition, new Vector3(0,90,0)));
							}
							else if(!tileNeighbor.ZNeg && (!tileNeighbor.XPosZNeg || !tileNeighbor.XNegZNeg) && !tileNeighbor.XPosZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE1)
										, addPosition, new Vector3(0,180,0)));
							}
							else if(!tileNeighbor.ZNeg && (!tileNeighbor.XPosZNeg || !tileNeighbor.XNegZNeg) && !tileNeighbor.XNegZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE2)
										, addPosition, new Vector3(0,180,0)));
							}
							else if(!tileNeighbor.XNeg && (!tileNeighbor.XNegZNeg || !tileNeighbor.XNegZPos) && !tileNeighbor.XPosZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE1)
										, addPosition, new Vector3(0,270,0)));
							}
							else if(!tileNeighbor.XNeg && (!tileNeighbor.XNegZNeg || !tileNeighbor.XNegZPos) && !tileNeighbor.XPosZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE2)
										, addPosition, new Vector3(0,270,0)));
							}

							// CASE:
							//	010
							//	1P1
							//	110

							else if(!tileNeighbor.XNegZNeg && !tileNeighbor.XNegZPos && !tileNeighbor.XPosZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_CORNER_CASE3)
										, addPosition, new Vector3(0,0,0)));
							}
							else if(!tileNeighbor.XNegZPos && !tileNeighbor.XPosZPos && !tileNeighbor.XPosZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_CORNER_CASE3)
										, addPosition, new Vector3(0,90,0)));
							}
							else if(!tileNeighbor.XPosZPos && !tileNeighbor.XPosZNeg && !tileNeighbor.XNegZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_CORNER_CASE3)
										, addPosition, new Vector3(0,180,0)));
							}
							else if(!tileNeighbor.XPosZNeg && !tileNeighbor.XNegZNeg && !tileNeighbor.XNegZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_CORNER_CASE3)
										, addPosition, new Vector3(0,270,0)));
							}

							// CASE:
							//	010
							//	1P1
							//	101

							else if(!tileNeighbor.ZPos && !tileNeighbor.XNegZNeg && !tileNeighbor.XPosZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE3)
										, addPosition, new Vector3(0,0,0)));
							}
							else if(!tileNeighbor.XPos && !tileNeighbor.XNegZNeg && !tileNeighbor.XNegZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE3)
										, addPosition, new Vector3(0,90,0)));
							}
							else if(!tileNeighbor.ZNeg && !tileNeighbor.XNegZPos && !tileNeighbor.XPosZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE3)
										, addPosition, new Vector3(0,180,0)));
							}
							else if(!tileNeighbor.XNeg && !tileNeighbor.XPosZPos && !tileNeighbor.XPosZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE3)
										, addPosition, new Vector3(0,270,0)));
							}

							//	CASE:
							// 000
							// 0P0
							// 111

							else if(tileNeighbor.ZNeg && tileNeighbor.XNegZNeg && tileNeighbor.XPosZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,0,0)));
							}
							else if(tileNeighbor.XNeg && tileNeighbor.XNegZNeg && tileNeighbor.XNegZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,90,0)));
							}
							else if(tileNeighbor.ZPos && tileNeighbor.XNegZPos && tileNeighbor.XPosZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,180,0)));
							}
							else if(tileNeighbor.XPos && tileNeighbor.XPosZNeg && tileNeighbor.XPosZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,270,0)));
							}

							else
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.GROUND_CLIFF)
										, addPosition, new Vector3(0,0,0)));	
							}
						}					
						else if(tileNeighbor.NeighborCount == 4)
						{
							//	CASE:
							// 010	| 101
							// 1P1	| 0P0
							// 010	| 101

							if(!tileNeighbor.XNeg && !tileNeighbor.XPos && !tileNeighbor.ZNeg && !tileNeighbor.ZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.CLIFF_LONE)
										, addPosition, new Vector3(0,0,0)));
							}
							else if(tileNeighbor.XNeg && tileNeighbor.XPos && tileNeighbor.ZNeg && tileNeighbor.ZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_CORNER_CASE4)
										, addPosition, new Vector3(0,0,0)));
							}

							// CASE:
							//	000	| 001
							//	1P1	| 1P1
							// 	101	| 100

							else if(!tileNeighbor.ZPos && !tileNeighbor.ZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE)
										, addPosition, new Vector3(0,0,0)));
							}
							else if(!tileNeighbor.XPos && !tileNeighbor.XNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE)
										, addPosition, new Vector3(0,90,0)));
							}
							
							// CASE:
							// 000	| 001
							// 1P0	| 1P0
							// 111	| 110

							if(!tileNeighbor.ZPos && !tileNeighbor.XPos && ((!tileNeighbor.XPosZPos && (!tileNeighbor.XNegZPos || !tileNeighbor.XPosZNeg)) || (tileNeighbor.XPosZPos && !tileNeighbor.XNegZPos && !tileNeighbor.XPosZNeg)))
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_SHARPCORNER)
										, addPosition, new Vector3(0,0,0)));
							}
							else if(!tileNeighbor.ZNeg && !tileNeighbor.XPos && ((!tileNeighbor.XPosZNeg && (!tileNeighbor.XPosZPos || !tileNeighbor.XNegZNeg)) || (tileNeighbor.XPosZNeg && !tileNeighbor.XPosZPos && !tileNeighbor.XNegZNeg)))
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_SHARPCORNER)
										, addPosition, new Vector3(0,90,0)));
							}
							else if(!tileNeighbor.ZNeg && !tileNeighbor.XNeg && ((!tileNeighbor.XNegZNeg && (!tileNeighbor.XPosZNeg || !tileNeighbor.XNegZPos)) || (tileNeighbor.XNegZNeg && !tileNeighbor.XPosZNeg && !tileNeighbor.XNegZPos)))
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_SHARPCORNER)
										, addPosition, new Vector3(0,180,0)));
							}
							else if(!tileNeighbor.ZPos && !tileNeighbor.XNeg && ((!tileNeighbor.XPosZPos && (!tileNeighbor.XNegZNeg || !tileNeighbor.XPosZPos)) || (!tileNeighbor.XNegZPos && !tileNeighbor.XNegZNeg && !tileNeighbor.XPosZPos)))
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_SHARPCORNER)
										, addPosition, new Vector3(0,270,0)));
							}
							
							// CASE:
							// 000
							// 1P1
							// 110

							else if(!tileNeighbor.ZPos && !tileNeighbor.XPosZPos && !tileNeighbor.XNegZPos && !tileNeighbor.XNegZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE1)
										, addPosition, new Vector3(0,0,0)));
							}
							else if(!tileNeighbor.ZPos && !tileNeighbor.XPosZPos && !tileNeighbor.XNegZPos && !tileNeighbor.XPosZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE2)
										, addPosition, new Vector3(0,0,0)));
							}
							else if(!tileNeighbor.XPos && !tileNeighbor.XPosZPos && !tileNeighbor.XPosZNeg && !tileNeighbor.XNegZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE1)
										, addPosition, new Vector3(0,90,0)));
							}
							else if(!tileNeighbor.XPos && !tileNeighbor.XPosZPos && !tileNeighbor.XPosZNeg && !tileNeighbor.XNegZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE2)
										, addPosition, new Vector3(0,90,0)));
							}
							else if(!tileNeighbor.ZNeg && !tileNeighbor.XPosZNeg && !tileNeighbor.XNegZNeg && !tileNeighbor.XPosZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE1)
										, addPosition, new Vector3(0,180,0)));
							}
							else if(!tileNeighbor.ZNeg && !tileNeighbor.XPosZNeg && !tileNeighbor.XNegZNeg && !tileNeighbor.XNegZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE2)
										, addPosition, new Vector3(0,180,0)));
							}
							else if(!tileNeighbor.XNeg && !tileNeighbor.XNegZNeg && !tileNeighbor.XNegZPos && !tileNeighbor.XPosZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE1)
										, addPosition, new Vector3(0,270,0)));
							}
							else if(!tileNeighbor.XNeg && !tileNeighbor.XNegZNeg && !tileNeighbor.XNegZPos && !tileNeighbor.XPosZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE2)
										, addPosition, new Vector3(0,270,0)));
							}

							//	CASE:
							// 001
							// 1P0
							// 101

							else if(!tileNeighbor.XPos && !tileNeighbor.ZPos && !tileNeighbor.XNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,0,0)));
							}
							else if(!tileNeighbor.XPos && !tileNeighbor.ZPos && !tileNeighbor.ZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,90,0)));
							}
							else if(!tileNeighbor.XPos && !tileNeighbor.ZNeg && !tileNeighbor.XNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,180,0)));
							}
							else if(!tileNeighbor.XNeg && !tileNeighbor.ZPos && !tileNeighbor.ZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,270,0)));
							}

							//	CASE:
							// 010
							// 110
							// 101

							else if(tileNeighbor.XNeg && tileNeighbor.ZPos && !tileNeighbor.XPos && !tileNeighbor.ZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_DIAGONAL_CASE1)
										, addPosition, new Vector3(0,90,0)));
							}
							else if(!tileNeighbor.XNeg && tileNeighbor.ZPos && tileNeighbor.XPos && !tileNeighbor.ZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_DIAGONAL_CASE1)
										, addPosition, new Vector3(0,180,0)));
							}
							else if(!tileNeighbor.XNeg && !tileNeighbor.ZPos && tileNeighbor.XPos && tileNeighbor.ZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_DIAGONAL_CASE1)
										, addPosition, new Vector3(0,270,0)));
							}
							else if(tileNeighbor.XNeg && !tileNeighbor.ZPos && !tileNeighbor.XPos && tileNeighbor.ZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_DIAGONAL_CASE1)
										, addPosition, new Vector3(0,0,0)));
							}
							
							// CASE : INTERSECTION
							else if(tileNeighbor.ZPos && tileNeighbor.XNeg && !tileNeighbor.ZNeg && tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE3)
										, addPosition, new Vector3(0,180,0)));
							}
							else if(tileNeighbor.ZPos && !tileNeighbor.XNeg && tileNeighbor.ZNeg && tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE3)
										, addPosition, new Vector3(0,270,0)));
							}
							else if(!tileNeighbor.ZPos && tileNeighbor.XNeg && tileNeighbor.ZNeg && tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE3)
										, addPosition, new Vector3(0,0,0)));
							}
							else if(tileNeighbor.ZPos && tileNeighbor.XNeg && tileNeighbor.ZNeg && !tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE3)
										, addPosition, new Vector3(0,90,0)));
							}
						}
						if(tileNeighbor.NeighborCount == 3)
						{

							if(!tileNeighbor.ZPos && !tileNeighbor.ZNeg && !tileNeighbor.XNeg && !tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.CLIFF_LONE)
										, addPosition, new Vector3(0,0,0)));
							}
							
							//	CASE:
							// 000
							// 0P0
							// 111

							else if(tileNeighbor.ZNeg && tileNeighbor.XNegZNeg && tileNeighbor.XPosZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,0,0)));
							}
							else if(tileNeighbor.XNeg && tileNeighbor.XNegZNeg && tileNeighbor.XNegZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,90,0)));
							}
							else if(tileNeighbor.ZPos && tileNeighbor.XNegZPos && tileNeighbor.XPosZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,180,0)));
							}
							else if(tileNeighbor.XPos && tileNeighbor.XPosZNeg && tileNeighbor.XPosZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,270,0)));
							}

							// CASE:
							// 000
							// 1P0
							// 110

							else if(tileNeighbor.XNeg && tileNeighbor.ZNeg && tileNeighbor.XNegZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_DIAGONAL)
										, addPosition, new Vector3(0,0,0)));
							}
							else if(tileNeighbor.XNeg && tileNeighbor.ZPos && tileNeighbor.XNegZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_SHARPCORNER)
										, addPosition, new Vector3(0,90,0)));
							}
							else if(tileNeighbor.XPos && tileNeighbor.ZPos && tileNeighbor.XPosZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_SHARPCORNER)
										, addPosition, new Vector3(0,180,0)));
							}
							else if(tileNeighbor.XPos && tileNeighbor.ZNeg && tileNeighbor.XPosZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_SHARPCORNER)
										, addPosition, new Vector3(0,270,0)));
							}

							// CASE:
							// 010
							// 1P0
							// 100

							else if(tileNeighbor.ZPos && tileNeighbor.XNeg && !tileNeighbor.ZNeg && !tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_DIAGONAL_CASE1)
										, addPosition, new Vector3(0,90,0)));
							}
							else if(tileNeighbor.ZPos && !tileNeighbor.XNeg && !tileNeighbor.ZNeg && tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_DIAGONAL_CASE1)
										, addPosition, new Vector3(0,180,0)));
							}
							else if(!tileNeighbor.ZPos && !tileNeighbor.XNeg && tileNeighbor.ZNeg && tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_DIAGONAL_CASE1)
										, addPosition, new Vector3(0,270,0)));
							}
							else if(!tileNeighbor.ZPos && tileNeighbor.XNeg && tileNeighbor.ZNeg && !tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_DIAGONAL_CASE1)
										, addPosition, new Vector3(0,0,0)));
							}
							// CASE:
							// 100
							// 0P0
							// 011

							else if(tileNeighbor.ZNeg && (tileNeighbor.XPosZNeg || tileNeighbor.XNegZPos) && !tileNeighbor.ZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,0,0)));
							}
							else if(tileNeighbor.XNeg && (tileNeighbor.XNegZNeg || tileNeighbor.XNegZPos) && !tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,90,0)));
							}
							else if(tileNeighbor.ZPos && (tileNeighbor.XPosZPos || tileNeighbor.XNegZPos) && !tileNeighbor.ZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,180,0)));
							}
							else if(tileNeighbor.XPos && (tileNeighbor.XPosZNeg || tileNeighbor.XPosZPos) && !tileNeighbor.XNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,270,0)));
							}
							
							// CASE:
							// 010
							// 1P1
							// 000

							else if(tileNeighbor.ZPos && tileNeighbor.XNeg && !tileNeighbor.ZNeg && tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE3)
										, addPosition, new Vector3(0,180,0)));
							}
							else if(tileNeighbor.ZPos && !tileNeighbor.XNeg && tileNeighbor.ZNeg && tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE3)
										, addPosition, new Vector3(0,270,0)));
							}
							else if(!tileNeighbor.ZPos && tileNeighbor.XNeg && tileNeighbor.ZNeg && tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE3)
										, addPosition, new Vector3(0,0,0)));
							}
							else if(tileNeighbor.ZPos && tileNeighbor.XNeg && tileNeighbor.ZNeg && !tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_CASE3)
										, addPosition, new Vector3(0,90,0)));
							}

							// CASE:
							// 000
							// 1P1
							// 000

							else if(!tileNeighbor.ZPos && tileNeighbor.XNeg && !tileNeighbor.ZNeg && tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE)
										, addPosition, new Vector3(0,90,0)));
							}
							else if(tileNeighbor.ZPos && !tileNeighbor.XNeg && tileNeighbor.ZNeg && !tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE)
										, addPosition, new Vector3(0,0,0)));
							}

							else
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.GROUND_CLIFF)
										, addPosition, new Vector3(0,0,0)));	
							}
						}
						else if(tileNeighbor.NeighborCount == 2)
						{
							// CASE : ELBOW
							if(tileNeighbor.ZPos && tileNeighbor.XNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_DIAGONAL_CASE1)
										, addPosition, new Vector3(0,90,0)));
							}
							else if(tileNeighbor.ZPos && tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_DIAGONAL_CASE1)
										, addPosition, new Vector3(0,180,0)));
							}
							else if(tileNeighbor.ZNeg && tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_DIAGONAL_CASE1)
										, addPosition, new Vector3(0,270,0)));
							}
							else if(tileNeighbor.XNeg && tileNeighbor.ZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_DIAGONAL_CASE1)
										, addPosition, new Vector3(0,0,0)));
							}

							//CASE : BRIDGE
							else if(tileNeighbor.XNeg && tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE)
										, addPosition, new Vector3(0,90,0)));
							}
							else if(tileNeighbor.ZPos && tileNeighbor.ZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE)
										, addPosition, new Vector3(0,0,0)));
							}

							//CASE : SINGLE CONNECTION
							else if(tileNeighbor.ZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,0,0)));
							}
							else if(tileNeighbor.XNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,90,0)));
							}
							else if(tileNeighbor.ZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,180,0)));
							}
							else if(tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,270,0)));
							}

							else
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.GROUND_CLIFF)
										, addPosition, new Vector3(0,0,0)));	
							}
						}
						else if(tileNeighbor.NeighborCount == 1)
						{
							//CASE : SINGLE CONNECTION
							if(tileNeighbor.ZNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,0,0)));
							}
							else if(tileNeighbor.XNeg)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,90,0)));
							}
							else if(tileNeighbor.ZPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,180,0)));
							}
							else if(tileNeighbor.XPos)
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.EDGE_CLIFF_STRAIGHT_LONE_END)
										, addPosition, new Vector3(0,270,0)));
							}

							else
							{
								MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.GROUND_CLIFF)
										, addPosition, new Vector3(0,0,0)));	
							}
						}
						else if(tileNeighbor.NeighborCount == 0)
						{							
							MeshesToAdd.Add(new MeshCombine(WorldMeshResource.GetResources(WorldMeshResource.MeshType.CLIFF_LONE)
								, addPosition, new Vector3(0,0,0)));
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
				Vector3 position = MeshesToAdd[i].Position.Vector3;
				Quaternion rotation = MeshesToAdd[i].Rotation;

				combineInstance[i].transform = Matrix4x4.TRS(position,rotation,Vector3.one);
			}
			MeshFilter myFilter = GetComponent<MeshFilter>();
			myFilter.mesh.CombineMeshes(combineInstance,true,true);
			GetComponent<MeshRenderer>().sharedMaterial = MeshMaterial;
		}

		Neighbors GetNeighboringTile(TilePosition _position)
		{
			Neighbors tileNeighbor = new Neighbors();
			tileNeighbor.NeighborCount = 0;			
			
			if(_position.X > 0){
				if(CliffMap[_position.X-1,_position.Z])
				{
					tileNeighbor.XNeg = true;
					tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
				}
				if(_position.Z > 0){
					if(CliffMap[_position.X-1,_position.Z-1])
					{
						tileNeighbor.XNegZNeg = true;
						tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
					}
				}
				if(_position.Z < ChunkSize.Z-1)
				{
					if(CliffMap[_position.X-1,_position.Z+1])
					{
						tileNeighbor.XNegZPos = true;
						tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
					}
				}
			}
			else
			{
				if(ChunkPosition.X > 0)
				{
					if(Owner.Chunks[ChunkPosition.X-1,ChunkPosition.Z].CliffMap[
						Owner.Chunks[ChunkPosition.X-1,ChunkPosition.Z].ChunkSize.X-1
						,_position.Z])
					{
						tileNeighbor.XNeg = true;
						tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
					}

					if(_position.Z > 0)
					{
						if(Owner.Chunks[ChunkPosition.X-1,ChunkPosition.Z].CliffMap[
						Owner.Chunks[ChunkPosition.X-1,ChunkPosition.Z].ChunkSize.X-1
						,_position.Z-1])
						{
							tileNeighbor.XNegZNeg = true;
							tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
						}
					}
					else
					{
						if(ChunkPosition.Z > 0)
						{
							if(Owner.Chunks[ChunkPosition.X-1,ChunkPosition.Z-1].CliffMap[
								Owner.Chunks[ChunkPosition.X-1,ChunkPosition.Z-1].ChunkSize.X-1
								,Owner.Chunks[ChunkPosition.X-1,ChunkPosition.Z-1].ChunkSize.Z-1])
							{
								tileNeighbor.XNegZNeg = true;
								tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
							}
						}
						else
						{
							tileNeighbor.XNegZNeg = true;
							tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
						}
					}
					if(_position.Z < ChunkSize.Z-1){
						if(Owner.Chunks[ChunkPosition.X-1,ChunkPosition.Z].CliffMap[
							Owner.Chunks[ChunkPosition.X-1,ChunkPosition.Z].ChunkSize.X-1
							,_position.Z])
						{
							tileNeighbor.XNegZPos = true;
							tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
						}
					}
					else
					{
						if(ChunkPosition.Z < Owner.ChunkCount.Z-1)
						{					
							if(Owner.Chunks[ChunkPosition.X-1,ChunkPosition.Z+1].CliffMap[
								Owner.Chunks[ChunkPosition.X-1,ChunkPosition.Z+1].ChunkSize.X-1
								,0])
							{
								tileNeighbor.XNegZPos = true;
								tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
							}
						}
						else
						{
							tileNeighbor.XNegZPos = true;
							tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
						}
					}
				}
				else
				{
					tileNeighbor.XNeg = true;
					tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
				}
			}

			if(_position.X < ChunkSize.X-1)
			{
				if(CliffMap[_position.X+1,_position.Z])
				{
					tileNeighbor.XPos = true;
					tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
				}
				if(_position.Z > 0){
					if(CliffMap[_position.X+1,_position.Z-1])
					{
						tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
						tileNeighbor.XPosZNeg = true;
					}
				}
				if(_position.Z < ChunkSize.Z - 1){
					if(CliffMap[_position.X+1,_position.Z+1])
					{
						tileNeighbor.XPosZPos = true;
						tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
					}
				}
			}
			else
			{
				if(ChunkPosition.X < Owner.ChunkCount.X - 1)
				{
					if(Owner.Chunks[ChunkPosition.X+1,ChunkPosition.Z].CliffMap[
						0,_position.Z])
					{
						tileNeighbor.XPos = true;
						tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
					}

					if(_position.Z > 0){
						if(Owner.Chunks[ChunkPosition.X+1,ChunkPosition.Z].CliffMap[
						0,_position.Z-1])
						{
							tileNeighbor.XPosZNeg = true;
							tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
						}
					}
					else
					{
						if(ChunkPosition.Z > 0)
						{
							if(Owner.Chunks[ChunkPosition.X+1,ChunkPosition.Z-1].CliffMap[
								0,Owner.Chunks[ChunkPosition.X+1,ChunkPosition.Z-1].ChunkSize.Z-1])
							{
								tileNeighbor.XPosZNeg = true;
								tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
							}
						}
						else
						{
							tileNeighbor.XPosZNeg = true;
							tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
						}
					}

					if(_position.Z < ChunkSize.Z-1){
						if(Owner.Chunks[ChunkPosition.X+1,ChunkPosition.Z].CliffMap[
								0,_position.Z])
						{
							tileNeighbor.XPosZPos = true;
							tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
						}
					}
					else
					{
						if(ChunkPosition.Z < Owner.ChunkCount.Z-1)
						{					
							if(Owner.Chunks[ChunkPosition.X+1,ChunkPosition.Z+1].CliffMap[
								0,0])
							{
								tileNeighbor.XPosZPos = true;
								tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
							}
						}
						else
						{
							tileNeighbor.XPosZPos = true;
							tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
						}
					}
				}
				else
				{
					tileNeighbor.XPos = true;
					tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
				}
			}
			
			if(_position.Z > 0){
				if(CliffMap[_position.X,_position.Z-1])
				{
					tileNeighbor.ZNeg = true;
					tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
				}
			}
			else
			{
				if(ChunkPosition.Z > 0)
				{
					if(Owner.Chunks[ChunkPosition.X,ChunkPosition.Z-1].CliffMap[_position.X
						,Owner.Chunks[ChunkPosition.X,ChunkPosition.Z-1].ChunkSize.Z-1])
					{
						tileNeighbor.ZNeg = true;
						tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
					}
				}
				else
				{
					tileNeighbor.ZNeg = true;
					tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
				}
			}

			if(_position.Z < ChunkSize.Z-1){
				if(CliffMap[_position.X,_position.Z+1])
				{
					tileNeighbor.ZPos = true;
					tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
				}
			}
			else
			{
				if(ChunkPosition.Z < Owner.ChunkCount.Z-1)
				{					
					if(Owner.Chunks[ChunkPosition.X,ChunkPosition.Z+1].CliffMap[_position.X
						,0])
					{
						tileNeighbor.ZPos = true;
						tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
					}
				}
				else
				{
					tileNeighbor.ZPos = true;
					tileNeighbor.NeighborCount = tileNeighbor.NeighborCount + 1;
				}
			}
					

			return tileNeighbor;
		}
	}
}
