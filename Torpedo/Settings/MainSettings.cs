using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Torpedo.Model;

namespace Torpedo.Settings
{
    public static class MainSettings
    {
        public const int GridHeight = 10, GridWidth = 10;
        public static readonly int[] PlayableShipsLength = new int[] { 2, 3 };
        public static readonly Brush DefaultFieldColor = Brushes.Transparent;
        public static readonly Brush DefaultFieldStrokeColor = Brushes.Black;
        public static readonly Brush ShipColor = Brushes.Blue;
        public static readonly Brush HitColor = Brushes.Coral;
        public static readonly Brush MissColor = Brushes.DarkGray;

        public static bool CoordinateValidation(Vector coordinate)
        {
            return (coordinate.X >= 0) && (coordinate.Y >= 0) && (coordinate.X < GridWidth) && (coordinate.Y < GridHeight);
        }
    }
}