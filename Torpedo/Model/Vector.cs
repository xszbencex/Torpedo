using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torpedo.Model
{

    public struct Vector
    {
        public static readonly Vector Up = new Vector(0, -1);
        public static readonly Vector Down = new Vector(0, 1);
        public static readonly Vector Right = new Vector(1, 0);
        public static readonly Vector Left = new Vector(-1, 0);

        public int Y { get; set; }

        public int X { get; set; }
        public Vector(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static bool operator ==(Vector a, Vector b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vector a, Vector b)
        {
            return !(a == b);
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }

        public static Vector operator *(Vector a, int b)
        {
            return new Vector(a.X * b, a.Y * b);
        }

        public override bool Equals(object? obj)
        {
            return obj is Vector vector &&
                   this.Y == vector.Y &&
                   this.X == vector.X;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Y, this.X);
        }
    }

}