using System;
using UnityEngine;

namespace URNTS
{
    [Serializable, SerializeField]
    public class TrafficNode : MonoBehaviour
    {
        public Vector3 right;
        public bool signal = true;
        [HideInInspector]
        public byte maxLanes = 4;
        public int nodeInd = 0;

        public void Initialize(Vector3 rootPos, Vector3 right)
        {
            transform.position = rootPos;
            this.right = right;
        }
        public Vector3 GetNodePos(byte lane, byte totalLanes, bool invert)
        {
            return transform.position + (invert ? -1 : 1) * right * ((2 * lane - 1) / (float)totalLanes);
        }

        public Vector3 GetNodePos(float fraction, bool invert)
        {
            return transform.position + (invert ? -1 : 1) * right * fraction;
        }

        public bool IsOnRight(Vector3 pos)
        {
            return Vector3.Dot(right, pos - transform.position) > 0;
        }

    }
}