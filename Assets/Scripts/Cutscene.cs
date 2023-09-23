using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    [SerializeField] private Material skyBox;
    void Start()
    {
        RenderSettings.skybox = skyBox;
    }

    void Update()
    {
        
    }
}
