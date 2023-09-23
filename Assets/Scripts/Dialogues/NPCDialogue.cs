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

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            canvas.SetActive(true);
            canvas.transform.GetChild(triggerIndex).gameObject.SetActive(true);
            var player = FindObjectOfType<PlayerController>();
            player.ChangeInputScheme();
            player.UnlockAbility(abilityIndex);
        }
    }
    private void OnTriggerExit(Collider collider)
    {
            canvas.SetActive(false);
    }
}
