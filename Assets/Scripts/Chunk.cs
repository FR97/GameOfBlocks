using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class Chunk : MonoBehaviour
    {

        private MeshFilter _meshFilter;
        
        private MeshCollider _meshCollider;
        private Mesh _mesh;

        private List<Vector3> _vertices;

        private List<int> _triangles;

        public byte Width;
        public byte Height;
        public byte Depth;

        public int ChunkStartPositionX;
        public int ChunkStartPositionZ;
        /// <summary>
        /// Representation of 
        /// </summary>
        private short[,,] _chunkBlocks;

        private GameObject player;

        // Use this for initialization
        void Start () {
            _chunkBlocks = new short[Width,Height,Depth];
            

            _meshFilter = GetComponent<MeshFilter>();
            _meshFilter.mesh = null;
            _mesh = new Mesh();
            _meshCollider = GetComponent<MeshCollider>();
            _meshCollider.sharedMesh = null;
            
            _vertices = new List<Vector3>();
            _triangles = new List<int>();

            player = GameObject.Find("FPSController");
           

            GenerateChunk();
            UpdateMesh();
        }

        void GenerateChunk()
        {
            Random r = new Random();
            
            for (byte y = 0; y < Height; y++)
            {
                for (byte x = 0; x < Width; x++)
                {
                    for (byte z = 0; z < Depth; z++)
                    {

                        short k =(short) r.Next(0,2);
                        // Only solid blocks for now.
                        _chunkBlocks[x, y, z] = k;

                    }
                }
            }
            for (byte y = 0; y < Height; y++)
            {
                for (byte x = 0; x < Width; x++)
                {
                    for (byte z = 0; z < Depth; z++)
                    {
                        if(_chunkBlocks[x, y, z] > 0)
                            PlaceBlock(x, y, z, 1);

                    }
                }
            }
          
        }

        void PlaceBlock(byte x, byte y, byte z, short blockId)
        {
            for (byte i = 0; i < 6; i++)
            {
                byte posX = x;  
                // Transparent blocks must have id <= 0
                short a = GetNeighborForSide(x, y, z, (BlockSide)i);
                if (a <= 0)
                { 
                    MakeBlockSide((BlockSide)i, new Position(x + ChunkStartPositionX, y, z + ChunkStartPositionZ));
                }   
            } 
        }

        public short GetNeighborForSide(byte x, byte y, byte z, BlockSide side)
        {
            Position offsetToCheck = _offsets[(int)side];
            Position neighborPosition = new Position(x + offsetToCheck.X, y + offsetToCheck.Y, z + offsetToCheck.Z);
            if (neighborPosition.X < 0 || neighborPosition.X >= Width || neighborPosition.Z < 0 || neighborPosition.Z >= Depth || neighborPosition.Y < 0 || neighborPosition.Y >= Height)
                return 0;

            return _chunkBlocks[neighborPosition.X, neighborPosition.Y, neighborPosition.Z];

        }

        void MakeBlockSide(BlockSide side, Position position)
        {
            _vertices.AddRange(CubicMeshData.GetVerticesForSide(side, position));
            _triangles.Add(_vertices.Count - 4);
            _triangles.Add(_vertices.Count - 4 + 1);
            _triangles.Add(_vertices.Count - 4 + 2);
            _triangles.Add(_vertices.Count - 4);
            _triangles.Add(_vertices.Count - 4 + 2);
            _triangles.Add(_vertices.Count - 4 + 3);
        }

        private readonly Position[] _offsets =
        {
            // North               South
            new Position(0, 0, 1), new Position(0, 0, -1),

            // East                West
            new Position(1, 0, 0), new Position(-1, 0, 0),

            // Top                 Bottom
            new Position(0, 1, 0), new Position(0, -1, 0)
        };

        void Update()
        {
            //Debug.Log(player.transform.position);
        }

        void UpdateMesh()
        {
            _mesh.Clear();
            _mesh.vertices = _vertices.ToArray();
            _mesh.triangles = _triangles.ToArray();

            _mesh.RecalculateBounds();
            _mesh.RecalculateNormals();
            _meshFilter.mesh = _mesh;
            GetComponent<MeshCollider>().sharedMesh = _mesh;
           

        }


    }

}
