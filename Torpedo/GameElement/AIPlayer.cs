using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torpedo.Model;
using Torpedo.Settings;

namespace Torpedo.GameElement
{
    public class AIPlayer : Player
    {
        // TODO AIPlayer implementáció (csak átmásoltam a RealPlayert)
        public AIPlayer(string name) : base(name)
        {
        }

        public void PutDownAllShip()
        {
            var random = new Random();
            while (MainSettings.PlayableShipsLength.Length != ShipCount)
            {
                try
                {
                    Vector shipStartPoint = new Vector(random.Next(MainSettings.GridWidth), random.Next(MainSettings.GridHeight));
                    PutDownAShip(shipStartPoint, GetShipEndPoint(shipStartPoint));
                }
                catch (Exception)
                {
                }
            }
        }

        private Vector GetShipEndPoint(Vector shipStartPoint)
        {
            var random = new Random();
            Vector direction;
            switch (random.Next(3))
            {
                case 0: direction = Vector.Up; break;
                case 1: direction = Vector.Down; break;
                case 2: direction = Vector.Left; break;
                default: direction = Vector.Right; break;
            }
            return shipStartPoint + (direction * (MainSettings.PlayableShipsLength[ShipCount] - 1));
        }

        public override Vector TakeAShot()
        {
            var random = new Random();
            Vector shot = new Vector(random.Next(MainSettings.GridWidth), random.Next(MainSettings.GridHeight));

            List<Vector> desirableTarget = new List<Vector>();

            FiredShots.Where(shot => shot.Hit == true).ToList().ForEach(s =>
            {
                desirableTarget.Add(s.Coordinate + Vector.Up);
                desirableTarget.Add(s.Coordinate + Vector.Down);
                desirableTarget.Add(s.Coordinate + Vector.Right);
                desirableTarget.Add(s.Coordinate + Vector.Left);
            });
            if (desirableTarget.Count != 0)
            {
                return desirableTarget[random.Next(desirableTarget.Count)];
            }

            while (FiredShots.Where(s => s.Coordinate == shot).Any())
            {
                shot = new Vector(random.Next(MainSettings.GridWidth), random.Next(MainSettings.GridHeight));
            }
            if (MainSettings.CoordinateValidation(shot))
            {
                return shot;
            }
            throw new Exception("shot is not on the table");
        }
    }
}
