using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Press_to_speak : MonoBehaviour
{
    private void OnTriggerStay2D()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            GetComponent<DialogueTrigger>().TriggerDialogue();
        }
    }
}
