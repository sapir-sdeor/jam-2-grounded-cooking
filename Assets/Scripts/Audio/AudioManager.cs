using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioSource _music;
    private static AudioSource _computerSound;

  
    // Start is called before the first frame update
    private void Start()
    {
        _music = GetComponent<AudioSource>();
        _computerSound = GameObject.FindWithTag("Computer").GetComponent<AudioSource>();
       
    }

    public static void StopMusic()
    {
        _music.Pause();
    }

    public static void ResumeMusic()
    {
        _music.Play();
    }

    public static void PlayComputerSound(AudioClip sellAudioClip)
    {
        _computerSound.clip = sellAudioClip;
        _computerSound.Play();
    }

   
    public static void EndComputerSound()
    {
        _computerSound.Pause();
    }
}
