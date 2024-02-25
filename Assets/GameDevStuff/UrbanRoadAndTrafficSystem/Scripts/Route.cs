using System;
using System.Collections.Generic;

namespace URNTS
{
    [Serializable]
    public class Route
    {
        public List<TrafficNode> route;

        public Route(List<TrafficNode> nodeList) { route = nodeList; }
    }
}