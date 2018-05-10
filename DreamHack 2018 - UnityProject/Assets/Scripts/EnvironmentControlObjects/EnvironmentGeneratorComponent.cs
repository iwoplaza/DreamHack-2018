using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Game.Environment;

namespace Game
{
	public class EnvironmentGeneratorComponent : EnvironmentControlComponent
    {
		[SerializeField] GameEnvironment m_environmentGen;
		[SerializeField] Vector2Int m_mapSize;
		[SerializeField] string m_mapSeed;
		
		public override void InitializeComponent()
        {
			m_environmentGen.CliffThreshold = 0.5f;
			m_environmentGen.WorldSeed = m_mapSeed;
			m_environmentGen.WorldSize = m_mapSize;
			m_environmentGen.GenerateMap();
		}
		
		public override void UpdateComponent()
        {

		}
	}
}
