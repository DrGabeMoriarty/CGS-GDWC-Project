using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AgentPlacer : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private int roomEnemiesCount;

    Room room;

    private void Awake()
    {
        room = FindObjectOfType<Room>();
    }

    public void PlaceAgents()
    {
       // if (room == null)
         //   return;
        PlaceEnemies(room, roomEnemiesCount);
        roomEnemiesCount += 2;
    }

    private void PlaceEnemies(Room room, int enemiesCount)
    {
        Debug.Log("here");

        for(int k = 0; k < enemiesCount; k++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            System.Random random = new System.Random();
            Vector2Int Tile = room.FloorTiles.ElementAt(random.Next(room.FloorTiles.Count));
            enemy.transform.localPosition = (Vector2)Tile + Vector2.one*0.5f;
        }
    }
}
