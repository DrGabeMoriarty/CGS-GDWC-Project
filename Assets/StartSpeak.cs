using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSpeak : MonoBehaviour
{
    
    void Awake()
    {
        Invoke("Speak", 0.2f);
    }

    void Speak()
    {
        GetComponent<DialogueTrigger>().TriggerDialogue();
    }

}
