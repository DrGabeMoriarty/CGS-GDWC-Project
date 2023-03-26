using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Next : MonoBehaviour,IDataPersistence
{
    public float time = 1f; 
    public bool Mike = false;
    private void Awake()
    {
        Invoke("NextLevel", time*60);
    }


    private void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SaveData(ref GameData data)
    {
        data.SceneNumber = SceneManager.GetActiveScene().buildIndex;
        if (!Mike)
            data.isPiano_1 = true;
        else
            data.isPiano_2 = true;
    }

    public void LoadData(GameData data){}
}
