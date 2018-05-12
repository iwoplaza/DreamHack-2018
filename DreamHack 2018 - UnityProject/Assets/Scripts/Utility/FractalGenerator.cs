using UnityEngine;
using Utility.Noise;

namespace Utility.Noise
{
    [System.Serializable]
    public class FractalGenerator : MonoBehaviour
    {
        public enum FractalDrawType
        {        
            ORIGINAL,
            FILTERED,
            FILTERED_BLACK_WHITE,
            SEGMENTED,
            SEGMENTED_FILTERED,

            CIRCLE_GRADIENT,
            CIRCLE_GRADIENT_SEGMENTED
        }

        float[,] m_currentNoise;
        public float[,] CurrentNoise { get{ return m_currentNoise; }}

        [SerializeField]Vector2Int m_textureRes;
        public Vector2Int CurrentRes { get{ return m_textureRes; } set { m_textureRes = value; } }
        [Range(1,100)]
        [SerializeField]float m_noiseFrequency;
        [Range(1,10)]
        [SerializeField]int m_noiseOctave;

        [Range(0.01f,10.0f)]
        [SerializeField]float m_noiseAmplifier;
        [SerializeField]string m_noiseSeed;
        public string CurrentSeed { get { return m_noiseSeed; } set { m_noiseSeed = value; } }
        
        [Space(10)]
        [SerializeField]FractalDrawType m_drawType;

        public FractalDrawType DrawType { get { return m_drawType; } }
        
        [SerializeField][HideInInspector]float m_filterHeight;
        [SerializeField][HideInInspector]int m_segmentCount;
        [SerializeField][HideInInspector]float m_circleRadius;
        [SerializeField][HideInInspector]float m_circlePower;

        public float FilterHeight { get{ return m_filterHeight; } set { m_filterHeight = value; } }
        public int SegmentCount { get{ return m_segmentCount; } set { m_segmentCount = value; } }
        public float CircleRadius { get{ return m_circleRadius; } set { m_circleRadius = value; } }
        public float CirclePower { get{ return m_circlePower; } set { m_circlePower = value; } }

        Texture2D m_noiseTex;

        public Texture2D CurrentTex { get { return m_noiseTex; } }

        public void Reset()
        {
            m_noiseSeed = "seed";
            m_textureRes = new Vector2Int(512,512);
            m_noiseFrequency = 1;
            m_noiseOctave = 1;
            m_noiseAmplifier = 1;
        }

        public void GenerateFractal()
        {
            GenerateNoise();
        }

        public void GenerateFractal(Vector2Int res)
        {
            m_textureRes = res;
            GenerateNoise();
        }

        public void GenerateFractal(string newSeed)
        {
            m_noiseSeed = newSeed;
            GenerateNoise();
        }

        public void GenerateFractal(string newSeed, Vector2Int res)
        {
            m_textureRes = res;
            m_noiseSeed = newSeed;
            GenerateNoise();
        }

