using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basement : MonoBehaviour
{
    private static int _pillarsInPlace;

    public static void PlacePillar()
    {
        _pillarsInPlace++;
        if (_pillarsInPlace == 5)
        {
            SaveBasement();
        }
    }

    private static void SaveBasement()
    {
        Debug.Log("BasementSaved!");
    }
}
