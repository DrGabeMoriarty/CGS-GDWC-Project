using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate_Dungeon : MonoBehaviour
{
    public SimpleRandomDungeonGenerator dungeon;
    // Start is called before the first frame update
    void Awake()
    {
        dungeon.RunProceduralGeneration();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}