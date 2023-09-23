using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class NPCdialogue : MonoBehaviour
{
    //private DialogueTrigger trigger;
    [SerializeField] public GameObject canvas;
    private DialogueTrigger dialtr;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log(dialtr.index);
            canvas.SetActive(true);
            canvas.transform.GetChild(dialtr.index).gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider collider)
    {
            canvas.SetActive(false);
    }
}
