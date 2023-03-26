using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource src;
    public static SoundManager instance { get; private set;}

    private void Awake(){ 
        instance = this;
        src = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip audio){
        src.PlayOneShot(audio);
    }



}
