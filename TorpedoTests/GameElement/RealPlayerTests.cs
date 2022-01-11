using Microsoft.VisualStudio.TestTools.UnitTesting;
using Torpedo.GameElement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torpedo.Model;
using Torpedo.Settings;

namespace Torpedo.GameElement.Tests
{
    [TestClass()]
    public class RealPlayerTests
    {

        [TestMethod()]
        public void GetShipParts_Down()
        {
            // Arrange
            Vector shipStartPoint = new Vector(1, 2);
            Vector shipEndPoint = new Vector(1, 5);
            List<ShipPart> expected = new List<ShipPart>();
            expected.Add(new ShipPart(new Vector(1, 5)));
            expected.Add(new ShipPart(new Vector(1, 2)));
            expected.Add(new ShipPart(new Vector(1, 3)));
            expected.Add(new ShipPart(new Vector(1, 4)));
            RealPlayer player = new RealPlayer("test");

            // Act
            var actual = player.GetShipParts(shipStartPoint, shipEndPoint);

            // Assert
            Assert.IsTrue(expected.All(actual.Contains) && (expected.Count == actual.Count));
        }

        [TestMethod()]
        public void PutDownAShip_PutFirstShipDown_TheShipPartsWillBeInTheShipsCoordinate()
        {
            // Arrange
            Vector shipStartPoint = new Vector(1, 2);
            Vector shipEndPoint = new Vector(1, 5);
            List<ShipPart> expected = new List<ShipPart>();
            expected.Add(new ShipPart(new Vector(1, 5)));
            expected.Add(new ShipPart(new Vector(1, 2)));
            expected.Add(new ShipPart(new Vector(1, 3)));
            expected.Add(new ShipPart(new Vector(1, 4)));
            RealPlayer player = new RealPlayer("test");

            // Act
            player.PutDownAShip(shipStartPoint, shipEndPoint);

            // Assert

            Assert.IsTrue(expected.All(player.ShipsCoordinate.Contains) && (expected.Count == player.ShipsCoordinate.Count));
            Assert.AreEqual(player.ShipCount, 1);
        }

        [TestMethod()]
        public void PutDownAShip_PutFirstShipDown_TheShipPartsWillBeInTheShip()
        {
            // Arrange
            Vector shipStartPoint = new Vector(1, 2);
            Vector shipEndPoint = new Vector(1, 5);
            List<ShipPart> expected = new List<ShipPart>();
            expected.Add(new ShipPart(new Vector(1, 5)));
            expected.Add(new ShipPart(new Vector(1, 2)));
            expected.Add(new ShipPart(new Vector(1, 3)));
            expected.Add(new ShipPart(new Vector(1, 4)));
            RealPlayer player = new RealPlayer("test");

            // Act
            player.PutDownAShip(shipStartPoint, shipEndPoint);

            // Assert

            Assert.IsTrue(expected.All(player.ShipsCoordinate.Contains) && (expected.Count == player.ShipsCoordinate.Count));
            Assert.IsTrue(expected.All(player.Ships[0].Parts.Contains) && (expected.Count == player.Ships[0].Parts.Count));
            Assert.AreEqual(player.ShipCount, 1);
        }

