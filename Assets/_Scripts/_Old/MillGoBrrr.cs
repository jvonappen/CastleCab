using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillGoBrrr : MonoBehaviour
{
    private GameObject brrr;
    [SerializeField] private float _brrrSpeed;
    private void Awake()
    {
        brrr = this.gameObject;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, _brrrSpeed * Time.deltaTime, Space.Self);
    }
}
