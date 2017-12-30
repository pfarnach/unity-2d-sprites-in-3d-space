using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BushRustle : MonoBehaviour {

    private Animator anim;
    private AudioSource audioSrc;

    private float originalAudioPitch;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        originalAudioPitch = audioSrc.pitch;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetTrigger("Rustle");
            audioSrc.pitch = Random.Range(originalAudioPitch - 0.05f, originalAudioPitch + 0.05f);
            audioSrc.Play();
        }
    }

}
