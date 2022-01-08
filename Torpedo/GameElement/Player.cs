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
        }

        public abstract void PutDownAShip(Vector shipStartPoint, Vector shipEndPoint);
        public string Name { get; set; }
        public List<ShipPart> ShipsCoordinate { get; set; }
        public List<FiredShot> FiredShots { get; set; }
        public int ShipCount { get; set; }
        public abstract Vector TakeAShot();
        /// <summary>
        /// decides whether the position for the given Ship length is correct
        /// </summary>
        /// <param name="position">Vector[] 0. elemnt is the Ship start point 1. elment is the direction of the ship</param>
        /// <param name="length">The length of the Ship</param>
        /// <returns>true if positon is correct and false otherwise</returns>
        /*protected bool IsShipPositionValid(Vector StartOfTheShip, Vector endOfTheShip)
        {
            if (!MainSettings.CoordinateValidation(StartOfTheShip))
            {
                return false;
            }

            if (!MainSettings.CoordinateValidation(endOfTheShip))
            {
                return false;
            }

            for (int i = 0; i < length; i++)
            {
                if (this.ShipsCoordinate.Exists(s => s.Coordinate == (position[0] + (position[1] * i))))
                {
                    return false;
                }
            }
            return true;
        }*/
    }
}
