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
        public ShipPart(Vector? coordinate) // TODO nem biztos, hogy kell a ?
        {
            this.Coordinate = coordinate;
            this.Destroyed = false;
        }

        public Vector? Coordinate { get; set; }
        public bool Destroyed { get; set; } = false;

        public override bool Equals(object? obj)
        {
            return obj is ShipPart part &&
                   EqualityComparer<Vector?>.Default.Equals(this.Coordinate, part.Coordinate) &&
                   this.Destroyed == part.Destroyed;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Coordinate, this.Destroyed);
        }
    }
}
