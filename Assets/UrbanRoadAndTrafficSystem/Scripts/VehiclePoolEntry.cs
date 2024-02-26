using System;
using UnityEngine;

namespace URNTS
{
    [Serializable]
    public class VehiclePoolEntry
    {
        public string name;
        public GameObject vehiclePrefab;
        public int amount = 5;
    }
}