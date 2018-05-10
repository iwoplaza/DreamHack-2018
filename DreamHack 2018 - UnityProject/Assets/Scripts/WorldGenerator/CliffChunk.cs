using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

[RequireComponent(typeof(MeshFilter))]
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
	}

	public TilePosition BaseTilePosition { get; private set; }
	public Vector2Int ChunkSize { get; private set; }
	public bool[,] CliffMap { get; set; }
	public List<MeshCombine> MeshesToAdd { get; private set; }
	public Material MeshMaterial { get; private set; }

	public void GenerateMeshMap()
	{

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
}
