using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateMapTexture : MonoBehaviour
{
    [Header("Map Details")]
    public int height;
    public int width;
    public float xOrigin;
    public float yOrigin;
    public float scale;
    private Texture2D noiseTexture;
    private Color[] pixels;

    [Header("References")]
    public Material debugMat;
    public GameObject cam;
    public GameObject player;
    public GameObject goal;

    void Start()
    {
        noiseTexture = new Texture2D(width, height);
        pixels = new Color[noiseTexture.width * noiseTexture.height];
        CalculateNoise();
    }

    [ContextMenu("Try")]
    void CalculateNoise()
    {
        if (GameObject.Find("Wall") != null)
            DeleteAllWalls();
        noiseTexture = new Texture2D(width, height);
        pixels = new Color[noiseTexture.width * noiseTexture.height];
        List<Vector3> locations = new List<Vector3>();
        for (float y = 0; y < height; y++)
        {
            for (float x = 0; x < width; x++)
            {
                float xCoord = xOrigin + x / noiseTexture.width * scale;
                float yCoord = yOrigin + y / noiseTexture.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                if (sample > 0.5f)
                {
                    sample = 1;
                    CreateCollision(x, y);
                }
                else if (sample <= 0.5f)
                {
                    sample = 0;
                    locations.Add(new Vector3(x, y, 0));
                }
                pixels[(int)y * noiseTexture.width + (int)x] = new Color(sample, sample, sample);
            }
        }
        SetStartAndGoal(locations);
        GenerateBounds();
        PositionCamera();
        noiseTexture.SetPixels(pixels);
        noiseTexture.Apply();
    }

    void CreateCollision(float x, float y)
    {
        Vector3 pos = new Vector3(x * scale, y * scale, 0);
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.transform.localScale = new Vector3(scale, scale, scale);
        wall.transform.position = pos;
        debugMat.color = Color.green;
        wall.GetComponent<Renderer>().material = debugMat;
        wall.name = "Wall";
    }

    void GenerateBounds()
    {
        for (float y = -2; y < height + 2; y++)
        {
            CreateCollision(-2, y);
            CreateCollision(width + 1, y);
        }

        for (float x = -2; x < width + 2; x++)
        {
            CreateCollision(x, -2);
            CreateCollision(x, height + 2);
        }
    }

    void DeleteAllWalls()
    {
        var walls = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Wall");
        foreach (GameObject ob in walls)
            Destroy(ob);
    }

    void PositionCamera()
    {
        Vector3 pos = new Vector3((width * scale) / 2, (height * scale) / 2, -10);
        cam.transform.position = pos;
        cam.GetComponent<Camera>().orthographicSize = (scale / 2.5f) * 71;
    }

    void SetStartAndGoal(List<Vector3> locations)
    {
        System.Random rng = new System.Random();
        var shuffledList = locations.OrderBy(x => rng.Next()).ToList();

        Vector3 playPos = new Vector3(shuffledList[0].x * scale, shuffledList[0].y * scale, 0);
        Vector3 goalPos = new Vector3(shuffledList[1].x * scale, shuffledList[1].y * scale, 0);
        player.transform.position = playPos;
        player.transform.rotation = Quaternion.identity;
        goal.transform.position = goalPos;
        goal.transform.localScale = new Vector3(scale, scale, scale);
    }
}
