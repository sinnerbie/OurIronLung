using UnityEngine.UI;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    [SerializeField] GenerateMapTexture map;

    [SerializeField] InputField coordinateX, coordinateY;
    [SerializeField] InputField mapResolution;
    [SerializeField] InputField mapScale;

    private int prevCoordX, prevCoordY;
    private int prevMapRes;
    private float prevMapScale;

    void Start()
    {
        coordinateX.text = map.xOrigin.ToString();
        coordinateY.text = map.yOrigin.ToString();
        mapResolution.text = (map.height * 2).ToString();
        mapScale.text = map.scale.ToString();
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

        if (mapResolution.text != prevMapRes.ToString())
        {
            prevMapRes = int.Parse(mapResolution.text);
            map.height = prevMapRes / 2;
            map.width = prevMapRes / 2;
        }

        if (mapScale.text != prevMapScale.ToString())
        {
            prevMapScale = float.Parse(mapScale.text);
            map.scale = prevMapScale;
        }

        map.CalculateNoise();
    }
}
