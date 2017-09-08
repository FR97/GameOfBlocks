using System.IO;
using NUnit.Framework.Constraints;
using UnityEngine;

namespace Assets.Scripts
{
    public class TextureLoader : MonoBehaviour{

        public void Start()
        {
            LoadTextures();
        }


        public static void LoadTextures()
        {

            Texture2D test = LoadTexture(@".\Assets\Resources\Textures\Blocks\dirt.png");
            Color[] pixels = test.GetPixels();
            Debug.Log(pixels[0]);
        }


        /// <summary>
        /// Creates readable Texture2D from file
        /// </summary>
        /// <param name="FilePath">Path to file</param>
        /// <returns>Texture2D made from file</returns>
        private static Texture2D LoadTexture(string filePath)
        {

            Debug.Log(filePath);
            Texture2D texure2D;
            byte[] fileData;

            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                texure2D = new Texture2D(2, 2);           // Create new "empty" texture
                if (texure2D.LoadImage(fileData, false))           // Load the imagedata into the texture (size is set automatically)
                    return texure2D;                 // If data = readable -> return texture
            }
            return null;                     // Return null if load failed
        }
    }
}
