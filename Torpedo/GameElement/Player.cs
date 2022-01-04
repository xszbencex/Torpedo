using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torpedo.Model;

namespace Torpedo.GameElement
{
    public abstract class Player
    {
        public string? Name { get; set; }
        public List<ShipPart>? ShipsCoordinate { get; set; } = new List<ShipPart>();
        public List<FiredShot> FiredShots { get; set; } = new List<FiredShot>();
        public abstract void PutDownAllShip();
        public abstract Vector TakeAShot();
    }
}
