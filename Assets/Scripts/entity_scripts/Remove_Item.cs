using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Remove_Item : MonoBehaviour
{

    //[SerializeField] private float health = 50;
    //[SerializeField] private AudioClip sound;
    void OnTriggerEnter2D(Collider2D other)
    {
     //   SoundManager.instance.PlaySound(sound);
        
        if(other.tag == "Player"){
            FindObjectOfType<Player_Controller>().Next_Dungeon();
            //other.GetComponent<Health>().GiveHealth(health);
            gameObject.SetActive(false);
        }
    }
}
