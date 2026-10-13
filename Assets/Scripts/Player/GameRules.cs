using UnityEngine.UI;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    [SerializeField] GenerateMapTexture map;

    [SerializeField] InputField coordinateX, coordinateY;
    [SerializeField] InputField mapArea;
    [SerializeField] InputField mapResolution;

    private int prevCoordX, prevCoordY;
    private int prevMapArea;
    private float prevMapRes;

    void Start()
    {
        coordinateX.text = map.xOrigin.ToString();
        coordinateY.text = map.yOrigin.ToString();
        mapArea.text = (map.height * 2).ToString();
        mapResolution.text = map.scale.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            Debug.Log("YOU WIN!");
        }
    }

    public void ChangeStartAndGoal()
    {
        map.CalculateNoise();
    }

    public void ApplyNewMapDimentions()
    {
        if (coordinateX.text != prevCoordX.ToString())
        {
            prevCoordX = int.Parse(coordinateX.text);
            map.xOrigin = prevCoordX;
        }

        if (coordinateY.text != prevCoordY.ToString())
        {
            prevCoordY = int.Parse(coordinateY.text);
            map.yOrigin = prevCoordY;
        }

        if (mapArea.text != prevMapArea.ToString())
        {
            prevMapArea = int.Parse(mapArea.text);
            map.height = prevMapArea / 2;
            map.width = prevMapArea / 2;
        }

        if (mapResolution.text != prevMapRes.ToString())
        {
            prevMapRes = float.Parse(mapResolution.text);
            map.scale = prevMapRes;
        }

        map.CalculateNoise();
    }
}
