using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteOnPause : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Player.Paused)
        {
            audioSource.Pause();
        }
        else if(Player.Paused == false)
        {
            audioSource.UnPause();
            
            
        }
    }
}
