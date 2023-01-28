using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class ProceduralGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile wall;

    private float mazePlacementThreshold = .1f;
    private float thinningCoefficent = .2f;

    void Start()
    {
        var maze = GenerateMaze(25,51);
        GenerateTilemap(maze);
    }

    void Update()
    {
        
    }


    void GenerateTilemap(int[,] maze)
    {
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        for (int x = 0; x <= rMax; x++) {
            for (int y = 0; y <= cMax; y++) {
                if(maze[x,y] == 1)
                {
                    Vector3Int position = new Vector3Int(x, y, 0);
                    tilemap.SetTile(position, wall);
                }
            }
        }
    }

    private int[,] GenerateMaze(int cols, int rows)
    {
        int[,] maze = new int[cols, rows];
 
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                // Make outer walls
                if (i == 0 || j == 0 || i == cols - 1 || j == rows - 1)
                {
                    maze[i, j] = 1;
                }

                // Make other walls 
                else if (i % 2 == 0 && j % 2 == 0)
                {
                    if (Random.value > .1f)
                    {
                        maze[i, j] = 1;

                        int a = Random.value < .5 ? 0 : (Random.value < .5f ? -1 : 1);
                        int b = a != 0 ? 0 : (Random.value < .5f ? -1 : 1);
                        maze[i+a, j+b] = 1;
                    }
                }
            }
        }

        RemoveDensity(ref maze);
        return maze;
    }

    private void RemoveDensity(ref int[,] maze)
    {
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        for (int x = 1; x < rMax; x++) {
            for (int y = 1; y < cMax; y++) {
                if(maze[x,y] == 1 && Random.value < thinningCoefficent)
                {
                    maze[x,y] = 0;
                }
            }
        }
    }


    void GenerateExits() 
    {
    }

    private class RoomExits
    {
        public RoomExits()
        {
            topExit = (Random.value > 0.5f);
            bottomExit = (Random.value > 0.5f);
            rightExit = true; // Always make right exit
            leftExit = false; // Never make left exit
        }

        public readonly bool topExit;
        public readonly bool bottomExit;
        public readonly bool rightExit;
        public readonly bool leftExit;
    }


}
