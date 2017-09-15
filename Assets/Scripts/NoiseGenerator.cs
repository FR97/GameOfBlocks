using UnityEngine;

namespace Assets.Scripts
{
    public class NoiseGenerator{

        public static float[,] GenerateHeights(int xSize, int zSize, int height, float noiseSize)
        {
            float[,] noise = new float[xSize, zSize];



            for (int x = 0; x < xSize; x++)
            {
                for (int z = 0; z < zSize; z++)
                {
                    

                    noise[x,z] = PerlinNoise(x,z, height, noiseSize);
                }
            }
            return noise;
        }

        //Function that inputs the position and spits out a float value based on the perlin noise
        private static float PerlinNoise(float x, float y, int height, float noiseSize)
        {
            //Generate a value from the given position, position is divided to make the noise more frequent.
            float noise = Mathf.PerlinNoise(x / noiseSize, y / noiseSize);

            //Return the noise value
            return noise * height; ;

        }

    }
}
