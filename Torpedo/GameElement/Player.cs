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
        protected Player(IInput input, string name)
        {
            _input = input;
            this.Name = name;
            this.ShipsCoordinate = new List<ShipPart>();
            this.FiredShots = new List<FiredShot>();
        }
        private protected IInput _input;
        public string Name { get; set; }
        public List<ShipPart> ShipsCoordinate { get; set; }
        public List<FiredShot> FiredShots { get; set; }
        public abstract void PutDownAllShip();
        public abstract Vector TakeAShot();
    }
}
