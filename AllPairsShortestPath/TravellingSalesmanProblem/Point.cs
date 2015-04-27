using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravellingSalesmanProblem
{
    public class Point:IComparable
    {
        public float X { get; set; }
        public float Y { get; set; }

        public static Point ParseLine(string line) {
            string[] coords = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Point point = null;
            float x;
            float y;
            if (float.TryParse(coords[0], out x) && float.TryParse(coords[1], out y)) {
                point = new Point() { X = x, Y = y };
            }
            return point;
        }

        public int CompareTo(object obj)
        {
            Point p = obj as Point;
            if (p == null) return -1;

            if (p == this) return 0;
            else return -1;
        }
    }
}
