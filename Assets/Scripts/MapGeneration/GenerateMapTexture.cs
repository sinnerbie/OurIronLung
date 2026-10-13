using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;

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
    private Renderer rend;

    [Header("References")]
    public Material debugMat;
    public GameObject player;
    public GameObject goal;
    public Text description;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    void Start()
    {
        noiseTexture = new Texture2D(width + 2, height + 2);
        pixels = new Color[noiseTexture.width * noiseTexture.height];
        rend.material.mainTexture = noiseTexture;
        CalculateNoise();
    }

    [ContextMenu("Try")]
    public void CalculateNoise()
    {
        if (GameObject.Find("Wall") != null)
            DeleteAllWalls();
        noiseTexture = new Texture2D(width + 2, height + 2);
        pixels = new Color[noiseTexture.width * noiseTexture.height];
        rend.material.mainTexture = noiseTexture;
        Dictionary<int, Vector3> locations = new Dictionary<int, Vector3>();
        for (float y = 0; y < height; y++)
        {
            for (float x = 0; x < width; x++)
            {
                if ((x == 0 || y == 0) || (x == width - 1 || y == height - 1))
                    pixels[(int)y * noiseTexture.width + (int)x] = new Color(0, 1, 0, 1);
                else if ((x == 1 || y == 1) || (x == width - 2 || y == height - 2))
                    pixels[(int)y * noiseTexture.width + (int)x] = new Color(0, 0, 0, 1);
                else
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
                        locations.Add((int)y * noiseTexture.width + (int)x, new Vector3(x, y, 0));
                    }
                    pixels[(int)y * noiseTexture.width + (int)x] = new Color(0, sample, 0, 1);
                }
            }
        }
        SetStartAndGoal(locations);
        GenerateBounds();
        noiseTexture.SetPixels(pixels);
        noiseTexture.Apply();
    }

    void CreateCollision(float x, float y)
    {
        Vector3 pos = new Vector3(x * scale, y * scale, 0);
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.transform.localScale = new Vector3(scale, scale, scale);
        wall.transform.position = pos;
        wall.layer = 3;
        debugMat.color = Color.green;
        wall.GetComponent<Renderer>().material = debugMat;
        wall.name = "Wall";
    }

    void GenerateBounds()
    {
        for (float y = 0; y < height; y++)
        {
            CreateCollision(0, y);
            CreateCollision(width - 1, y);
        }

        for (float x = 0; x < width; x++)
        {
            CreateCollision(x, 0);
            CreateCollision(x, height - 1);
        }
    }

    void DeleteAllWalls()
    {
        var walls = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Wall");
        foreach (GameObject ob in walls)
            Destroy(ob);
    }

    void SetStartAndGoal(Dictionary<int, Vector3> locations)
    {
        string locationText = "";
        System.Random rng = new System.Random();
        var shuffledList = locations.OrderBy(x => rng.Next()).ToList();
        Vector3 playPos = new Vector3(shuffledList[0].Value.x * scale, shuffledList[0].Value.y * scale, 0);
        locationText += "Start Coordinate = x." + shuffledList[0].Value.x + " y." + shuffledList[0].Value.y; 
        pixels[shuffledList[0].Key] = new Color(1, 0, 0, 1);
        Vector3 goalPos = new Vector3(shuffledList[1].Value.x * scale, shuffledList[1].Value.y * scale, 0);
        locationText += "\nGoal  Coordinate = x." + shuffledList[1].Value.x + " y." + shuffledList[1].Value.y;
        description.text = locationText;
        pixels[shuffledList[1].Key] = new Color(1, 1, 0, 1);
        player.transform.position = playPos;
        player.transform.rotation = Quaternion.identity;
        goal.transform.position = goalPos;
        goal.transform.localScale = new Vector3(scale, scale, scale);
    }
}
