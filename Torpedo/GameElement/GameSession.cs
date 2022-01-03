using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torpedo.Model;

namespace Torpedo.GameElement
{
    public class GameSession
    {
        public Player? Player1 { get; set; }
        public Player? Player2 { get; set; }
        public Player? ActualPlayer { get; set; }
        public bool GameOver { get; set; }
        public int Winner { get; set; }
        public Vector LastShot { get; set; }

        public void PuttDownTheShips()
        {
            Player1.PutDownAllShip();
            Player2.PutDownAllShip();
        }

        public void ActualPlayerTakeAShot()
        {
            LastShot = ActualPlayer.TakeAShot();
            Player otherPlayer;
            if (ActualPlayer.Equals(Player1))
            {
                otherPlayer = Player2;
            }
            else
            {
                otherPlayer = Player1;
            }
            try//Nem vagyok benne biztos hogy működik
            {
                otherPlayer.ShipsCoordinate.Where(s => s.Coordinate == LastShot).Single().Destroyed = true;
                ActualPlayer.FiredShots.Add(new FiredShot(LastShot, true));
            }
            catch
            {
                ActualPlayer.FiredShots.Add(new FiredShot(LastShot, false));
            }
        }

        private bool IsGameOver()
        {
            return ActualPlayer.ShipsCoordinate.Where(s => s.Destroyed == false).Any();
        }
    }
}
