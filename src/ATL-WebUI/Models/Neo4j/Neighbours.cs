using System.Collections.Generic;

namespace ATL_WebUI.Models
{
    public class ValuesNeighbours
    {
        public string Node { get; set; }
        public string Neighbour { get; set; }
    }

    public class Neighbours
    {
        public ValuesNeighbours Values { get; set; }
        public List<string> Keys { get; set; }
    }
}
