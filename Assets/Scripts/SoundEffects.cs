using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    private AudioSource audioSource;
    //public AudioClip Steps;

    private void OnControllerColliderHit(ControllerColliderHit collision)
    {
        if (collision.gameObject.tag == "House")
        {
            audioSource = GetComponent<AudioSource>();
            //audioSource.clip = Steps;
            audioSource.Play();
            Debug.Log("HEEEELp");
        }
    }
}

