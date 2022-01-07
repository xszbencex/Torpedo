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
        protected Player(string name)
        {
            this.Name = name;
            this.ShipsCoordinate = new List<ShipPart>();
            this.FiredShots = new List<FiredShot>();
        }
        private protected IInput? _input;

        public abstract void PutDownAShip(Vector shipStartPoint, Vector shipEndPoint);
        public string Name { get; set; }
        public List<ShipPart> ShipsCoordinate { get; set; }
        public List<FiredShot> FiredShots { get; set; }
        public abstract void PutDownAllShip();
        public abstract Vector TakeAShot();
    }
}
