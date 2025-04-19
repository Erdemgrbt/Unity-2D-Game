using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXSound : MonoBehaviour
{
    private AudioSource audioSource;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnBecameInvisible()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.enabled = false;
        }
    }

    private void OnBecameVisible()
    {
        if (audioSource != null)
        {
            audioSource.enabled = true;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && audioSource.enabled)
        {
            audioSource.PlayOneShot(clip);
        }
    }

}
