using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Noise;
using Utility.Noise.Internal;

namespace Utility.Noise
{
	public class FractalChain : MonoBehaviour {		
		
		[SerializeField]Vector2Int m_currentRes;
		public Vector2Int CurrentRes { get { return m_currentRes; } set { m_currentRes = value; } }
		[SerializeField]List<FractalGeneratorChain> m_chain;

		float[,] m_currentNoise;
		public float[,] CurrentNoise { get{ return m_currentNoise; } }

		Texture2D m_currentTexture;
		public Texture2D CurrentTexture { get{ return m_currentTexture; } }

		public void Reset()
		{
			m_currentRes = new Vector2Int(512,512);
		}		

		public void GenerateMap(Vector2Int resolution, string seed)
		{
			if(m_chain == null || m_chain.Count == 0)
			{
				return;
			}
			foreach(FractalGeneratorChain chain in m_chain)
			{
				chain.CurrentGenerator.CurrentRes = resolution;
				chain.CurrentGenerator.CurrentSeed = seed;				
			}
			m_currentRes = resolution;
			GenerateMap();
		}

		public void GenerateMap()
		{
			if(m_chain == null || m_chain.Count == 0)
			{
				Debug.LogWarning("List is empty!");
				return;
			}
			Vector2Int size = m_currentRes;			

			m_currentTexture = new Texture2D(size.x,size.y);
			m_currentNoise = new float[size.x,size.y];
			for(int i = 0; i < m_chain.Count; i++)
			{
				m_chain[i].CurrentGenerator.GenerateFractal(m_currentRes);
			}

			for(int x = 0; x < size.x; x++)
			{
				for(int y = 0; y < size.y; y++)
				{
					float currentValue = 0;
					for(int i = 0; i < m_chain.Count; i++)
					{						
						if(i != 0)
						{
							if(m_chain[i].OperationType == FractalGeneratorChain.OperatorType.ADD)
							{
								currentValue += m_chain[i].CurrentGenerator.CurrentNoise[x,y];
							}
							if(m_chain[i].OperationType == FractalGeneratorChain.OperatorType.REMOVE)
							{
								currentValue -= m_chain[i].CurrentGenerator.CurrentNoise[x,y];
							}
							if(m_chain[i].OperationType == FractalGeneratorChain.OperatorType.MULTIPLY)
							{
								currentValue *= m_chain[i].CurrentGenerator.CurrentNoise[x,y];
							}
							if(m_chain[i].OperationType == FractalGeneratorChain.OperatorType.DIVIDE)
							{
								currentValue /= m_chain[i].CurrentGenerator.CurrentNoise[x,y];
							}
						}
						else
						{
							currentValue = m_chain[0].CurrentGenerator.CurrentNoise[x,y];
						}
					}
					m_currentNoise[x,y] = Mathf.Clamp01(currentValue);
					m_currentTexture.SetPixel(x,y,new Color(m_currentNoise[x,y],m_currentNoise[x,y],m_currentNoise[x,y],1));
				}
			}

			m_currentTexture.Apply();
		}
	}
}
