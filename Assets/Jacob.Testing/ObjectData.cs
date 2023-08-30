using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object Data", menuName = "Object Data")]
public class ObjectData : ScriptableObject
{
    public int dishonourAmount;
    public string sfxName;
    //particles

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Dishonour.dishonourLevel = Dishonour.dishonourLevel + dishonourAmount;
            AudioManager.Instance.PlaySFX(sfxName);
            //play particles
        }
    }
}


