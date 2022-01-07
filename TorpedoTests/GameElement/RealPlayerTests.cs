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
        public void RealPlayerTest()
        {
            throw new NotImplementedException();
            Assert.Fail();
        }

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
            Player player = new RealPlayer(input ,"test");
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
            Player player = new RealPlayer(input, "test");
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
        public void TakeAShotTest()
        {
            throw new NotImplementedException();
            Assert.Fail();
        }
    }
}