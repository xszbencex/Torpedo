﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torpedo.Model
{
    public class FiredShot
    {
        public FiredShot(Vector coordinate, bool hit)
        {
            this.Coordinate = coordinate;
            this.Hit = hit;
        }

        public Vector Coordinate { get; set; }
        public bool Hit { get; set; } = false;
    }
}
