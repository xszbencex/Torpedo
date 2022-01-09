using Microsoft.VisualStudio.TestTools.UnitTesting;
using Torpedo.GameElement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torpedo.Settings;

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
        public void TakeAShotTest()
        {
            Assert.Fail();
        }
    }
}