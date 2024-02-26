using UnityEngine;

namespace URNTS
{
    [SelectionBase]
    public class TrafficSignal : MonoBehaviour
    {
        public enum SignalType { green,yellow,red};
        Renderer rend;
        public SignalType currentSignal;
        private void Awake()
        {
            rend = GetComponentInChildren<Renderer>();
        }

        // called when signal is changed
        public void OnSignalChange(SignalType signalType)
        {
            currentSignal = signalType;
            if (rend == null) return;
            rend.material.SetVector("_Signal",new Vector3(signalType==SignalType.red?1:0,signalType==SignalType.green?1:0,signalType== SignalType.yellow?1:0));
        }
    }
}

