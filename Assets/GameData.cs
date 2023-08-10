using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int score;
    public int difficulty;
    public int lines;
    public int currentTetrominoRotation;
    public string currentTetrominoName;

    [Serializable]
    public struct vector3S
    {
        public float x, y, z;
        public vector3S(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public vector3S(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }
    }
    public vector3S currentTetrominoPosition;
    public vector3S nextTetrominoPosition;
    public int nextTetrominoRotation;
    public string nextTetrominoName;

    public List<BlockData> blockDataList = new List<BlockData>();
}

[Serializable]
public class BlockData
{
    [Serializable]
    public struct vector3S
    {
        public float x, y, z;
        public vector3S(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public vector3S(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }
    }
    public vector3S blockPosition;
    
    public string textureName;
    public int textureRotation;
    public vector3S texturePosition;
    public vector3S textureScale;

}
