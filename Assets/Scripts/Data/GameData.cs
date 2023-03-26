using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public bool isBinge;
    public bool isPiano_1;
    public bool isPiano_2;
    public int SceneNumber;
    public int keys;

    public GameData()
    {
        this.isPiano_1 = false;
        this.isPiano_2 = false;
        this.isBinge = false;
        this.SceneNumber = 1;
        this.keys = 0;
    }
}
