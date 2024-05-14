using UnityEngine;

public class MapScreenLocation : MonoBehaviour
{
    public static MapScreenLocation Instance;

    [Header("Map")]
    [SerializeField] GameObject mapImage;
    [SerializeField] GameObject borderImage;

    [Header("Positions")]
    [SerializeField] GameObject pos1;
    [SerializeField] GameObject pos2;
    [SerializeField] GameObject pos3;

    private void Awake()
    {
        if(Instance == null) { Instance = this; }
    }

    public void MapPosUpdate()
    {
        if (WagonData.playerNumber == 1) { mapImage.transform.position = pos1.transform.position; }
        if (WagonData.playerNumber == 2) { mapImage.transform.position = pos2.transform.position; }
        if (WagonData.playerNumber >= 3) { mapImage.transform.position = pos3.transform.position; }

        borderImage.transform.position = mapImage.transform.position;
    }
}
