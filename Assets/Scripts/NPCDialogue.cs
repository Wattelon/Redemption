using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] public GameObject canvas;
    public int triggerIndex;
    [SerializeField] private int abilityIndex;
    [SerializeField] private PlayerController player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.IsTalking = true;
            canvas.SetActive(true);
            canvas.transform.GetChild(triggerIndex).gameObject.SetActive(true);
            player.UIInputScheme();
            player.UnlockAbility(abilityIndex);
            player.transform.LookAt(transform);
            player.GetComponentInChildren<Camera>().transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        player.IsTalking = false;
        player.PlayerInputScheme();
        canvas.SetActive(false);
    }
}