        [TestMethod()]
        public void PutDownAShip_PuttwoShipDown_TheShipsPartsWillBeInTheShipsCoordinate()
        {
            // Arrange
            Vector shipStartPoint = new Vector(1, 2);
            Vector shipEndPoint = new Vector(1, 5);
            Vector shipStartPoint2 = new Vector(3, 2);
            Vector shipEndPoint2 = new Vector(3, 5);
            List<ShipPart> expected = new List<ShipPart>();
            expected.Add(new ShipPart(new Vector(1, 5)));
            expected.Add(new ShipPart(new Vector(1, 2)));
            expected.Add(new ShipPart(new Vector(1, 3)));
            expected.Add(new ShipPart(new Vector(1, 4)));
            expected.Add(new ShipPart(new Vector(3, 5)));
            expected.Add(new ShipPart(new Vector(3, 2)));
            expected.Add(new ShipPart(new Vector(3, 3)));
            expected.Add(new ShipPart(new Vector(3, 4)));
            RealPlayer player = new RealPlayer("test");

            // Act
            player.PutDownAShip(shipStartPoint, shipEndPoint);
            player.PutDownAShip(shipStartPoint2, shipEndPoint2);

            // Assert
            Assert.IsTrue(expected.All(player.ShipsCoordinate.Contains) && (expected.Count == player.ShipsCoordinate.Count));
            Assert.AreEqual(player.ShipCount, 2);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void PutDownAShip_PuttwoShipTooCloseDown_ThrowExeption()
        {
            // Arrange
            Vector shipStartPoint = new Vector(1, 2);
            Vector shipEndPoint = new Vector(1, 5);
            Vector shipStartPoint2 = new Vector(2, 2);
            Vector shipEndPoint2 = new Vector(2, 5);
            List<ShipPart> expected = new List<ShipPart>();
            expected.Add(new ShipPart(new Vector(1, 5)));
            expected.Add(new ShipPart(new Vector(1, 2)));
            expected.Add(new ShipPart(new Vector(1, 3)));
            expected.Add(new ShipPart(new Vector(1, 4)));
            expected.Add(new ShipPart(new Vector(2, 5)));
            expected.Add(new ShipPart(new Vector(2, 2)));
            expected.Add(new ShipPart(new Vector(2, 3)));
            expected.Add(new ShipPart(new Vector(2, 4)));
            RealPlayer player = new RealPlayer("test");

            // Act
            player.PutDownAShip(shipStartPoint, shipEndPoint);
            player.PutDownAShip(shipStartPoint2, shipEndPoint2);

            // Assert
            Assert.IsTrue(expected.All(player.ShipsCoordinate.Contains) && (expected.Count == player.ShipsCoordinate.Count));
            Assert.AreEqual(player.ShipCount, 2);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void PutDownAShip_PutShipDownWhatHasSameCoordinateWhitAnotherShip_ThrowExeption()
        {
            // Arrange
            Vector shipStartPoint = new Vector(1, 2);
            Vector shipEndPoint = new Vector(1, 5);
            List<ShipPart> expected = new List<ShipPart>();
            expected.Add(new ShipPart(new Vector(1, 5)));
            expected.Add(new ShipPart(new Vector(1, 2)));
            expected.Add(new ShipPart(new Vector(1, 3)));
            expected.Add(new ShipPart(new Vector(1, 4)));
            RealPlayer player = new RealPlayer("test");
            player.PutDownAShip(shipStartPoint, shipEndPoint);

            // Act
            player.PutDownAShip(new Vector(1, 3), new Vector(5, 3));

            // Assert
            Assert.Fail();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void PutDownAShip_PutShipDownwhitNotInLineCordinates_ThrowExeption()
        {
            // Arrange
            Vector shipStartPoint = new Vector(3, 2);
            Vector shipEndPoint = new Vector(1, 5);
            List<ShipPart> expected = new List<ShipPart>();
            RealPlayer player = new RealPlayer("test");

            // Act
            player.PutDownAShip(shipStartPoint, shipEndPoint);

            // Assert
            Assert.Fail();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void PutDownAShip_TryNotCorrectSizeShip_ThrowExeption()
        {
            // Arrange
            Vector shipStartPoint = new Vector(1, 1);
            Vector shipEndPoint = new Vector(1, 5);
            List<ShipPart> expected = new List<ShipPart>();
            RealPlayer player = new RealPlayer("test");

            // Act
            player.PutDownAShip(shipStartPoint, shipEndPoint);

            // Assert
            Assert.Fail();
        }

        [TestMethod()]
        public void GetShipParts_Up()
        {
            // Arrange
            Vector shipStartPoint = new Vector(1, 5);
            Vector shipEndPoint = new Vector(1, 2);
            List<ShipPart> expected = new List<ShipPart>();
            expected.Add(new ShipPart(new Vector(1, 5)));
            expected.Add(new ShipPart(new Vector(1, 2)));
            expected.Add(new ShipPart(new Vector(1, 3)));
            expected.Add(new ShipPart(new Vector(1, 4)));
            RealPlayer player = new RealPlayer("test");

            // Act
            var actual = player.GetShipParts(shipStartPoint, shipEndPoint);

            // Assert
            Assert.IsTrue(expected.All(actual.Contains) && (expected.Count == actual.Count));

        }

        /*
        [TestMethod()]
        public void PutDownAllShip_whitValidPositions_PutDownAllShip()
        {
            // Arrange
            List<Vector> coordinates = new List<Vector>();
            List<Vector> directions = new List<Vector>();


            for (int i = 0; i < MainSettings.PlayableShipsLength.Length; i++)
            {
                coordinates.Add(new Vector(0, i));
                directions.Add(Vector.Right);
            }

            DummyInput input = new DummyInput(coordinates, directions, 0);
            Player player = new RealPlayer("test");
 
            List<ShipPart> expected = new List<ShipPart>();
            for (int i = 0; i < MainSettings.PlayableShipsLength.Length; i++)
            {
                for (int j = 0; j < MainSettings.PlayableShipsLength[i]; j++)
                {
                    expected.Add(new ShipPart(new Vector(j, i)));
                }
            }

            // Act
            player.PutDownAllShip();

            // Assert
            Assert.IsTrue(expected.All(player.ShipsCoordinate.Contains) && (expected.Count == player.ShipsCoordinate.Count));
        }

        [TestMethod()]
        public void PutDownAllShip_whitWrongPositions_PutDownAllShip()
        {
            // Arrange
            List<Vector> coordinates = new List<Vector>();
            List<Vector> directions = new List<Vector>();

            for (int i = 0; i < MainSettings.PlayableShipsLength.Length; i++)
            {
                coordinates.Add(new Vector(0, i));
                directions.Add(Vector.Right);
            }

            coordinates.Insert(0, new Vector(-1, 0));
            directions.Insert(0, Vector.Right);

            coordinates.Insert(0, new Vector(0, MainSettings.GridHeight + 1));
            directions.Insert(0, Vector.Up);

            coordinates.Insert(MainSettings.PlayableShipsLength.Length - 2, new Vector(0, 0));
            directions.Insert(0, Vector.Down);

            DummyInput input = new DummyInput(coordinates, directions, 0);
            Player player = new RealPlayer("test");
            List<ShipPart> expected = new List<ShipPart>();
            for (int i = 0; i < MainSettings.PlayableShipsLength.Length; i++)
            {
                for (int j = 0; j < MainSettings.PlayableShipsLength[i]; j++)
                {
                    expected.Add(new ShipPart(new Vector(j, i)));
                }
            }

            // Act
            player.PutDownAllShip();

            // Assert
            Assert.IsTrue(expected.All(player.ShipsCoordinate.Contains) && (expected.Count == player.ShipsCoordinate.Count));
        }*/

        
    }
}