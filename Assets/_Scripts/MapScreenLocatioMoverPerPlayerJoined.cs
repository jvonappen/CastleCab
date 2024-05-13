using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapScreenLocatioMoverPerPlayerJoined : MonoBehaviour
{
    public static MapScreenLocatioMoverPerPlayerJoined Instance;

    [Header("Map")]
    [SerializeField] GameObject mapImage;

    [Header("Boarder")]
    [SerializeField] GameObject boarderImage;

    [Header("Positions")]
    [SerializeField] GameObject pos1;
    [SerializeField] GameObject pos2;
    [SerializeField] GameObject pos3;

    [Header("Map Cam")]
    [SerializeField] private GameObject mapCam;

    [Header("Debug - LookAt Values")]
    [SerializeField] private float X = 0;
    [SerializeField] private float Y = 180;
    [SerializeField] private float Z = 0;

    private void Awake()
    {
        if(Instance == null) { Instance = this; }
    }

    public void MapPosUpdate()
    {
        if (WagonData.playerNumber == 1) { mapImage.transform.position = pos1.transform.position; }
        if (WagonData.playerNumber == 2) { mapImage.transform.position = pos2.transform.position; }
        if (WagonData.playerNumber >= 3) { mapImage.transform.position = pos3.transform.position; }

        boarderImage.transform.position = mapImage.transform.position;
    }
    public void LookAtMiniMap(GameObject playerIcon)
    {
        if(mapCam!=null)
        {
            //playerIcon.transform.LookAt(mapCam.transform);
           //playerIcon.transform.Rotate(X, Y, Z);

            
        }
        else { Debug.Log("It ain't doin' it"); }
        
    }

}
