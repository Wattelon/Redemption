using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    [SerializeField] private Material skyBox;
    [SerializeField] private Transform bro;
    [SerializeField] private Transform sis;
    [SerializeField] private Transform mom;
    [SerializeField] private Transform ded;
    [SerializeField] private Transform player;
    private static readonly int IsCutscene = Animator.StringToHash("isCutscene");
    private static readonly int Bro = Animator.StringToHash("Bro");
    private bool isEnd;
    public bool isTrigger;
    private int i;

    void Start()
    {
    }

    public void startTrigger()
    {
        isTrigger = true;
    }

    public void Trigger()
    {
        if (isEnd || !isTrigger) return;
        var thisTransform = transform;
        RenderSettings.skybox = skyBox;
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<PlayerController>().movementSpeed = 0;
        player.transform.position = thisTransform.position - Vector3.up * 2;
        player.GetComponent<CharacterController>().enabled = true;
        bro.transform.position = thisTransform.position + thisTransform.forward * 5;
        sis.transform.position = thisTransform.position + thisTransform.forward * 5 + thisTransform.right * 4;
        mom.transform.position = thisTransform.position + thisTransform.forward * 5 + thisTransform.right * 2;
        ded.transform.position = thisTransform.position + thisTransform.forward * 5 + thisTransform.right * -2;
        player.transform.LookAt(bro);
        sis.GetComponentInChildren<Animator>().SetBool(IsCutscene, true);
        mom.GetComponentInChildren<Animator>().SetBool(IsCutscene, true);
        ded.GetComponentInChildren<Animator>().SetBool(IsCutscene, true);
        bro.GetComponentInChildren<Animator>().SetTrigger(Bro);
        bro.GetComponentInChildren<NPCDialogue>().triggerIndex = 11;
        bro.GetComponentInChildren<SphereCollider>().radius = 3;
        isEnd = true;
    }

    public void Exit()
    {
        if (isEnd)
        {
            i++;
            if (i == 3)
            {
                Application.Quit();
            }
        }
    }
}