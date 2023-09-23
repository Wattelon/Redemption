using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBrother : MonoBehaviour
{
    [SerializeField] private NPCDialogue bro;
    [SerializeField] private Transform point;

    public void TeleportBro()
    {
        bro.triggerIndex = 9;
        bro.transform.root.position = point.position;
    }
}
