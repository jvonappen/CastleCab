using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpin : MonoBehaviour
{
    public float degreePerSec = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    // Position Storage Variables
    Vector3 positionOffset = new Vector3();
    Vector3 tempPosition = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        positionOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, 100f * Time.deltaTime, 0f), Space.Self);


        tempPosition = positionOffset;
        tempPosition.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPosition;
    }
}
