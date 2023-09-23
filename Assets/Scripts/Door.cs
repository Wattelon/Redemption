using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform teleport;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger");
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = teleport.position;
            other.GetComponent<CharacterController>().enabled = true;
        }
    }
}
