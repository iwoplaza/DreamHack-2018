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
	int m_cullingSleep;

	Thread m_cullingThread;

	public override void InitializeComponent()
	{
		m_environment = WorldController.Instance.MainState.GameEnvironment;
		m_chunkCount = m_environment.ChunkCount;
		m_camera = GetComponent<Camera>();
	}

	public override void UpdateComponent()
	{
		if(m_cullingSleep >= m_cullingSleepCount)
		{
			/*if(m_cullingThread == null)
			{
				m_cullingThread = new Thread(() => CullingThread());
			}
			
			m_cullingThread.Start();*/

			CullingThread();

			m_cullingSleep = 0;
		}
		m_cullingSleep++;
	}

	public void CullingThread()
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(m_camera);
		for(int x = 0; x < m_chunkCount.x; x++)
		{
			for(int y = 0; y < m_chunkCount.y; y++)
			{				
				bool isInView = m_environment.Chunks[x,y].GetComponent<MeshRenderer>().isVisible;//GeometryUtility.TestPlanesAABB(planes,m_environment.Chunks[x,y].GetComponent<MeshRenderer>().bounds);
				if(m_environment.Chunks[x,y].gameObject.activeSelf && !isInView)
				{
					m_environment.AddCullQueue(m_environment.Chunks[x,y].gameObject, true);
				}
				if(!m_environment.Chunks[x,y].gameObject.activeSelf && isInView)
				{
					m_environment.AddCullQueue(m_environment.Chunks[x,y].gameObject, true);
				}
			}
		}
	}
}
