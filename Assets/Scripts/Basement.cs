using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basement : MonoBehaviour
{
    [SerializeField] private NPCDialogue dad;
    private static int _pillarsInPlace;
    private int i = 0;

    public void PlacePillar()
    {
        _pillarsInPlace++;
        if (_pillarsInPlace == 5)
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
