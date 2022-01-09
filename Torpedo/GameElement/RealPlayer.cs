using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torpedo.Model;
using Torpedo.Settings;

namespace Torpedo.GameElement
{
    public class RealPlayer : Player
    {
        public RealPlayer(string name) : base(name)
        {
        }

        public override void PutDownAShip(Vector shipStartPoint, Vector shipEndPoint)
        {
            if ((shipStartPoint.X != shipEndPoint.X) && (shipStartPoint.Y != shipEndPoint.Y))

                throw new ArgumentException("A hajó kezdő és vég pontjának egybe kell esnie");

            System.Diagnostics.Contracts.Contract.EndContractBlock();

            List<ShipPart> newShipParts = GetShipParts(shipStartPoint, shipEndPoint);
            if (newShipParts.Any(ShipsCoordinate.Contains))
            {
                throw new ArgumentException("Hajók Ütköznek");
            }
            ShipsCoordinate.AddRange(newShipParts);
            ShipCount = ShipCount + 1;
        }
        public override Vector TakeAShot()
        {
            throw new NotImplementedException();
        }

        public List<ShipPart> GetShipParts(Vector shipStartPoint, Vector shipEndPoint)
        {
            List<ShipPart> shipParts = new List<ShipPart>();
            Vector vector = Norm(shipEndPoint - shipStartPoint);
            shipParts.Add(new ShipPart(shipEndPoint));
            shipParts.Add(new ShipPart(shipStartPoint));

            while (!shipParts.Contains(new ShipPart(shipParts.Last().Coordinate + vector)))
            {
                shipParts.Add(new ShipPart( shipParts.Last().Coordinate + vector));
            }

            return shipParts;
        }

        private  Vector Norm(Vector a)
        {
            Vector actual = new Vector(a.X, a.Y);
            if (actual.Y != 0)
            {
                actual.Y = actual.Y / Math.Abs(actual.Y);
            }
            if (actual.X != 0)
            {
                actual.X = actual.X / Math.Abs(actual.X);
            }
            return actual;
        }
    }
}
