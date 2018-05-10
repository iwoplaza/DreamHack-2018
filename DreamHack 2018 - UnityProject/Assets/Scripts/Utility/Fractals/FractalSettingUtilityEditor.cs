using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utility.Noise;
[CustomEditor(typeof(FractalGenerator))]
public class FractalGeneratorUtilityEditor : Editor {

	Texture2D m_currentTex;
	bool m_texUpdated;
	int m_segmentCount;
	float m_filterHeight;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		FractalGenerator t = (target as FractalGenerator);

		if(t.DrawType == FractalGenerator.FractalDrawType.FILTERED || t.DrawType == FractalGenerator.FractalDrawType.FILTERED_BLACK_WHITE)
		{
			m_filterHeight = t.FilterHeight;
			m_filterHeight = EditorGUILayout.Slider("Filter out below",m_filterHeight,0,1);
			t.FilterHeight = m_filterHeight;
		}

		if(t.DrawType == FractalGenerator.FractalDrawType.SEGMENTED)
		{
			m_segmentCount = t.SegmentCount;
			m_segmentCount = EditorGUILayout.IntSlider("Segment count",m_segmentCount,0,32);
			t.SegmentCount = m_segmentCount;
		}

		GUILayout.Space(10);
		if(t.CurrentSeed != null)
			GUILayout.Label("Current seed is: " + t.CurrentSeed.GetHashCode().ToString());
		GUILayout.Space(10);

		if(GUILayout.Button("DRAW NOISE"))
		{
			t.GenerateNoise();
			m_texUpdated = false;
		}

		if(t.CurrentTex != null && !m_texUpdated)
		{
			m_currentTex = ResizeTexture(t.CurrentTex, new Vector2Int(386,386));
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
