using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Press_to_speak : MonoBehaviour
{
    public bool isInRange = false;

    private void Update()
    {
        if (isInRange)
            Talk();
    }
    private void OnTriggerEnter2D()
    {
        isInRange = true; 
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isInRange = false;
    }

    private void Talk()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            GetComponent<DialogueTrigger>().TriggerDialogue();
        }
    }
}
