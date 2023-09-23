using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrotherHelp : MonoBehaviour
{
    [SerializeField] private NPCDialogue bro;
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 rotation;

    public void TeleportBro()
    {
        bro.triggerIndex = 6;
        bro.transform.root.position = position;
        bro.transform.root.rotation = Quaternion.Euler(rotation);
    }
}
