using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue ()
    {
        gameObject.SetActive(false);
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
