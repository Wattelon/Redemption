using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomSight : MonoBehaviour
{
    [SerializeField] private NPCDialogue bro;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var otherTransform = other.transform;
            bro.triggerIndex = 5;
            bro.transform.root.position = otherTransform.position + otherTransform.forward * 3 + otherTransform.right * 3;
            bro.transform.LookAt(other.transform);
        }
    }
}