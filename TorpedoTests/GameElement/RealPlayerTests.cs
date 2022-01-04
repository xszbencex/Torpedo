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
        public void PutDownAllShipTest()
        {
            List<Vector> coordinates = new List<Vector>();
            List<Vector> directions = new List<Vector>();

            for (int i = 0; i < MainSettings.PlayableShipsLength.Length; i++)
            {
                coordinates.Add(new Vector(0, i));
                directions.Add(Vector.Right);
            }

            DummyInput input = new DummyInput(coordinates, directions, 0);
            Player player = new RealPlayer(input ,"test");
            player.PutDownAllShip();

            Console.WriteLine(player.ShipsCoordinate);
            

            Assert.Fail();
        }

        [TestMethod()]
        public void TakeAShotTest()
        {
            throw new NotImplementedException();
            Assert.Fail();
        }
    }
}