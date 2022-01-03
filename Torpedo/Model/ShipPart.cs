using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torpedo.Model;

namespace Torpedo.Model
{
    public class ShipPart
    {
        public Vector? Coordinate { get; set; }
        public bool Destroyed { get; set; } = false;
    }
}
