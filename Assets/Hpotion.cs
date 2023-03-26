using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hpotion : MonoBehaviour
{
    [SerializeField] private float health = 50;
    [SerializeField] private AudioClip sound;

    private bool canWork = false;
    private Collider2D player;

    private void Update()
    {
        if(canWork)
            Work();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player = other;
            canWork = true;
        }   
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
            canWork = false;
    }

    private void Work()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            SoundManager.instance.PlaySound(sound);
            player.GetComponent<Health>().GiveHealth(health);
            Destroy(gameObject);
        }
    }
}
