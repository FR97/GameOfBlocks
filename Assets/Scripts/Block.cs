
using UnityEngine;

namespace Assets.Scripts
{

    
    public class Block
    {
       


    }

    public enum BlockSide
    {
        North, South, East, West, Top, Bottom
    }

    /// <summary>
    /// Position in 3D space
    /// </summary>
    public struct Position
    {
        public int X;
        public int Y;
        public int Z;

        public Position(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

    }
}
