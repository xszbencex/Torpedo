using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torpedo.Model;
using Torpedo.Settings;

namespace Torpedo.GameElement
{
    public class AIPlayer : Player
    {
        // TODO AIPlayer implementáció (csak átmásoltam a RealPlayert)
        public AIPlayer(string name) : base(name)
        {
        }

        public void PutDownAllShip()
        {

        }

        public override Vector TakeAShot()
        {
            throw new NotImplementedException();
        }
    }
}
