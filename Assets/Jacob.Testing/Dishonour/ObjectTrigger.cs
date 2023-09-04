using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class ObjectTrigger : MonoBehaviour
{
    [SerializeField] private ObjectData _objData;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Dishonour.dishonourLevel = Dishonour.dishonourLevel + _objData.dishonourAmount;
            AudioManager.Instance.PlaySFX(_objData.sfxName);
            //play _objData.particles etc...
        }
    }
}
