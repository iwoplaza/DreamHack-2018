using UnityEngine;
using Utility.Noise;

public class FractalGenerator : MonoBehaviour
{
    public enum FractalDrawType
    {        
        ORIGINAL,
        FILTERED,
        FILTERED_BLACK_WHITE,
        SEGMENTED
    }

    float[,] m_currentNoise;
    public float[,] CurrentNoise { get{ return m_currentNoise; }}

    [SerializeField]Vector2Int m_textureRes;
    public Vector2Int CurrentRes { get{ return m_textureRes; } }
    [Range(1,100)]
    [SerializeField]float m_noiseFrequency;
    [Range(1,10)]
    [SerializeField]int m_noiseOctave;

    [Range(0.01f,10.0f)]
    [SerializeField]float m_noiseAmplifier;
    [SerializeField]string m_noiseSeed;
    public string CurrentSeed { get { return m_noiseSeed; } }
    
    [Space(10)]
    [SerializeField]FractalDrawType m_drawType;

    public FractalDrawType DrawType { get { return m_drawType; } }
    
    public float FilterHeight { get; set; }
    public int SegmentCount { get; set; }

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

    public void GenerateFractal(string newSeed)
    {
        m_noiseSeed = newSeed;
        GenerateNoise();
    }

    public void GenerateNoise()
    {
        m_noiseTex = new Texture2D(m_textureRes.x,m_textureRes.y);
        float[,] noise = PerlinFractalNoiseGenerator.GenerateNoise
                        (m_textureRes.x, m_textureRes.y, m_noiseFrequency
                        , m_noiseOctave, m_noiseAmplifier, m_noiseSeed);
        
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
                if(m_drawType == FractalDrawType.SEGMENTED)
                {
                    float colorPoint = (float)Mathf.RoundToInt(noise[x,y] * SegmentCount)/SegmentCount;
                    m_currentNoise[x,y] = colorPoint;
                    Color color = new Color(colorPoint,colorPoint,colorPoint,1);
                    m_noiseTex.SetPixel(x,y,color);             
                }
            }
        }

        m_noiseTex.Apply(true);
    }
}