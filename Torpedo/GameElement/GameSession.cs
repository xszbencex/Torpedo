using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torpedo.Model;
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
        public int Winner { get; set; }
        public GameSession(Player player1, Player player2, Player actualPlayer)
        {
            this.Player1 = player1;
            this.Player2 = player2;
            this.ActualPlayer = actualPlayer;
            this.IsPuttingDownPhase = true;
        }
        /// <summary>
        /// !!!!Nincs tesztelve hogy a kordináták a táblán vannak e és egy vonalban vannak e
        /// </summary>
        /// <param name="shipStartPoint"></param>
        /// <param name="shipEndPoint"></param>
        public void ActualPlayerPutsDownShip(Vector shipStartPoint, Vector shipEndPoint)
        {
            // TODO actual player PutDownShip-jét meghívni,
            // ha lerakta az összeset, akkor váltani az actual playert a másikra
            // ha a másik is lerakta az összeset vagy ha az enemy AI akkor hívni a PutAllDown-t és
            // IsPuttingDownPhase = false-ra állítása

            // Ha az ai nál a PutDownShip-re lerakja az összeset és a shipCout ott max ra állitja (ShipCountott most adtam hozzá)
            // akkor nincs szükség a PutDownAllShipre  Anyi hogy a paraméterek lesznek figyelmen kivűl hagyva azt nem tudom menyire gáz???

             ActualPlayer.PutDownAShip(shipStartPoint, shipEndPoint);

            if ((Player1.ShipCount == MainSettings.PlayableShipsLength.Length) && (Player2.ShipCount == MainSettings.PlayableShipsLength.Length))
            {
                IsPuttingDownPhase = false;
            }

            if (ActualPlayer.ShipCount == MainSettings.PlayableShipsLength.Length)
            {
                ActualPlayer = GetOtherPlayer();
            }
        }

        public void ActualPlayerTakeAShot(Vector shotPoint)
        {
            // TODO shotPoint kezelése, azzal kell hívni az actual player TakeAShot-ját
            // A TakeShot azt Bemenetnek Szántam volna de ha innen meg kapja akkor igazábol lehet hogy felesleges is
            // Ennek a végén lehet meg lehetne már hívni a GameOvert mert egy lövés után lesz vége meg meg lehet álapitani hogy kinyert az aki épen lőtt
            // és ha arra fel lehet iratkozni akkor innen ji lehet váltani a játék végét (??? nem vagyok benne teljesen biztos hogy ez működhet)
            Player otherPlayer = GetOtherPlayer();
            try// Nem vagyok benne biztos hogy működik !!!!!Kézel testelve működni Látszik!!!!!
            {
                otherPlayer.ShipsCoordinate.Where(s => s.Coordinate == shotPoint).Single().Destroyed = true;
                ActualPlayer.FiredShots.Add(new FiredShot(shotPoint, true));
            }
            catch
            {
                ActualPlayer.FiredShots.Add(new FiredShot(shotPoint, false));
            }

            ActualPlayer = GetOtherPlayer();
            // Ai elleni játéknál ha nem nyert az igazi játékos akkor it lőhet az ai és meg
            // vizsgáljuk, hogy az ai nyert e majd vissza álitjuk az actualPlayert nem tünik szép megoldásnak de működhet
        }

        private Player GetOtherPlayer()
        {
            Player otherPlayer;
            if (ActualPlayer.Equals(Player1))
            {
                otherPlayer = Player2;
            }
            else
            {
                otherPlayer = Player1;
            }

            return otherPlayer;
        }

        private bool IsGameOver()
        {
            return ActualPlayer.ShipsCoordinate.Where(s => s.Destroyed == false).Any();
        }
    }
}
