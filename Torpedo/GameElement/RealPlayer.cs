using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torpedo.Model;
using Torpedo.Settings;

namespace Torpedo.GameElement
{
    public class RealPlayer : Player
    {
        public RealPlayer(string name) : base(name)
        {
        }

        public override Vector TakeAShot()
        {
            throw new NotImplementedException();
        }
    }
}
