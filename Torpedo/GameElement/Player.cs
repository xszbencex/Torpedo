using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torpedo.Model;
using Torpedo.Settings;

namespace Torpedo.GameElement
{
    public abstract class Player
    {
        protected Player(string name)
        {
            this.Name = name;
            this.ShipsCoordinate = new List<ShipPart>();
            this.FiredShots = new List<FiredShot>();
            this.ShipCount = 0;
            initShips();
        }

        private void initShips()
        {
            Ships = new List<Ship>();
            foreach (int length in MainSettings.PlayableShipsLength)
            {
                Ships.Add(new Ship(length));
            }
        }

        public string Name { get; set; }
        public List<ShipPart> ShipsCoordinate { get; set; }
        public List<FiredShot> FiredShots { get; set; }
        public int ShipCount { get; set; }
        public List<Ship> Ships { get; private set; }
        public abstract Vector TakeAShot();

        public void PutDownAShip(Vector shipStartPoint, Vector shipEndPoint)
        {
            if ((shipStartPoint.X != shipEndPoint.X) && (shipStartPoint.Y != shipEndPoint.Y))
            {
                throw new ArgumentException("The ship coordinates are not valid!");
            }
            if (!MainSettings.CoordinateValidation(shipStartPoint))
            {
                throw new ArgumentException("Land Captain!!!!");
            }
            if (!MainSettings.CoordinateValidation(shipEndPoint))
            {
                throw new ArgumentException("Land Captain!!!!");
            }

            System.Diagnostics.Contracts.Contract.EndContractBlock();

            List<ShipPart> newShipParts = GetShipParts(shipStartPoint, shipEndPoint);

            if (newShipParts.Count != MainSettings.PlayableShipsLength[ShipCount])
            {
                throw new ArgumentException($"Your ship is not {MainSettings.PlayableShipsLength[ShipCount]} units long!");
            }

            if (newShipParts.ConvertAll(s => s.Coordinate).Any(GetNotValidCoordinates().Contains))
            {
                throw new ArgumentException("You can't put ships next to each other.");
            }
            ShipsCoordinate.AddRange(newShipParts);
            Ships[ShipCount].Parts.AddRange(newShipParts);
            ShipCount = ShipCount + 1;
        }
        public List<ShipPart> GetShipParts(Vector shipStartPoint, Vector shipEndPoint)
        {
            List<ShipPart> shipParts = new List<ShipPart>();
            Vector vector = Norm(shipEndPoint - shipStartPoint);
            shipParts.Add(new ShipPart(shipEndPoint));
            shipParts.Add(new ShipPart(shipStartPoint));

            while (!shipParts.Contains(new ShipPart(shipParts.Last().Coordinate + vector)))
            {
                shipParts.Add(new ShipPart(shipParts.Last().Coordinate + vector));
            }

            return shipParts;
        }

        public List<Vector> GetNotValidCoordinates()
        {
            List<Vector> notValidCoordinates = new List<Vector>();
            notValidCoordinates.AddRange(this.ShipsCoordinate.ConvertAll(s => s.Coordinate));

            notValidCoordinates.AddRange(notValidCoordinates.ConvertAll(s => s + Vector.Down));
            notValidCoordinates.AddRange(notValidCoordinates.ConvertAll(s => s + Vector.Up));
            notValidCoordinates.AddRange(notValidCoordinates.ConvertAll(s => s + Vector.Left));
            notValidCoordinates.AddRange(notValidCoordinates.ConvertAll(s => s + Vector.Right));
            notValidCoordinates = notValidCoordinates.Distinct().ToList();

            return notValidCoordinates;
        }
        private Vector Norm(Vector a)
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
