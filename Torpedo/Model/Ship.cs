using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torpedo.Model
{
    public class Ship
    {
        public Ship(int length)
        {
            this.Length = length;
            this.IsDestroyed = false;
            this.Parts = new List<ShipPart>();
        }

        public int Length { get; set; }
        public List<ShipPart> Parts { get; set; }
        public bool IsDestroyed { get; private set; }

        public void Update()
        {
            if (this.Parts.All(p => p.Destroyed))
            {
                IsDestroyed = true;
            }
        }
    }
}
