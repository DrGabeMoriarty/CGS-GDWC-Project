using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class distorted : MonoBehaviour
{
    private void Update()
    {
        if (FindObjectsOfType<Enemy>().ToList().Count == 0 &&
            FindObjectsOfType<Ranged>().ToList().Count == 0)
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1));
    }
}
