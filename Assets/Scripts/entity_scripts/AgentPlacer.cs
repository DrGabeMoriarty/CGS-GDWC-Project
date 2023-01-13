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

    public Room room;

    public void PlaceAgents()
    {
       // if (room == null)
         //   return;
        PlaceEnemies(room, roomEnemiesCount);
        roomEnemiesCount += 2;
    }

    private void PlaceEnemies(Room room, int enemiesCount)
    {
        System.Random random = new System.Random();
        for (int k = 0; k < enemiesCount; k++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            Vector2Int Tile = room.FloorTiles.ElementAt(random.Next(room.FloorTiles.Count));
            enemy.transform.localPosition = (Vector2)Tile + Vector2.one*0.5f;
        }
    }
}
