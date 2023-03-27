using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class saves : MonoBehaviour,IDataPersistence
{
    public void SaveData(ref GameData data)
    {
        data.SceneNumber = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadData(GameData data) { }
}
