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
        public AIPlayer() : base("Bot")
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
            var desirableTarget = new List<Vector>();
            var destroyedShip = new List<Vector>();

            List<FiredShot> hits = FiredShots.Where(s => s.Hit).ToList();

            if (hits.Any())
            {
                foreach (var hit in hits)
                {
                    foreach (Vector direction in _directions)
                    {
                        if (hits.Where(s => s.Coordinate == (hit.Coordinate + direction)).Any())
                        {
                            List<Vector> target = new List<Vector>();
                            target.Add(hit.Coordinate - direction);
                            if (InValidTargetElimination(target).Any())
                            {
                                return target.First();
                            }
                            destroyedShip.Add(hit.Coordinate + (new Vector(direction.Y, direction.X)));
                            destroyedShip.Add(hit.Coordinate + (new Vector(direction.Y, direction.X) * -1));
                        }
                    }
                }
 
                foreach (Vector direction in _directions)
                {
                    hits.ToList().ForEach(h => desirableTarget.Add(h.Coordinate + direction));
                }
            }
            desirableTarget = InValidTargetElimination(desirableTarget);
            desirableTarget.RemoveAll(destroyedShip.Contains);

            if (desirableTarget.Count != 0)
            {
                return desirableTarget[random.Next(desirableTarget.Count)];
            }

            Vector shot = new Vector(random.Next(MainSettings.GridWidth), random.Next(MainSettings.GridHeight));
            while (FiredShots.Where(s => s.Coordinate == shot).Any() || destroyedShip.Where(s => s == shot).Any())
            {
                shot = new Vector(random.Next(MainSettings.GridWidth), random.Next(MainSettings.GridHeight));
            }
            if (MainSettings.CoordinateValidation(shot))
            {

                return shot;
            }
            throw new Exception("shot is not on the table");
        }

        private List<Vector> InValidTargetElimination(List<Vector> desirableTarget)
        {
            desirableTarget = desirableTarget.Where(s => MainSettings.CoordinateValidation(s)).ToList();

            desirableTarget = desirableTarget.Where(s => !FiredShots.Contains(new FiredShot(s, true))).ToList();
            return desirableTarget;
        }
        /*
            if (desirableTarget.Count != 0)
            {
                return desirableTarget[random.Next(desirableTarget.Count)];
            }

            desirableTarget = LockingOnToATarget();

            if (desirableTarget.Count != 0)
            {
                return desirableTarget[random.Next(desirableTarget.Count)];
            }

            Vector shot = new Vector(random.Next(MainSettings.GridWidth), random.Next(MainSettings.GridHeight));

            List<Vector> desirableTarget = new List<Vector>();

            FiredShots
                .Where(shot => shot.Hit == true)
                .ToList()
                .ForEach(s =>
            {
                desirableTarget.Add(s.Coordinate + Vector.Up);
                desirableTarget.Add(s.Coordinate + Vector.Down);
                desirableTarget.Add(s.Coordinate + Vector.Right);
                desirableTarget.Add(s.Coordinate + Vector.Left);
            });

            // Nincs Test
            desirableTarget = desirableTarget.Where(s => MainSettings.CoordinateValidation(s)).ToList();

            // Nincs Test
            desirableTarget = desirableTarget.Where(s => !FiredShots.Contains(new FiredShot(s, true))).ToList();

            if (desirableTarget.Count != 0)
            {
                return desirableTarget[random.Next(desirableTarget.Count)];
            }

        private bool IsShipDistroyedInTheDirection(Vector hit, Vector direction, int depth)
        {
            var actual = hit + direction;
            var shot = FiredShots.Find(s => s.Coordinate == actual);
            if (shot != null)
            {
                if (shot.Hit)
                {
                   return IsShipDistroyedInTheDirection(actual, direction, depth + 1);
                }
                else
                {

                    return true;
                }
            }
            throw new Exception("shot is not on the table");
        }
        */
    }
}
