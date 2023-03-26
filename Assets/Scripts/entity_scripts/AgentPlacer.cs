using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AgentPlacer : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] GameObject playerPrefab;

    [SerializeField] private int playerRoomIndex;
    [SerializeField] private List<int> roomEnemiesCount;

    private DungeonData dungeonData;

    private void Awake()
    {
        dungeonData = FindObjectOfType<DungeonData>();
    }

    public void PlaceAgents()
    {
        if (dungeonData == null)
                return;

        for (int i = 0; i < dungeonData.Rooms.Count; i++)
        {
            Room room = dungeonData.Rooms[i];
            RoomGraph roomGraph = new RoomGraph(room.FloorTiles);

            HashSet<Vector2Int> roomFloor = new HashSet<Vector2Int>(room.FloorTiles);
            roomFloor.IntersectWith(dungeonData.Path);

            Dictionary<Vector2Int, Vector2Int> roomMap
                = roomGraph.RunBFS(roomFloor.First(), room.PropPositions);

            room.PositionAccessibleFromPath = roomMap.Keys.OrderBy(x => Guid.NewGuid()).ToList();

            if(roomEnemiesCount.Count > i)
            {
                PlaceEnemies(room, roomEnemiesCount[i]);
            }

            if(i == playerRoomIndex)
            {
                GameObject player = null;
                if ((player = FindObjectOfType<Player_Controller>().gameObject) == null)
                    player = Instantiate(playerPrefab);
                player.transform.localPosition = dungeonData.Rooms[i].RoomCenterPos + Vector2.one * 0.5f;
            }
        }
    }

    private void PlaceEnemies(Room room, int enemiesCount)
    {
        for (int k = 0; k < enemiesCount; k++)
        {
            if(room.PositionAccessibleFromPath.Count <= k)
            {
                return;
            }
            GameObject enemy = Instantiate(enemyPrefabs[UnityEngine.Random.Range(0,enemyPrefabs.Count)]);
            enemy.transform.localPosition = (Vector2) room.PositionAccessibleFromPath[k] + Vector2.one*0.5f;
            room.EnemiesInTheRoom.Add(enemy);
        }
    }
}

public class RoomGraph
{
    public static List<Vector2Int> fourDirections = new List<Vector2Int>() {
    Vector2Int.up,
    Vector2Int.right,
    Vector2Int.down,
    Vector2Int.left
    };

    Dictionary<Vector2Int, List<Vector2Int>> graph = new Dictionary<Vector2Int, List<Vector2Int>>();

    public RoomGraph(HashSet<Vector2Int> roomFloor)
    {
        foreach (Vector2Int pos in roomFloor)
        {
            List<Vector2Int> neighbours = new List<Vector2Int>();
            foreach (Vector2Int direction in fourDirections)
            {
                Vector2Int newPos = pos + direction;
                if (roomFloor.Contains(newPos))
                {
                    neighbours.Add(newPos);
                }
            }
            graph.Add(pos, neighbours);
        }
    }

    public Dictionary<Vector2Int, Vector2Int> RunBFS(Vector2Int startPos, HashSet<Vector2Int> occupiedNodes)
    {
        Queue<Vector2Int> nodesToVisit = new Queue<Vector2Int>();
        nodesToVisit.Enqueue(startPos);

        HashSet<Vector2Int> visitedNodes = new HashSet<Vector2Int>();
        visitedNodes.Add(startPos);

        Dictionary<Vector2Int, Vector2Int> map = new Dictionary<Vector2Int, Vector2Int>();
        map.Add(startPos, startPos);

        while(nodesToVisit.Count > 0)
        {
            Vector2Int node = nodesToVisit.Dequeue();
            List<Vector2Int> neighbours = graph[node];

            foreach (Vector2Int neighbourPosition in neighbours)
            {
                if(visitedNodes.Contains(neighbourPosition) == false &&
                    occupiedNodes.Contains(neighbourPosition) == false)
                {
                    nodesToVisit.Enqueue(neighbourPosition);
                    visitedNodes.Add(neighbourPosition);
                    map[neighbourPosition] = node;
                }
            }

        }
        return map;
    } 
}