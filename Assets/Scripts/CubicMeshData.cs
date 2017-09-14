using UnityEngine;

namespace Assets.Scripts
{
    public class CubicMeshData
    {

        /*
         * 
         *           THIS IS HOW I IMAGINE CUBE FOR DRAWING
         * 
         *        (-1,1,1)____________________(1,1,1)
         *              /|                   /|
         *             / |                  / |
         *            /  |     TOP         /  |
         *           /   |       (1,1,-1) /   |
         * (-1,1,-1)/____|______________ /    |
         *         |     |      NORTH   |     |
         *         |     |              |     | EAST
         *         |     |(-1,-1,1)     |     |
         *   WEST  |     |______________|_____| (1,-1,1)             
         *         |    /   SOUTH       |    /
         *         |   /                |   /
         *         |  /       BOTTOM    |  /
         *         | /                  | /
         *         |/___________________|/
         * (-1,-1,-1)                  (1,-1,-1)
         *                
         * 
         */

        /// <summary>
        /// All vertices for 1 cube
        /// Cube has 6 sides, each side is quad which is formed with 2 triangles,
        /// and 8 points(vertices)
        /// 
        /// Important: 0.5 value is used in other to end up with cube with 1x1x1 dimension
        /// since distance between -0.5 and 0.5 is 1
        /// </summary>
        private static readonly Vector3[] Vertices = new Vector3[]
        {   
            // VERTICES FOR NORTH SIDE
            new Vector3( 0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f,-0.5f, 0.5f),
            new Vector3( 0.5f,-0.5f, 0.5f),

            // VERTICES FOR SOUTH SIDE
            new Vector3(-0.5f, 0.5f, -0.5f),
            new Vector3( 0.5f, 0.5f, -0.5f),
            new Vector3( 0.5f,-0.5f, -0.5f),
            new Vector3(-0.5f,-0.5f, -0.5f)
        };

        /// <summary>
        /// Should I name this Qauds?
        /// 
        /// Groups indexes of vertices based on side
        /// Each group has 4 idexes which are needed to form quad for that side
        /// 
        /// This is order that I prefer to use because it is easy to connect indexes to sides
        /// 
        /// </summary>
        private static readonly int[][] Triangles =
        {
            // NORTH
            new int[]{0,1,2,3},

            // SOUTH
            new int[]{4,5,6,7},
        
            // EAST
            new int[]{5,0,3,6},

            // WEST
            new int[]{1,4,7,2},

            // TOP
            new int[]{5,4,1,0},

            // BOTTOM
            new int[]{3,2,7,6}
        };



        /// <summary>
        /// Provides needed vertices for requested block side at given position
        /// </summary>
        /// <param name="side">What cube side you need?</param>
        /// <param name="position">Where to place cube?</param>
        /// <returns>Vertices needed to draw that quad</returns>
        public static Vector3[] GetVerticesForSide(BlockSide side, Position position)
        {
            Vector3[] sideVertices = new Vector3[4];
            for (int i = 0; i < sideVertices.Length; i++)
            {
                
                sideVertices[i] = Vertices[Triangles[(int)side][i]] + new Vector3(position.X, position.Y, position.Z);
            }
            return sideVertices;
        }
    }
}

