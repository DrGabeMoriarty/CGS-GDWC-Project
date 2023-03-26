using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Animator animator;

    private Executioner exec = null;
    public King king;
    public spawn_music sp;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        exec = FindObjectOfType<Executioner>().GetComponent<Executioner>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if(exec != null)
            exec.Pause();
        if (sp != null)
            sp.Pause();
        if (king != null)
            king.Pause();
            
        animator.SetBool("isOpen", true);
        nameText.text = dialogue.name;
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        if (exec != null)
            exec.Resume();
        if (sp != null)
            sp.Resume();
        if (king != null)
            king.Resume(); 
        animator.SetBool("isOpen", false);
    }
}
