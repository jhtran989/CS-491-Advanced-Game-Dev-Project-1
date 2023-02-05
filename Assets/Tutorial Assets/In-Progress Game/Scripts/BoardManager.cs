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

    public int columns = 8;
    public int rows = 8;
    //TODO: make wallCount and itemCount scale with level number (like EnemyCount)?
    public Count wallCount = new Count (5, 9);
    public Count itemCount = new Count (1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] itemTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();


    // TODO: Edit generation to make more interesting levels?
    void InitializeList()
    {
        gridPositions.Clear();
        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }


    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;
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

    //TODO: Mess with EnemyCount scaling?
    Count ChooseEnemyCount(int level)
    {
        int enemyCount = (int)Mathf.Log(level, 2f);
        return new Count(enemyCount, enemyCount); 
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(wallTiles, wallCount);
        LayoutObjectAtRandom(itemTiles, itemCount);
        Count enemyCount = ChooseEnemyCount(level);
        LayoutObjectAtRandom(enemyTiles, enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0F),  Quaternion.identity);
    }
}
