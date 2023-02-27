using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class BoardManager : MonoBehaviour
{
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count (int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    private float enemyCountScaling = 5;

    public int columns = 8;
    public int rows = 8;
    public int colMin = 10;
    public int colMax = 14;
    public int rowMin = 6;
    public int rowMax = 10;
    //TODO: make wallCount and itemCount scale with level number (like EnemyCount)?
    public Count wallCount = new Count (7, 11);
    public Count itemCount = new Count (1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] itemTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    // need to initialize this to null for initial check
    private Transform boardHolder = null;
    private List<Vector3> gridPositions = new List<Vector3>();

    void SetRandomDimensions()
    {
        columns = Random.Range(colMin, colMax);
        rows = Random.Range(rowMin, rowMax);
        int area = columns * rows;
        wallCount = new Count(area/8, area/5);
    }


    // TODO: Edit generation to make more interesting levels?
    void InitializeList()
    {
        gridPositions.Clear();
        if (Random.value > 0.5f)
        {
            for (int x = 0; x < columns; x++)
            {
                for (int y = 1; y < rows - 1; y++)
                {
                    gridPositions.Add(new Vector3(x, y, 0f));
                }
            }
        }
        else 
        {
            for (int x = 1; x < columns - 1; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    gridPositions.Add(new Vector3(x, y, 0f));
                }
            }
        }

    }


    void BoardSetup()
    {
        // FIXME: only create a new board if there is no Board object already present
        // FIXME: quick fix for now...
        if (GameObject.Find(Constants.boardGameObject) == null)
        {
            boardHolder = new GameObject(Constants.boardGameObject).transform;
        }

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if(x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }
    }


    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    Vector3 RandomPositionNearExit()
    {
        bool valid = false;
        int randomIndex = -1;
        Vector3 randomPosition = Vector3.zero;
        while (!valid) 
        {
            randomIndex = Random.Range(0, gridPositions.Count);
            randomPosition = gridPositions[randomIndex];
            if (randomPosition.x > rows/2 && randomPosition.y > columns/2)
                valid = true;
        }

        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, Count count)
    {
        int objectCount = Random.Range(count.minimum, count.maximum + 1);

        for(int i = 0; i < objectCount; i++) 
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    void LayoutEnemies(GameObject[] tileArray, int level)
    {
        int enemyCount = ChooseEnemyCount(level);

        for(int i = 0; i < enemyCount; i++) 
        {
            Vector3 randomPosition = RandomPositionNearExit();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    int ChooseEnemyCount(int level)
    {
        return (int)Mathf.Log(level, enemyCountScaling);
    }

    public void SetupScene(int level)
    {
        SetRandomDimensions();
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(wallTiles, wallCount);
        LayoutObjectAtRandom(itemTiles, itemCount);
        LayoutEnemies(enemyTiles, level);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0F),  Quaternion.identity);
    }
}
