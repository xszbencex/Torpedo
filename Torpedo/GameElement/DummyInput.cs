using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torpedo.Model;

namespace Torpedo.GameElement
{
    public class DummyInput : IInput
    {
        public DummyInput(List<Vector> coordinate, List<Vector> direction, int counter)
        {
            this.Coordinate = coordinate;
            this.Direction = direction;
            this.Counter = counter;
        }

        public List<Vector> Coordinate { get; set; }
        public List<Vector> Direction { get; set; }
        public int Counter { get; set; }
        public Vector[] GetNewShipPosition(int length)
        {
            Vector[] position = new Vector[2];
            position[0] = Coordinate[Counter];
            position[1] = Direction[Counter];
            Counter = Counter + 1;

            return position;
        }

        public Vector[] GetNewShot()
        {
            throw new NotImplementedException();
        }
    }
}
