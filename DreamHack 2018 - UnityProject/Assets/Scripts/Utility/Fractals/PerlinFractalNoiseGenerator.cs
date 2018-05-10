using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.Noise
{
	public static class PerlinFractalNoiseGenerator
	{
		public static float[,] GenerateNoise(int xRes, int yRes, float freq, int octaves, float amplifier, int seed)
		{
			float[,] noise = new float[xRes,yRes];

			seed %= 100000;
			
			for(int x = 0; x < xRes; x++)
			{
				for(int y = 0; y < yRes; y++)
				{
					float elevation = 0;
					
					float multiplier = 0;

					for(int i = 0; i < octaves; i++)
					{
						float freqCount = (freq * Mathf.Pow(2,i));
						freqCount -= (freq * Mathf.Pow(2,i))/2;
						float sampleX = (((float)(x+1)/xRes) - 0.5f) * freqCount + seed;
						float sampleY = (((float)(y+1)/yRes) - 0.5f) * freqCount + seed;
						float currentMult = Mathf.Pow(0.5f,i);						
						multiplier += currentMult;	
						elevation += currentMult * Mathf.PerlinNoise(sampleX,sampleY);			
					}
					elevation = elevation/multiplier;					
					Mathf.Clamp01(elevation);				
					noise[x,y] = Mathf.Pow(elevation,amplifier);					
				}
			}

			return noise;
		}

		public static float[,] GenerateNoise(int xRes, int yRes, float freq, int octaves, float amplifier)
		{
			return GenerateNoise(xRes,yRes,freq,octaves,amplifier,0);
		}

		public static float[,] GenerateNoise(int xRes, int yRes, float freq, int octaves, float amplifier, string seed)
		{
			return GenerateNoise(xRes,yRes,freq,octaves,amplifier,seed.GetHashCode());
		}
	}
}
