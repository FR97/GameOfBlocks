using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    
    public class World : MonoBehaviour
    {

        public byte NumberOfChunks;

        public int Seed = 0;

        private GameObject _player;

        private Dictionary<Position,Chunk> _chunks;

        public byte ChunkViewDistance;

        public Chunk ChunkProtype;

        private Position _lastPlayerPosition;

        private Position _lastCentralChunkPosition;
        // Use this for initialization
        void Start ()
        { 
           // Spiral(5,5);
            if (Seed == 0)
                Seed = Random.Range(100000, 999999);

            _player = GameObject.Find("FPSController");
            //_player.transform.position = new Vector3(Random.Range(-16000, 16000), 129 ,Random.Range(-16000, 16000));           
            _lastPlayerPosition = new Position((int)_player.transform.position.x, (int)_player.transform.position.y, (int)_player.transform.position.z);
            _chunks = new Dictionary<Position, Chunk>();

            _lastPlayerPosition.X = (int)_player.transform.position.x;
            _lastPlayerPosition.Y = (int)_player.transform.position.y;
            _lastPlayerPosition.Z = (int)_player.transform.position.z;

            int centralChunkPosX = (int)_lastPlayerPosition.X;
            int centralChunkPosZ = (int)_lastPlayerPosition.Z;
            while (centralChunkPosX % 16 != 0)
            {
                if (centralChunkPosX > 0)
                    centralChunkPosX--;
                else if (centralChunkPosX < 0)
                    centralChunkPosX++;
            }
            while (centralChunkPosZ % 16 != 0)
            {
                if (centralChunkPosZ > 0)
                    centralChunkPosZ--;
                else if (centralChunkPosZ < 0)
                    centralChunkPosZ++;
            }
            if (ShouldGenerateChunks(centralChunkPosX, centralChunkPosZ))
                GenerateChunks(centralChunkPosX, centralChunkPosZ);

        }
	    
        // Update is called once per frame
        void Update () {
            if (_lastPlayerPosition.X != (int)_player.transform.position.x ||
                
                _lastPlayerPosition.Z != (int)_player.transform.position.z)
            {
                _lastPlayerPosition.X = (int)_player.transform.position.x;
                _lastPlayerPosition.Y = (int)_player.transform.position.y;
                _lastPlayerPosition.Z = (int)_player.transform.position.z;

                int centralChunkPosX = (int)_lastPlayerPosition.X;
                int centralChunkPosZ = (int)_lastPlayerPosition.Z;
                while (centralChunkPosX % 16 != 0)
                {
                    if (centralChunkPosX > 0)
                        centralChunkPosX--;
                    else if (centralChunkPosX < 0)
                        centralChunkPosX++;
                }
                while (centralChunkPosZ % 16 != 0)
                {
                    if (centralChunkPosZ > 0)
                        centralChunkPosZ--;
                    else if (centralChunkPosZ < 0)
                        centralChunkPosZ++;
                }
                if(ShouldGenerateChunks(centralChunkPosX, centralChunkPosZ))
                    GenerateChunks(centralChunkPosX, centralChunkPosZ);
            }
        }



        bool ShouldGenerateChunks(int centralChunkPosX, int centralChunkPosZ)
        {
            Debug.Log("CCP: " + centralChunkPosX + "  " + centralChunkPosZ);
            
            if (_lastCentralChunkPosition == null)
            {
                _lastCentralChunkPosition = new Position(centralChunkPosX, 0, centralChunkPosX);
                Debug.Log("LCCP: " + _lastCentralChunkPosition.X + "  " + _lastCentralChunkPosition.Z);
                return true;
            }
            if (_lastCentralChunkPosition.X != centralChunkPosX || _lastCentralChunkPosition.Z != centralChunkPosZ)
            {
                _lastCentralChunkPosition.X = centralChunkPosX;
                _lastCentralChunkPosition.Z = centralChunkPosZ;
                Debug.Log("LCCP: " + _lastCentralChunkPosition.X + "  " + _lastCentralChunkPosition.Z);
                return true;
            }
            return false;
        }
        void Spiral(int X, int Y)
        {
            int x, y, dx, dy;
            x = y = dx = 0;
            dy = -1;
            int t = Mathf.Max(X, Y);
            int maxI = t * t;
            for (int i = 0; i < maxI; i++)
            {
                if ((-X / 2 <= x) && (x <= X / 2) && (-Y / 2 <= y) && (y <= Y / 2))
                {
                   
                }
                if ((x == y) || ((x < 0) && (x == -y)) || ((x > 0) && (x == 1 - y)))
                {
                    t = dx;
                    dx = -dy;
                    dy = t;
                }
                x += dx;
                y += dy;
            }
        }
        void GenerateChunks(int centralChunkPosX, int centralChunkPosZ)
        {
            int x, y, dx, dy;
            x = y = dx = 0;
            dy = -1;
            int totalChunkViewDistance = ChunkViewDistance * 2 + 1;
            int t = totalChunkViewDistance;
            Debug.Log("T " + t);
            int maxI = t * t;
            for (int i = 0; i < maxI; i++)
            {
                if ((-totalChunkViewDistance / 2 <= x) && (x <= totalChunkViewDistance / 2) && (-totalChunkViewDistance / 2 <= y) && (y <= totalChunkViewDistance / 2))
                {
                    _chunks.Add(new Position(x, 0, y), Instantiate(ChunkProtype, new Vector3(centralChunkPosX + 16*x, 0, centralChunkPosZ + 16*y), Quaternion.identity, this.transform));
                }
                if ((x == y) || ((x < 0) && (x == -y)) || ((x > 0) && (x == 1 - y)))
                {
                    t = dx;
                    dx = -dy;
                    dy = t;
                }
                x += dx;
                y += dy;
            }

            Debug.Log(_chunks.ToArray());


            
            
            
        }
    }
}
