using Microsoft.VisualStudio.TestTools.UnitTesting;
using Torpedo.GameElement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torpedo.Model;

namespace Torpedo.GameElement.Tests
{
    [TestClass()]
    public class GameSessionTests
    {
        [TestMethod()]
        public void ActualPlayerTakeAShotTest_TakeAHit_HitVillRegistredInPlayerShips()
        {
            // Arrange
            Player player1 = new RealPlayer("Ubul");
            Player player2 = new RealPlayer("Hans");

            GameSession session = new GameSession(player1, player2);

            Vector shipStartPoint = new Vector(1, 1);
            Vector shipEndPoint = new Vector(1, 4);

            Vector shot = new Vector(1, 3);

            session.ActualPlayer = session.Player1;
            session.ActualPlayerPutsDownShip(shipStartPoint, shipEndPoint);
            session.ActualPlayer = session.Player2;

            // Act
            session.ActualPlayerTakeAShot(shot);

            // Assert
            Assert.IsTrue(session.Player1.Ships[0].Parts.Find(s => s.Coordinate == shot).Destroyed);
            Assert.IsFalse(session.Player1.Ships[0].IsDestroyed);
        }

        [TestMethod()]
        public void ActualPlayerTakeAShotTest_ONshipDystroyd_ShipWillBeMarkdAsDestoyed()
        {
            // Arrange
            Player player1 = new RealPlayer("Ubul");
            Player player2 = new RealPlayer("Hans");

            GameSession session = new GameSession(player1, player2);

            Vector shipStartPoint = new Vector(1, 1);
            Vector shipEndPoint = new Vector(1, 4);

            List<Vector> shots = new List<Vector>();
            shots.Add(shipEndPoint);
            shots.Add(shipStartPoint);
            shots.Add(new Vector(1, 2));
            shots.Add(new Vector(1, 3));

            session.ActualPlayer = session.Player1;
            session.ActualPlayerPutsDownShip(shipStartPoint, shipEndPoint);

            session.Player1.ShipsCoordinate.Add(new ShipPart ( new Vector(0, 0)));

            session.ActualPlayer = session.Player2;

            // Act
            shots.ForEach(shot =>
            {
                session.ActualPlayer = session.Player2;
                session.ActualPlayerTakeAShot(shot);
            });

            // Assert
            Assert.IsTrue(session.Player1.Ships[0].IsDestroyed);
        }
    }
}