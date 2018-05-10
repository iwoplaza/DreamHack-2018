using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utility.Noise;
[CustomEditor(typeof(FractalChain))]
public class FractalChainEditor : Editor {

	Texture2D m_currentTex;
	bool m_texUpdated;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		FractalChain t = (target as FractalChain);		

		if(GUILayout.Button("GENERATE"))
		{
			t.GenerateMap();
			m_texUpdated = false;
		}

		if(t.CurrentTexture != null && !m_texUpdated)
		{
			m_currentTex = ResizeTexture(t.CurrentTexture, new Vector2Int(386,386));
			m_texUpdated = true;
		}

		if(m_currentTex != null)
		{
			GUILayout.Label(m_currentTex);
		}
	}

	public static Texture2D ResizeTexture(Texture2D src, Vector2Int newSize)
	{
		Texture2D newTex = new Texture2D(newSize.x,newSize.y);

		try
		{
			newTex.GetPixels();
		}
		catch
		{
			return newTex;
		}

		if(src.height == 0 || src.width == 0)
		{
			return newTex;
		}

		for(int x = 0; x <= newSize.x; x++)
		{
			for(int y = 0; y <= newSize.y; y++)
			{
				int xPos = Mathf.RoundToInt(Mathf.Clamp01((float)(x+1)/newSize.x) * src.width);
				int yPos = Mathf.RoundToInt(Mathf.Clamp01((float)(y+1)/newSize.y) * src.height);
				newTex.SetPixel(x,y, src.GetPixel(xPos,yPos));
			}
		}

		newTex.Apply();

		return newTex;
	}
}