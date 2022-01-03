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
        public Player()
        {

        }
        public string? Name { get; set; }
        public List<ShipPart>? ShipsCoordinate { get; set; }
        public List<FiredShot> FiredShots { get; set; }
        public abstract void PutDownAllShip();
        public abstract Vector TakeAShot();
    }
}
