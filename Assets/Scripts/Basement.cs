using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basement : MonoBehaviour
{
    [SerializeField] private NPCDialogue dad;
    private static int _pillarsInPlace;

    public void PlacePillar()
    {
        _pillarsInPlace++;
        if (_pillarsInPlace == 4)
        {
            SaveBasement();
        }
    }

    private void SaveBasement()
    {
        dad.triggerIndex = 10;
        dad.GetComponent<SphereCollider>().enabled = true;
    }
}
