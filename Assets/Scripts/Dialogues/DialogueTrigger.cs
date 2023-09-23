using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    //private NPCdialogue Npc;
    public int index;


    public void TriggerDialogue ()
    {
        index++;
        gameObject.SetActive(false);
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
