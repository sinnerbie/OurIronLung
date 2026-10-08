using UnityEngine;

public class GenerateMapTexture : MonoBehaviour
{
    public int height;
    public int width;

    public float xOrigin;
    public float yOrigin;

    public float scale;

    private Texture2D noiseTexture;
    private Color[] pixels;
    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    void Start()
    {
        noiseTexture = new Texture2D(width, height);
        pixels = new Color[noiseTexture.width * noiseTexture.height];
        rend.material.mainTexture = noiseTexture;
        CalculateNoise();
    }

    [ContextMenu("Try")]
    void CalculateNoise()
    {
        noiseTexture = new Texture2D(width, height);
        pixels = new Color[noiseTexture.width * noiseTexture.height];
        rend.material.mainTexture = noiseTexture;
        for (float y = 0; y < height; y++)
        {
            for (float x = 0; x < width; x++)
            {
                float xCoord = xOrigin + x / noiseTexture.width * scale;
                float yCoord = yOrigin + y / noiseTexture.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                pixels[(int)y * noiseTexture.width + (int)x] = new Color(sample, sample, sample);
            }
        }

        noiseTexture.SetPixels(pixels);
        noiseTexture.Apply();
    }
}
