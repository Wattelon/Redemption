using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pile : MonoBehaviour
{
    [SerializeField] private int movableLayer;
    private List<Collider> _rocks;
    
    void Start()
    {
        _rocks = transform.GetComponentsInChildren<Collider>().ToList();
        _rocks.RemoveAt(0);
        _rocks[0].gameObject.layer = movableLayer;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == _rocks[0])
        {
            _rocks.RemoveAt(0);
            if (_rocks.Count > 0)
            {
                _rocks[0].gameObject.layer = movableLayer;
            }
            else
            {
                Destroy(GetComponent<Collider>());
            }
        }
    }
}