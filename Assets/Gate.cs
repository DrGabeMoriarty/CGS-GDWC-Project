using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private Transform curr_room;
    [SerializeField] private GameObject next_room;
    [SerializeField] private CameraController cam;
    //[SerializeField] private AudioClip audio;
    //[SerializeField] private AudioClip audio2;
    private bool once = true;

    private void OnTriggerEnter2D(Collider2D collison)
    {

        if (collison.tag == "Player")
        {
            next_room.SetActive(true);

            if (collison.transform.position.y > transform.position.y)
            {
                cam.Change_Rooms(next_room.GetComponent<Transform>());
                if (once)
                {
                    once = false;
                    //SoundManager.instance.PlaySound(audio);
                    //SoundManager.instance.PlaySound(audio2);
                }
            }
            else cam.Change_Rooms(curr_room);

        }

    }
}
