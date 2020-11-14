using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlay : MonoBehaviour
{
    public AudioClip[] sfxs;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Play()
    {
        if(source.isPlaying)
            return;
        source.clip = sfxs[Mathf.RoundToInt(Random.Range(0, sfxs.Length))];
        source.Play();
    }
}
