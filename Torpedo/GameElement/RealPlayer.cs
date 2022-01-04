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
        public override void PutDownAllShip()
        {
            foreach (int length in MainSettings.PlayableShipsLength)
            {
                Vector[] position;
                do
                {
                    position = GetNewShipPosition(length);
                }
                while (IsShipPositionValid(position, length));
                for (int i = 0; i < length; i++)
                {
                    this.ShipsCoordinate.Add(new ShipPart(position[0] + (position[1] * i)));
                }
            }
        }

        public override Vector TakeAShot()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a Ship position
        /// </summary>
        /// <param name="length"> int The ship lenght</param>
        /// <returns>Vector[] 0. elemnt is the Ship start point 1. elment is the direction of the ship</returns>
        /// <exception cref="NotImplementedException"></exception>
        private Vector[] GetNewShipPosition(int length)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// decides whether the position for the given Ship length is correct
        /// </summary>
        /// <param name="position">Vector[] 0. elemnt is the Ship start point 1. elment is the direction of the ship</param>
        /// <param name="length">The length of the Ship</param>
        /// <returns>true if positon is correct and false otherwise</returns>
        private bool IsShipPositionValid(Vector[] position, int length)
        {
            if (MainSettings.CoordinateValidation(position[0]))
            {
                return false;
            }
            Vector endOfTheShip = position[0] + (position[1] * length);

            return MainSettings.CoordinateValidation(endOfTheShip);
        }
    }
}
