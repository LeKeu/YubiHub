using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_scripts : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField] AudioClip certo;
    [SerializeField] AudioClip errado;

    public void SoundAcertar()
    {
        audioSource.clip = certo;
        Debug.Log("certo");
        audioSource.Play();
    }

    public void SoundErrar()
    {
        audioSource.clip = errado;
        audioSource.Play();
        Debug.Log("errado");

    }
}
