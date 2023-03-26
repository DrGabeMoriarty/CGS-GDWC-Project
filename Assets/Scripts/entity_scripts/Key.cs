using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Key : MonoBehaviour
{
    [SerializeField] private AudioClip sound;
    private void OnTriggerEnter2D(Collider2D other)
    {
     SoundManager.instance.PlaySound(sound);
        if(other.tag == "Player"){
            FindObjectOfType<Generate_Dungeon>().Next_Dungeon();
            Destroy(gameObject);
        }
    }
}
