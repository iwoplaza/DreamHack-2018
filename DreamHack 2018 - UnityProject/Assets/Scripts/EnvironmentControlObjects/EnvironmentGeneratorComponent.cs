using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Game.Environment;

namespace Game
{
	public class EnvironmentGeneratorComponent : EnvironmentControlComponent
    {
        GameState m_gameState;

		[SerializeField] GameEnvironment m_environmentGen;
		[SerializeField] Vector2Int m_mapSize;
		[SerializeField] string m_mapSeed;
		
		public override void InitializeComponent()
        {
			WorldMeshResource.UpdateMeshDictionary();
			m_environmentGen.CliffThreshold = 0.5f;
			m_environmentGen.WorldSeed = m_mapSeed;
			m_environmentGen.WorldSize = m_mapSize;
			m_environmentGen.ChunkSize = new Vector2Int(10, 10);
            m_environmentGen.GenerateMap();
		}
		
		public void GenerateMap()
		{
			
		}

		public override void UpdateComponent()
        {

		}
	}
}
