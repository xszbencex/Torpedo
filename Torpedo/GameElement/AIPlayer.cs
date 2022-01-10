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

        private readonly Vector[] _directions = new Vector[4] { Vector.Up, Vector.Down, Vector.Right, Vector.Left };
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
            while (FiredShots.Where(s => s.Coordinate == shot).Any() && destroyedShip.Where(s => s == shot).Any())
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

        private List<Vector> LockingOnToATarget()
        {
            List<Vector> desirableTarget = new List<Vector>();
            FiredShots
                .Where(shot => shot.Hit == true)
                .ToList()
                .ForEach(s =>
            {
                foreach (Vector direction in _directions)
                {
                    desirableTarget.Add(s.Coordinate + direction);
                }
            });

            desirableTarget = desirableTarget.Where(s => MainSettings.CoordinateValidation(s)).ToList();

            desirableTarget = desirableTarget.Where(s => !FiredShots.Contains(new FiredShot(s, true))).ToList();

            return InValidTargetElimination(DestroydShipElimination(desirableTarget));
        }

        private List<Vector> LockedTarget()
        {
            List<Vector> desirableTarget = new List<Vector>();

            FiredShots
                .Where(s => s.Hit)
                .ToList()
                .ForEach(actual =>
            {
                List<FiredShot> Hits = FiredShots.Where(s => s.Hit).ToList();
                foreach (Vector direction in _directions)
                {
                    if (Hits.Any(s => s.Coordinate == (actual.Coordinate + direction)))
                    {
                        desirableTarget.Add(actual.Coordinate - direction);
                    }
                }
            });
            return InValidTargetElimination(desirableTarget);
        }

        private List<Vector> InValidTargetElimination(List<Vector> desirableTarget)
        {
            desirableTarget = desirableTarget.Where(s => MainSettings.CoordinateValidation(s)).ToList();

            desirableTarget = desirableTarget.Where(s => !FiredShots.Contains(new FiredShot(s, true))).ToList();
            return desirableTarget;
        }

        private List<Vector> DestroydShipElimination(List<Vector> desirableTarget)
        {
            List<Vector> destroyedShip = new List<Vector>();
            List<Vector> hits = FiredShots
                .Where(s => s.Hit)
                .Select(s => s.Coordinate).ToList();
            foreach (Vector hit in hits)
            {
                if (IsPartOfDestroyedShip(hit))
                {
                    foreach(Vector direction in _directions)
                    {
                        destroyedShip.Add(hit + direction);
                    }
                }
            }
            desirableTarget = desirableTarget.Where(s => !destroyedShip.Contains(s)).ToList();
            return desirableTarget;
        }

        private bool IsPartOfDestroyedShip(Vector hit)
        {
            return (IsShipDistroyedInTheDirection(hit, Vector.Up, 0) && IsShipDistroyedInTheDirection(hit, Vector.Down, 0))
                || (IsShipDistroyedInTheDirection(hit, Vector.Right, 0) && IsShipDistroyedInTheDirection(hit, Vector.Left, 0));
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
            return false;

        }
        */
    }
}
