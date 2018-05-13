using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Game;
using Game.Environment;

[RequireComponent(typeof(Camera))]
public class ChunkCulling : EnvironmentControlComponent {

	private Camera m_camera;
	private GameEnvironment m_environment;
	private Vector2Int m_chunkCount;

	[SerializeField]int m_cullingSleepCount = 3;

	[Range(0.005f,1f)]
	[SerializeField]float m_boundsExpandSize;
	int m_cullingSleep;

	Thread m_cullingThread;

	Bounds[,] m_chunkBounds;

	public override void InitializeComponent()
	{
		m_environment = WorldController.Instance.MainState.GameEnvironment;
		m_chunkCount = m_environment.ChunkCount;
		UpdateChunkBoundsDictionary();
		m_camera = GetComponent<Camera>();
	}

	public override void UpdateComponent()
	{
		if(m_cullingSleep >= m_cullingSleepCount)
		{
			Plane camPlane = new Plane(m_camera.transform.rotation * Vector3.forward,0);
			Matrix4x4 projectionMatrix = m_camera.projectionMatrix;
			Matrix4x4 worldToLocalMatrix = m_camera.transform.worldToLocalMatrix;
			float clippingDistance = m_camera.farClipPlane;
			camPlane.distance = camPlane.GetDistanceToPoint(m_camera.transform.position);
			m_cullingThread = new Thread(() => CullingThread(projectionMatrix,worldToLocalMatrix,camPlane,clippingDistance));			
			
			m_cullingThread.Start();

			m_cullingSleep = 0;
		}
		m_cullingSleep++;
	}

	public void UpdateChunkBoundsDictionary()
	{
		m_chunkBounds = new Bounds[m_environment.WorldSize.x,m_environment.WorldSize.y];
		for(int x = 0; x < m_chunkCount.x; x++)
		{
			for(int y = 0; y < m_chunkCount.y; y++)
			{
				MeshChunk current = m_environment.Chunks[x,y];
				m_chunkBounds[x,y] = new Bounds();
				m_chunkBounds[x,y].SetMinMax(new Vector3(0.5f,0,0.5f) + current.ChunkBasePosition.Vector3,
											new Vector3(0.5f,0,0.5f) + current.ChunkBasePosition.Vector3 +
											new Vector3(current.ChunkSize.x,2,current.ChunkSize.y));			
			}
		}
	}	

	public void CullingThread(Matrix4x4 _projectionMatrix, Matrix4x4 _worldToLocalMatrix, Plane _camPlane, float _clippingDistance)
	{
		for(int x = 0; x < m_chunkCount.x; x++)
		{
			for(int y = 0; y < m_chunkCount.y; y++)
			{				
				bool isInView = BoundsInView(m_chunkBounds[x,y], _projectionMatrix,_worldToLocalMatrix,_camPlane,_clippingDistance);
				if(!isInView)
				{
					m_environment.AddCullQueue(new TilePosition(x,y), true);
				}
				if(isInView)
				{
					m_environment.AddCullQueue(new TilePosition(x,y), false);
				}
			}
		}
	}

	bool BoundsInView(Bounds _bound,Matrix4x4 _projectionMatrix, Matrix4x4 _worldToLocalMatrix,Plane _camPlane, float _clippingDistance)
	{
		Vector4[] bounds = new Vector4[8];
		bounds[0] = GetPos(_projectionMatrix,_worldToLocalMatrix,_bound.min,_camPlane,_clippingDistance);
		bounds[1] = GetPos(_projectionMatrix,_worldToLocalMatrix,_bound.min + new Vector3(_bound.size.x,0,0),_camPlane,_clippingDistance);
		bounds[2] = GetPos(_projectionMatrix,_worldToLocalMatrix,_bound.min + new Vector3(_bound.size.x,0,_bound.size.z),_camPlane,_clippingDistance);
		bounds[3] = GetPos(_projectionMatrix,_worldToLocalMatrix,_bound.min + new Vector3(0,0,_bound.size.z),_camPlane,_clippingDistance);
		bounds[4] = GetPos(_projectionMatrix,_worldToLocalMatrix,_bound.max,_camPlane,_clippingDistance);
		bounds[5] = GetPos(_projectionMatrix,_worldToLocalMatrix,_bound.max - new Vector3(0,0,_bound.size.z),_camPlane,_clippingDistance);
		bounds[6] = GetPos(_projectionMatrix,_worldToLocalMatrix,_bound.min - new Vector3(_bound.size.x,0,_bound.size.z),_camPlane,_clippingDistance);
		bounds[7] = GetPos(_projectionMatrix,_worldToLocalMatrix,_bound.min - new Vector3(_bound.size.x,0,0),_camPlane,_clippingDistance);

		Bounds newBound = new Bounds(bounds[0],Vector3.zero);

		foreach(Vector4 bound in bounds)
		{
			if(newBound.max.x < bound.x)
			{
				newBound.max = new Vector3(bound.x,newBound.max.y,newBound.max.z);
			}
			if(newBound.min.x > bound.x)
			{
				newBound.min = new Vector3(bound.x,newBound.min.y,newBound.min.z);
			}
			if(newBound.max.y < bound.y)
			{
				newBound.max = new Vector3(newBound.max.x,bound.y,newBound.max.z);
			}
			if(newBound.min.y > bound.y)
			{
				newBound.min = new Vector3(newBound.min.x,bound.y,newBound.min.z);
			}
			if(newBound.max.z < bound.z)
			{
				newBound.max = new Vector3(newBound.max.x,newBound.max.y,bound.z);
			}
			if(newBound.min.z > bound.z)
			{
				newBound.min = new Vector3(newBound.min.x,newBound.min.y,bound.z);
			}
		}

		Bounds cameraBound = new Bounds(new Vector3(0.5f,0.5f,0.5f),Vector3.one);
		cameraBound.Expand(m_boundsExpandSize);

		return cameraBound.Intersects(newBound);
	}

	Vector4 GetPos(Matrix4x4 _projectionMatrix, Matrix4x4 _worldToLocalMatrix, Vector3 _position, Plane _camPlane, float _clippingDistance)
    {
        Matrix4x4 VP = _projectionMatrix * _worldToLocalMatrix;
        Vector4 v = VP * new Vector4( _position.x, _position.y, _position.z, 1 );        
        Vector4 ViewportPoint = v / -v.w;
        ViewportPoint.y = 0.5f + ViewportPoint.y/2f;
        ViewportPoint.x = 0.5f + ViewportPoint.x/2f;
        ViewportPoint.w = v.z;
		ViewportPoint.z = _camPlane.GetDistanceToPoint(_position)/_clippingDistance;
        return ViewportPoint;
    }
}
