using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torpedo.Model;

namespace Torpedo.Repository
{
    internal class MatchRepository
    {
        public static IList<Match> GetMatches()
        {
            using (var database = new MatchContext())
            {
                var matches = database.Matches.ToList();

                return matches;
            }
        }

        public static Match GetMatch(long id)
        {
            using (var database = new MatchContext())
            {
                var match = database.Matches.Where(m => m.Id == id).FirstOrDefault();

                return match;
            }
        }

        public static void AddMatch(Match match)
        {
            using (var database = new MatchContext())
            {
                database.Matches.Add(match);

                database.SaveChanges();
            }
        }
    }
}
