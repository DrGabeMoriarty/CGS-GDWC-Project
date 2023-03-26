using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour,IDataPersistence
{
    private bool Binge = false;
    private bool isPiano1 = false;
    private bool isPiano2 = false;

    public GameObject Bingeobj;
    public GameObject good;
    public GameObject n1;
    public GameObject n2;

    private void Decide()
    {
        if (Binge)
        {
            Bingeobj.SetActive(true);
        }
        else if (isPiano2)
        {
            good.SetActive(true);
        }
        else if (isPiano1)
        {
            n1.SetActive(true);
        }
        else
        {
            n2.SetActive(true);
        }
    }

    public void LoadData(GameData data)
    {
        Binge = data.isBinge;
        isPiano1 = data.isPiano_1;
        isPiano2 = data.isPiano_2;
    }
    public void SaveData(ref GameData data)
    {
        data.SceneNumber = 13;
    }
}
