using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassBar : MonoBehaviour
{
    [SerializeField] private RectTransform compassTransform;
    [SerializeField] private RectTransform objectiveMarkerTransform;
    [SerializeField] private RectTransform northMarkerTransform;
    [SerializeField] private RectTransform southMarkerTransform;
    [SerializeField] private RectTransform eastMarkerTransform;
    [SerializeField] private RectTransform westMarkerTransform;

    [SerializeField] private RectTransform northEastMarkerTransform;
    [SerializeField] private RectTransform northWestMarkerTransform;
    [SerializeField] private RectTransform southEastMarkerTransform;
    [SerializeField] private RectTransform southWestMarkerTransform;

    [SerializeField] private Transform cameraObjectTransform;
    public static Transform objectiveObjectTransform;

    [SerializeField] private GameObject objectiveMarkerImage;
    //[Header("Debug")]

    private void Start()
    {
        
    }

    void Update()
    {
        SetMarkerPosition(northMarkerTransform, Vector3.forward * 10000);
        SetMarkerPosition(southMarkerTransform, Vector3.back * 10000);
        SetMarkerPosition(eastMarkerTransform, Vector3.right * 10000);
        SetMarkerPosition(westMarkerTransform, Vector3.left * 10000);

        SetMarkerPosition(northEastMarkerTransform, (Vector3.forward + Vector3.right) / 2 * 10000);
        SetMarkerPosition(northWestMarkerTransform, (Vector3.forward + Vector3.left) / 2 * 10000);
        SetMarkerPosition(southWestMarkerTransform, (Vector3.back - Vector3.right) / 2 * 10000);
        SetMarkerPosition(southEastMarkerTransform, (Vector3.back - Vector3.left) / 2 * 10000);

        if (objectiveObjectTransform == null)
        {
            objectiveMarkerImage.SetActive(false);
        }
        if(objectiveObjectTransform != null)
        {
            objectiveMarkerImage.SetActive(true);
            SetMarkerPosition(objectiveMarkerTransform, objectiveObjectTransform.position);
        }
    }

    private void SetMarkerPosition(RectTransform markerTransform, Vector3 worldPos)
    {
            Vector3 directionToTarget = worldPos - cameraObjectTransform.position;
            float angle = Vector2.SignedAngle(new Vector2(directionToTarget.x, directionToTarget.z), new Vector2(cameraObjectTransform.transform.forward.x, cameraObjectTransform.transform.forward.z));
            float compassPosX = Mathf.Clamp(2 * angle / Camera.main.fieldOfView, -1, 1);
            markerTransform.anchoredPosition = new Vector2(compassTransform.rect.width / 2 * compassPosX, 0);
    }
}
