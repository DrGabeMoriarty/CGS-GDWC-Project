using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Press_to_speak : MonoBehaviour
{
    private void OnMouseDown()
    {
        GetComponent<DialogueTrigger>().TriggerDialogue();
    }
}
