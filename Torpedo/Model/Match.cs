using System;
using System.ComponentModel.DataAnnotations;

namespace Torpedo.Model
{
    public class Match
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }
        public int Rounds { get; set; }
        public int Player1Hits { get; set; }
        public int Player2Hits { get; set; }
        public string Winner { get; set; }

        public Match(string player1Name, string player2Name, int rounds, int player1Hits, int player2Hits, string winner)
        {
            this.Player1Name = player1Name;
            this.Player2Name = player2Name;
            this.Rounds = rounds;
            this.Player1Hits = player1Hits;
            this.Player2Hits = player2Hits;
            this.Winner = winner;
        }
    }
}
