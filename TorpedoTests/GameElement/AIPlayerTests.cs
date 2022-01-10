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
            AIPlayer actual = new AIPlayer();

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
            AIPlayer ai = new AIPlayer();

            // Act
            Vector actual = ai.TakeAShot();

            // Assert
            Assert.IsTrue(MainSettings.CoordinateValidation(actual));
        }

        [TestMethod()]
        public void TakeAShot_TakeAllShots_DoNotShotAPlaceTwice()
        {
            // Arrange
            AIPlayer actual = new AIPlayer();

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
            AIPlayer ai = new AIPlayer();
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

        [TestMethod()]
        public void TakeAShot_ifTwoHitsNextToEachOther_TakeAshotToTheEndOfTheLine()
        {
            // Arrange
            AIPlayer ai = new AIPlayer();
            Vector hitVector = new Vector(4, 4);
            Vector secondHitVector = new Vector(4, 3);
            FiredShot hit = new FiredShot(hitVector, true);
            FiredShot secondHit = new FiredShot(secondHitVector, true);
            ai.FiredShots.Add(hit);
            ai.FiredShots.Add(secondHit);
            List<Vector> expected = new List<Vector>();
            expected.Add(new Vector(4, 5));
            expected.Add(new Vector(4, 2));

            // Act
            var actual = ai.TakeAShot();

            // Assert
            Assert.IsTrue(expected.Contains(actual));
        }

        [TestMethod()]
        public void TakeAShot_TherIsADistroydShip_MoveOnToLookingAnotherShip()
        {
            // Arrange
            AIPlayer ai = new AIPlayer();
            Vector hitVector = new Vector(4, 4);
            Vector secondHitVector = new Vector(4, 3);
            Vector shotVector = new Vector(4, 5);
            Vector secondShotVector = new Vector(4, 2);

            FiredShot hit = new FiredShot(hitVector, true);
            FiredShot secondHit = new FiredShot(secondHitVector, true);

            FiredShot shot = new FiredShot(shotVector, false);
            FiredShot secondShot = new FiredShot(secondShotVector, false);
            ai.FiredShots.Add(hit);
            ai.FiredShots.Add(secondHit);
            ai.FiredShots.Add(shot);
            ai.FiredShots.Add(secondShot);
            List<Vector> notExpected = new List<Vector>();
            notExpected.Add(hitVector + Vector.Right);
            notExpected.Add(hitVector + Vector.Left);
            notExpected.Add(secondHitVector + Vector.Right);
            notExpected.Add(secondHitVector + Vector.Left);
            notExpected.Add(shotVector + Vector.Right);
            notExpected.Add(shotVector + Vector.Left);
            notExpected.Add(secondShotVector + Vector.Right);
            notExpected.Add(secondShotVector + Vector.Left);

            // Act
            var actual = ai.TakeAShot();

            // Assert
            Assert.IsFalse(notExpected.Contains(actual));
        }
    }
}
