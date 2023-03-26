using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Generate_Dungeon : MonoBehaviour,IDataPersistence
{

    [SerializeField] private float cameraDistOffset = 10;

    private AbstractDungeonGenerator dungeon;
    private DungeonData dungeonData;
    [SerializeField] private Camera mainCamera;
    private GameObject player;
    private int numofKeys = 0;

    public int Scenenumber = 2;
    private bool isPiano = false;

    private void Awake()
    {
        dungeonData = GetComponent<DungeonData>();
        dungeon = GetComponent<AbstractDungeonGenerator>();
        player = GameObject.Find("Player");
        dungeonData.Reset();
        dungeon.GenerateDungeon();
    }

    void Update()
    {
        Vector3 playerInfo = player.transform.transform.position;
        mainCamera.transform.position = new Vector3(playerInfo.x, playerInfo.y, playerInfo.z - cameraDistOffset);
    }

    public void Next_Dungeon()
    {
        transform.position = Vector3.zero;
        numofKeys++;
        dungeon.GenerateDungeon();

        //Enter Boss Level
        if (numofKeys >= 10)
        {
            numofKeys = 0;
            if(SceneManager.GetActiveScene().buildIndex == 7 && isPiano)
            {
                SceneManager.LoadScene(8);
            }
            else
                SceneManager.LoadScene(11);
        }
    }

    public void LoadData(GameData data)
    {
        numofKeys = data.keys;
        isPiano = data.isPiano_1;
    }

    public void SaveData(ref GameData data)
    {
        data.keys = numofKeys;
        data.SceneNumber = Scenenumber; 
    }
}