        public void GenerateNoise()
        {
            m_noiseTex = new Texture2D(m_textureRes.x,m_textureRes.y);
            float[,] noise = new float[0,0];
            if(m_drawType != FractalDrawType.CIRCLE_GRADIENT)
            {
                noise = PerlinFractalNoiseGenerator.GenerateNoise
                            (m_textureRes.x, m_textureRes.y, m_noiseFrequency
                            , m_noiseOctave, m_noiseAmplifier, m_noiseSeed);
            }
            m_currentNoise = new float[m_textureRes.x,m_textureRes.y];
            if(m_drawType == FractalDrawType.ORIGINAL){
                m_currentNoise = noise;
            }
            for(int x = 0; x < m_textureRes.x; x++)
            {
                for(int y = 0; y < m_textureRes.y; y++)
                {
                    if(m_drawType == FractalDrawType.ORIGINAL){
                        m_currentNoise = noise;
                        Color color = new Color(noise[x,y],noise[x,y],noise[x,y],1);
                        m_noiseTex.SetPixel(x,y,color);
                    }
                    if(m_drawType == FractalDrawType.FILTERED_BLACK_WHITE)
                    {
                        if(noise[x,y] > FilterHeight)
                        {
                            m_currentNoise[x,y] = 1;
                            Color color = new Color(1,1,1,1);
                            m_noiseTex.SetPixel(x,y,color);
                        }
                        else
                        {
                            m_currentNoise[x,y] = 0;
                            Color color = new Color(0,0,0,1);
                            m_noiseTex.SetPixel(x,y,color);
                        }
                    }
                    if(m_drawType == FractalDrawType.FILTERED)
                    {
                        if(noise[x,y] > FilterHeight)
                        {
                            m_currentNoise[x,y] = noise[x,y];
                            Color color = new Color(noise[x,y],noise[x,y],noise[x,y],1);
                            m_noiseTex.SetPixel(x,y,color);
                        }
                        else
                        {
                            m_currentNoise[x,y] = 0;
                            Color color = new Color(0,0,0,1);
                            m_noiseTex.SetPixel(x,y,color);
                        }
                    }
                    if(m_drawType == FractalDrawType.SEGMENTED || m_drawType == FractalDrawType.SEGMENTED_FILTERED)
                    {
                        float colorPoint = (float)Mathf.RoundToInt(noise[x,y] * SegmentCount)/SegmentCount;
                        if(m_drawType == FractalDrawType.SEGMENTED_FILTERED)
                        {
                            if(noise[x,y] > m_filterHeight)
                            {
                                m_currentNoise[x,y] = colorPoint;
                                Color color = new Color(colorPoint,colorPoint,colorPoint,1);
                                m_noiseTex.SetPixel(x,y,color);
                            }
                            else
                            {
                                colorPoint = 0;
                                m_currentNoise[x,y] = colorPoint;
                                Color color = new Color(colorPoint,colorPoint,colorPoint,1);
                                m_noiseTex.SetPixel(x,y,color);
                            }
                        }
                        else
                        {
                            m_currentNoise[x,y] = colorPoint;
                            Color color = new Color(colorPoint,colorPoint,colorPoint,1);
                            m_noiseTex.SetPixel(x,y,color);
                        }
                    }
                    if(m_drawType == FractalDrawType.CIRCLE_GRADIENT || m_drawType == FractalDrawType.CIRCLE_GRADIENT_SEGMENTED)
                    {
                        float resolutionRatio = (float)m_textureRes.x/(float)m_textureRes.y;
                        float xPosition = ((float)x / resolutionRatio);
                        float yPosition = ((float) y);
                        xPosition = (float)m_textureRes.y * 0.5f - xPosition;
                        yPosition = (float)m_textureRes.y * 0.5f - yPosition;

                        float pixelRadius = (float)m_textureRes.y * 0.5f * m_circleRadius;
                        
                        float colorPoint = 0;

                        float distToCenter = new Vector2(xPosition,yPosition).magnitude/pixelRadius;
                        
                        if(distToCenter <= 1)
                        {
                            colorPoint = Mathf.Pow(1f- distToCenter, m_circlePower);
                            if(m_drawType == FractalDrawType.CIRCLE_GRADIENT_SEGMENTED)
                            {
                                colorPoint = (float)Mathf.RoundToInt(colorPoint * SegmentCount)/SegmentCount;
                            }
                            Color color = new Color(colorPoint,colorPoint,colorPoint,1);
                            m_currentNoise[x,y] = colorPoint;
                            m_noiseTex.SetPixel(x,y,color);
                        }
                        else
                        {
                            colorPoint = 0;
                            Color color = new Color(colorPoint,colorPoint,colorPoint,1);
                            m_currentNoise[x,y] = colorPoint;
                            m_noiseTex.SetPixel(x,y,color);
                        }
                    }
                }
            }

            m_noiseTex.Apply(true);
        }
    }
}