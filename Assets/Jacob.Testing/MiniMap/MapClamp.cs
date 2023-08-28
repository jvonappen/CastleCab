using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapClamp : MonoBehaviour
{
    public static Transform minimapCamTransform;
    [SerializeField] private float MinimapSize;
    Vector3 TempV3;

    void Update()
    {
        TempV3 = transform.parent.transform.position;
        TempV3.y = transform.position.y;
        transform.position = TempV3;
    }

    void LateUpdate()
    {
        // Center of Minimap
        Vector3 centerPosition = minimapCamTransform.transform.localPosition;

        // Just to keep a distance between Minimap camera and this Object (So that camera don't clip it out)
        centerPosition.y = 10;

        // Distance from the gameObject to Minimap
        float Distance = Vector3.Distance(transform.position, centerPosition);

        // If the Distance is less than MinimapSize, it is within the Minimap view and we don't need to do anything
        // But if the Distance is greater than the MinimapSize, then do this
        if (Distance > MinimapSize)
        {
            // Gameobject - Minimap
            Vector3 fromOriginToObject = transform.position - centerPosition;

            // Multiply by MinimapSize and Divide by Distance
            fromOriginToObject *= MinimapSize / Distance;

            // Minimap + above calculation
            transform.position = centerPosition + fromOriginToObject;
        }
    }
}
