using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintYeWagon : MonoBehaviour
{
    [SerializeField] private GameObject wagon;
   // public Paint[] paint;

    private void OnTriggerEnter(Collider other)
    {
        PaintMeWagon();
    }

    void PaintMeWagon()
    {
        if (Dishonour.dishonourLevel > Dishonour._oneStar)
        {
            wagon.GetComponent<Material>().SetColor(name, Color.red);

            //wagon.SetColor(2, Color.green);
        }
    }

 

}
