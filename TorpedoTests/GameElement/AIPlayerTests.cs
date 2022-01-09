using Microsoft.VisualStudio.TestTools.UnitTesting;
using Torpedo.GameElement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torpedo.Settings;
using Torpedo.Model;

namespace Torpedo.GameElement.Tests
{
    [TestClass()]
    public class AIPlayerTests
    {

        [TestMethod()]
        public void PutDownAllShip__TheShipWillBePuttedDown()
        {
            // Arrange
            AIPlayer actual = new AIPlayer("Ubul");

            // Act
            actual.PutDownAllShip();

            // Assert
            Assert.AreEqual(MainSettings.PlayableShipsLength.Sum(), actual.ShipsCoordinate.Distinct().Count(), actual.ShipsCoordinate.Count);
            Assert.IsTrue(actual.ShipsCoordinate.ConvertAll(s => s.Coordinate).All(MainSettings.CoordinateValidation));
        }

        [TestMethod()]
        public void TakeAShot_FirstShot_ShotWillBeOnTheGrid()
        {
            // Arrange
            AIPlayer ai = new AIPlayer("Ubul");

            // Act
            Vector actual = ai.TakeAShot();

            // Assert
            Assert.IsTrue(MainSettings.CoordinateValidation(actual));
        }

        [TestMethod()]
        public void TakeAShot_TakeAllShots_DoNotShotAPlaceTwice()
        {
            // Arrange
            AIPlayer actual = new AIPlayer("Ubul");

            // Act
            for (int i = 0; i < MainSettings.GridWidth * MainSettings.GridHeight; i++)
            {
                actual.FiredShots.Add(new FiredShot(actual.TakeAShot(), false));
            }

            // Assert
            Assert.AreEqual(actual.FiredShots.Distinct().Count(), actual.FiredShots.Count, MainSettings.GridWidth * MainSettings.GridHeight);
        }

        [TestMethod()]
        public void TakeAShot_ifHasOnlyOneHit_DoTakeAShothNextToIt()
        {
            // Arrange
            AIPlayer ai = new AIPlayer("Ubul");
            Vector hitVector = new Vector(4, 4);
            FiredShot hit = new FiredShot(hitVector, true);
            ai.FiredShots.Add(hit);
            List<Vector> direction = new List<Vector>();
            direction.Add(Vector.Left);
            direction.Add(Vector.Right);
            direction.Add(Vector.Up);
            direction.Add(Vector.Down);
            // Act
            var actual = ai.TakeAShot();

            // Assert
            Assert.IsTrue(direction.Contains(actual - hitVector));

        }
    }
}