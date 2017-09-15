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
           
            if (Seed == 0)
                Seed = Random.Range(100000, 999999);

            _player = GameObject.Find("FPSController");
            _player.transform.position = new Vector3(Random.Range(-16000, 16000), 129 ,Random.Range(-16000, 16000));           
            _lastPlayerPosition = new Position((int)_player.transform.position.x, (int)_player.transform.position.y, (int)_player.transform.position.z);
            _chunks = new Dictionary<Position, Chunk>();

            _lastPlayerPosition.X = (int)_player.transform.position.x;
            _lastPlayerPosition.Y = (int)_player.transform.position.y;
            _lastPlayerPosition.Z = (int)_player.transform.position.z;

            _lastCentralChunkPosition = new Position(-1, -1, -1);

            int centralChunkPosX = CalculateChunkStartPosition((int)_lastPlayerPosition.X);
            int centralChunkPosZ = CalculateChunkStartPosition((int)_lastPlayerPosition.Z);
 
            if (ShouldGenerateChunks(centralChunkPosX, centralChunkPosZ))
                GenerateChunks(centralChunkPosX, centralChunkPosZ);

        }

        int CalculateChunkStartPosition(int playerPositionOnAxis)
        {
            while (playerPositionOnAxis % 16 != 0)
            {
                if (playerPositionOnAxis > 0)
                    playerPositionOnAxis--;
                else if (playerPositionOnAxis < 0)
                    playerPositionOnAxis++;
            }
            return playerPositionOnAxis;
        }

        // Update is called once per frame
        void Update () {
            if (_lastPlayerPosition.X != (int)_player.transform.position.x ||
                
                _lastPlayerPosition.Z != (int)_player.transform.position.z)
            {
                _lastPlayerPosition.X = (int)_player.transform.position.x;
                _lastPlayerPosition.Y = (int)_player.transform.position.y;
                _lastPlayerPosition.Z = (int)_player.transform.position.z;

                int centralChunkPosX = CalculateChunkStartPosition((int)_lastPlayerPosition.X);
                int centralChunkPosZ = CalculateChunkStartPosition((int)_lastPlayerPosition.Z);

                if (ShouldGenerateChunks(centralChunkPosX, centralChunkPosZ))
                {
                    GenerateChunks(centralChunkPosX, centralChunkPosZ);
                   
                }
                   
            }
        }



        bool ShouldGenerateChunks(int centralChunkPosX, int centralChunkPosZ)
        {
                
            if (_lastCentralChunkPosition.X != centralChunkPosX || _lastCentralChunkPosition.Z != centralChunkPosZ)
            {
                _lastCentralChunkPosition = new Position(centralChunkPosX, 0, centralChunkPosX);
                return true;
            }

            return false;
        }


        void GenerateChunks(int centralChunkPosX, int centralChunkPosZ)
        {
            int x, z, dx, dz;
            x = z = dx = 0;
            dz = -1;
            int totalChunkViewDistance = ChunkViewDistance * 2 + 1;
            int t = totalChunkViewDistance;
            
            int maxI = t * t;
            for (int i = 0; i < maxI; i++)
            {
                if ((-totalChunkViewDistance / 2 <= x) && (x <= totalChunkViewDistance / 2) && (-totalChunkViewDistance / 2 <= z) && (z <= totalChunkViewDistance / 2))
                {
                    Position pos = new Position(x, 0, z);
                    if (!_chunks.ContainsKey(pos))
                    {
                        _chunks.Add(pos,
                            Instantiate(ChunkProtype,
                                new Vector3(centralChunkPosX + 16 * x, 0, centralChunkPosZ + 16 * z),
                                Quaternion.identity, this.transform));
                        _chunks[pos].name = "Chunk" + _chunks[pos].transform.position;
                        
                    }
                    else
                    {
                      if ((int)_chunks[pos].transform.position.x != centralChunkPosX + 16 * x ||
                            (int)_chunks[pos].transform.position.z != centralChunkPosZ + 16 * z)
                        {
                            Destroy(_chunks[pos].gameObject, 0.1f);
                           _chunks[pos] = Instantiate(ChunkProtype,
                                new Vector3(centralChunkPosX + 16 * x, 0, centralChunkPosZ + 16 * z),
                                Quaternion.identity, this.transform);
                            _chunks[pos].name = "Chunk" + _chunks[pos].transform.position;
                        }
                    }
                }
                if ((x == z) || ((x < 0) && (x == -z)) || ((x > 0) && (x == 1 - z)))
                {
                    t = dx;
                    dx = -dz;
                    dz = t;
                }
                x += dx;
                z += dz;
            }
                

            }

            


            
            
            
        
    }
}
