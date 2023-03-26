using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class next_level : MonoBehaviour
{
    public int Scene = 5;

    private void Awake()
    {
        if(Scene == 13)
            Invoke("Next", 8f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene(Scene);
    }

    public void Next()
    {
        SceneManager.LoadScene(Scene);
    }
}
