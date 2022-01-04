﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torpedo.Model;

namespace Torpedo.Settings
{
    public static class MainSettings
    {
        public const int GridHeight = 15, GridWidth = 15;
        public static readonly int[] PlayableShipsLength = new int[] { 2, 2, 4, 5, 6 };

        public static bool CoordinateValidation(Vector coordinate)
        {
            return (coordinate.X >= 0) && (coordinate.Y >= 0) && (coordinate.X < GridWidth) && (coordinate.Y < GridHeight);
        }
    }
}