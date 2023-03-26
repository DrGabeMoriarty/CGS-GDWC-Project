using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour,IDataPersistence
{
    private int SceneNumber = 1;
    public void LoadNext()
    {
        SceneManager.LoadScene(SceneNumber);
    }

    public void LoadNew()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadBosses()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadData(GameData data)
    {
        SceneNumber = data.SceneNumber;
    }
    public void SaveData(ref GameData data) { }

}
