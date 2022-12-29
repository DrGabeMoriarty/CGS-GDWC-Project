using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Next : MonoBehaviour
{

    public UnityEvent evt;

    private void Update()
    {
        if(FindObjectsOfType<Enemy>() == null)
        {
            Debug.Log("here");

            if (FindObjectOfType<idk>() == null)
            {
                Debug.Log("here2");
                evt?.Invoke();
            }
        }
    }

}
