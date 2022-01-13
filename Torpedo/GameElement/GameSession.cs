using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torpedo.Model;
using Torpedo.Repository;
using Torpedo.Settings;
namespace Torpedo.GameElement
{
    public class GameSession
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Player ActualPlayer { get; set; }
        public bool IsPuttingDownPhase { get; set; }
        public Vector? ShipStartPoint { get; set; }
        public bool GameOver { get; set; }
        public int RoundNumber { get; set; }
        public bool RoundSwitch { get; set; }
        public GameSession(Player player1, Player player2)
        {
            this.Player1 = player1;
            this.Player2 = player2;
            this.ActualPlayer = player1;
            this.IsPuttingDownPhase = true;
            this.RoundNumber = 1;
            this.RoundSwitch = false;
        }

        public void ActualPlayerPutsDownShip(Vector shipStartPoint, Vector shipEndPoint)
        {
             ActualPlayer.PutDownAShip(shipStartPoint, shipEndPoint);

            if ((Player1.ShipCount == MainSettings.PlayableShipsLength.Length) && (Player2.ShipCount == MainSettings.PlayableShipsLength.Length))
            {
                IsPuttingDownPhase = false;
                SecondPhaseInIt();
            }

            if (ActualPlayer.ShipCount == MainSettings.PlayableShipsLength.Length)
            {
                ActualPlayer = GetOtherPlayer();
                if (ActualPlayer is AIPlayer aI)
                {
                    AIPlayer ai = (AIPlayer)ActualPlayer;
                    ai.PutDownAllShip();
                    IsPuttingDownPhase = false;
                    SecondPhaseInIt();
                }
            }
        }

        private void SecondPhaseInIt()
        {
            var random = new Random();
            var randomInt = random.Next(2);
            ActualPlayer = randomInt == 0 ? Player1 : Player2;
            if (ActualPlayer is AIPlayer aI)
            {
                AIShot();
            }
        }

        public void ActualPlayerTakeAShot(Vector shotPoint)
        {
            RegisteringAShot(shotPoint);

            ActualPlayer = GetOtherPlayer();
            if (ActualPlayer is AIPlayer aI)
            {
                AIShot();
            }
        }

        private void AIShot()
        {
            RegisteringAShot(ActualPlayer.TakeAShot());
            ActualPlayer = GetOtherPlayer();
        }

        private void RegisteringAShot(Vector shotPoint)
        {
            if (ActualPlayer.FiredShots.Where(f => f.Coordinate == shotPoint).Any())
            {
                throw new ArgumentException("You've already shot here!");
            }
            Player otherPlayer = GetOtherPlayer();
            try
            {
                otherPlayer.ShipsCoordinate.Where(s => s.Coordinate == shotPoint).Single().Destroyed = true;
                otherPlayer.Ships.ForEach(s => s.Update());
                ActualPlayer.FiredShots.Add(new FiredShot(shotPoint, true));
            }
            catch
            {
                ActualPlayer.FiredShots.Add(new FiredShot(shotPoint, false));
            }
            if (IsGameOver())
            {
                Match match = new Match(
                    Player1.Name,
                    Player2.Name,
                    RoundNumber,
                    Player1.FiredShots.FindAll(shot => shot.Hit).Count,
                    Player2.FiredShots.FindAll(shot => shot.Hit).Count,
                    ActualPlayer.Name);
                MatchRepository.AddMatch(match);
                throw new GameOverExeption($"{ActualPlayer.Name} wins!");
            }
            if (RoundSwitch)
            {
                RoundNumber++;
                RoundSwitch = !RoundSwitch;
            }
            else
            {
                RoundSwitch = !RoundSwitch;
            }
        }

        public Player GetOtherPlayer()
        {
            return this.ActualPlayer.Equals(this.Player1) ? this.Player2 : this.Player1;
        }

        private bool IsGameOver()
        {
            return !GetOtherPlayer().ShipsCoordinate.Where(s => s.Destroyed == false).Any();
        }
    }
}
